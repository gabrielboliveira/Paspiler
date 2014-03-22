using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public interface IAstNode
    {
	    void SetStartLoc(SourceLocation location);
	    void SetEndLoc(SourceLocation location);
    }
}
