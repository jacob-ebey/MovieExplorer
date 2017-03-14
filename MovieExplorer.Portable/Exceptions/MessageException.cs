using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieExplorer.Exceptions
{
    class MessageException : Exception
    {
        public MessageException() 
            : this("Something went seriously wrong... We have our best engineers on it :D")
        {
        }

        public MessageException(string userMessage) 
            : this(userMessage, userMessage)
        {
        }

        public MessageException(string userMessage, string exceptionMessage)
            : base(exceptionMessage)
        {
            UserMessage = userMessage ?? exceptionMessage;
        }

        public string UserMessage { get; }
    }
}
