using System;

namespace Cayd.Uuid.Exceptions
{
    public class WrongDataLengthException : Exception
    {
        public WrongDataLengthException(string name, int length)
            : base($"The length of {name} must be {length}.")
        {
        }
    }
}
