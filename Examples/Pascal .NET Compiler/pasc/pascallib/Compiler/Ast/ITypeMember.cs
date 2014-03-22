using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace pascallib.Compiler.Ast
{
    public interface ITypeMember : IAstNode
    {
        string Name
        {
            get;
        }

        void Prepare(TypeBuilder typeBuilder, SymbolTable symbolTable);

        void Generate();
    }
}
