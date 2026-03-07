# `HtmlBasic` - BASIC-to-HTML Transpiler Tailored for Web Development

## Description
`HtmlBasic` is a transpiler that converts BASIC code into HTML, focusing on web development. Inspired by @DualBrain's [JsBasic](https://github.com/DualBrain/JsBasic) project, it is designed to be easy to create HTML webpages with embedded JavaScript. The transpiler is written in VB.NET and uses the Irony library for parsing BASIC code, but from my perspective, the BASIC language itself needs to be overhauled to support web development. *__The overhauled version of the language is still based on GW-BASIC, and will be renamed HTML-BASIC.__*

HTML-BASIC is designed to lower the barrier for two key groups: **beginner developers** familiar with BASIC (GW-BASIC, VB6, VB.NET, etc.) who want to transition to web development without learning JavaScript/HTML from scratch, and **rapid prototypers** needing to build simple interactive web pages (e.g., form tools, demo interfaces, educational widgets) in minutes.

While JavaScript is a great language for web development, and BASIC shines for its beginner-friendliness on personal computers, HTML-BASIC merges these strengths using BASIC's intuitive syntax. The expected workflow is intentionally straightforward: write HTML-BASIC code in a `.bas` file, run the transpiler to generate a self-contained HTML file (with embedded transpiled JavaScript), and open the HTML file directly in any modern web browser. No additional build tools or dependencies required.

## Current Status of the Project
**HTML-BASIC is currently in the alpha stage of development, with new features actively being implemented. However, the codebase has unresolved issues that prevent the transpiler from being fully operational. It is anticipated that resolving these challenges and stabilizing the codebase will take approximately one month or more.**

A key design adjustment is now being finalized for keyword usage, aiming to align the language with modern programming conventions while avoiding the introduction of new keywords. Specifically:  
- The `RETURN` keyword, which in traditional GW-BASIC was exclusively paired with `GOSUB` for subroutine returns, will be repurposed to handle return values for functions defined via `DEF FN` or subroutines via `DEF SUB`.  
- To maintain consistency with this new usage of `RETURN` and uphold modern coding practices:  
  1. The `GOSUB` keyword is prohibited within the body of any `DEF FN`/`DEF SUB` block;  
  2. The `GOTO` keyword must not be used to jump from inside a function/subroutine body to external code, nor shall global-level `GOTO` statements incorrectly branch into the scope of a function/subroutine.

This adjustment preserves backward compatibility with core BASIC syntax while refining control flow semantics to match contemporary programming expectations, eliminating the need for additional keywords and reducing syntactic ambiguity.

## Differentiation from `JsBasic`
While inspired by JsBasic, a BASIC-to-JavaScript transpiler, HTML-BASIC might stand out in three core ways:

**Web-Centric Syntax**: It bakes in native support for HTML elements and event handlers, avoiding the need to wrap raw JavaScript/DOM calls in BASIC.

**Modern Language Features**: Unlike JsBasic retaining a more traditional BASIC structure, or other vintage BASIC tools, HTML-BASIC adds structs, enums, first-class functions, and lambda expressions, bridging classic BASIC's simplicity with modern programming paradigms. 

**Zero-Configuration Output**: Transpilation directly produces ready-to-run HTML files (not just JavaScript snippets), eliminating the extra step of manually embedding code into HTML or linking external files. This makes it uniquely suited for users prioritizing speed and ease of use for small-to-medium web projects.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
