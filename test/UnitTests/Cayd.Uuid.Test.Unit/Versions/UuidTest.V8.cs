using System.Collections.Generic;
using System;
using Xunit;
using System.Linq;
using System.Security.Cryptography;
using Cayd.Uuid.Exceptions;

namespace Cayd.Uuid.Test.Unit
{
    public partial class UuidTest
    {
        [Fact]
        public void V8_Generate_ShouldGenerateCorrectGuid()
        {
            // Arrange
            var customData = new byte[16];
            RandomNumberGenerator.Fill(customData);

            // Act
            var result = Uuid.V8.Generate(customData);

            // Assert
            CheckVersionAndVariantBits(Uuid.GetBytesOfGuid(result), 0x80, 0x80);
        }

        [Fact]
        public void V8_GeneratePartial_ShouldGenerateCorrectGuid()
        {
            // Arrange
            var customA = new byte[6];
            var customB = new byte[2];
            var customC = new byte[8];
            RandomNumberGenerator.Fill(customA);
            RandomNumberGenerator.Fill(customB);
            RandomNumberGenerator.Fill(customC);

            // Act
            var result = Uuid.V8.Generate(customA, customB, customC);

            // Assert
            CheckVersionAndVariantBits(Uuid.GetBytesOfGuid(result), 0x80, 0x80);
        }

        [Fact]
        public void V8_Generate_WhenLengthIsWrong_ShouldThrowException()
        {
            // Arrange
            var customData = new byte[4];
            RandomNumberGenerator.Fill(customData);

            // Act
            var result = Record.Exception(() =>
            {
                Uuid.V8.Generate(customData);
            });

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WrongDataLengthException>(result);
        }

        [Theory]
        [InlineData(4, 2, 8)]
        [InlineData(6, 4, 8)]
        [InlineData(6, 2, 4)]
        public void V8_GeneratePartial_WhenLengthIsWrong_ShouldThrowException(int lengthA, int lengthB, int lengthC)
        {
            // Arrange
            var customA = new byte[lengthA];
            var customB = new byte[lengthB];
            var customC = new byte[lengthC];
            RandomNumberGenerator.Fill(customA);
            RandomNumberGenerator.Fill(customB);
            RandomNumberGenerator.Fill(customC);

            // Act
            var result = Record.Exception(() =>
            {
                var result = Uuid.V8.Generate(customA, customB, customC);
            });

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WrongDataLengthException>(result);
        }

        [Fact]
        public void V8_Generate_ShouldGenerateUniqueGuids()
        {
            // Arrange
            var guids = new List<Guid>();

            // Act
            for (int i = 0; i < _numberOfGuidsToGenerate; ++i)
            {
                var customData = new byte[16];
                RandomNumberGenerator.Fill(customData);

                guids.Add(Uuid.V8.Generate(customData));
            }

            // Assert
            var guidSet = guids.ToHashSet();
            Assert.Equal(guids.Count, guidSet.Count);
        }

        [Fact]
        public void V8_GeneratePartial_ShouldGenerateUniqueGuids()
        {
            // Arrange
            var guids = new List<Guid>();

            // Act
            for (int i = 0; i < _numberOfGuidsToGenerate; ++i)
            {
                var customA = new byte[6];
                var customB = new byte[2];
                var customC = new byte[8];
                RandomNumberGenerator.Fill(customA);
                RandomNumberGenerator.Fill(customB);
                RandomNumberGenerator.Fill(customC);

                guids.Add(Uuid.V8.Generate(customA, customB, customC));
            }

            // Assert
            var guidSet = guids.ToHashSet();
            Assert.Equal(guids.Count, guidSet.Count);
        }
    }
}
