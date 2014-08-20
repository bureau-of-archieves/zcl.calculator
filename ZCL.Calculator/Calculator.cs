using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCL.Interpreters
{
    public partial class Calculator
    {
        //available symbols for operator in this calculator. Order here matters for parsing.
        private static readonly string[] OP_TOKENS = new string[] { "!=", ">=", "<=", ":=", "+", "-", "*", "/", "^", "%", "!", "=", ">", "<", "(", ")", "," };
        private static readonly CommandProvider COMMANDS;//operators + functions
        private static readonly Tokenizer TOKENIZER;

        private Stack<OpSlot> _opStack;
        private Stack<ValueSlot> _valueStack;
        private Scope _scope;
        private string _expr;
        private TokenBufferItem[] _tokenBuffer;

        static Calculator()
        {
            COMMANDS = new CommandProvider();
            TOKENIZER = new Tokenizer(OP_TOKENS);
        }

        public Calculator()
        {
            _scope = new Scope(); //scope lives with this instance
            _opStack = new Stack<OpSlot>();
            _valueStack = new Stack<ValueSlot>();
            _tokenBuffer = new TokenBufferItem[2]; //need to look 2 ahead to decide type of operator (e.g. prefix or suffix)

            _scope.SetConstant("PI", Math.PI);
            _scope.SetConstant("E", Math.E);
        }

        /// <summary>
        /// This property is used to number Val-Op block.
        /// E.g.
        /// a + -b * -c
        /// 0 1  2 3  4
        /// The id increases when value and op alternates.
        /// </summary>
        private int MaxTokenBlockId
        {
            get
            {
                if (_opStack.Count == 0) return 0;
                int lastValuePos = -1;
                if (_valueStack.Count > 0)
                    lastValuePos = _valueStack.Peek().Position;

                int lastOpPos = _opStack.Peek().Position;

                return lastOpPos > lastValuePos ? lastOpPos : lastValuePos;
            }
        }

        private bool LastIsValue
        {
            get
            {
                if (_opStack.Count == 0)
                    return false;
                if (_valueStack.Count > 0)
                {
                    var valSlot = _valueStack.Peek();
                    var opSlot = _opStack.Peek();

                    if (valSlot.Position > opSlot.Position)
                        return true;
                    else
                    {
                        if (valSlot.Position == opSlot.Position && opSlot.Type == OperatorType.Suffix)
                            return true;

                        return false;
                    }
                }

                return false;
            }
        }

        public double Compute(string expr)
        {
            _expr = expr;

            //parse VARNAME := RIGHT | RIGHT
            int index = 0;
            string varname = null;

            while (true)
            {
                int tokenStartIndex = index;
                var token = TOKENIZER.ReadToken(expr, ref index);
                if (token == TokenType.Invalid) new CalculatorException(string.Format("Invalid token at column {0}", tokenStartIndex));
                if (token == TokenType.Whitespace) continue;

                if (varname == null)
                {
                    if (token != TokenType.Identifier)
                    {
                        index = 0;
                        break;
                    }
                    varname = expr.Substring(tokenStartIndex, index - tokenStartIndex);
                }
                else
                {
                    if (token == TokenType.Op && expr.Substring(tokenStartIndex, index - tokenStartIndex) == ":=")
                    {
                        break;
                    }
                    else
                    {
                        varname = null;
                        index = 0;
                        break;
                    }
                }
            }

            double retval = ComputeRightValue(index);
            if (varname != null)
                _scope[varname] = retval;
            return retval;
        }

        private double ComputeRightValue(int index)
        {
            _opStack.Clear();
            _valueStack.Clear();

            //add a surrounding () for simplification.
            _tokenBuffer[1].Type = TokenType.Op;
            _tokenBuffer[1].Token = "(";
            _tokenBuffer[1].StartIndex = -1;

            while (true)
            {
                int tokenStartIndex = index;
                TokenType type = TOKENIZER.ReadToken(_expr, ref index);

                if (type == TokenType.Invalid)
                    throw new CalculatorException(string.Format("Invalid token at column {0}", tokenStartIndex));

                if (type == TokenType.Whitespace) continue;

                _tokenBuffer[0] = _tokenBuffer[1];

                _tokenBuffer[1].Type = type;
                _tokenBuffer[1].StartIndex = tokenStartIndex;
                _tokenBuffer[1].Token = _expr.Substring(tokenStartIndex, index - tokenStartIndex);

                if (!ProcessToken0())
                    break;
            }

            if (!(_valueStack.Count == 1 && _opStack.Count == 0))
                throw new CalculatorException("Bracket mismatch.");

            return _valueStack.Pop().Value;
        }

        /// <summary>
        /// Process the token stored in tokenBuffer position 9.
        /// </summary>
        /// <returns></returns>
        private bool ProcessToken0()
        {
            if (_tokenBuffer[0].Type == TokenType.End)
            {
                PushEndBracket();
                return false;
            }

            if (_tokenBuffer[0].Type == TokenType.Identifier)
            {
                if (LastIsValue) throw new CalculatorException(string.Format("Missing operator at column {0}", _tokenBuffer[0].StartIndex));

                double value = _scope[_tokenBuffer[0].Token]; //see if its a value
                if (!double.IsNaN(value))
                {
                    _valueStack.Push(new ValueSlot(value, MaxTokenBlockId + 1));
                    return true;
                }

                if (COMMANDS.HasCommand(_tokenBuffer[0].Token)) //see if its a func
                {
                    _opStack.Push(new OpSlot(_tokenBuffer[0].Token, OperatorType.None, MaxTokenBlockId));
                    return true;
                }
                throw new CalculatorException(string.Format("Unknown name at column {0}", _tokenBuffer[0].StartIndex));
            }

            if (_tokenBuffer[0].Type == TokenType.Number)
            {
                if (LastIsValue) throw new CalculatorException(string.Format("Missing operator at column {0}", _tokenBuffer[0].StartIndex));

                double value;
                try
                {
                    value = double.Parse(_tokenBuffer[0].Token);
                }
                catch (FormatException ex)
                {
                    throw new CalculatorException(string.Format("Cannot parse value '{0}' at column {1}", _tokenBuffer[0].Token, _tokenBuffer[0].StartIndex), ex);
                }

                _valueStack.Push(new ValueSlot(value, MaxTokenBlockId + 1));
                return true;
            }

            //must be an op
            if (_tokenBuffer[0].Token == "(")
            {
                if (LastIsValue)
                    throw new CalculatorException(string.Format("Invalid opening bracket at column {0}", _tokenBuffer[0].StartIndex));

                _opStack.Push(new OpSlot("(", OperatorType.None, MaxTokenBlockId));
                return true;
            }

            if (_tokenBuffer[0].Token == ",")
            {
                if (!LastIsValue)
                    throw new CalculatorException(string.Format("Illegal separator at column {0}", _tokenBuffer[0].StartIndex));
                _opStack.Push(new OpSlot(",", OperatorType.None, MaxTokenBlockId));
                return true;
            }

            if (_tokenBuffer[0].Token == ")")
            {
                if (!LastIsValue)
                    throw new CalculatorException(string.Format("Invalid closing bracket at column {0}", _tokenBuffer[0].StartIndex));

                PushEndBracket();
                return true;
            }

            PushOperator(_tokenBuffer[0].Token);
            return true;
        }

        private void PushOperator(string key)
        {
            Operator newOp = null;
            OperatorType newOpType = OperatorType.None;

            if (key != ")") //passed an actual operator
            {
                newOp = (Operator)COMMANDS.GetCommand(key);

                if (LastIsValue)//check if is ok to add it here
                {
                    if (_tokenBuffer[1].Type == TokenType.Identifier || _tokenBuffer[1].Type == TokenType.Number || _tokenBuffer[1].Token == "(")
                    {
                        if ((newOp.Type & OperatorType.Binary) == 0)
                            throw new CalculatorException(string.Format("Operator {0} is not a binary operator at column {1}.", _tokenBuffer[1].Token, _tokenBuffer[1].StartIndex));
                        newOpType = OperatorType.Binary;
                    }
                    else
                    {
                        if ((newOp.Type & OperatorType.Suffix) != 0)
                            newOpType = OperatorType.Suffix;
                        else if ((newOp.Type & OperatorType.Binary) != 0)
                        {
                            newOpType = OperatorType.Binary;
                        }
                        else
                            throw new CalculatorException(string.Format("Illegal position for prefix operator {0} at column {1}.", _tokenBuffer[1].Token, _tokenBuffer[1].StartIndex));
                    }
                }
                else
                {
                    if ((newOp.Type & OperatorType.Prefix) == 0)
                        throw new CalculatorException(string.Format("Operator {0} is not a prefix operator at column {1}.", _tokenBuffer[1].Token, _tokenBuffer[1].StartIndex));
                    newOpType = OperatorType.Prefix;
                }
            }

            //calculate all operators in op stack that has lower precedence than newOp
            //if newOp is ) calculate to a ( but dont pop it
            while (true)
            {
                var lastOpSlot = _opStack.Peek();
                if (lastOpSlot.Value == ",")
                {
                    _opStack.Pop();
                    continue;
                }

                if (!COMMANDS.HasCommand(lastOpSlot.Value)) break;
                Operator lastOp = (Operator)COMMANDS.GetCommand(lastOpSlot.Value);

                if (lastOpSlot.Type == OperatorType.Suffix)
                {
                    var lastValueSlot = _valueStack.Pop();
                    _opStack.Pop();
                    _valueStack.Push(new ValueSlot(lastOp.Body(lastValueSlot.Value, double.NaN), lastValueSlot.Position));
                    continue;

                }
                if (newOpType == OperatorType.Suffix || newOpType == OperatorType.Prefix) break;

                if (lastOpSlot.Type == OperatorType.Prefix)
                {
                    var lastValueSlot = _valueStack.Pop();
                    _opStack.Pop();

                    _valueStack.Push(new ValueSlot(lastOp.Body(double.NaN, lastValueSlot.Value), lastValueSlot.Position));
                    continue;
                }

                //mast be binary
                if (newOp == null || lastOp.Precedence > newOp.Precedence || (lastOp.Precedence == newOp.Precedence && lastOp.Association == AssociationType.Left))
                {
                    _opStack.Pop();
                    var val2 = _valueStack.Pop();
                    var val1 = _valueStack.Pop();
                    _valueStack.Push(new ValueSlot(lastOp.Body(val1.Value, val2.Value), val1.Position));
                    continue;
                }
                break;
            }

            if (newOp != null)
            {
                if (newOpType == OperatorType.Prefix || newOpType == OperatorType.Suffix)
                    _opStack.Push(new OpSlot(key, newOpType, MaxTokenBlockId));
                else if (newOpType == OperatorType.Binary)
                    _opStack.Push(new OpSlot(key, newOpType, MaxTokenBlockId + 1));
            }
        }

        private void PushEndBracket()
        {
            if (_opStack.Count == 0) throw new CalculatorException("Missing opening bracket.");

            while (true)
            {
                var slot = _opStack.Peek();
                if (slot.Value == "(")
                {
                    _opStack.Pop();
                    if (_opStack.Count > 0)
                    {
                        var lastSlot = _opStack.Peek();
                        if (lastSlot.Value != "(")
                        {
                            var lastOp = COMMANDS.GetCommand(lastSlot.Value) as Function;
                            if (lastOp != null)
                            {
                                List<double> args = new List<double>();
                                while (_valueStack.Count > 0)
                                {
                                    var valueSlot = _valueStack.Peek();
                                    if (valueSlot.Position < lastSlot.Position)
                                        break;

                                    _valueStack.Pop();
                                    args.Add(valueSlot.Value);
                                }

                                _opStack.Pop();
                                args.Reverse();
                                _valueStack.Push(new ValueSlot(lastOp.Body(args), lastSlot.Position + 1));
                            }
                        }
                    }
                    break;
                }
                else
                {
                    PushOperator(")");//calc all previous excl. ( 
                }
            }

        }

    }

}
