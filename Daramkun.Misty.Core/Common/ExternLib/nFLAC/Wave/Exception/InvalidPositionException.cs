using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Nflac.Wave.Exception
{
    class InvalidPositionException : System.Exception
    {
        public InvalidPositionException(string str)
            : base(str)
        {

        }

        public InvalidPositionException()
            : base()
        {

        }
    }
}
