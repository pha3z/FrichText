using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pha3z.FrichText
{
    public struct FrichTextCmdBuffer
    {
        public ushort CmdCount { get; private set; }

        /// <summary>
        /// NOTE: This array may contain trailing unused space. Use the LastCmdIdx property to find out the index to the last command in the array.
        /// </summary>
        public FrichTextCmd[] TextCmds { get; private set; }

        public string[] TextForTextCmds { get; private set; }

        public FrichTextCmdBuffer(FrichTextCmd[] cmds, ushort cmdCnt, string[] txtForTextCmds)
        {
            TextCmds = cmds;
            CmdCount = cmdCnt;
            TextForTextCmds = txtForTextCmds;
        }
    }
}
