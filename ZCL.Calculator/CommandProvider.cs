using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters.Calculator
{

        /// <summary>
        /// This class defines the operations supported by this calculator.
        /// </summary>
        internal class CommandProvider : ICommandProvider
        {
            private readonly IDictionary<string, StackCommand> _commands;
            private readonly IList<String> _operators;
            private readonly IDictionary<String, double> _constants;

            public CommandProvider(CommandConfigurer commandConfigurer) {
                _commands = new Dictionary<string, StackCommand>();
                _constants = new Dictionary<String, double>();
                 commandConfigurer.config(this);
                _operators = _commands.Where(entry => entry.Value is Operator).Select(entry => entry.Key).ToList();
            }

            internal StackCommand GetCommand(string opName)
            {
                return _commands[opName];
            }

            internal IList<String> GetOperators()
            {
                return _operators;
            }

            internal bool HasCommand(string opName)
            {
                return _commands.ContainsKey(opName);
            }

            internal void loadConstants(Scope scope)
            {
                foreach (var entry in _constants) {
                    scope.SetConstant(entry.Key, entry.Value);
                }
            }

            public void CreateOperator(String opToken, OperatorType type, int precedence, AssociationType association, Func<double, double, double> body)
            {
                _commands.Add(opToken, new Operator(opToken, type, precedence, association, body));
            }

            public void CreateFunction(string name, Func<IList<double>, double> body)
            {
                _commands.Add(name, new Function(name, body));
            }

            public void CreateConstant(String name, double value) {
                _constants.Add(name, value);
            }

        }
    
}
