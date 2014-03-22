using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler.Ast
{
    public enum BinaryExpressionType
    {
        Arithmetic,
        Conditional,
        Logical,
        Assignment,
        Dotted
    }
}
