using Cayd.Uuid.Exceptions;
using System;

namespace Cayd.Uuid
{
    public static partial class Uuid
    {
        public static class V8
        {
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
