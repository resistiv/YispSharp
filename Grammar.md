# Yisp Grammar
This document will be used to define and iterate upon the structure of Yisp's grammar during the development process of Y#. The variant of EBNF used to describe the grammar closely follows the variant used in [*Crafting Interpreters* by Robert Nystrom](https://craftinginterpreters.com/).

``nil`` or ``()`` will be parsed as an empty list and evaluated as nil, rather than grabbed via Scanner. This is done to avoid odd edge cases; consider ``(set something '()``, where the ``()`` after the single quote would be incorrectly parsed out as ``nil``.

```ebnf
yisp  Å® sexpr yisp | É√
sexpr Å® atom | list
atom  Å® number | symbol | string
list  Å® "(" ( sexpr )? ")"
```