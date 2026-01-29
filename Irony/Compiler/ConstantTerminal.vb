Namespace Irony.Compiler
  ' Token: 0x02000011 RID: 17
  Public Class ConstantTerminal
    Inherits Terminal

    ' Token: 0x06000039 RID: 57 RVA: 0x00002797 File Offset: 0x00000997
    Public Sub New(name As String)
      MyBase.New(name)
      SetOption(TermOptions.IsConstant)
    End Sub

    ' Token: 0x0600003A RID: 58 RVA: 0x000027B3 File Offset: 0x000009B3
    Public Sub Add(lexeme As String, value As Object)
      Table(lexeme) = value
    End Sub

    ' Token: 0x0600003B RID: 59 RVA: 0x000027C4 File Offset: 0x000009C4
    Public Overrides Function TryMatch(context As CompilerContext, source As ISourceStream) As Token
      Dim text As String = source.Text
      For Each text2 As String In Table.Keys
        If source.Position + text2.Length <= text.Length AndAlso source.MatchSymbol(text2, Not Grammar.CaseSensitive) Then
          Dim result As Token = Token.Create(Me, context, source.TokenStart, text2, Table(text2))
          source.Position += text2.Length
          Return result
        End If
      Next
      Return Nothing
    End Function

    ' Token: 0x0600003C RID: 60 RVA: 0x00002880 File Offset: 0x00000A80
    Public Overrides Function GetFirsts() As IList(Of String)
      Dim array As String() = New String(Table.Count - 1) {}
      Table.Keys.CopyTo(array, 0)
      Return array
    End Function

    ' Token: 0x04000060 RID: 96
    Public Table As New ConstantsTable()
  End Class
End Namespace
