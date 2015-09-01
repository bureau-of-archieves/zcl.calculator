using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters.Calculator
{
    internal enum TokenType
    {
        Op,
        Number,
        Identifier,
        Whitespace,
        End,
        Invalid
    }
}
