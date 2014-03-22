using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace pascallib.Compiler
{
    public sealed class Tokenizer
    {
        //Setting variables
        private bool ignoreCase = true;
        private string[] keywords;

        //The scanner states
        private STATE lookaheadState;
        private STATE actualState;
    
        public Tokenizer(System.IO.TextReader textReader)
        {
            this.TextReader = textReader;

            Initialize();
        }

        private void Initialize()
        {
            this.actualState = new STATE();

            this.Ln = 1;
            this.Ch = 1;
            this.Index = 0;

            this.keywords = new string[31] { 
                "Program",
                "user",
                "type",
                "var",
                "begin",
                "end",
                "Integer",
                "Boolean",
                "String",
                "Character",
                "NULL",
                "record",
                "array",
                "set",
                "of",
                "case",
                "goto",
                "label",
                "and",
                "or",
                "not",
                "if",
                "else",
                "then",
                "while",
                "do",
                "for",
                "repeat",
                "until",
                "procedure",
                "function"
            };

            this.Errors = new List<Error>();  
        }

        /// <summary>
        /// Get a value indicating whether the parser has reached the end of the stream.
        /// </summary>
        public bool IsEOF
        {
            get { return this.actualState.IsEOF; }
            private set { this.actualState.IsEOF = value; }
        }

        /// <summary>
        /// Gets the number of the current line.
        /// </summary>
        public int Ln
        {
            get { return this.actualState.Ln; }
            private set { this.actualState.Ln = value; }
        }

        /// <summary>
        /// Gets the number of the current character of a line.
        /// </summary>
        public int Ch
        {
            get { return this.actualState.Ch; }
            private set { this.actualState.Ch = value; }
        }

        /// <summary>
        /// Gets the index of the character.
        /// </summary>
        public int Index
        {
            get { return this.actualState.Index; }
            private set { this.actualState.Index = value; }
        }

        /// <summary>
        /// Gets the TextReader.
        /// </summary>
        public TextReader TextReader
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
        /// Gets the current token that was previously read.
        /// </summary>
        public Token CurrentToken
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the next token from stream.
        /// </summary>
        public Token GetNext()
        {
            if (this.lookaheadState != null)
            {
                this.actualState = lookaheadState;
                this.lookaheadState = null; 
                
                return this.actualState.Token;            
            }

            Token token = scan();

            actualState = new STATE()
            {
                Token = token,
                Ch = Ch,
                Ln = Ln,
                Index = Index,
                IsEOF = IsEOF
            };

            CurrentToken = token;

            return token;
        }

        /// <summary>
        /// Peeks the next token from stream.
        /// </summary>
        public Token Peek()
        {
            if (this.lookaheadState == null)
            {
                Token token = scan();

                this.lookaheadState = new STATE()
                {
                    Token = token,
                    Ch = Ch,
                    Ln = Ln,
                    Index = Index,
                    IsEOF = IsEOF
                };
            }
       
            return this.lookaheadState.Token;
        }

        private Token scan()
        {
            StringBuilder sb = new StringBuilder();

            int ch, ln, index;
            char CH;

            while (this.hasNextCh())
            {
                /* Scans identifiers. */

                CH = this.peekCh();

                if (char.IsLetter(CH) || CH == '_')
                {
                    ch = this.Ch;
                    ln = this.Ln;
                    index = this.Index;

                    do
                    {
                        this.readCh();

                        sb.Append(CH);

                        CH = this.peekCh();
                    }
                    while (char.IsLetter(CH) || CH == '_');

                    if(identifyAsKeyword(sb.ToString()))
                    {
                        return resolveKeyword(sb, ch, ln, index);
                    }

                    return new Token(sb.ToString(), TokenKind.Identifier, ch, ln, index);
                }
                else if(char.IsDigit(CH))
                {
                    /* Scans for numbers. */
                    /* TODO: Improve this part. */

                    bool isFloat = false;

                    ch = this.Ch;
                    ln = this.Ln;
                    index = this.Index;

                    do
                    {
                        this.readCh();

                        sb.Append(CH);

                        CH = this.peekCh();
                    }
                    while (char.IsDigit(CH) || CH == '.');

                    if (isFloat)
                    {
                        return new Token(Double.Parse(sb.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint), TokenKind.RealNumber, ch, ln, index);
                    }

                    return new Token(Int32.Parse(sb.ToString()), TokenKind.IntNumber, ch, ln, index);
                }
                else if(CH == '\'')
                {
                    /* Scans for string/character literals. */

                    CH = readCh();

                    ch = this.Ch;
                    ln = this.Ln;
                    index = this.Index;

                    do
                    {
                        this.readCh();

                        sb.Append(CH);

                        CH = this.peekCh();
                    }
                    while (CH != '\'');

                    return new Token(sb.ToString(), TokenKind.StringLiteral, ch, ln, index);
                }
                else
                {
                    /* Scans for symbols. */

                    CH = this.readCh();

                    switch (CH)
                    {
                        case '+':
                            return new Token("+", TokenKind.Plus, this.Ch, this.Ln, this.Index);

                        case '-':
                            return new Token("-", TokenKind.Minus, this.Ch, this.Ln, this.Index);

                        case '*':
                            CH = this.peekCh();
                            if (CH == ')')
                            {
                                this.readCh();
                                return new Token("*)", TokenKind.CommentEnd, this.Ch - 1, this.Ln - 1, this.Index - 1);
                            }
                            return new Token("*", TokenKind.Star, this.Ch, this.Ln, this.Index);

                        case '/':
                            return new Token("/", TokenKind.Slash, this.Ch, this.Ln, this.Index);

                        case '%':
                            return new Token("%", TokenKind.Percent, this.Ch, this.Ln, this.Index);

                        case '^':
                            return new Token("^", TokenKind.Power | TokenKind.Caret, this.Ch, this.Ln, this.Index);

                        case '=':
                            return new Token("=", TokenKind.Equals, this.Ch, this.Ln, this.Index);

                        case '\'':
                            return new Token("\'", TokenKind.SingleQuote, this.Ch, this.Ln, this.Index);

                        case '\"':
                            return new Token("\"", TokenKind.DoubleQuote, this.Ch, this.Ln, this.Index);

                        case '(':
                            CH = this.peekCh();
                            if (CH == '*')
                            {
                                this.readCh();
                                return new Token("(*", TokenKind.CommentStart, this.Ch - 1, this.Ln - 1, this.Index - 1);
                            }
                            return new Token("(", TokenKind.LeftParenthesis, this.Ch, this.Ln, this.Index);

                        case ')':
                            return new Token(")", TokenKind.RightParenthesis, this.Ch, this.Ln, this.Index);

                        case '[':
                            return new Token("[", TokenKind.LeftSquareBracket, this.Ch, this.Ln, this.Index);

                        case ']':
                            return new Token("]", TokenKind.RightSquareBracket, this.Ch, this.Ln, this.Index);

                        case '<':
                            CH = this.peekCh();
                            if (CH == '>')
                            {
                                this.readCh();
                                return new Token("<>", TokenKind.NotEqual, this.Ch - 1, this.Ln - 1, this.Index - 1);
                            }
                            else if (CH == '=')
                            {
                                this.readCh();
                                return new Token("<=", TokenKind.LessEqual, this.Ch - 1, this.Ln - 1, this.Index - 1);
                            }
                            return new Token("<", TokenKind.Less, this.Ch, this.Ln, this.Index);

                        case '>':
                            CH = this.peekCh();
                            if (CH == '=')
                            {
                                this.readCh();
                                return new Token(">=", TokenKind.GreaterEqual, this.Ch - 1, this.Ln - 1, this.Index - 1);
                            }
                            return new Token(">", TokenKind.Greater, this.Ch, this.Ln, this.Index);

                        case ':':
                            CH = this.peekCh();
                            if (CH == '=')
                            {
                                this.readCh();
                                return new Token(":=", TokenKind.Assign, this.Ch - 1, this.Ln - 1, this.Index - 1);
                            }
                            return new Token(":", TokenKind.Colon, this.Ch, this.Ln, this.Index);

                        case ';':
                            return new Token(";", TokenKind.Semicolon, this.Ch, this.Ln, this.Index);

                        case '.':
                            return new Token(";", TokenKind.Period, this.Ch, this.Ln, this.Index);

                        case ',':
                            return new Token(";", TokenKind.Comma, this.Ch, this.Ln, this.Index);

                        case '\r':
                        case ' ':
                        case '\t':
                            break;

                        case '\n':
                            this.Ch = 1;
                            this.Ln++;
                            break;

                        default:
                            string description = string.Format("Unexpected character \'{0}\'.", CH);
                            this.Errors.Add(new Error(0, description, this.GetSourceLoc()));
                            break;
                    }
                }
            }

            return new Token("EOF", TokenKind.EOF, this.Ch, this.Ln, this.Index);
        }

        private static Token resolveKeyword(StringBuilder sb, int ch, int ln, int index)
        {
            string value = sb.ToString();

            if(string.Compare("and", value, true) == 0)
            {
                return new Token(sb.ToString(), TokenKind.And, ch, ln, index);
            }
            else if (string.Compare("or", value, true) == 0)
            {
                return new Token(sb.ToString(), TokenKind.Or, ch, ln, index);
            }
            else
            {
                return new Token(sb.ToString(), TokenKind.Keyword, ch, ln, index);
            }
        }

        private bool identifyAsKeyword(string p)
        {
            for(int i = 0; i < this.keywords.Length; i++)
            {
                if(string.Compare(p, this.keywords[i], this.ignoreCase) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool hasNextCh()
        {
            return this.TextReader.Peek() > -1;
        }

        private char peekCh()
        {
            return (char)this.TextReader.Peek();
        }

        private char readCh()
        {
            this.Ch++;
            this.Index++;

            return (char)this.TextReader.Read();
        }

        private char readCh2()
        {
            this.Index++;

            return (char)this.TextReader.Read();
        }

        /// <summary>
        /// Gets the current source location.
        /// </summary>
        /// <returns></returns>
        public SourceLocation GetSourceLoc()
        {
            return new SourceLocation(this.Ln, this.Ch, this.Index);
        }

        class STATE
        {
            public Token Token
            {
                get;
                set;
            }

            public bool IsEOF
            {
                get;
                set;
            }

            public int Ln
            {
                get;
                set;
            }

            public int Ch
            {
                get;
                set;
            }

            public int Index
            {
                get;
                set;
            }
        }
    }
}
