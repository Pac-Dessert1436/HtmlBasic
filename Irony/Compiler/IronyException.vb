Namespace Irony.Compiler
  ' Token: 0x02000033 RID: 51
  Public Class IronyException
    Inherits Exception

    ' Token: 0x060000FC RID: 252 RVA: 0x00004F74 File Offset: 0x00003174
    Public Sub New(message As String)
      MyBase.New(message)
    End Sub

    ' Token: 0x060000FD RID: 253 RVA: 0x00004F7D File Offset: 0x0000317D
    Public Sub New(message As String, inner As Exception)
      MyBase.New(message, inner)
    End Sub
  End Class
End Namespace
