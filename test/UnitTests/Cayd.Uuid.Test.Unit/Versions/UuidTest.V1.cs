using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Cayd.Uuid.Test.Unit
{
    public partial class UuidTest
    {
        [Fact]
        public void V1_Generate_ShouldGenerateCorrectGuid()
        {
            // Act
            var result = Uuid.V1.Generate();

            // Assert
            CheckVersionAndVariantBits(Uuid.GetBytesOfGuid(result), 0x10, 0x80);
        }

        [Fact]
        public void V1_Generate_ShouldGenerateUniqueAndSequentialGuids()
        {
            // Arrange
            var guids = new List<Guid>();

            // Act
            for (int i = 0; i < _numberOfGuidsToGenerate; ++i)
            {
                guids.Add(Uuid.V1.Generate(true));
            }

            // Assert
            var guidSet = guids.ToHashSet();
            Assert.Equal(guids.Count, guidSet.Count);

            var sortedGuids = guids.OrderBy(g => g, Comparer<Guid>.Create((x, y) => x.CompareTo(y))).ToList();
            Assert.Equal(guids, sortedGuids);
        }
    }
}
