using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public class IntegerNumberExpression : Expression
    {
        private int p;

        public IntegerNumberExpression(int p)
        {
            // TODO: Complete member initialization
            this.p = p;
        }

        public override void Prepare(System.Reflection.Emit.ILGenerator generator, SymbolTable symbolTable)
        {
            throw new NotImplementedException();
        }
        
        public override void Generate()
        {
            throw new NotImplementedException();
        }
    }
}
