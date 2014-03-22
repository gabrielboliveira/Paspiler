using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace pascallib.Compiler.Ast
{
    public sealed class ProgramRoot
    {
        private List<TypeDeclBase> typeDecls;
        private List<FunctionDeclBase> funcDecls;

        public ProgramRoot()
        {
            this.typeDecls = new List<TypeDeclBase>();
            this.funcDecls = new List<FunctionDeclBase>();
        }

        public TypeDeclBase[] GetTypeDecls()
        {
            return this.typeDecls.ToArray();
        }

        public FunctionDeclBase[] GetFunctionDecls()
        {
            return this.funcDecls.ToArray();
        }

        public void AddTypeDecl(TypeDeclBase typeDecl)
        {
            if (typeDecl == null)
                throw new NullReferenceException("Cannot be null.");

            this.typeDecls.Add(typeDecl);
        }

        public void AddFunctionDecl(FunctionDeclBase funcDecl)
        {
            if (funcDecl == null)
                throw new NullReferenceException("Cannot be null.");

            this.funcDecls.Add(funcDecl);
        }

        public FunctionDecl GetEntrypoint()
        {
            foreach (var f in this.GetFunctionDecls())
            {
                if (f.Name == "main")
                {
                    return (FunctionDecl)f;
                }
            }

            return null;
        }

        public int Prepare()
        {
            return -1;
        }

        public int Generate(AssemblyBuilder assembly)
        {
            return -1;
        }
    }
}
