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
        public FrichTextCmd[] Cmds { get; private set; }

        /// <summary>
        /// This array may contain trailing unused space. This should be non-issue because it is not intended for iteration. Its merely a look-up table to locate text used in FrichTextCmds of Kind Text
        /// </summary>
        public string[] TextMap { get; private set; }

        public FrichTextCmdBuffer(FrichTextCmd[] cmds, ushort cmdCnt, string[] txtForTextCmds)
        {
            Cmds = cmds;
            CmdCount = cmdCnt;
            TextMap = txtForTextCmds;
        }
    }
}
