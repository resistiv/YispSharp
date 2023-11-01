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
list      = "(" nil | function | operation ")"
nil       = epsilon
function  = identifier ( sexpr )?
operation = (operator | keyword) ( sexpr )?
```
There is likely a better way to define ``nil``, but this will work for now. Additionally, this format is still vague, but will be a good jumping-off point for implementing basic logic; some built-in operations will need further definition, such as ``define``.


Keywords and operators are defined from the project instructions as:
```ebnf
operator = "+" | "-" | "*" | "/" | "<" | ">" | "="
keyword  = "define" | "set" | "cons" | "cond" | "car" | "cdr" | "and?" | "or?" | "not?" | "number?" | "symbol?" | "list?" | "nil?" | "eq?"
```
