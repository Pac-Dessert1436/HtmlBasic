Namespace Nodes

  Friend Class BranchStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property BranchType As BranchType
    Public Property DestinationLine As Integer

    Public Sub New(args As Irony.Compiler.AstNodeArgs)

      MyBase.New(args)

      Dim command = CType(args.ChildNodes(0), Irony.Compiler.Token)

      If args.ChildNodes.Count = 1 Then
        BranchType = BranchType.Return
      Else
        Dim line = CType(args.ChildNodes(1), Irony.Compiler.Token)
        BranchType = (If(command.Text.ToLowerInvariant.Equals("goto"), BranchType.[Goto], BranchType.[Gosub]))
        DestinationLine = Integer.Parse(line.Text, Globalization.CultureInfo.InvariantCulture)
      End If

    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      If BranchType = BranchType.Gosub Then
        textWriter.Write($"line{DestinationLine}();")
      ElseIf BranchType = BranchType.Return Then
        textWriter.Write("return;")
      Else
        textWriter.Write($"return line{DestinationLine};")
      End If
    End Sub

  End Class

  Public Enum BranchType
    [Goto]
    [Gosub]
    [Return]
  End Enum

End Namespace