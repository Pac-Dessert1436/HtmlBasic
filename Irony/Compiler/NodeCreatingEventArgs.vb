Namespace Irony.Compiler
  ' Token: 0x02000021 RID: 33
  Public Class NodeCreatingEventArgs
    Inherits EventArgs

    ' Token: 0x06000088 RID: 136 RVA: 0x00003AC0 File Offset: 0x00001CC0
    Public Sub New(context As CompilerContext, state As ParserState, span As SourceSpan, action As ActionRecord, childNodes As AstNodeList)
      Me.Context = context
      Me.State = state
      Me.Span = span
      Me.Action = action
      Me.ChildNodes = childNodes
    End Sub

    ' Token: 0x06000089 RID: 137 RVA: 0x00003AED File Offset: 0x00001CED
    Public Overrides Function ToString() As String
      Return State.ToString() & ": " & Action.ToString()
    End Function

    ' Token: 0x04000085 RID: 133
    Public Context As CompilerContext

    ' Token: 0x04000086 RID: 134
    Public State As ParserState

    ' Token: 0x04000087 RID: 135
    Public Span As SourceSpan

    ' Token: 0x04000088 RID: 136
    Public Action As ActionRecord

    ' Token: 0x04000089 RID: 137
    Public ChildNodes As AstNodeList

    ' Token: 0x0400008A RID: 138
    Public NewNode As AstNode
  End Class
End Namespace
