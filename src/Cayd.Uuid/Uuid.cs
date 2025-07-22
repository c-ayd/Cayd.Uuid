using System;

namespace Cayd.Uuid
{
    public static partial class Uuid
    {
        public static readonly Guid Max = new Guid(0xFFFFFFFF, 0xFFFF, 0xFFFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF);
        public static readonly Guid Empty = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    }
}
