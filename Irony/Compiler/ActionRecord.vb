Namespace Irony.Compiler
  ' Token: 0x0200003D RID: 61
  Public Class ActionRecord
    ' Token: 0x06000135 RID: 309 RVA: 0x00005C04 File Offset: 0x00003E04
    Friend Sub New(key As String, type As ParserActionType, newState As ParserState, reduceProduction As Production)
      Me.Key = key
      ActionType = type
      Me.NewState = newState
      If reduceProduction IsNot Nothing Then
        ReduceProductions.Add(reduceProduction)
      End If
    End Sub

    ' Token: 0x06000136 RID: 310 RVA: 0x00005C3D File Offset: 0x00003E3D
    Public Function CreateDerived(type As ParserActionType, reduceProduction As Production) As ActionRecord
      Return New ActionRecord(Key, type, NewState, reduceProduction)
    End Function

    ' Token: 0x1700003A RID: 58
    ' (get) Token: 0x06000137 RID: 311 RVA: 0x00005C52 File Offset: 0x00003E52
    Public ReadOnly Property Production As Production
      Get
        If ReduceProductions.Count <= 0 Then
          Return Nothing
        End If
        Return ReduceProductions(0)
      End Get
    End Property

    ' Token: 0x1700003B RID: 59
    ' (get) Token: 0x06000138 RID: 312 RVA: 0x00005C70 File Offset: 0x00003E70
    Public ReadOnly Property NonTerminal As NonTerminal
      Get
        If Production IsNot Nothing Then
          Return Production.LValue
        End If
        Return Nothing
      End Get
    End Property

    ' Token: 0x1700003C RID: 60
    ' (get) Token: 0x06000139 RID: 313 RVA: 0x00005C87 File Offset: 0x00003E87
    Public ReadOnly Property PopCount As Integer
      Get
        Return Production.RValues.Count
      End Get
    End Property

    ' Token: 0x0600013A RID: 314 RVA: 0x00005C9C File Offset: 0x00003E9C
    Public Function HasConflict() As Boolean
      Select Case ActionType
        Case ParserActionType.Shift
          Return ReduceProductions.Count > 0
        Case ParserActionType.Reduce
          Return ReduceProductions.Count > 1
        Case ParserActionType.[Operator]
          Return True
        Case Else
          Return False
      End Select
    End Function

    ' Token: 0x0600013B RID: 315 RVA: 0x00005CE8 File Offset: 0x00003EE8
    Public Overrides Function ToString() As String
      Dim text As String = ActionType.ToString()
      If ActionType = ParserActionType.Reduce AndAlso ReduceProductions.Count > 0 Then
        text = text + " on " + ReduceProductions(0).ToString()
      End If
      Return text
    End Function

    ' Token: 0x040000CF RID: 207
    Public Key As String

    ' Token: 0x040000D0 RID: 208
    Public ActionType As ParserActionType

    ' Token: 0x040000D1 RID: 209
    Public NewState As ParserState

    ' Token: 0x040000D2 RID: 210
    Public ReduceProductions As New ProductionList()
  End Class
End Namespace
