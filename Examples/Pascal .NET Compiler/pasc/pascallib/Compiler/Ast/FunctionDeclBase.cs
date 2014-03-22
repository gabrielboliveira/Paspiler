using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public abstract class FunctionDeclBase
    {
        public FunctionDeclBase(string name, string returnType)
        {
            this.Name = name;
            this.ReturnType = returnType;

            this.Parameters = new List<FunctionParam>();
            this.Body = new List<Statement>();
        }

        public string Name
        {
            get;
            protected set;
        }

        public string ReturnType
        {
            get;
            protected set;
        }

        public List<FunctionParam> Parameters
        {
            get;
            protected set;
        }

        public List<Statement> Body
        {
            get;
            protected set;
        }
    }
}
