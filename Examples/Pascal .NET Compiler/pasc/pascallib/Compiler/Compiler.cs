using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pascallib.Compiler.Ast;

namespace pascallib.Compiler
{
    public class Compiler
    {
        public Compiler(Parser parser)
        {
            throw new System.NotImplementedException();
        }

        public Parser Parser
        {
            get
            {
                throw new System.NotImplementedException();
            }
            protected set
            {
            }
        }

        public List<Error> Errors
        {
            get
            {
                throw new System.NotImplementedException();
            }
            protected set
            {
            }
        }

        public bool Compile(string path)
        {
            throw new System.NotImplementedException();
        }

        private BinaryExpressionType GetExpressionType(BinaryExpression expression)
        {
            switch (expression.Operator)
            {
                case Operator.Period:
                    return BinaryExpressionType.Dotted;

                case Operator.Mul:
                case Operator.Div:
                case Operator.Sub:
                case Operator.Add:
                case Operator.Rem:
                    return BinaryExpressionType.Arithmetic;

                case Operator.Or:
                case Operator.And:
                    return BinaryExpressionType.Logical;

                case Operator.Less:
                case Operator.LessEqual:
                case Operator.Greater:
                case Operator.GreaterEqual:
                case Operator.Equals:
                case Operator.NotEquals:
                    return BinaryExpressionType.Conditional;

                case Operator.Assign:
                    return BinaryExpressionType.Assignment;

                default:
                    throw new Exception("This is not supposed to happen.");
            }
        }
    }
}
