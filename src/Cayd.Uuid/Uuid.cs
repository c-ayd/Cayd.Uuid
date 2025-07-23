using System;

namespace Cayd.Uuid
{
    /// <summary>
    /// Generates <see cref="Guid"/> based on RFC 9562.
    /// </summary>
    public static partial class Uuid
    {
        /// <summary>
        /// Read only <see cref="Guid"/> instance whose value is 'ffffffff-ffff-ffff-ffff-ffffffffffff'.
        /// </summary>
        public static readonly Guid Max = new Guid(0xFFFFFFFF, 0xFFFF, 0xFFFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF);
        /// <summary>
        /// Read only <see cref="Guid"/> instance whose value is '00000000-0000-0000-0000-000000000000'.
        /// </summary>
        public static readonly Guid Empty = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        /// <summary>
        /// Gets the correct UUID byte format of a given <see cref="Guid"/>.
        /// Normally, the byte order of <see cref="Guid"/> is as follow:
        /// <code>
        /// [3][2][1][0]-[5][4]-[7][6]-[8-15]
        /// </code>
        /// </summary>
        /// <param name="guid">Value to get the bytes from</param>
        /// <returns>Returns a byte array of a given <see cref="Guid"/>.</returns>
        public static byte[] GetBytesOfGuid(Guid guid)
        {
            var bytes = guid.ToByteArray();

            SwapBytes(bytes, 0, 3);
            SwapBytes(bytes, 1, 2);
            SwapBytes(bytes, 4, 5);
            SwapBytes(bytes, 6, 7);

            return bytes;
        }

        /// <summary>
        /// Converts a given byte array to generate its correct <see cref="Guid"/> representation based on UUID byte format.
        /// Normally, the byte order of <see cref="Guid"/> is as follow:
        /// <code>
        /// [3][2][1][0]-[5][4]-[7][6]-[8-15]
        /// </code>
        /// While generating <see cref="Guid"/>, the order of a given byte array changes accordingly.
        /// </summary>
        /// <param name="bytes">Byte array to generate <see cref="Guid"/> from</param>
        /// <returns>Returns a <see cref="Guid"/> based-on a given UUID byte format.</returns>
        public static Guid GenerateGuidFromBytes(byte[] bytes)
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

#if !NETSTANDARD2_0
        /// <summary>
        /// Converts a given byte array to generate its correct <see cref="Guid"/> representation based on UUID byte format.
        /// Normally, the byte order of <see cref="Guid"/> is as follow:
        /// <code>
        /// [3][2][1][0]-[5][4]-[7][6]-[8-15]
        /// </code>
        /// While generating <see cref="Guid"/>, the order of a given byte array changes accordingly.
        /// </summary>
        /// <param name="bytes">Byte array to generate <see cref="Guid"/> from</param>
        /// <returns>Returns a <see cref="Guid"/> based on a given UUID byte format.</returns>
        public static Guid GenerateGuidFromBytes(Span<byte> bytes)
        {
            SwapBytes(bytes, 0, 3);
            SwapBytes(bytes, 1, 2);
            SwapBytes(bytes, 4, 5);
            SwapBytes(bytes, 6, 7);

            if (bytes.Length > 16)
            {
                var trimmedBytes = bytes.Slice(0, 16);
                return new Guid(trimmedBytes);
            }

            return new Guid(bytes);
        }
#endif

        private static void SwapBytes(byte[] bytes, int leftIndex, int rightIndex)
        {
            byte temp = bytes[leftIndex];
            bytes[leftIndex] = bytes[rightIndex];
            bytes[rightIndex] = temp;
        }

#if !NETSTANDARD2_0
        private static void SwapBytes(Span<byte> bytes, int leftIndex, int rightIndex)
        {
            byte temp = bytes[leftIndex];
            bytes[leftIndex] = bytes[rightIndex];
            bytes[rightIndex] = temp;
        }
#endif
    }
}
