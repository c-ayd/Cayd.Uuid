using System.Collections.Generic;
using System;
using Xunit;
using System.Linq;

namespace Cayd.Uuid.Test.Unit
{
    public partial class UuidTest
    {
        [Fact]
        public void V6_Generate_ShouldGenerateCorrectGuid()
        {
            // Act
            var result = Uuid.V6.Generate();

            // Assert
            CheckVersionAndVariantBits(Uuid.GetBytesOfGuid(result), 0x60, 0x80);
        }

        [Fact]
        public void V6_Generate_ShouldGenerateUniqueGuids()
        {
            // Arrange
            var guids = new List<Guid>();

            // Act
            for (int i = 0; i < _numberOfGuidsToGenerate; ++i)
            {
                guids.Add(Uuid.V6.Generate(true));
            }

            // Assert
            var guidSet = guids.ToHashSet();
            Assert.Equal(guids.Count, guidSet.Count);
        }
    }
}
