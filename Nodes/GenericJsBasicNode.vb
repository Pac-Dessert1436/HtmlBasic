Namespace Nodes

  ''' <summary>
  ''' The base-class for all the AST nodes in JsBasic. This is useful only to hold a reference
  ''' to all the Children, so that the AST tree can be traversed easily using an iterator.
  ''' </summary>
  Friend Class GenericJsBasicNode
    Inherits Irony.Compiler.AstNode
    Implements IJsBasicNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
    End Sub

    Public Overridable Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter) Implements IJsBasicNode.GenerateJavaScript
      GenerateNodes(context, textWriter, ChildNodes)
    End Sub

  End Class

End Namespace