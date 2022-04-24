using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Pha3z.FrichText
{
    public class FrichText
    {
        public TextSpan[] TextSpans { get; set; }
        public string Text { get; set; }


        public static FrichText ParseFrichText(string frichText, int start = 0, int length = 0 )
        {
            //TODO: Change this to a local implementation of FastList (copy into this library and namespace)
            List<TextSpan> spans = new List<TextSpan>();
            string s = frichText;
            for (int i = 0; i < length - 1; i++)
            {
                if(s[i] == '[' && s[i] != '[')
                {
                    i = ParseSpanToken(frichText, i, spans);
                }
            }
        }

        public static int ParseSpanToken(string frichText, int openingBracketPos, List<TextSpan> spans)
        {
            string txt = frichText;
            int i = openingBracketPos + 1;
            TextSpan s = spans.Add();

            while(i < txt.Length - 1)
            {
                if (txt[i] == ']')
                    return i +1;
                else if (txt[i] == 'b')
                    s.StyleFlags |= TextStyleFlags.Bold;
                else if (txt[i] == 'i')
                    s.StyleFlags |= TextStyleFlags.Italic;
                else if (txt[i] == 'u')
                    s.StyleFlags |= TextStyleFlags.Underline;
                else if (txt[i] == 'p')
                    s.StyleFlags |= TextStyleFlags.ParagraphStart;
                else if (txt[i] == 'k')
                {
                    int eon = FindEndOfNumber(txt, i + 2);
                    s.Kerning = (byte)IntParser.ParseInt(txt, i, eon);
                    i = eon;
                }
                else if (txt[i] == 'l' && txt[i + 1] == 'h')
                {
                    int eon = FindEndOfNumber(txt, i + 2);
                    s.LineHeight = (byte)IntParser.ParseInt(txt, i, eon);
                    i = eon;
                }
                else if (Char.IsDigit(txt[i]))
                {
                    //A number preceeded by a space or appearing immediately at start of span marker should be intrepretted as font size.
                    int eon = FindEndOfNumber(txt, i);
                    s.FontSize = (byte)IntParser.ParseInt(txt, i, eon);
                    i = eon;
                }

                i++;
                if (txt[i] != ' ' && txt[i] != ']')
                    throw new FrichTextException("Encountered unexpected character after text: ");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FindEndOfNumber(string txt, int numberPos)
        {
            //Colons are optional. Skip if present.
            if(txt[numberPos] == ':')
                numberPos++;

            if (!Char.IsDigit(txt[numberPos]))
                throw new FrichTextException("Expected a number value for after the following text: ");

            int valueTerminator = numberPos + 1;
            while (valueTerminator < valueTerminator + 2)
            {
                if (!Char.IsDigit(txt[valueTerminator]))
                    break;
                valueTerminator++;
            }

            if (Char.IsDigit(txt[valueTerminator]))
                throw new FrichTextException($"Maximum value is 999. Encountered invalid value in the following text: ");

            return valueTerminator - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsMatch(string needle, string haystack, int position)
        {
            for(int i = 0; i < needle.Length; i++)
            {
                if (needle[i] != haystack[position + i])
                    return false;
            }

            return true;
        }
    }
}
