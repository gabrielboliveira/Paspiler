using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler
{
    public class Token
    {
        public Token(object value, TokenKind kind, int ch, int ln, int index)
        {
            this.Value = value;
            this.Kind = kind;
            this.Ch = ch;
            this.Ln = ln;
            this.Index = index;
        }

        public object Value
        {
            get;
            protected set;
        }

        public TokenKind Kind
        {
            get;
            protected set;
        }

        public int Ch
        {
            get;
            protected set;
        }

        public int Ln
        {
            get;
            protected set;
        }

        public int Index
        {
            get;
            protected set;
        }

        public SourceLocation GetSourceLoc()
        {
            return new SourceLocation(this.Ln, this.Ch, this.Index);
        }
        
        public override string ToString()
        {
            return this.Value.ToString();
        }

        #region Implicit operators
      
        public static implicit operator TokenKind(Token token)
        {
            return token.Kind;
        }

        public static implicit operator string(Token token)
        {
            return token.Value.ToString();
        }

        #endregion

        #region Operator overloads

        public static bool operator == (Token token, TokenKind tokenKind)
        {
            return token.Kind == tokenKind;
        }

        public static bool operator != (Token token, TokenKind tokenKind)
        {
            return token.Kind != tokenKind;
        }

        public static bool operator == (Token token, string value)
        {
            return token.Value.ToString() == value;
        }

        public static bool operator != (Token token, string value)
        {
            return token.Value.ToString() == value;
        }

        #endregion

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
