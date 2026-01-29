Namespace Irony.Compiler
  ' Token: 0x02000026 RID: 38
  Public Class SymbolTerminal
    Inherits Terminal

    ' Token: 0x06000091 RID: 145 RVA: 0x00003ED4 File Offset: 0x000020D4
    Private Sub New(symbol As String, name As String)
      MyBase.New(name)
      _symbol = symbol
      Key = symbol.Trim()
      Priority = -1000 + symbol.Length
    End Sub

    ' Token: 0x17000012 RID: 18
    ' (get) Token: 0x06000092 RID: 146 RVA: 0x00003F02 File Offset: 0x00002102
    Public ReadOnly Property Symbol As String
      <DebuggerStepThrough()>
      Get
        Return _symbol
      End Get
    End Property

    ' Token: 0x06000093 RID: 147 RVA: 0x00003F0C File Offset: 0x0000210C
    Public Overrides Function TryMatch(context As CompilerContext, source As ISourceStream) As Token
      If Not source.MatchSymbol(_symbol, Not Grammar.CaseSensitive) Then
        Return Nothing
      End If
      source.Position += _symbol.Length
      Return Token.Create(Me, context, source.TokenStart, Symbol)
    End Function

    ' Token: 0x06000094 RID: 148 RVA: 0x00003F64 File Offset: 0x00002164
    Public Overrides Function GetFirsts() As IList(Of String)
      Return New String() {_symbol}
    End Function

    ' Token: 0x06000095 RID: 149 RVA: 0x00003F82 File Offset: 0x00002182
    Public Overrides Function ToString() As String
      Return _symbol
    End Function

    ' Token: 0x06000096 RID: 150 RVA: 0x00003F8A File Offset: 0x0000218A
    Public Shared Sub ClearSymbols()
      _symbols.Clear()
    End Sub

    ' Token: 0x06000097 RID: 151 RVA: 0x00003F96 File Offset: 0x00002196
    Public Shared Function GetSymbol(symbol As String) As SymbolTerminal
      Return GetSymbol(symbol, symbol)
    End Function

    ' Token: 0x06000098 RID: 152 RVA: 0x00003FA0 File Offset: 0x000021A0
    Public Shared Function GetSymbol(symbol As String, name As String) As SymbolTerminal
      Dim symbolTerminal As SymbolTerminal = Nothing
      If _symbols.TryGetValue(symbol, symbolTerminal) Then
        If name <> symbol AndAlso symbolTerminal.Name <> name Then
          symbolTerminal.Name = name
        End If
        Return symbolTerminal
      End If
      String.Intern(symbol)
      symbolTerminal = New SymbolTerminal(symbol, name)
      symbolTerminal.SetOption(TermOptions.IsGrammarSymbol, True)
      _symbols(symbol) = symbolTerminal
      Return symbolTerminal
    End Function

    ' Token: 0x06000099 RID: 153 RVA: 0x00004000 File Offset: 0x00002200
    <DebuggerStepThrough()>
    Public Overrides Function Equals(obj As Object) As Boolean
      Return MyBase.Equals(obj)
    End Function

    ' Token: 0x0600009A RID: 154 RVA: 0x00004009 File Offset: 0x00002209
    <DebuggerStepThrough()>
    Public Overrides Function GetHashCode() As Integer
      Return _symbol.GetHashCode()
    End Function

    ' Token: 0x04000091 RID: 145
    Private _symbol As String

    ' Token: 0x04000092 RID: 146
    Private Shared _symbols As New SymbolTerminalTable()
  End Class
End Namespace
