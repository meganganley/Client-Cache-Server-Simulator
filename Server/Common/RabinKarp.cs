using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class RabinKarp
    {
        public static List<byte[]> Slice(byte[] b, ulong boundary)
        {
            List<byte[]> ret = new List<byte[]>();
            ulong Q = 1000007; //Use a much large prime number in your code!
            ulong D = 256;
            ulong pow = 1;
            int windowSize = 48;
            for (int k = 1; k < windowSize; k++)
                pow = (pow * D) % Q;
            ulong sig = 0;
            int lastIndex = 0;
            for (int i = 0; i < windowSize; i++)
                sig = (sig * D + (ulong)b[i]) % Q;
            for (int j = 1; j <= b.Length - windowSize; j++)
            {
                sig = (sig + Q - pow * (ulong)b[j - 1] % Q) % Q;
                sig = (sig * D + (ulong)b[j + windowSize - 1]) % Q;
                if ((sig & boundary) == 0)
                {
                    if (j + 1 - lastIndex >= 2048)
                    {
                        ret.Add(SubArray(b, lastIndex, j + 1 - lastIndex));
                        lastIndex = j + 1;
                    }
                }
                else if (j + 1 - lastIndex >= 65536)
                {
                    ret.Add(SubArray(b, lastIndex, j + 1 - lastIndex));
                    lastIndex = j + 1;
                }
            }
            if (lastIndex < b.Length - 1)
                ret.Add(SubArray(b, lastIndex, (b.Length - lastIndex)));
            return ret;
        }
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }


        public static List<string> StringSlice(string s, ulong boundary)
        {
            List<string> ret = new List<string>();
            ulong Q = 100007; //Use a much large prime number in your code!
            ulong D  = 256;
            ulong pow = 1;
            int windowSize = 48;
            for (int k = 1; k < windowSize; k++)
                pow = (pow * D) % Q;
            ulong sig = 0;
            int lastIndex = 0;
            for (int i = 0; i < windowSize; i++)
                sig = (sig * D + (ulong)s[i]) % Q;
            for (int j = 1; j <= s.Length - windowSize; j++)
            {
                sig = (sig + Q - pow * (ulong)s[j - 1] % Q) % Q;
                sig = (sig * D + (ulong)s[j + windowSize - 1]) % Q;
                if ((sig & boundary) == 0)
                {
                    if (j + 1 - lastIndex >= 2048)
                    {
                        ret.Add(s.Substring(lastIndex, j + 1 - lastIndex));
                        lastIndex = j + 1;
                    }
                }
                else if (j + 1 - lastIndex >= 65536)
                {
                    ret.Add(s.Substring(lastIndex, j + 1 - lastIndex));
                    lastIndex = j + 1;
                }
            }
            if (lastIndex < s.Length - 1)
                ret.Add(s.Substring(lastIndex));
            return ret;
        }
    }
}
