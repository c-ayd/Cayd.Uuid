using System.Text;
using System;
using Xunit;

namespace Cayd.Uuid.Test.Unit
{
    public partial class UuidTest
    {
        private readonly int _numberOfGuidsToGenerate = 1000;

        private string GenerateRandomString()
        {
            var random = new Random();
            var builder = new StringBuilder(50);

            for (int i = 0; i < 50; i++)
            {
                builder.Append((char)random.Next(32, 127));
            }

            return builder.ToString();
        }

        private void CheckVersionAndVariantBits(byte[] bytes, byte versionBits, byte variantBits)
        {
            Assert.True((bytes[6] & versionBits) == versionBits, "The version bits differ.");
            Assert.True((bytes[8] & variantBits) == variantBits, "The variant bits differ.");
        }
    }
}
