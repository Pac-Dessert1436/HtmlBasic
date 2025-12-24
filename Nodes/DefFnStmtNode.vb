' Syntax for defining a function or a subroutine in HTML-BASIC:
' DEF FN FunctionName(param1 AS TYPE1, param2 AS TYPE2, ...) AS ReturnType
' DEF SUB SubroutineName(param1 AS TYPE1, param2 AS TYPE2, ...)
' ReturnType can be omitted for functions that implicitly return a Variant type.
Namespace Nodes

  Friend Class DefFnStmtNode
    Inherits GenericJsBasicNode

    Public Property FnName As Irony.Compiler.AstNode
    Public Property ParamList As Irony.Compiler.AstNode
    Public Property ReturnType As Irony.Compiler.AstNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      ' Adjust indices to match your grammar
      FnName = If(args.ChildNodes.Count > 1, args.ChildNodes(1), Nothing)
      ParamList = If(args.ChildNodes.Count > 2, args.ChildNodes(2), Nothing)
      ReturnType = Nothing ' parse when grammar provides it
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      ' Emit a JS function. Param extraction depends on grammar node layout.
      textWriter.Write(context.IndentationText)
      textWriter.Write("function ")
      If FnName IsNot Nothing Then
        GeneratorHelper.GenerateNode(context, textWriter, FnName)
      Else
        textWriter.Write("AnonymousFn")
      End If
      textWriter.Write("(")
      If ParamList IsNot Nothing Then
        Dim first As Boolean = True
        For Each p As Irony.Compiler.AstNode In ParamList.ChildNodes
          If Not first Then textWriter.Write(", ")
          first = False
          ' Prefer token name if available
          If p.FindToken() IsNot Nothing Then
            textWriter.Write(p.FindToken().Text)
          Else
            GeneratorHelper.GenerateNode(context, textWriter, p)
          End If
        Next
      End If
      textWriter.WriteLine(") {")
      textWriter.Write(context.IndentationText)
      textWriter.WriteLine("  // TODO: translate function body")
      textWriter.Write(context.IndentationText)
      textWriter.WriteLine("}")
    End Sub

  End Class

End Namespace