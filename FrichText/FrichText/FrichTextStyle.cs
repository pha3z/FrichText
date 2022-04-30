using System;
using System.Collections.Generic;
using System.Text;

namespace Pha3z.FrichText
{
    public struct FrichTextStyle
    {
        /// <summary>
        /// One-based index to a font in an array of fonts associated with the whole text.<br/>Zero means value not specified.
        /// </summary>
        public byte FontIndex;

        /// <summary>
        /// Zero means value not specified.
        /// </summary>
        public byte LineHeight;

        /// <summary>
        /// Zero means value not specified.
        /// </summary>
        public byte FontSize;

        /// <summary>
        /// Unit: Thousandths of an 'em'.  A value of 1 means 1/1000 em (relative to the font size).<br/>Zero means value not specified.
        /// </summary>
        public byte LetterSpacing;

        public TextStyleFlags StyleFlags;
    }
}
