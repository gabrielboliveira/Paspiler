using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public class NegateExpression : Expression
    {
        public NegateExpression(Ast.Expression expression)
        {
            this.Expression = expression;
        }
        
        public Expression Expression
        {
            get;
            protected set;
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
