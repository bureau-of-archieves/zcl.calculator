using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters
{
    /// <summary>
    /// The calculator interface.
    /// </summary>
    public interface ICalculator
    {
        /// <summary>
        /// Evaluate an expression and return the result.
        /// </summary>
        /// <param name="expr">All values are converted to double.</param>
        /// <returns>The computed value.</returns>
        double Compute(string expr);

        /// <summary>
        /// Clear all variables.
        /// </summary>
        void Clear();
    }
}
