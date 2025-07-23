using Cayd.Uuid.Exceptions;
using System;

namespace Cayd.Uuid
{
    public static partial class Uuid
    {
        /// <summary>
        /// UUIDv8 provides a format for experimental or vendor-specific use cases. The only requirement is that the variant and version bits
        /// must be set. UUIDv8's uniqueness is based on implementation specific and must not be assumed.
        /// </summary>
        public static class V8
        {
            /// <summary>
            /// Generates a new <see cref="Guid"/> based on UUIDv8 rules.
            /// <para>
            /// While generating <see cref="Guid"/>, the order of a given byte array changes accordingly.
            /// </para>
            /// </summary>
            /// <param name="customData">Bytes of the given custom data. The length of the array must be 16.</param>
            /// <returns>Returns a <see cref="Guid"/> based on UUIDv8 rules.</returns>
            /// <exception cref="WrongDataLengthException">When the length of <c>customData</c> is wrong</exception>
            public static Guid Generate(byte[] customData)
            {
                if (customData.Length != 16)
                    throw new WrongDataLengthException(nameof(customData), length: 16);

                // 'ver' bits
                customData[6] = (byte)(0x80 | (customData[6] & 0x0F));

                // 'var' bits
                customData[8] = (byte)(0x80 | (customData[8] & 0x3F));

                return GenerateGuidFromBytes(customData);
            }

#if !NETSTANDARD2_0
            /// <summary>
            /// Generates a new <see cref="Guid"/> based on UUIDv8 rules.
            /// <para>
            /// While generating <see cref="Guid"/>, the order of a given byte array changes accordingly.
            /// </para>
            /// </summary>
            /// <param name="customData">Bytes of the given custom data. The length of the array must be 16.</param>
            /// <returns>Returns a <see cref="Guid"/> based on UUIDv8 rules.</returns>
            /// <exception cref="WrongDataLengthException">When the length of <c>customData</c> is wrong</exception>
            public static Guid Generate(Span<byte> customData)
            {
                if (customData.Length != 16)
                    throw new WrongDataLengthException(nameof(customData), length: 16);

                // 'ver' bits
                customData[6] = (byte)(0x80 | (customData[6] & 0x0F));

                // 'var' bits
                customData[8] = (byte)(0x80 | (customData[8] & 0x3F));

                return GenerateGuidFromBytes(customData);
            }
#endif

            /// <summary>
            /// Generates a new <see cref="Guid"/> based on UUIDv8 rules.
            /// <para>
            /// While generating <see cref="Guid"/>, the order of a given byte array changes accordingly.
            /// </para>
            /// </summary>
            /// <param name="customA">First 48 bits. The length of the array must be 6.</param>
            /// <param name="customB">12 bits after the version bits. The length of the array must be 2.</param>
            /// <param name="customC">62 bits after the variant bits. The length of the array must be 8.</param>
            /// <returns>Returns a <see cref="Guid"/> based on UUIDv8 rules.</returns>
            /// <exception cref="WrongDataLengthException">When the length of any custom data is wrong</exception>
            public static Guid Generate(byte[] customA, byte[] customB, byte[] customC)
            {
                if (customA.Length != 6)
                    throw new WrongDataLengthException(nameof(customA), length: 6);
                if (customB.Length != 2)
                    throw new WrongDataLengthException(nameof(customA), length: 2);
                if (customC.Length != 8)
                    throw new WrongDataLengthException(nameof(customC), length: 8);

                var bytes = new byte[16];
                Buffer.BlockCopy(customA, 0, bytes, 0, customA.Length);
                Buffer.BlockCopy(customB, 0, bytes, customA.Length, customB.Length);
                Buffer.BlockCopy(customC, 0, bytes, customB.Length + customB.Length, customC.Length);

                return Generate(bytes);
            }

#if !NETSTANDARD2_0
            /// <summary>
            /// Generates a new <see cref="Guid"/> based on UUIDv8 rules.
            /// <para>
            /// While generating <see cref="Guid"/>, the order of a given byte array changes accordingly.
            /// </para>
            /// </summary>
            /// <param name="customA">First 48 bits. The length of the array must be 6.</param>
            /// <param name="customB">12 bits after the version bits. The length of the array must be 2.</param>
            /// <param name="customC">62 bits after the variant bits. The length of the array must be 8.</param>
            /// <returns>Returns a <see cref="Guid"/> based on UUIDv8 rules.</returns>
            /// <exception cref="WrongDataLengthException">When the length of any custom data is wrong</exception>
            public static Guid Generate(Span<byte> customA, Span<byte> customB, Span<byte> customC)
            {
                if (customA.Length != 6)
                    throw new WrongDataLengthException(nameof(customA), length: 6);
                if (customB.Length != 2)
                    throw new WrongDataLengthException(nameof(customA), length: 2);
                if (customC.Length != 8)
                    throw new WrongDataLengthException(nameof(customC), length: 8);

                Span<byte> bytes = new byte[16];
                customA.CopyTo(bytes.Slice(0, customA.Length));
                customA.CopyTo(bytes.Slice(customA.Length, customB.Length));
                customA.CopyTo(bytes.Slice(customA.Length + customB.Length, customC.Length));

                return Generate(bytes);
            }
#endif
        }
    }
}
