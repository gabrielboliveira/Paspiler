using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PascalCompiler.Model
{
    class Sintatico
    {
        private int _index;

        private int _erros = 0;

        public int Erros
        {
            get { return _erros; }
            set { _erros = value; }
        }

        private Token _tokenAtual = null;

        private List<Token> _allTokens = null;

        private BackgroundWorker _worker;

        private BindingList<OutputMessage> _outputList = null;

        internal BindingList<OutputMessage> OutputList
        {
            get 
            {
                if (this._outputList == null)
                {
                    this._outputList = new BindingList<OutputMessage>();
                }
                    
                return _outputList; 
            }
            set { _outputList = value; }
        }

        public void Preparar(List<Token> tokens)
        {
            this._erros = 0;

            this._index = -1;

            this._allTokens = tokens;

            this._outputList.Clear();
        }

        public void Iniciar(BackgroundWorker worker)
        {
            UpdateOutput(new OutputMessage("Iniciando sintático...",
                        OutputTypeEnum.Message, null));
            this._worker = worker;
            this.Program();
            this.PegaProximo();
            if (this._tokenAtual != null)
            {
                Erro("Texto após 'end.'!");
            }
        }

        public void UpdateOutput(OutputMessage message)
        {
            Globals.Execute((Action)(() => //Invoke at UI thread
            { //run in UI thread
                this._outputList.Add(message);
            }), null);
        }

        private void Erro(string message)
        {
            _erros++;
            UpdateOutput(new OutputMessage("Erro fatal: " + message + ".",
                    OutputTypeEnum.Error, this._tokenAtual));

            // Paro de executar
            _worker.CancelAsync();
        }

        //REGRAS DOS MÉTODOS: Antes de retornar de um método eu pego o próximo, logo, não se pega o próximo no começo de nenhum método
        //Só nao pego o próximo no final do método se o último comando for outro método

        // REGRA DOS MÉTODOS MODIFICADA: Somente chamo o PegaProximo quando for realmente utilizar. 
        // Se eu não utilizar, devo chamar PegaAnterior para não perder o valor
        private void PegaProximo()
        {
            _index++;
            if (_index < _allTokens.Count)
            {
                _tokenAtual = _allTokens[_index];
            }
            else
            {
                _tokenAtual = null;
            }
        }

        private void PegaAnterior()
        {
            if (_index > 0)
            {
                _index--;
                _tokenAtual = _allTokens[_index];
            }
        }

        private void Constant()
        {
            this.PegaProximo();

            if(this._tokenAtual != null)
            {
                switch(this._tokenAtual.TokenType)
                {
                    case Token.TokenTypeEnum.Text:
                    case Token.TokenTypeEnum.Identifier: // certo seria COIDEN
                    case Token.TokenTypeEnum.IntegerNumber:
                    case Token.TokenTypeEnum.RealNumber:
                        {
                            return;
                        }
                    case Token.TokenTypeEnum.Sum:
                    case Token.TokenTypeEnum.Sub:
                        {
                            this.PegaProximo();
                            if(this._tokenAtual!= null &&
                                (this._tokenAtual.TokenType == Token.TokenTypeEnum.Identifier ||
                                this._tokenAtual.TokenType == Token.TokenTypeEnum.RealNumber ||
                                this._tokenAtual.TokenType == Token.TokenTypeEnum.IntegerNumber))
                            {
                                return;
                            }
                            else 
                            {
                                Erro("Esperado identificador válido ou número!");
                            }
                            break;
                        }
                    default:
                        {
                            Erro("Esperado texto, identificador válido, número, '+' ou '-'!");
                            break;
                        }
                } // Switch
            }
            else 
            {
                Erro("Esperado texto, identificador válido, número, '+' ou '-'!");
            }
        } // Constant

        private void Type()
        {
            this.PegaProximo();
            // não vou dar suporte a array e nem nada complicado... vamos deixar simples!
            if(this._tokenAtual != null && 
                (this._tokenAtual.TokenType == Token.TokenTypeEnum.Integer ||
                this._tokenAtual.TokenType == Token.TokenTypeEnum.Boolean ||
                this._tokenAtual.TokenType == Token.TokenTypeEnum.Real ||
                this._tokenAtual.TokenType == Token.TokenTypeEnum.String ||
                this._tokenAtual.TokenType == Token.TokenTypeEnum.Character))
            {
                return;
            }
            else
            {
                Erro("Esperado 'integer', 'boolean', 'char', 'real' ou 'string'!");
            }
        } // Type

        private void Factor()
        {
            this.PegaProximo();
            if(this._tokenAtual != null)
            {
                switch(this._tokenAtual.TokenType)
                {
                    case Token.TokenTypeEnum.Identifier:
                    case Token.TokenTypeEnum.IntegerNumber:
                    case Token.TokenTypeEnum.RealNumber:
                    case Token.TokenTypeEnum.Null:
                    case Token.TokenTypeEnum.Text:
                        {
                            return;
                        }
                    case Token.TokenTypeEnum.InitialParenthesis:
                        {
                            this.Expression();
                            this.PegaProximo();
                            if (this._tokenAtual != null &&
                                this._tokenAtual.TokenType != Token.TokenTypeEnum.FinalParenthesis)
                            {
                                Erro("Esperado ')'!");
                            }
                            return;
                        }
                    case Token.TokenTypeEnum.Not:
                        {
                            this.Factor();
                            return;
                        }
                    default:
                        {
                            Erro("Esperado número, identificador válido, 'null', texto, 'not'!");
                            break;
                        }
                }
            }
            else
            {
                Erro("Esperado número, identificador válido, 'null', texto, 'not'!");
            }
        } // Factor

        private void Term()
        {
            bool volta = true;
            do
            {
                this.Factor();
                this.PegaProximo();
                if(this._tokenAtual != null)
                {
                    switch(this._tokenAtual.TokenType)
                    {
                        case Token.TokenTypeEnum.Multiplier:
                        case Token.TokenTypeEnum.Divisor:
                        case Token.TokenTypeEnum.And:
                        case Token.TokenTypeEnum.Div:
                        case Token.TokenTypeEnum.Mod:
                            {
                                break;
                            }
                        default:
                            this.PegaAnterior();
                            return;
                    }
                }
                else
                {
                    return;
                }

            } while (volta);
        }      
        private void SiExpression()
        {
            this.PegaProximo();
            if(this._tokenAtual != null &&
                (this._tokenAtual.TokenType == Token.TokenTypeEnum.Sub ||
                this._tokenAtual.TokenType == Token.TokenTypeEnum.Sum))
            {
                // ok
            }
            else
            {
                this.PegaAnterior();
            }
            bool volta = true;
            do
            {
                this.Term();
                this.PegaProximo();
                if (this._tokenAtual != null)
                {
                    switch (this._tokenAtual.TokenType)
                    {
                        case Token.TokenTypeEnum.Sum:
                        case Token.TokenTypeEnum.Sub:
                        case Token.TokenTypeEnum.Or:
                            {
                                break;
                            }
                        default:
                            this.PegaAnterior();
                            return;
                    }
                }
                else
                {
                    return;
                }
            } while (volta);

        }
        private void Expression()
        {
            this.SiExpression();
            this.PegaProximo();
            if (this._tokenAtual != null)
            {
                switch(this._tokenAtual.TokenType)
                {
                    case Token.TokenTypeEnum.EqualsTo:
                    case Token.TokenTypeEnum.LessThan:
                    case Token.TokenTypeEnum.GreaterThan:
                    case Token.TokenTypeEnum.DifferentTo:
                    case Token.TokenTypeEnum.GreaterThanOrEqual:
                    case Token.TokenTypeEnum.LessThanOrEqual:
                    case Token.TokenTypeEnum.In:
                        {
                            this.SiExpression();
                            return;
                        }
                    default:
                        this.PegaAnterior();
                        return;
                }
            }
            else
            {
                return;
            }
        }

        private void Block()
        {
            bool volta, valido = false;
            // Label não se utiliza mais, removi
            this.PegaProximo();

            #region Comentado
            /* if (this.tokenAtual != null &&
            //    this.tokenAtual.TokenType == Token.TokenTypeEnum.Const)
            //{
            //    valido = true;
            //    this.PegaProximo();
            //    if (this.tokenAtual != null &&
            //        this.tokenAtual.TokenType == Token.TokenTypeEnum.Identifier)
            //    {
            //        volta = true;
            //        pega = true;
            //        do
            //        {
            //            this.PegaProximo();
            //            if (this.tokenAtual != null &&
            //                this.tokenAtual.TokenType == Token.TokenTypeEnum.EqualsTo)
            //            {
            //                this.Constant();
            //                this.PegaProximo();
            //                if (this.tokenAtual != null &&
            //                    this.tokenAtual.TokenType == Token.TokenTypeEnum.Semicolon)
            //                {
            //                    this.PegaProximo();
            //                    if (this.tokenAtual != null &&
            //                        this.tokenAtual.TokenType != Token.TokenTypeEnum.Identifier)
            //                    {
            //                        volta = false;
            //                        this.PegaAnterior(); // evitar perder o valor anterior
            //                    }
            //                }
            //                else
            //                {
            //                    Erro("Block");
            //                    volta = false;
            //                }
            //            }
            //            else
            //            {
            //                Erro("Block");
            //                volta = false;
            //            }
            //        } while (volta);

            //        this.PegaProximo();

            //    }
            //} Const */
            #endregion

            if (this._tokenAtual != null &&
                this._tokenAtual.TokenType == Token.TokenTypeEnum.Var)
            {
                valido = true;
                this.PegaProximo();
                if (this._tokenAtual != null &&
                    this._tokenAtual.TokenType == Token.TokenTypeEnum.Identifier)
                {
                    volta = true;
                    do
                    {
                        this.PegaProximo();
                        
                        if (this._tokenAtual != null)
                        {
                            switch(this._tokenAtual.TokenType)
                            {
                                case Token.TokenTypeEnum.Comma:
                                    {
                                        this.PegaProximo();
                                        if (this._tokenAtual != null &&
                                            this._tokenAtual.TokenType != Token.TokenTypeEnum.Identifier)
                                        {
                                            Erro("Esperado um identificador válido!");
                                            volta = false; // evitar loop eterno
                                        }
                                        break;
                                    }
                                case Token.TokenTypeEnum.Colon:
                                    {
                                        this.Type();
                                        this.PegaProximo();
                                        if (this._tokenAtual != null &&
                                            this._tokenAtual.TokenType == Token.TokenTypeEnum.Semicolon)
                                        {
                                            this.PegaProximo();
                                            if (this._tokenAtual != null &&
                                                this._tokenAtual.TokenType != Token.TokenTypeEnum.Identifier)
                                            {
                                                volta = false; // evitar loop eterno
                                                this.PegaAnterior();  // evitar perder o valor anterior
                                            }
                                        }
                                        else
                                        {
                                            volta = false; // evitar loop eterno
                                            Erro("Esperado ';'!");
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        volta = false;
                                        Erro("Esperado ',' ou ':'!");
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            volta = false;
                            Erro("Esperado ',' ou ':'!");
                        }
                    } while (volta);

                    this.PegaProximo();
                }
                else
                {
                    Erro("Esperado identificador válido!");
                }
            } // Var

            if (this._tokenAtual != null &&
                this._tokenAtual.TokenType == Token.TokenTypeEnum.Begin)
            {
                valido = true;
                volta = true;
                do
                { // loop eterno
                    this.Statement();
                    this.PegaProximo();
                    if (this._tokenAtual != null)
                    {
                        if(this._tokenAtual.TokenType == Token.TokenTypeEnum.Semicolon)
                        {
                            volta = true;
                            continue;
                        }
                        else if (this._tokenAtual.TokenType == Token.TokenTypeEnum.End)
                        {
                            return;
                        }
                        else
                        {
                            volta = false;
                            Erro("Esperado ';' ou 'end'!");
                        }
                    }
                    else
                    {
                        volta = false;
                        Erro("Esperado ';' ou 'end'!");
                    }
                } while (volta);
            } // Begin

            if(!valido)
            {
                Erro("Esperado 'var' ou 'begin'!");
            }
        }
        private void Statement()
        {
            bool volta;
            this.PegaProximo();
            if (this._tokenAtual != null)
            {
                switch (this._tokenAtual.TokenType)
                {
                    case Token.TokenTypeEnum.Identifier: // Correto seria VAIDEN
                        {
                            this.PegaProximo();
                            if (this._tokenAtual != null &&
                                this._tokenAtual.TokenType == Token.TokenTypeEnum.Attribution)
                            {
                                this.Expression();
                            }
                            else
                            {
                                Erro("Esperado ':='!");
                            }
                            return;
                        }
                    case Token.TokenTypeEnum.Begin:
                        {
                            volta = true;
                            do
                            {
                                this.Statement();
                                this.PegaProximo();
                                if (this._tokenAtual != null)
                                {
                                    if (this._tokenAtual.TokenType == Token.TokenTypeEnum.Semicolon)
                                    {
                                        // repete
                                        volta = true;
                                        continue;
                                    }
                                    else if (this._tokenAtual.TokenType == Token.TokenTypeEnum.End)
                                    {
                                        this.PegaAnterior();
                                        return;
                                    }
                                    else
                                    {
                                        Erro("Esperado ';' ou 'end'!");
                                        return;
                                    }
                                }
                                else
                                {
                                    Erro("Esperado ';' ou 'end'!");
                                    return;
                                }
                            } while (volta);
                            return;
                        }
                    case Token.TokenTypeEnum.If:
                        {
                            this.Expression();
                            this.PegaProximo();
                            if (this._tokenAtual != null && 
                                this._tokenAtual.TokenType == Token.TokenTypeEnum.Then)
                            {
                                this.Statement();
                                this.PegaProximo();
                                if (this._tokenAtual != null && 
                                    this._tokenAtual.TokenType == Token.TokenTypeEnum.Else)
                                {
                                    this.Statement();
                                }
                                return;
                            }
                            else
                            {
                                Erro("Esperado 'then'!");
                            }
                            return;
                        }
                    case Token.TokenTypeEnum.While:
                        {
                            this.Expression();
                            this.PegaProximo();
                            if (this._tokenAtual != null &&
                                this._tokenAtual.TokenType == Token.TokenTypeEnum.Do)
                            {
                                this.Statement();
                            }
                            else
                            {
                                Erro("Esperado 'do'!");
                            }
                            return;
                        }
                    case Token.TokenTypeEnum.Repeat:
                        {
                            volta = true;
                            do
                            {
                                this.Statement();
                                this.PegaProximo();
                                if (this._tokenAtual != null &&
                                    this._tokenAtual.TokenType == Token.TokenTypeEnum.Semicolon)
                                {
                                    continue;
                                }
                                else if (this._tokenAtual != null &&
                                    this._tokenAtual.TokenType == Token.TokenTypeEnum.Until)
                                {
                                    this.Expression();
                                    return;
                                }
                                else
                                {
                                    Erro("Esperado ';' ou 'until'!");
                                }
                            } while (volta);
                            return;
                        }
                    case Token.TokenTypeEnum.For:
                        {
                            this.PegaProximo();
                            if (this._tokenAtual != null &&
                                this._tokenAtual.TokenType == Token.TokenTypeEnum.Identifier) // Correto seria VAIDEN
                            {
                                this.PegaProximo();
                                if (this._tokenAtual != null &&
                                    this._tokenAtual.TokenType == Token.TokenTypeEnum.Attribution)
                                {
                                    this.Expression();
                                    this.PegaProximo();
                                    if (this._tokenAtual != null &&
                                        (this._tokenAtual.TokenType == Token.TokenTypeEnum.To ||
                                        this._tokenAtual.TokenType == Token.TokenTypeEnum.DownTo))
                                    {
                                        this.Expression();
                                        this.PegaProximo();
                                        if (this._tokenAtual != null &&
                                            (this._tokenAtual.TokenType == Token.TokenTypeEnum.Do))
                                        {
                                            this.Statement();
                                        }
                                        else
                                        {
                                            Erro("Esperado 'do'!");
                                        }
                                    }
                                    else
                                    {
                                        Erro("Esperado 'to' ou 'downto'!");
                                    }
                                }
                                else
                                {
                                    Erro("Esperado ':='!");
                                }
                            }
                            else
                            {
                                Erro("Esperado identificador válido!");
                            }
                            return;
                        }
                    default:
                        {
                            this.PegaAnterior();
                            return;
                        }
                }
            }
            else
            {
                Erro("Fim de programa inesperado!");
            }


        }
        private void Program()
        {
            this.PegaProximo();
            if (this._tokenAtual != null &&
                this._tokenAtual.TokenType == Token.TokenTypeEnum.Program)
            {
                this.PegaProximo();
                if (this._tokenAtual != null &&
                    this._tokenAtual.TokenType == Token.TokenTypeEnum.Identifier)
                {
                    this.PegaProximo();

                    // OBS: removido a parte do parênteses, pois não se utiliza mais
                    if (this._tokenAtual != null &&
                        this._tokenAtual.TokenType == Token.TokenTypeEnum.Semicolon)
                    {
                        this.Block();
                    }
                    else
                    {
                        Erro("Esperado ';'!");
                    } // if Semicolon
                }
                else
                {
                    Erro("Esperado identificador válido!");
                } // if Identifier

            } // if Program
            else
            {
                Erro("O programa deve começar com 'program'.");
            }
        }

    }
}