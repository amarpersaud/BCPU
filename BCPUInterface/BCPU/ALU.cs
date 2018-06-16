using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCPU
{
    public class ALU : Register
    {
        /// <summary>
        /// Mode select for selecting which function to execute
        /// </summary>
        public UInt32 ModeSelect;
        /// <summary>
        /// Mode for selecting arithmetic vs logic functions
        /// </summary>
        public UInt32 Mode;

        /// <summary>
        /// A register for first input
        /// </summary>
        private Register ARegister;
        /// <summary>
        /// B register for second input
        /// </summary>
        private Register BRegister;
        /// <summary>
        /// Flag register to check for certain conditions
        /// </summary>
        private FlagRegister FlagRegister;

        /// <summary>
        /// Mask for getting the Mode bit
        /// </summary>
        private UInt32 ModeMask = ControlLogic.ALUM;
        /// <summary>
        /// Mask for getting the mode select bits
        /// </summary>
        private UInt32 ModeSelectMask = ControlLogic.ALUS0 | ControlLogic.ALUS1 | ControlLogic.ALUS2 | ControlLogic.ALUS3;

        public ALU(Register ARegister, Register BRegister, FlagRegister FlagRegister) : base() {
            this.ARegister = ARegister;
            this.BRegister = BRegister;
            this.FlagRegister = FlagRegister;
        }


        public override ushort Get()
        {
         
            //Todo: interpret the modeselect and mode and return the correct 16 bit result, set the appropriate flags in the flag register


            return base.Get();
        }

        public void SetMode(UInt32 ModeSelect, UInt32 Mode) {
            this.ModeSelect = ModeSelect;
            this.Mode = Mode;
        }

        public void SetMode(UInt32 ControlSignals)
        {
            UInt32 mselect = (ControlSignals & ModeSelectMask) >> 17;
            UInt32 m = (ControlSignals & ModeMask) >> 16;
            SetMode(mselect, m);
        }
    }
}
