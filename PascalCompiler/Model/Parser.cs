using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PascalCompiler.Model
{
    /// <summary>
    /// Recebe um token, manda para <see cref="Token"/> fazer a validação, e guarda uma lista de tokens válidos.
    /// </summary>
    class Parser
    {
        private BindingList<Token> tokens = new BindingList<Token>();

        /// <summary>
        /// Lista de tokens válidos
        /// </summary>
        internal BindingList<Token> Tokens
        {
            get { return tokens; }
            set { tokens = value; }
        }

        public Parser() { }

        /// <summary>
        /// Limpa os objetos internos.
        /// </summary>
        public void Clear()
        {
            tokens.Clear();
        }

        /// <summary>
        /// Recebe um token, envia para <see cref="Token"/> validar e caso seja válido, salva em <see cref="Tokens"/>
        /// </summary>
        /// <param name="strToken">Token a ser validado.</param>
        public void ParseToken(string strToken, int startIndex)
        {
            // Manda processar o token
            Token token = Token.GetToken(strToken, startIndex);

            // Caso o token seja válido, adiciona a lista de tokens válidos e processados
            if (token.TokenType != Token.TokenTypeEnum.NonExistant)
            {
                // MessageBox.Show(token.ToString());
                tokens.Add(token);
            }
        }
    }
}
