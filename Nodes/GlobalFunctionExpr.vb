Namespace Nodes

  Friend Class GlobalFunctionExpr
    Inherits ExpressionNode

    Public Property FunctionName As Irony.Compiler.Token
    Public Property InputParameter1 As ExpressionNode
    Public Property InputParameter2 As ExpressionNode
    Public Property InputParameter3 As ExpressionNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      FunctionName = args.ChildNodes(0).DepthFirstTraversal.OfType(Of Irony.Compiler.Token).[Single]
      Dim funcArgs = args.ChildNodes(2).ChildNodes
      InputParameter1 = CType(funcArgs(0), ExpressionNode)
      If funcArgs.Count > 1 Then InputParameter2 = CType(funcArgs(1), ExpressionNode)
      If funcArgs.Count > 2 Then InputParameter3 = CType(funcArgs(2), ExpressionNode)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)

      Select Case FunctionName.Text.ToLowerInvariant()
        Case "len" : Write(context, textWriter, InputParameter1, ".length")
        Case "left$" : Write(context, textWriter, InputParameter1, ".substring(0, ", InputParameter2, ")")
        Case "right$" : Write(context, textWriter, InputParameter1, ".substring(", InputParameter1, ".length - ", InputParameter2, ")")
        Case "mid$"
          If InputParameter3 Is Nothing Then
            Write(context, textWriter, InputParameter1, ".substring(", InputParameter2, "-1)")
          Else
            Write(context, textWriter, InputParameter1, ".substring(", InputParameter2, "-1,", InputParameter2, "-1+", InputParameter3, ")")
          End If
        Case "abs" : Write(context, textWriter, "Math.abs(", InputParameter1, ")")
        Case "asc" : Write(context, textWriter, InputParameter1, ".charCodeAt(0)")
        Case "chr$" : Write(context, textWriter, "String.fromCharCode(", InputParameter1, ")")
        Case "cint" : Write(context, textWriter, "Math.round(", InputParameter1, ")")
        Case "cvi" : Write(context, textWriter, "parseInt(", InputParameter1, ")")
        Case "cvs", "cvd" : Write(context, textWriter, "parseFloat(", InputParameter1, ")")
        Case "exp" : Write(context, textWriter, "Math.exp(", InputParameter1, ")")
        Case "fix" : Write(context, textWriter, "Math.floor(", InputParameter1, ")")
        Case "pos" : Write(context, textWriter, "console.cursorPosition.column")
        Case "sgn" : Write(context, textWriter, "(", $"{InputParameter1} == 0 ? 0 : (", InputParameter1, " > 0 ? 1 : -1))")
        Case "sin" : Write(context, textWriter, "Math.sin(", InputParameter1, ")")
        Case "cos" : Write(context, textWriter, "Math.cos(", InputParameter1, ")")
        Case "tan" : Write(context, textWriter, "Math.tan(", InputParameter1, ")")
        Case "spc", "space$" : Write(context, textWriter, "getSpaces(", InputParameter1, ")")
        Case "instr"
          If InputParameter3 Is Nothing Then
            Write(context, textWriter, $"({InputParameter1}", ".indexOf(", InputParameter2, ")+1)")
          Else
            Write(context, textWriter, $"({InputParameter2}", ".indexOf(", InputParameter3, $",{InputParameter1})+1)")
          End If
        Case "log" : Write(context, textWriter, "Math.log(", InputParameter1, ")")
        Case "sqr" : Write(context, textWriter, "Math.sqrt(", InputParameter1, ")")
        Case "str$" : Write(context, textWriter, "('' + ", InputParameter1, ")")
        Case "string$" : Write(context, textWriter, "generateString(", InputParameter1, ",", InputParameter2, ")")
        Case "val" : Write(context, textWriter, "parseInt(", InputParameter1, ")")
        Case Else
          Throw New BasicSyntaxErrorException($"Unknown global function {FunctionName.Text}")
      End Select
    End Sub

    Private Shared Sub Write(context As JsContext, textWriter As IO.TextWriter, ParamArray values As Object())
      For Each value In values
        If TypeOf value Is Irony.Compiler.AstNode Then
          textWriter.Write("(")
          GeneratorHelper.GenerateNode(context, textWriter, CType(value, Irony.Compiler.AstNode))
          textWriter.Write(")")
        Else
          textWriter.Write(value)
        End If
      Next
    End Sub

  End Class

End Namespace