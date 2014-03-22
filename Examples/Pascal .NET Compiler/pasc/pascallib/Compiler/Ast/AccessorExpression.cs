using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public sealed class AccessorExpression : Expression
    {
        public AccessorExpression(Expression operand, List<Expression> arguments)
        {
            this.Operand = operand;
            this.Arguments = arguments;
        }

        public Expression Operand
        {
            get;
            private set;
        }

        public List<Expression> Arguments
        {
            get;
            private set;
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
