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
        public FrichTextCmd[] TextCmds { get; private set; }
        public int LastCmdIdx { get; private set; }

        public static NimbleFrichText ParseFrichText(string frichText, int start = 0, int length = 0 )
        {
            RefList<FrichTextCmd> cmds = new RefList<FrichTextCmd>((frichText.Length / 256) + 4);

            DoRecursiveParse(frichText, cmds, spanIdx: 0, start, length - 1);

            return new NimbleFrichText()
            {
                TextCmds = cmds.Items,
                LastCmdIdx = cmds.Count - 1
            };
        }

        static void DoRecursiveParse(string txt, RefList<FrichTextCmd> cmds, int spanIdx, int position, int stopParsingAt)
        {
            int textSpanStart = position;
            int i = position;
            do
            {
                if (txt[i] == '[' && txt[i + 1] != '[')
                {
                    if (i > textSpanStart)
                    {
                        //Scoop up all plain text before the token and turn it into a text command
                        ref FrichTextCmd cmd = ref cmds[spanIdx];
                        cmd.Text = txt.Substring(textSpanStart, i - textSpanStart);
                    }

                    //Now process the token. Advance to first character inside token
                    i++; 

                    //Skip leading whitespace
                    while (txt[i] == ' ')
                    {
                        i++;
                        if (i == stopParsingAt)
                            Ex.Throw("Text ends with an unterminated format token.");
                    }

                    //Check if this is a closing token
                    if (txt[i] == '/')
                    {
                        ref FrichTextCmd cmd = ref cmds.AddByRef();
                        cmd.Kind = FrichTextCmdKind.RestorePreviousState;
                        i = textSpanStart = FindNextClosingBracket(txt, i) + 1;
                        spanIdx++;
                    }
                    else
                    {
                        ref FrichTextCmd cmd = ref cmds.AddByRef();
                        cmd.Kind = FrichTextCmdKind.SaveStyleState; //Anytime a format token is encountered, we consider it beginning of a new style state
                        i = textSpanStart = GenerateCmdsFromToken(txt, i, cmds);
                        DoRecursiveParse(txt, cmds, spanIdx + 1, textSpanStart, stopParsingAt);
                    }

                }
            } while (i < stopParsingAt);
        }

        /// <summary></summary>
        /// <returns>Index after the closing bracket position.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GenerateCmdsFromToken(string frichText, int openingBracketPos, RefList<FrichTextCmd> cmds)
        {
            string txt = frichText;
            int i = openingBracketPos + 1;

            while(i < txt.Length - 2 && txt[i] != ']')
            {
                if (txt[i] == ' ')
                    continue; //skip whitespace

                if (txt[i] == ']')
                    return i + 1;

                ref FrichTextCmd cmd = ref cmds.AddByRef();

                if (txt[i] == 'b')
                    cmd.Kind = FrichTextCmdKind.Bold;
                else if (txt[i] == 'i')
                    cmd.Kind = FrichTextCmdKind.Italic;
                else if (txt[i] == 'u')
                    cmd.Kind = FrichTextCmdKind.Underline;
                else if (txt[i] == 'p')
                    cmd.Kind = FrichTextCmdKind.ParagraphStart;
                else if (txt[i] == '#')
                {
                    cmd.Kind = FrichTextCmdKind.Color;
                    cmd.Value = IntParser.ParseHex(txt, i + 1, 6);
                    i = i + 6;
                }
                else if (txt[i] == 'f')
                {
                    cmd.Kind = FrichTextCmdKind.FontIndex;
                    int eon = FindEndOfNumber(txt, i + 1);
                    cmd.Value = (byte)IntParser.ParseInt(txt, i, eon);
                    i = eon;
                }
                else if (txt[i] == 't') //Tracking
                {
                    cmd.Kind = FrichTextCmdKind.LetterSpacing;
                    int eon = FindEndOfNumber(txt, i + 1);
                    cmd.Value = (byte)IntParser.ParseInt(txt, i, eon);
                    i = eon;
                }
                else if (txt[i] == 'l' && txt[i + 1] == 'h')
                {
                    cmd.Kind = FrichTextCmdKind.LineHeight;
                    int eon = FindEndOfNumber(txt, i + 2);
                    cmd.Value = (byte)IntParser.ParseInt(txt, i, eon);
                    i = eon;
                }
                else if (!Char.IsDigit(txt[i]))
                {
                    if (txt[i] != ' ' && txt[i] != ']')
                        Ex.Throw("Encountered unexpected character after text: ");
                }
                else
                {
                    //A number preceeded by a space or appearing immediately at start of span marker should be intrepretted as font size.
                    cmd.Kind = FrichTextCmdKind.FontSize;
                    int eon = FindEndOfNumber(txt, i);
                    cmd.Value = (byte)IntParser.ParseInt(txt, i, eon);
                    i = eon;
                }

                i++;
                if (txt[i] != ' ' && txt[i] != ']')
                    Ex.Throw("Encountered unexpected character after text: ");
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
                    Ex.Throw("Encountered format token without a terminating bracket, beginning with text: ");
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
                Ex.Throw("Expected a number value for after the following text: ");

            int valueTerminator = numberPos + 1;
            while (valueTerminator < valueTerminator + 2)
            {
                if (!Char.IsDigit(txt[valueTerminator]))
                    break;
                valueTerminator++;
            }

            if (Char.IsDigit(txt[valueTerminator]))
                Ex.Throw($"Maximum value is 999. Encountered invalid value in the following text: ");

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
