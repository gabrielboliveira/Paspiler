using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public sealed class FunctionParam
    {
        public FunctionParam(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Type
        {
            get;
            private set;
        }
    }
}
