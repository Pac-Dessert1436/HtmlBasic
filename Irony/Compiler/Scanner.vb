Namespace Irony.Compiler
  ' Token: 0x02000034 RID: 52
  Public Class Scanner
    ' Token: 0x060000FE RID: 254 RVA: 0x00004F88 File Offset: 0x00003188
    Public Sub New(data As GrammarData)
      _data = data
      _lineTerminators = _data.Grammar.LineTerminators.ToCharArray()
    End Sub

    ' Token: 0x14000004 RID: 4
    ' (add) Token: 0x060000FF RID: 255 RVA: 0x00004FDB File Offset: 0x000031DB
    ' (remove) Token: 0x06000100 RID: 256 RVA: 0x00004FF4 File Offset: 0x000031F4
    Public Event TokenCreated As EventHandler(Of TokenEventArgs)

    ' Token: 0x06000101 RID: 257 RVA: 0x0000500D File Offset: 0x0000320D
    Protected Sub OnTokenCreated(token As Token)
      _tokenArgs.Token = token
      RaiseEvent TokenCreated(Me, _tokenArgs)
    End Sub

    ' Token: 0x06000102 RID: 258 RVA: 0x00005036 File Offset: 0x00003236
    Public Sub Prepare(context As CompilerContext, source As ISourceStream)
      _context = context
      _caseSensitive = context.Compiler.Grammar.CaseSensitive
      _source = source
      _currentToken = Nothing
      _bufferedTokens.Clear()
      ResetSource()
    End Sub

    ' Token: 0x06000103 RID: 259 RVA: 0x0000519C File Offset: 0x0000339C
    Public Iterator Function BeginScan() As IEnumerable(Of Token)
      Do
        _currentToken = ReadToken()
        OnTokenCreated(_currentToken)
        Yield _currentToken
      Loop While _currentToken.Terminal IsNot Grammar.Eof
      Return
    End Function

    ' Token: 0x06000104 RID: 260 RVA: 0x000051B9 File Offset: 0x000033B9
    Public Function GetNext(ByRef state As Integer) As Token
      Return ReadToken()
    End Function

    ' Token: 0x06000105 RID: 261 RVA: 0x000051C4 File Offset: 0x000033C4
    Private Function ReadToken() As Token
      If _bufferedTokens.Count > 0 Then
        Dim result As Token = _bufferedTokens(0)
        _bufferedTokens.RemoveAt(0)
        Return result
      End If
      While _data.Grammar.WhitespaceChars.Contains(_source.CurrentChar)
        _source.Position += 1
      End While
      SetTokenStartLocation()
      If _source.EOF() Then
        Return Token.Create(Grammar.Eof, _context, _source.TokenStart, String.Empty, Grammar.Eof.Name)
      End If
      Dim terminals As TerminalList = SelectTerminals(_source.CurrentChar)
      Dim tkn As Token = MatchTerminals(terminals)
      If tkn Is Nothing AndAlso _data.FallbackTerminals.Count > 0 Then
        tkn = MatchTerminals(_data.FallbackTerminals)
      End If
      If tkn Is Nothing Then
        tkn = _data.Grammar.TryMatch(_context, _source)
      End If
      If tkn IsNot Nothing AndAlso tkn.IsMultiToken() Then
        For Each astNode As AstNode In tkn.ChildNodes
          Dim item As Token = CType(astNode, Token)
          _bufferedTokens.Add(item)
        Next
        tkn = _bufferedTokens(0)
        _bufferedTokens.RemoveAt(0)
      End If
      If tkn IsNot Nothing AndAlso Not tkn.IsError() Then
        _source.Position = _source.TokenStart.Position + tkn.Length
        Return tkn
      End If
      If tkn Is Nothing Then
        tkn = Grammar.CreateSyntaxErrorToken(_context, _source.TokenStart, "Invalid character: '{0}'", New Object() {_source.CurrentChar})
      End If
      Recover()
      Return tkn
    End Function

    ' Token: 0x06000106 RID: 262 RVA: 0x000053B8 File Offset: 0x000035B8
    Private Function MatchTerminals(terminals As TerminalList) As Token
      Dim token As Token = Nothing
      For Each terminal As Terminal In terminals
        If token IsNot Nothing AndAlso token.Terminal.Priority > terminal.Priority Then
          Exit For
        End If
        _source.Position = _source.TokenStart.Position
        Dim token2 As Token = terminal.TryMatch(_context, _source)
        If token2 IsNot Nothing AndAlso (token2.IsError() OrElse token Is Nothing OrElse token2.Length > token.Length) Then
          token = token2
        End If
        If token IsNot Nothing AndAlso token.IsError() Then
          Exit For
        End If
      Next
      Return token
    End Function

    ' Token: 0x06000107 RID: 263 RVA: 0x00005470 File Offset: 0x00003670
    Private Function SelectTerminals(current As Char) As TerminalList
      If Not _caseSensitive Then
        current = Char.ToLower(current)
      End If
      Dim result As TerminalList = Nothing
      If _data.TerminalsLookup.TryGetValue(current, result) Then
        Return result
      End If
      Return _data.FallbackTerminals
    End Function

    ' Token: 0x06000108 RID: 264 RVA: 0x000054B0 File Offset: 0x000036B0
    Private Sub Recover()
      While Not _source.EOF() AndAlso _data.ScannerRecoverySymbols.IndexOf(_source.CurrentChar) < 0
        _source.Position += 1
      End While
    End Sub

    ' Token: 0x06000109 RID: 265 RVA: 0x000054FD File Offset: 0x000036FD
    Public Overrides Function ToString() As String
      Return _source.ToString()
    End Function

    ' Token: 0x0600010A RID: 266 RVA: 0x0000550C File Offset: 0x0000370C
    Public Sub ResetSource()
      _source.Position = 0
      _source.TokenStart = Nothing
      _nextNewLinePosition = _source.Text.IndexOf(vbLf)
    End Sub

    ' Token: 0x0600010B RID: 267 RVA: 0x00005554 File Offset: 0x00003754
    Friend Sub SetTokenStartLocation()
      Dim tokenStart As SourceLocation = _source.TokenStart
      Dim position As Integer = _source.Position
      Dim text As String = _source.Text
      If position <= _nextNewLinePosition OrElse _nextNewLinePosition < 0 Then
        tokenStart.Column += position - tokenStart.Position
        tokenStart.Position = position
        _source.TokenStart = tokenStart
        Return
      End If
      Dim nextNewLinePosition As Integer = _nextNewLinePosition
      Dim num As Integer = 1
      CountCharsInText(text, _lineTerminators, nextNewLinePosition + 1, position - 1, num, nextNewLinePosition)
      tokenStart.Line += num
      Dim num2 As Integer = 0
      Dim num3 As Integer = 0
      If _source.TabWidth > 1 Then
        CountCharsInText(text, _tab_arr, nextNewLinePosition, position - 1, num2, num3)
      End If
      tokenStart.Position = position
      tokenStart.Column = position - nextNewLinePosition - 1
      If num2 > 0 Then
        tokenStart.Column += (_source.TabWidth - 1) * num2
      End If
      _nextNewLinePosition = text.IndexOfAny(_lineTerminators, position)
      _source.TokenStart = tokenStart
    End Sub

    ' Token: 0x0600010C RID: 268 RVA: 0x00005674 File Offset: 0x00003874
    Private Shared Sub CountCharsInText(text As String, chars As Char(), from As Integer, until As Integer, ByRef count As Integer, ByRef lastPosition As Integer)
      If from > until Then Exit Sub
      Do
        Dim num As Integer = text.IndexOfAny(chars, from, until - from + 1)
        If num < 0 Then Exit Do
        If text(num) <> vbLf OrElse num <= 0 OrElse text(num - 1) <> vbCr Then
          count += 1
        End If
        lastPosition = num
        from = num + 1
      Loop
    End Sub

    ' Token: 0x040000A8 RID: 168
    Private _data As GrammarData

    ' Token: 0x040000A9 RID: 169
    Private _source As ISourceStream

    ' Token: 0x040000AA RID: 170
    Private _context As CompilerContext

    ' Token: 0x040000AB RID: 171
    Private _lineTerminators As Char()

    ' Token: 0x040000AC RID: 172
    Private _caseSensitive As Boolean

    ' Token: 0x040000AD RID: 173
    Private _currentToken As Token

    ' Token: 0x040000AE RID: 174
    Private _bufferedTokens As New TokenList()

    ' Token: 0x040000B0 RID: 176
    Private _tokenArgs As New TokenEventArgs(Nothing)

    ' Token: 0x040000B1 RID: 177
    Private _nextNewLinePosition As Integer = -1

    ' Token: 0x040000B2 RID: 178
    Private Shared _tab_arr As Char() = New Char() {vbTab}
  End Class
End Namespace
