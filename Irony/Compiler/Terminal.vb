Namespace Irony.Compiler
  ' Token: 0x02000010 RID: 16
  Public Class Terminal
    Inherits BnfTerm

    ' Token: 0x06000032 RID: 50 RVA: 0x00002702 File Offset: 0x00000902
    Public Sub New(name As String)
      MyBase.New(name)
      Nullable = False
      NodeType = GetType(Token)
    End Sub

    ' Token: 0x06000033 RID: 51 RVA: 0x0000273B File Offset: 0x0000093B
    Public Sub New(name As String, category As TokenCategory)
      Me.New(name)
      Me.Category = category
    End Sub

    ' Token: 0x06000034 RID: 52 RVA: 0x0000274B File Offset: 0x0000094B
    Public Sub New(name As String, matchMode As TokenMatchMode)
      Me.New(name)
      Me.MatchMode = matchMode
    End Sub

    ' Token: 0x06000035 RID: 53 RVA: 0x0000275B File Offset: 0x0000095B
    Public Overridable Function TryMatch(context As CompilerContext, source As ISourceStream) As Token
      Return Nothing
    End Function

    ' Token: 0x06000036 RID: 54 RVA: 0x0000275E File Offset: 0x0000095E
    Public Overridable Function GetFirsts() As IList(Of String)
      Return Nothing
    End Function

    ' Token: 0x06000037 RID: 55 RVA: 0x00002761 File Offset: 0x00000961
    Public Shared Function ByName(x As Terminal, y As Terminal) As Integer
      Return String.Compare(x.ToString(), y.ToString())
    End Function

    ' Token: 0x06000038 RID: 56 RVA: 0x00002774 File Offset: 0x00000974
    Public Shared Function ByPriorityReverse(x As Terminal, y As Terminal) As Integer
      If x.Priority > y.Priority Then
        Return -1
      End If
      If x.Priority = y.Priority Then
        Return 0
      End If
      Return 1
    End Function

    ' Token: 0x0400005A RID: 90
    Public MatchMode As TokenMatchMode = TokenMatchMode.ByValueThenByType

    ' Token: 0x0400005B RID: 91
    Public Category As TokenCategory

    ' Token: 0x0400005C RID: 92
    Public Precedence As Integer = Integer.MaxValue

    ' Token: 0x0400005D RID: 93
    Public Associativity As Associativity = Associativity.Neutral

    ' Token: 0x0400005E RID: 94
    Public IsPairFor As Terminal

    ' Token: 0x0400005F RID: 95
    Public Priority As Integer
  End Class
End Namespace
