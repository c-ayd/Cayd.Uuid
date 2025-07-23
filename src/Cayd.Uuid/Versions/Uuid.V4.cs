using System;
using System.Security.Cryptography;

namespace Cayd.Uuid
{
    public static partial class Uuid
    {
        /// <summary>
        /// UUIDv4 is meant for generating UUIDs from truly random or pseudorandom numbers.
        /// </summary>
        public static class V4
        {
            /// <summary>
            /// Generates a new <see cref="Guid"/> based on UUIDv4 rules.
            /// </summary>
            /// <returns>Returns a <see cref="Guid"/> based on UUIDv4 rules.</returns>
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
