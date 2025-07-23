using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace Cayd.Uuid
{
    public static partial class Uuid
    {
        public static class V1
        {
            private static readonly DateTime GregorianReformDate = new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);
            private static ushort _clockSequence = GenerateClockSequence();
            private static byte[] _nodeId = GetNodeId();

            private static object _lockObj = new object();
            private static long _lastTimestamp = 0;

            public static Guid Generate(bool useLock = false)
            {
                Guid result;
                if (useLock)
                {
                    lock (_lockObj)
                    {
                        result = GenerateGuid();
                    }
                }
                else
                {
                    result = GenerateGuid();
                }

                return result;
            }

            public static void RefreshNodeId()
            {
                var currentNodeId = GetNodeId();
                var isNodeIdSame = currentNodeId.SequenceEqual(_nodeId);
                if (!isNodeIdSame)
                {
                    _nodeId = currentNodeId;
                    _clockSequence = GenerateClockSequence();
                }
            }

            private static Guid GenerateGuid()
            {
                var bytes = new byte[16];

                var timestamp = (DateTime.UtcNow - GregorianReformDate).Ticks * 10;

                // If the clock is set backwards, generate a new clock sequence to ensure uniquness
                if (timestamp < _lastTimestamp)
                {
                    _clockSequence = GenerateClockSequence();
                }

                _lastTimestamp = timestamp;

                // 'time_low' bytes
                bytes[0] = (byte)((timestamp >> 24) & 0xFF);
                bytes[1] = (byte)((timestamp >> 16) & 0xFF);
                bytes[2] = (byte)((timestamp >> 8) & 0xFF);
                bytes[3] = (byte)(timestamp & 0xFF);

                // 'time_mid' bytes
                bytes[4] = (byte)((timestamp >> 40) & 0xFF);
                bytes[5] = (byte)((timestamp >> 32) & 0xFF);

                // 'ver' bytes
                bytes[6] = 0x10;

                // 'time_high' bytes
                bytes[6] |= (byte)((timestamp >> 56) & 0x0F);
                bytes[7] = (byte)((timestamp >> 48) & 0xFF);

                // 'var' bytes
                bytes[8] = 0x80;

                // 'clock_seq' bytes
                bytes[8] |= (byte)((_clockSequence >> 8) & 0x3F);
                bytes[9] = (byte)(_clockSequence & 0xFF);

                // 'node' bytes
                for (int i = 0; i < 6; ++i)
                {
                    bytes[i + 10] = _nodeId[i];
                }

                return GenerateGuidFromBytes(bytes);
            }

            private static ushort GenerateClockSequence()
            {
                var bytes = new byte[2];

#if NETSTANDARD2_0
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(bytes);
                }
#else
                RandomNumberGenerator.Fill(bytes);
#endif

                return (ushort)(((bytes[0] << 8) & 0x3F) | bytes[1]);
            }

            private static byte[] GetNodeId()
            {
                var macAddresses = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(ni => ni.OperationalStatus == OperationalStatus.Up)
                    .Select(ni => ni.GetPhysicalAddress())
                        .Where(pa => pa != null)
                    .ToList();

                foreach (var macAddress in macAddresses)
                {
                    var bytes = macAddress.GetAddressBytes();
                    if (bytes.Length == 6)
                        return bytes;
                }

                var randomBytes = new byte[6];

#if NETSTANDARD2_0
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomBytes);
                }
#else
                RandomNumberGenerator.Fill(randomBytes);
#endif

                return randomBytes;
            }
        }
    }
}
