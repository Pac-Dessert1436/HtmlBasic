Namespace Irony.Compiler
  ' Token: 0x02000002 RID: 2
  Public Class LanguageCompiler
    ' Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
    Public Sub New(grammar As Grammar)
      Me.Grammar = grammar
      Dim num As Long = CLng(Environment.TickCount)
      Dim grammarDataBuilder As New GrammarDataBuilder(grammar)
      grammarDataBuilder.Build()
      InitTime = CLng(Environment.TickCount) - num
      Data = grammarDataBuilder.Data
      Parser = New Parser(Data)
      Scanner = New Scanner(Data)
    End Sub

    ' Token: 0x06000002 RID: 2 RVA: 0x000020BA File Offset: 0x000002BA
    Public Sub New(data As GrammarData)
      Me.Data = data
      Grammar = data.Grammar
      Parser = New Parser(Me.Data)
      Scanner = New Scanner(Me.Data)
    End Sub

    ' Token: 0x06000003 RID: 3 RVA: 0x000020F8 File Offset: 0x000002F8
    Public Shared Function CreateDummy() As LanguageCompiler
      Return New LanguageCompiler(New GrammarData() With {.Grammar = New Grammar()})
    End Function

    ' Token: 0x17000001 RID: 1
    ' (get) Token: 0x06000004 RID: 4 RVA: 0x0000211E File Offset: 0x0000031E
    Public ReadOnly Property CompileTime As Long
      <DebuggerStepThrough()>
      Get
        Return _compileTime
      End Get
    End Property

    ' Token: 0x17000002 RID: 2
    ' (get) Token: 0x06000005 RID: 5 RVA: 0x00002126 File Offset: 0x00000326
    Public ReadOnly Property Context As CompilerContext
      <DebuggerStepThrough()>
      Get
        Return _context
      End Get
    End Property

    ' Token: 0x06000006 RID: 6 RVA: 0x0000212E File Offset: 0x0000032E
    Public Function Parse(source As String) As AstNode
      Return Parse(New CompilerContext(Me), New SourceFile(source, "Source"))
    End Function

    ' Token: 0x06000007 RID: 7 RVA: 0x00002148 File Offset: 0x00000348
    Public Function Parse(context As CompilerContext, source As SourceFile) As AstNode
      _context = context
      Dim tickCount As Integer = Environment.TickCount
      Scanner.Prepare(context, source)
      Dim enumerable As IEnumerable(Of Token) = Scanner.BeginScan()
      For Each tokenFilter As TokenFilter In Grammar.TokenFilters
        enumerable = tokenFilter.BeginFiltering(context, enumerable)
      Next
      Dim result As AstNode = Parser.Parse(context, enumerable)
      _compileTime = CLng((Environment.TickCount - tickCount))
      Return result
    End Function

    ' Token: 0x04000001 RID: 1
    Public Grammar As Grammar

    ' Token: 0x04000002 RID: 2
    Public Data As GrammarData

    ' Token: 0x04000003 RID: 3
    Public Scanner As Scanner

    ' Token: 0x04000004 RID: 4
    Public Parser As Parser

    ' Token: 0x04000005 RID: 5
    Public InitTime As Long

    ' Token: 0x04000006 RID: 6
    Private _compileTime As Long

    ' Token: 0x04000007 RID: 7
    Private _context As CompilerContext
  End Class
End Namespace
