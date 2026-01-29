Namespace Irony.Compiler
  ' Token: 0x02000032 RID: 50
  Public Class GrammarErrorException
    Inherits Exception

    ' Token: 0x060000FA RID: 250 RVA: 0x00004F61 File Offset: 0x00003161
    Public Sub New(message As String)
      MyBase.New(message)
    End Sub

    ' Token: 0x060000FB RID: 251 RVA: 0x00004F6A File Offset: 0x0000316A
    Public Sub New(message As String, inner As Exception)
      MyBase.New(message, inner)
    End Sub
  End Class
End Namespace
