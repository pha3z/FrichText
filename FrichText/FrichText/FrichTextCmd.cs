using System;

namespace Pha3z.FrichText
{
    [Flags]
    public enum FrichTextCmdKind : short
    {
        SaveStyleState = 0,
        RestorePreviousState = 1,
        Text = 1 << 1,
        Weight = 1 << 2,
        Italic = 1 << 3,
        Underline = 1 << 4,
        Strikethrough = 1 << 5,
        Superscript = 1 << 6,
        Subscript = 1 << 7,
        ParagraphStart = 1 << 8,
        FontFamilyIndex = 1 << 9,
        LetterSpacing = 1 << 10,
        LineHeight = 1 << 11,
        FontSize = 1 << 12,

        /// <summary>Color Value is stored as a signed integer encoding ARGB </summary>
        Color = 1 << 13,
        Align = 1 << 14,
    }

    [Flags]
    public enum TextAlign : byte
    {
        LEFT = 0,
        RIGHT = 1,
        CENTER = 1 << 1,
        JUSTIFY = 1 << 2,
        TOP = 1 << 3,
        MIDDLE = 1 << 4,
        BOTTOM = 1 << 5,

        TOP_LEFT = TOP | LEFT,
        TOP_CENTER = TOP | CENTER,
        TOP_RIGHT = TOP | RIGHT,
        MIDDLE_LEFT = MIDDLE | LEFT,
        MIDDLE_CENTER = MIDDLE | CENTER,
        MIDDLE_RIGHT = MIDDLE | RIGHT,
        BOTTOM_LEFT = BOTTOM | LEFT,
        BOTTOM_CENTER = BOTTOM | CENTER,
        BOTTOM_RIGHT = BOTTOM | RIGHT,
    }

    public struct FrichTextCmd
    {
        public FrichTextCmdKind Kind;

        /// <summary>
        /// If the command requires a numeric value as its parameter, this is the value.<br/><br/>
        /// Bit values for Alignment are defined in TextAlign enum<br/>
        /// For Text Kind, Value is an index into a seperate string[] acting as a Text buffer.<br/>
        /// For Kind Weight, Value is 1 to 9
        /// </summary>
        public int Value;

        public FrichTextCmd(FrichTextCmdKind kind)
        {
            Kind = kind;
            Value = 0;
        }

        public FrichTextCmd(FrichTextCmdKind kind, int value)
        {
            Kind=kind;
            Value = value;
        }
    }
}
