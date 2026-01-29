Namespace Irony.Compiler
  ' Token: 0x02000057 RID: 87
  Public Structure AstNodeArgs
    ' Token: 0x06000250 RID: 592 RVA: 0x0000B68C File Offset: 0x0000988C
    Public Sub New(term As BnfTerm, context As CompilerContext, span As SourceSpan, childNodes As AstNodeList)
      Me.Context = context
      Me.Term = term
      Me.Span = span
      Me.ChildNodes = childNodes
    End Sub

    ' Token: 0x1700004B RID: 75
    ' (get) Token: 0x06000251 RID: 593 RVA: 0x0000B6AB File Offset: 0x000098AB
    Public ReadOnly Property NonTerminal As NonTerminal
      Get
        Return TryCast(Term, NonTerminal)
      End Get
    End Property

    ' Token: 0x1700004C RID: 76
    ' (get) Token: 0x06000252 RID: 594 RVA: 0x0000B6B8 File Offset: 0x000098B8
    Public ReadOnly Property Terminal As Terminal
      Get
        Return TryCast(Term, Terminal)
      End Get
    End Property

    ' Token: 0x04000135 RID: 309
    Public Term As BnfTerm

    ' Token: 0x04000136 RID: 310
    Public Context As CompilerContext

    ' Token: 0x04000137 RID: 311
    Public Span As SourceSpan

    ' Token: 0x04000138 RID: 312
    Public ChildNodes As AstNodeList
  End Structure
End Namespace
