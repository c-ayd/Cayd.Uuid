using System.Collections.Generic;
using System;
using Xunit;
using System.Linq;

namespace Cayd.Uuid.Test.Unit
{
    public partial class UuidTest
    {
        [Fact]
        public void V7_Generate_ShouldGenerateUniqueGuids()
        {
            // Arrange
            var guids = new List<Guid>();

            // Act
            for (int i = 0; i < _numberOfGuidsToGenerate; ++i)
            {
                guids.Add(Uuid.V7.Generate());
            }

            // Assert
            var guidSet = guids.ToHashSet();
            Assert.Equal(guids.Count, guidSet.Count);
        }
    }
}
