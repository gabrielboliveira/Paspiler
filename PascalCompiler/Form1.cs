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

namespace PascalCompiler
{
    public partial class Form1 : Form
    {
        StringBuilder sb = new StringBuilder();
        public Form1()
        {
            InitializeComponent();
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char caractere;            
            string verifyChar = "^[a-zA-Z]"; /*regular expression that will check if "caractere" is a letter*/
            string verifyCharorNumber = "^[a-zA-Z0-9]"; /*regular expression that will check if "caractere" is a letter or a number*/

            caractere = e.KeyChar;

            if (Regex.IsMatch(caractere.ToString(), verifyCharorNumber)) 
            {
                /*if caractere is a letter: continue getting chars until find the space key or special char*/                
                sb.Append(caractere); /*Add chars on Buffer*/
            }
            else 
            {
                String possibleToken = sb.ToString();
                sb = new StringBuilder();
                /*if possibleToken starts with a char, it's valid*/
                try {
                    if (Regex.IsMatch(possibleToken.ElementAt(0).ToString(), verifyChar))
                    {
                        MessageBox.Show(possibleToken);
                        /* looks on table of special words. If possibleToken is already there, add it on the hashtable with  
                        * its type. If is not create a new id for this token*/
                    }
                    else
                    {
                        MessageBox.Show("nao é valido: irá gerar um erro");
                    }                
                }catch(Exception erro){
                    MessageBox.Show("caractere inválido: " + erro);
                }                
            }// fim else
        }//fim metodo key_press      
    }
}
