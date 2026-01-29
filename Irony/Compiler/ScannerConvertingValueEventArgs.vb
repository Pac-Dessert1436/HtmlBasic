Namespace Irony.Compiler
  ' Token: 0x02000020 RID: 32
  Public Class ScannerConvertingValueEventArgs
    Inherits EventArgs

    ' Token: 0x06000087 RID: 135 RVA: 0x00003AB1 File Offset: 0x00001CB1
    Public Sub New(details As ScanDetails)
      Me.Details = details
    End Sub

    ' Token: 0x04000082 RID: 130
    Public Details As ScanDetails

    ' Token: 0x04000083 RID: 131
    Public Value As Object

    ' Token: 0x04000084 RID: 132
    Public Converted As Boolean
  End Class
End Namespace
