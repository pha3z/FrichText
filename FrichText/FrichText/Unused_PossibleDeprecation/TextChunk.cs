using System;

namespace Pha3z.FrichText
{
    public struct TextChunk
    {
        /// <summary>
        /// Index to a font in an array of fonts assigned to the whole text.<br/>255 means value not specified.
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
        public byte Kerning;

        public TextStyleFlags StyleFlags;

        public string Text;
    }
}
