using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler
{
    public class SourceLocation
    {
        public SourceLocation(int ln, int ch, int index)
        {
            this.Ln = ln;
            this.Ch = ch;
            this.Index = index;
        }

        public int Ln
        {
            get;
            protected set;
        }

        public int Ch
        {
            get;
            protected set;
        }

        public int Index
        {
            get;
            protected set;
        }

        public override string ToString()
        {
            return string.Format("Ln: {0}, Col: {1}", this.Ln, this.Ch);
        }
    }
}
