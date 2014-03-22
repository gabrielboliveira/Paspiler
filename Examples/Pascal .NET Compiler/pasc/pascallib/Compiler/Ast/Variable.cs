using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public class Variable
    {
        public Variable(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string Name
        {
            get;
            protected set;
        }

        public string Type
        {
            get;
            protected set;
        }
    }
}
