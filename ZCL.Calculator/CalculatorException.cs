using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters
{
    public class CalculatorException : ApplicationException
    {
        public CalculatorException(string message, Exception innerException = null)
            : base(message, innerException)
        { }
    }
}
