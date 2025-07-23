using System;
using System.Security.Cryptography;
using System.Text;

namespace Cayd.Uuid
{
    public static partial class Uuid
    {
        public static class V3
        {
            public static readonly Guid DnsNamespaceId = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
            public static readonly Guid UrlNamespaceId = new Guid("6ba7b811-9dad-11d1-80b4-00c04fd430c8");
            public static readonly Guid OidNamespaceId = new Guid("6ba7b812-9dad-11d1-80b4-00c04fd430c8");
            public static readonly Guid X500NamespaceId = new Guid("6ba7b814-9dad-11d1-80b4-00c04fd430c8");

            public static Guid Generate(Guid namespaceId, string name)
            {
                var namespaceBytes = GetBytesOfGuid(namespaceId);
                var nameBytes = Encoding.UTF8.GetBytes(name);

                var data = new byte[namespaceBytes.Length + nameBytes.Length];
                Buffer.BlockCopy(namespaceBytes, 0, data, 0, namespaceBytes.Length);
                Buffer.BlockCopy(nameBytes, 0, data, namespaceBytes.Length, nameBytes.Length);

                byte[] bytes;
                using (var md5 = MD5.Create())
                {
                    bytes = md5.ComputeHash(data);
                }

                // 'ver' bits
                bytes[6] = (byte)(0x30 | (bytes[6] & 0x0F));

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
