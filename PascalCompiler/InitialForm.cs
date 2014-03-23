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
                case (Keys.F2):
                    {
                        OpenFileDialog theDialog = new OpenFileDialog();
                        theDialog.Title = "Abrir código fonte Pascal";
                        theDialog.Filter = "Código Fonte Pascal|*.pas";
                        theDialog.InitialDirectory = @"C:\";
                        if (theDialog.ShowDialog() == DialogResult.OK)
                        {
                            string filename = theDialog.FileName;

                            string[] filelines = File.ReadAllLines(filename);

                            _codeTextBox.Text = string.Join("\n", filelines);
                        }
                        return true;
                    }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void _codeTextBox_KeyPress(object sender, KeyPressEventArgs keyPressed)
        {
        /*    switch (keyPressed.KeyChar)
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
                case ':':
                case '(':
                    {
                        parser.ParseToken(sb.ToString());
                        sb.Clear();
                        sb.Append(keyPressed.KeyChar);
                        break;
                    }
                case '*':
                    {
                        try
                        {
                            //se nao for comentario, entao salvo só *
                            if ((sb.ToString().ElementAt((sb.Length - 1))) != '(')
                            {
                                parser.ParseToken(sb.ToString());
                                sb.Clear();
                                parser.ParseToken(keyPressed.KeyChar.ToString());
                            }
                            else
                            {
                                parser.ParseToken("(*");
                                sb.Clear();
                            }
                        }
                        catch (Exception e) { sb.Append(keyPressed.KeyChar); }
                        break;
                    }
                case ')':
                    {
                        try
                        {
                            //se nao for comentario, entao salvo só *
                            if ((sb.ToString().ElementAt((sb.Length - 1))) != '*')
                            {
                                parser.ParseToken(sb.ToString());
                                sb.Clear();
                                parser.ParseToken(keyPressed.KeyChar.ToString());
                            }
                            else
                            {
                                parser.ParseToken("*)");
                                sb.Clear();
                            }
                        }
                        catch (Exception e) {
                            sb.Append(keyPressed.KeyChar); 
                        }
                        break; 
                    }
                case '[':
                case ']':
                case '{':
                case '}':
                case '+':
                case '-':
                case '/':                
                case '#':     
                case ',':
                    {
                        parser.ParseToken(sb.ToString());
                        sb.Clear();
                        parser.ParseToken(keyPressed.KeyChar.ToString());                        
                        break;
                    }
                case '=':
                    {
                        try
                        {
                            if ((sb.ToString().ElementAt((sb.Length - 1))) == ':')
                            {
                                parser.ParseToken(":=");
                                sb.Clear();
                            }else
                            {
                                if ((sb.ToString().ElementAt((sb.Length - 1))) == '=')
                                {
                                    parser.ParseToken("==");
                                    sb.Clear();
                                }
                                else
                                {
                                    //Se não tiver ":" ou "=" é poq é o primeiro "=" adiciono o token que vem antes
                                    parser.ParseToken(sb.ToString());
                                    sb.Clear();
                                    sb.Append(keyPressed.KeyChar);                                    
                                }                                   
                            }  
                            break;
                        }
                        catch (Exception e) 
                        { 
                            //Se não tiver nada antes do "=" salva só o "="
                            sb.Append(keyPressed.KeyChar); 
                        }
                        break;
                    }
                default:
                    {
                        // Caso a tecla apertada seja um número, letra, ou pontuação válida, adiciona ao sb para validar o token
                        if (Char.IsLetter(keyPressed.KeyChar) || Char.IsSeparator(keyPressed.KeyChar) || Char.IsPunctuation(keyPressed.KeyChar) || Char.IsNumber(keyPressed.KeyChar)) 
                        {
                            try
                            {
                                //verifico se o caracter anterior é parenteses poq pode ou nao ser um comentario
                                if ((sb.ToString().ElementAt((sb.Length - 1))) == '(')
                                {
                                    parser.ParseToken("(");
                                    sb.Clear();
                                    sb.Append(keyPressed.KeyChar);
                                }
                                else
                                {
                                    if ((sb.ToString().ElementAt((sb.Length - 1))) == ':')
                                    {
                                        parser.ParseToken(":");
                                        sb.Clear();
                                        sb.Append(keyPressed.KeyChar);
                                    }
                                    else
                                        sb.Append(keyPressed.KeyChar);
                                }
                                
                            }
                            catch (Exception e) { sb.Append(keyPressed.KeyChar); }
    
                        }
                            
                        break;
                    }
            }*/

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
            // Limpa o parser pra não repetir os erros
            parser.Clear();
            // position guarda a posição que está dentro da string do programa (codeText)
            int position = 0, startIndex = 0;
            char key, apostrofo;
            //coloquei em uma variavel poq no case nao tava dando certo
            apostrofo = Convert.ToChar(39);
            string codeText = _codeTextBox.Text;
           
            do
            {
                key = codeText.ElementAt(position);
                if (key == apostrofo)
                {
                    parser.ParseToken(sb.ToString(), startIndex);
                    sb.Clear();
                    startIndex = position;
                    parser.ParseToken("'", startIndex);
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
                                    parser.ParseToken("(*", startIndex);
                                    // pula a posição em seguida, pois já foi verificado
                                    position++;
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
                        

            // Pensamentos da madrugada:

            // utilizar Char.IsPunctuation(Char var) no while para verificar se é uma pontuação (, [, {, etc.
            // eu acho que isso dá certo
            // usar isso pra ficar mais fácil separar identificadores/números colados, sem espaço entre eles...
        }

    }
}
