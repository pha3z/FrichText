using System;

namespace Pha3z.FrichText
{
    [Flags]
    public enum FrichTextCmdKind : int
    {
        SaveStyleState = 0,
        RestorePreviousState = 1,
        /// <summary>
        /// Cmds that invoke a simple on/off style such as "Bold" or "Italic" will turn style ON by default. If TURN_STYLE_OFF is on, then the style is turned off.
        /// </summary>
        TURN_STYLE_OFF = 1 << 1,
        Text = 1 << 2,
        Bold = 1 << 3,
        Italic = 1 << 4,
        Underline = 1 << 5,
        Strikeout = 1 << 6,
        Strikethrough = 1 << 7,
        Superscript = 1 << 8,
        Subscript = 1 << 9,
        ParagraphStart = 1 << 10,
        FontIndex = 1 << 11,
        LetterSpacing = 1 << 12,
        LineHeight = 1 << 13,
        FontSize = 1 << 14,

        /// <summary>Color Value is stored as a signed integer encoding ARGB </summary>
        Color = 1 << 15,
        Align = 1 << 16,
    }

    [Flags]
    public enum TextAlign : byte
    {
        LEFT = 1,
        RIGHT = 2,
        CENTER = 4,
        JUSTIFY = 8,
    }

    public struct FrichTextCmd
    {
        public FrichTextCmdKind Kind;

        /// <summary>
        /// If the command requires a numeric value as its parameter, this is the value.<br/><br/>
        /// Bit values for Alignment are defined in TextAlign enum
        /// </summary>
        public int Value;

        /// <summary>
        /// If the command is a text command, this is the string.
        /// </summary>
        public string Text;
    }
}
