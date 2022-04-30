using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace Pha3z.FrichText
{
    public static unsafe class ColorEncoding
    {
        //https://www.asciitable.com/
        private const char StartingPoint = '0';

        private const int UpperToLowerCaseOffset = 'a' - 'A'; //This is a positive num

        private const int NumNextToFirstLetterOffset = 'A' - ('9' + 1); //This is a positive num

        private static readonly int* HexTable;

        static ColorEncoding()
        {
            //Don't allocate HexTable if there's AVX2 support!
#if RELEASE
            if (Avx2.IsSupported)
            {
                return;
            }
#endif

            const int TotalEntries = 127 + 1;

            HexTable = (int*)NativeMemory.AllocZeroed((nuint)TotalEntries * sizeof(int) + 64);

            //Get the original addr of '0'
            var ZeroPos = (nint)(HexTable + '0');

            //Get next aligned boundary
            var NewZeroPos = (ZeroPos + (64 - 1)) & ~(64 - 1);

            var ByteOffset = NewZeroPos - ZeroPos;

            //We will never deallocate this, so don't bother storing original start
            HexTable += ByteOffset;

            byte HexVal = 0;

            for (var Current = '0'; Current <= '9'; Current++, HexVal++)
            {
                HexTable[Current] = HexVal;
            }

            for (var Current = 'A'; Current <= 'F'; Current++, HexVal++)
            {
                HexTable[Current] = HexVal;
                HexTable[Current + UpperToLowerCaseOffset] = HexVal;
            }
        }

        public static int RRGGBBHexToARGB32(ReadOnlySpan<char> HexSpan)
        {
            if (Avx2.IsSupported)
            {
                return RRGGBBHexToARGB32_AVX2(HexSpan);
            }

            else
            {
                return RRGGBBHexToARGB32_Scalar(HexSpan);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int RRGGBBHexToARGB32_AVX2(ReadOnlySpan<char> HexSpan)
        {
            ref var FirstChar = ref MemoryMarshal.GetReference(HexSpan);

#if NET7_0_OR_GREATER
            var Sm0lVec = Vector128.LoadUnsafe(ref Unsafe.Subtract(ref Unsafe.As<char, short>(ref FirstChar), 2));
#else
            var Sm0lVec = Unsafe.ReadUnaligned<Vector128<short>>(ref Unsafe.As<char, byte>(ref Unsafe.Subtract(ref FirstChar, 2)));
#endif

            var IsLowerCaseVec = Avx2.CompareGreaterThan(Sm0lVec, Vector128.Create((short)'F'));

            var IsAlphabet = Avx2.CompareGreaterThan(Sm0lVec, Vector128.Create((short)'9'));

            Sm0lVec = Avx2.Subtract(Sm0lVec, Avx2.And(Vector128.Create((short)UpperToLowerCaseOffset), IsLowerCaseVec));

            Sm0lVec = Avx2.Subtract(Sm0lVec, Avx2.And(Vector128.Create((short)NumNextToFirstLetterOffset), IsAlphabet));

            Sm0lVec = Avx2.Subtract(Sm0lVec, Vector128.Create((short)StartingPoint));

            Sm0lVec = Avx2.Or(Sm0lVec, Vector128.Create(-1, -1, 0, 0, 0, 0, 0, 0));

            var Vec = Avx2.ConvertToVector256Int32(Sm0lVec);

            Vec = Avx2.ShiftLeftLogicalVariable(Vec, Vector256.Create((uint)23, 23, 20, 16, 12, 8, 4, 0));

#if NET7_0_OR_GREATER
            return Vector256.Sum(Vec);
#else
            var Upper = Vec.GetUpper();

            var Lower = Vec.GetLower();

            Upper = Avx2.Add(Upper, Lower);

            Upper = Avx2.HorizontalAdd(Upper, Upper);

            Upper = Avx2.HorizontalAdd(Upper, Upper);

            return Upper.GetElement(0);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RRGGBBHexToARGB32_Scalar(ReadOnlySpan<char> HexSpan)
        {
            ref var FirstChar = ref MemoryMarshal.GetReference(HexSpan);

            var TablePtr = HexTable;

            var _0 = TablePtr[FirstChar] << 20;

            var _1 = TablePtr[Unsafe.Add(ref FirstChar, 1)] << 16;

            var _2 = TablePtr[Unsafe.Add(ref FirstChar, 2)] << 12;

            var _3 = TablePtr[Unsafe.Add(ref FirstChar, 3)] << 8;

            var _4 = TablePtr[Unsafe.Add(ref FirstChar, 4)] << 4;

            var _5 = TablePtr[Unsafe.Add(ref FirstChar, 5)];

            return -16777216 | _0 | _1 | _2 | _3 | _4 | _5;
        }

        public static int RRGGBBAAHexToARGB32(ReadOnlySpan<char> HexSpan)
        {
            if (Avx2.IsSupported)
            {
                return RRGGBBAAHexToARGB32_AVX2(HexSpan);
            }

            else
            {
                return RRGGBBAAHexToARGB32_Scalar(HexSpan);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int RRGGBBAAHexToARGB32_AVX2(ReadOnlySpan<char> HexSpan)
        {
            ref var FirstChar = ref MemoryMarshal.GetReference(HexSpan);

#if NET7_0_OR_GREATER
            var Sm0lVec = Vector128.LoadUnsafe(ref Unsafe.As<char, short>(ref FirstChar));
#else
            var Sm0lVec = Unsafe.ReadUnaligned<Vector128<short>>(ref Unsafe.As<char, byte>(ref FirstChar));
#endif

            var IsLowerCaseVec = Avx2.CompareGreaterThan(Sm0lVec, Vector128.Create((short)'F'));

            var IsAlphabet = Avx2.CompareGreaterThan(Sm0lVec, Vector128.Create((short)'9'));

            Sm0lVec = Avx2.Subtract(Sm0lVec, Avx2.And(Vector128.Create((short)UpperToLowerCaseOffset), IsLowerCaseVec));

            Sm0lVec = Avx2.Subtract(Sm0lVec, Avx2.And(Vector128.Create((short)NumNextToFirstLetterOffset), IsAlphabet));

            Sm0lVec = Avx2.Subtract(Sm0lVec, Vector128.Create((short)StartingPoint));

            var Vec = Avx2.ConvertToVector256Int32(Sm0lVec);

            Vec = Avx2.ShiftLeftLogicalVariable(Vec, Vector256.Create((uint)20, 16, 12, 8, 4, 0, 28, 24));

#if NET7_0_OR_GREATER
            return Vector256.Sum(Vec);
#else
            var Upper = Vec.GetUpper();

            var Lower = Vec.GetLower();

            Upper = Avx2.Add(Upper, Lower);

            Upper = Avx2.HorizontalAdd(Upper, Upper);

            Upper = Avx2.HorizontalAdd(Upper, Upper);

            return Upper.GetElement(0);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RRGGBBAAHexToARGB32_Scalar(ReadOnlySpan<char> HexSpan)
        {
            ref var FirstChar = ref MemoryMarshal.GetReference(HexSpan);

            var _0 = HexTable[FirstChar] << 20;

            var _1 = HexTable[Unsafe.Add(ref FirstChar, 1)] << 16;

            var _2 = HexTable[Unsafe.Add(ref FirstChar, 2)] << 12;

            var _3 = HexTable[Unsafe.Add(ref FirstChar, 3)] << 8;

            var _4 = HexTable[Unsafe.Add(ref FirstChar, 4)] << 4;

            var _5 = HexTable[Unsafe.Add(ref FirstChar, 5)];

            var _6 = HexTable[Unsafe.Add(ref FirstChar, 6)] << 28;

            var _7 = HexTable[Unsafe.Add(ref FirstChar, 7)] << 24;

            return _0 | _1 | _2 | _3 | _4 | _5 | _6 | _7;
        }
    }
}
