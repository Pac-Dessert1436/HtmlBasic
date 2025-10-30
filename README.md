# `HtmlBasic` - BASIC-to-HTML Transpiler Tailored for Web Development

_**Note:** This project is just my sudden inspiration, though I don't want to let such a good idea go to waste. So far I have managed to make some progress about preparing for Postgraduate Entrance Exam, but my anxious feeling tells me that I ought to stick to it until the end of this year, so I simply upload this project and set the project aside for the time being. **The actual content of the project is subject to change in the future, but now it's enough to refill my dopamine - the next thing I will do is memorize all the key points for the exam like there's no tomorrow.**_

## Description
`HtmlBasic` is a transpiler that converts BASIC code into HTML, focusing on web development. Inspired by @DualBrain's [JsBasic](https://github.com/DualBrain/JsBasic) project, it is designed to be easy to create HTML webpages with embedded JavaScript. The transpiler is written in VB.NET and uses the Irony library for parsing BASIC code, but from my perspective, the BASIC language itself needs to be overhauled to support web development. *__The overhauled version of the language is still based on GW-BASIC, and will be renamed HTML-BASIC.__*

HTML-BASIC is designed to lower the barrier for two key groups: **beginner developers** familiar with BASIC (GW-BASIC, VB6, VB.NET, etc.) who want to transition to web development without learning JavaScript/HTML from scratch, and **rapid prototypers** needing to build simple interactive web pages (e.g., form tools, demo interfaces, educational widgets) in minutes.

While JavaScript is a great language for web development, and BASIC shines for its beginner-friendliness on personal computers, HTML-BASIC merges these strengths using BASIC's intuitive syntax. The expected workflow is intentionally straightforward: write HTML-BASIC code in a `.bas` file, run the transpiler to generate a self-contained HTML file (with embedded transpiled JavaScript), and open the HTML file directly in any modern web browser. No additional build tools or dependencies required.

## Why Is the Project Suspended
There are less than two months for my preparation for Postgraduate Entrance Exam, so the programming logic of the `JsBasic` project remains unchanged. The only thing I do for the time being is re-organize the namespaces of the codebase, upload this project, and preserve such a good idea on GitHub until I complete the exam - *__any extra hours spent coding would take away from my exam preparation, and Politics, together with TCM-related subjects, has been a tough nut to crack for me when preparing for the exam.__* By the way, the exam also includes English, but I don't have to worry about my English vocabulary.

## Differentiation from `JsBasic`
While inspired by JsBasic, a BASIC-to-JavaScript transpiler, HTML-BASIC might stand out in three core ways:

**Web-Centric Syntax**: It bakes in native support for HTML elements and event handlers, avoiding the need to wrap raw JavaScript/DOM calls in BASIC.

**Modern Language Features**: Unlike JsBasic retaining a more traditional BASIC structure, or other vintage BASIC tools, HTML-BASIC adds structs, enums, first-class functions, and lambda expressions, bridging classic BASIC's simplicity with modern programming paradigms. 

**Zero-Configuration Output**: Transpilation directly produces ready-to-run HTML files (not just JavaScript snippets), eliminating the extra step of manually embedding code into HTML or linking external files. This makes it uniquely suited for users prioritizing speed and ease of use for small-to-medium web projects.

## HTML-BASIC Language Specification
HTML-BASIC is still based on GW-BASIC but with quite a few modifications, which include adding support for HTML elements, attributes, and events. In addition, first-class functions/subroutines, together with structs and enums, will be also supported as well.

The three common keywords `INPUT`, `PRINT`, and `MSGBOX` will be adjusted for HTML-BASIC:
| HTML-BASIC Syntax | Transpiled JavaScript |
| ----------------- | --------------------- |
| `INPUT prompt$; var` | `let var_ = prompt(prompt_);` |
| `PRINT var1, var2, ...` | `console.log(var1, " ", var2, " ", ...);` |
| `MSGBOX message$` | `alert(message);` |

It is recommended that the HTML-BASIC code be written in snake_case style (except for structs and enums, which are written in PascalCase style), but whether you prefer snake_case or camelCase when defining variables and functions, they are always converted to camelCase in the transpiled JavaScript code.

### Primitive Types in HTML-BASIC
`LET` and `CONST` keywords are used to define variables of primitive types. `LET` is used to define a mutable variable such as `LET x% = 3`, while `CONST` is used to define a constant like `CONST PI! = 3.1415926535`. Primitive types in HTML-BASIC, together with their type characters, are listed as follows:

| Type Character | Type Name |
| -------------- | --------- |
| `%`            | Integer   |
| `!`            | Single    |
| `#`            | Double    |
| `&`            | Long      |
| `$`            | String    |

> Note: Values of the type "Integer" or "Long", when transpiled into JavaScript, will be wrapped into the `Math.floor` function.

Variables defined with `LET` and `CONST`, without a type character, are considered as double-precision floating-point numbers, so the string variables must have the `$` suffix. Boolean values are defined as integer constants, where `TRUE` equals -1 and `FALSE` equals 0. Conversion from boolean to integers will generate this function in the transpiled JavaScript code:
``` js
function _boolToInt(expression) {
    return expression ? -1 : 0;
}
```

### Defining and Handling Arrays
Arrays in HTML-BASIC are defined using the `DIM` keyword. The following code shows an example of defining and handling arrays of integers:
``` basic
10 DIM arr%(10)
20 FOR i% = 0 TO 9 : arr(i) = i + 1 : NEXT i
30 arr(5) = arr(3) + 1
40 FOR itm IN arr : PRINT itm : NEXT itm
50 REM The above code outputs 1, 2, 3, 4, 5, 5, 7, 8, 9, 10 line by line.
60 ERASE arr      ' Set the array to 0 elements
70 REDIM arr(20)  ' Resize the array (type remains the same)
```

Arrays can be also created using braces, and the type of the array elements must be the same as the array type. Arrays can be also modified using the `insert`, `append`, and `delete` methods.
``` basic
10 DIM floats! = {1.0!, 2.0!, 3.0!, 4.0!, 5.0!}
20 floats.insert 2, 2.5! 
30 floats.append 5.5!
40 floats.delete 1.0!
50 FOR itm IN floats : PRINT itm : NEXT itm
60 REM Similar to VB.NET's "For Each", the array elements being looped through cannot be modified.
60 REM The above code outputs 2, 2.5, 3, 4, 5, 5.5 line by line.
70 DIM new_arr! = floats.map(FN(x) x * 1.5)
80 FOR itm IN new_arr : PRINT itm : NEXT itm
90 REM The above code outputs 3, 3.75, 4.5, 6, 7.5, 8.25 line by line.
```

The second example transpiled into JavaScript code is as follows:
``` js
const floats = [1.0, 2.0, 3.0, 4.0, 5.0];
floats.splice(2, 0, 2.5);
floats.push(5.5);
floats.splice(floats.indexOf(1.0), 1);
for (const itm of floats) console.log(itm);
// Similar to VB.NET's "For Each", the array elements being looped through cannot be modified.
// The above code outputs 2, 2.5, 3, 4, 5, 5.5 line by line.
const newArr = floats.map(x => x * 1.5);
for (const itm of newArr) console.log(itm);
// The above code outputs 3, 3.75, 4.5, 6, 7.5, 8.25 line by line.
```

### New Syntax for Structs, Enums, and Multi-line Functions
Structs and enums are two important features in BASIC that are not supported in GW-BASIC. In HTML-BASIC, structs and enums are supported with a new syntax. The new syntax features "member functions (`M_FN`)" and "member subroutines (`M_SUB`)" for structs, similar to prototypes in the early version of JavaScript, and the struct supports the `to_string`, `equals` and `clone` methods in itself. A new instance of a struct, when created using the `SET` statement, can be actually modified, but its type cannot be changed.

``` basic
10 DEF STRUCT Vi2d(x%, y%)       ' Unlike VB.NET, structs in HTML-BASIC are reference types
20 M_LET Vi2d.name$ = "Player"   ' Member property defined with `M_LET`
30 DEF M_FN Vi2d.add(x%, y%) = NEW Vi2d(ME.x + x, ME.y + y)
40 DEF FN Vi2d.dist!(vec1, vec2)  ' Static methods are defined using `FN` keyword
50 LET sqr_diff_x = (vec1.x - vec2.x) ^ 2
60 LET sqr_diff_y = (vec1.y - vec2.y) ^ 2
70 RESULT= SQRT(sqr_diff_x + sqr_diff_y)  ' Use `RESULT=` to return a value
80 END DEF
90 SET p1 = NEW Vi2d(3, 5) : p1.name = "Alice"
100 PRINT p1.x, p1.y, p1.name  ' Output: 3 5 Alice
110 p1 = NEW Vi2d(2, 7)
120 PRINT p1.x, p1.y, p1.name  ' Output: 2 7 Player
130 SET p2 = p1.clone()
140 PRINT p1.equals(p2)  ' Output: True
150 p2.x = p2.x + 1
160 PRINT p1.equals(p2)  ' Output: False
170 p2 = NEW Button      ' Error: "Button" differs from "Vi2d"
```
The above HTML-BASIC code transpiles into the following JavaScript code, only when the compilation error is eliminated:
``` js
function Vi2d(x, y) {
    this.x = x;
    this.y = y;
    this.toString = () => "Vi2d(" + this.x + ", " + this.y + ")";
    this.equals = (other) => this.x === other.x && this.y === other.y;
    this.clone = () => new Vi2d(this.x, this.y);
}
Vi2d.prototype.name = "Player";
Vi2d.prototype.add = (x, y) => new Vi2d(this.x + x, this.y + y);
Vi2d.dist = (vec1, vec2) => {
    let sqrDiffX = Math.pow(vec1.x - vec2.x, 2);
    let sqrDiffY = Math.pow(vec1.y - vec2.y, 2);
    return Math.sqrt(sqrDiffX + sqrDiffY);
};
let p1 = new Vi2d(3, 5); p1.name = "Alice";
console.log(p1.x, p1.y, p1.name);
p1 = new Vi2d(2, 7);
console.log(p1.x, p1.y, p1.name);
const p2 = p1.clone();
console.log(p1.equals(p2));
p2.x += 1;
console.log(p1.equals(p2));
```

On a related note, the `KEY` keyword is used to define a read-only field of a struct. The field can be accessed like a regular one, but cannot be modified. For instance, if you write `p1.x = 10` after `DEF STRUCT Vf2d(KEY x!, KEY y!)`, it will throw a compilation error.

HTML-BASIC provides a new way to define functions and subroutines, either single-line or multi-line. The new syntax features the `DEF FN`, `DEF SUB` keywords for defining a function/subroutine, and the `END DEF` keyword for ending the function/subroutine. The following code shows an example of all the new syntax, but there are a few more key points:
- A struct can have no fields, like `DEF STRUCT MyStruct()`. Creating a new instance of a struct with no fields makes parentheses optional, like `SET obj = NEW MyStruct`.
- The `NEW` keyword is used to create a new instance of a struct.
- The `ME` keyword is used to refer to the current instance of a struct.
- Single-line function: `DEF FN name(...params) = expression`
- Single-line subroutine: `DEF SUB name(...params) = action`
- The `RESULT=` keyword is used to return a value from a function, and `PASS` to exit a subroutine. For example, `IF x > 10 THEN PASS` transpiled in JavaScript is `if (x > 10) return;`
- `GOTO`, `GOSUB` and `RETURN` is still supported, but you can neither jump from the global code directly into the function bodies, nor jump out of the functions/subroutines when using these keywords inside them.

``` basic
10 DEF STRUCT Vf2d(x!, y!)  ' A struct for 2D floating-point vector
20 DEF ENUM Elements(WOOD, FIRE, EARTH, METAL, WATER)
30 DEF M_FN Vf2d.add(x!, y!) = NEW Vf2d(ME.x + x, ME.y + y)
40 DEF M_SUB Vf2d.move(x!, y!)
50 ME.x = ME.x + x
60 ME.y = ME.y + y
70 END DEF
80 SET p1 = NEW Vf2d(3.2, 5.6) : p1.move -1.5, -2.5
90 SET p2 = p1.add(2.5, 3.5)
100 PRINT "p2 = ", p2  ' The `to_string` method is called automatically
110 DEF FN element_name$(element%)
120 IF element = Elements.WOOD THEN RESULT= "wood"
130 IF element = Elements.FIRE THEN RESULT= "fire"
140 IF element = Elements.EARTH THEN RESULT= "earth"
150 IF element = Elements.METAL THEN RESULT= "metal"
160 IF element = Elements.WATER THEN RESULT= "water"
170 RESULT= "unknown"
180 END DEF
190 PRINT element_name(Elements.WATER)
```
The above BASIC code will be transpiled into the following HTML webpage:
``` html
<!DOCTYPE html>
<html>

<body>
</body>

<script>
    function Vf2d(x, y) {
        this.x = x;
        this.y = y;
        this.toString = () => `Vf2d(${this.x}, ${this.y})`;
    }
    const Elements = {
        WOOD: 0,
        FIRE: 1,
        EARTH: 2,
        METAL: 3,
        WATER: 4
    };
    Vf2d.prototype.add = (x, y) => new Vf2d(this.x + x, this.y + y);
    Vf2d.prototype.move = (x, y) => {
        this.x += x;
        this.y += y;
    }
    const p1 = new Vf2d(3.2, 5.6); p1.move(-1.5, -2.5);
    const p2 = p1.add(2.5, 3.5);
    console.log("p2 = ", p2.toString());
    function elementName(element) {
        if (element === Elements.WOOD) return "wood";
        if (element === Elements.FIRE) return "fire";
        if (element === Elements.EARTH) return "earth";
        if (element === Elements.METAL) return "metal";
        if (element === Elements.WATER) return "water";
        return "unknown";
    }
    console.log(elementName(Elements.WATER));
</script>

</html>
```

### Functions/Subroutines as First-Class Objects

Functions and subroutines are first-class objects in HTML-BASIC, and variables of this type have the `@` suffix. They can be assigned to variables, passed as arguments to other functions, and returned as values from other functions. Lambda expressions (single-line only) can be used to create anonymous functions, which are useful for short and simple functions.
``` basic
10 DEF FN mult_add!(a!, b!) = FN(x!) a * x + b 
20 REM The above is a single-line function that returns a lambda expression.
30 DEF FN add#(a#, b#) = a + b
40 DEF FN compute#(func@, x#, y#) = func(x, y)
50 SET subtract@ = FN(x!, y!) x - y    ' Lambda expressions are reference types
60 PRINT mult_add(2!, 3!)(4!)          ' Output: 10
70 PRINT compute(FN add, 4.0, 5.0)     ' Output: 9
80 PRINT compute(subtract, 4.0, 5.0)   ' Output: -1
```

The above BASIC code will be transpiled into the following JavaScript code:
``` js
const multAdd = (a, b) => ((x) => a * x + b);
const add = (a, b) => a + b;
const compute = (func, x, y) => func(x, y);
const subtract = (a, b) => a - b;
console.log(compute(multAdd(2, 3), 4));  // Output: 10.0
console.log(compute(add, 4.0, 5.0));  // Output: 9.0
console.log(compute(subtract, 4.0, 5.0));  // Output: -1.0
```

## A Fresh Start for Web Development
This example shows how to create a simple button element in HTML using the `HtmlBasic` transpiler. All the elements created in the BASIC code will be automatically added to the `body` element, and the transpiled JavaScript code will be wrapped into the `script` tag.
``` basic
10 SET btn = NEW Button : SET lbl = NEW Label
20 btn.locate 50, 50 
30 lbl.locate 50, 80
40 btn.set_text "Click me"
50 btn.set_bgcolor "lightgreen"
60 btn.bind_hover btn, Prop.BGCOLOR, "darkgreen"
70 btn.bind_hover btn, Prop.FGCOLOR, "white"
80 btn.bind_hover lbl, Prop.TEXT, "Nice to meet you!"
90 btn.bind_leave btn, Prop.BGCOLOR, "lightgreen"
100 btn.bind_leave btn, Prop.FGCOLOR, "black"
110 btn.bind_leave lbl, Prop.TEXT, ""
120 btn.bind_click lbl, Prop.TEXT, "Button clicked!"
130 btn.on_click SUB() MSGBOX "Have a wonderful day!"
```
The above BASIC code will be transpiled into the following HTML webpage:
``` html
<!DOCTYPE html>
<html>

<body>
</body>

<script>
    const btn = document.createElement("button");
    document.body.appendChild(btn);
    const lbl = document.createElement("label");
    document.body.appendChild(lbl);
    btn.style.position = "absolute";
    btn.style.left = "50px";
    btn.style.top = "50px";
    lbl.style.position = "absolute";
    lbl.style.left = "50px";
    lbl.style.top = "80px";
    btn.textContent = "Click me";
    btn.style.backgroundColor = "lightgreen";
    btn.onmouseover = () => {
        btn.style.backgroundColor = "darkgreen";
        btn.style.color = "white";
        lbl.textContent = "Nice to meet you!";
    }
    btn.onmouseout = () => {
        btn.style.backgroundColor = "lightgreen";
        btn.style.color = "black";
        lbl.textContent = "";
    }
    btn.onclick = () => {
        lbl.textContent = "Button clicked!";
        alert("Have a wonderful day!");
    }
</script>

</html>
```

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
