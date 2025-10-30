Namespace Nodes

  Friend Class BinaryOpNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Private ReadOnly m_operator As Irony.Compiler.Token

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      m_operator = CType(args.ChildNodes(0), Irony.Compiler.Token)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      Dim op As String
      Select Case m_operator.Text
        Case "=" : op = "=="
        Case "<>" : op = "!="
        Case "and" : op = "&&"
        Case "or" : op = "||"
        Case Else
          op = m_operator.Text
      End Select
      textWriter.Write(op)
    End Sub

  End Class

End Namespace