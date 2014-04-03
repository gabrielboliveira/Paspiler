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
    /// <summary>
    /// Reconhece o tipo de token entre Keyword (reservado pascal), Real Number, Integer Number, ou Identifier.
    /// </summary>
    class Token
    {
        /// <summary>
        /// Reconhece o token e salva dentro de seu objeto. 
        /// Caso seja um objeto válido, <see cref="TokenType" /> guarda o valor.
        /// Caso não seja um objeto váido, <see cref="TokenType" /> é NonExistant.
        /// </summary>
        private Token(string token, int startIndex)
        {
            this.StartIndex = startIndex;
            this.Value = token;
            // Verifica se é um número
            this.TokenType = IsNumber(token);
            if (this.TokenType == Token.TokenTypeEnum.NonExistant)
            {
                // Verifica se é uma string válida
                this.TokenType = IsString(token);
                if (this.TokenType == Token.TokenTypeEnum.NonExistant)
                {
                    // Verifica se é uma palavra reservada do Pascal
                    this.TokenType = IsKeyword(token);
                    if (this.TokenType == Token.TokenTypeEnum.NonExistant) 
                    {
                        //Verifica se é um identificador válido
                        this.TokenType = IsValidIdentifier(token);
                    }
                }
                    
             }
        }        
        
        private string value;

        /// <summary>
        /// Salva o valor do token identificado.
        /// </summary>
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        private TokenTypeEnum tokenType = TokenTypeEnum.NonExistant;

        /// <summary>
        /// Identifica qual o tipo do token.
        /// Caso seja um objeto válido, guarda o valor.
        /// Caso não seja um objeto váido, é NonExistant.
        /// </summary>
        public TokenTypeEnum TokenType
        {
            get { return tokenType; }
            set { tokenType = value; }
        }

        public override string ToString()
        {
            return Convert.ToString(this.tokenType);
        }

        private int startIndex = 0;

        /// <summary>
        /// Armazena a posição inicial dentro do código.
        /// </summary>
        public int StartIndex
        {
            get { return startIndex; }
            set { startIndex = value; }
        }


        #region Dicionário
        /// <summary>
        /// Referencia cada palavra reservada em String a seu devido tipo do enumerador <see cref="TokenTypeEnum" />.
        /// </summary>
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
            { TokenTypeEnum.Real,"real" },
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
            { TokenTypeEnum.Equals, ":="},
            { TokenTypeEnum.Semicolon, ";"},
            { TokenTypeEnum.Colon, ":"},
            { TokenTypeEnum.Comma, ","},
            { TokenTypeEnum.Point, "."},
            { TokenTypeEnum.Apostrophe, "'"},
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
            { TokenTypeEnum.FinalComment, "*)"},
            { TokenTypeEnum.LineComment, "//"},
            { TokenTypeEnum.EndPoint, "end." }
        };
        #endregion

        #region Enum Tipo do Token
        /// <summary>
        /// Tipos válidos de tokens.
        /// </summary>
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
            Real,
            Character,
            Null,
            Text,
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
            Apostrophe,
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
            EndPoint,

            RealNumber,
            IntegerNumber,

            Identifier
        }
        #endregion

        /// <summary>
        /// Verifica se um token é uma palavra reservada do Pascal.
        /// </summary>
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

        /// <summary>
        /// Verifica se o token é um número.
        /// </summary>
        private static TokenTypeEnum IsNumber(string token)
        {
            TokenTypeEnum value = TokenTypeEnum.NonExistant;

            int aux;
            float aux2;

            if (int.TryParse(token, out aux))
            {
                value = TokenTypeEnum.IntegerNumber;
            }
            else if (float.TryParse(token, out aux2))
            {
                value = TokenTypeEnum.RealNumber;
            }

            return value;
        }

        /// <summary>
        /// Verifica se o token é um identificador válido.
        /// </summary>
        private static TokenTypeEnum IsValidIdentifier(string token)
        {
            TokenTypeEnum value = TokenTypeEnum.NonExistant;

            // Trecho Amanda

            string verifyChar = "^[a-zA-Z]"; // regular expression that will check if "caractere" is a letter

            try
            {
                if (Regex.IsMatch(token.ElementAt(0).ToString(), verifyChar))
                {
                    value = TokenTypeEnum.Identifier;
                    //MessageBox.Show(token);
                }
            }
            catch (Exception erro)
            {
                //MessageBox.Show("caractere inválido: " + erro);
            }

            return value;
        }
        /// <summary>
        /// Verifica se o token é uma string.
        /// </summary>
        private static TokenTypeEnum IsString(string token) 
        {
            TokenTypeEnum value = TokenTypeEnum.NonExistant;
            try
            {
                if (token.ElementAt(0) == Convert.ToChar(39)) 
                {
                    value = TokenTypeEnum.Text;
                }
            }
            catch (Exception e) { }
            return value;
        }

        /// <summary>
        /// Identifica um token, comparando com as palavras reservadas e devidas verificações. Caso seja um token,
        /// é armazenado o seu tipo. Caso não seja um token, seu tipo é NonExistant.
        /// </summary>
        /// <param name="token">Token a ser validado.</param>
        /// <returns>Retorna um novo objeto com o Token tratado.</returns>
        public static Token GetToken(string token, int startIndex)
        {
            return new Token(token, startIndex);
        }
    }
}
