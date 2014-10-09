using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler.Model
{
    static class Globals
    {
        private static System.Threading.SynchronizationContext mContext = null;

        public static System.Threading.SynchronizationContext MContext
        {
            get { return Globals.mContext; }
            set { Globals.mContext = value; }
        }

        public delegate IAsyncResult Invoke(Delegate method, params object[] args);

        public static Invoke Execute;
    }
}
