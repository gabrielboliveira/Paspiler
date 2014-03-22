using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler
{
    /// <summary>
    /// ResourceHelper class
    /// </summary>
    public class ResourceHelper
    {
        /// <summary>
        /// Resolves the name of a kind of a token.
        /// </summary>
        /// <param name="tokenKind"></param>
        /// <returns></returns>
        public static string ResolveName(TokenKind tokenKind)
        {
            switch (tokenKind)
            {
                case TokenKind.Identifier:
                    return "identifier";

                case TokenKind.Keyword:
                    return "keyword";

                case TokenKind.IntNumber:
                    return "integer number";

                case TokenKind.RealNumber:
                    return "floating poing number";

                case TokenKind.StringLiteral:
                    return "string";

                //case TokenKind.Plus:
                //    return "plus";

                //case TokenKind.Minus:
                //    return "minus";

                //case TokenKind.Slash:
                //    return "slash";

                //case TokenKind.Percent:
                //    return "percent";

                //case TokenKind.Power:
                //    break;

                //case TokenKind.Assign:
                //    break;

                //case TokenKind.Equals:
                //    break;

                //case TokenKind.NotEqual:
                //case TokenKind.Greater:
                //case TokenKind.Less:
                //case TokenKind.GreaterEqual:
                //case TokenKind.LessEqual:
                //case TokenKind.Colon:
                //case TokenKind.Semicolon:
                //case TokenKind.Period:
                //case TokenKind.Star:
                //case TokenKind.Caret:
                //case TokenKind.SingleQuote:
                //case TokenKind.DoubleQuote:

                case TokenKind.LeftParenthesis:
                case TokenKind.RightParenthesis:
                    return "parenthesis";

                //case TokenKind.LeftSquareBracket:
                //    break;

                //case TokenKind.RightSquareBracket:
                //    break;

                //case TokenKind.CommentStart:
                //    break;

                //case TokenKind.CommentEnd:
                //    break;

                case TokenKind.EOF:
                    return "End-of-file";

                case TokenKind.EOL:
                    return "End-of-line";

                default:
                    throw new Exception("The kind of token does not have a specific name.");
            }
        }
    }
}
