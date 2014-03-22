using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using pascallib.Compiler.Ast;

namespace pascallib.Compiler
{
    public sealed class Parser
    {
        private bool hasParsed = false;
        private List<string> allowedKeywords;

        public Parser(Tokenizer tokenizer)
        {
            this.Tokenizer = tokenizer;
            this.Errors = Tokenizer.Errors;

            Initialize();
        }

        public Parser(TextReader textReader)
        {
            this.Tokenizer = new Tokenizer(textReader);
            this.Errors = Tokenizer.Errors;

            Initialize();
        }

        private void Initialize()
        {
            this.allowedKeywords = new List<string>() { 
                "Integer", "Real", "Boolean", "Character", "String" };
        }

        /// <summary>
        /// Gets the tokenizer used by this parser.
        /// </summary>
        public Tokenizer Tokenizer
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of errors.
        /// </summary>
        public List<Error> Errors
        {
            get;
            private set;
        }

        /// <summary>
        /// Parses the input. (IMPORTANT: May only be executed once)
        /// </summary>
        /// <returns></returns>
        public ProgramRoot Parse()
        {
            if (!hasParsed)
            {
                ProgramRoot root = parseRoot();

                hasParsed = true;

                return root;
            }

            throw new Exception("Parser has already processed the file.");
        }

        private ProgramRoot parseRoot()
        {
            ProgramRoot root = new ProgramRoot();

            while (!this.Tokenizer.IsEOF)
            {
                //if (this.MaybeEat("type"))
                //{
                //    root.AddTypeDecl(null);
                //}
                //else
                //{
                //    this.ReportError("Expected ", this.Tokenizer.GetSourceLoc());
                //}
            }

            return root;
        }

        private FunctionDecl parseFunctionDecl()
        {
            //Token token;

            //string name, returnType;

            //List<Statement> statements;
            //List<FunctionParam> functionParam;

            //Eat("Function");

            //token = this.Tokenizer.GetNext();

            //name = token.ToString();

            //Expect(TokenKind.Colon);

            //token = this.Eat(TokenKind.Identifier | TokenKind.Keyword);

            //returnType = token.ToString();

            //functionParam = this.parseParameters();

            //statements = 

            //FunctionDecl functionDecl = new FunctionDecl(name, returnType);

            //throw new NotImplementedException();

            throw new NotImplementedException();
        }

        private List<FunctionParam> parseParameters()
        {
            bool flag = true;

            List<FunctionParam> functionParam = new List<FunctionParam>();

            Expect(TokenKind.LeftParenthesis);

            while (flag)
            {

            }

            Expect(TokenKind.RightParenthesis);

            return functionParam;
        }

        public Expression ParseExpression(int precedence = 0)
        {
            Expression ret = this.ParsePrimary();

            while (true)
            {
                Token lookaheadToken = this.Tokenizer.Peek();

                if (this.isOperator(lookaheadToken))
                {
                    Operator op = this.getOperator(lookaheadToken);
                    int num = this.getOperatorPrecedence(op);

                    if (num >= precedence)
                    {
                        this.Tokenizer.GetNext();

                        SourceLocation startLoc = ret.SourceSpan.Start;

                        Expression right = this.ParseExpression(num + 1);
                        ret = new BinaryExpression(op, ret, right);

                        ret.SetStartLoc(startLoc);
                        ret.SetEndLoc(this.Tokenizer.GetSourceLoc());
                    }
                    else
                    {
                        return ret;
                    }
                }
                else
                {
                    return ret;
                }
            }
        }

        private Expression ParsePrimary()
        {
            Expression expression = null;

            Token token = this.Tokenizer.GetNext();

            switch (token.Kind)
            {
                case TokenKind.Identifier:
                    expression = new IdentifierExpression(token.ToString());
                    expression = ParseFunctionCall(expression);
                    expression = ParseAccessor(expression);
                    break;

                case TokenKind.Keyword:
                    expression = this.identifyAllowedKeyword(token);
                    break;

                case TokenKind.IntNumber:
                    expression = new IntegerNumberExpression((int)token.Value);
                    break;

                case TokenKind.RealNumber:
                    expression = new RealNumberExpression((double)token.Value);
                    break;

                case TokenKind.LeftParenthesis:
                    expression = this.ParseParenthesisExpr();
                    break;

                case TokenKind.Minus:
                    expression = new NegateExpression(this.ParsePrimary());
                    break;

                default:
                    string message = string.Format("Unexpected {0}", ResourceHelper.ResolveName(token.Kind));

                    Error error = this.ReportError(message, token.GetSourceLoc());

                    expression = new ErrorExpression(error);
                    break;
            }

            expression.SetStartLoc(token.GetSourceLoc());
            expression.SetEndLoc(this.Tokenizer.GetSourceLoc());

            return expression;
        }

        private Expression ParseFunctionCall(Expression expression)
        {
            List<Expression> list = new List<Expression>();

            Token token = this.Tokenizer.Peek();

            if (token == TokenKind.LeftParenthesis)
            {
                this.Tokenizer.GetNext();

                while (token != TokenKind.RightParenthesis)
                {
                    list.Add(this.ParseExpression());

                    MaybeEat(TokenKind.Comma);

                    token = this.Tokenizer.Peek();
                }
  
                this.Eat(TokenKind.RightParenthesis);
                
                expression = new FunctionCallExpression(expression, list);
            }

            return expression;
        }

        private Expression ParseAccessor(Expression expression)
        {
            List<Expression> list = new List<Expression>();

            Token token = this.Tokenizer.Peek();

            if (token == TokenKind.LeftSquareBracket)
            {
                this.Tokenizer.GetNext();

                while(token != TokenKind.RightSquareBracket)
                {
                    list.Add(this.ParseExpression());

                    MaybeEat(TokenKind.Comma);

                    token = this.Tokenizer.Peek();
                }

                this.Eat(TokenKind.RightSquareBracket);
                
                expression = new AccessorExpression(expression, list);
            }

            return expression;
        }

        private Expression identifyAllowedKeyword(Token token)
        {
            string value = token.ToString();

            for (int i = 0; i < this.allowedKeywords.Count; i++)
            {
                if (string.Compare(this.allowedKeywords[i], token.ToString(), true) == 0)
                {
                    return new IdentifierExpression(value);
                }
            }

            string message = string.Format("Unexpected {0}", ResourceHelper.ResolveName(token.Kind));

            Error error = this.ReportError(message, token.GetSourceLoc());

            return new ErrorExpression(error);
        }

        private Expression ParseParenthesisExpr()
        {
            Expression expression = this.ParseExpression();

            this.Expect(TokenKind.RightParenthesis);

            return expression;
        }

        private Operator getOperator(Token token)
        {
            switch (token.Kind)
            {
                case TokenKind.Plus:
                    return Operator.Add;

                case TokenKind.Minus:
                    return Operator.Sub;

                case TokenKind.Star:
                    return Operator.Mul;

                case TokenKind.Slash:
                    return Operator.Div;

                case TokenKind.Percent:
                    return Operator.Rem;

                case TokenKind.Or:
                    return Operator.Or;

                case TokenKind.And:
                    return Operator.And;

                case TokenKind.Assign:
                    return Operator.Assign;

                case TokenKind.Period:
                    return Operator.Period;

                case TokenKind.Less:
                    return Operator.Less;

                case TokenKind.LessEqual:
                    return Operator.LessEqual;

                case TokenKind.Greater:
                    return Operator.Greater;

                case TokenKind.GreaterEqual:
                    return Operator.GreaterEqual;

                case TokenKind.Equals:
                    return Operator.Equals;

                case TokenKind.NotEqual:
                    return Operator.NotEquals;

                default:
                    throw new Exception("Not an operator.");
            }
        }

        private int getOperatorPrecedence(Operator op)
        {
            switch (op)
            {
                case Operator.Period:
                    return 8;

                case Operator.Mul:
                case Operator.Div:
                    return 7;

                case Operator.Sub:
                case Operator.Add:
                    return 6;

                case Operator.Rem:
                    return 5;

                case Operator.Or:
                    return 4;

                case Operator.And:
                    return 3;

                case Operator.Less:
                case Operator.LessEqual:
                case Operator.Greater:
                case Operator.GreaterEqual:
                    return 2;

                case Operator.Equals:
                case Operator.NotEquals:
                    return 2;

                case Operator.Assign:
                    return 1;

                default:
                    return -1;
            }
        }

        private bool isOperator(Token token)
        {
            switch (token.Kind)
            {
                case TokenKind.Plus:
                case TokenKind.Minus:
                case TokenKind.Star:
                case TokenKind.Slash:
                case TokenKind.Percent:
                case TokenKind.Period:
                case TokenKind.Or:
                case TokenKind.And:
                case TokenKind.Assign:
                case TokenKind.Less:
                case TokenKind.LessEqual:
                case TokenKind.Greater:
                case TokenKind.GreaterEqual:
                case TokenKind.Equals:
                case TokenKind.NotEqual:
                    return true;

                default:
                    return false;
            }
        }

        private bool Expect(TokenKind tokenKind)
        {
            Token token = this.Tokenizer.GetNext();

            if (token.Kind == tokenKind)
            {
                return true;
            }

            string message = string.Format("Expected {0}.", ResourceHelper.ResolveName(tokenKind));
            Error error = new Error(-1, message, token.GetSourceLoc());
            this.Errors.Add(error);

            return false;
        }

        private bool Expect(string value)
        {
            Token token = this.Tokenizer.GetNext();

            if (string.Compare(value, token.ToString(), true) == 0)
            {
                return true;
            }

            string message = string.Format("Expected {0}.", value);
            Error error = new Error(-1, message, token.GetSourceLoc());
            this.Errors.Add(error);

            return false;
        }

        private Token MaybeEat(TokenKind tokenKind)
        {
            Token token = this.Tokenizer.Peek();

            if (token.Kind == tokenKind)
            {
                this.Tokenizer.GetNext();
                return token;
            }

            return null;
        }

        private Token MaybeEat(string value)
        {
            Token token = this.Tokenizer.Peek();

            if (string.Compare(token.Value.ToString(), value, true) == 0)
            {
                this.Tokenizer.GetNext();
                return token;
            }

            return null;
        }

        private Token Eat(TokenKind tokenKind)
        {
            Token token = this.Tokenizer.GetNext();

            if (token.Kind == tokenKind)
            {
                return token;
            }

            string message = string.Format("Unexpected {0}.", ResourceHelper.ResolveName(token.Kind));
            Error error = new Error(-1, message, token.GetSourceLoc());
            this.Errors.Add(error);

            return null;
        }

        private Token Eat(string value)
        {
            Token token = this.Tokenizer.GetNext();

            if (string.Compare(token.Value.ToString(), value, true) == 0)
            {
                return token;
            }

            string message = string.Format("Unexpected \"{0}\".", token.Value.ToString());
            Error error = new Error(-1, message, token.GetSourceLoc());
            this.Errors.Add(error);

            return null;
        }

        private Error ReportError(string message, SourceLocation sourceLocation)
        {
            Error error = new Error(-1, message, sourceLocation);
            this.Errors.Add(error);

            return error;
        }
    }
}
