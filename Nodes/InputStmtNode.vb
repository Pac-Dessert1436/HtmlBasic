Namespace Nodes

  Friend Class InputStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Private ReadOnly m_toPrint As ExprListNode
    Private ReadOnly m_variable As Irony.Compiler.Token

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      m_toPrint = CType(args.ChildNodes(1), ExprListNode)
      m_variable = CType(args.ChildNodes(2), Irony.Compiler.Token)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      GeneratorHelper.GenerateNode(context, textWriter, m_variable)
      textWriter.Write("= console.input(")
      m_toPrint?.GenerateJavaScript(context, textWriter)
      textWriter.Write(");")
    End Sub

  End Class

End Namespace