using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public class BinaryExpression : Expression
    {
        public BinaryExpression(Operator op, Expression left, Expression right)
        {
            this.Operator = op;
            this.Left = left;
            this.Right = right;
        }

        public Operator Operator
        {
            get;
            protected set;
        }

        public Expression Left
        {
            get;
            protected set;
        }

        public Expression Right
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
