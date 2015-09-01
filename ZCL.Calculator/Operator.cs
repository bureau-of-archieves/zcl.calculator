using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters.Calculator
{

    internal class Operator : StackCommand
    {
        public Operator(string name, OperatorType type, int precedence, AssociationType association, Func<double, double, double> body)
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
