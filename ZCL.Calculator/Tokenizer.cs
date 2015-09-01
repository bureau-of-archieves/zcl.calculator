
namespace ZCL.Interpreters.Calculator
{

    /// <summary>
    /// Breaks the input expression string into a series of tokens.
    /// </summary>
    internal class Tokenizer
    {
        private string[] _operators;

        public Tokenizer(string[] operators)
        {
            this._operators = operators;
        }

        private static bool StartWith(string expr, int index, string substring)
        {
            for (int i = 0; i < substring.Length; i++)
            {
                int subIndex = index + i;
                if (subIndex >= expr.Length)
                    return false;
                if (expr[subIndex] != substring[i]) return false;
            }

            return true;
        }

        /// <summary>
        /// Read a token.
        /// </summary>
        /// <param name="expr">Source expression.</param>
        /// <param name="index">Input the start index of the next token, output the start index of the next-next token.</param>
        /// <returns>The token type</returns>
        public TokenType ReadToken(string expr, ref int index)
        {
            if (index >= expr.Length) return TokenType.End;


            for (int i = 0; i < _operators.Length; i++)
                if (StartWith(expr, index, _operators[i]))
                {
                    index += _operators[i].Length;
                    return TokenType.Op;
                }

            if (char.IsWhiteSpace(expr[index]))
            {
                while (++index < expr.Length && char.IsWhiteSpace(expr[index])) ;
                return TokenType.Whitespace;
            }

            if (char.IsDigit(expr[index]))
            {
                while (++index < expr.Length && (char.IsDigit(expr[index]) || expr[index] == '.')) ;
                return TokenType.Number;
            }

            if (char.IsLetter(expr[index]) || expr[index] == '_')
            {
                while (++index < expr.Length && (char.IsLetterOrDigit(expr[index]) || expr[index] == '_')) ;
                return TokenType.Identifier;
            }

            return TokenType.Invalid;
        }

    }
}
