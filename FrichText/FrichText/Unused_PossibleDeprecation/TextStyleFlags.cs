using System;
using System.Collections.Generic;
using System.Text;

namespace Pha3z.FrichText
{
    public enum TextStyleFlags : byte
    {
        None = 0,
        Bold = 1,
        Italic = 1 << 1,
        Underline = 1 << 2,
        Strikeout = 1 << 3,
        Strikethrough = 1 << 4,
        Superscript = 1 << 5,
        Subscript = 1 << 6,
        ParagraphStart = 1 << 7,

    }
}
