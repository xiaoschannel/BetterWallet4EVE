using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cn.zuoanqh.open.zut;
using cn.zuoanqh.open.zut.Data;


namespace cn.zuoanqh.open.bw4eve.Data
{
    public class ISK : IComparable<ISK>
    {
        public readonly long AmountCents;
        public long WholeISK { get { return AmountCents / 100; } }
        public long CentsOnly { get { return AmountCents % 100; } }
        public double Value { get { return AmountCents / 100.0d; } }
        public ISK(string GameRepresentationAmount)
            : this(Convert.ToInt64(GameRepresentationAmount.Replace(",", "").Replace(".", "").Replace(" ISK", "")))
        { }
        public ISK(long cents)
        {
            this.AmountCents = cents;
        }
        public static ISK operator +(ISK op1, ISK op2)
        {
            return new ISK(op1.AmountCents + op2.AmountCents);
        }
        /// <summary>
        /// Converts the amount of ISK back to game format, such as 1,000,000.00 ISK
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToStringSep(3);
        }
        /// <summary>
        /// Converts the amount of ISK to ten-thousand separated form, such as 100,0000.00 ISK
        /// </summary>
        /// <returns></returns>
        public string ToString4()
        {
            return ToStringSep(4);
        }

        private string ToStringSep(int amtDigits)
        { 
            return zusp.List(",", zusp.DivideByLengthRight(WholeISK + "", amtDigits)) + "." + CentsOnly + " ISK"; 
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            return this.AmountCents == ((ISK)obj).AmountCents;
        }


        public int CompareTo(ISK other)
        {
            return (int)(this.AmountCents-other.AmountCents);
        }
    }
}
