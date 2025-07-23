using System.Text;
using System;

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
    }
}
