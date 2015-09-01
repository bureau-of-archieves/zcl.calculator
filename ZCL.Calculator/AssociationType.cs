using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters.Calculator
{
 
    public enum AssociationType
    {
        Left,
        Right,
        None //use this for prefix only or suffix only operators for clearity.
    }
}
