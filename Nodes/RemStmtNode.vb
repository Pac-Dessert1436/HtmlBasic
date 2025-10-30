Namespace Nodes

  Friend Class RemStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Private ReadOnly m_comment As String

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      Dim token = CType(args.ChildNodes(0), Irony.Compiler.Token)
      Dim text = token.Text
      If text.Length < 5 Then
        m_comment = "no comment"
      Else
        m_comment = text.Substring(4)
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      textWriter.Write($"// {m_comment}")
    End Sub

  End Class

End Namespace