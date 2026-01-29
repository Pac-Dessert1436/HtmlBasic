Namespace Irony.Compiler
  ' Token: 0x02000013 RID: 19
  Public Structure SourceLocation
    ' Token: 0x06000048 RID: 72 RVA: 0x000028B1 File Offset: 0x00000AB1
    Public Sub New(position As Integer, line As Integer, column As Integer)
      Me.Position = position
      Me.Line = line
      Me.Column = column
    End Sub

    ' Token: 0x06000049 RID: 73 RVA: 0x000028C8 File Offset: 0x00000AC8
    Public Overrides Function ToString() As String
      Return String.Concat(New Object() {"L", Line, ":C", Column})
    End Function

    ' Token: 0x04000061 RID: 97
    Public Position As Integer

    ' Token: 0x04000062 RID: 98
    Public Line As Integer

    ' Token: 0x04000063 RID: 99
    Public Column As Integer
  End Structure
End Namespace
