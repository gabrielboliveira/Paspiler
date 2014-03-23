using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PascalCompiler.Model
{
    class Parser
    {
        private BindingList<Token> tokens = new BindingList<Token>();

        internal BindingList<Token> Tokens
        {
            get { return tokens; }
            set { tokens = value; }
        }

        public Parser() { }

        // Recebe um token e processa
        public void ParseToken(string strToken)
        {
            // Manda processar o token
            Token token = Token.GetToken(strToken);
            int aux;

            // Caso o token seja válido, adiciona a lista de tokens válidos e processados
            if (token.TokenType != Token.TokenTypeEnum.NonExistant)
            {
                // MessageBox.Show(token.ToString());
                tokens.Add(token);
            }
        }
    }
}
