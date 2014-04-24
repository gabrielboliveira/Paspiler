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
        private void PegaProximo()
        {
           //Pegar tipo do proximo token
        }
        private void erro()
        {
           //Ver a linha do erro e o motivo
        }
        private void constant()
        {
            if (tokentype == "String")
            {
                return;
            }
            else
            {
                if ((tokentype == "+") || (tokentype == "-"))
                {
                    if ((tokentype == "Coiden") || (tokentype == "Numb"))
                        return;
                    else
                        erro();
                }
                else
                    erro();
            }
        }

        
        private void sitype() 
        {
            if (tokentype == "Tyiden")
            {
                return;
            }
            else
            {
                if (tokentype == "(")
                {
                Loop :    PegaProximo();
                
                    if (tokentype == "Iden")
                    {
                        PegaProximo();
                        if (tokentype == ")")
                            return;
                        else
                        {
                           if (tokentype == ",")
                               goto Loop;
                           else
                               erro();
                        }                                      
                    }
                }
                else
                {
                    if (tokentype == "const")
                    {
                        constant();
                        PegaProximo();
                        if (tokentype == "..")
                            constant();
                        else
                            erro();
                    }
                    else
                        erro();
                } 
            }

        }
        private void term()
        {
            fator();
            PegaProximo();
            if ((tokentype == "*") || (tokentype == "/") || (tokentype == "div") || (tokentype == "mod") || (tokentype == "and"))
                term();
            else
                //TODO: TESTAR SE É LAMBDA SENAO
                erro();
        }      
        private void siexpr()
        {
            PegaProximo();
            if ((tokentype == "+") || (tokentype == "-"))
            {
            Termo:
                term();
                PegaProximo();
                if ((tokentype == "+") || (tokentype == "-") || (tokentype == "or"))
                    goto Termo;
                else
                    //TODO: TESTAR SE É LAMBDA SENAO
                    erro();
            }
            else
                erro();

        }
        private void expr()
        {
            siexpr();
            PegaProximo();
            if ((tokentype == "=") || (tokentype == "<") || (tokentype == ">") || (tokentype == "<>") || (tokentype == ">=") || (tokentype == "<=") || (tokentype == "in"))
                siexpr();
            else
                //TODO: TESTAR SE É LAMBDA SENAO
                erro();
        }
        private void statm()
        {
            PegaProximo();
            if (tokentype == "NUMB")
            {
                PegaProximo();
                if (tokentype == ":")
                    statm();
                else
                    erro();
            }
            else
            {
                if (tokentype == "VAIDEN")
                {
                    infipo();
                    PegaProximo();
                    if (tokentype == ":=")
                        expr();
                }
                else
                {
                    if (tokentype == "FUIDEN")
                    {
                        PegaProximo();
                        if (tokentype == ":=")
                            expr();
                    }
                    else
                    {
                        if (tokentype == "PRIDEN")
                        {
                            if (tokentype == "(")
                            {
                            Priden: PegaProximo();
                                if (tokentype == "PRIDEN")
                                {
                                Volta: PegaProximo();
                                    if (tokentype == ")")
                                        return;
                                    else
                                    {
                                        if (tokentype == ",")
                                            goto Priden;
                                    }
                                }
                                else
                                {
                                    expr();
                                    goto Volta;
                                }

                            }
                            else
                                //TODO:  VERIFICAR SE É LAMBIDA SENAO 
                                erro();
                        }
                        else
                        {
                            if (tokentype == "begin")
                            {
                            Statm:
                                statm();
                                PegaProximo();
                                if (tokentype == ";")
                                    goto Statm;
                                else
                                    if (tokentype == "end")
                                        return;
                                    else
                                        erro();
                            }
                            else
                            {
                                if (tokentype == "if")
                                {
                                    expr();
                                    PegaProximo();
                                    if (tokentype == "then")
                                    {
                                        statm();
                                        PegaProximo();
                                        if (tokentype == "else")
                                        {
                                            statm();
                                            return;
                                        }
                                        else
                                            //TODO: VERIFICAR SE É LAMBIDA SENAO 
                                            erro();
                                    }
                                }
                                else
                                {
                                    if (tokentype == "case")
                                    { }
                                    else
                                    {
                                        if (tokentype == "while")
                                        { }
                                        else
                                        {
                                            if (tokentype == "repeat")
                                            { }
                                            else
                                            {
                                                if (tokentype == "for")
                                                { }
                                                else
                                                {
                                                    if (tokentype == "with")
                                                    { }
                                                    else
                                                    {
                                                        if (tokentype == "goto")
                                                        {
                                                            PegaProximo();
                                                            if (tokentype == "NUMB")
                                                                return;
                                                            else
                                                                erro();
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

 
        }
    }
}
