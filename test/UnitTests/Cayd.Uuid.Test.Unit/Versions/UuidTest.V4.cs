using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Cayd.Uuid.Test.Unit
{
    public partial class UuidTest
    {
        [Fact]
        public void V4_Generate_ShouldGenerateUniqueGuids()
        {
            // Arrange
            var guids = new List<Guid>();

            // Act
            for (int i = 0; i < _numberOfGuidsToGenerate; ++i)
            {
                guids.Add(Uuid.V4.Generate());
            }

            // Assert
            var guidSet = guids.ToHashSet();
            Assert.Equal(guids.Count, guidSet.Count);
        }
    }
}
