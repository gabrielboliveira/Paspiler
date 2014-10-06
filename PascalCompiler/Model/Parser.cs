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
        private List<Token> allTokens = new List<Token>();

        internal List<Token> AllTokens
        {
            get { return allTokens; }
            set { allTokens = value; }
        }

        private int position = 0, startIndex = 0;

        private BindingList<Token> validTokens = new BindingList<Token>();

        /// <summary>
        /// Lista de tokens válidos - Bindable
        /// </summary>
        internal BindingList<Token> ValidTokens
        {
            get { return validTokens; }
            set { validTokens = value; }
        }

        private BindingList<Token> notValidTokens = new BindingList<Token>();

        /// <summary>
        /// Lista de tokens inválidos - Bindable
        /// </summary>
        internal BindingList<Token> NotValidTokens
        {
            get { return notValidTokens; }
            set { notValidTokens = value; }
        }

        public Parser() { }

        /// <summary>
        /// Limpa os objetos internos.
        /// </summary>
        public void Clear()
        {
            validTokens.Clear();
            notValidTokens.Clear();
        }

        /// <summary>
        /// Recebe um token, envia para <see cref="Token"/> validar e caso seja válido, salva em <see cref="ValidTokens"/>
        /// </summary>
        /// <param name="strToken">Token a ser validado.</param>
        private void ParseToken(string strToken, int startIndex)
        {
            if (strToken != "") 
            {
                // Manda processar o token
                Token token = Token.GetToken(strToken, startIndex);

                // Caso o token seja válido, adiciona a lista de tokens válidos e processados
                if (token.TokenType != Token.TokenTypeEnum.NonExistant)
                {
                    // MessageBox.Show(token.ToString());
                    validTokens.Add(token);
                    
                }
                // não é válido, adiciona a lista de tokens não válidos
                else
                {
                    notValidTokens.Add(token);
                }
                allTokens.Add(token);
            }
        }

        public void Execute(string code)
        {
            this.RecognizeTokens(code);
        }

        private void RecognizeTokens(string code)
        {
            // position guarda a posição que está dentro da string do programa (codeText)
            int position = 0, startIndex = 0;

            StringBuilder sb = new StringBuilder();

            string texto;

            char key, apostrofo;

            //coloquei em uma variavel poq no case nao tava dando certo
            apostrofo = Convert.ToChar(39);

            // Limpa o parser pra não repetir os erros
            this.Clear();

            do
            {
                key = code.ElementAt(position);
                if (key == apostrofo)
                {
                    this.ParseToken(sb.ToString(), startIndex);
                    sb.Clear();
                    startIndex = position;
                    texto = "";
                    position++;
                    while (code.ElementAt(position).ToString() != "'")
                    {
                        //vai concatenando em uma string tudo o que estiver entre os apóstrofos
                        texto += code.ElementAt(position);
                        position++;
                        if (position == code.Length)
                            break;

                    }
                    this.ParseToken("'" + texto + "'", startIndex);

                }
                else
                {
                    switch (key)
                    {

                        case ' ': // espaço

                        case '\n': // enter (carriage return)
                            {
                                // se for espaço ou enter (\r), deve mandar verificar o token
                                this.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                break;
                            }
                        case ';':
                            {
                                // Se for ponto-e-vírgula, deve-se adicionar o token armazenado e também o ";".
                                this.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                this.ParseToken(";", startIndex);
                                break;
                            }
                        case ':':
                            {
                                // se encontrar ":", verifica se o caractere em seguida é um "=", para
                                // separar ":=" de somente ":"
                                this.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                if ((code.ElementAt(position + 1)) == '=')
                                {
                                    this.ParseToken(":=", startIndex);
                                    // pula a posição em seguida, pois já foi verificado
                                    position++;
                                }
                                else
                                {
                                    this.ParseToken(":", startIndex);
                                }
                                break;
                            }
                        case '(':
                            {
                                // se encontrar "(", verifica se o caractere em seguida é um "*", para
                                // separar "(*" de somente "("
                                this.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                if ((code.ElementAt(position + 1)) == '*')
                                {
                                    // ignora tudo até encontrar o outro * e ) - comentario nao precisa ser identificado na tabela
                                    do
                                    {
                                        position++;

                                        if (code.ElementAt(position) == '*' && code.ElementAt(position + 1) == ')')
                                        {
                                            position++;
                                            break;
                                        }

                                    } while (position < (code.Length - 1));
                                }
                                else
                                {
                                    this.ParseToken("(", startIndex);
                                }
                                break;
                            }
                        case '*':
                            {
                                // se encontrar "*", verifica se o caractere em seguida é um ")", para
                                // separar "*)" de somente "*"
                                this.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                if ((code.ElementAt(position + 1)) == ')')
                                {
                                    this.ParseToken("*)", startIndex);
                                    position++;
                                }
                                else
                                {
                                    this.ParseToken("*", startIndex);
                                }
                                break;
                            }
                        case '/':
                            {
                                // se encontrar "/", verifica se o caractere em seguida é uma outra "/", para
                                // ignorar o comentário até o fim da linha
                                this.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                if ((code.ElementAt(position + 1)) == '/')
                                {
                                    //ignora toda a linha
                                    do
                                    {
                                        position++;
                                    } while (code.ElementAt(position) != '\n');
                                }
                                else
                                {
                                    this.ParseToken("/", startIndex);
                                }
                                break;
                            }
                        case '<':
                            {
                                // verifica se tem um ">" ou "=" logo em seguida
                                this.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                if ((code.ElementAt(position + 1)) == '=')
                                {
                                    this.ParseToken(key + "=", startIndex);
                                    position++;
                                }
                                else if ((code.ElementAt(position + 1)) == '>')
                                {
                                    this.ParseToken(key + ">", startIndex);
                                    position++;
                                }
                                else
                                {
                                    this.ParseToken(key.ToString(), startIndex);
                                }
                                break;
                            }
                        case '>':
                            {
                                // verifica se tem um "=" logo em seguida
                                this.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                if ((code.ElementAt(position + 1)) == '=')
                                {
                                    this.ParseToken(key + "=", startIndex);
                                    position++;
                                }
                                else
                                {
                                    this.ParseToken(key.ToString(), startIndex);
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
                                this.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                this.ParseToken(key.ToString(), startIndex);
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
                                    if (position == (code.Length - 1))
                                    {
                                        this.ParseToken(sb.ToString(), startIndex);
                                        sb.Clear();
                                        startIndex = position;
                                    }

                                }
                                break;
                            }
                    }
                }
                position++;
            } while (position < code.Length);
        }
    }
}
