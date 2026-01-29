Namespace Irony.Compiler
  ' Token: 0x02000041 RID: 65
  Public Class LRItem
    ' Token: 0x06000142 RID: 322 RVA: 0x00005F01 File Offset: 0x00004101
    Public Sub New(state As ParserState, core As LR0Item)
      Me.State = state
      Me.Core = core
    End Sub

    ' Token: 0x06000143 RID: 323 RVA: 0x00005F38 File Offset: 0x00004138
    Public Overrides Function ToString() As String
      Return Core.ToString() + "  LOOKAHEADS: " + Lookaheads.ToString(" ")
    End Function

    ' Token: 0x040000D9 RID: 217
    Public State As ParserState

    ' Token: 0x040000DA RID: 218
    Public Core As LR0Item

    ' Token: 0x040000DB RID: 219
    Public PropagateTargets As New LRItemList()

    ' Token: 0x040000DC RID: 220
    Public Lookaheads As New KeyList()

    ' Token: 0x040000DD RID: 221
    Public NewLookaheads As New KeyList()
  End Class
End Namespace
