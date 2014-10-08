using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PascalCompiler.Model
{
    /// <summary>
    /// Recebe um token, manda para <see cref="Token"/> fazer a validação, e guarda uma lista de tokens válidos.
    /// </summary>
    class Parser
    {
        private string _code = "";

        private BackgroundWorker _worker, _wQueue;

        private bool _repeatQueueWorker = true, _awaitQueue = true;

        private Sintatico _sintatico = new Sintatico();

        private List<Token> _allTokens = new List<Token>();

        internal List<Token> AllTokens
        {
            get { return _allTokens; }
            private set { _allTokens = value; }
        }

        private List<Token> _validTokensList = new List<Token>();
        private BindingList<Token> _validTokens = new BindingList<Token>();

        /// <summary>
        /// Lista de tokens válidos - Bindable
        /// </summary>
        internal BindingList<Token> ValidTokens
        {
            get { return _validTokens; }
            private set { _validTokens = value; }
        }

        private List<Token> _notValidTokensList = new List<Token>();
        private BindingList<Token> _notValidTokens = new BindingList<Token>();

        /// <summary>
        /// Lista de tokens inválidos - Bindable
        /// </summary>
        internal BindingList<Token> NotValidTokens
        {
            get { return _notValidTokens; }
            private set { _notValidTokens = value; }
        }

        private BindingList<OutputMessage> _output;

        internal BindingList<OutputMessage> Output
        {
            get { return _output; }
            private set { _output = value; }
        }

        public Parser() 
        {
            this.Output = this._sintatico.OutputList;
        }

        /// <summary>
        /// Limpa os objetos internos.
        /// </summary>
        public void Clear()
        {
            _allTokens.Clear();
            _validTokensList.Clear();
            _validTokens.Clear();
            _notValidTokensList.Clear();
            _notValidTokens.Clear();
            _sintatico.OutputList.Clear();
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
                Token token = Token.GetToken(strToken, startIndex - strToken.Length);

                // Caso o token seja válido, adiciona a lista de tokens válidos e processados
                if (token.TokenType != Token.TokenTypeEnum.NonExistant)
                {
                    // MessageBox.Show(token.ToString());

                    //_qValidTokens.Enqueue(token);
                    _validTokensList.Add(token);

                }
                // não é válido, adiciona a lista de tokens não válidos
                else
                {
                    //UpdateTokens(_notValidTokens, token);
                    //_qNotValidTokens.Enqueue(token);

                    _notValidTokensList.Add(token);
                }
                _allTokens.Add(token);
            }
        }

        public void UpdateTokensList()
        {
            foreach (Token t in _validTokensList)
            {
                Globals.Execute((Action)(() => //Invoke at UI thread
                { //run in UI thread

                    ValidTokens.Add(t);

                }), null);
            }
            foreach(Token t in _notValidTokensList)
            {
                Globals.Execute((Action)(() => //Invoke at UI thread
                { //run in UI thread

                    NotValidTokens.Add(t);

                }), null);
            }
        }

        public void Execute(string code)
        {
            this._sintatico.UpdateOutput(new OutputMessage("Iniciando analisador léxico...",
                        OutputTypeEnum.Message, null));

            this._code = code;

            // Limpa o parser pra não repetir os erros
            this.Clear();

            this._sintatico.Preparar(AllTokens);

            if (this._worker == null)
            {
                this._worker = new BackgroundWorker();

                this._worker.WorkerSupportsCancellation = true;

                // Utilizo o Worker para não travar a janela principal
                this._worker.DoWork += new DoWorkEventHandler(DoWorkHandler);

                this._worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DoWorkCompletedHandler);
            }

            if (_worker.IsBusy != true)
            {
                _worker.RunWorkerAsync();
            }

        }

        private void DoWorkCompletedHandler(object sender, RunWorkerCompletedEventArgs e)
        {
            this._sintatico.UpdateOutput(new OutputMessage("Finalizado sintático com " + this._sintatico.Erros.ToString() + " erros.",
                        OutputTypeEnum.Message, null));
            UpdateTokensList();
        }

        private void DoWorkHandler(object sender, DoWorkEventArgs e)
        {
            this.MontarTabelaLexico();

            this._sintatico.UpdateOutput(
                new OutputMessage("Analisador Léxico finalizado. Foram encontrados " +
                    AllTokens.Count + " tokens (" + ValidTokens.Count + " válidos e " +
                    NotValidTokens.Count + " inválidos).",
                    OutputTypeEnum.Message, null));

            foreach (Token t in NotValidTokens)
            {
                this._sintatico.UpdateOutput(
                    new OutputMessage("Erro fatal: Token inválido/não reconhecido: '" +
                        t.Value + ".",
                        OutputTypeEnum.Message, null));
            }
            if (NotValidTokens.Count == 0)
            {
                this._sintatico.Iniciar(_worker);
            }

        }

        private void MontarTabelaLexico()
        {
            // position guarda a posição que está dentro da string do programa (codeText)
            int position = 0, startIndex = 0;

            StringBuilder sb = new StringBuilder();

            string texto;

            char key, apostrofo;

            //coloquei em uma variavel poq no case nao tava dando certo
            apostrofo = Convert.ToChar(39);

            do
            {
                key = this._code.ElementAt(position);
                if (key == apostrofo)
                {
                    this.ParseToken(sb.ToString(), startIndex);
                    sb.Clear();
                    startIndex = position;
                    texto = "";
                    position++;
                    while (this._code.ElementAt(position).ToString() != "'")
                    {
                        //vai concatenando em uma string tudo o que estiver entre os apóstrofos
                        texto += this._code.ElementAt(position);

                        position++;
                        if (position == this._code.Length)
                            break;
                    }
                    this.ParseToken("'" + texto + "'", startIndex);

                }
                else
                {
                    switch (key)
                    {

                        case ' ': // espaço
                            {
                                // se for espaço ou enter (\r), deve mandar verificar o token
                                this.ParseToken(sb.ToString(), startIndex);
                                sb.Clear();
                                startIndex = position;
                                break;
                            }
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
                                if ((this._code.ElementAt(position + 1)) == '=')
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
                                if ((this._code.ElementAt(position + 1)) == '*')
                                {
                                    // ignora tudo até encontrar o outro * e ) - comentario nao precisa ser identificado na tabela
                                    do
                                    {
                                        position++;

                                        if (this._code.ElementAt(position) == '*' && this._code.ElementAt(position + 1) == ')')
                                        {
                                            position++;
                                            break;
                                        }

                                    } while (position < (this._code.Length - 1));
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
                                if ((this._code.ElementAt(position + 1)) == ')')
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
                                if ((this._code.ElementAt(position + 1)) == '/')
                                {
                                    //ignora toda a linha
                                    do
                                    {
                                        position++;
                                    } while (this._code.ElementAt(position) != '\n');
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
                                if ((this._code.ElementAt(position + 1)) == '=')
                                {
                                    this.ParseToken(key + "=", startIndex);
                                    position++;
                                }
                                else if ((this._code.ElementAt(position + 1)) == '>')
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
                                if ((this._code.ElementAt(position + 1)) == '=')
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
                                    if (position == (this._code.Length - 1))
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
            } while (position < this._code.Length);
        }
    }
}
