Namespace Nodes

  Friend Class PrintStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Private ReadOnly m_exprList As ExprListNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      m_exprList = CType(args.ChildNodes(1), ExprListNode)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      textWriter.Write("console.println(")
      m_exprList.GenerateJavaScript(context, textWriter)
      textWriter.Write(");")
    End Sub

  End Class

End Namespace