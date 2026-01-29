Imports System.Text.RegularExpressions

Namespace Irony.Compiler
  ' Token: 0x02000028 RID: 40
  Public Class RegexBasedTerminal
    Inherits Terminal

    ' Token: 0x060000C6 RID: 198 RVA: 0x00004653 File Offset: 0x00002853
    Public Sub New(pattern As String)
      MyBase.New("RegEx:{" + pattern + "}")
      _expression = New Regex(pattern)
    End Sub

    ' Token: 0x17000016 RID: 22
    ' (get) Token: 0x060000C7 RID: 199 RVA: 0x00004677 File Offset: 0x00002877
    Public ReadOnly Property Expression As Regex
      Get
        Return _expression
      End Get
    End Property

    ' Token: 0x060000C8 RID: 200 RVA: 0x00004680 File Offset: 0x00002880
    Public Overrides Function TryMatch(context As CompilerContext, source As ISourceStream) As Token
      Dim match As Match = _expression.Match(source.Text, source.Position)
      If Not match.Success Then
        Return Nothing
      End If
      source.Position += match.Length + 1
      Dim lexeme As String = source.GetLexeme()
      Return Token.Create(Me, context, source.TokenStart, lexeme)
    End Function

    ' Token: 0x04000095 RID: 149
    Private _expression As Regex
  End Class
End Namespace
