Namespace Irony.Compiler
  ' Token: 0x0200001E RID: 30
  Public Class TokenEventArgs
    Inherits EventArgs

    ' Token: 0x06000082 RID: 130 RVA: 0x00003A2E File Offset: 0x00001C2E
    Friend Sub New(token As Token)
      _token = token
    End Sub

    ' Token: 0x17000011 RID: 17
    ' (get) Token: 0x06000083 RID: 131 RVA: 0x00003A3D File Offset: 0x00001C3D
    ' (set) Token: 0x06000084 RID: 132 RVA: 0x00003A45 File Offset: 0x00001C45
    Public Property Token As Token
      Get
        Return _token
      End Get
      Set(value As Token)
        _token = value
      End Set
    End Property

    ' Token: 0x0400007E RID: 126
    Private _token As Token
  End Class
End Namespace
