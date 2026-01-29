Imports System.Text

Namespace Irony.Compiler
  ' Token: 0x0200003F RID: 63
  Public Class Production
    ' Token: 0x0600013D RID: 317 RVA: 0x00005D40 File Offset: 0x00003F40
    Public Sub New(isInitial As Boolean, lvalue As NonTerminal, rvalues As BnfTermList)
      Me.LValue = lvalue
      For Each bnfTerm As BnfTerm In rvalues
        If bnfTerm IsNot Grammar.Empty Then
          Me.RValues.Add(bnfTerm)
        End If
      Next
      For Each bnfTerm2 As BnfTerm In Me.RValues
        Dim terminal As Terminal = TryCast(bnfTerm2, Terminal)
        If terminal IsNot Nothing Then
          HasTerminals = True
          If terminal.Category = TokenCategory.[Error] Then
            IsError = True
          End If
        End If
      Next
      For i As Integer = 0 To Me.RValues.Count
        LR0Items.Add(New LR0Item(Me, i))
      Next
    End Sub

    ' Token: 0x0600013E RID: 318 RVA: 0x00005E44 File Offset: 0x00004044
    Public Function IsEmpty() As Boolean
      Return RValues.Count = 0
    End Function

    ' Token: 0x0600013F RID: 319 RVA: 0x00005E54 File Offset: 0x00004054
    Public Overrides Overloads Function ToString() As String
      Return ToString(-1)
    End Function

    ' Token: 0x06000140 RID: 320 RVA: 0x00005E60 File Offset: 0x00004060
    Friend Overloads Function ToString(dotPosition As Integer) As String
      Dim value As Char = "·"c
      Dim stringBuilder As New StringBuilder()
      stringBuilder.Append(LValue.Name)
      stringBuilder.Append(" -> ")
      For i As Integer = 0 To RValues.Count - 1
        If i = dotPosition Then
          stringBuilder.Append(value)
        End If
        stringBuilder.Append(RValues(i).Name)
        stringBuilder.Append(" ")
      Next
      If dotPosition = RValues.Count Then
        stringBuilder.Append(value)
      End If
      Return stringBuilder.ToString()
    End Function

    ' Token: 0x040000D3 RID: 211
    Public IsInitial As Boolean

    ' Token: 0x040000D4 RID: 212
    Public HasTerminals As Boolean

    ' Token: 0x040000D5 RID: 213
    Public IsError As Boolean

    ' Token: 0x040000D6 RID: 214
    Public LValue As NonTerminal

    ' Token: 0x040000D7 RID: 215
    Public RValues As New BnfTermList()

    ' Token: 0x040000D8 RID: 216
    Public LR0Items As New LR0ItemList()
  End Class
End Namespace
