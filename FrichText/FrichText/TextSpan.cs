using System;

namespace Pha3z.FrichText
{
    public struct TextSpan
    {
        /// <summary>
        /// Index to a font in an array of fonts associated with the whole text.
        /// </summary>
        public byte FontIndex;

        public byte LineHeight;
        public byte FontSize;

        /// <summary>
        /// Unit: Thousandths of an 'em'.  A value of 1 means 1/1000 em (relative to the font size).
        /// </summary>
        public byte Kerning;

        /// <summary>
        /// OpeningTokenStart + OpeningTokenLength maps to the closing bracket position of opening span token in original text
        /// </summary>
        public byte OpeningTokenLength;

        public byte ClosingTokenLength;

        public TextStyleFlags StyleFlags;

        /// <summary>
        /// The opening bracket position of the opening span token in origina ltext
        /// </summary>
        public short OpeningTokenStart;

        public short ClosingTokenStart;        

        public short InnerTextFirstCharIdx() => (short)(OpeningTokenStart + OpeningTokenLength + 1);
        public short InnerTextLastCharIdx() => (short)(ClosingTokenStart - 1);
    }
}
