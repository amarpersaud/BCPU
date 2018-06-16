using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading; 
using System.Threading.Tasks;

namespace BCPU
{
    public class BCPU
    {
        /// <summary>
        /// Default size of the ram, in words (2 byte pairs)
        /// </summary>
        public const int DEFAULT_RAM_SIZE = 1024;

        /// <summary>
        /// A register
        /// </summary>
        public Register ARegister;
        
        /// <summary>
        /// B register 
        /// </summary>
        public Register BRegister;

        public Register OutputRegister;
        public Counter OutputMode;

        /// <summary>
        /// Program counter.
        /// </summary>
        public Counter ProgramCounter;

        /// <summary>
        /// Instruction register
        /// </summary>
        public Register InstructionRegister;

        /// <summary>
        /// Flag register
        /// </summary>
        public FlagRegister FlagRegister;

        /// <summary>
        /// Step counter for counting microsteps
        /// </summary>
        public Counter StepCounter;


        /// <summary>
        /// RAM Address Register
        /// </summary>
        public Register RAMAddressRegister;
        /// <summary>
        /// ROM Address Register
        /// </summary>
        public Register ROMAddressRegister;

        /// <summary>
        /// UInt16 array for the ROM to simplify things, rather than have a
        /// separate EEPROM class
        /// </summary>
        public UInt16[] ROM;
        /// <summary>
        /// UInt16 array for the RAM to simplify things, rather than have a
        /// separate SRAM class
        /// </summary>
        public UInt16[] RAM;

        /// <summary>
        /// UInt16 value to represent the 16 bit bus
        /// </summary>
        public UInt16 Bus;

        /// <summary>
        /// The Arithmetic Logic Unit (ALU)
        /// </summary>
        public ALU ALU;

        /// <summary>
        /// Shows whether the clock is halted or not
        /// </summary>
        public bool Halted;


        /// <summary>
        /// The frequency at which the clock will be simulated at, in Hz.
        /// </summary>
        public double SimulatedClockFrequency
        {
            get { return _freq; }
            set {
                _freq = value;
                CycleTime = (1 / value);
            }
        }
        private double _freq;

        public double CycleTime { get; private set; }


        /// <summary>
        /// Stopwatch to measure the time in order to update at the right time.
        /// </summary>
        private Stopwatch stopwatch = new Stopwatch();

        public BCPU(double Frequency) {
            this.SimulatedClockFrequency = Frequency;

            this.ARegister = new Register();
            this.BRegister = new Register();

            this.RAMAddressRegister = new Register();
            this.ROMAddressRegister = new Register();

            this.ProgramCounter = new Counter(true);
            this.StepCounter = new Counter(false);
            this.FlagRegister = new FlagRegister();
            this.InstructionRegister = new Register();

            this.RAM = new UInt16[DEFAULT_RAM_SIZE];
        }

        public BCPU(int RAMSize, UInt16[] ROM, double Frequency)
        {
            this.SimulatedClockFrequency = Frequency;


            this.ARegister = new Register();
            this.BRegister = new Register();

            this.RAMAddressRegister = new Register();
            this.ROMAddressRegister = new Register();

            this.ProgramCounter = new Counter(true);
            this.StepCounter = new Counter(false);
            this.FlagRegister = new FlagRegister();
            this.InstructionRegister = new Register();

            this.ROM = (UInt16[])ROM.Clone();
            this.RAM = new UInt16[RAMSize];

        }


        public void Run() {
            stopwatch.Start();
            // Throttle based on time and clock freq. 
            while (true)
            {
                if (stopwatch.Elapsed.TotalSeconds >= CycleTime)
                {
                    Update();
                    stopwatch.Restart();
                }
            }
        }



        /// <summary>
        /// Update CPU. One cycle of the clock (up, down)
        /// </summary>
        public void Update() {

            if (Halted)
            {
                return;
            }
            /*
               Do rising edge clock stuff, or stuff that should be done first.
               Falling edge stuff that should be done is connected to Q-not.
                    ____      ____      ____      ____
               ____|    |____|    |____|    |____|              Q

                   ↑  

               ____      ____      ____      ____               _
                   |____|    |____|    |____|    |____          Q
                
            */

            //Increase the step counter by 1.
            StepCounter.Increment();
            /*
                Changing the step counter changes the output of the control logic. 
                The SC = 0000 slot for the control logic should be blank to ensure the first actual code runs, 
                at least until I do tests to check if it will run. TODO: check if it works, as the reset will
                cause it to execute the first microcode instruction immediately (on clock down) and then increase the
                SC by one, skipping the first instruction if it's not fast enough.
            */

            UInt32 ControlLogicOutput = ControlLogic.GetControlSignals(StepCounter.Get(), InstructionRegister.Get());

            //Set the alu mode bits to the appropriate control signals
            ALU.SetMode(ControlLogicOutput);


            /*
                Falling edge clock stuff.
             */


            //The bus output bits pass through a 4:10 (4:16) decoder, so decode it to figure out which register is ouputting
            // to the bus.
            UInt32 BusOutputEncoderOut = ControlLogic.GetDecoderOutput(ControlLogic.GetEncodedBusOutputBits(ControlLogicOutput));

            //Set bus to whatever wants to write to the bus
            SetBusToOutput(BusOutputEncoderOut);

            //Read from the bus into whatever register(s) the control logic selects
            GetInputFromBus(ControlLogicOutput);



        }

        
        /// <summary>
        /// Set the bus to whichever counter/register/memory is selected by the control logic
        /// </summary>
        /// <param name="EncoderOutput">Output of the 4:10 / 4:16 encoder for the bus output bits</param>
        public void SetBusToOutput(UInt32 EncoderOutput) {
            switch (EncoderOutput)
            {
                case ControlLogic.CO:
                    Bus = ProgramCounter.Get();
                    break;
                case ControlLogic.AO:
                    Bus = ARegister.Get();
                    break;
                case ControlLogic.BO:
                    Bus = BRegister.Get();
                    break;
                case ControlLogic.MO:
                    Bus = ROMAddressRegister.Get();
                    break;
                case ControlLogic.ALUO:
                    Bus = ALU.Get();
                    break;
                case ControlLogic.FLO:
                    Bus = FlagRegister.Get();
                    break;
                case ControlLogic.RAO:
                    Bus = RAM[RAMAddressRegister.Get()];
                    break;
                case ControlLogic.ROO:
                    Bus = ROM[ROMAddressRegister.Get()];
                    break;
                default:
                    Bus = 0;
                    break;
            }
        }

        /// <summary>
        /// Check each bit for the appropriate input from bus flag, then set the appropriate register
        /// to the value on the bus
        /// </summary>
        /// <param name="ControlSignals">Control signals from the control logic</param>
        public void GetInputFromBus(UInt32 ControlSignals)
        {
            if (ControlSignals.HasFlag(ControlLogic.AI))
            {
                ARegister.Set(Bus);
            }
            if (ControlSignals.HasFlag(ControlLogic.BI))
            {
                BRegister.Set(Bus);
            }
            if (ControlSignals.HasFlag(ControlLogic.CI))
            {
                ProgramCounter.Set(Bus);
            }
            if (ControlSignals.HasFlag(ControlLogic.MI))
            {
                ROMAddressRegister.Set(Bus);
            }
            if (ControlSignals.HasFlag(ControlLogic.II))
            {
                InstructionRegister.Set(Bus);
            }
            if (ControlSignals.HasFlag(ControlLogic.RAI))
            {
                RAM[RAMAddressRegister.Get()] = Bus;
            }
            if (ControlSignals.HasFlag(ControlLogic.RARI))
            {
                RAMAddressRegister.Set(Bus);
            }
            if (ControlSignals.HasFlag(ControlLogic.OI))
            {
                OutputRegister.Set(Bus);
            }
            if (ControlSignals.HasFlag(ControlLogic.AI))
            {
                ARegister.Set(Bus);
            }
        }


    }
}
