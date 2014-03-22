using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace pascallib.Compiler.Ast
{
    public interface ICodeStructure : IAstNode
    {
        void Prepare(ILGenerator generator, SymbolTable symbolTable);
        void Generate();
    }
}
