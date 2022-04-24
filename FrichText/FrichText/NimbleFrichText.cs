using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Pha3z.FrichText
{
    /// <summary>
    /// Nimble text uses a storage system optimized for straight-ahead processing for drawing/rendering of text.<br/>
    /// All data is stored as a series of commands. A command can:<br/>
    /// - Set next text string
    /// - Set a font or style attribute
    /// </summary>
    public class NimbleFrichText
    {
        /// <summary>
        /// NOTE: This array may contain trailing unused space. Use the LastCmdIdx property to find out the index to the last command in the array.
        /// </summary>
        public TextCmd[] TextCmds { get; private set; }
        public int LastCmdIdx { get; private set; }

        public static NimbleFrichText ParseFrichText(string frichText, int start = 0, int length = 0 )
        {
            RefList<TextCmd> cmds = new RefList<TextCmd>((frichText.Length / 256) + 4);

            DoRecursiveParse(frichText, cmds, iToken: 0, start, length - 1);

            return new NimbleFrichText()
            {
                TextCmds = cmds.Items,
                LastCmdIdx = cmds.Count - 1
            };
        }

        static void DoRecursiveParse(string txt, RefList<TextCmd> cmds, int iToken, int position, int stopParsingAt)
        {
            int innerTextStart = -1;

            int i = position;
            do
            {
                if (txt[i] == '[' && txt[i + 1] != '[')
                {
                    int tokenStart = i;
                    i++; //advance to first character inside token

                    //Skip leading whitespace
                    while (txt[i] == ' ')
                    {
                        i++;
                        if (i == stopParsingAt)
                            throw new FrichTextException("Text ends with an improperly terminated format token.");
                    }

                    //Check if this is a closing token
                    if (txt[i] == '/')
                    {
                        if (innerTextStart == -1)
                            throw new FrichTextException("Encountered a closing format token ([/]) for which there is no matched preceeding opener token.");

                        ref TextCmd cmd = ref cmds[iToken];
                        cmd.Text = txt.Substring(innerTextStart, tokenStart - innerTextStart);
                        innerTextStart = -1;
                        i = FindNextClosingBracket(txt, i) + 1;
                        iToken++;
                    }
                    else
                    {
                        ref TextCmd cmd = ref cmds.AddByRef();
                        i = innerTextStart = ParseSpanToken(txt, i, ref cmd);

                        DoRecursiveParse(txt, cmds, iToken + 1, innerTextStart, stopParsingAt);
                    }

                }
            } while (i < stopParsingAt);
        }

        /// <summary></summary>
        /// <returns>Index after the closing bracket position.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int ParseSpanToken(string frichText, int openingBracketPos, ref TextCmd cmd)
        {
            string txt = frichText;
            int i = openingBracketPos + 1;

            while(i < txt.Length - 2 && txt[i] != ']')
            {
                if (txt[i] == ' ')
                    continue; //skip whitespace

                if (txt[i] == ']')
                    return i + 1;

                if (txt[i] == 'b')
                    cmd.Kind = TextCmdKind.Bold;
                else if (txt[i] == 'i')
                    cmd.Kind = TextCmdKind.Italic;
                else if (txt[i] == 'u')
                    cmd.Kind = TextCmdKind.Underline;
                else if (txt[i] == 'p')
                    cmd.Kind = TextCmdKind.ParagraphStart;
                else if (txt[i] == 'f')
                {
                    cmd.Kind = TextCmdKind.FontIndex;
                    int eon = FindEndOfNumber(txt, i + 1);
                    cmd.CmdValue = (byte)IntParser.ParseInt(txt, i, eon);
                    i = eon;
                }
                else if (txt[i] == 'k')
                {
                    cmd.Kind = TextCmdKind.Kerning;
                    int eon = FindEndOfNumber(txt, i + 1);
                    cmd.CmdValue = (byte)IntParser.ParseInt(txt, i, eon);
                    i = eon;
                }
                else if (txt[i] == 'l' && txt[i + 1] == 'h')
                {
                    cmd.Kind = TextCmdKind.LineHeight;
                    int eon = FindEndOfNumber(txt, i + 2);
                    cmd.CmdValue = (byte)IntParser.ParseInt(txt, i, eon);
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
                    cmd.Kind = TextCmdKind.FontSize;
                    int eon = FindEndOfNumber(txt, i);
                    cmd.CmdValue = (byte)IntParser.ParseInt(txt, i, eon);
                    i = eon;
                }

                i++;
                if (txt[i] != ' ' && txt[i] != ']')
                    throw new FrichTextException("Encountered unexpected character after text: ");
            }

            return i + 1;
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
