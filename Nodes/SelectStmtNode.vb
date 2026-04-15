Namespace Nodes

  ''' <summary>
  ''' AST node for SELECT/CASE statements (similar to VB.NET's Select Case)
  ''' </summary>
  Friend Class SelectStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property SelectExpression As ExpressionNode
    Public Property CaseClauses As List(Of CaseClauseNode)
    Public Property DefaultClause As StatementListNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      CaseClauses = New List(Of CaseClauseNode)

      ' Parse the SELECT expression and case clauses
      If args.ChildNodes.Count > 1 Then
        SelectExpression = CType(args.ChildNodes(1), ExpressionNode)
        
        ' Parse case clauses (starting from index 2)
        For i As Integer = 2 To args.ChildNodes.Count - 1
          Dim child = args.ChildNodes(i)
          If TypeOf child Is CaseClauseNode Then
            CaseClauses.Add(CType(child, CaseClauseNode))
          ElseIf TypeOf child Is StatementListNode Then
            ' This is the DEFAULT clause
            DefaultClause = CType(child, StatementListNode)
          End If
        Next
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      textWriter.Write("switch (")
      SelectExpression.GenerateJavaScript(context, textWriter)
      textWriter.WriteLine(") {")
      
      context.Indentation += 1
      
      ' Generate case clauses
      For Each caseClause In CaseClauses
        textWriter.Write(context.IndentationText)
        textWriter.Write("case ")
        caseClause.CaseExpression.GenerateJavaScript(context, textWriter)
        textWriter.WriteLine(":")
        
        context.Indentation += 1
        textWriter.Write(context.IndentationText)
        caseClause.Statements.GenerateJavaScript(context, textWriter)
        textWriter.WriteLine("break;")
        context.Indentation -= 1
      Next
      
      ' Generate default clause
      If DefaultClause IsNot Nothing Then
        textWriter.Write(context.IndentationText)
        textWriter.WriteLine("default:")
        context.Indentation += 1
        textWriter.Write(context.IndentationText)
        DefaultClause.GenerateJavaScript(context, textWriter)
        textWriter.WriteLine("break;")
        context.Indentation -= 1
      End If
      
      context.Indentation -= 1
      textWriter.Write(context.IndentationText)
      textWriter.Write("}")
    End Sub

  End Class

  ''' <summary>
  ''' Represents a single CASE clause in a SELECT statement
  ''' </summary>
  Friend Class CaseClauseNode
    Inherits GenericJsBasicNode

    Public Property CaseExpression As ExpressionNode
    Public Property Statements As StatementListNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      If args.ChildNodes.Count > 1 Then
        CaseExpression = CType(args.ChildNodes(1), ExpressionNode)
        Statements = CType(args.ChildNodes(2), StatementListNode)
      End If
    End Sub

  End Class

End Namespace