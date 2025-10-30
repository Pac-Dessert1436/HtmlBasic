Namespace Nodes

  ''' <summary>
  ''' A AST-node which can generate JavaScript code.
  ''' </summary>
  Public Interface IJsBasicNode

    Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)

  End Interface

  ''' <summary>
  ''' A class to hold the current indentation information when compiling
  ''' JavaScript (e.g. code in while {} statements is indented).
  ''' </summary>
  Public Class JsContext

    Public Property Indentation As Integer

    Public ReadOnly Property IndentationText As String
      Get
        Return New [String](" "c, Indentation * 2)
      End Get
    End Property

  End Class

End Namespace