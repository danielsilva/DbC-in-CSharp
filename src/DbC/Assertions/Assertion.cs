using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbC
{
    public class Assertion : IAssertion
    {
        private bool isValid;
        private string message;

        protected string Message 
        {
            get 
            { 
                return message ?? (message = "DbC rule violated."); 
            }
        }

        protected Assertion() { }

        protected Assertion(bool condition) 
        {
            this.isValid = condition;
        }

        public static Assertion That(bool condition)
        {
            return new Assertion(condition);
        }

        public Assertion WhenNot(string errorMessage) 
        {
            this.message = errorMessage;
            return this;
        }

        public virtual void DoValidation(List<string> messages)
        {
            if (!isValid)
            {
                messages.Add(Message);
            }
        }

        public void Validate()
        {
            var messages = new List<string>();
            DoValidation(messages);

            if (messages.Count == 0)
            {
                return;
            }
            else
            {
                var errorMessagesBuilder = new StringBuilder();
                foreach (string m in messages)
                {
                    string breakLine = errorMessagesBuilder.Length == 0 ? "" : Environment.NewLine;
                    errorMessagesBuilder.Append(breakLine + m);
                }
                throw new DbCException(errorMessagesBuilder.ToString());
            }
        }

        public static Assertion operator &(Assertion one, Assertion other)
        {
            return new AndAssertion(one, other);
        }

        public static bool operator true(Assertion a)
        {
            a.Validate();
            return false;
        }

        public static bool operator false(Assertion a)
        {
            a.Validate();
            return false;
        }
    }
}
