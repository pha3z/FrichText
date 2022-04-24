using System;
using System.Collections.Generic;
using System.Text;

namespace Pha3z.FrichText
{
    internal static class Ex
    {
        public static void Throw(string msg)
        {
            throw new FrichTextException(msg);
        }
    }
}
