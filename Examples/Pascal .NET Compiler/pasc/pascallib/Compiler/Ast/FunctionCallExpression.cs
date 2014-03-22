using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public sealed class FunctionCallExpression : Expression
    {
        public FunctionCallExpression(Expression name, List<Expression> arguments)
        {
            this.Name = name;
            this.Arguments = arguments;
        }

        public Expression Name
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
