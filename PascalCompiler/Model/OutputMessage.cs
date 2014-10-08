using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler.Model
{
    class OutputMessage
    {
        public OutputMessage(string message, OutputTypeEnum type)
        {
            this.message = message;
            this.token = null;
            this.messageType = type;
        }

        public OutputMessage(string message, OutputTypeEnum type, Token token)
        {
            this.message = message;
            this.token = token;
            this.messageType = type;
        }

        private OutputTypeEnum messageType;

        public OutputTypeEnum MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }

        private string message = "";

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        private Token token = null;

        public Token Token
        {
            get { return token; }
            private set { token = value; }
        }
    }

    public enum OutputTypeEnum
    {
        Message,
        Error,
        Warning
    }
}
