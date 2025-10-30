Imports HtmlBasic.Nodes
Imports Irony.Compiler

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
    Dim stringLiteral = New StringLiteral("STRING",
                                          """",
                                          ScanFlags.None)
    'Important: do not add comment term to
    'base.NonGrammarTerminals list - we do
    'use this terminal in grammar rules
    Dim comment = New CommentTerminal("Comment",
                                      "REM",
                                      vbLf)

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

    ' Set the PROGRAM to be the root node of BASIC programs.
    ' A program is a bunch of lines
    Root = PROGRAM

#End Region

#Region "Grammar declaration"

    ' A program is a collection of lines
    PROGRAM.Rule = MakePlusRule(PROGRAM,
                                Nothing,
                                LINE)

    ' A line can be an empty line, or it's a number
    ' followed by a statement list ended by a new-line.
    LINE.Rule = NewLine Or
                number + NewLine Or
                number + STATEMENT_LIST + NewLine Or
                STATEMENT_LIST + NewLine

    ' A statement list is 1 or more statements separated
    ' by the ':' character
    STATEMENT_LIST.Rule = MakePlusRule(STATEMENT_LIST,
                                       Symbol(":"),
                                       STATEMENT)

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
                     WEND_STMT
    ' The different statements are defined here
    PRINT_STMT.Rule = "print" + EXPR_LIST
    INPUT_STMT.Rule = "input" + EXPR_LIST + variable
    IF_STMT.Rule = "if" + EXPR + "then" + STATEMENT_LIST + ELSE_CLAUSE_OPT
    ELSE_CLAUSE_OPT.Rule = Empty Or
                           "else" + STATEMENT_LIST
    BRANCH_STMT.Rule = "goto" + number Or
                       "gosub" + number Or
                       "return"
    ASSIGN_STMT.Rule = variable + "=" + EXPR
    LOCATE_STMT.Rule = "locate" + EXPR + comma + EXPR
    SWAP_STMT.Rule = "swap" + EXPR + comma + EXPR
    COMMAND.Rule = Symbol("end") Or
                   "cls"
    COMMENT_STMT.Rule = comment

    ' An expression is a number, or a variable, a string,
    ' or the result of a binary comparison.
    EXPR.Rule = number Or
                variable Or
                stringLiteral Or
                BINARY_EXPR Or
                GLOBAL_VAR_EXPR Or
                GLOBAL_FUNCTION_EXPR Or
                "(" + EXPR + ")"

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
    EXPR_LIST.Rule = MakeStarRule(EXPR_LIST,
                                  Nothing,
                                  EXPR)

    FOR_STMT.Rule = "for" + ASSIGN_STMT + "to" + EXPR + STEP_OPT
    STEP_OPT.Rule = Empty Or
                    "step" + number
    NEXT_STMT.Rule = "next" Or
                     "next" + variable
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

    ARG_LIST.Rule = MakePlusRule(ARG_LIST,
                                 comma,
                                 EXPR)

    GLOBAL_VAR_EXPR.Rule = Symbol("rnd") Or
                           "timer" Or
                           "inkey$" Or
                           "csrlin"

    ' By registering these strings as "punctuation",
    ' we exclude them from appearing in as nodes in
    ' the compiled node tree.
    RegisterPunctuation("(", ")", ",")

#End Region

  End Sub

End Class