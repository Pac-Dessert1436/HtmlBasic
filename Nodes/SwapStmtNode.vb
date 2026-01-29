Namespace Nodes

  Friend Class SwapStmtNode
    Inherits GenericJsBasicNode

    Public Property Variable1 As Irony.Compiler.AstNode
    Public Property Variable2 As Irony.Compiler.AstNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      Variable1 = args.ChildNodes(1)
      Variable2 = args.ChildNodes(3)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      textWriter.Write("tempVar = ")
      GenerateNode(context, textWriter, Variable1)
      textWriter.WriteLine(";")
      textWriter.Write(context.IndentationText)
      GenerateNode(context, textWriter, Variable1)
      textWriter.Write(" = ")
      GenerateNode(context, textWriter, Variable2)
      textWriter.WriteLine(";")
      textWriter.Write(context.IndentationText)
      GenerateNode(context, textWriter, Variable2)
      textWriter.WriteLine(" = tempVar;")
      textWriter.Write(context.IndentationText)
      textWriter.Write("tempVar = null;")
    End Sub

  End Class

End Namespace