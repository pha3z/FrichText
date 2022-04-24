using System;

namespace Pha3z.FrichText
{
    public enum FrichTextCmdKind : short
    {
        SaveStyleState = 0,
        RestorePreviousState = 1,
        Text = 1 << 1,
        Bold = 1 << 2,
        Italic = 1 << 3,
        Underline = 1 << 4,
        Strikeout = 1 << 5,
        Strikethrough = 1 << 6,
        Superscript = 1 << 7,
        Subscript = 1 << 8,
        ParagraphStart = 1 << 9,
        FontIndex = 1 << 10,
        LetterSpacing = 1 << 11,
        LineHeight = 1 << 12,
        FontSize = 1 << 13,

        /// <summary>Color Value is stored as a signed integer representing RGBA </summary>
        Color = 1 << 14,
    }

    public struct FrichTextCmd
    {
        public FrichTextCmdKind Kind;

        /// <summary>
        /// If the command requires a numeric value as its parameter, this is the value.
        /// </summary>
        public int Value;

        /// <summary>
        /// If the command is a text command, this is the string.
        /// </summary>
        public string Text;
    }
}
