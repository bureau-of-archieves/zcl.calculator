using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCL.Interpreters.Calculator
{

    public interface ICommandProvider
    {
        void CreateOperator(String opToken, OperatorType type, int precedence, AssociationType association, Func<double, double, double> body);

        void CreateFunction(string name, Func<IList<double>, double> body);

        void CreateConstant(String name, double value);

    }

}
