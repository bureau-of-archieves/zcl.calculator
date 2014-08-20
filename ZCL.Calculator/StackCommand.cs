using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters
{
    public partial class Calculator
    {
        private abstract class StackCommand
        {
            public StackCommand(string name)
            {
                this.Name = name;
            }

            public string Name
            {
                get;
                private set;
            }
        }
    }
}
