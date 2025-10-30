Namespace Nodes

  Friend Class NextStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      textWriter.WriteLine()
      context.Indentation -= 1
      textWriter.Write(context.IndentationText)
      textWriter.Write("}")
    End Sub

  End Class

End Namespace