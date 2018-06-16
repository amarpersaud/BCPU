using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCPU
{
    public class Counter : Register
    {

        public bool Settable { get; private set; }

        public Counter(bool Settable) : base()
        {
            this.Settable = Settable;
        }

        /// <summary>
        /// Create counter.
        /// </summary>
        /// <param name="val">Value of counter</param>
        public Counter(int val, bool Settable) : base(val)
        {
            this.Settable = Settable;
        }

        /// <summary>
        /// Create counter.
        /// </summary>
        /// <param name="val">Value of counter</param>
        public Counter(UInt16 val, bool Settable) : base(val)
        {
            this.Settable = Settable;
        }

        /// <summary>
        /// Set counter value if the counter is settable
        /// </summary>
        /// <param name="val">Value to set counter to</param>
        public override void Set(UInt16 val)
        {
            if (Settable)
            {
                base.Set(val);
            }
        }

        public void Increment() {
            this.n++;
        }
        public void Decrement()
        {
            this.n--;
        }

    }
}
