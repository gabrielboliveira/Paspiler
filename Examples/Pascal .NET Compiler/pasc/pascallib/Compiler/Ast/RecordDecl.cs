using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public sealed class RecordDecl : TypeDeclBase
    {
        public RecordDecl(string name)
            : base(name)
        {

        }

        public override void Prepare(System.Reflection.Emit.AssemblyBuilder assemblyBuilder, SymbolTable symbolTable)
        {
            throw new NotImplementedException();
        }

        public override void Generate()
        {
            throw new NotImplementedException();
        }
    }
}
