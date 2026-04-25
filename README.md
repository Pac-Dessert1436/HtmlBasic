# `HtmlBasic` - BASIC-to-HTML Transpiler Tailored for Web Development

> **Note:** This project is still in the alpha stage of development, see [Current Status of the Project](#current-status-of-the-project) for more details.

## Description
`HtmlBasic` is a transpiler that converts BASIC code into HTML, focusing on web development. Inspired by @DualBrain's [JsBasic](https://github.com/DualBrain/JsBasic) project, it is designed to be easy to create HTML webpages with embedded JavaScript. The transpiler is written in VB.NET and uses the Irony library for parsing BASIC code, but from my perspective, the BASIC language itself needs to be overhauled to support web development. *__The overhauled version of the language is still based on GW-BASIC, and will be renamed HTML-BASIC.__*

HTML-BASIC is designed to lower the barrier for two key groups: **beginner developers** familiar with BASIC (GW-BASIC, VB6, VB.NET, etc.) who want to transition to web development without learning JavaScript/HTML from scratch, and **rapid prototypers** needing to build simple interactive web pages (e.g., form tools, demo interfaces, educational widgets) in minutes.

While JavaScript is a great language for web development, and BASIC shines for its beginner-friendliness on personal computers, HTML-BASIC merges these strengths using BASIC's intuitive syntax. The expected workflow is intentionally straightforward: write HTML-BASIC code in a `.bas` file, run the transpiler to generate a self-contained HTML file (with embedded transpiled JavaScript), and open the HTML file directly in any modern web browser. No additional build tools or dependencies required.

## Current Status of the Project
**⚠️ Work in Progress - Not Production-Ready**: This project is currently in active development and should be considered experimental. The core transpiler functionality is being built and tested, but it is not yet ready for production use.

### Recent Changes
- **Switched to Original Irony.dll**: _After encountering issues with the extracted Irony codebase, the project has been reverted to using the original, stable Irony.dll library._ This resolves the conversion errors and parsing issues that were previously blocking development.
- **Basic Parsing Working**: The transpiler can now successfully parse simple BASIC programs without the previous string conversion errors.
- **Grammar Development**: The BASIC grammar definition is being refined to properly handle HTML-BASIC's extended syntax features.

### Current Capabilities
✅ **Working Features:**
- Line number and expression parsing
- AST nodes from the original `JsBasic` project
- Rudimentary JavaScript code generation

🔄 **In Development:**
- HTML element integration
- Event handler, attribute binding and lambda expressions
- Extended language features (structs, enums, functions)
- Complete grammar coverage
- Full JavaScript code generation

### Next Steps
1. Complete the HTML-BASIC grammar implementation
2. Create comprehensive test suite
3. Develop documentation and examples

### Known Limitations
- The transpiler currently only handles very simple BASIC programs
- HTML integration features are not yet implemented
- Error handling and reporting need improvement
- Performance optimization is pending

**Note**: This project is ideal for contributors interested in language design, transpiler development, or BASIC/web development integration. Feedback and contributions are welcome!

## Differentiation from `JsBasic`
While inspired by JsBasic, a BASIC-to-JavaScript transpiler, HTML-BASIC might stand out in three core ways:

**Web-Centric Syntax**: It bakes in native support for HTML elements and event handlers, avoiding the need to wrap raw JavaScript/DOM calls in BASIC.

**Modern Language Features**: Unlike JsBasic retaining a more traditional BASIC structure, or other vintage BASIC tools, HTML-BASIC adds structs, enums, first-class functions, and lambda expressions, bridging classic BASIC's simplicity with modern programming paradigms. 

**Zero-Configuration Output**: Transpilation directly produces ready-to-run HTML files (not just JavaScript snippets), eliminating the extra step of manually embedding code into HTML or linking external files. This makes it uniquely suited for users prioritizing speed and ease of use for small-to-medium web projects.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.