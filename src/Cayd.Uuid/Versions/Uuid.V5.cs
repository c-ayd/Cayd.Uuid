using System;
using System.Security.Cryptography;
using System.Text;

namespace Cayd.Uuid
{
    public static partial class Uuid
    {
        /// <summary>
        /// UUIDv5 is meant for generating UUIDs from names that are drawn from, and unique within, some namespace.
        /// <para>
        /// UUIDv5 values are created by computing an SHA-1 hash over a given Namespace ID value concatenated with the desired name value
        /// after both have been converted to a canonical sequence of octets, as defined by the standards or conventions of its namespace,
        /// in network byte order.
        /// </para>
        /// </summary>
        public static class V5
        {
            /// <summary>
            /// The <see cref="Guid"/> for DNS namespace to generate UUIDs based on DNS names.
            /// </summary>
            public static readonly Guid DnsNamespaceId = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
            /// <summary>
            /// The <see cref="Guid"/> for URL namespace to generate UUIDs based on URL names.
            /// </summary>
            public static readonly Guid UrlNamespaceId = new Guid("6ba7b811-9dad-11d1-80b4-00c04fd430c8");
            /// <summary>
            /// The <see cref="Guid"/> for OID namespace to generate UUIDs based on OID names.
            /// </summary>
            public static readonly Guid OidNamespaceId = new Guid("6ba7b812-9dad-11d1-80b4-00c04fd430c8");
            /// <summary>
            /// The <see cref="Guid"/> for X.500 Directory namespace to generate UUIDs based on X.500 names.
            /// </summary>
            public static readonly Guid X500NamespaceId = new Guid("6ba7b814-9dad-11d1-80b4-00c04fd430c8");

            /// <summary>
            /// Generates a new <see cref="Guid"/> based on UUIDv5 rules.
            /// </summary>
            /// <param name="namespaceId">Namespace ID to base a <see cref="Guid"/> that will be generated</param>
            /// <param name="name">Name to generate a <see cref="Guid"/> from within the given namespace</param>
            /// <returns>Returns a <see cref="Guid"/> based on UUIDv5 rules.</returns>
            public static Guid Generate(Guid namespaceId, string name)
            {
                var namespaceBytes = GetBytesOfGuid(namespaceId);
                var nameBytes = Encoding.UTF8.GetBytes(name);

                var data = new byte[namespaceBytes.Length + nameBytes.Length];
                Buffer.BlockCopy(namespaceBytes, 0, data, 0, namespaceBytes.Length);
                Buffer.BlockCopy(nameBytes, 0, data, namespaceBytes.Length, nameBytes.Length);

                byte[] bytes;
                using (var sha1 = SHA1.Create())
                {
                    bytes = sha1.ComputeHash(data);
                }

                // 'ver' bits
                bytes[6] = (byte)(0x50 | (bytes[6] & 0x0F));

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
