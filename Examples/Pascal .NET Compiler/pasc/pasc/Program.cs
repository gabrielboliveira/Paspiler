using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pascallib.Compiler;
using System.IO;
using pascallib.Compiler.Ast;

namespace PascalNet
{
    class Program
    {
        static void Main(string[] args)
        {
            Tokenizer tokenizer = new Tokenizer(new StreamReader("test.pas"));
            //Token token = tokenizer.Peek();

            //List<Token> tokenList = new List<Token>();

            //for (int i = 0; i < 45; i++)
            //{
            //    tokenList.Add(tokenizer.GetNext());
            //}      

            Parser parser = new Parser(tokenizer);
            Expression expr = parser.ParseExpression();

            foreach (var error in tokenizer.Errors)
            {
                Console.WriteLine(error);
            }

            //Compiler compiler = new Compiler(new Parser(tokenizer));
            //compiler.Compile("output.exe");
        }
    }
}
