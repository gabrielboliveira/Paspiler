using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public class ErrorExpression : Expression
    {
        private Error error;

        public ErrorExpression(Error error)
        {
            // TODO: Complete member initialization
            this.error = error;
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
