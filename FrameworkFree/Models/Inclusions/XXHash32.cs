using System.Runtime.CompilerServices;
using Own.Permanent;
namespace Inclusions
{
    internal static partial class XXHash32
    {
        private const uint PRIME32_1 = 2654435761U;
        private const uint PRIME32_2 = 2246822519U;
        private const uint PRIME32_3 = 3266489917U;
        private const uint PRIME32_4 = 668265263U;
        private const uint PRIME32_5 = 374761393U;

        public unsafe static uint Hash(in string input, in uint seed = Constants.Zero)
        {
            fixed (char* buffer = input)
            {
                return Hash((byte*)buffer, input.Length * sizeof(char), seed);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe uint Hash(byte* buffer, in int len, in uint seed = Constants.Zero)
        {
            unchecked
            {
                uint h32;

                byte* bEnd = buffer + len;

                if (len >= 16)
                {
                    byte* limit = bEnd - 16;

                    uint v1 = seed + PRIME32_1 + PRIME32_2;
                    uint v2 = seed + PRIME32_2;
                    uint v3 = seed + Constants.Zero;
                    uint v4 = seed - PRIME32_1;

                    do
                    {
                        v1 += *((uint*)buffer) * PRIME32_2;
                        buffer += sizeof(uint);
                        v2 += *((uint*)buffer) * PRIME32_2;
                        buffer += sizeof(uint);
                        v3 += *((uint*)buffer) * PRIME32_2;
                        buffer += sizeof(uint);
                        v4 += *((uint*)buffer) * PRIME32_2;
                        buffer += sizeof(uint);

                        v1 = rol13(v1);
                        v2 = rol13(v2);
                        v3 = rol13(v3);
                        v4 = rol13(v4);

                        v1 *= PRIME32_1;
                        v2 *= PRIME32_1;
                        v3 *= PRIME32_1;
                        v4 *= PRIME32_1;
                    }
                    while (buffer <= limit);

                    h32 = rol1(v1) + rol7(v2) + rol12(v3) + rol18(v4);
                }
                else
                {
                    h32 = seed + PRIME32_5;
                }

                h32 += (uint)len;


                while (buffer + 4 <= bEnd)
                {
                    h32 += *((uint*)buffer) * PRIME32_3;
                    h32 = rol17(h32) * PRIME32_4;
                    buffer += 4;
                }

                while (buffer < bEnd)
                {
                    h32 += (uint)(*buffer) * PRIME32_5;
                    h32 = rol11(h32) * PRIME32_1;
                    buffer++;
                }

                h32 ^= h32 >> 15;
                h32 *= PRIME32_2;
                h32 ^= h32 >> 13;
                h32 *= PRIME32_3;
                h32 ^= h32 >> 16;

                return h32;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint rol1(in uint x) { return (x << Constants.One) | (x >> (32 - Constants.One)); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint rol7(in uint x) { return (x << 7) | (x >> (32 - 7)); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint rol11(in uint x) { return (x << 11) | (x >> (32 - 11)); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint rol12(in uint x) { return (x << 12) | (x >> (32 - 12)); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint rol18(in uint x) { return (x << 18) | (x >> (32 - 18)); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint rol13(in uint x) { return (x << 13) | (x >> (32 - 13)); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint rol17(in uint x) { return (x << 17) | (x >> (32 - 17)); }

    }


}
