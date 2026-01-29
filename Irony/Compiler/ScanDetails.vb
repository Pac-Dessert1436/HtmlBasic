Namespace Irony.Compiler
  ' Token: 0x0200004A RID: 74
  Public Class ScanDetails
    ' Token: 0x06000177 RID: 375 RVA: 0x00007F87 File Offset: 0x00006187
    Public Function IsSet(flag As ScanFlags) As Boolean
      Return (Flags And flag) <> ScanFlags.None
    End Function

    ' Token: 0x06000178 RID: 376 RVA: 0x00007F97 File Offset: 0x00006197
    Public Function HasError() As Boolean
      Return Not String.IsNullOrEmpty([Error])
    End Function

    ' Token: 0x040000F1 RID: 241
    Public Prefix As String

    ' Token: 0x040000F2 RID: 242
    Public Body As String

    ' Token: 0x040000F3 RID: 243
    Public Suffix As String

    ' Token: 0x040000F4 RID: 244
    Public Flags As ScanFlags

    ' Token: 0x040000F5 RID: 245
    Public [Error] As String

    ' Token: 0x040000F6 RID: 246
    Public TypeCodes As TypeCode()

    ' Token: 0x040000F7 RID: 247
    Public ControlSymbol As String

    ' Token: 0x040000F8 RID: 248
    Public Value As Object
  End Class
End Namespace
