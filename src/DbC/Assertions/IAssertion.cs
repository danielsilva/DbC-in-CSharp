using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbC
{
    public interface IAssertion
    {
        //string Message { get; }

        void DoValidation(List<string> messages);
    }
}
