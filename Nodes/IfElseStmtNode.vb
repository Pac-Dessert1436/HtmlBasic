Namespace Nodes

  Friend Class IfElseStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Private ReadOnly m_condition As IJsBasicNode
    Private ReadOnly m_thenExpression As IJsBasicNode
    Private ReadOnly m_elseExpression As IJsBasicNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)

      m_condition = CType(args.ChildNodes(1), IJsBasicNode)
      m_thenExpression = CType(args.ChildNodes(3), IJsBasicNode)
      'Child #4 is ELSE_CLAUSE
      Dim elseClause = args.ChildNodes(4)
      If elseClause.ChildNodes.Count > 0 Then
        m_elseExpression = CType(elseClause.ChildNodes(1), IJsBasicNode)
      End If

    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)

      textWriter.Write("if (")
      m_condition.GenerateJavaScript(context, textWriter)
      textWriter.WriteLine(") {")
      context.Indentation += 1
      textWriter.Write(context.IndentationText)
      m_thenExpression.GenerateJavaScript(context, textWriter)
      textWriter.WriteLine()

      context.Indentation -= 1
      textWriter.Write(context.IndentationText)

      If m_elseExpression IsNot Nothing Then

        textWriter.WriteLine("} else {")
        context.Indentation += 1
        textWriter.Write(context.IndentationText)

        m_elseExpression.GenerateJavaScript(context, textWriter)
        context.Indentation -= 1

        textWriter.WriteLine()
        textWriter.Write(context.IndentationText)

      End If

      textWriter.WriteLine("}")

    End Sub

  End Class

End Namespace