Namespace Irony.Compiler
  ' Token: 0x02000014 RID: 20
  Public Structure SourceSpan
    ' Token: 0x0600004A RID: 74 RVA: 0x0000290E File Offset: 0x00000B0E
    Public Sub New(start As SourceLocation, length As Integer)
      Me.Start = start
      Me.Length = length
    End Sub

    ' Token: 0x17000009 RID: 9
    ' (get) Token: 0x0600004B RID: 75 RVA: 0x0000291E File Offset: 0x00000B1E
    Public ReadOnly Property EndPos As Integer
      Get
        Return Start.Position + Length
      End Get
    End Property

    ' Token: 0x04000064 RID: 100
    Public Start As SourceLocation

    ' Token: 0x04000065 RID: 101
    Public Length As Integer
  End Structure
End Namespace
