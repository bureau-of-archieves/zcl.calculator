using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters
{
    public partial class Calculator
    {
        private enum TokenType
        {
            Op,
            Number,
            Identifier,
            Whitespace,
            End,
            Invalid
        }
    }
}
