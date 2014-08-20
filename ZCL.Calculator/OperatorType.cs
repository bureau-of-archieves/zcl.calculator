using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters
{
    public partial class Calculator
    {
        [Flags]
        private enum OperatorType
        {
            None = 0x0,
            Prefix = 0x1,
            Suffix = 0x2,
            Binary = 0x4
        }
    }
}
