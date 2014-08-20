using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters
{
    public partial class Calculator
    {
        private class Operator : StackCommand
        {
            internal Operator(string name, OperatorType type, int precedence, AssociationType association, Func<double, double, double> body)
                : base(name)
            {
                this.Type = type;
                this.Precedence = precedence;
                this.Association = association;
                this.Body = body;
            }

            public OperatorType Type
            {
                get;
                private set;
            }

            public AssociationType Association
            {
                get;
                private set;
            }

            public int Precedence
            {
                get;
                private set;
            }

            public Func<double, double, double> Body
            {
                get;
                private set;
            }

        }
    }

}
