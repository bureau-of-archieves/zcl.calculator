using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters.Calculator
{
    public class CommandConfigurer
    {
        public virtual void config(ICommandProvider commandProvider)
        {
            commandProvider.CreateConstant("PI", Math.PI);
            commandProvider.CreateConstant("E", Math.E);

            //sufix > prefix > binary
            commandProvider.CreateOperator("+", OperatorType.Binary, 5000, AssociationType.Left, (x, y) => x + y);
            commandProvider.CreateOperator("-", OperatorType.Binary | OperatorType.Prefix, 5000, AssociationType.Left,
                (x, y) => (double.IsNaN(x) ? 0 : x) - y
            );
            commandProvider.CreateOperator("*", OperatorType.Binary, 6000, AssociationType.Left, (x, y) => x * y);
            commandProvider.CreateOperator("/", OperatorType.Binary, 6000, AssociationType.Left, (x, y) => x / y);
            commandProvider.CreateOperator("%", OperatorType.Binary, 7000, AssociationType.Left, (x, y) => (long)x % (long)y);
            commandProvider.CreateOperator("^", OperatorType.Binary, 8000, AssociationType.Right, (x, y) => Math.Pow(x, y));
            commandProvider.CreateOperator("!", OperatorType.Prefix | OperatorType.Suffix, 5000, AssociationType.Right,
                (x, y) =>
                {
                    if (double.IsNaN(y))
                    {
                        double retval = 1;
                        for (int i = (int)x; i > 0; i--)
                            retval *= i;
                        return retval;
                    }
                    else
                    {
                        return y == 0 ? 1 : 0;
                    }

                });
            commandProvider.CreateOperator("=", OperatorType.Binary, 4000, AssociationType.Left, (x, y) => x == y ? 1 : 0);
            commandProvider.CreateOperator("!=", OperatorType.Binary, 4000, AssociationType.Left, (x, y) => x != y ? 1 : 0);
            commandProvider.CreateOperator(">", OperatorType.Binary, 4000, AssociationType.Left, (x, y) => x > y ? 1 : 0);
            commandProvider.CreateOperator(">=", OperatorType.Binary, 4000, AssociationType.Left, (x, y) => x >= y ? 1 : 0);
            commandProvider.CreateOperator("<", OperatorType.Binary, 4000, AssociationType.Left, (x, y) => x < y ? 1 : 0);
            commandProvider.CreateOperator("<=", OperatorType.Binary, 4000, AssociationType.Left, (x, y) => x <= y ? 1 : 0);

            //functions
            commandProvider.CreateFunction("log", args => Math.Log(args[1], args[0]));
            commandProvider.CreateFunction("sin", args => Math.Sin(args[0]));
            commandProvider.CreateFunction("cos", args => Math.Cos(args[0]));
            commandProvider.CreateFunction("tan", args => Math.Tan(args[0]));
            commandProvider.CreateFunction("sum", args => args.Sum());
            commandProvider.CreateFunction("nan", args => double.NaN);
        }
    }

}
