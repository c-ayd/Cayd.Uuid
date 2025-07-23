using System.Collections.Generic;
using System;
using Xunit;
using System.Linq;

namespace Cayd.Uuid.Test.Unit
{
    public partial class UuidTest
    {
        [Fact]
        public void V5_Generate_ShouldGenerateCorrectGuid()
        {
            // Act
            var result = Uuid.V5.Generate(Uuid.V5.DnsNamespaceId, GenerateRandomString());

            // Assert
            CheckVersionAndVariantBits(Uuid.GetBytesOfGuid(result), 0x50, 0x80);
        }

        [Fact]
        public void V5_Generate_ShouldGenerateUniqueGuids()
        {
            // Arrange
            var guids = new List<Guid>();

            // Act
            for (int i = 0; i < _numberOfGuidsToGenerate; ++i)
            {
                guids.Add(Uuid.V5.Generate(Uuid.V5.DnsNamespaceId, GenerateRandomString()));
            }

            // Assert
            var guidSet = guids.ToHashSet();
            Assert.Equal(guids.Count, guidSet.Count);
        }

        [Fact]
        public void V5_Generate_ShouldGenerateSameGuidForSameNamespaceIdAndSameName()
        {
            // Arrange
            var dnsGuids = new Guid[2];
            var urlGuids = new Guid[2];
            var oidGuids = new Guid[2];
            var x500Guids = new Guid[2];
            var customGuids = new Guid[2];

            var customGuid = Guid.NewGuid();
            var name = "Test";

            // Act
            dnsGuids[0] = Uuid.V5.Generate(Uuid.V5.DnsNamespaceId, name);
            dnsGuids[1] = Uuid.V5.Generate(Uuid.V5.DnsNamespaceId, name);

            urlGuids[0] = Uuid.V5.Generate(Uuid.V5.UrlNamespaceId, name);
            urlGuids[1] = Uuid.V5.Generate(Uuid.V5.UrlNamespaceId, name);

            oidGuids[0] = Uuid.V5.Generate(Uuid.V5.OidNamespaceId, name);
            oidGuids[1] = Uuid.V5.Generate(Uuid.V5.OidNamespaceId, name);

            x500Guids[0] = Uuid.V5.Generate(Uuid.V5.X500NamespaceId, name);
            x500Guids[1] = Uuid.V5.Generate(Uuid.V5.X500NamespaceId, name);

            customGuids[0] = Uuid.V5.Generate(customGuid, name);
            customGuids[1] = Uuid.V5.Generate(customGuid, name);

            // Assert
            Assert.Equal(dnsGuids[0], dnsGuids[1]);
            Assert.Equal(urlGuids[0], urlGuids[1]);
            Assert.Equal(oidGuids[0], oidGuids[1]);
            Assert.Equal(x500Guids[0], x500Guids[1]);
            Assert.Equal(customGuids[0], customGuids[1]);

            Assert.NotEqual(dnsGuids, urlGuids);
            Assert.NotEqual(dnsGuids, oidGuids);
            Assert.NotEqual(dnsGuids, x500Guids);
            Assert.NotEqual(dnsGuids, customGuids);

            Assert.NotEqual(urlGuids, oidGuids);
            Assert.NotEqual(urlGuids, x500Guids);
            Assert.NotEqual(urlGuids, customGuids);

            Assert.NotEqual(oidGuids, x500Guids);
            Assert.NotEqual(oidGuids, customGuids);

            Assert.NotEqual(x500Guids, customGuids);
        }
    }
}
