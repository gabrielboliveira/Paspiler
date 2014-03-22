using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public abstract class Expression : ICodeStructure
    {
	    private SourceLocation start;
	    private SourceLocation end;

	    public SourceSpan SourceSpan
	    {
		    get 
            {
                if (this.start == null || this.end == null)
                {
                    return null;
                }

                return new SourceSpan(this.start, this.end); 
            }
	    }

        public abstract void Prepare(System.Reflection.Emit.ILGenerator generator, SymbolTable symbolTable);
        
        public abstract void Generate();

	    public void SetStartLoc(SourceLocation location)
	    {
		    this.start = location;
	    }
	
        public void SetEndLoc(SourceLocation location)
	    {
		    this.end = location;
	    }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.GetType(), this.SourceSpan);
        }
    }
}
