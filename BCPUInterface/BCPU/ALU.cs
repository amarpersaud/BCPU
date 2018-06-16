using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCPU
{
    public class ALU : Register
    {
        //Todo : implement

        protected Register ARegister;
        protected Register BRegister;
        protected FlagRegister FlagRegister;

        public ALU(Register ARegister, Register BRegister, FlagRegister FlagRegister) : base() {
            this.ARegister = ARegister;
            this.BRegister = BRegister;
            this.FlagRegister = FlagRegister;
        }

        public UInt32 ModeSelect;
        public UInt32 Mode;

        public override ushort Get()
        {
         
            //Todo: interpret the modeselect and mode and return the correct 16 bit result, set the appropriate flags in the flag register


            return base.Get();
        }

        public void SetMode(UInt32 ModeSelect, UInt32 Mode) {
            this.ModeSelect = ModeSelect;
            this.Mode = Mode;
        }
        
    }
}
