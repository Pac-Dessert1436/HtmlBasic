Namespace Irony.Compiler
  ' Token: 0x02000052 RID: 82
  Public Structure ParserStackElement
    ' Token: 0x060001A1 RID: 417 RVA: 0x00008C0D File Offset: 0x00006E0D
    Public Sub New(node As AstNode, state As ParserState)
      Me.Node = node
      Me.State = state
    End Sub

    ' Token: 0x060001A2 RID: 418 RVA: 0x00008C1D File Offset: 0x00006E1D
    Public Overrides Function ToString() As String
      Return State.Name + " " + Node.ToString()
    End Function

    ' Token: 0x04000114 RID: 276
    Public Node As AstNode

    ' Token: 0x04000115 RID: 277
    Public State As ParserState
  End Structure
End Namespace
