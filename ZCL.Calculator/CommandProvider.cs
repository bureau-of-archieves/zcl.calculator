using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters
{
    public partial class Calculator
    {
        /// <summary>
        /// This class defines the operations supported by this calculator.
        /// </summary>
        private class CommandProvider
        {
            private static IDictionary<string, StackCommand> _commands;

            static CommandProvider()
            {
                //sufix > prefix > binary
                _commands = new Dictionary<string, StackCommand>();
                _commands.Add("+", new Operator("+", OperatorType.Binary, 5000, AssociationType.Left, (x, y) => x + y));
                _commands.Add("-", new Operator("-", OperatorType.Binary | OperatorType.Prefix, 5000, AssociationType.Left,
                    (x, y) => (double.IsNaN(x) ? 0 : x) - y
                ));
                _commands.Add("*", new Operator("*", OperatorType.Binary, 6000, AssociationType.Left, (x, y) => x * y));
                _commands.Add("/", new Operator("/", OperatorType.Binary, 6000, AssociationType.Left, (x, y) => x / y));
                _commands.Add("%", new Operator("%", OperatorType.Binary, 7000, AssociationType.Left, (x, y) => (long)x % (long)y));
                _commands.Add("^", new Operator("^", OperatorType.Binary, 8000, AssociationType.Right, (x, y) => Math.Pow(x, y)));
                _commands.Add("!", new Operator("!", OperatorType.Prefix | OperatorType.Suffix, 5000, AssociationType.Right,
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

                    }));

                _commands.Add("=", new Operator("=", OperatorType.Binary, 4000, AssociationType.Left, (x, y) => x == y ? 1 : 0));
                _commands.Add("!=", new Operator("!=", OperatorType.Binary, 4000, AssociationType.Left, (x, y) => x != y ? 1 : 0));
                _commands.Add(">", new Operator(">", OperatorType.Binary, 4000, AssociationType.Left, (x, y) => x > y ? 1 : 0));
                _commands.Add(">=", new Operator(">=", OperatorType.Binary, 4000, AssociationType.Left, (x, y) => x >= y ? 1 : 0));
                _commands.Add("<", new Operator("<", OperatorType.Binary, 4000, AssociationType.Left, (x, y) => x < y ? 1 : 0));
                _commands.Add("<=", new Operator("<=", OperatorType.Binary, 4000, AssociationType.Left, (x, y) => x <= y ? 1 : 0));


                //functions
                _commands.Add("log", new Function("log", args => Math.Log(args[1], args[0])));
                _commands.Add("sin", new Function("sin", args => Math.Sin(args[0])));
                _commands.Add("cos", new Function("cos", args => Math.Cos(args[0])));
                _commands.Add("tan", new Function("tan", args => Math.Tan(args[0])));
                _commands.Add("sum", new Function("sum", args => args.Sum()));
                _commands.Add("nan", new Function("nan", args => double.NaN));
            }

            public StackCommand GetCommand(string opName)
            {
                return _commands[opName];
            }

            public bool HasCommand(string opName)
            {
                return _commands.ContainsKey(opName);
            }
        }
    }
}
