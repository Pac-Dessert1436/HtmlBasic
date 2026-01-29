Namespace Irony.Compiler
  ' Token: 0x0200002A RID: 42
  Public Class NonTerminal
    Inherits BnfTerm

    ' Token: 0x060000CA RID: 202 RVA: 0x000046E1 File Offset: 0x000028E1
    Public Sub New(name As String)
      MyBase.New(name)
    End Sub

    ' Token: 0x060000CB RID: 203 RVA: 0x0000470B File Offset: 0x0000290B
    Public Sub New(name As String, [alias] As String)
      MyBase.New(name, [alias])
    End Sub

    ' Token: 0x060000CC RID: 204 RVA: 0x00004736 File Offset: 0x00002936
    Public Sub New(name As String, nodeType As Type)
      Me.New(name)
      Me.NodeType = nodeType
    End Sub

    ' Token: 0x060000CD RID: 205 RVA: 0x00004746 File Offset: 0x00002946
    Public Sub New(nodeType As Type)
      Me.New(nodeType.Name)
      Me.NodeType = nodeType
    End Sub

    ' Token: 0x060000CE RID: 206 RVA: 0x0000475B File Offset: 0x0000295B
    Public Sub New(name As String, expression As BnfExpression)
      Me.New(name)
      _rule = expression
    End Sub

    ' Token: 0x17000017 RID: 23
    ' (get) Token: 0x060000CF RID: 207 RVA: 0x0000476B File Offset: 0x0000296B
    ' (set) Token: 0x060000D0 RID: 208 RVA: 0x00004773 File Offset: 0x00002973
    Public Property Rule As BnfExpression
      <DebuggerStepThrough()>
      Get
        Return _rule
      End Get
      Set(value As BnfExpression)
        _rule = value
      End Set
    End Property

    ' Token: 0x17000018 RID: 24
    ' (get) Token: 0x060000D1 RID: 209 RVA: 0x0000477C File Offset: 0x0000297C
    ' (set) Token: 0x060000D2 RID: 210 RVA: 0x00004784 File Offset: 0x00002984
    Public Property ErrorRule As BnfExpression
      <DebuggerStepThrough()>
      Get
        Return _errorRule
      End Get
      Set(value As BnfExpression)
        _errorRule = value
      End Set
    End Property

    ' Token: 0x14000002 RID: 2
    ' (add) Token: 0x060000D3 RID: 211 RVA: 0x0000478D File Offset: 0x0000298D
    ' (remove) Token: 0x060000D4 RID: 212 RVA: 0x000047A6 File Offset: 0x000029A6
    Public Event NodeCreating As EventHandler(Of NodeCreatingEventArgs)

    ' Token: 0x14000003 RID: 3
    ' (add) Token: 0x060000D5 RID: 213 RVA: 0x000047BF File Offset: 0x000029BF
    ' (remove) Token: 0x060000D6 RID: 214 RVA: 0x000047D8 File Offset: 0x000029D8
    Public Event NodeCreated As EventHandler(Of NodeCreatedEventArgs)

    ' Token: 0x060000D7 RID: 215 RVA: 0x000047F4 File Offset: 0x000029F4
    Protected Friend Function OnNodeCreating(context As CompilerContext, state As ParserState, action As ActionRecord, span As SourceSpan, childNodes As AstNodeList) As AstNode
      Dim nodeCreatingEventArgs As New NodeCreatingEventArgs(context, state, span, action, childNodes)
      RaiseEvent NodeCreating(Me, nodeCreatingEventArgs)
      Return nodeCreatingEventArgs.NewNode
    End Function

    ' Token: 0x060000D8 RID: 216 RVA: 0x0000482C File Offset: 0x00002A2C
    Protected Friend Sub OnNodeCreated(node As AstNode)
      Dim e As New NodeCreatedEventArgs(node)
      RaiseEvent NodeCreated(Me, e)
    End Sub

    ' Token: 0x060000D9 RID: 217 RVA: 0x00004858 File Offset: 0x00002A58
    Public Overrides Function ToString() As String
      Dim result As String = Name
      If String.IsNullOrEmpty(Name) Then
        result = "(unnamed)"
      End If
      Return result
    End Function

    ' Token: 0x04000096 RID: 150
    Private _rule As BnfExpression

    ' Token: 0x04000097 RID: 151
    Private _errorRule As BnfExpression

    ' Token: 0x04000098 RID: 152
    Public Productions As New ProductionList()

    ' Token: 0x04000099 RID: 153
    Public Firsts As New KeyList()

    ' Token: 0x0400009A RID: 154
    Public PropagateFirstsTo As New NonTerminalList()
  End Class
End Namespace
