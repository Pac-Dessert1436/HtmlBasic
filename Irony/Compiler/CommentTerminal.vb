Namespace Irony.Compiler
  ' Token: 0x0200001D RID: 29
  Public Class CommentTerminal
    Inherits Terminal

    ' Token: 0x0600007E RID: 126 RVA: 0x00003803 File Offset: 0x00001A03
    Public Sub New(name As String, startSymbol As String, ParamArray endSymbols As String())
      MyBase.New(name, TokenCategory.Comment)
      Me.StartSymbol = startSymbol
      Me.EndSymbols = New StringList()
      Me.EndSymbols.AddRange(endSymbols)
    End Sub

    ' Token: 0x0600007F RID: 127 RVA: 0x0000382C File Offset: 0x00001A2C
    Public Overrides Sub Init(grammar As Grammar)
      MyBase.Init(grammar)
      _endSymbolsFirsts = New Char(EndSymbols.Count - 1) {}
      For i As Integer = 0 To EndSymbols.Count - 1
        Dim text As String = EndSymbols(i)
        _endSymbolsFirsts(i) = text(0)
        _isLineComment = _isLineComment Or text.Contains(vbLf)
      Next
    End Sub

    ' Token: 0x06000080 RID: 128 RVA: 0x000038A0 File Offset: 0x00001AA0
    Public Overrides Function TryMatch(context As CompilerContext, source As ISourceStream) As Token
      Dim ignoreCase As Boolean = Not Grammar.CaseSensitive
      If Not source.MatchSymbol(StartSymbol, ignoreCase) Then
        Return Nothing
      End If
      source.Position += StartSymbol.Length
      While Not source.EOF()
        Dim num As Integer
        If EndSymbols.Count = 1 Then
          num = source.Text.IndexOf(EndSymbols(0), source.Position)
        Else
          num = source.Text.IndexOfAny(_endSymbolsFirsts, source.Position)
        End If
        If num >= 0 Then
          source.Position = num
          For Each text As String In EndSymbols
            If source.MatchSymbol(text, ignoreCase) Then
              source.Position += text.Length
              Return Token.Create(Me, context, source.TokenStart, source.GetLexeme())
            End If
          Next
          source.Position += 1
          Continue While
        End If
        source.Position = source.Text.Length
        If _isLineComment Then
          Return Token.Create(Me, context, source.TokenStart, source.GetLexeme())
        End If
        Return Grammar.CreateSyntaxErrorToken(context, source.TokenStart, "Unclosed comment block", New Object(-1) {})
      End While
      Return Nothing
    End Function

    ' Token: 0x06000081 RID: 129 RVA: 0x00003A10 File Offset: 0x00001C10
    Public Overrides Function GetFirsts() As IList(Of String)
      Return New String() {StartSymbol}
    End Function

    ' Token: 0x0400007A RID: 122
    Public StartSymbol As String

    ' Token: 0x0400007B RID: 123
    Public EndSymbols As StringList

    ' Token: 0x0400007C RID: 124
    Private _endSymbolsFirsts As Char()

    ' Token: 0x0400007D RID: 125
    Private _isLineComment As Boolean
  End Class
End Namespace
