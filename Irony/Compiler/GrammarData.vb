Namespace Irony.Compiler
  ' Token: 0x02000038 RID: 56
  Public Class GrammarData
    ' Token: 0x040000BF RID: 191
    Public Grammar As Grammar

    ' Token: 0x040000C0 RID: 192
    Public AugmentedRoot As NonTerminal

    ' Token: 0x040000C1 RID: 193
    Public InitialState As ParserState

    ' Token: 0x040000C2 RID: 194
    Public FinalState As ParserState

    ' Token: 0x040000C3 RID: 195
    Public NonTerminals As New NonTerminalList()

    ' Token: 0x040000C4 RID: 196
    Public Terminals As New TerminalList()

    ' Token: 0x040000C5 RID: 197
    Public TerminalsLookup As New TerminalLookupTable()

    ' Token: 0x040000C6 RID: 198
    Public FallbackTerminals As New TerminalList()

    ' Token: 0x040000C7 RID: 199
    Public Productions As New ProductionList()

    ' Token: 0x040000C8 RID: 200
    Public States As New ParserStateList()

    ' Token: 0x040000C9 RID: 201
    Public Errors As New KeyList()

    ' Token: 0x040000CA RID: 202
    Public ScannerRecoverySymbols As String = ""

    ' Token: 0x040000CB RID: 203
    Public AnalysisCanceled As Boolean
  End Class
End Namespace
