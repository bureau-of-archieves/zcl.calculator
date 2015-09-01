using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCL.Interpreters;
using ZCL.Interpreters.Calculator;

namespace ZCL.Interpreters
{
    public class CalculatorFactory
    {
        private readonly CommandProvider commandProvider;

        public CalculatorFactory()
            : this(new CommandConfigurer())
        { }

        public CalculatorFactory(CommandConfigurer commandConfigurer)
        {
            commandProvider = new CommandProvider(commandConfigurer);
        }


        public ICalculator createInstance() {
            return new CalculatorImpl(commandProvider);
        }

    }


}
