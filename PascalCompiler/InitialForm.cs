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
using System.IO;

namespace PascalCompiler
{
    public partial class InitialForm : Form
    {
        Parser _parser = new Parser();

        string _initialDirectory = @"C:\";

        bool _debugMode = false, _outputMode = false;

        public InitialForm()
        {
            InitializeComponent();

            // Teste Classe Token
            //Token tk = Token.GetToken("program");

            Globals.Execute = BeginInvoke;

            var validSource = new BindingSource(_parser.ValidTokens, null);
            gridViewValidTokens.DataSource = validSource;
            gridViewValidTokens.Visible = _debugMode;

            var notValidSource = new BindingSource(_parser.NotValidTokens, null);
            gridViewNotValidTokens.DataSource = notValidSource;
            gridViewNotValidTokens.Visible = _debugMode;

            var outputSource = new BindingSource(_parser.Output, null);
            gridViewOutput.DataSource = outputSource;
            gridViewOutput.Visible = _outputMode;
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
                        //ExecutaParser();
                        _parser.Execute(_codeTextBox.Text);
                        ChangeOutput(true);
                        return true;
                    }
                case (Keys.F2):
                    {
                        OpenFileDialog theDialog = new OpenFileDialog();
                        theDialog.Title = "Abrir código fonte Pascal";
                        theDialog.Filter = "Código Fonte Pascal|*.pas";
                        theDialog.InitialDirectory = _initialDirectory;
                        if (theDialog.ShowDialog() == DialogResult.OK)
                        {
                            string filename = theDialog.FileName;

                            _initialDirectory = Path.GetDirectoryName(filename);

                            string[] filelines = File.ReadAllLines(filename);

                            _codeTextBox.Text = string.Join('\n'.ToString(), filelines);
                        }
                        return true;
                    }
                case (Keys.F9):
                    {
                        _debugMode = !_debugMode;
                        gridViewValidTokens.Visible = _debugMode;
                        gridViewNotValidTokens.Visible = _debugMode;
                        int factor = (gridViewValidTokens.Width + gridViewNotValidTokens.Width + 12);
                        if(_debugMode)
                        {
                            _codeTextBox.Width -= factor;
                            gridViewOutput.Width -= factor;
                        }
                        else
                        {
                            _codeTextBox.Width += factor;
                            gridViewOutput.Width += factor;
                        }
                        return true;
                    }
                case (Keys.F8):
                    {
                        ChangeOutput();
                        return true;
                    }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ChangeOutput(bool reference)
        {
            if (reference == this._outputMode)
                return;
            _outputMode = !_outputMode;
            gridViewOutput.Visible = _outputMode;
            int factor = (gridViewOutput.Height + 6);
            if (_outputMode)
            {
                _codeTextBox.Height -= factor;
            }
            else
            {
                _codeTextBox.Height += factor;
            }
        }

        private void ChangeOutput()
        {
            this.ChangeOutput(!this._outputMode);
        }
        /*
        /// <summary>
        /// Executa o parser no código digitado.
        /// </summary>
        private void ExecutaParser()
        {
            
            string codeText = _codeTextBox.Text;
            string texto;
            
            // position guarda a posição que está dentro da string do programa (codeText)
            int position = 0, startIndex = 0;

            char key, apostrofo;
            //coloquei em uma variavel poq no case nao tava dando certo
            apostrofo = Convert.ToChar(39);

            // Limpa o parser pra não repetir os erros
            parser.Clear();
           
            do
            {
                key = codeText.ElementAt(position);
                if (key == apostrofo)
                {
                    parser.ParseToken(sb.ToString(), startIndex);
                    sb.Clear();
                    startIndex = position;
                    texto = "";
                    position++;
                    while (codeText.ElementAt(position).ToString() != "'")
                    {
                        //vai concatenando em uma string tudo o que estiver entre os apóstrofos
                        texto += codeText.ElementAt(position);
                        position++;
                        if (position == codeText.Length)
                            break;
                       
                    } 
                    parser.ParseToken("'"+texto+"'", startIndex);
                               
                }
                else
                {
                    switch (key)
                    {

                        case ' ': // espaço
                        
                        case '\n': // enter (carriage return)
                            {
                                // se for espaço ou enter (\r), deve mandar verificar o token
                                parser.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                break;
                            }
                        case ';':
                            {
                                // Se for ponto-e-vírgula, deve-se adicionar o token armazenado e também o ";".
                                parser.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                parser.ParseToken(";", startIndex);
                                break;
                            }
                        case ':':
                            {
                                // se encontrar ":", verifica se o caractere em seguida é um "=", para
                                // separar ":=" de somente ":"
                                parser.ParseToken(sb.ToString(),startIndex);
                                sb.Clear();
                                startIndex = position;
                                if ((codeText.ElementAt(position + 1)) == '=')
                                {
                                    parser.ParseToken(":=", startIndex);
                                    // pula a posição em seguida, pois já foi verificado
                                    position++;
                                }
                                else
                                {
                                    parser.ParseToken(":", startIndex);
                                }
                                break;
                            }
                        case '(':
                            {
                                // se encontrar "(", verifica se o caractere em seguida é um "*", para
                                // separar "(*" de somente "("
                                parser.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                if ((codeText.ElementAt(position + 1)) == '*')
                                {
                                    // ignora tudo até encontrar o outro * e ) - comentario nao precisa ser identificado na tabela
                                    do
                                    {
                                        position++;
                                        
                                        if (codeText.ElementAt(position) == '*' && codeText.ElementAt(position + 1) == ')') 
                                        {
                                            position++;
                                            break;
                                        }

                                    } while (position < (codeText.Length-1));
                                }
                                else
                                {
                                    parser.ParseToken("(", startIndex);
                                }
                                break;
                            }
                        case '*':
                            {
                                // se encontrar "*", verifica se o caractere em seguida é um ")", para
                                // separar "*)" de somente "*"
                                parser.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                if ((codeText.ElementAt(position + 1)) == ')')
                                {
                                    parser.ParseToken("*)", startIndex);
                                    position++;
                                }
                                else
                                {
                                    parser.ParseToken("*", startIndex);
                                }
                                break;
                            }
                        case '/':
                            {
                                // se encontrar "/", verifica se o caractere em seguida é uma outra "/", para
                                // ignorar o comentário até o fim da linha
                                parser.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                if ((codeText.ElementAt(position + 1)) == '/')
                                {
                                    //ignora toda a linha
                                    do
                                    {
                                        position++;
                                    } while (codeText.ElementAt(position) != '\n');                           
                                }
                                else
                                {
                                    parser.ParseToken("/", startIndex);
                                }
                                break;
                            }
                        case '<':
                        case '>': 
                            {
                                // verifica se tem um "=" logo em seguida
                                parser.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                if ((codeText.ElementAt(position + 1)) == '=')
                                {
                                    parser.ParseToken(key + "=", startIndex);
                                    position++;                                  
                                }
                                else
                                {
                                    parser.ParseToken(key.ToString(), startIndex);
                                }
                                break;
                            }
                        case ')':
                        case '[':
                        case ']':
                        case '{':
                        case '}':
                        case '+':
                        case '-':
                        case '#':
                        case ',':
                        case '=':
                        case '^':
                        case '@':
                        case '.':
                            {
                                parser.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                parser.ParseToken(key.ToString(), startIndex);
                                break;
                            }
                        //case '=':
                        //    {
                        //        parser.ParseToken(sb.ToString());
                        //        sb.Clear();
                        //        if ((codeText.ElementAt(position + 1)) == '=')
                        //        {
                        //            parser.ParseToken("==");
                        //            position++;
                        //        }
                        //        else
                        //        {
                        //            parser.ParseToken("=");
                        //        }
                        //        break;
                        //    }
                        default:
                            {
                                // Caso a tecla apertada seja um número, letra, ou pontuação válida, adiciona ao sb para validar o token
                                if (Char.IsLetter(key) || Char.IsSeparator(key) || Char.IsPunctuation(key) || Char.IsNumber(key))
                                {
                                    sb.Append(key);
                                    // trata fim de "arquivo"
                                    if (position == (codeText.Length-1)) 
                                    {
                                        parser.ParseToken(sb.ToString(), startIndex);
                                        sb.Clear();
                                        startIndex = position;
                                    }

                                }
                                break;
                            }
                    }
                }
                position++;
            } while (position < codeText.Length);

            AnalisadorSintatico();
        }

        private void AnalisadorSintatico()
        {
            int i = 0;

            
            MessageBox.Show(parser.ValidTokens.ElementAt(1).TokenType.ToString());
            
        }*/

    }
}
