using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler
{
    public class SourceSpan
    {
        public SourceSpan(SourceLocation start, SourceLocation end)
        {
            this.Start = start;
            this.End = end;
        }

        public SourceLocation Start
        {
            get;
            protected set;
        }

        public SourceLocation End
        {
            get;
            protected set;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Start, this.End);
        }
    }
}
