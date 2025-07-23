using System;
using System.Security.Cryptography;

namespace Cayd.Uuid
{
    public static partial class Uuid
    {
        /// <summary>
        /// UUIDv7 features a time-ordered value field derived from the widely implemented and well-known Unix Epoch timestamp source,
        /// the number of milliseconds since midnight 1 Jan 1970 UTC, leap seconds excluded. Generally, UUIDv7 has improved entropy characteristics
        /// over UUIDv1 or UUIDv6.
        /// </summary>
        public static class V7
        {
            /// <summary>
            /// Generates a new <see cref="Guid"/> based on UUIDv7 rules.
            /// </summary>
            /// <returns>Returns a <see cref="Guid"/> based on UUIDv7 rules.</returns>
            public static Guid Generate()
                => GenerateGuid(DateTimeOffset.UtcNow);

            /// <summary>
            /// Generates a new <see cref="Guid"/> based on UUIDv7 rules.
            /// </summary>
            /// <param name="offset">Time offset to be used to generate a <see cref="Guid"/></param>
            /// <returns>Returns a <see cref="Guid"/> based on UUIDv7 rules.</returns>
            public static Guid Generate(DateTimeOffset offset)
                => GenerateGuid(offset);

            private static Guid GenerateGuid(DateTimeOffset offset)
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

                long timestamp = offset.ToUnixTimeMilliseconds();

                // 'unix_ts_ms' bits
                bytes[0] = (byte)((timestamp >> 40) & 0xFF);
                bytes[1] = (byte)((timestamp >> 32) & 0xFF);
                bytes[2] = (byte)((timestamp >> 24) & 0xFF);
                bytes[3] = (byte)((timestamp >> 16) & 0xFF);
                bytes[4] = (byte)((timestamp >> 8) & 0xFF);
                bytes[5] = (byte)(timestamp & 0xFF);

                // 'ver' bits
                bytes[6] = (byte)(0x70 | (bytes[6] & 0x0F));

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
