using System;
namespace pascallib.Compiler.Ast
{
    public interface IDataStructure : IAstNode
    {
        void Generate();
        void Prepare(System.Reflection.Emit.AssemblyBuilder assemblyBuilder, SymbolTable symbolTable);
    }
}
