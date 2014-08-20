using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters
{
    public partial class Calculator
    {
        private struct ValueSlot
        {
            public ValueSlot(double value, int position)
                : this()
            {
                this.Value = value;
                this.Position = position;
            }

            public double Value
            {
                get;
                private set;
            }

            /// <summary>
            /// Value-Op postion in expression, starting from 0.
            /// The value of this property comes from MaxTokenBlockId.
            /// </summary>
            public int Position
            {
                get;
                private set;
            }
        }

        private struct OpSlot
        {
            public OpSlot(string value, OperatorType type, int position)
                : this()
            {
                this.Value = value;
                this.Type = type;
                this.Position = position;
            }

            public string Value
            {
                get;
                private set;
            }

            public OperatorType Type
            {
                get;
                private set;

            }

            /// <summary>
            /// Value-Op postion in expression, starting from 0.
            /// The value of this property comes from MaxTokenBlockId.
            /// </summary>
            public int Position
            {
                get;
                private set;
            }
        }
    }
}
