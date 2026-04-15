Imports HtmlBasic.Nodes
Imports HtmlBasic.Irony.Compiler

''' <summary>
''' This class defines the Grammar for the BASIC language.
''' </summary>
Public Class BasicGrammar
  Inherits Grammar

  Public Sub New()

#Region "Init"

    ' BASIC is not case sensitive... 
    CaseSensitive = False

    ' By default, new-line characters are ignored. Because
    ' BASIC uses line breaks to delimit lines, we need to
    ' know where the line breaks are.  The following line
    ' is required for this.
    TokenFilters.Add(New CodeOutlineFilter(False))

    ' Define the Terminals
    Dim number = New NumberLiteral("NUMBER")
    Dim variable = New VariableIdentifierTerminal
    Dim stringLiteral = New StringLiteral("STRING", String.Empty)
    'Important: do not add comment term to
    'base.NonGrammarTerminals list - we do
    'our own comment handling in CodeOutlineFilter
    Dim comment = New CommentTerminal("COMMENT", "'", vbLf)
    Dim comma = Symbol(",", "comma")

    ' Make sure reserved keywords of the BASIC language
    ' aren't mistaken for variables.
    ' Only the keywords ending with '$' could be mistaken
    ' for variables.
    variable.AddKeywords("inkey$", "left$", "right$", "mid$", "chr$", "space$", "str$", "string$")

    ' Define the non-terminals
    Dim PROGRAM = New NonTerminal("PROGRAM", GetType(ProgramNode))
    Dim LINE = New NonTerminal("LINE", GetType(LineNode))
    Dim STATEMENT_LIST = New NonTerminal("STATEMENT_LIST", GetType(StatementListNode))
    Dim STATEMENT = New NonTerminal("STATEMENT", GetType(StatementNode))
    Dim COMMAND = New NonTerminal("COMMAND", GetType(StatementNode)) 'TODO: create command node
    Dim PRINT_STMT = New NonTerminal("PRINT_STMT", GetType(PrintStmtNode))
    Dim INPUT_STMT = New NonTerminal("INPUT_STMT", GetType(InputStmtNode))
    Dim IF_STMT = New NonTerminal("IF_STMT", GetType(IfElseStmtNode)) 'TODO: join IfStmtNode and IfElseStmtNode in one
    Dim ELSE_CLAUSE_OPT = New NonTerminal("ELSE_CLAUSE_OPT", GetType(GenericJsBasicNode))
    Dim EXPR = New NonTerminal("EXPRESSION", GetType(ExpressionNode))
    Dim EXPR_LIST = New NonTerminal("EXPRESSION_LIST", GetType(ExprListNode))
    Dim BINARY_OP = New NonTerminal("BINARY_OP", GetType(BinaryOpNode))
    Dim BINARY_EXPR = New NonTerminal("BINARY_EXPR", GetType(GenericJsBasicNode)) 'TODO: create Binary_expr node
    Dim BRANCH_STMT = New NonTerminal("BRANCH_STMT", GetType(BranchStmtNode))
    Dim ASSIGN_STMT = New NonTerminal("ASSIGN_STMT", GetType(AssignStmtNode))
    Dim FOR_STMT = New NonTerminal("FOR_STMT", GetType(ForStmtNode))
    Dim STEP_OPT = New NonTerminal("STEP_OPT", GetType(GenericJsBasicNode))  'TODO: create step specifier node
    Dim NEXT_STMT = New NonTerminal("NEXT_STMT", GetType(NextStmtNode))
    Dim LOCATE_STMT = New NonTerminal("LOCATE_STMT", GetType(LocateStmtNode))
    Dim WHILE_STMT = New NonTerminal("WHILE_STMT", GetType(WhileStmtNode))
    Dim WEND_STMT = New NonTerminal("WEND_STMT", GetType(WendStmtNode))
    Dim SWAP_STMT = New NonTerminal("SWAP_STMT", GetType(SwapStmtNode))
    Dim GLOBAL_FUNCTION_EXPR = New NonTerminal("GLOBAL_FUNCTION_EXPR", GetType(GlobalFunctionExpr))
    Dim ARG_LIST = New NonTerminal("ARG_LIST", GetType(GenericJsBasicNode))
    Dim FUNC_NAME = New NonTerminal("FUNC_NAME", GetType(GenericJsBasicNode))
    Dim COMMENT_STMT = New NonTerminal("COMMENT_STMT", GetType(RemStmtNode))
    Dim GLOBAL_VAR_EXPR = New NonTerminal("GLOBAL_VAR_EXPR", GetType(GenericJsBasicNode))

    ' New syntax for functions, structs, and enums
    Dim DEF_FN_STMT = New NonTerminal("DEF_FN_STMT", GetType(DefFnStmtNode))
    Dim DEF_SUB_STMT = New NonTerminal("DEF_SUB_STMT", GetType(DefFnStmtNode))
    Dim DEF_STRUCT_STMT = New NonTerminal("DEF_STRUCT_STMT", GetType(DefStructStmtNode))
    Dim DEF_ENUM_STMT = New NonTerminal("DEF_ENUM_STMT", GetType(DefEnumStmtNode))
    Dim FN_BODY = New NonTerminal("FN_BODY", GetType(StatementListNode))
    Dim ENUM_VALUES = New NonTerminal("ENUM_VALUES", GetType(GenericJsBasicNode))
    Dim STRUCT_MEMBERS = New NonTerminal("STRUCT_MEMBERS", GetType(GenericJsBasicNode))

    ' New AST nodes for extended language features
    Dim SELECT_STMT = New NonTerminal("SELECT_STMT", GetType(SelectStmtNode))
    Dim CASE_CLAUSE = New NonTerminal("CASE_CLAUSE", GetType(CaseClauseNode))
    Dim ON_ERROR_STMT = New NonTerminal("ON_ERROR_STMT", GetType(OnErrorStmtNode))
    Dim RESUME_STMT = New NonTerminal("RESUME_STMT", GetType(ResumeStmtNode))
    Dim ARRAY_STMT = New NonTerminal("ARRAY_STMT", GetType(ArrayStmtNode))
    Dim ARRAY_LITERAL = New NonTerminal("ARRAY_LITERAL", GetType(ArrayLiteralNode))
    Dim ARRAY_METHOD = New NonTerminal("ARRAY_METHOD", GetType(ArrayMethodNode))
    Dim FUNCTION_REF = New NonTerminal("FUNCTION_REF", GetType(FunctionRefNode))
    Dim LAMBDA_EXPR = New NonTerminal("LAMBDA_EXPR", GetType(LambdaExprNode))
    Dim FUNCTION_CALL = New NonTerminal("FUNCTION_CALL", GetType(FunctionCallNode))
    Dim MEMBER_PROPERTY = New NonTerminal("MEMBER_PROPERTY", GetType(MemberPropertyNode))
    Dim MEMBER_METHOD = New NonTerminal("MEMBER_METHOD", GetType(MemberMethodNode))
    Dim HTML_ELEMENT_STMT = New NonTerminal("HTML_ELEMENT_STMT", GetType(HtmlElementStmtNode))
    Dim LOCATE_ELEMENT_STMT = New NonTerminal("LOCATE_ELEMENT_STMT", GetType(LocateElementStmtNode))
    Dim SET_PROPERTY_STMT = New NonTerminal("SET_PROPERTY_STMT", GetType(SetPropertyStmtNode))
    Dim EVENT_HANDLER_STMT = New NonTerminal("EVENT_HANDLER_STMT", GetType(EventHandlerStmtNode))
    Dim MSGBOX_STMT = New NonTerminal("MSGBOX_STMT", GetType(MsgBoxStmtNode))
    Dim ERR_VARIABLE = New NonTerminal("ERR_VARIABLE", GetType(ErrVariableNode))

    ' Set the PROGRAM to be the root node of BASIC programs.
    ' A program is a bunch of lines
    Root = PROGRAM

#End Region

#Region "Grammar declaration"

    ' A program is a collection of lines
    PROGRAM.Rule = MakePlusRule(PROGRAM, Nothing, LINE)

    ' A line can be an empty line, or it's a number
    ' followed by a statement list ended by a new-line.
    LINE.Rule = NewLine Or
                number + NewLine Or
                number + STATEMENT_LIST + NewLine Or
                STATEMENT_LIST + NewLine

    ' A statement list is 1 or more statements separated
    ' by the ':' character
    STATEMENT_LIST.Rule = MakePlusRule(STATEMENT_LIST, Symbol(":"), STATEMENT)

    ' A statement can be one of a number of types
    STATEMENT.Rule = EXPR Or
                     ASSIGN_STMT Or
                     PRINT_STMT Or
                     INPUT_STMT Or
                     IF_STMT Or
                     COMMENT_STMT Or
                     BRANCH_STMT Or
                     COMMAND Or
                     FOR_STMT Or
                     NEXT_STMT Or
                     LOCATE_STMT Or
                     SWAP_STMT Or
                     WHILE_STMT Or
                     WEND_STMT Or
                     DEF_FN_STMT Or
                     DEF_SUB_STMT Or
                     DEF_STRUCT_STMT Or
                     DEF_ENUM_STMT Or
                     SELECT_STMT Or
                     ON_ERROR_STMT Or
                     RESUME_STMT Or
                     ARRAY_STMT Or
                     HTML_ELEMENT_STMT Or
                     LOCATE_ELEMENT_STMT Or
                     SET_PROPERTY_STMT Or
                     EVENT_HANDLER_STMT Or
                     MSGBOX_STMT

    ' The different statements are defined here
    PRINT_STMT.Rule = "print" + EXPR_LIST
    INPUT_STMT.Rule = "input" + EXPR_LIST + variable
    IF_STMT.Rule = "if" + EXPR + "then" + STATEMENT_LIST + ELSE_CLAUSE_OPT
    ELSE_CLAUSE_OPT.Rule = Empty Or "else" + STATEMENT_LIST
    BRANCH_STMT.Rule = "goto" + number Or "gosub" + number Or "return"
    ASSIGN_STMT.Rule = variable + "=" + EXPR
    LOCATE_STMT.Rule = "locate" + EXPR + comma + EXPR
    SWAP_STMT.Rule = "swap" + EXPR + comma + EXPR
    COMMAND.Rule = Symbol("end") Or "cls"
    COMMENT_STMT.Rule = comment
    
    ' New statement rules for extended language features
    SELECT_STMT.Rule = "select" + EXPR + MakePlusRule(SELECT_STMT, Nothing, CASE_CLAUSE) + "end" + "select"
    CASE_CLAUSE.Rule = "case" + EXPR + ":" + STATEMENT_LIST
    ON_ERROR_STMT.Rule = "on" + "error" + "goto" + number Or "on" + "error" + "resume" + "next"
    RESUME_STMT.Rule = "resume" + "next"
    ARRAY_STMT.Rule = "dim" + variable + "(" + ARG_LIST + ")" Or 
                     "dim" + variable + "=" + ARRAY_LITERAL Or
                     "redim" + variable + "(" + ARG_LIST + ")" Or
                     "erase" + variable
    ARRAY_LITERAL.Rule = "{" + ARG_LIST + "}"
    HTML_ELEMENT_STMT.Rule = "set" + variable + "=" + "new" + variable
    LOCATE_ELEMENT_STMT.Rule = variable + "." + "locate" + EXPR + comma + EXPR
    SET_PROPERTY_STMT.Rule = variable + "." + "set_" + variable + EXPR
    EVENT_HANDLER_STMT.Rule = variable + "." + "on_" + variable + "sub" + "(" + ")" + STATEMENT_LIST + "end" + "sub"
    MSGBOX_STMT.Rule = "msgbox" + EXPR

    ' An expression is a number, or a variable, a string,
    ' or the result of a binary comparison.
    EXPR.Rule = number Or
                variable Or
                stringLiteral Or
                BINARY_EXPR Or
                GLOBAL_VAR_EXPR Or
                GLOBAL_FUNCTION_EXPR Or
                FUNCTION_REF Or
                LAMBDA_EXPR Or
                ARRAY_METHOD Or
                ERR_VARIABLE Or
                "(" + EXPR + ")"

    ' Function reference with @ suffix
    FUNCTION_REF.Rule = variable + "@"
    
    ' Lambda expression: FN(x) x * 2
    LAMBDA_EXPR.Rule = "fn" + "(" + ARG_LIST + ")" + EXPR
    
    ' Array method calls: arr.insert(index, value), arr.append(value), etc.
    ARRAY_METHOD.Rule = variable + "." + variable + "(" + ARG_LIST + ")"
    
    ' ERR variable for error handling
    ERR_VARIABLE.Rule = "err"

    BINARY_EXPR.Rule = EXPR + BINARY_OP + EXPR

    BINARY_OP.Rule = Symbol("+") Or
                      "-" Or
                      "*" Or
                      "/" Or
                      "\" Or
                      "=" Or
                      "<=" Or
                      ">=" Or
                      "<" Or
                      ">" Or
                      "<>" Or
                      "and" Or
                      "or"

    'let's do operator precedence right here
    RegisterOperators(50, "*", "/", "\")
    RegisterOperators(40, "+", "-")
    RegisterOperators(30, "=", "<=", ">=", "<", ">", "<>")
    RegisterOperators(20, "and", "or")

    ' Used by PRINT and INPUT to allow a bunch of expressions 
    ' separated by whitespace, or be empty, for example:
    ' PRINT
    ' PRINT "Hi"
    ' PRINT "Hi " a$
    ' All of these match "print" EXPR_LIST
    EXPR_LIST.Rule = MakeStarRule(EXPR_LIST, Nothing, EXPR)

    FOR_STMT.Rule = "for" + ASSIGN_STMT + "to" + EXPR + STEP_OPT
    STEP_OPT.Rule = Empty Or "step" + number
    NEXT_STMT.Rule = "next" Or "next" + variable
    WHILE_STMT.Rule = "while" + EXPR
    WEND_STMT.Rule = "wend"

    'TODO: check number of arguments for particular
    'function in node constructor
    GLOBAL_FUNCTION_EXPR.Rule = FUNC_NAME + "(" + ARG_LIST + ")"

    FUNC_NAME.Rule = Symbol("len") Or
                     "left$" Or
                     "mid$" Or
                     "right$" Or
                     "abs" Or
                     "asc" Or
                     "chr$" Or
                     "csrlin$" Or
                     "cvi" Or
                     "cvs" Or
                     "cvd" Or
                     "exp" Or
                     "fix" Or
                     "log" Or
                     "pos" Or
                     "sgn" Or
                     "sin" Or
                     "cos" Or
                     "tan" Or
                     "instr" Or
                     "space$" Or
                     "spc" Or
                     "sqr" Or
                     "str$" Or
                     "string$" Or
                     "val" Or
                     "cint"

    ARG_LIST.Rule = MakePlusRule(ARG_LIST, comma, EXPR)

    GLOBAL_VAR_EXPR.Rule = Symbol("rnd") Or
                           "timer" Or
                           "inkey$" Or
                           "csrlin"

    ' New syntax rules
    DEF_FN_STMT.Rule = "def" + "fn" + variable + "(" + ARG_LIST + ")" + "=" + EXPR + "end" + "def" Or
                       "def" + "fn" + variable + "(" + ARG_LIST + ")" + FN_BODY + "end" + "def"
    DEF_SUB_STMT.Rule = "def" + "sub" + variable + "(" + ARG_LIST + ")" + FN_BODY + "end" + "def"
    DEF_STRUCT_STMT.Rule = "def" + "struct" + variable + "(" + STRUCT_MEMBERS + ")" + MakeStarRule(DEF_STRUCT_STMT, Nothing, MEMBER_PROPERTY) + MakeStarRule(DEF_STRUCT_STMT, Nothing, MEMBER_METHOD) + "end" + "struct"
    DEF_ENUM_STMT.Rule = "def" + "enum" + variable + "{" + ENUM_VALUES + "}"

    FN_BODY.Rule = MakePlusRule(FN_BODY, Nothing, STATEMENT_LIST)
    ENUM_VALUES.Rule = MakePlusRule(ENUM_VALUES, Symbol(","), variable)
    STRUCT_MEMBERS.Rule = MakePlusRule(STRUCT_MEMBERS, Symbol(","), variable)
    
    ' Member property and method definitions for structs
    MEMBER_PROPERTY.Rule = "m_let" + variable + "." + variable + "=" + EXPR Or 
                          "key" + variable + "." + variable + "=" + EXPR
    MEMBER_METHOD.Rule = "m_fn" + variable + "." + variable + "(" + ARG_LIST + ")" + "=" + EXPR Or
                        "m_sub" + variable + "." + variable + "(" + ARG_LIST + ")" + FN_BODY + "end" + "sub"

    ' By registering these strings as "punctuation",
    ' we exclude them from appearing in as nodes in
    ' the compiled node tree.
    RegisterPunctuation("(", ")", ",", "{", "}")

#End Region

  End Sub

End Class