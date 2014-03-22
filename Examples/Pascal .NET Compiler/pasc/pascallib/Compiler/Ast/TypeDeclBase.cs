using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace pascallib.Compiler.Ast
{
    public abstract class TypeDeclBase : pascallib.Compiler.Ast.IDataStructure
    {
        private SourceLocation start;
        private SourceLocation end;

        protected List<ITypeMember> members;

        public TypeDeclBase(string name)
        {
            this.Name = name;
            this.members = new List<ITypeMember>();
        }

        public string Name
        {
            get;
            protected set;
        }

        public List<ITypeMember> Members
        {
            get
            {
                return new List<ITypeMember>(this.members);
            }

            protected set
            {
                this.members = value;
            }
        }

        public abstract void Prepare(AssemblyBuilder assemblyBuilder, SymbolTable symbolTable);

        public abstract void Generate();

        public void SetStartLoc(SourceLocation location)
        {
            this.start = location;
        }

        public void SetEndLoc(SourceLocation location)
        {
            this.end = location;
        }
    }
}
