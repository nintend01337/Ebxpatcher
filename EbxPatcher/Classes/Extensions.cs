using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbxPatcher.Classes
{
    public static class Extensions
    {
        public static int IndexOf(byte[] source, byte[] pattern, int offset)
        {
            int success = 0;
            for (int i = offset; i < source.Length; i++)
            {
                if (source[i] == pattern[success])
                {
                    success++;
                }
                else
                {
                    success = 0;
                }

                if (pattern.Length == success)
                {
                    return i - pattern.Length + 1;
                }
            }
            return -1;
        }
    }
}
