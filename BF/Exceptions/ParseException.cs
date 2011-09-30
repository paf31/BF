using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BF.Exceptions
{
    public class ParseException : Exception
    {
        public ParseException()
            : base()
        {
        }

        public ParseException(string message)
            : base(message)
        {
        }
    }
}
