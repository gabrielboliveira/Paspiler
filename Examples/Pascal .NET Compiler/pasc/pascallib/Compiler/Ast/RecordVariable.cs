using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public sealed class RecordVariable : Variable, ITypeMember
    {
        public RecordVariable(string name, string type) 
            : base(name, type)
        {

        }

        public void Prepare(System.Reflection.Emit.TypeBuilder typeBuilder, SymbolTable symbolTable)
        {
            throw new NotImplementedException();
        }

        public void Generate()
        {
            throw new NotImplementedException();
        }

        public void SetStartLoc(SourceLocation location)
        {
            throw new NotImplementedException();
        }

        public void SetEndLoc(SourceLocation location)
        {
            throw new NotImplementedException();
        }
    }
}
