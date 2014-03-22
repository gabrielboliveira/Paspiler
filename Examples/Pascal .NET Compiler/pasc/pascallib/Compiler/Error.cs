using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pascallib.Compiler
{
    public class Error
    {
        public Error(int errorCode, string description, SourceLocation location, bool isInternal = false)
        {
            this.ErrorCode = errorCode;
            this.Description = description;
            this.Location = location;
            this.IsInternal = isInternal;
        }

        public string Description
        {
            get;
            protected set;
        }

        public int ErrorCode
        {
            get;
            protected set;
        }

        public bool IsInternal
        {
            get;
            protected set;
        }

        public SourceLocation Location
        {
            get;
            protected set;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Description, this.Location);
        }
    }
}
