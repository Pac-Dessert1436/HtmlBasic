Imports Irony.Compiler

Namespace Nodes

  Friend Class LineNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property LineNumber As Integer
    Public Property StatementList As GenericJsBasicNode
    Public Property ReturnText As String
    Public Property LineTypes As LineTypes

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      LineTypes = LineTypes.InternalLine ' overwritten later by JavaScriptGenerator
      Dim index = 0
      Dim tkn = TryCast(args.ChildNodes(index), Token)
      If tkn IsNot Nothing AndAlso IsNumeric(tkn.Value) Then
        LineNumber = CInt(CLng(Fix(tkn.Value)) Mod Integer.MaxValue)
        index += 1
      End If
      If args.ChildNodes.Count > (index + 1) Then
        StatementList = CType(args.ChildNodes(index), GenericJsBasicNode)
      Else
        StatementList = New GenericJsBasicNode(args) 'empty node
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter1 As IO.TextWriter)

      If LineTypes = LineTypes.None Then Throw New ApplicationException($"Line type not set for line {LineNumber}")

      If (LineTypes And LineTypes.FunctionStart) > 0 Then
        textWriter1.Write(context.IndentationText)
        textWriter1.WriteLine($"function line{LineNumber}() {{")
        context.Indentation += 1
      End If

      textWriter1.Write(context.IndentationText)
      StatementList.GenerateJavaScript(context, textWriter1)
      textWriter1.WriteLine()

      If (LineTypes And LineTypes.FunctionEnd) > 0 Then
        If Not String.IsNullOrEmpty(ReturnText) Then
          textWriter1.Write(context.IndentationText)
          textWriter1.WriteLine(ReturnText)
        End If
        context.Indentation -= 1
        textWriter1.Write(context.IndentationText)
        textWriter1.WriteLine("}")
      End If

    End Sub

    Public Overrides Function ToString() As String
      Return $"{MyBase.ToString} ({LineNumber})"
    End Function

  End Class

  <Flags>
  Public Enum LineTypes
    None = 0
    FunctionStart = 1
    InternalLine = 2
    FunctionEnd = 4
  End Enum

End Namespace