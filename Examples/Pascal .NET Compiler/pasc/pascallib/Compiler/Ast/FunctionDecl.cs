using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public class FunctionDecl : FunctionDeclBase
    {
        public FunctionDecl(string name, string returnType) 
            : base(name, returnType)
        {

        }
    }
}
