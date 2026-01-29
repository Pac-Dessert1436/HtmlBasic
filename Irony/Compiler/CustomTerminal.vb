Namespace Irony.Compiler
  ' Token: 0x0200004F RID: 79
  Public Class CustomTerminal
    Inherits Terminal

    ' Token: 0x0600019A RID: 410 RVA: 0x00008B96 File Offset: 0x00006D96
    Public Sub New(name As String, handler As MatchHandler, ParamArray prefixes As String())
      MyBase.New(name)
      _handler = handler
      If prefixes IsNot Nothing Then
        Me.Prefixes.AddRange(prefixes)
      End If
    End Sub

    ' Token: 0x17000044 RID: 68
    ' (get) Token: 0x0600019B RID: 411 RVA: 0x00008BC0 File Offset: 0x00006DC0
    Public ReadOnly Property Handler As MatchHandler
      <DebuggerStepThrough()>
      Get
        Return _handler
      End Get
    End Property

    ' Token: 0x0600019C RID: 412 RVA: 0x00008BC8 File Offset: 0x00006DC8
    Public Overrides Function TryMatch(context As CompilerContext, source As ISourceStream) As Token
      Return _handler(Me, context, source)
    End Function

    ' Token: 0x0600019D RID: 413 RVA: 0x00008BD8 File Offset: 0x00006DD8
    <DebuggerStepThrough()>
    Public Overrides Function GetFirsts() As IList(Of String)
      Return Prefixes
    End Function

    ' Token: 0x0400010F RID: 271
    Public Prefixes As New KeyList()

    ' Token: 0x04000110 RID: 272
    Private _handler As MatchHandler
  End Class
End Namespace
