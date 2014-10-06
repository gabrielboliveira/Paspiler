using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler.Model
{
    class Sintatico
    {
        string tokentype = "";

        private int index = -1;

        private Token tokenAtual = null;

        private List<Token> allTokens = null;

        public void Iniciar(List<Token> tokens)
        {
            this.allTokens = tokens;
            this.Program();
        }

        //REGRAS DOS MÉTODOS: Antes de retornar de um método eu pego o próximo, logo, não se pega o próximo no começo de nenhum método
        //Só nao pego o próximo no final do método se o último comando for outro método

        // REGRA DOS MÉTODOS MODIFICADA: Somente chamo o PegaProximo quando for realmente utilizar...
        private void PegaProximo()
        {
            index++;
            if (allTokens.Count <= index)
            {
                tokenAtual = allTokens[index];
            }
            else
            {
                tokenAtual = null;
            }
        }

        private void erro(String erro)
        {
           //Ver a linha do erro e o motivo
        }

        private void Constant()
        {
            this.PegaProximo();

            if(this.tokenAtual != null)
            {
                switch(this.tokenAtual.TokenType)
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
                            if(this.tokenAtual!= null &&
                                (this.tokenAtual.TokenType == Token.TokenTypeEnum.Identifier ||
                                this.tokenAtual.TokenType == Token.TokenTypeEnum.RealNumber ||
                                this.tokenAtual.TokenType == Token.TokenTypeEnum.IntegerNumber))
                            {
                                return;
                            }
                            else 
                            {
                                // erro
                            }
                            break;
                        }
                    default:
                        {
                            // erro
                            break;
                        }
                } // Switch
            }
            else 
            {
                // erro
            }
        }

        private void sitype() 
        {
            bool volta;
            if (tokentype == "TYIDEN")
            {
                PegaProximo();
                return;
            }
            else
            {
                if (tokentype == "(")
                {
                    PegaProximo();
                    volta = true;
                    while (volta)
                    {
                        if (tokentype == "IDEN")
                        {
                            PegaProximo();
                            if (tokentype == ")")
                            {
                                //so para garantir volta = false
                                volta = false;
                                PegaProximo();
                                return;
                            }                               
                            else
                            {
                                if (tokentype == ",")
                                    PegaProximo();
                                else
                                {
                                    erro("Esperava ) ou , obteve " + tokentype);
                                    volta = false;
                                }                                    
                            }
                        } 
                    }// fim while 
                }
                else
                {
                    PegaProximo();
                    Constant();
                    if (tokentype == "..")
                    {
                        PegaProximo();
                        Constant();
                        // so pego o proximo se antes de retornar se nao o termo anterior nao é função
                        return;
                    }
                    else
                        erro("Esperava .. obteve " + tokentype);
                } 
            }

        }

        private void Type()
        {
            this.PegaProximo();
            // não vou dar suporte a array e nem nada complicado... vamos deixar simples!
            if(this.tokenAtual != null && 
                (this.tokenAtual.TokenType == Token.TokenTypeEnum.Integer ||
                this.tokenAtual.TokenType == Token.TokenTypeEnum.Boolean ||
                this.tokenAtual.TokenType == Token.TokenTypeEnum.String ||
                this.tokenAtual.TokenType == Token.TokenTypeEnum.Character))
            {
                return;
            }
            else
            {
                // erro
            }
        }
        private void filist()
        {
            //O livro está com manchas.. não consegui ver o q eram alguns símbolos
        }
        private void casefilist()
        {//como nao tem referências a esse metodo ainda nao conferi
            bool volta = true;
            while (volta)
            {
                if ((tokentype == "String") || (tokentype == "COIDEN") || (tokentype == "NUMB")||(tokentype == "+")||(tokentype == "-"))
                {
                    PegaProximo();
                    if ((tokentype == "+") || (tokentype == "-"))
                    {
                        if ((tokentype != "COIDEN") && (tokentype != "NUMB"))
                            erro("erro");
                        else
                            PegaProximo();
                    }                                       
                    if (tokentype == ",")
                        PegaProximo();
                    if (tokentype == ":")
                    {
                        PegaProximo();
                        if (tokentype == "(")
                        {
                            PegaProximo();
                            filist();
                            PegaProximo();
                            if (tokentype == ")")
                            {
                                if (tokentype == ";")
                                    PegaProximo();
                                else
                                {
                                    volta = false;
                                    //Testar esse com outro if
                                }
                            }

                        }
                    }
                }
               else
                {
                    if (tokentype == ";")
                        PegaProximo();
                    else
                        volta = false;
                        //testar esse com outro if
                 }
                        
            }

        }
        private void infipo()
        {
            bool volta, voltainicio;
            
            voltainicio = true;
            PegaProximo();
            while (voltainicio)
            {
                if (tokentype == "[")
                {
                    volta = true;
                    while (volta)
                    {
                        PegaProximo();
                        Expression();
                        if (tokentype == ",")
                            PegaProximo();
                        else
                            if (tokentype == "]")
                            {
                                volta = false;
                                PegaProximo();
                            }
                            else
                            {
                                volta = false;                                
                                erro("Esperava ] obteve " + tokentype);
                            }
                    }                    
                }
                else
                    if (tokentype == ".")
                    {
                        PegaProximo();
                        if (tokentype == "FIIDEN")
                            PegaProximo();
                    }
                    else
                    {
                        voltainicio = false;
                        return;
                    }
            }           
        }
        private void Factor()
        {
            this.PegaProximo();
            if(this.tokenAtual != null)
            {
                switch(this.tokenAtual.TokenType)
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
                            if (this.tokenAtual != null &&
                                this.tokenAtual.TokenType != Token.TokenTypeEnum.FinalParenthesis)
                            {
                                // erro
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
                            // erro
                            break;
                        }
                }
            }
            else
            {
                // erro
            }
        }
        private void Term()
        {
            bool volta = true;
            do
            {
                this.Factor();
                this.PegaProximo();
                if(this.tokenAtual != null)
                {
                    switch(this.tokenAtual.TokenType)
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
            if(this.tokenAtual != null &&
                (this.tokenAtual.TokenType == Token.TokenTypeEnum.Sub ||
                this.tokenAtual.TokenType == Token.TokenTypeEnum.Sum))
            {
                // ok
            }
            else
            {
                // erro
            }
            bool volta = true;
            do
            {
                this.Term();
                this.PegaProximo();
                if (this.tokenAtual != null)
                {
                    switch (this.tokenAtual.TokenType)
                    {
                        case Token.TokenTypeEnum.Sum:
                        case Token.TokenTypeEnum.Sub:
                        case Token.TokenTypeEnum.Or:
                            {
                                break;
                            }
                        default:
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
            if (this.tokenAtual != null)
            {
                switch(this.tokenAtual.TokenType)
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
                        return;
                }
            }
            else
            {
                return;
            }
               
        }
        private void palist()
        {//nao conferi pois nao tinha referencias
 
        }
        private void Block()
        {
            bool volta, pega = true;
            // Label não se utiliza mais, removi
            this.PegaProximo();

            if (this.tokenAtual != null &&
                this.tokenAtual.TokenType == Token.TokenTypeEnum.Const)
            {
                this.PegaProximo();
                if (this.tokenAtual != null &&
                    this.tokenAtual.TokenType == Token.TokenTypeEnum.Identifier)
                {
                    volta = true;
                    pega = true;
                    do
                    {
                        this.PegaProximo();
                        if (this.tokenAtual != null &&
                            this.tokenAtual.TokenType == Token.TokenTypeEnum.EqualsTo)
                        {
                            this.Constant();
                            this.PegaProximo();
                            if (this.tokenAtual != null &&
                                this.tokenAtual.TokenType == Token.TokenTypeEnum.Semicolon)
                            {
                                this.PegaProximo();
                                if (this.tokenAtual != null &&
                                    this.tokenAtual.TokenType != Token.TokenTypeEnum.Identifier)
                                {
                                    volta = false;
                                    pega = false; // evitar perder o valor anterior
                                }
                            }
                            else
                            {
                                // erro
                                volta = false;
                            }
                        }
                        else
                        {
                            // erro
                            volta = false;
                        }
                    } while (volta);
                    
                    if(pega)
                        this.PegaProximo();

                }
            } // Const

            if (this.tokenAtual != null &&
                this.tokenAtual.TokenType == Token.TokenTypeEnum.Var)
            {
                this.PegaProximo();
                if (this.tokenAtual != null &&
                    this.tokenAtual.TokenType == Token.TokenTypeEnum.Identifier)
                {
                    volta = true;
                    pega = true;
                    do
                    {
                        this.PegaProximo();
                        
                        if (this.tokenAtual != null)
                        {
                            switch(this.tokenAtual.TokenType)
                            {
                                case Token.TokenTypeEnum.Comma:
                                    {
                                        this.PegaProximo();
                                        if (this.tokenAtual != null &&
                                            this.tokenAtual.TokenType != Token.TokenTypeEnum.Identifier)
                                        {
                                            // erro
                                            volta = false; // evitar loop eterno
                                        }
                                        break;
                                    }
                                case Token.TokenTypeEnum.Colon:
                                    {
                                        this.Type();
                                        this.PegaProximo();
                                        if (this.tokenAtual != null &&
                                            this.tokenAtual.TokenType == Token.TokenTypeEnum.Semicolon)
                                        {
                                            this.PegaProximo();
                                            if (this.tokenAtual != null &&
                                                this.tokenAtual.TokenType != Token.TokenTypeEnum.Identifier)
                                            {
                                                volta = false; // evitar loop eterno
                                                pega = false;  // evitar perder o valor anterior
                                            }
                                        }
                                        else
                                        {
                                            volta = false; // evitar loop eterno
                                            // erro
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        volta = false;
                                        // erro
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            volta = false;
                            // erro
                        }
                    } while (volta);

                    if(pega)
                        this.PegaProximo();
                }
            } // Var

            if (this.tokenAtual != null &&
                this.tokenAtual.TokenType == Token.TokenTypeEnum.Begin)
            {
                volta = true;
                do
                {
                    this.Statement(); //----------------------------------------
                    this.PegaProximo();
                    if (this.tokenAtual != null)
                    {
                        if(this.tokenAtual.TokenType == Token.TokenTypeEnum.Semicolon)
                        { 

                        } //----------------------------------------
                        else if (this.tokenAtual.TokenType == Token.TokenTypeEnum.End)
                        {
                            return;
                        }
                        else
                        {
                            // erro
                        }
                    }
                    else
                    {
                        // erro
                    }
                } while (volta);
            } // Begin
        }
        private void Statement()
        {
            bool volta;
            this.PegaProximo();
            if (this.tokenAtual != null)
            {
                switch (this.tokenAtual.TokenType)
                {
                    case Token.TokenTypeEnum.Identifier: // Correto seria VAIDEN
                        {
                            this.PegaProximo();
                            if (this.tokenAtual != null &&
                                this.tokenAtual.TokenType == Token.TokenTypeEnum.Attribution)
                            {
                                this.Expression();
                            }
                            else
                            {
                                // erro
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
                                if (this.tokenAtual != null)
                                {
                                    if (this.tokenAtual.TokenType == Token.TokenTypeEnum.Semicolon)
                                    {
                                        // repete
                                        continue;
                                    }
                                    else if (this.tokenAtual.TokenType == Token.TokenTypeEnum.End)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        // erro
                                        return;
                                    }
                                }
                                else
                                {
                                    // erro
                                    return;
                                }
                            } while (volta);
                            return;
                        }
                    case Token.TokenTypeEnum.If:
                        {
                            this.Expression();
                            this.PegaProximo();
                            if (this.tokenAtual != null && 
                                this.tokenAtual.TokenType == Token.TokenTypeEnum.Then)
                            {
                                this.Statement();
                                this.PegaProximo();
                                if (this.tokenAtual != null && 
                                    this.tokenAtual.TokenType == Token.TokenTypeEnum.Else)
                                {
                                    this.Statement();
                                }
                                return;
                            }
                            else
                            {
                                // erro
                            }
                            return;
                        }
                    case Token.TokenTypeEnum.While:
                        {
                            this.Expression();
                            this.PegaProximo();
                            if (this.tokenAtual != null &&
                                this.tokenAtual.TokenType == Token.TokenTypeEnum.Do)
                            {
                                this.Statement();
                            }
                            else
                            {
                                // erro
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
                                if (this.tokenAtual != null &&
                                    this.tokenAtual.TokenType == Token.TokenTypeEnum.Semicolon)
                                {
                                    continue;
                                }
                                else if (this.tokenAtual != null &&
                                    this.tokenAtual.TokenType == Token.TokenTypeEnum.Until)
                                {
                                    this.Expression();
                                    return;
                                }
                            } while (volta);
                            return;
                        }
                    case Token.TokenTypeEnum.For:
                        {
                            / --------------------
                            return;
                        }
                }
            }


        }
        private void Program()
        {
            this.PegaProximo();
            if (this.tokenAtual != null &&
                this.tokenAtual.TokenType == Token.TokenTypeEnum.Program)
            {
                this.PegaProximo();
                if (this.tokenAtual != null &&
                    this.tokenAtual.TokenType == Token.TokenTypeEnum.Identifier)
                {
                    this.PegaProximo();

                    // OBS: removido a parte do parênteses, pois não se utiliza mais
                    if (this.tokenAtual != null &&
                        this.tokenAtual.TokenType == Token.TokenTypeEnum.Semicolon)
                    {
                        this.Block();
                    }
                    else
                    {
                        // erro
                    } // if Semicolon
                }
                else
                {
                    // erro
                } // if Identifier

            } // if Program
        }

    }
}
