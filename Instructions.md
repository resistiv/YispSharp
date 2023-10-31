# LISP Interpreter

#### LISP Interpreter Assignment Summary:
* Create a lisp interpreter.
* You may choose any language for implementation other than python or javascript (becuese they are weakly typed).
* I do not mind some variation in the Lisp dialect but keep it close to the model presented in class.
* You will turn in your interpreter, as well as a complete test and assessment of your interpreter. Weaknesses presented will be better than weaknesses found.
* My grader should be preseneted with complete instructions for building your project (make no assumptions, aim for a cs100 student as the grader) but also provide documentation of testing that is complete enough that the grader may choose not to build the project.
* Your tests should be simple to read and asses for correctness. (think Unit testing) You documentation should include the output of your testing.
* The grader should be able to run the tests and verify your results. 
* Failure to complete documentation as required here will significantly effect your grade on this assignement.

#### Objectives:
Implement a small dynamic programming language.

#### What you will build:
A LISP Interpreter, test code for verification of the LISP interpreter, and a demonstration of testing.

#### What you will turn in:
A zip folder containing all source code, complete compilation instructions, and results of verification and testing. I should be able to grade your project without compiling the code due to the completeness and clarity of your demonstration of testing.

#### Programming Language:
You may choose your language from your experience. A statically typed, compiled language (eg: C/C++/Java/C#/Swift)

#### Specifics:
You may extend the language.
You should support the following commands/Builtin names (not case sensitive)

DEFINE 
SET
CONS
COND
CAR
CDR

AND?
OR?
NOT?
NUMBER?
SYMBOL?
LIST?
NIL?
EQ?

\+ or add or plus
\- or sub or minus
\* or mult or times
/ or div or divide
= or ==
< or lt or less
\> ot gt or greater

#### Definitions/Semantics:
(optional - choose between if or cond)

**IF**
*(if exp1 expT expF)*
Depending on the result of exp1 either expT or expF is evaluated and returned. If exp1 is () then expF is evaluated, otherwise, expT is evaluated.

**COND**
*(cond t1 r1 t2 r2 t3 r3)*
if t1 is true returns r1...if t2 is true return r2...
Most efficient if lazy evalauation is used.
Behavior undefined if no tn is true. (probably return nil, but exit(1) is also fine)

**SET** (should only used globally)
*(set name exp)*
The symbol name is associated with the value of exp
can return the name, or the value, or nil even

**+**
*(+ expr1 epr2)*
Returns the sum of expressions. The expressions must be numbers

**-**
*(- expr1 epr2)*
Returns the difference of expressions. The expressions must be numbers

**\***
*(\* expr1 epr2)*
Returns the product of expressions. The expressions must be numbers

**/**
*(/ expr1 epr2)*
Returns the quotient. The expressions must be numbers

**=**
*(= expr1 expr2)*
Compares the values of two atoms or (). Returns () when either expression is a larger list.

**<**
*(< expr1 expr2)*
Return t when expr1 is less than expr2. Expr1 and expr2 must be numbers.

**\>**
*(> expr1 expr2)*
Return t when expr1 is greater  expr2. Expr1 and expr2 must be numbers.

**CONS**
*(cons expr1 expr2)*
Create a cons cell with expr1 as car and expr2 and cdr: ie: (exp1 . expr2)

**CAR**
*(car expr)*
Expr should be a non empty list. Car returns the car cell of the first cons cell

**CDR**
*(cdr expr)*
Expr should be a non empty list. Cdr returns the cdr cell of the first cons cell

**NUMBER?**
*(number? Expr)*
Returns T if the expr is numeric, () otherwise

**SYMBOL?**
*(symbol? Expr)*
Returns T if the expr is a name, () otherwise

**LIST?**
*(list? Expr)*
Returns T iff Expr is not an atom

**NIL?**
*(nil? Expr)*
Return T iff Expr is ()

**AND?**
*(AND? exp1 exp2)*
Return nil if either expression is nil

**OR?**
*(OR? exp1 exp2)*
Return nil if both expressions are nil

**Define**
*(define name (arg1 .. argN) expr)*
Defines a function, name. Args is a list of formal parameters. When called the expression will be evualuated with the actual parameters replacing the formal parameters.

**Informative: call syntax**
*(funname expr1 exprN)*
Calling of function defined by funname. The expressions are evaluated and passed as arguments to the function.

#### Challenges:
You may extend the language/make minor deviation (plus for +, etc, Null? for nil?).
+, - etc might take more than two arguments.
Implement LAMBDA
implement QUOTE
implement EVAL
implement APPLY
etcÅc