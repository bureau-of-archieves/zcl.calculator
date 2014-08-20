using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters
{
    public partial class Calculator
    {
        private struct TokenBufferItem
        {
            public int StartIndex;
            public string Token;
            public TokenType Type;
        }
    }
}
