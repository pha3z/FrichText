using System;
using System.Collections.Generic;
using System.Text;

namespace Pha3z.FrichText
{
    public struct NimbleFrichTextBuilder
    {
        RefList<FrichTextCmd> _cmds;
        
        public NimbleFrichTextBuilder(int initialCmdCapacity)
        {
            _cmds = new RefList<FrichTextCmd>(initialCmdCapacity);
        }

        public NimbleFrichText Build() => new NimbleFrichText(_cmds.Items, _cmds.Count);

        public NimbleFrichTextBuilder Text(string text)
        {
            ref var c = ref _cmds.AddByRef();
            c.Kind = FrichTextCmdKind.Text;
            c.Text = text;
            return this;
        }

        public NimbleFrichTextBuilder Left()
        {
            ref var c = ref _cmds.AddByRef();
            c.Kind = FrichTextCmdKind.Align;
            c.Value = (int)TextAlign.LEFT; 
            return this;
        }

        public NimbleFrichTextBuilder Right()
        {
            ref var c = ref _cmds.AddByRef();
            c.Kind = FrichTextCmdKind.Align;
            c.Value = (int)TextAlign.RIGHT;
            return this;
        }

        public NimbleFrichTextBuilder Center()
        {
            ref var c = ref _cmds.AddByRef();
            c.Kind = FrichTextCmdKind.Align;
            c.Value = (int)TextAlign.CENTER;
            return this;
        }

        public NimbleFrichTextBuilder Justify()
        {
            ref var c = ref _cmds.AddByRef();
            c.Kind = FrichTextCmdKind.Align;
            c.Value = (int)TextAlign.JUSTIFY;
            return this;
        }

        public NimbleFrichTextBuilder Color(string hex)
        {
            if (hex[0] == '#')
                return Color(IntParser.ParseHex(hex, 1, 6));
            else
                return Color(IntParser.ParseHex(hex, 0, 6));
        }

        public NimbleFrichTextBuilder Color(byte r, byte g, byte b, byte a)
        {
            int clr = r << 24;
            clr += g << 16;
            clr += g << 8;
            clr += a;
            return Color(clr);
        }

        public NimbleFrichTextBuilder Color(int color)
        {
            ref var c = ref _cmds.AddByRef();
            c.Kind = FrichTextCmdKind.Color;
            c.Value = color;
            return this;
        }

        public NimbleFrichTextBuilder FontSize(byte size)
        {
            ref var c = ref _cmds.AddByRef();
            c.Kind = FrichTextCmdKind.FontSize;
            c.Value = size;
            return this;
        }

        public NimbleFrichTextBuilder LineHeight(byte size)
        {
            ref var c = ref _cmds.AddByRef();
            c.Kind = FrichTextCmdKind.LineHeight;
            c.Value = size;
            return this;
        }

        public NimbleFrichTextBuilder LetterSpacing(byte size)
        {
            ref var c = ref _cmds.AddByRef();
            c.Kind = FrichTextCmdKind.LetterSpacing;
            c.Value = size;
            return this;
        }

        public NimbleFrichTextBuilder Bold(bool turnon = true)
        {
            ref var c = ref _cmds.AddByRef();
            c.Kind = FrichTextCmdKind.Bold;
            if (!turnon)
                c.Kind |= FrichTextCmdKind.TURN_STYLE_OFF;
            return this;
        }

        public NimbleFrichTextBuilder Italic(bool turnon = true)
        {
            ref var c = ref _cmds.AddByRef();
            c.Kind = FrichTextCmdKind.Italic;
            if (!turnon)
                c.Kind |= FrichTextCmdKind.TURN_STYLE_OFF;
            return this;
        }

        public NimbleFrichTextBuilder Underline(bool turnon = true)
        {
            ref var c = ref _cmds.AddByRef();
            c.Kind = FrichTextCmdKind.Underline;
            if (!turnon)
                c.Kind |= FrichTextCmdKind.TURN_STYLE_OFF;
            return this;
        }

        public NimbleFrichTextBuilder Strikethrough(bool turnon = true)
        {
            ref var c = ref _cmds.AddByRef();
            c.Kind = FrichTextCmdKind.Strikethrough;
            if (!turnon)
                c.Kind |= FrichTextCmdKind.TURN_STYLE_OFF;
            return this;
        }

        public NimbleFrichTextBuilder Paragraph()
        {
            ref var c = ref _cmds.AddByRef();
            c.Kind = FrichTextCmdKind.ParagraphStart;
            return this;
        }

        public NimbleFrichTextBuilder FontIndex(int index)
        {
            ref var c = ref _cmds.AddByRef();
            c.Kind = FrichTextCmdKind.FontIndex;
            c.Value = index;
            return this;
        }
    }
}
