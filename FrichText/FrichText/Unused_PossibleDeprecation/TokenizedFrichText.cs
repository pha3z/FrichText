using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Pha3z.FrichText
{
    /// <summary>
    /// Tokenized text stores one string for the entire plain text and a collection of TextSpans to indicate formatting for spans of text.
    /// </summary>
    public class TokenizedFrichText
    {
        public TextSpan[] TextSpans { get; private set; }
        public string Text { get; private set; }

        public static TokenizedFrichText ParseFrichText(string frichText, int start = 0, int length = 0 )
        {
            RefList<TextSpan> spans = new RefList<TextSpan>((frichText.Length / 20) + 4);

            DoRecursiveParse(frichText, spans, iToken: 0, start, length);

            return new TokenizedFrichText()
            {
                Text = frichText,
                TextSpans = spans.Items
            };
        }

        static void DoRecursiveParse(string txt, RefList<TextSpan> spans, int iToken, int position, int stopParsingAt)
        {
            throw new NotImplementedException("This implementation is buggy. See the NimbleFrichText class for a correct algorithm and use it to update this one if you need this variation.");

            for (int i = position; i < stopParsingAt - 1; i++)
            {
                if (txt[i] == '[' && txt[i] != '[')
                {
                    //Check if this is a closing token
                    if (txt[i + 1] == '/' || txt[i + 2] == '/') //Allow pattern: [/] or [ /].  Just because we don't want to create grief because of one extra space.
                    {
                        ref TextSpan spanToClose = ref spans[iToken];
                        spanToClose.ClosingTokenStart = (short)i;
                        spanToClose.ClosingTokenLength = (byte)(FindNextClosingBracket(txt, i) - i + 1);
                        iToken++;
                    }
                    else
                    {
                        i = ParseSpanToken(txt, i, spans);
                        DoRecursiveParse(txt, spans, iToken + 1, i, stopParsingAt);
                    }
                }
            }
        }

        /// <summary></summary>
        /// <returns>The closing bracket position.</returns>
        static int ParseSpanToken(string frichText, int openingBracketPos, RefList<TextSpan> spans)
        {
            string txt = frichText;
            int i = openingBracketPos + 1;
            ref TextSpan s = ref spans.AddByRef();

            s.OpeningTokenStart = (short)openingBracketPos;

            while(i < txt.Length - 2 && txt[i] != ']')
            {
                if (txt[i] == ' ')
                    continue; //skip whitespace

                if (txt[i] == ']')
                    return i + 1;
                else if (txt[i] == 'b')
                    s.StyleFlags |= TextStyleFlags.Bold;
                else if (txt[i] == 'i')
                    s.StyleFlags |= TextStyleFlags.Italic;
                else if (txt[i] == 'u')
                    s.StyleFlags |= TextStyleFlags.Underline;
                else if (txt[i] == 'p')
                    s.StyleFlags |= TextStyleFlags.ParagraphStart;
                else if (txt[i] == 'f')
                {
                    int eon = FindEndOfNumber(txt, i + 1);
                    s.FontIndex = (byte)IntParser.ParseInt(txt, i, eon);
                    i = eon;
                }
                else if (txt[i] == 'k')
                {
                    int eon = FindEndOfNumber(txt, i + 1);
                    s.Kerning = (byte)IntParser.ParseInt(txt, i, eon);
                    i = eon;
                }
                else if (txt[i] == 'l' && txt[i + 1] == 'h')
                {
                    int eon = FindEndOfNumber(txt, i + 2);
                    s.LineHeight = (byte)IntParser.ParseInt(txt, i, eon);
                    i = eon;
                }
                else if (!Char.IsDigit(txt[i]))
                {
                    if (txt[i] != ' ' && txt[i] != ']')
                        throw new FrichTextException("Encountered unexpected character after text: ");
                }
                else
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

            s.OpeningTokenLength = (byte)(i - s.OpeningTokenStart + 1);
            return i;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int FindNextClosingBracket(string txt, int openingBracket)
        {
            int i = openingBracket + 2;
            while (txt[i] != ']')
            {
                i++;

                if (i == txt.Length)
                    throw new FrichTextException("Encountered format token without a terminating bracket, beginning with text: ");
            }

            return i;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int FindEndOfNumber(string txt, int numberPos)
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
        static bool IsMatch(string needle, string haystack, int position)
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
