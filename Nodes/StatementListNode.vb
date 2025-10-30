Namespace Nodes

  Friend Class StatementListNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public ReadOnly Property Statements As IEnumerable(Of StatementNode)
      Get
        Return ChildNodes.Cast(Of StatementNode)()
      End Get
    End Property

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      Dim isFirst As Boolean = True
      For Each stmt As StatementNode In Statements
        If Not isFirst Then
          textWriter.WriteLine()
          textWriter.Write(context.IndentationText)
        End If
        stmt.GenerateJavaScript(context, textWriter)
        isFirst = False
      Next
    End Sub

  End Class

End Namespace