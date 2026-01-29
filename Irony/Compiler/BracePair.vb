Namespace Irony.Compiler
  ' Token: 0x0200002D RID: 45
  Public Class BracePair
    ' Token: 0x060000F0 RID: 240 RVA: 0x00004A54 File Offset: 0x00002C54
    Public Sub New(open As Token, close As Token)
      Me.Open = open
      Me.Close = close
    End Sub

    ' Token: 0x040000A0 RID: 160
    Public Open As Token

    ' Token: 0x040000A1 RID: 161
    Public Close As Token
  End Class
End Namespace
