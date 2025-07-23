using System;
using System.Security.Cryptography;

namespace Cayd.Uuid
{
    public static partial class Uuid
    {
        public static class V4
        {
            public static Guid Generate()
            {
                var bytes = new byte[16];

#if NETSTANDARD2_0
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(bytes);
                }
#else
                RandomNumberGenerator.Fill(bytes);
#endif

                // 'ver' bits
                bytes[6] = (byte)(0x40 | (bytes[6] & 0x0F));

                // 'var' bits
                bytes[8] = (byte)(0x80 | (bytes[8] & 0x3F));

#if !NETSTANDARD2_0
                return GenerateGuidFromBytes(bytes.AsSpan());
#else
                return GenerateGuidFromBytes(bytes);
#endif
            }
        }
    }
}
