using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public class ProcedureDecl : FunctionDeclBase
    {
        public ProcedureDecl(string name) 
            : base(name, "void")
        {

        }
    }
}
