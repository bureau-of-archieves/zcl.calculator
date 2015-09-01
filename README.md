# zcl.calculator
This project implements a simple yet extensible calculator. All values are stored as double.

###Features
* Can store result in a variable
* Support unary and binary operators
* Can define your own operator, function and constant by subclassing <code>CommandConfigurer</code>
* By default support constants <code>PI</code>, <code>E</code>; operators: <code>+</code>, <code>-</code>, <code>*</code>, <code>/</code>, <code>%</code>, <code>^</code>, <code>!</code> (<small>negate when used as prefix, factorial used as suffix</small>), <code>=</code> (<small>equal comparison</small>), <code>!=</code>, <code>&gt;</code>, <code>&gt;=</code>, <code>&lt;</code>, <code>&lt;=</code>; functions <code>log</code>, <code>sin</code>, <code>cos</code>, <code>tan</code>, <code>sum</code>, <code>nan</code>.




###Example
<pre>
  ICalculator calculator = new CalculatorFactory().createInstance();
  double retval = calc.Compute("(5*2)!+3 = 3628803");
  Assert.AreEqual(1d, retval);
</pre>


More details see the unit tests.