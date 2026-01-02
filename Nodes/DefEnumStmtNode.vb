' Syntax for defining an enum in HTML-BASIC:
' DEF ENUM EnumName {Value1, Value2, ...}
Namespace Nodes

  Friend Class DefEnumStmtNode
    Inherits GenericJsBasicNode

    Public Property EnumName As Irony.Compiler.AstNode
    Public Property ValueList As Irony.Compiler.AstNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      ' Adjust indices to your grammar's child layout
      EnumName = If(args.ChildNodes.Count > 1, args.ChildNodes(1), Nothing)
      ValueList = If(args.ChildNodes.Count > 2, args.ChildNodes(2), Nothing)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      ' Emit a JS enum-like object. Example:
      ' var EnumName = { Value1: 0, Value2: 1, ... };
      textWriter.Write(context.IndentationText)
      textWriter.Write("var ")
      If EnumName IsNot Nothing Then
        GeneratorHelper.GenerateNode(context, textWriter, EnumName)
      Else
        textWriter.Write("AnonymousEnum")
      End If
      textWriter.Write(" = {")
      If ValueList IsNot Nothing Then
        Dim idx As Integer = 0
        For Each v As Irony.Compiler.AstNode In ValueList.ChildNodes
          If idx > 0 Then textWriter.Write(", ")
          ' Generate enum value name using GeneratorHelper
          GeneratorHelper.GenerateNode(context, textWriter, v)
          textWriter.Write(": " & idx.ToString())
          idx += 1
        Next
      End If
      textWriter.WriteLine("};")
    End Sub

  End Class

End Namespace