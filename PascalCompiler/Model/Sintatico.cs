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
        private void erro(String erro)
        {
           //Ver a linha do erro e o motivo
        }
        private void constant()
        {//ok
            if (tokentype == "STRING")
            {
                PegaProximo();
                return;
            }
            else
            {
                if ((tokentype == "+") || (tokentype == "-"))
                {
                    if ((tokentype == "COIDEN") || (tokentype == "NUMB"))
                    {
                        PegaProximo();
                        return;
                    }

                    else
                    {
                        erro("Esperava numero ou coiden(?) obteve " + tokentype);                        
                    }
                }
                else
                {
                    erro("Esperava String, + ou - obteve "+tokentype);                    
                }                   
            }
        }        
        private void sitype() 
        {//ok
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
                    constant();
                    if (tokentype == "..")
                    {
                        PegaProximo();
                        constant();
                        // so pego o proximo se antes de retornar se nao o termo anterior nao é função
                        return;
                    }
                    else
                        erro("Esperava .. obteve " + tokentype);
                } 
            }

        }
        private void type()
        {//ok
            
            if (tokentype == "TYIDEN")
            {
                PegaProximo();
                return;
            }
            if ((tokentype == "packed")||(tokentype == "array")||(tokentype == "file")||(tokentype == "set")||(tokentype == "record"))
            {
                if (tokentype == "packed")
                {
                    PegaProximo();
                }
                    else
                    {
                        PegaProximo();
                        sitype();
                    }                       
                }
                if (tokentype == "array")
                {
                    PegaProximo();
                    if (tokentype == "[")
                    {
                        bool volta = true;
                        while (volta)
                        {
                            PegaProximo();
                            sitype();                                
                            if (tokentype == ",")
                                PegaProximo();
                            else
                                if (tokentype == "]")
                                {
                                    volta = false;
                                    PegaProximo();
                                    if (tokentype == "of")
                                    {
                                        PegaProximo();
                                        type();                                        
                                        return;
                                    }
                                    else
                                        erro("Esperava of obteve "+tokentype);
                                }
                                else
                                    erro("Esperava , ou ] obteve "+tokentype);
                        }
                    }
                }// fim if array
                else 
                    if (tokentype == "file")
                    {
                        PegaProximo();
                        if (tokentype == "of")
                        {
                            PegaProximo();
                            type();                           
                            return;
                        }
                        else
                            erro("Esperava of obteve "+tokentype);
                        }
                    else
                        if (tokentype == "set")
                        {
                            PegaProximo();
                            if (tokentype == "of")
                            {
                                PegaProximo();
                                sitype();
                                return;
                            }
                            else
                                erro("Esperava of obteve "+tokentype);
                            }
                        else
                            if (tokentype == "record")
                            {
                                PegaProximo();
                                filist();                                
                                if (tokentype == "end")
                                {
                                    PegaProximo();
                                    return;
                                }                                   
                                else
                                    erro("Esperava end obteve "+ tokentype);
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
        {// falta proc e func
            bool volta;
            if (tokentype == "label")
            {
                PegaProximo();
                volta = true;
                while (volta)
                {
                    if (tokentype == "NUMB")
                    {
                        PegaProximo();
                        if (tokentype == ",")
                            PegaProximo();
                        else
                            if (tokentype == ";")
                            {
                                volta = false;
                                PegaProximo();
                                block();
                            }
                            else
                            {
                                erro("Esperava , ou ; obteve " + tokentype);
                                volta = false;
                            }
                    }
                    else
                    {
                        erro("Esperava NUMB obteve " + tokentype);     
                    }
                }      
            }
            else
                if (tokentype == "const")
                {
                    PegaProximo();
                    if (tokentype == "IDEN")
                    {
                        PegaProximo();
                        volta = true;
                        while (volta)
                        {
                            if (tokentype == "=")
                            {
                                PegaProximo();
                                constant();
                                if (tokentype == ";")
                                {
                                    PegaProximo();
                                    if (tokentype == "IDEN")
                                    {
                                        PegaProximo();
                                    }
                                    else
                                    {
                                        volta = false;
                                        PegaProximo();
                                        block();
                                    }
                                }
                                else
                                {
                                    erro("Esperava = obteve " + tokentype);
                                    volta = false;
                                }
                            }
                            else
                            {
                                erro("Esperava = obteve " + tokentype);
                                volta = false;
                            }
                        }
                    }
                    else
                    {
                        erro("Esperava  identificador obteve " + tokentype);                        
                    }
                }else
                    if (tokentype == "type")
                    {
                        PegaProximo();
                        if (tokentype == "IDEN")
                        {
                            volta = true;
                            PegaProximo();
                            while (volta)
                            {
                                if (tokentype == "=")
                                {
                                    PegaProximo();
                                    type();
                                    if (tokentype == ";")
                                    {
                                        PegaProximo();
                                        if (tokentype == "IDEN")
                                            PegaProximo();
                                        else
                                        {
                                            volta = false;
                                            PegaProximo();
                                            block();
                                        }
                                    }
                                    else
                                    {
                                        erro("Esperava ; obteve " + tokentype);
                                        volta = false;
                                    }                                       
                                }
                                else
                                {
                                    erro("Esperava = obteve " + tokentype);
                                    volta = false;
                                }                                    
                            }
                        }
                        else
                            erro("Esperava Identificador, obteve " + tokentype);
                    }
                    else
                        if (tokentype == "var")
                        {
                            PegaProximo();
                            if (tokentype == "IDEN")
                            {
                                PegaProximo();
                                volta = true;
                                while (volta)
                                {
                                    if (tokentype == ",")
                                    {
                                        PegaProximo();
                                        if (tokentype == "IDEN")
                                            PegaProximo();
                                        else
                                        {
                                            erro("Esperava identificador obteve " + tokentype);
                                            volta = false;
                                        }
                                            
                                    }
                                    else
                                        if (tokentype == ":")
                                        {
                                            PegaProximo();
                                            type();
                                            if (tokentype == ";")
                                            {
                                                PegaProximo();
                                                if (tokentype == "IDEN")
                                                    PegaProximo();
                                                else
                                                {
                                                    volta = false;
                                                    PegaProximo();
                                                    block();
                                                }
                                            }
                                            else
                                            {
                                                erro("Esperava ; obteve: " + tokentype);
                                                volta = false; ;
                                            }
                                                

                                        }
                                        else
                                        {
                                            erro("Esperava , ou : obteve " + tokentype);
                                            volta = false;
                                        }                                           
                                }

                            }
                            else
                                erro("Esperava identificador obteve " + tokentype);
                        }//fim if var
                            //TODO: falta proc e func
                        else
                            if (tokentype == "begin")
                            {
                                volta = true;
                                while (volta)
                                {
                                    PegaProximo();
                                    statm();
                                    if (tokentype == ";")
                                        PegaProximo();
                                    else
                                        if (tokentype == "end")
                                        {
                                            PegaProximo();
                                            return;
                                        }
                                        else
                                        {
                                            erro("Esperava ; ou end obteve" + tokentype);
                                            volta = false;
                                        }
                                            
                                }
                            }
            //terminar

        }
        private void statm()
        {
           
            if (tokentype == "NUMB")
            {
                PegaProximo();
                if (tokentype == ":")
                {
                    PegaProximo();
                    statm();
                }
                else
                    erro("Esperava : obteve "+ tokentype);
            }
            else
            {
                if (tokentype == "VAIDEN")
                {
                    PegaProximo();
                    infipo();
                    if (tokentype == ":=")
                    {
                        PegaProximo();
                        expr();
                        return;
                    }
                    else
                        erro("Esperava := obteve " + tokentype);
                }
                else
                {
                    if (tokentype == "FUIDEN")
                    {
                        PegaProximo();
                        if (tokentype == ":=")
                        {
                            PegaProximo();
                            expr();
                            return;
                        }
                        else
                            erro("Esperava := obteve " + tokentype);
                    }
                    else
                    {
                        if (tokentype == "PRIDEN")
                        {                            
                            PegaProximo();
                            if (tokentype == "(")
                            {
                                PegaProximo();
                                bool volta = true;
                                while (volta)
                                {
                                    if (tokentype == "PRIDEN")
                                    {
                                        PegaProximo();
                                        if (tokentype == ")")
                                        {
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
                                    else
                                    {
                                        PegaProximo();
                                        expr();
                                        if (tokentype == ")")
                                        {
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
                                }
                            }
                            else
                            {
                                //nao pega o proximo pois é lambda
                                return; 
                            }
                               
                        }
                        else
                        {
                            if (tokentype == "begin")
                            {
                                PegaProximo();
                                bool volta = true;
                                while(volta)
                                {
                                    statm();
                                    if (tokentype == ";")
                                        PegaProximo();
                                    else
                                        if (tokentype == "end")
                                        {
                                            PegaProximo();
                                            return;
                                        }
                                        else
                                        {
                                            erro("Esperava ; ou end obteve " + tokentype);
                                            volta = false;
                                        }
                                }                              
                            }
                            else
                            {
                                if (tokentype == "if")
                                {
                                    PegaProximo();
                                    expr();                                    
                                    if (tokentype == "then")
                                    {
                                        PegaProximo();
                                        statm();
                                        if (tokentype == "else")
                                        {
                                            PegaProximo();
                                            statm();                                          
                                            return;
                                        }
                                        else
                                        {
                                            PegaProximo();
                                            return;
                                        }                                           
                                    }
                                }
                                else
                                {
                                    if (tokentype == "case")
                                    {
                                        PegaProximo();
                                        expr();
                                        if (tokentype == "of")
                                        {
                                            bool volta = true;
                                            PegaProximo();
                                            while (volta)
                                            {
                                                if ((tokentype == "STRING")||(tokentype == "COIDEN")||(tokentype == "NUMB"))
                                                {
                                                    PegaProximo();
                                                    if (tokentype == ",")
                                                        PegaProximo();
                                                    else
                                                        if (tokentype == ":")
                                                        {
                                                            PegaProximo();
                                                            statm();
                                                            if (tokentype == ";")
                                                                PegaProximo();
                                                            else
                                                                if (tokentype == "end")
                                                                {
                                                                    volta = false;
                                                                    PegaProximo();
                                                                    return;
                                                                }
                                                        } 
                                                }
                                                else
                                                    if ((tokentype == "+") || (tokentype == "-"))
                                                    {
                                                        PegaProximo();
                                                        if ((tokentype == "NUMB") || (tokentype == "COIDEN"))
                                                        {
                                                            PegaProximo();
                                                            if (tokentype == ",")
                                                                PegaProximo();
                                                            else
                                                                if (tokentype == ":")
                                                                {
                                                                    PegaProximo();
                                                                    statm();
                                                                    if (tokentype == ";")
                                                                        PegaProximo();
                                                                    else
                                                                        if (tokentype == "end")
                                                                        {
                                                                            volta = false;
                                                                            PegaProximo();
                                                                            return;
                                                                        }
                                                                }  
                                                        }
                                                    }
                                            }// fim while
                                        }
                                        else 
                                        {
                                            erro("Esperava of obteve " + tokentype);
                                        }
                                       
                                    }
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
            
            if (tokentype == "program")
            {
                PegaProximo();
                if (tokentype == "IDEN")
                {
                    bool volta = true;
                    PegaProximo();
                    if (tokentype == "(")
                    {
                        PegaProximo();
                        while (volta)
                        {

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
                                            PegaProximo();
                                            block();
                                            if (tokentype == ".")
                                                return;
                                            else
                                                erro("Esperou . obteve " + tokentype);
                                        }
                                        else
                                            erro("Esperou ; obteve " + tokentype);
                                    }
                                    else
                                        erro("Esperou ) obteve " + tokentype);
                            }
                        }
 
                    }
                        
                }
 
            }
           
        }

    }
}
