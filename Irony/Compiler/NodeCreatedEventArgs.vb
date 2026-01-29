Namespace Irony.Compiler
  ' Token: 0x02000022 RID: 34
  Public Class NodeCreatedEventArgs
    Inherits EventArgs

    ' Token: 0x0600008A RID: 138 RVA: 0x00003B05 File Offset: 0x00001D05
    Public Sub New(node As AstNode)
      Me.Node = node
    End Sub

    ' Token: 0x0400008B RID: 139
    Public Node As AstNode
  End Class
End Namespace
