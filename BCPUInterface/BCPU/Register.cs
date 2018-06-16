using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCPU
{
    /// <summary>
    /// CPU register
    /// </summary>
    public class Register
    {
        /// <summary>
        /// Value
        /// </summary>
        protected UInt16 n;

        /// <summary>
        /// Create register. Defaults to 0.
        /// </summary>
        public Register() {
            n = 0;
        }

        /// <summary>
        /// Create register.
        /// </summary>
        /// <param name="val">Value of register</param>
        public Register(int val)
        {
            n = (UInt16)val;
        }

        /// <summary>
        /// Create register.
        /// </summary>
        /// <param name="val">Value of register</param>
        public Register(UInt16 val)
        {
            n = val;
        }

        /// <summary>
        /// Get register value.
        /// </summary>
        public virtual UInt16 Get()
        {
            return n;
        }
        /// <summary>
        /// Set register value
        /// </summary>
        /// <param name="val">Value to set register to</param>
        public virtual void Set(UInt16 val)
        {
            this.n = val;
        }

        /// <summary>
        /// Reset the register to 0
        /// </summary>
        public void Reset() {
            n = 0;
        }
    }
}
