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
        //REGRAS DOS MÉTODOS: Antes de retornar de um método eu pego o próximo, logo, não se pega o próximo no começo de nenhum método
        //Só nao pego o próximo no final do método se o último comando for outro método
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
        {//ok
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
                        expr();
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
        private void factor()
        {//ok
            if ((tokentype == "COIDEN")||(tokentype == "NUMB")||(tokentype == "nil")||(tokentype == "STRING"))
            {
                PegaProximo();
                return;
            }
            else
                if (tokentype == "VAIDEN")
                {
                    PegaProximo();
                    infipo();
                    return;
                }else
                    if (tokentype == "FUIDEN")
                    {
                        PegaProximo();
                        if (tokentype == "(")
                        {
                            bool volta = true;
                            PegaProximo();
                            while (volta)
                            {
                                expr();
                                if (tokentype == ",")
                                    PegaProximo();
                                else
                                    if (tokentype == ")")
                                    {
                                        volta = false;
                                        PegaProximo();
                                        return;
                                    }
                                    else
                                    {
                                        volta = false;
                                        erro("Esperava ) obteve " + tokentype);
                                    }
                            }
                        }
                        else
                            return;
                    }else
                        if (tokentype == "(")
                        {
                            PegaProximo();
                            expr();
                            if (tokentype == ")")
                            {
                                PegaProximo();
                                return;
                            }
                            else
                            {
                                erro("Esperava ) obteve "+tokentype);
                            }                               
                        }
                        else
                            if (tokentype == "not")
                            {
                                PegaProximo();
                                factor();
                                return;
                            }else
                                if (tokentype == "[")
                                {
                                    PegaProximo();
                                    if (tokentype == "]")
                                    {
                                        PegaProximo();
                                        return;
                                    }
                                    else
                                    {
                                        bool volta = true;
                                        PegaProximo();
                                        while (volta)
                                        {
                                            expr();
                                            if (tokentype == "..")
                                            {
                                                PegaProximo();
                                                expr();
                                                if (tokentype == ",")
                                                    PegaProximo();
                                                else
                                                    if (tokentype == "]")
                                                    {
                                                        volta = false;
                                                        PegaProximo();
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        volta = false;
                                                        erro("Esperava ] obteve " + tokentype);

                                                    }
                                            }
                                            else
                                            {
                                                if (tokentype == ",")
                                                    PegaProximo();
                                                else
                                                    if (tokentype == "]")
                                                    {
                                                        volta = false;
                                                        PegaProximo();
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        volta = false;
                                                        erro("Esperava ] obteve " + tokentype);

                                                    }
                                            }
                                        }
                                    }
                                }                
        }
        private void term()
        {//ok
            bool volta = true;
            PegaProximo();
            while (volta)
            {
                factor();
                if ((tokentype == "*") || (tokentype == "/") || (tokentype == "div") || (tokentype == "mod") || (tokentype == "and"))
                    PegaProximo();
                else
                {
                    volta = false;
                    return;
                }
                   
            }            
        }      
        private void siexpr()
        {//ok
            if ((tokentype == "+") || (tokentype == "-"))
            {
                bool volta = true;
                PegaProximo();
                while (volta)
                {
                    term();
                    if ((tokentype == "+") || (tokentype == "-") || (tokentype == "or"))
                    {
                        PegaProximo();
                    }
                    else 
                    {
                        volta = false;
                        return;
                    }                       
                }              
            }
            else
                erro("Esperava + ou - obteve "+tokentype);

        }
        private void expr()
        {//ok
            PegaProximo();
            siexpr();
            if ((tokentype == "=") || (tokentype == "<") || (tokentype == ">") || (tokentype == "<>") || (tokentype == ">=") || (tokentype == "<=") || (tokentype == "in"))
            {
                PegaProximo();
                siexpr();
                return;
            }
            else
            {
                return; 
            }
               
        }
        private void palist()
        {//nao conferi pois nao tinha referencias
 
        }
        private void block()
        {// ok - não será preciso implementar por enquanto proc e func
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
        {//ok
           
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
                                        {
                                            PegaProximo();
                                            expr();
                                            if (tokentype == "do")
                                            {
                                                PegaProximo();
                                                statm();
                                                return;
                                            }
                                            else
                                                erro("Esperava do obteve " + tokentype);
                                        }
                                        else
                                        {
                                            if (tokentype == "repeat")
                                            {
                                                bool volta = true;
                                                while (volta)
                                                {
                                                    PegaProximo();
                                                    statm();
                                                    if (tokentype == ";")
                                                        PegaProximo();
                                                    else
                                                    {
                                                        if (tokentype == "until")
                                                        {
                                                            volta = false;
                                                            PegaProximo();
                                                            expr();
                                                            return;
                                                        }
                                                        else 
                                                        {
                                                            volta = false;
                                                            erro("Esperava until obteve " + tokentype);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (tokentype == "for")
                                                {
                                                    PegaProximo();
                                                    if (tokentype == "VAIDEN")
                                                    {
                                                        PegaProximo();
                                                        infipo();
                                                        if (tokentype == ":=")
                                                        {
                                                            PegaProximo();
                                                            expr();
                                                            if ((tokentype == "to") || (tokentype == "downto"))
                                                            {
                                                                PegaProximo();
                                                                expr();
                                                                if (tokentype == "do")
                                                                {
                                                                    PegaProximo();
                                                                    statm();
                                                                    return;
                                                                }
                                                                else
                                                                    erro("Esperava do obteve " + tokentype);
                                                            }
                                                            else
                                                                erro("Esperava tokentype ou downto obteve " + tokentype);
                                                        }
                                                        else
                                                            erro("Esperava := obteve " + tokentype);
                                                    }
                                                    else
                                                        erro("Esperava VARIDEN obteve " + tokentype);
                                                }
                                                else
                                                {
                                                    if (tokentype == "with")
                                                    {
                                                        bool volta = true;
                                                        PegaProximo();
                                                        while (volta)
                                                        {
                                                            if (tokentype == "VAIDEN")
                                                            {
                                                                PegaProximo();
                                                                infipo();
                                                                if (tokentype == ",")
                                                                    PegaProximo();
                                                                else
                                                                {
                                                                    if (tokentype == "do")
                                                                    {
                                                                        PegaProximo();
                                                                        statm();
                                                                        return;
                                                                    }
                                                                    else
                                                                    {
                                                                        volta = false;
                                                                        erro("Esperava do obteve " + tokentype);
                                                                    }
                                                                        
                                                                }
                                                            }
                                                            else
                                                                erro("Esperava VAIDEN obteve " + tokentype);
                                                        }
                                                       
                                                    }
                                                    else
                                                    {
                                                        if (tokentype == "goto")
                                                        {
                                                            PegaProximo();
                                                            if (tokentype == "NUMB")
                                                            {
                                                                PegaProximo();
                                                                return;
                                                            }
                                                            else
                                                                erro("Esperava número obteve " + tokentype);
                                                        }
                                                        else
                                                            erro("statm inválido " + tokentype);
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
        {//ok
            
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
                                {
                                    if (tokentype == ")")
                                    {
                                        volta = false;
                                        PegaProximo();
                                        if (tokentype == ";")
                                        {
                                            PegaProximo();
                                            block();
                                            if (tokentype == ".")
                                            {
                                                PegaProximo();
                                                return;
                                            }
                                            else
                                            {
                                                volta = false;
                                                erro("Esperou . obteve " + tokentype);
                                            }
                                        }
                                        else
                                        {
                                            volta = false;
                                            erro("Esperou ; obteve " + tokentype);
                                        }
                                    }
                                    else
                                    {
                                        volta = false;
                                        erro("Esperou ) obteve " + tokentype);
                                    }
                                }
                            }
                        }

                    }
                    else
                        erro("Esperou ( obteve " + tokentype);

                }
                else
                    erro("Esperava identificador obteve " + tokentype);
 
            }
           
        }

    }
}
