using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters
{
    public partial class Calculator
    {
        private class Scope
        {
            private Dictionary<String, ValueSlot> _storage = new Dictionary<string, ValueSlot>();

            public double this[string varname]
            {
                get
                {
                    ValueSlot slot;
                    if (_storage.TryGetValue(varname, out slot))
                        return slot.Value;
                    return double.NaN;
                }

                set
                {
                    ValueSlot slot;
                    if (_storage.TryGetValue(varname, out slot))
                    {
                        if (slot.Position != 0) throw new CalculatorException("Cannot assign a value to a constant.");

                        if (double.IsNaN(value))
                        {
                            _storage.Remove(varname);
                            return;
                        }
                    }

                    _storage[varname] = new ValueSlot(value, 0);
                }
            }

            public void SetConstant(string constName, double value)
            {
                this._storage[constName] = new ValueSlot(value, 1);
            }
        }
    }
}
