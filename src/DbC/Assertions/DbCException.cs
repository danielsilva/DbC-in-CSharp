using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbC
{
    public class DbCException : Exception
    {
        public DbCException() : base() { }

        public DbCException(string message) : base(message) { }

        public DbCException(string message, Exception innerException) : base(message, innerException) { }
    }
}
