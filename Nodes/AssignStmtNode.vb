Namespace Nodes

  Friend Class AssignStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property Variable As Irony.Compiler.Token
    Public Property Expression As ExpressionNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      Variable = CType(args.ChildNodes(0), Irony.Compiler.Token)
      Expression = CType(args.ChildNodes(2), ExpressionNode)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      GenerateNodes(context, textWriter, ChildNodes)
      textWriter.Write(";")
    End Sub

  End Class

End Namespace