using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZCL.Interpreters;
using ZCL.Interpreters.Calculator;

namespace TextbookDataStructures.Test
{
    /// <summary>
    /// Summary description for CalculatorTest
    /// </summary>
    [TestClass]
    public class CalculatorTest
    {
        private CalculatorFactory calculatorFactory;

        public CalculatorTest()
        {
            calculatorFactory = new CalculatorFactory();
        }

        [TestMethod]
        public void Calculator_Constant_Test()
        {
            ICalculator calc = calculatorFactory.createInstance();

            double retval = calc.Compute("1024.55");
            Assert.AreEqual(retval, 1024.55);

            retval = calc.Compute("PI");
            Assert.AreEqual(retval, Math.PI);

            retval = calc.Compute("E");
            Assert.AreEqual(retval, Math.E);
        }

        [TestMethod]
        public void Calculator_Assign_Test()
        {
            ICalculator calc = calculatorFactory.createInstance();

            double retval = calc.Compute("a := 999.77");
            Assert.AreEqual(retval, 999.77);

            retval = calc.Compute("a");
            Assert.AreEqual(retval, 999.77);

            retval = calc.Compute("a:= -24.55");
            Assert.AreEqual(retval, -24.55);

            retval = calc.Compute("a");
            Assert.AreEqual(retval, -24.55);

            retval = calc.Compute("b :=-0.771");
            Assert.AreEqual(retval, -0.771);

            retval = calc.Compute("b ");
            Assert.AreEqual(retval, -0.771);
        }

        [TestMethod]
        public void Calculator_Operator_Test()
        {
            ICalculator calc = calculatorFactory.createInstance();

            double retval = calc.Compute("1 + 2");
            Assert.AreEqual(retval, 3d);

            retval = calc.Compute("b := -5 + 2");
            Assert.AreEqual(retval, -3d);

            retval = calc.Compute("99 - 112");
            Assert.AreEqual(retval, -13d);

            retval = calc.Compute("b := 44 - 5");
            Assert.AreEqual(retval, 39d);

            retval = calc.Compute("11*11");
            Assert.AreEqual(retval, 121d);

            retval = calc.Compute("b := -0.5 * 6");
            Assert.AreEqual(retval, -3d);

            retval = calc.Compute("89 / 4");
            Assert.AreEqual(retval, 22.25d);

            retval = calc.Compute("b := 1/8");
            Assert.AreEqual(retval, 0.125d);

            retval = calc.Compute("1 > 2");
            Assert.AreEqual(retval, 0d);

            retval = calc.Compute("b := 1 < 2");
            Assert.AreEqual(retval, 1d);

            retval = calc.Compute("0.999 >= 0.998");
            Assert.AreEqual(retval, 1d);

            retval = calc.Compute("b := 5.444 <= 5.444");
            Assert.AreEqual(retval, 1d);

            retval = calc.Compute("32 != 32");
            Assert.AreEqual(retval, 0d);

            retval = calc.Compute("b := 0.1 != 0.2");
            Assert.AreEqual(retval, 1d);

            retval = calc.Compute("32 = 32");
            Assert.AreEqual(retval, 1d);

            retval = calc.Compute("b := 0.1 = 0.2");
            Assert.AreEqual(retval, 0d);

            retval = calc.Compute("2^ 10");
            Assert.AreEqual(retval, 1024d);

            retval = calc.Compute("b := 5 ^ 3");
            Assert.AreEqual(retval, 125d);

            retval = calc.Compute("99.1 % 3");
            Assert.AreEqual(retval, 0d);

            retval = calc.Compute("b := 17 % 6");
            Assert.AreEqual(retval, 5d);

            retval = calc.Compute("!32");
            Assert.AreEqual(retval, 0d);

            retval = calc.Compute("4!");
            Assert.AreEqual(retval, 24d);

        }

        [TestMethod]
        public void Calculator_MultiOp_Test()
        {
            ICalculator calc = calculatorFactory.createInstance();

            double retval = calc.Compute("1000 + 990 + 9");
            Assert.AreEqual(retval, 1999d);

            retval = calc.Compute("-5-5-15.99");
            Assert.AreEqual(Math.Round(retval, 2), -25.99d);

            retval = calc.Compute("-1 + -2*5");
            Assert.AreEqual(retval, -11d);

            retval = calc.Compute("-1*9.5 + 15");
            Assert.AreEqual(retval, 5.5d);

            retval = calc.Compute("-1.5*-2*2");
            Assert.AreEqual(retval, 6d);

            retval = calc.Compute("---9.01");
            Assert.AreEqual(retval, -9.01d);

            retval = calc.Compute("1*-2*3*4*-5*6");
            Assert.AreEqual(retval, 720d);

            retval = calc.Compute("--6*11*---2");
            Assert.AreEqual(retval, -132d);

            retval = calc.Compute("99.125---2");
            Assert.AreEqual(retval, 97.125d);

        }

        [TestMethod]
        public void Calculator_MultiOpBracket_Test()
        {
            ICalculator calc = calculatorFactory.createInstance();

            double retval = calc.Compute("-(1 + -2)*5");
            Assert.AreEqual(retval, 5d);

            retval = calc.Compute("(-3 + 99)*5*0.5");
            Assert.AreEqual(retval, 240d);

            retval = calc.Compute("((-(1 + -2))*(5))");
            Assert.AreEqual(retval, 5d);

            retval = calc.Compute("(-3 + 99)*((5*0.5))");
            Assert.AreEqual(retval, 240d);

            retval = calc.Compute("(2^10-(-24 + 200)*5.25)");
            Assert.AreEqual(retval, 100d);

            retval = calc.Compute("(1+2) * (3+4) / 2");
            Assert.AreEqual(retval, 10.5d);

            retval = calc.Compute("-((72))-5*-(10+2)");
            Assert.AreEqual(retval, -12d);

            retval = calc.Compute("(-7.5*4) + (33 - 2) * 3.5");
            Assert.AreEqual(retval, 78.5d);

            retval = calc.Compute("17*(55-(54))*3 + 0.025");
            Assert.AreEqual(retval, 51.025d);

            retval = calc.Compute("(2-1)*(2-1)*(2-1)*(2-1)*(2-1)*(2-1)*(2-1)*(2-1)*(2-1)*(2-1)*(2-1)*3.5");
            Assert.AreEqual(retval, 3.5d);

            retval = calc.Compute("((2-1)*(2-1))*((2-1)*(2-1)*(2-1))*(2-1)*(2-1)*(2-1)*((2-1)*(2-1)*(2-1)*3.5)");
            Assert.AreEqual(retval, 3.5d);

            retval = calc.Compute("(---9+5)*4-2^(-3-2+6)*11");
            Assert.AreEqual(retval, -38d);

        }

        [ExpectedException(typeof(CalculatorException))]
        [TestMethod]
        public void Calculator_Bracket_Mismatch_Test()
        {
            ICalculator calc = calculatorFactory.createInstance();
            double retval = calc.Compute("a:=(PI*2+3");
        }

        [ExpectedException(typeof(CalculatorException))]
        [TestMethod]
        public void Calculator_Bracket_Mismatch_Test2()
        {
            ICalculator calc = calculatorFactory.createInstance();
            double retval = calc.Compute("a:=(PI*2+3)))");
        }

        [ExpectedException(typeof(CalculatorException))]
        [TestMethod]
        public void Calculator_InvalidToken_Test()
        {
            ICalculator calc = calculatorFactory.createInstance();
            double retval = calc.Compute("a$23a:=(PI*2+3)))");
        }

        [ExpectedException(typeof(CalculatorException))]
        [TestMethod]
        public void Calculator_InvalidToken_Test2()
        {
            ICalculator calc = calculatorFactory.createInstance();
            double retval = calc.Compute("a:=(PI*2+3$#2)))");
        }

        [ExpectedException(typeof(CalculatorException))]
        [TestMethod]
        public void Calculator_InvalidToken_Test3()
        {
            ICalculator calc = calculatorFactory.createInstance();
            double retval = calc.Compute("a:=3.3.3");
        }

        [ExpectedException(typeof(CalculatorException))]
        [TestMethod]
        public void Calculator_Unknown_Function_Test()
        {
            ICalculator calc = calculatorFactory.createInstance();
            double retval = calc.Compute("Sin(32.5)*3.44-1");
        }

        [ExpectedException(typeof(CalculatorException))]
        [TestMethod]
        public void Calculator_Unknown_Function_Test2()
        {
            ICalculator calc = calculatorFactory.createInstance();
            double retval = calc.Compute("3.44-1*Sin(32.5)");
        }

        [ExpectedException(typeof(CalculatorException))]
        [TestMethod]
        public void Calculator_Missing_Op_Test()
        {
            ICalculator calc = calculatorFactory.createInstance();
            double retval = calc.Compute("2 3 4 5");
        }

        [TestMethod]
        public void Calculator_Function_Test()
        {
            ICalculator calc = calculatorFactory.createInstance();
            double retval = calc.Compute("cos(32.5)*3.44-1");
            Assert.AreEqual(Math.Cos(32.5) * 3.44 - 1, retval);

            retval = calc.Compute("sin(33.5)+2*15-4+tan(2.112)");
            Assert.AreEqual(Math.Sin(33.5) + 2 * 15 - 4 + Math.Tan(2.112), retval);

            retval = calc.Compute("sin(sin(21)*5) + 6");
            Assert.AreEqual(Math.Sin(Math.Sin(21) * 5) + 6, retval);

            retval = calc.Compute("(sin(sin(21)*(5+3))) + 6");
            Assert.AreEqual((Math.Sin(Math.Sin(21) * (5 + 3))) + 6, retval);

            retval = calc.Compute("-sin((-sin(3.1)-PI*3)+5)*(21+log(2,5))");
            Assert.AreEqual(-Math.Sin((-Math.Sin(3.1) - Math.PI * 3) + 5) * (21 + Math.Log(5, 2)), retval);

            retval = calc.Compute("sum(1,2,3, 4)+9");
            Assert.AreEqual(19, retval);
        }

        [ExpectedException(typeof(CalculatorException))]
        [TestMethod]
        public void Calculator_Cannot_Set_Constant_Test()
        {
            ICalculator calc = calculatorFactory.createInstance();
            double retval = calc.Compute("PI:=PI+1");
        }

        [ExpectedException(typeof(CalculatorException))]
        [TestMethod]
        public void Calculator_Remove_Var_Test()
        {
            ICalculator calc = calculatorFactory.createInstance();
            double retval = calc.Compute("a := 1");
            retval = calc.Compute("a := nan(0)");
            retval = calc.Compute("a");
        }

        [TestMethod]
        public void Calculator_Full_Test()
        {
            ICalculator calc = calculatorFactory.createInstance();
            double retval = calc.Compute("a:=PI*2+3");
            retval = calc.Compute("a - 55.12345");
            Assert.AreEqual(retval, Math.PI * 2 + 3 - 55.12345);

            retval = calc.Compute("a := 11.5221");
            retval = calc.Compute("b := sin(a+3)*7");
            retval = calc.Compute("b");
            Assert.AreEqual(retval, Math.Sin(11.5221 + 3) * 7);

            retval = calc.Compute("((-b)+(a))");
            Assert.AreEqual(retval, -Math.Sin(11.5221 + 3) * 7 + 11.5221);

            retval = calc.Compute("x1:= 12.2");
            retval = calc.Compute("y1:= 98.23");
            retval = calc.Compute("x2:= -43.21");
            retval = calc.Compute("y2:= 71.4302");
            retval = calc.Compute("((x2-x1)^2+(y2-y1)^2)^0.5");
            Assert.AreEqual(retval, Math.Pow(Math.Pow(-43.21 - 12.2, 2) + Math.Pow(71.4302 - 98.23, 2), 0.5));

            retval = calc.Compute("10!/(1.33^2-log(3.1,9))");
            Assert.AreEqual(3628800d / (Math.Pow(1.33, 2) - Math.Log(9, 3.1)), retval);

            retval = calc.Compute("(5*2)! =3628800");
            Assert.AreEqual(1d, retval);

            retval = calc.Compute("(5*2)!+3 = 3628803");
            Assert.AreEqual(1d, retval);

            retval = calc.Compute("(!(a!)) = 0");
            Assert.AreEqual(1d, retval);

        }

        private class TestConfigurer : CommandConfigurer
        {

            public override void config(ICommandProvider commandProvider)
            {
                base.config(commandProvider);

                //additional operators
                commandProvider.CreateOperator("~", OperatorType.Prefix | OperatorType.Suffix, 0, AssociationType.None, (x, y) => { return double.IsNaN(y) ? (x - (long)x) : (long)y; });
            }
        }

        [TestMethod]
        public void Configurer_Test()
        {
            var configurer = new TestConfigurer();
            ICalculator calculator = new CalculatorFactory(configurer).createInstance();

            double result = calculator.Compute("~3.3");
            Assert.AreEqual(3d, result);

            result = calculator.Compute("~3.3+2.8");
            Assert.AreEqual(5.8d, result);

            result = calculator.Compute("3.3+2.8~");
            Assert.AreEqual(4.1d, result);

            result = calculator.Compute("3.3+2.8~*2");
            Assert.AreEqual(4.9d, result, 0.00001);
        }

    }
}
