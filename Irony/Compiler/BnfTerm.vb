Namespace Irony.Compiler
  ' Token: 0x0200000F RID: 15
  Public Class BnfTerm
    ' Token: 0x0600001A RID: 26 RVA: 0x000023E4 File Offset: 0x000005E4
    Public Sub New(name As String)
      Me.New(name, name)
    End Sub

    ' Token: 0x0600001B RID: 27 RVA: 0x000023EE File Offset: 0x000005EE
    Public Sub New(name As String, displayName As String)
      Me.Name = name
      Me.DisplayName = displayName
      Key = Me.Name + vbBack
    End Sub

    ' Token: 0x0600001C RID: 28 RVA: 0x0000241A File Offset: 0x0000061A
    Public Overridable Sub Init(grammar As Grammar)
      Me.Grammar = grammar
    End Sub

    ' Token: 0x0600001D RID: 29 RVA: 0x00002423 File Offset: 0x00000623
    Public Overrides Function ToString() As String
      Return "[" + Name + "]"
    End Function

    ' Token: 0x0600001E RID: 30 RVA: 0x0000243A File Offset: 0x0000063A
    <DebuggerStepThrough()>
    Public Function IsSet([option] As TermOptions) As Boolean
      Return (Options And [option]) <> TermOptions.None
    End Function

    ' Token: 0x0600001F RID: 31 RVA: 0x0000244A File Offset: 0x0000064A
    <DebuggerStepThrough()>
    Public Sub SetOption([option] As TermOptions)
      SetOption([option], True)
    End Sub

    ' Token: 0x06000020 RID: 32 RVA: 0x00002454 File Offset: 0x00000654
    <DebuggerStepThrough()>
    Public Sub SetOption([option] As TermOptions, value As Boolean)
      If value Then
        Options = Options Or [option]
        Return
      End If
      Options = Options And Not [option]
    End Sub

    ' Token: 0x06000021 RID: 33 RVA: 0x00002478 File Offset: 0x00000678
    Public Function Q() As BnfExpression
      Dim bnfExpression As BnfExpression = Grammar.Empty Or Me
      bnfExpression.Name = Name + "?"
      Return bnfExpression
    End Function

    ' Token: 0x06000022 RID: 34 RVA: 0x000024A8 File Offset: 0x000006A8
    Public Function Plus() As NonTerminal
      Return Plus(Name + "+")
    End Function

    ' Token: 0x06000023 RID: 35 RVA: 0x000024C0 File Offset: 0x000006C0
    Public Function Plus(name As String) As NonTerminal
      Dim nonTerminal As New NonTerminal(name)
      nonTerminal.SetOption(TermOptions.IsList)
      nonTerminal.Rule = (Me Or nonTerminal + Me)
      Return nonTerminal
    End Function

    ' Token: 0x06000024 RID: 36 RVA: 0x000024F3 File Offset: 0x000006F3
    Public Function Plus(delimiter As BnfTerm) As NonTerminal
      Return Plus(Name + "_list", delimiter)
    End Function

    ' Token: 0x06000025 RID: 37 RVA: 0x0000250C File Offset: 0x0000070C
    Public Function Plus(name As String, delimiter As BnfTerm) As NonTerminal
      Dim nonTerminal As New NonTerminal(name)
      nonTerminal.SetOption(TermOptions.IsList)
      nonTerminal.Rule = (Me Or nonTerminal + delimiter + Me)
      Return nonTerminal
    End Function

    ' Token: 0x06000026 RID: 38 RVA: 0x00002545 File Offset: 0x00000745
    Public Function Star() As NonTerminal
      Return Star(Name + "*")
    End Function

    ' Token: 0x06000027 RID: 39 RVA: 0x00002560 File Offset: 0x00000760
    Public Function Star(name As String) As NonTerminal
      Dim nonTerminal As New NonTerminal(name)
      nonTerminal.SetOption(TermOptions.IsList)
      nonTerminal.Rule = (Grammar.Empty Or nonTerminal + Me)
      Return nonTerminal
    End Function

    ' Token: 0x06000028 RID: 40 RVA: 0x00002597 File Offset: 0x00000797
    Public Function Star(delimiter As BnfTerm) As NonTerminal
      Return Star(Name + "*", delimiter)
    End Function

    ' Token: 0x06000029 RID: 41 RVA: 0x000025B0 File Offset: 0x000007B0
    Public Function Star(name As String, delimiter As BnfTerm) As NonTerminal
      Dim term As NonTerminal = Plus(Me.Name + "+")
      Return New NonTerminal(name) With {.Rule = (Grammar.Empty Or term)}
    End Function

    ' Token: 0x0600002A RID: 42 RVA: 0x000025ED File Offset: 0x000007ED
    Public Shared Operator +(term1 As BnfTerm, term2 As BnfTerm) As BnfExpression
      Return Op_Plus(term1, term2)
    End Operator

    ' Token: 0x0600002B RID: 43 RVA: 0x000025F6 File Offset: 0x000007F6
    Public Shared Operator +(term1 As BnfTerm, symbol2 As String) As BnfExpression
      Return Op_Plus(term1, SymbolTerminal.GetSymbol(symbol2))
    End Operator

    ' Token: 0x0600002C RID: 44 RVA: 0x00002604 File Offset: 0x00000804
    Public Shared Operator +(symbol1 As String, term2 As BnfTerm) As BnfExpression
      Return Op_Plus(SymbolTerminal.GetSymbol(symbol1), term2)
    End Operator

    ' Token: 0x0600002D RID: 45 RVA: 0x00002612 File Offset: 0x00000812
    Public Shared Operator Or(term1 As BnfTerm, term2 As BnfTerm) As BnfExpression
      Return Op_Pipe(term1, term2)
    End Operator

    ' Token: 0x0600002E RID: 46 RVA: 0x0000261B File Offset: 0x0000081B
    Public Shared Operator Or(term1 As BnfTerm, symbol2 As String) As BnfExpression
      Return Op_Pipe(term1, SymbolTerminal.GetSymbol(symbol2))
    End Operator

    ' Token: 0x0600002F RID: 47 RVA: 0x00002629 File Offset: 0x00000829
    Public Shared Operator Or(symbol1 As String, term2 As BnfTerm) As BnfExpression
      Return Op_Pipe(SymbolTerminal.GetSymbol(symbol1), term2)
    End Operator

    ' Token: 0x06000030 RID: 48 RVA: 0x00002638 File Offset: 0x00000838
    Friend Shared Function Op_Plus(term1 As BnfTerm, term2 As BnfTerm) As BnfExpression
      Dim bnfExpression As BnfExpression = TryCast(term1, BnfExpression)
      If bnfExpression Is Nothing OrElse bnfExpression.Data.Count > 1 Then
        bnfExpression = New BnfExpression(term1)
      End If
      bnfExpression.Data(bnfExpression.Data.Count - 1).Add(term2)
      Return bnfExpression
    End Function

    ' Token: 0x06000031 RID: 49 RVA: 0x00002684 File Offset: 0x00000884
    Friend Shared Function Op_Pipe(term1 As BnfTerm, term2 As BnfTerm) As BnfExpression
      Dim bnfExpression As BnfExpression = TryCast(term1, BnfExpression)
      If bnfExpression Is Nothing Then
        bnfExpression = New BnfExpression(term1)
      End If
      Dim bnfExpression2 As BnfExpression = TryCast(term2, BnfExpression)
      If bnfExpression2 IsNot Nothing AndAlso bnfExpression2.Data.Count = 1 Then
        bnfExpression.Data.Add(bnfExpression2.Data(0))
        Return bnfExpression
      End If
      bnfExpression.Data.Add(New BnfTermList())
      bnfExpression.Data(bnfExpression.Data.Count - 1).Add(term2)
      Return bnfExpression
    End Function

    ' Token: 0x04000053 RID: 83
    Public Name As String

    ' Token: 0x04000054 RID: 84
    Public DisplayName As String

    ' Token: 0x04000055 RID: 85
    Public Key As String

    ' Token: 0x04000056 RID: 86
    Public Options As TermOptions

    ' Token: 0x04000057 RID: 87
    Public NodeType As Type

    ' Token: 0x04000058 RID: 88
    Protected Grammar As Grammar

    ' Token: 0x04000059 RID: 89
    Public Nullable As Boolean
  End Class
End Namespace
