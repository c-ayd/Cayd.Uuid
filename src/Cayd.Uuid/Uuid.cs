using System;

namespace Cayd.Uuid
{
    public static partial class Uuid
    {
        public static readonly Guid Max = new Guid(0xFFFFFFFF, 0xFFFF, 0xFFFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF);
        public static readonly Guid Empty = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        /** 
         * Guid byte order: [3][2][1][0]-[5][4]-[7][6] - [8-15]
         */
        private static byte[] GetBytesOfGuid(Guid guid)
        {
            var bytes = guid.ToByteArray();

            SwapBytes(bytes, 0, 3);
            SwapBytes(bytes, 1, 2);
            SwapBytes(bytes, 4, 5);
            SwapBytes(bytes, 6, 7);

            return bytes;
        }

        private static Guid GenerateGuidFromBytes(byte[] bytes)
        {
            SwapBytes(bytes, 0, 3);
            SwapBytes(bytes, 1, 2);
            SwapBytes(bytes, 4, 5);
            SwapBytes(bytes, 6, 7);

            if (bytes.Length > 16)
            {
                var trimmedBytes = new byte[16];
                Buffer.BlockCopy(bytes, 0, trimmedBytes, 0, 16);
                return new Guid(trimmedBytes);
            }

            return new Guid(bytes);
        }

        private static void SwapBytes(byte[] bytes, int leftIndex, int rightIndex)
        {
            byte temp = bytes[leftIndex];
            bytes[leftIndex] = bytes[rightIndex];
            bytes[rightIndex] = temp;
        }
    }
}
