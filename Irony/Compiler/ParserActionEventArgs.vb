Namespace Irony.Compiler
  ' Token: 0x0200001F RID: 31
  Public Class ParserActionEventArgs
    Inherits EventArgs

    ' Token: 0x06000085 RID: 133 RVA: 0x00003A4E File Offset: 0x00001C4E
    Public Sub New(state As ParserState, input As Token, action As ActionRecord)
      Me.State = state
      Me.Input = input
      Me.Action = action
    End Sub

    ' Token: 0x06000086 RID: 134 RVA: 0x00003A6C File Offset: 0x00001C6C
    Public Overrides Function ToString() As String
      Return String.Concat(New Object() {State, "/", Input, ": ", Action})
    End Function

    ' Token: 0x0400007F RID: 127
    Public State As ParserState

    ' Token: 0x04000080 RID: 128
    Public Input As Token

    ' Token: 0x04000081 RID: 129
    Public Action As ActionRecord
  End Class
End Namespace
