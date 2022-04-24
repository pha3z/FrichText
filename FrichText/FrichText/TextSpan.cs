using System;

namespace Pha3z.FrichText
{
    public struct TextSpan
    {
        public TextStyleFlags StyleFlags;

        /// <summary>
        /// Index to a font in an array of fonts associated with the whole text.
        /// </summary>
        byte FontIndex;

        public byte LineHeight;
        public byte FontSize;

        /// <summary>
        /// Unit: Thousandths of an 'em'.  A value of 1 means 1/1000 em (relative to the font size).
        /// </summary>
        public byte Kerning;

        /// <summary>
        /// The opening bracket position in original text
        /// </summary>
        public int Start;

        /// <summary>
        /// STart + Length maps to the closing bracket position in original text
        /// </summary>
        public int Length;
   
    }
}
