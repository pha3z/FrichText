using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Pha3z.FrichText
{
    /// <summary>
    /// Contributed by TrumpMcDonaldz. 2022-4-23
    /// </summary>
    public static unsafe class IntParser
    {

        /// <summary>
        /// If you attempt to parse text that contains non-numeric characters, you will get an invalid result.
        /// There is no check for validity or even range. Use this carefully!!
        /// </summary>
        /// <param name="text"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int ParseInt(this string text, int start, int count)
        {
            fixed (char* ptr = text)
            {
                return ParseIntInternal(ptr, start, count);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ParseIntInternal(char* start, int startIdx, int count)
        {
            var Current = start + startIdx;

            var LastOffsetByOne = Current + count;

            //https://cdn.discordapp.com/attachments/956964976466231316/967596154491633674/ASCII_Table.png
            const int OffsetToActualInt = 48;

            var Accumulator = 0;

            for (; Current != LastOffsetByOne; Current++)
            {
                Accumulator *= 10;

                var ActualDigit = unchecked((int)Current - OffsetToActualInt);

                Accumulator += ActualDigit;
            }

            return Accumulator;
        }

    }
}
