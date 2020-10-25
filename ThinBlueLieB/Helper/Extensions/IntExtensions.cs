using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLieB.Helper.Extensions
{
    public class IntExtensions
    {
        public static int[] IntToArray(int? input)
        {
            List<int> list = new List<int>();
            if (input == null)
            {
                return new int[] {0};
            }
            else
            {
                for (int i = 1; input > 0; i *= 2)
                {
                    if ((input & i) != 0)
                    {
                        list.Add(i);
                        input -= i;
                    }
                }
            }           
                
            var array = list.ToArray();
            return array;
        }
    }
}
