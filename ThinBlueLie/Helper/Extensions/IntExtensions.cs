using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLie.Helper.Extensions
{
    public static class IntExtensions
    {
        public static int[] IntToArray(int? n)
        {
            List<int> powers = new List<int>();
            int power = 0;
            if (n == null || n == 0)
            {
                return new int[] { 0 };
            }
            while (n != 0)
            {
                if ((n & 1) != 0)
                {
                    powers.Add(1 << power);
                    // or, if you just need the exponents:
                    // powers.add(power);
                }
                ++power;
                n >>= 1;
            }
            return powers.ToArray();
        }      
        public static bool ToBool(this int value)
        {
            if (value == 0)
                return false;
            else
                return true;
        }
        public static bool ToBool(this byte value)
        {
            if (value == 0)
                return false;
            else
                return true;
        }
    }
}
