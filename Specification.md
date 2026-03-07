# HTML-BASIC Language Specification

HTML-BASIC is a modern programming language that combines the simplicity of classic BASIC with web development capabilities. It transpiles to JavaScript and HTML, making it ideal for creating web applications with a familiar BASIC-like syntax.

## Table of Contents
1. [Overview](#overview)
2. [Basic Syntax](#basic-syntax)
3. [Primitive Types](#primitive-types)
4. [Variables and Constants](#variables-and-constants)
5. [Arrays](#arrays)
6. [Control Flow](#control-flow)
7. [Functions and Subroutines](#functions-and-subroutines)
8. [Structs and Enums](#structs-and-enums)
9. [HTML Elements and Web Development](#html-elements-and-web-development)
10. [First-Class Functions](#first-class-functions)
11. [Error Handling](#error-handling)
12. [Standard Library](#standard-library)
13. [Code Examples](#code-examples)

## Overview

HTML-BASIC maintains the line-numbered approach of classic BASIC while adding modern features like:
- Object-oriented capabilities through structs
- First-class functions and lambda expressions
- Direct HTML element manipulation
- Event handling for web interfaces
- Modern control structures

The language is case-insensitive and transpiles to JavaScript, which runs in any modern web browser.

## Basic Syntax

HTML-BASIC code is organized into numbered lines, similar to classic BASIC:

```basic
10 REM This is a comment
20 LET x% = 5
30 PRINT "The value is:", x%
```

Multiple statements can be combined on a single line using the colon (`:`) separator:

```basic
10 LET a = 1 : LET b = 2 : PRINT a + b
```

Line numbers are optional for modern code style:

```basic
LET name$ = "HTML-BASIC"
PRINT "Welcome to", name$
```

## Primitive Types

HTML-BASIC supports the following primitive types, identified by type suffixes:

| Type Suffix | Type Name | JavaScript Equivalent |
| ----------- | --------- | -------------------- |
| `%`         | Integer   | `Math.floor(number)` |
| `&`         | Long      | `Math.floor(number)` |
| `!`         | Single    | `number`             |
| `#`         | Double    | `number`             |
| `$`         | String    | `string`             |

If no type suffix is specified, the variable is assumed to be a Double (floating-point number).

### Boolean Values

Boolean values are represented as integers:
- `TRUE` equals `-1`
- `FALSE` equals `0`

When transpiled to JavaScript, boolean to integer conversion uses this helper function:
```javascript
function _boolToInt(expression) {
    return expression ? -1 : 0;
}
```

## Variables and Constants

### Variables

Variables are declared using the `LET` keyword:

```basic
10 LET count% = 10          ' Integer variable
20 LET price! = 19.99        ' Single precision variable
30 LET name$ = "Product"     ' String variable
40 LET total = price * count ' Double precision (default)
```

### Constants

Constants are declared using the `CONST` keyword:

```basic
10 CONST PI! = 3.1415926535
20 CONST MAX_USERS% = 100
30 CONST APP_NAME$ = "MyApp"
```

### Naming Conventions

It is recommended to use snake_case for variable names, but both snake_case and camelCase are supported. All identifiers are converted to camelCase in the transpiled JavaScript code.

Struct and enum names should use PascalCase.

## Arrays

Arrays are defined using the `DIM` keyword:

```basic
10 DIM numbers%(10)         ' Array of 11 integers (0-10)
20 DIM names$(5)            ' Array of 6 strings
30 DIM values(20)           ' Array of 21 doubles (default)
```

### Array Operations

Arrays can be manipulated using various methods:

```basic
10 DIM arr%(10)
20 FOR i% = 0 TO 9 : arr%(i) = i + 1 : NEXT i
30 arr%(5) = arr%(3) + 1
40 FOR itm IN arr% : PRINT itm : NEXT itm
50 ERASE arr%               ' Set array to 0 elements
60 REDIM arr%(20)           ' Resize array (type remains the same)
```

### Array Literals

Arrays can be created using brace notation:

```basic
10 DIM floats! = {1.0!, 2.0!, 3.0!, 4.0!, 5.0!}
20 floats!.insert 2, 2.5!   ' Insert 2.5 at index 2
30 floats!.append 5.5!      ' Add 5.5 to the end
40 floats!.delete 1.0!      ' Remove first occurrence of 1.0
50 FOR itm IN floats! : PRINT itm : NEXT itm
```

### Array Methods

HTML-BASIC arrays support the following methods:
- `insert(index, value)` - Insert a value at the specified index
- `append(value)` - Add a value to the end of the array
- `delete(value)` - Remove the first occurrence of a value
- `map(function)` - Create a new array by applying a function to each element

## Control Flow

### IF...THEN...ELSE

```basic
10 IF x > 0 THEN PRINT "Positive"
20 IF x < 0 THEN 
30   PRINT "Negative"
40 ELSE
50   PRINT "Zero"
60 END IF
```

### FOR...NEXT

```basic
10 FOR i% = 1 TO 10
20   PRINT i%
30 NEXT i%
```

With a step value:

```basic
10 FOR i% = 0 TO 100 STEP 10
20   PRINT i%
30 NEXT i%
```

### WHILE...WEND

```basic
10 WHILE count < 10
20   PRINT count
30   count = count + 1
40 WEND
```

### SELECT Statement

The `SELECT` statement is similar to VB.NET's "Select Case":

```basic
10 SELECT grade$
20 CASE "A" : PRINT "Excellent"
30 CASE "B" : PRINT "Good"
40 CASE "C" : PRINT "Average"
50 DEFAULT : PRINT "Needs improvement"
60 END SELECT
```

### GOTO, GOSUB, and RETURN

```basic
10 GOTO 100
20 PRINT "This line is skipped"
30 END
100 PRINT "Jumped to this line"
110 GOSUB 200
120 PRINT "Back from subroutine"
130 END
200 PRINT "In subroutine"
210 RETURN
```

Note that the `GOSUB` keyword is **prohibited** within the body of any `DEF FN`/`DEF SUB` block, and the `GOTO` keyword **must not** be used to jump from inside a function/subroutine body to external code, nor shall global-level `GOTO` statements incorrectly branch into the scope of a function/subroutine.

## Functions and Subroutines

### Single-Line Functions

```basic
10 DEF FN add#(a#, b#) = a + b
20 DEF FN square%(x%) = x * x
```

### Multi-Line Functions

```basic
10 DEF FN factorial%(n%)
20 IF n% <= 1 THEN
30   RETURN 1
40 ELSE
50   RETURN n% * factorial(n% - 1)
60 END IF
70 END DEF
```

### Subroutines

```basic
10 DEF SUB print_header(title$)
20 PRINT "====", title$, "===="
30 END DEF
40 CALL print_header("My Report")
```

### Function Return Values

The `RETURN` keyword is used to return a value from a function or exit a subroutine:

```basic
10 DEF FN is_positive(num) 
20 IF num > 0 THEN
30   RETURN TRUE
40 ELSE
50   RETURN FALSE
60 END IF
70 END DEF
```

## Structs and Enums

### Structs

Unlike VB.NET, structs are reference types with fields and methods:

```basic
10 DEF STRUCT Point(x%, y%)
20 DEF M_FN Point.add(other) = NEW Point(ME.x + other.x, ME.y + other.y)
30 DEF M_SUB Point.move(dx%, dy%)
40   ME.x = ME.x + dx%
50   ME.y = ME.y + dy%
60 END DEF
70 END STRUCT
```

#### Member Properties

Member properties are defined with `M_LET`:

```basic
10 DEF STRUCT Person(name$)
20 M_LET Person.age% = 0
30 END STRUCT
40 SET p = NEW Person("John")
50 p.age = 30
```

#### Read-Only Fields

The `KEY` keyword defines read-only fields:

```basic
10 DEF STRUCT Product(KEY id$, name$)
20 M_LET Product.price! = 0.0
30 END STRUCT
40 SET item = NEW Product("123", "Widget")
50 REM item.id = "456"  ' Error: Cannot modify read-only field
```

#### Built-in Methods

All structs automatically have these methods:
- `to_string()` - Returns a string representation
- `equals(other)` - Compares two structs for equality
- `clone()` - Creates a copy of the struct

### Enums

Enums define a set of named constants:

```basic
10 DEF ENUM Colors{RED, GREEN, BLUE}
20 DEF ENUM Status{ACTIVE=1, INACTIVE, PENDING}
```

Enum values are accessed using dot notation:

```basic
10 LET c = Colors.RED
20 IF status = Status.ACTIVE THEN PRINT "Active"
```

## HTML Elements and Web Development

HTML-BASIC provides direct access to HTML elements for web development:

### Creating Elements

```basic
10 SET btn = NEW Button
20 SET lbl = NEW Label
30 SET div = NEW Div
```

### Positioning Elements

```basic
10 btn.locate 50, 50      ' x=50, y=50
20 lbl.locate 50, 80
```

### Setting Properties

```basic
10 btn.set_text "Click me"
20 btn.set_bgcolor "lightgreen"
30 lbl.set_text "Hello, World!"
```

### Event Handling

```basic
10 btn.on_click SUB() 
20   MSGBOX "Button clicked!"
30 END SUB
40 
50 btn.bind_hover btn, Prop.BGCOLOR, "darkgreen"
60 btn.bind_leave btn, Prop.BGCOLOR, "lightgreen"
```

### Supported HTML Elements

HTML-BASIC supports a wide range of HTML elements:
- Text elements: `H1`, `H2`, `H3`, `H4`, `H5`, `H6`, `P`, `Span`, `Div`
- List elements: `Ul`, `Ol`, `Li`
- Table elements: `Table`, `Tr`, `Td`, `Th`
- Form elements: `Form`, `Input`, `Button`, `[Select]`, `Option`
- Semantic elements: `Header`, `Footer`, `Nav`, `Section`, `Article`, `Aside`
- Other elements: `A`, `Img`, `Br`, `Hr`, `Strong`, `Em`, `Code`, `Pre`

## First-Class Functions

Functions and subroutines are first-class objects in HTML-BASIC, identified by the `@` suffix:

```basic
10 DEF FN mult_add!(a!, b!) = FN(x!) a * x + b
20 DEF FN add#(a#, b#) = a + b
30 DEF FN compute#(func@, x#, y#) = func(x, y)
40 SET subtract@ = FN(x!, y!) x - y
50 PRINT mult_add(2!, 3!)(4!)      ' Output: 10
60 PRINT compute(FN add, 4.0, 5.0)   ' Output: 9
70 PRINT compute(subtract, 4.0, 5.0) ' Output: -1
```

Lambda expressions (single-line only) can be used to create anonymous functions:

```basic
10 SET double@ = FN(x) x * 2
20 PRINT double(5)  ' Output: 10
```

## Error Handling

HTML-BASIC provides basic error handling through the `ON ERROR` statement:

```basic
10 ON ERROR GOTO 100
20 LET x = 1 / 0  ' This will cause an error
30 PRINT "This line won't be reached"
40 END
100 PRINT "Error occurred:", ERR
110 RESUME NEXT
```

## Standard Library

HTML-BASIC includes a standard library of functions for common operations:

### String Functions
- `LEN(string$)` - Returns the length of a string
- `LEFT$(string$, count)` - Returns the leftmost characters
- `RIGHT$(string$, count)` - Returns the rightmost characters
- `MID$(string$, start, count)` - Returns a substring
- `INSTR(start, string$, substring$)` - Finds a substring
- `STR$(number)` - Converts a number to a string
- `VAL(string$)` - Converts a string to a number
- `CHR$(code)` - Returns the character for an ASCII code
- `ASC(char$)` - Returns the ASCII code for a character
- `SPACE$(count)` - Returns a string of spaces
- `STRING$(count, char$)` - Repeats a character

### Math Functions
- `ABS(number)` - Absolute value
- `SQR(number)` - Square root
- `SIN(number)` - Sine
- `COS(number)` - Cosine
- `TAN(number)` - Tangent
- `EXP(number)` - Exponential
- `LOG(number)` - Natural logarithm
- `SGN(number)` - Sign of a number
- `FIX(number)` - Truncate to integer
- `CINT(number)` - Convert to integer
- `RND` - Random number

### System Functions
- `TIMER` - System timer
- `INKEY$` - Read a key from keyboard
- `CSRLIN` - Current cursor line
- `POS` - Current cursor column

## Code Examples

### Example 1: Basic Web Page

```basic
10 SET title = NEW H1
20 title.set_text "Welcome to HTML-BASIC"
30 title.locate 50, 20
40 
50 SET btn = NEW Button
60 btn.set_text "Click Me"
70 btn.locate 50, 60
80 
90 btn.on_click SUB()
100   MSGBOX "Hello from HTML-BASIC!"
110 END SUB
```

### Example 2: Simple Calculator

```basic
10 DEF STRUCT Calculator()
20 M_LET Calculator.result = 0
30 
40 DEF M_FN Calculator.add(num) = ME.result + num
50 DEF M_FN Calculator.subtract(num) = ME.result - num
60 DEF M_FN Calculator.multiply(num) = ME.result * num
70 DEF M_FN Calculator.divide(num) = ME.result / num
80 END STRUCT
90 
100 SET calc = NEW Calculator()
110 calc.result = 10
120 PRINT calc.add(5)        ' Output: 15
130 PRINT calc.subtract(3)   ' Output: 12
140 PRINT calc.multiply(2)   ' Output: 24
150 PRINT calc.divide(4)     ' Output: 6
```

### Example 3: Interactive Form

```basic
10 SET form = NEW Form
20 form.locate 50, 50
30 
40 SET nameLbl = NEW Label
50 nameLbl.set_text "Name:"
60 nameLbl.locate 60, 80
70 
80 SET nameInput = NEW Input
90 nameInput.locate 120, 80
100 
110 SET submitBtn = NEW Button
120 submitBtn.set_text "Submit"
130 submitBtn.locate 60, 110
140 
150 SET resultLbl = NEW Label
160 resultLbl.locate 60, 140
170 
180 submitBtn.on_click SUB()
190   LET name$ = nameInput.get_value()
200   resultLbl.set_text "Hello, " + name$ + "!"
210 END SUB
```

## Transpilation to JavaScript

HTML-BASIC code is transpiled to JavaScript and wrapped in an HTML document. For example:

```basic
10 LET name$ = "World"
20 PRINT "Hello," + name$
```

Transpiles to:

```html
<!DOCTYPE html>
<html>
<body>
</body>
<script>
let name = "World";
console.log("Hello," + name);
</script>
</html>
```

## Conclusion

HTML-BASIC combines the simplicity of classic BASIC with modern web development capabilities. It provides an easy transition for BASIC programmers to create web applications while maintaining a familiar syntax and programming model.