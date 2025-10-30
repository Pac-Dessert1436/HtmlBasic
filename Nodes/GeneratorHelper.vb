Namespace Nodes

  Friend Module GeneratorHelper

    Public Sub GenerateNode(context As JsContext, textWriter As IO.TextWriter, node As Irony.Compiler.AstNode)

      If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))

      If TypeOf node Is IJsBasicNode Then
        Dim jsBasicNode = CType(node, IJsBasicNode)
        jsBasicNode.GenerateJavaScript(context, textWriter)
      ElseIf TypeOf node Is Irony.Compiler.Token Then
        Dim t = CType(node, Irony.Compiler.Token)
        Select Case t.Terminal.Name
          Case "VARIABLE"
            If t.Text.EndsWith("$") Then
              textWriter.Write($"str_{t.Text.Substring(0, t.Text.Length - 1)}")
            ElseIf t.Text.EndsWith("%") Then
              textWriter.Write($"int_{t.Text.Substring(0, t.Text.Length - 1)}")
            ElseIf t.Text.EndsWith("!") Then
              textWriter.Write($"sng_{t.Text.Substring(0, t.Text.Length - 1)}")
            ElseIf t.Text.EndsWith("&") Then
              textWriter.Write($"lng_{t.Text.Substring(0, t.Text.Length - 1)}")
            ElseIf t.Text.EndsWith("#") Then
              textWriter.Write($"dbl_{t.Text.Substring(0, t.Text.Length - 1)}")
            Else
              textWriter.Write(t.Text)
              '  Throw New BasicSyntaxErrorException($"Invalid variable name: { t.Text}")
            End If
          Case "BOOLEAN_OP"
            Select Case t.Text.ToLowerInvariant()
              Case "and" : textWriter.Write("&&")
              Case "or" : textWriter.Write("||")
              Case Else
                textWriter.Write(t.Text)
            End Select
          Case Else
            textWriter.Write(ConvertToJavascript(t.Text))
        End Select
        textWriter.Write(" ")
      Else
        Throw New ApplicationException($"Unhandled statement type: {node.[GetType].FullName}")
      End If

    End Sub

    Private Function ConvertToJavascript(keyword As String) As String
      Select Case keyword.ToLowerInvariant()
        Case "end" : Return "throw ""ProgramAbortException"";"
        Case "cls" : Return "console.cls();"
        Case "rnd" : Return "Math.random()"
        Case "timer" : Return "(new Date()).getMilliseconds()"
        Case "inkey$" : Return "getInkey()"
        Case "csrlin" : Return "console.cursorPosition.row"
        Case Else
          Return keyword
      End Select
    End Function

    Public Sub GenerateNodes(context As JsContext, textWriter As IO.TextWriter, nodes As Irony.Compiler.AstNodeList)
      For Each node In nodes
        GenerateNode(context, textWriter, node)
      Next
    End Sub

  End Module

End Namespace