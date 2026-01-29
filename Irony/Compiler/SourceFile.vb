Namespace Irony.Compiler
  ' Token: 0x02000015 RID: 21
  Public Class SourceFile
    Implements ISourceStream

    ' Token: 0x0600004C RID: 76 RVA: 0x00002932 File Offset: 0x00000B32
    Public Sub New(text As String, fileName As String, tabWidth As Integer)
      _text = text
      _fileName = fileName
      _tabWidth = tabWidth
    End Sub

    ' Token: 0x0600004D RID: 77 RVA: 0x0000294F File Offset: 0x00000B4F
    Public Sub New(text As String, fileName As String)
      Me.New(text, fileName, 8)
    End Sub

    ' Token: 0x1700000A RID: 10
    ' (get) Token: 0x0600004E RID: 78 RVA: 0x0000295A File Offset: 0x00000B5A
    Public ReadOnly Property FileName As String
      <DebuggerStepThrough()>
      Get
        Return _fileName
      End Get
    End Property

    ' Token: 0x1700000B RID: 11
    ' (get) Token: 0x0600004F RID: 79 RVA: 0x00002962 File Offset: 0x00000B62
    ' (set) Token: 0x06000050 RID: 80 RVA: 0x0000296A File Offset: 0x00000B6A
    Public Property TabWidth As Integer Implements Irony.Compiler.ISourceStream.TabWidth
      <DebuggerStepThrough()>
      Get
        Return _tabWidth
      End Get
      Set(value As Integer)
        _tabWidth = value
      End Set
    End Property

    ' Token: 0x1700000C RID: 12
    ' (get) Token: 0x06000051 RID: 81 RVA: 0x00002973 File Offset: 0x00000B73
    ' (set) Token: 0x06000052 RID: 82 RVA: 0x0000297B File Offset: 0x00000B7B
    Public Property Position As Integer Implements Irony.Compiler.ISourceStream.Position
      <DebuggerStepThrough()>
      Get
        Return _position
      End Get
      Set(value As Integer)
        _position = value
      End Set
    End Property

    ' Token: 0x06000053 RID: 83 RVA: 0x00002984 File Offset: 0x00000B84
    <DebuggerStepThrough()>
    Public Function EOF() As Boolean Implements Irony.Compiler.ISourceStream.EOF
      Return _position >= Text.Length
    End Function

    ' Token: 0x1700000D RID: 13
    ' (get) Token: 0x06000054 RID: 84 RVA: 0x0000299C File Offset: 0x00000B9C
    Public ReadOnly Property CurrentChar As Char Implements Irony.Compiler.ISourceStream.CurrentChar
      <DebuggerStepThrough()>
      Get
        Dim result As Char
        Try
          result = _text(_position)
        Catch
          result = vbNullChar
        End Try
        Return result
      End Get
    End Property

    ' Token: 0x1700000E RID: 14
    ' (get) Token: 0x06000055 RID: 85 RVA: 0x000029D4 File Offset: 0x00000BD4
    Public ReadOnly Property NextChar As Char Implements Irony.Compiler.ISourceStream.NextChar
      <DebuggerStepThrough()>
      Get
        Dim result As Char
        Try
          result = _text(_position + 1)
        Catch
          result = vbNullChar
        End Try
        Return result
      End Get
    End Property

    ' Token: 0x06000056 RID: 86 RVA: 0x00002A10 File Offset: 0x00000C10
    Public Function MatchSymbol(symbol As String, ignoreCase As Boolean) As Boolean Implements Irony.Compiler.ISourceStream.MatchSymbol
      Dim result As Boolean
      Try
        Dim num As Integer = String.Compare(_text, _position, symbol, 0, symbol.Length, ignoreCase)
        result = (num = 0)
      Catch
        result = False
      End Try
      Return result
    End Function

    ' Token: 0x1700000F RID: 15
    ' (get) Token: 0x06000057 RID: 87 RVA: 0x00002A58 File Offset: 0x00000C58
    Public ReadOnly Property Text As String Implements Irony.Compiler.ISourceStream.Text
      <DebuggerStepThrough()>
      Get
        Return _text
      End Get
    End Property

    ' Token: 0x06000058 RID: 88 RVA: 0x00002A60 File Offset: 0x00000C60
    <DebuggerStepThrough()>
    Public Function GetLexeme() As String Implements Irony.Compiler.ISourceStream.GetLexeme
      Return _text.Substring(_tokenStart.Position, _position - _tokenStart.Position)
    End Function

    ' Token: 0x17000010 RID: 16
    ' (get) Token: 0x06000059 RID: 89 RVA: 0x00002A97 File Offset: 0x00000C97
    ' (set) Token: 0x0600005A RID: 90 RVA: 0x00002A9F File Offset: 0x00000C9F
    Public Property TokenStart As SourceLocation Implements Irony.Compiler.ISourceStream.TokenStart
      <DebuggerStepThrough()>
      Get
        Return _tokenStart
      End Get
      Set(value As SourceLocation)
        _tokenStart = value
      End Set
    End Property

    ' Token: 0x0600005B RID: 91 RVA: 0x00002AA8 File Offset: 0x00000CA8
    Public Overrides Function ToString() As String
      Dim result As String
      If Position + 30 < Text.Length Then
        result = Text.Substring(Position, 30)
      Else
        result = Text.Substring(Position)
      End If
      Return result
    End Function

    ' Token: 0x04000066 RID: 102
    Private _fileName As String

    ' Token: 0x04000067 RID: 103
    Private _tabWidth As Integer

    ' Token: 0x04000068 RID: 104
    Private _position As Integer

    ' Token: 0x04000069 RID: 105
    Private _text As String

    ' Token: 0x0400006A RID: 106
    Private _tokenStart As SourceLocation
  End Class
End Namespace
