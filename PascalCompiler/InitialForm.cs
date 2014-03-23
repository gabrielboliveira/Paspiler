using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using PascalCompiler.Model;

namespace PascalCompiler
{
    public partial class InitialForm : Form
    {
        StringBuilder sb = new StringBuilder();

        Parser parser = new Parser();

        public InitialForm()
        {
            InitializeComponent();

            // Teste Classe Token
            //Token tk = Token.GetToken("program");

            var source = new BindingSource(parser.Tokens, null);
            gridViewTokens.DataSource = source;
        }

        /// <summary>
        /// Processa as teclas pressionadas no form.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                // F5 executa o parser.
                case (Keys.F5):
                    {
                        ExecutaParser();
                        return true;
                    }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void _codeTextBox_KeyPress(object sender, KeyPressEventArgs keyPressed)
        {
            switch (keyPressed.KeyChar)
            {
                // Caso o usuário aperte o backspace
                // Precisa ver se o usuário removeu algum token
                case '\b':
                    {
                        // Remove o último elemento
                        if (sb.Length > 0)
                            sb.Length--;
                        break;
                    }
                case ' ': // espaço
                case '\r': // enter (carriage return)
                    {
                        // Caso o usuário aperte espaço ou enter (\r), deve mandar verificar o token
                        parser.ParseToken(sb.ToString());
                        sb.Clear();
                        break;
                    }
                case ';':
                    {
                        // Se digitar ponto-e-vírgula, deve-se adicionar o token armazenado e também o ";".
                        parser.ParseToken(sb.ToString());
                        parser.ParseToken(";");
                        sb.Clear();
                        break;
                    }
                default:
                    {
                        // Caso a tecla apertada seja um número, letra, ou pontuação válida, adiciona ao sb para validar o token
                        if (Char.IsLetter(keyPressed.KeyChar) || Char.IsSeparator(keyPressed.KeyChar) || Char.IsPunctuation(keyPressed.KeyChar))
                            sb.Append(keyPressed.KeyChar);
                        break;
                    }
            }

            // Obs: comentei pq mudei a verificação de identificador de lugar
            // Agora está na classe Token

            //char caractere;

            //string verifyChar = "^[a-zA-Z]"; // regular expression that will check if "caractere" is a letter
            //string verifyCharorNumber = "^[a-zA-Z0-9]"; // regular expression that will check if "caractere" is a letter or a number

            //caractere = e.KeyChar;

            //if (Regex.IsMatch(caractere.ToString(), verifyCharorNumber))
            //{
            //    // if caractere is a letter: continue getting chars until find the space key or special char
            //    sb.Append(caractere); // Add chars on Buffer
            //}
            //else
            //{
            //    String possibleToken = sb.ToString();
            //    sb = new StringBuilder();
            //    // if possibleToken starts with a char, it's valid
            //    try
            //    {
            //        if (Regex.IsMatch(possibleToken.ElementAt(0).ToString(), verifyChar))
            //        {
            //            MessageBox.Show(possibleToken);
            //            // TODO: looks on table of special words. If possibleToken is already there, add it on the hashtable with  
            //            // its type. If is not create a new id for this token
            //        }
            //        else
            //        {
            //            MessageBox.Show("nao é valido: irá gerar um erro");
            //        }
            //    }
            //    catch (Exception erro)
            //    {
            //        MessageBox.Show("caractere inválido: " + erro);
            //    }
            //} // fim else
        } //fim metodo key_press

        /// <summary>
        /// Executa o parser no código digitado.
        /// </summary>
        private void ExecutaParser()
        {
            MessageBox.Show("Não Implementado!");


            // Pensamentos da madrugada:

            // utilizar Char.IsPunctuation(Char var) no while para verificar se é uma pontuação (, [, {, etc.
            // eu acho que isso dá certo
            // usar isso pra ficar mais fácil separar identificadores/números colados, sem espaço entre eles...
        }

    }
}
