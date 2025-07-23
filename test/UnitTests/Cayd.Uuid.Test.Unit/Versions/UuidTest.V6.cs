using System.Collections.Generic;
using System;
using Xunit;
using System.Linq;

namespace Cayd.Uuid.Test.Unit
{
    public partial class UuidTest
    {
        [Fact]
        public void V6_Generate_ShouldGenerateUniqueAndSequentialGuids()
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

            var sortedGuids = guids.OrderByDescending(g => g, Comparer<Guid>.Create((x, y) => x.CompareTo(y))).ToList();
            guids.Reverse();
            Assert.Equal(guids, sortedGuids);
        }
    }
}
