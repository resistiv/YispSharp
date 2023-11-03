# Yisp Grammar
This document will be used to define and iterate upon the structure of Yisp's grammar during the development process of Y#. The variant of EBNF used to describe the grammar closely follows the variant used in [*Crafting Interpreters* by Robert Nystrom](https://craftinginterpreters.com/).

``nil`` or ``()`` will be parsed as an empty list and evaluated as nil, rather than grabbed via Scanner. This is done to avoid odd edge cases; consider ``(set something '()``, where the ``()`` after the single quote would be incorrectly parsed out as ``nil``.

### Base Grammar
This is the most general form of Yisp, providing a general outline for all that is to follow.
```ebnf
yisp  = sexpr yisp | epsilon
sexpr = atom | list
atom  = number | symbol | string
list  = "(" ( sexpr )? ")"
```

### Expanded Grammar
From the base grammar, we need to strictly define how to interpret the different forms of a list; lists can represent ``nil`` with ``()``, a function call with ``(func-name args)``, and a built-in operation with ``(op args)``. With this, we also need to expand the definition of a symbol to contain keywords and identifiers. So, the grammar is expanded to:
```ebnf
symbol    = keyword | identifier
list      = "(" nil | function | operation | basiclist ")"
nil       = epsilon
function  = identifier ( sexpr )?
operation = (operator | keyword) ( sexpr )?
basiclist = ( sexpr )+
```
There is likely a better way to define ``nil``, but this will work for now. Additionally, this format is still vague, but will be a good jumping-off point for implementing basic logic; some built-in operations will need further definition, such as ``define``.


Keywords and operators are defined from the project instructions as:
```ebnf
operator = "+" | "-" | "*" | "/" | "<" | ">" | "="
keyword  = "define" | "set" | "cons" | "cond" | "car" | "cdr" | "and?" | "or?" | "not?" | "number?" | "symbol?" | "list?" | "nil?" | "eq?"
```

Let's break down our operations into unary, binary, and ternary operations.
Lucky enough, the only built-in ternary operation, so we can specify it a bit more and write it as ``define``. We'll do the same with ``set`` and ``cond``, since they're fairly unique.
We'll also section these bits out into expressions (things that produce a value) and statements (control flow and things that don't produce a value on their own)
```ebnf
operation = unary | binary | ternary

// Expressions
unary = unaryops sexpr
unaryops = "car" | "cdr" | "number?" | "symbol?" | "not?" | "list?" | "nil?"

binary = binaryops sexpr sexpr
binaryops = "+" | "-" | "*" | "/" | "<" | ">" | "=" | "cons" | "and?" | "or?"

// Statements & Control Flow
define = "define" symbol arglist sexpr
arglist = "(" ( symbol )? ")"

set = "set" symbol sexpr

cond = "cond" ( condpair )+
condpair = "(" sexpr sexpr ")"
```
