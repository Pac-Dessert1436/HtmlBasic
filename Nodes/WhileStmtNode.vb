Namespace Nodes

  Friend Class WhileStmtNode
    Inherits GenericJsBasicNode

    Private ReadOnly m_condition As ExpressionNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      m_condition = CType(args.ChildNodes(1), ExpressionNode)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      context.Indentation += 1
      textWriter.Write("while (")
      m_condition.GenerateJavaScript(context, textWriter)
      textWriter.Write(") {")
    End Sub

  End Class

End Namespace