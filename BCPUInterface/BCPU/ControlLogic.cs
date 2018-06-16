using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCPU
{
    public class ControlLogic
    {


        /*
            Todo: make a dictionary or something that maps the above control signals to a string, 
            then make a json file or xlsl file that maps the instruction and stuff to the appropriate  
            control signals. Then use the dictionary to read the file, map the control signals, then return
            it in the following method
        */

        /// <summary>
        /// Returns the control signals associated with a step counter value and instruction
        /// Todo : implement
        /// </summary>
        /// <param name="StepCounter"></param>
        /// <param name="Instruction"></param>
        /// <returns></returns>
        public static UInt32 GetControlSignals(UInt16 StepCounter, UInt16 Instruction) {
            return 0;
        }


        /// <summary>
        /// Counter In (set)
        /// </summary>
        public const UInt32 CI = 1;   //0th bit
        /// <summary>
        /// Counter count - increment counter
        /// </summary>
        public const UInt32 CC = 1 << 1;
        /// <summary>
        /// Counter reset
        /// </summary>
        public const UInt32 CR = 1 << 2;

        /// <summary>
        /// MAR in
        /// </summary>
        public const UInt32 MI = 1 << 3;

        /// <summary>
        /// Instruction register in
        /// </summary>
        public const UInt32 II = 1 << 4;


        /// <summary>
        /// RAM in
        /// </summary>
        public const UInt32 RAI = 1 << 5;

        /// <summary>
        /// Step counter reset
        /// </summary>
        public const UInt32 SCR = 1 << 6;
        /// <summary>
        /// Step counter Halt.
        /// Sends a halt signal and shuts down the CPU.
        /// </summary>
        public const UInt32 SCH = 1 << 7;


        /// <summary>
        /// Flag register conditional jump multiplexed 3:8 A
        /// </summary>
        public const UInt32 FLA = 1 << 8;
        /// <summary>
        /// Flag register conditional jump multiplexed 3:8 B
        /// </summary>
        public const UInt32 FLB = 1 << 9;
        /// <summary>
        /// Flag register conditional jump multiplexed 3:8 C
        /// </summary>
        public const UInt32 FLC = 1 << 10;

        /// <summary>
        /// RAM address register in
        /// </summary>
        public const UInt32 RARI = 1 << 11;

        /// <summary>
        /// A register in
        /// </summary>
        public const UInt32 AI = 1 << 12;

        /// <summary>
        /// B register in
        /// </summary>
        public const UInt32 BI = 1 << 13;

        /// <summary>
        /// Output register in
        /// </summary>
        public const UInt32 OI = 1 << 14;
        /// <summary>
        /// Output register mode
        /// </summary>
        public const UInt32 OM = 1 << 15;

        /// <summary>
        /// ALU mode
        /// Puts the ALU into logic mode. Default is arithmetic mode.
        /// </summary>
        public const UInt32 ALUM = 1 << 16;

        /// <summary>
        /// ALU mode select 0
        /// </summary>
        public const UInt32 ALUS0 = 1 << 17;
        /// <summary>
        /// ALU mode select 1
        /// </summary>
        public const UInt32 ALUS1 = 1 << 18;
        /// <summary>
        /// ALU mode select 2
        /// </summary>
        public const UInt32 ALUS2 = 1 << 19;
        /// <summary>
        /// ALU mode select 3
        /// </summary>
        public const UInt32 ALUS3 = 1 << 20;  //bit 20

        /*
            Bits 21 - 24 (4 bits/pins) occupied by the 4:10 (or 4:16) decoder for outputs. 
         */

        public const UInt32 NextPin = 1 << 25; // bit 25

        /*
            The following are the outputs of the 4:10 decoder for the outputs to bus. 
            9th and 10th bit are unused. 11-16 are unavailable. May be swapped for two
            chained 3:8 decoders for up to 16 outputs.
        */

        /// <summary>
        /// Counter out (decoder output)
        /// </summary>
        public const UInt32 CO = 1 << 0;
        /// <summary>
        /// MAR out (decoder output)
        /// </summary>
        public const UInt32 MO = 1 << 1;
        /// <summary>
        /// ROM Out (decoder output)
        /// </summary>
        public const UInt32 ROO = 1 << 2;
        /// <summary>
        /// RAM out (decoder output)
        /// </summary>
        public const UInt32 RAO = 1 << 3;
        /// <summary>
        /// Flag register out (decoder output)
        /// </summary>
        public const UInt32 FLO = 1 << 4;
        /// <summary>
        /// A register out (decoder output)
        /// </summary>
        public const UInt32 AO = 1 << 5;
        /// <summary>
        /// B register out (decoder output)
        /// </summary>
        public const UInt32 BO = 1 << 6;
        /// <summary>
        /// ALU out (decoder output)
        /// </summary>
        public const UInt32 ALUO = 1 << 7;



        public static UInt32 GetEncodedBusOutputBits(UInt32 ControlValues)
        {
            //The bits for the bus outputs are bits 21-24
            return (ControlValues & 0b0001_1110_0000_0000_0000_0000_0000) >> 21;
        }

        public static UInt32 GetDecoderOutput(UInt32 input) {
            return (1U << (int)input);
        }
        public static UInt32 GetDecoderInput(UInt32 output)
        {
            for (UInt32 i = 0; i < 32; i++) {
                if(output >> (int)i == 1)
                {
                    return i;
                }
            }
            return 0;
        }


    }
}
