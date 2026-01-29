Namespace Irony.Compiler
  ' Token: 0x02000056 RID: 86
  Public Class Grammar
    ' Token: 0x1700004A RID: 74
    ' (get) Token: 0x06000239 RID: 569 RVA: 0x0000B373 File Offset: 0x00009573
    ' (set) Token: 0x0600023A RID: 570 RVA: 0x0000B37B File Offset: 0x0000957B
    Public Property Root As NonTerminal
      <DebuggerStepThrough()>
      Get
        Return _root
      End Get
      Set(value As NonTerminal)
        _root = value
      End Set
    End Property

    ' Token: 0x0600023B RID: 571 RVA: 0x0000B384 File Offset: 0x00009584
    Public Shared Sub RegisterPunctuation(ParamArray symbols As String())
      For Each symbol As String In symbols
        Dim symbol2 As SymbolTerminal = SymbolTerminal.GetSymbol(symbol)
        symbol2.SetOption(TermOptions.IsPunctuation)
      Next
    End Sub

    ' Token: 0x0600023C RID: 572 RVA: 0x0000B3B4 File Offset: 0x000095B4
    Public Shared Sub RegisterPunctuation(ParamArray elements As BnfTerm())
      For Each bnfTerm As BnfTerm In elements
        bnfTerm.SetOption(TermOptions.IsPunctuation)
      Next
    End Sub

    ' Token: 0x0600023D RID: 573 RVA: 0x0000B3DD File Offset: 0x000095DD
    Public Sub RegisterOperators(precedence As Integer, ParamArray opSymbols As String())
      RegisterOperators(precedence, Associativity.Left, opSymbols)
    End Sub

    ' Token: 0x0600023E RID: 574 RVA: 0x0000B3E8 File Offset: 0x000095E8
    Public Shared Sub RegisterOperators(precedence As Integer, associativity As Associativity, ParamArray opSymbols As String())
      For Each symbol As String In opSymbols
        Dim symbol2 As SymbolTerminal = SymbolTerminal.GetSymbol(symbol)
        symbol2.SetOption(TermOptions.IsOperator, True)
        symbol2.Precedence = precedence
        symbol2.Associativity = associativity
      Next
    End Sub

    ' Token: 0x0600023F RID: 575 RVA: 0x0000B428 File Offset: 0x00009628
    Public Shared Sub RegisterBracePair(openBrace As String, closeBrace As String)
      Dim symbol As SymbolTerminal = SymbolTerminal.GetSymbol(openBrace)
      Dim symbol2 As SymbolTerminal = SymbolTerminal.GetSymbol(closeBrace)
      symbol.SetOption(TermOptions.IsOpenBrace)
      symbol.IsPairFor = symbol2
      symbol2.SetOption(TermOptions.IsCloseBrace)
      symbol2.IsPairFor = symbol
    End Sub

    ' Token: 0x06000240 RID: 576 RVA: 0x0000B45F File Offset: 0x0000965F
    Public Overridable Function TryMatch(context As CompilerContext, source As ISourceStream) As Token
      Return Nothing
    End Function

    ' Token: 0x06000241 RID: 577 RVA: 0x0000B462 File Offset: 0x00009662
    Public Overridable Function CreateNode(context As CompilerContext, reduceAction As ActionRecord, sourceSpan As SourceSpan, childNodes As AstNodeList) As AstNode
      Return Nothing
    End Function

    ' Token: 0x06000242 RID: 578 RVA: 0x0000B465 File Offset: 0x00009665
    Public Overridable Function GetSyntaxErrorMessage(context As CompilerContext, expectedList As KeyList) As String
      Return Nothing
    End Function

    ' Token: 0x06000243 RID: 579 RVA: 0x0000B468 File Offset: 0x00009668
    Public Overridable Sub OnActionSelected(parser As Parser, input As Token, action As ActionRecord)
    End Sub

    ' Token: 0x06000244 RID: 580 RVA: 0x0000B46A File Offset: 0x0000966A
    Public Overridable Function OnActionConflict(parser As Parser, input As Token, action As ActionRecord) As ActionRecord
      Return action
    End Function

    ' Token: 0x06000245 RID: 581 RVA: 0x0000B46D File Offset: 0x0000966D
    Protected Shared Function Symbol(sym As String) As SymbolTerminal
      Return SymbolTerminal.GetSymbol(sym)
    End Function

    ' Token: 0x06000246 RID: 582 RVA: 0x0000B475 File Offset: 0x00009675
    Protected Shared Function Symbol(sym As String, name As String) As SymbolTerminal
      Return SymbolTerminal.GetSymbol(sym, name)
    End Function

    ' Token: 0x06000247 RID: 583 RVA: 0x0000B480 File Offset: 0x00009680
    Protected Shared Function ToElement(expression As BnfExpression) As BnfTerm
      Dim name As String = expression.ToString()
      Return New NonTerminal(name, expression)
    End Function

    ' Token: 0x06000248 RID: 584 RVA: 0x0000B49B File Offset: 0x0000969B
    Protected Shared Function WithStar(expression As BnfExpression) As BnfTerm
      Return ToElement(expression).Star()
    End Function

    ' Token: 0x06000249 RID: 585 RVA: 0x0000B4A8 File Offset: 0x000096A8
    Protected Shared Function WithPlus(expression As BnfExpression) As BnfTerm
      Return ToElement(expression).Plus()
    End Function

    ' Token: 0x0600024A RID: 586 RVA: 0x0000B4B5 File Offset: 0x000096B5
    Protected Shared Function WithQ(expression As BnfExpression) As BnfTerm
      Return ToElement(expression).Q()
    End Function

    ' Token: 0x0600024B RID: 587 RVA: 0x0000B4C2 File Offset: 0x000096C2
    Public Shared Function CreateSyntaxErrorToken(context As CompilerContext, location As SourceLocation, message As String, ParamArray args As Object()) As Token
      If args IsNot Nothing AndAlso args.Length > 0 Then
        message = String.Format(message, args)
      End If
      Return Token.Create(SyntaxError, context, location, message)
    End Function

    ' Token: 0x0600024C RID: 588 RVA: 0x0000B4E4 File Offset: 0x000096E4
    Public Shared Function MakePlusRule(listNonTerminal As NonTerminal, delimiter As BnfTerm, listMember As BnfTerm) As BnfExpression
      listNonTerminal.SetOption(TermOptions.IsList)
      If delimiter Is Nothing Then
        listNonTerminal.Rule = (listMember Or listNonTerminal + listMember)
      Else
        listNonTerminal.Rule = (listMember Or listNonTerminal + delimiter + listMember)
      End If
      Return listNonTerminal.Rule
    End Function

    ' Token: 0x0600024D RID: 589 RVA: 0x0000B534 File Offset: 0x00009734
    Public Function MakeStarRule(listNonTerminal As NonTerminal, delimiter As BnfTerm, listMember As BnfTerm) As BnfExpression
      If delimiter Is Nothing Then
        listNonTerminal.SetOption(TermOptions.IsList)
        listNonTerminal.Rule = (Empty Or listNonTerminal + listMember)
        Return listNonTerminal.Rule
      End If
      Dim nonTerminal As New NonTerminal(listMember.Name + "+")
      MakePlusRule(nonTerminal, delimiter, listMember)
      listNonTerminal.Rule = (Empty Or nonTerminal)
      Return listNonTerminal.Rule
    End Function

    ' Token: 0x04000125 RID: 293
    Public CaseSensitive As Boolean = True

    ' Token: 0x04000126 RID: 294
    Public Delimiters As String = ",;[](){}"

    ' Token: 0x04000127 RID: 295
    Public WhitespaceChars As String = " " & vbTab & vbCrLf & vbVerticalTab

    ' Token: 0x04000128 RID: 296
    Public LineTerminators As String = vbLf & vbCr & vbVerticalTab

    ' Token: 0x04000129 RID: 297
    Public NonGrammarTerminals As New TerminalList()

    ' Token: 0x0400012A RID: 298
    Public FallbackTerminals As New TerminalList()

    ' Token: 0x0400012B RID: 299
    Public DefaultNodeType As Type = GetType(AstNode)

    ' Token: 0x0400012C RID: 300
    Private _root As NonTerminal

    ' Token: 0x0400012D RID: 301
    Public TokenFilters As New TokenFilterList()

    ' Token: 0x0400012E RID: 302
    Public Shared Empty As New Terminal("EMPTY")

    ' Token: 0x0400012F RID: 303
    Public Shared NewLine As New Terminal("LF", TokenCategory.Outline)

    ' Token: 0x04000130 RID: 304
    Public Shared Indent As New Terminal("INDENT", TokenCategory.Outline)

    ' Token: 0x04000131 RID: 305
    Public Shared Dedent As New Terminal("DEDENT", TokenCategory.Outline)

    ' Token: 0x04000132 RID: 306
    Public Shared Eof As New Terminal("EOF", TokenCategory.Outline)

    ' Token: 0x04000133 RID: 307
    Public Shared Eos As New Terminal("EOS", TokenCategory.Outline)

    ' Token: 0x04000134 RID: 308
    Public Shared SyntaxError As New Terminal("SYNTAX_ERROR", TokenCategory.[Error])
  End Class
End Namespace
