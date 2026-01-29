Namespace Irony.Compiler
  ' Token: 0x02000031 RID: 49
  Public Class BnfExpression
    Inherits BnfTerm

    ' Token: 0x060000F5 RID: 245 RVA: 0x00004E30 File Offset: 0x00003030
    Public Sub New(element As BnfTerm)
      MyBase.New(Nothing)
      Data = New BnfExpressionData()
      Data.Add(New BnfTermList())
      Data(0).Add(element)
    End Sub

    ' Token: 0x060000F6 RID: 246 RVA: 0x00004E68 File Offset: 0x00003068
    Public Overrides Function ToString() As String
      If _toString IsNot Nothing Then
        Return _toString
      End If
      Dim result As String
      Try
        Dim array As String() = New String(Data.Count - 1) {}
        For i As Integer = 0 To Data.Count - 1
          Dim bnfTermList As BnfTermList = Data(i)
          Dim array2 As String() = New String(bnfTermList.Count - 1) {}
          For j As Integer = 0 To bnfTermList.Count - 1
            array2(j) = bnfTermList(j).ToString()
          Next
          array(i) = String.Join("+", array2)
        Next
        _toString = String.Join("|", array)
        result = _toString
      Catch ex As Exception
        result = "(error: " + ex.Message + ")"
      End Try
      Return result
    End Function

    ' Token: 0x060000F7 RID: 247 RVA: 0x00004F44 File Offset: 0x00003144
    Public Shared Widening Operator CType(symbol As String) As BnfExpression
      Return New BnfExpression(SymbolTerminal.GetSymbol(symbol))
    End Operator

    ' Token: 0x060000F8 RID: 248 RVA: 0x00004F51 File Offset: 0x00003151
    Public Shared Widening Operator CType(term As Terminal) As BnfExpression
      Return New BnfExpression(term)
    End Operator

    ' Token: 0x060000F9 RID: 249 RVA: 0x00004F59 File Offset: 0x00003159
    Public Shared Widening Operator CType(nonTerm As NonTerminal) As BnfExpression
      Return New BnfExpression(nonTerm)
    End Operator

    ' Token: 0x040000A6 RID: 166
    Public Data As BnfExpressionData

    ' Token: 0x040000A7 RID: 167
    Private _toString As String
  End Class
End Namespace
