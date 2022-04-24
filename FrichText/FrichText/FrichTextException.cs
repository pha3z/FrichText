using System;
using System.Collections.Generic;
using System.Text;

namespace Pha3z.FrichText
{
    /// <summary>
    /// This exception indicates the layout state is invalid.<br/>
    /// This is usually caused by an incorrectly built layout.
    /// </summary>
    public class FrichTextException : Exception
    {
        public FrichTextException(string message)
            : base(message)
        {
        }

        public FrichTextException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
