Namespace Nodes

  Friend Class LocateStmtNode
    Inherits GenericJsBasicNode

    Public Property TargetRow As Irony.Compiler.AstNode
    Public Property TargetColumn As Irony.Compiler.AstNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      TargetRow = args.ChildNodes(1)
      TargetColumn = args.ChildNodes(3)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      textWriter.Write("console.setCursorPos(")
      GeneratorHelper.GenerateNode(context, textWriter, TargetRow)
      textWriter.Write(", ")
      GeneratorHelper.GenerateNode(context, textWriter, TargetColumn)
      textWriter.Write(");")
    End Sub

  End Class

End Namespace