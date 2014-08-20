using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters
{
    public partial class Calculator
    {

        private class Function : StackCommand
        {
            public Function(string name, Func<IList<double>, double> body)
                : base(name)
            {
                this.Body = body;
            }

            public Func<IList<double>, double> Body
            {
                get;
                private set;
            }
        }
    }
}
