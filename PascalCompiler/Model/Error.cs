using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler.Model
{
    class Error
    {
        public Error(string message, Token token) 
        {
            this.message = message;
            this.token = token;
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
}
