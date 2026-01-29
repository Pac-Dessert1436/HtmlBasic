Namespace Irony.Compiler
  ' Token: 0x02000058 RID: 88
  Public Class CodeOutlineFilter
    Inherits TokenFilter

    ' Token: 0x06000253 RID: 595 RVA: 0x0000B6C5 File Offset: 0x000098C5
    Public Sub New(trackIndents As Boolean)
      _trackIndents = trackIndents
    End Sub

    ' Token: 0x1700004D RID: 77
    ' (get) Token: 0x06000254 RID: 596 RVA: 0x0000B6E6 File Offset: 0x000098E6
    ' (set) Token: 0x06000255 RID: 597 RVA: 0x0000B6EE File Offset: 0x000098EE
    Public Property TrackIndents As Boolean
      <DebuggerStepThrough()>
      Get
        Return _trackIndents
      End Get
      Set(value As Boolean)
        _trackIndents = value
      End Set
    End Property

    ' Token: 0x06000256 RID: 598 RVA: 0x0000BCCC File Offset: 0x00009ECC
    Public Overrides Iterator Function BeginFiltering(context As CompilerContext, tokens As IEnumerable(Of Token)) As IEnumerable(Of Token)
      _prevLine = 0
      _indents.Clear()
      For Each token As Token In tokens
        If token.Terminal Is Grammar.Eof Then
          Yield CreateSpecialToken(Grammar.NewLine, context, token.Location)
          If _trackIndents Then
            For Each num As Integer In _indents
              Yield CreateSpecialToken(Grammar.Dedent, context, token.Location)
            Next
          End If
          _indents.Clear()
          Yield token
          Return
        End If
        If token.Terminal.Category <> TokenCategory.Content OrElse token.Location.Line = _prevLine Then
          Yield token
        Else
          Yield CreateSpecialToken(Grammar.NewLine, context, token.Location)
          _prevLine = token.Location.Line
          If Not _trackIndents Then
            Yield token
          Else
            Dim currIndent As Integer = token.Location.Column
            Dim prevIndent As Integer = If((_indents.Count = 0), 0, _indents.Peek())
            If currIndent > prevIndent Then
              _indents.Push(currIndent)
              Yield CreateSpecialToken(Grammar.Indent, context, token.Location)
            ElseIf currIndent < prevIndent Then
              While _indents.Peek() > currIndent
                _indents.Pop()
                Yield CreateSpecialToken(Grammar.Dedent, context, token.Location)
              End While
              If _indents.Peek() <> currIndent Then
                Yield Grammar.CreateSyntaxErrorToken(context, token.Location, "Invalid dedent level, no previous matching indent found.", New Object(-1) {})
              End If
            End If
            Yield token
          End If
        End If
      Next
      Return
    End Function

    ' Token: 0x06000257 RID: 599 RVA: 0x0000BCF7 File Offset: 0x00009EF7
    Private Shared Function CreateSpecialToken(term As Terminal, context As CompilerContext, location As SourceLocation) As Token
      Return Token.Create(term, context, location, String.Empty)
    End Function

    ' Token: 0x04000139 RID: 313
    Private _prevLine As Integer

    ' Token: 0x0400013A RID: 314
    Private _indents As New Stack(Of Integer)()

    ' Token: 0x0400013B RID: 315
    Private _trackIndents As Boolean = True
  End Class
End Namespace
