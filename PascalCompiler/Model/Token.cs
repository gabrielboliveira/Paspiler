using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PascalCompiler.Model
{
    class Token
    {
        private Token(string token)
        {
            value = token;
            tokenType = IsKeyword(token);
            // Caso o token não seja válido, verificar se é número ou identificador válido
            if (tokenType == Token.TokenTypeEnum.NonExistant)
            {
                int aux;
                // Verifica se é um número INTEIRO - precisa arrumar pra Real
                if (int.TryParse(token, out aux))
                {
                    tokenType = TokenTypeEnum.Number;
                }
                // Não é número, verificar se é id. válido
                else
                {
                    // Trecho Amanda

                    string verifyChar = "^[a-zA-Z]"; // regular expression that will check if "caractere" is a letter
                    string verifyCharorNumber = "^[a-zA-Z0-9]"; // regular expression that will check if "caractere" is a letter or a number

                    try
                    {
                        if (Regex.IsMatch(token.ElementAt(0).ToString(), verifyChar))
                        {
                            MessageBox.Show(token);
                            // TODO: looks on table of special words. If possibleToken is already there, add it on the hashtable with  
                            // its type. If is not create a new id for this token
                        }
                        else
                        {
                            MessageBox.Show("nao é valido: irá gerar um erro");
                        }
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show("caractere inválido: " + erro);
                    }
                }
            }
        }

        private string value;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        private TokenTypeEnum tokenType = TokenTypeEnum.NonExistant;

        public TokenTypeEnum TokenType
        {
            get { return tokenType; }
            set { tokenType = value; }
        }

        public override string ToString()
        {
            return Convert.ToString(this.tokenType);
        }

        #region Dicionário
        // Relaciona cada Enum com sua palavra reservada
        private static Dictionary<TokenTypeEnum, string> tokenValues = new Dictionary<TokenTypeEnum, string>()
        {
            { TokenTypeEnum.Program , "program" },
            { TokenTypeEnum.User,"user" },
            { TokenTypeEnum.Type, "type" },
            { TokenTypeEnum.Var,"var" },
            { TokenTypeEnum.Begin,"begin" },
            { TokenTypeEnum.End,"end" },
            { TokenTypeEnum.Integer,"integer" },
            { TokenTypeEnum.Boolean,"boolean" },
            { TokenTypeEnum.String,"string" },
            { TokenTypeEnum.Character,"character" },
            { TokenTypeEnum.Null,"null" },
            { TokenTypeEnum.Record,"record" },
            { TokenTypeEnum.Array,"array" },
            { TokenTypeEnum.Set,"set" },
            { TokenTypeEnum.Of,"of" },
            { TokenTypeEnum.Case,"case" },
            { TokenTypeEnum.Goto, "goto" },
            { TokenTypeEnum.Label, "label" },
            { TokenTypeEnum.And, "and" },
            { TokenTypeEnum.Or, "or" },
            { TokenTypeEnum.Not, "not" },
            { TokenTypeEnum.If, "if" },
            { TokenTypeEnum.Else, "else" },
            { TokenTypeEnum.Then, "then" },
            { TokenTypeEnum.While, "while" },
            { TokenTypeEnum.Do, "do"},
            { TokenTypeEnum.For, "for"},
            { TokenTypeEnum.Repeat, "repeat"},
            { TokenTypeEnum.Until, "until"},
            { TokenTypeEnum.Procedure, "procedure"},
            { TokenTypeEnum.Function, "function"},
            { TokenTypeEnum.Attribution, ":="},
            { TokenTypeEnum.EqualsTo, "="},
            { TokenTypeEnum.At, "@"},
            { TokenTypeEnum.Up, "^"},
            { TokenTypeEnum.Sum, "+"},
            { TokenTypeEnum.Sub, "-"},
            { TokenTypeEnum.Multiplier, "*"},
            { TokenTypeEnum.Divisor, "/"},
            { TokenTypeEnum.Equals, "="},
            { TokenTypeEnum.Semicolon, ";"},
            { TokenTypeEnum.Colon, ":"},
            { TokenTypeEnum.Comma, ","},
            { TokenTypeEnum.Point, "."},
            { TokenTypeEnum.LessThan, "<"},
            { TokenTypeEnum.LessThanOrEqual, "<="},
            { TokenTypeEnum.GreaterThan, ">"},
            { TokenTypeEnum.GreaterThanOrEqual, ">="},
            { TokenTypeEnum.InitialSquareBracket, "["},
            { TokenTypeEnum.FinalSquareBracket, "]"},
            { TokenTypeEnum.InitialParentheses, "("},
            { TokenTypeEnum.FinalParentheses, ")"},
            { TokenTypeEnum.InitialCurlyBrackets, "{"},
            { TokenTypeEnum.FinalCurlyBrackets, "}"},
            { TokenTypeEnum.Dollar, "$"},
            { TokenTypeEnum.Hash, "#"},
            { TokenTypeEnum.InitialComment, "(*"},
            { TokenTypeEnum.FinalComment, "(*"},
            { TokenTypeEnum.LineComment, "//"}
        };
        #endregion

        #region Enum Tipo do Token
        public enum TokenTypeEnum
        {
            NonExistant,
            Program,
            User,
            Type,
            Var,
            Begin,
            End,
            Integer,
            Boolean,
            String,
            Character,
            Null,
            Record,
            Array,
            Set,
            Of,
            Case,
            Goto,
            Label,
            And,
            Or,
            Not,
            If,
            Else,
            Then,
            While,
            Do,
            For,
            Repeat,
            Until,
            Procedure,
            Function,
            Attribution,
            EqualsTo,
            At,
            Up,
            Sum,
            Sub,
            Multiplier,
            Divisor,
            Equals,
            Semicolon,
            Colon,
            Comma,
            Point,
            LessThan,
            LessThanOrEqual,
            GreaterThan,
            GreaterThanOrEqual,
            InitialSquareBracket,
            FinalSquareBracket,
            InitialParentheses,
            FinalParentheses,
            InitialCurlyBrackets,
            FinalCurlyBrackets,
            Dollar,
            Hash,
            InitialComment,
            FinalComment,
            LineComment,

            Number,
            Identifier
        }
        #endregion

        // Verifica se um token é uma palavra reservada - keyword
        private static TokenTypeEnum IsKeyword(string token)
        {
            TokenTypeEnum value = TokenTypeEnum.NonExistant;

            try
            {
                // Procura no dicionário se a palavra existe
                value = tokenValues.FirstOrDefault(tk => tk.Value.Equals(token.ToLower())).Key;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return value;
        }

        public static Token GetToken(string token)
        {
            return new Token(token);
        }
    }
}
