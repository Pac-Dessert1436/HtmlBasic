Namespace Irony.Compiler
  ' Token: 0x0200002F RID: 47
  Public Class BraceMatchFilter
    Inherits TokenFilter

    ' Token: 0x060000F2 RID: 242 RVA: 0x00004DD4 File Offset: 0x00002FD4
    Public Overrides Iterator Function BeginFiltering(context As CompilerContext, tokens As IEnumerable(Of Token)) As IEnumerable(Of Token)
      For Each token As Token In tokens
        If Not token.Term.IsSet(TermOptions.IsBrace) Then
          Yield token
        ElseIf token.Term.IsSet(TermOptions.IsOpenBrace) Then
          _braces.Push(token)
          Yield token
        ElseIf token.Term.IsSet(TermOptions.IsCloseBrace) Then
          Dim lastOpen As Token = _braces.Peek()
          If _braces.Count > 0 AndAlso lastOpen.Symbol.IsPairFor Is token.Symbol Then
            If BuildPairsList Then
              BracePairs.Add(New BracePair(lastOpen, token))
            End If
            _braces.Pop()
            Yield token
          Else
            Yield Grammar.CreateSyntaxErrorToken(context, token.Span.Start, "Unmatched closing brace '{0}' - expected '{1}'", New Object() {token.Text, lastOpen.Symbol.IsPairFor.Name})
          End If
        End If
      Next
      Return
    End Function

    ' Token: 0x040000A2 RID: 162
    Private _stack As New StringList()

    ' Token: 0x040000A3 RID: 163
    Private _braces As New Stack(Of Token)()

    ' Token: 0x040000A4 RID: 164
    Public BracePairs As New BracePairList()

    ' Token: 0x040000A5 RID: 165
    Public BuildPairsList As Boolean
  End Class
End Namespace
