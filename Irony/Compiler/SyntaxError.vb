Namespace Irony.Compiler
  ' Token: 0x02000050 RID: 80
  Public Class SyntaxError
    ' Token: 0x0600019E RID: 414 RVA: 0x00008BE0 File Offset: 0x00006DE0
    Public Sub New(location As SourceLocation, message As String, state As ParserState)
      Me.Location = location
      Me.Message = message
      Me.State = state
    End Sub

    ' Token: 0x0600019F RID: 415 RVA: 0x00008BFD File Offset: 0x00006DFD
    Public Overrides Function ToString() As String
      Return Message
    End Function

    ' Token: 0x04000111 RID: 273
    Public Location As SourceLocation

    ' Token: 0x04000112 RID: 274
    Public Message As String

    ' Token: 0x04000113 RID: 275
    Public State As ParserState
  End Class
End Namespace
