using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler
{
    public enum TokenKind
    {
        Identifier,
        Keyword,
        IntNumber,
        RealNumber,
        StringLiteral,
        Plus,
        Minus,
        Slash,
        Percent,
        Power,
        Assign,
        Equals,
        NotEqual,
        Greater,
        Less,
        GreaterEqual,
        LessEqual,
        And,
        Or,
        Colon,
        Comma,
        Semicolon,
        Period,
        Star,
        Caret,
        SingleQuote,
        DoubleQuote,
        LeftParenthesis,
        RightParenthesis,
        LeftSquareBracket,
        RightSquareBracket,
        CommentStart,
        CommentEnd,
        EOF,
        EOL
    }
}
