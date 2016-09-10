using System;
using System.Collections.Generic;

namespace Common
{
    public static class RabinKarp
    {
        public static List<byte[]> Slice(byte[] b, ulong mask)
        {
            // mask of 13 bits will give approximately 8192 byte chunks
            List<byte[]> chunks = new List<byte[]>();

            // magic numbers
            // Q - large prime number 
            ulong Q = 10000007;
            ulong D = 512;

            int windowSize = 48;

            // have power raised to appropriate extent 
            ulong pow = 1;
            for (int k = 1; k < windowSize; k++)
            {
                pow = (pow * D) % Q;
            }

            // keep track of where last chunk ended and new chunk is to begin
            int lastIndex = 0;

            ulong hash = GetFirstHash(SubArray(b, 0, windowSize), Q, D);

            // next hashes can reuse computations
            for (int j = 1; j <= b.Length - windowSize; j++)
            {
                hash = GetSubsequentHash(b[j - 1], b[j + windowSize - 1], hash, Q, D, pow);

                // as size of bit mask increases, the probability that at least one relevant bit is 0 increases,
                // so chunk sizes will be smaller on average.
                if ((hash & mask) == 0)
                {
                    chunks.Add(SubArray(b, lastIndex, j + 1 - lastIndex));
                    lastIndex = j + 1;
                }
            }
            // no more matches at end of file, so add last chunk manually
            if (lastIndex < b.Length - 1)
            {
                chunks.Add(SubArray(b, lastIndex, (b.Length - lastIndex)));
            }
            return chunks;
        }

        public static ulong GetFirstHash(byte[] window, ulong Q, ulong D)
        {
            ulong hash = 0;
            for (int i = 0; i < window.Length; i++)
            {
                hash = (hash * D + window[i]) % Q;
            }
            return hash;
        }

        public static ulong GetSubsequentHash(byte removeByte, byte addByte, ulong hash, ulong Q, ulong D, ulong pow)
        {
            // add next element to window
            hash = (hash + Q - pow * removeByte % Q) % Q;
            // remove last element from window
            hash = (hash * D + addByte) % Q;

            return hash;
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}