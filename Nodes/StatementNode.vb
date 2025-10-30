Namespace Nodes

  Friend Class StatementNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      MyBase.GenerateJavaScript(context, textWriter)
      ' put semi-colons on the end of expression-statements.
      If ChildNodes?.Count <> 1 Then Return
      Dim expr = TryCast(ChildNodes(0), ExpressionNode)
      If expr Is Nothing Then Return
      textWriter.Write(";")
    End Sub

  End Class

End Namespace