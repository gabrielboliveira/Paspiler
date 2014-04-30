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
        private void type()
        {
            PegaProximo();
            if (tokentype == "TYIDEN")
            {
                return;
            }
            if ((tokentype == "packed")||(tokentype == "array")||(tokentype == "file")||(tokentype == "set")||(tokentype == "record"))
            {
                if (tokentype == "packed")
                {
                    PegaProximo();
                    if ((tokentype == "array") || (tokentype == "file") || (tokentype == "set") || (tokentype == "record"))
                    {
                        PegaProximo();
                    }
                    else
                        sitype();
                }
                else
                {
                    if (tokentype == "array")
                    {
                        PegaProximo();
                        if (tokentype == "[")
                        {
                            bool volta = true;
                            while (volta)
                            {
                                sitype();
                                PegaProximo();
                                if (tokentype == ",")
                                    PegaProximo();
                                else
                                    if (tokentype == "]")
                                    {
                                        volta = false;
                                        PegaProximo();
                                        if (tokentype == "of")
                                        {
                                            type();
                                            return;
                                        }
                                        else
                                            erro();
                                    }
                                    else
                                        erro();
                            }

                        }
                    }
                    else 
                        if (tokentype == "file")
                        {
                            PegaProximo();
                            if (tokentype == "of")
                            {
                                type();
                                return;
                            }
                            else
                                erro();
                        }
                        else
                            if (tokentype == "set")
                            {
                                PegaProximo();
                                if (tokentype == "of")
                                {
                                    sitype();
                                    return;
                                }
                                else
                                    erro();
                            }
                            else
                                if (tokentype == "record")
                                {
                                    filist();
                                    PegaProximo();
                                    if (tokentype == "end")
                                        return;
                                    else
                                        erro();
                                }
                }
                
            }
        }
        private void filist()
        {
            bool volta, voltainicio;
            volta = true;
            voltainicio = true;
            while (voltainicio)
            {
                while (volta)
                {
                    if (tokentype == "IDEN")
                    {
                        PegaProximo();
                        if (tokentype == ",")
                            PegaProximo();
                        if (tokentype == ":")
                        {
                            volta = false;
                            PegaProximo();
                        }
                    }
                    else
                        if (tokentype == ";")
                            PegaProximo();
                        else
                            if (tokentype == "case")
                            {
                                voltainicio = false;
                                PegaProximo();
                                casefilist();
                            }                             
                }
                    type();
                    PegaProximo();
                    if (tokentype == ";")
                        PegaProximo();
                    if (tokentype == "case")
                    {
                        voltainicio = false;
                        PegaProximo();
                        casefilist();
                    }
                    else 
                    {
                        //Verifica este em outras estruturas
                    }                
            }
        }
        private void casefilist()
        {
            bool volta = true;
            while (volta)
            {
                if ((tokentype == "String") || (tokentype == "COIDEN") || (tokentype == "NUMB")||(tokentype == "+")||(tokentype == "-"))
                {
                    PegaProximo();
                    if ((tokentype == "+") || (tokentype == "-"))
                    {
                        if ((tokentype != "COIDEN") && (tokentype != "NUMB"))
                            erro();
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
            volta = true;
            voltainicio = true;
            PegaProximo();
            while (voltainicio)
            {
                if (tokentype == "[")
                {
                    while (volta)
                    {
                        expr();
                        if (tokentype == ",")
                            PegaProximo();
                        else
                            if (tokentype == "]")
                            {
                                volta = false;
                                PegaProximo();
                            } 
                    }                    
                }
                else
                    if (tokentype == ".")
                    { }
                    else
                        return;
            }           
        }
        private void factor()
        { }
        private void term()
        {
            factor();
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
        private void palist()
        {
 
        }
        private void block()
        { }
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
                            bool volta = false;
                            if (tokentype == "(")
                            {
                                PegaProximo();
                                while (volta)
                                {
                                    if (tokentype == "PRIDEN")
                                    {
                                        PegaProximo();
                                        if (tokentype == ")")
                                        {
                                            volta = false;
                                            return;
                                        }
                                        else
                                        {
                                            if (tokentype == ",")
                                                PegaProximo();
                                        }
                                    }
                                    else
                                    {
                                        expr();
                                        PegaProximo();
                                        if (tokentype == ")")
                                        {
                                            volta = false;
                                            return;
                                        }
                                        else
                                        {
                                            if (tokentype == ",")
                                                PegaProximo();
                                        }                                       
                                    } 
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
        private void progrm()
        {
            PegaProximo();
            if (tokentype == "IDEN")
            {
                bool volta = true;
                PegaProximo();
                if (tokentype == "(")
                    while (volta)
                    {
                        PegaProximo();
                        if (tokentype == "IDEN")
                        {
                            PegaProximo();
                            if (tokentype == ",")
                                PegaProximo();
                            else
                                if (tokentype == ")")
                                {
                                    volta = false;
                                    PegaProximo();
                                    if (tokentype == ";")
                                    {
                                        block();
                                        PegaProximo();
                                        if (tokentype == ".")
                                            return;
                                        else
                                            erro();
                                    }
                                    else
                                        erro();
                                }
                        }

                    }
            }
        }

    }
}
