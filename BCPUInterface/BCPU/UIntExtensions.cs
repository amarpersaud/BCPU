using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCPU
{
    public static class UIntExtensions
    {
        /// <summary>
        /// Checks if the bit is set to 1
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="bit">Number of the bit (little endian)</param>
        /// <returns>If the bit is set</returns>
        public static bool IsBitSet(this UInt16 value, int bit)
        {
            return ((value >> bit) & 0x1) == 1;
        }
        /// <summary>
        /// Checks if the bit is set to 1
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="bit">Number of the bit (little endian)</param>
        /// <returns>If the bit is set</returns>
        public static bool IsBitSet(this UInt64 value, int bit)
        {
            return ((value >> bit) & 0x1) == 1;
        }

        /// <summary>
        /// Checks if the value has the flag/flags
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="flag">Flag(s) to check (2^n)</param>
        /// <returns></returns>
        public static bool HasFlag(this UInt16 value, UInt16 flag)
        {
            return (value & flag) != 0;
        }
        public static bool HasFlag(this UInt64 value, UInt64 flag)
        {
            return (value & flag) != 0;
        }
    }
}
