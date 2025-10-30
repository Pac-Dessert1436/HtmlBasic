Namespace Nodes

  Friend Class ExprListNode
    Inherits GenericJsBasicNode

    Public ReadOnly Property Expressions As IEnumerable(Of ExpressionNode)
      Get
        Return ChildNodes.Cast(Of ExpressionNode)()
      End Get
    End Property

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      Dim isFirst = True
      For Each expr In Expressions
        If Not isFirst Then textWriter.Write("+ ")
        expr.GenerateJavaScript(context, textWriter)
        isFirst = False
      Next
    End Sub

  End Class

End Namespace