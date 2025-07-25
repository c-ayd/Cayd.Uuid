﻿using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace Cayd.Uuid
{
    public static partial class Uuid
    {
        /// <summary>
        /// UUIDv1 is a time-based UUID featuring a 60-bit timestamp represented by Coordinated Universal Time (UTC) as a count of 100-nanosecond intervals 
        /// since 00:00:00.00, 15 October 1582 (the date of Gregorian reform to the Christian calendar).
        /// </summary>
        public static class V1
        {
            /// <summary>
            /// Whether it should use real a MAC address or a random node ID for node bits.
            /// </summary>
            public static bool UseRandomNodeId = false;

            private static readonly DateTime GregorianReformDate = new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);
            private static ushort _clockSequence = GenerateClockSequence();
            private static byte[] _nodeId = GetNodeId();

            private static object _lockObj = new object();
            private static long _lastTimestamp = 0;

            /// <summary>
            /// Generates a new <see cref="Guid"/> based on UUIDv1 rules.
            /// </summary>
            /// <param name="useLock">Whether to use the locking mechanism for multithreading situations</param>
            /// <returns>Returns a <see cref="Guid"/> based on UUIDv1 rules.</returns>
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

            /// <summary>
            /// Refreshes the node ID.
            /// If the new node ID is different than the old one, it also generates a new clock sequence.
            /// </summary>
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

                var timestamp = (DateTime.UtcNow - GregorianReformDate).Ticks;

                // If the clock is set backwards, generate a new clock sequence to ensure uniquness
                if (timestamp < _lastTimestamp)
                {
                    _clockSequence = GenerateClockSequence();
                }

                _lastTimestamp = timestamp;

                // 'time_low' bits
                bytes[0] = (byte)((timestamp >> 24) & 0xFF);
                bytes[1] = (byte)((timestamp >> 16) & 0xFF);
                bytes[2] = (byte)((timestamp >> 8) & 0xFF);
                bytes[3] = (byte)(timestamp & 0xFF);

                // 'time_mid' bits
                bytes[4] = (byte)((timestamp >> 40) & 0xFF);
                bytes[5] = (byte)((timestamp >> 32) & 0xFF);

                // 'ver' bits
                bytes[6] = 0x10;

                // 'time_high' bits
                bytes[6] |= (byte)((timestamp >> 56) & 0x0F);
                bytes[7] = (byte)((timestamp >> 48) & 0xFF);

                // 'var' bits
                bytes[8] = 0x80;

                // 'clock_seq' bits
                bytes[8] |= (byte)((_clockSequence >> 8) & 0x3F);
                bytes[9] = (byte)(_clockSequence & 0xFF);

                // 'node' bits
                for (int i = 0; i < 6; ++i)
                {
                    bytes[i + 10] = _nodeId[i];
                }

#if !NETSTANDARD2_0
                return GenerateGuidFromBytes(bytes.AsSpan());
#else
                return GenerateGuidFromBytes(bytes);
#endif
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
                if (!UseRandomNodeId)
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
