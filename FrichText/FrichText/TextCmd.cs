using System;

namespace Pha3z.FrichText
{
    public enum TextCmdKind : short
    {
        Text = 0,
        Bold = 1,
        Italic = 1 << 1,
        Underline = 1 << 2,
        Strikeout = 1 << 3,
        Strikethrough = 1 << 4,
        Superscript = 1 << 5,
        Subscript = 1 << 6,
        ParagraphStart = 1 << 7,
        FontIndex = 1 << 8,
        Kerning = 1 << 9,
        LineHeight = 1 << 10,
        FontSize = 1 << 11,
    }

    public struct TextCmd
    {
        public TextCmdKind Kind;

        /// <summary>
        /// If the command requires a numeric value as its parameter, this is the value.
        /// </summary>
        public short CmdValue;

        /// <summary>
        /// If the command is a text command, this is the string.
        /// </summary>
        public string Text;
    }
}
