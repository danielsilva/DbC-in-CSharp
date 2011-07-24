using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbC
{
    public class AndAssertion : Assertion
    {
        private IAssertion one;
        private IAssertion other;

        public AndAssertion(Assertion one, Assertion other)
        {
            this.one = one;
            this.other = other;
        }

        public override void DoValidation(List<string> messages)
        {
            one.DoValidation(messages);
            other.DoValidation(messages);
        }
    }
}
