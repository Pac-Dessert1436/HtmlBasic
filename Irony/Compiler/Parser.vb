Namespace Irony.Compiler
  ' Token: 0x0200004B RID: 75
  Public Class Parser
    ' Token: 0x0600017A RID: 378 RVA: 0x00007FAF File Offset: 0x000061AF
    Public Sub New(data As GrammarData)
      Me.Data = data
    End Sub

    ' Token: 0x1700003F RID: 63
    ' (get) Token: 0x0600017B RID: 379 RVA: 0x00007FE0 File Offset: 0x000061E0
    Public ReadOnly Property Input As IEnumerator(Of Token)
      <DebuggerStepThrough()>
      Get
        Return _input
      End Get
    End Property

    ' Token: 0x17000040 RID: 64
    ' (get) Token: 0x0600017C RID: 380 RVA: 0x00007FE8 File Offset: 0x000061E8
    Public ReadOnly Property CurrentToken As Token
      <DebuggerStepThrough()>
      Get
        Return _currentToken
      End Get
    End Property

    ' Token: 0x17000041 RID: 65
    ' (get) Token: 0x0600017D RID: 381 RVA: 0x00007FF0 File Offset: 0x000061F0
    Public ReadOnly Property CurrentState As ParserState
      <DebuggerStepThrough()>
      Get
        Return _currentState
      End Get
    End Property

    ' Token: 0x17000042 RID: 66
    ' (get) Token: 0x0600017E RID: 382 RVA: 0x00007FF8 File Offset: 0x000061F8
    Public ReadOnly Property LineCount As Integer
      <DebuggerStepThrough()>
      Get
        Return _currentLine
      End Get
    End Property

    ' Token: 0x17000043 RID: 67
    ' (get) Token: 0x0600017F RID: 383 RVA: 0x00008000 File Offset: 0x00006200
    Public ReadOnly Property TokenCount As Integer
      <DebuggerStepThrough()>
      Get
        Return _tokenCount
      End Get
    End Property

    ' Token: 0x14000005 RID: 5
    ' (add) Token: 0x06000180 RID: 384 RVA: 0x00008008 File Offset: 0x00006208
    ' (remove) Token: 0x06000181 RID: 385 RVA: 0x00008021 File Offset: 0x00006221
    Public Event ActionSelected As EventHandler(Of ParserActionEventArgs)

    ' Token: 0x14000006 RID: 6
    ' (add) Token: 0x06000182 RID: 386 RVA: 0x0000803A File Offset: 0x0000623A
    ' (remove) Token: 0x06000183 RID: 387 RVA: 0x00008053 File Offset: 0x00006253
    Public Event TokenReceived As EventHandler(Of TokenEventArgs)

    ' Token: 0x06000184 RID: 388 RVA: 0x0000806C File Offset: 0x0000626C
    Protected Sub OnTokenReceived(token As Token)
      _tokenArgs.Token = token
      RaiseEvent TokenReceived(Me, _tokenArgs)
    End Sub

    ' Token: 0x06000185 RID: 389 RVA: 0x00008095 File Offset: 0x00006295
    Private Sub Reset()
      Stack.Reset()
      _currentState = Data.InitialState
      _currentLine = 0
      _tokenCount = 0
      _context.Errors.Clear()
    End Sub

    ' Token: 0x06000186 RID: 390 RVA: 0x000080D4 File Offset: 0x000062D4
    Private Function ReadToken() As Token
      While _input.MoveNext()
        Dim token As Token = _input.Current
        _tokenCount += 1
        _currentLine = token.Span.Start.Line + 1
        OnTokenReceived(token)
        If Not token.Terminal.IsSet(TermOptions.IsNonGrammar) Then
          Return token
        End If
      End While
      Return Nothing
    End Function

    ' Token: 0x06000187 RID: 391 RVA: 0x00008148 File Offset: 0x00006348
    Private Sub NextToken()
      If _previewBuffer.Count > 0 Then
        _currentToken = _previewBuffer(0)
        _previewBuffer.RemoveAt(0)
        Return
      End If
      _currentToken = ReadToken()
      If _currentToken Is Nothing Then
        _currentToken = Token.Create(Grammar.Eof, _context, New SourceLocation(0, _currentLine - 1, 0), String.Empty)
      End If
    End Sub

    ' Token: 0x06000188 RID: 392 RVA: 0x000081C0 File Offset: 0x000063C0
    Public Function PreviewSymbols(symbols As KeyList) As Token
      Dim enumerator As List(Of Token).Enumerator = _previewBuffer.GetEnumerator()
      Using enumerator
        While enumerator.MoveNext()
          Dim token As Token = enumerator.Current
          If symbols.Contains(token.Text) Then
            Return token
          End If
        End While
        GoTo IL_5D
      End Using
IL_41:
      Dim token2 As Token = Nothing
      _previewBuffer.Add(token2)
      If symbols.Contains(token2.Text) Then
        Return token2
      End If
IL_5D:
      Dim token3 As Token = ReadToken()
      token2 = token3
      If token3 Is Nothing Then
        Return Nothing
      End If
      GoTo IL_41
    End Function

    ' Token: 0x06000189 RID: 393 RVA: 0x00008248 File Offset: 0x00006448
    Public Function Parse(context As CompilerContext, tokenStream As IEnumerable(Of Token)) As AstNode
      _context = context
      _caseSensitive = _context.Compiler.Grammar.CaseSensitive
      Reset()
      _input = tokenStream.GetEnumerator()
      NextToken()
      While _currentState IsNot Data.FinalState
        If _currentToken.Terminal.Category = TokenCategory.[Error] Then
          ReportScannerError()
          If Not Recover() Then
            Return Nothing
          End If
        Else
          Dim actionRecord As ActionRecord = GetCurrentAction()
          If actionRecord Is Nothing Then
            ReportParserError()
            If Not Recover() Then
              Return Nothing
            End If
          Else
            If actionRecord.HasConflict() Then
              actionRecord = Data.Grammar.OnActionConflict(Me, _currentToken, actionRecord)
            End If
            OnActionSelected(_currentToken, actionRecord)
            Select Case actionRecord.ActionType
              Case ParserActionType.Shift
              Case ParserActionType.Reduce
                GoTo IL_10F
              Case ParserActionType.[Operator]
                If GetActionTypeForOperation(_currentToken) <> ParserActionType.Shift Then
                  GoTo IL_10F
                End If
              Case Else
                Continue While
            End Select
            ExecuteShiftAction(actionRecord)
            Continue While
IL_10F:
            ExecuteReduceAction(actionRecord)
          End If
        End If
      End While
      Dim node As AstNode = Stack(0).Node
      Stack.Reset()
      Return node
    End Function

    ' Token: 0x0600018A RID: 394 RVA: 0x0000836F File Offset: 0x0000656F
    Private Sub ReportError(location As SourceLocation, message As String, ParamArray args As Object())
      If args IsNot Nothing AndAlso args.Length > 0 Then
        message = String.Format(message, args)
      End If
      _context.AddError(location, message, _currentState)
    End Sub

    ' Token: 0x0600018B RID: 395 RVA: 0x00008396 File Offset: 0x00006596
    Private Sub ReportScannerError()
      _context.AddError(_currentToken.Location, _currentToken.Text, _currentState)
    End Sub

    ' Token: 0x0600018C RID: 396 RVA: 0x000083C0 File Offset: 0x000065C0
    Private Sub ReportParserError()
      If _currentToken.Terminal Is Grammar.Eof Then
        ReportError(_currentToken.Location, "Unexpected end of file.", New Object(-1) {})
        Return
      End If
      Dim currentExpectedSymbols As KeyList = GetCurrentExpectedSymbols()
      Dim text As String = Data.Grammar.GetSyntaxErrorMessage(_context, currentExpectedSymbols)
      If text Is Nothing Then
        text = "Syntax error" + If((currentExpectedSymbols.Count = 0), ".", (", expected: " + currentExpectedSymbols.ToString(" ")))
      End If
      ReportError(_currentToken.Location, text, New Object(-1) {})
    End Sub

    ' Token: 0x0600018D RID: 397 RVA: 0x00008468 File Offset: 0x00006668
    Private Function GetCurrentExpectedSymbols() As KeyList
      Dim bnfTermList As New BnfTermList()
      Dim keyList As New KeyList()
      keyList.AddRange(_currentState.Actions.Keys)
      For Each nonTerminal As NonTerminal In Data.NonTerminals
        If keyList.Contains(nonTerminal.Key) Then
          If String.IsNullOrEmpty(nonTerminal.DisplayName) Then
            keyList.Remove(nonTerminal.Key)
          Else
            bnfTermList.Add(nonTerminal)
            For Each key As String In nonTerminal.Firsts
              keyList.Remove(key)
            Next
          End If
        End If
      Next
      For Each terminal As Terminal In Data.Terminals
        If keyList.Contains(terminal.Key) Then
          bnfTermList.Add(terminal)
        End If
      Next
      Dim keyList2 As New KeyList()
      For Each bnfTerm As BnfTerm In bnfTermList
        keyList2.Add(If(String.IsNullOrEmpty(bnfTerm.DisplayName), bnfTerm.Name, bnfTerm.DisplayName))
      Next
      keyList2.Sort()
      Return keyList2
    End Function

    ' Token: 0x0600018E RID: 398 RVA: 0x00008618 File Offset: 0x00006818
    Private Function Recover() As Boolean
      If _currentToken.Category <> TokenCategory.[Error] Then
        _currentToken = Grammar.CreateSyntaxErrorToken(_context, _currentToken.Location, "Syntax error.", New Object(-1) {})
      End If
      Dim currentAction As ActionRecord = GetCurrentAction()
      If currentAction IsNot Nothing Then
        If currentAction.ActionType <> ParserActionType.Reduce Then
          GoTo IL_8D
        End If
      End If
      While Stack.Count > 0
        _currentState = Stack.Top.State
        Stack.Pop(1)
        currentAction = GetCurrentAction()
        If currentAction IsNot Nothing AndAlso currentAction.ActionType <> ParserActionType.Reduce Then
          Exit While
        End If
      End While
IL_8D:
      If currentAction Is Nothing OrElse currentAction.ActionType = ParserActionType.Reduce Then
        Return False
      End If
      ExecuteShiftAction(currentAction)
      While _currentToken.Terminal IsNot Grammar.Eof
        currentAction = GetCurrentAction()
        If currentAction Is Nothing Then
          NextToken()
        Else
          If currentAction.ActionType = ParserActionType.Reduce OrElse currentAction.ActionType = ParserActionType.[Operator] Then
            ExecuteReduceAction(currentAction)
            Return True
          End If
          ExecuteShiftAction(currentAction)
        End If
      End While
      Return False
    End Function

    ' Token: 0x0600018F RID: 399 RVA: 0x00008710 File Offset: 0x00006910
    Protected Sub OnActionSelected(input As Token, action As ActionRecord)
      Data.Grammar.OnActionSelected(Me, _currentToken, action)
      Dim e As New ParserActionEventArgs(CurrentState, input, action)
      RaiseEvent ActionSelected(Me, e)
    End Sub

    ' Token: 0x06000190 RID: 400 RVA: 0x00008758 File Offset: 0x00006958
    Private Function GetCurrentAction() As ActionRecord
      Dim result As ActionRecord = Nothing
      If _currentToken.MatchByValue Then
        Dim text As String = CurrentToken.Text
        If Not _caseSensitive Then
          text = text.ToLower()
        End If
        If _currentState.Actions.TryGetValue(text, result) Then
          Return result
        End If
      End If
      If _currentToken.MatchByType AndAlso _currentState.Actions.TryGetValue(_currentToken.Terminal.Key, result) Then
        Return result
      End If
      Return Nothing
    End Function

    ' Token: 0x06000191 RID: 401 RVA: 0x000087DC File Offset: 0x000069DC
    Private Function GetActionTypeForOperation(current As Token) As ParserActionType
      Dim terminal As Terminal = current.Terminal
      For i As Integer = Stack.Count - 2 To 0 Step -1
        If Stack(i).Node IsNot Nothing Then
          Dim term As BnfTerm = Stack(i).Node.Term
          If term.IsSet(TermOptions.IsOperator) Then
            Dim terminal2 As Terminal = TryCast(term, Terminal)
            If terminal2.Precedence <> terminal.Precedence Then
              Return If((terminal2.Precedence > terminal.Precedence), ParserActionType.Reduce, ParserActionType.Shift)
            End If
            If terminal.Associativity <> Associativity.Left Then
              Return ParserActionType.Shift
            End If
            Return ParserActionType.Reduce
          End If
        End If
      Next
      Return ParserActionType.Shift
    End Function

    ' Token: 0x06000192 RID: 402 RVA: 0x00008874 File Offset: 0x00006A74
    Private Sub ExecuteShiftAction(action As ActionRecord)
      Stack.Push(_currentToken, _currentState)
      _currentState = action.NewState
      NextToken()
    End Sub

    ' Token: 0x06000193 RID: 403 RVA: 0x000088A0 File Offset: 0x00006AA0
    Private Sub ExecuteReduceAction(action As ActionRecord)
      Dim currentState As ParserState = _currentState
      Dim popCount As Integer = action.PopCount
      Dim astNodeList As New AstNodeList()
      For i As Integer = 0 To action.PopCount - 1
        Dim node As AstNode = Stack(Stack.Count - popCount + i).Node
        astNodeList.Add(node)
      Next
      Dim sourceSpan As SourceSpan
      If popCount = 0 Then
        sourceSpan = New SourceSpan(_currentToken.Location, 0)
      Else
        Dim location As SourceLocation = Stack(Stack.Count - popCount).Node.Location
        Dim endPos As Integer = Stack(Stack.Count - 1).Node.Span.EndPos
        sourceSpan = New SourceSpan(location, endPos - location.Position)
        _currentState = Stack(Stack.Count - popCount).State
        Stack.Pop(popCount)
      End If
      Dim node2 As AstNode = CreateNode(action, sourceSpan, astNodeList)
      Stack.Push(node2, _currentState)
      Dim actionRecord As ActionRecord = Nothing
      If _currentState.Actions.TryGetValue(action.NonTerminal.Key, actionRecord) Then
        _currentState = actionRecord.NewState
        Return
      End If
      Throw New IronyException(String.Format("Cannot find transition for input {0}; state: {1}, popped state: {2}", action.NonTerminal, currentState, _currentState))
    End Sub

    ' Token: 0x06000194 RID: 404 RVA: 0x00008A14 File Offset: 0x00006C14
    Private Function CreateNode(reduceAction As ActionRecord, sourceSpan As SourceSpan, childNodes As AstNodeList) As AstNode
      Dim nonTerminal As NonTerminal = reduceAction.NonTerminal
      Dim astNode As AstNode = nonTerminal.OnNodeCreating(_context, _currentState, reduceAction, sourceSpan, childNodes)
      If astNode IsNot Nothing Then
        Return astNode
      End If
      Dim defaultNodeType As Type = _context.Compiler.Grammar.DefaultNodeType
      Dim type As Type = If(nonTerminal.NodeType, (If(defaultNodeType, GetType(AstNode))))
      Dim flag As Boolean = nonTerminal.IsSet(TermOptions.IsList)
      If flag AndAlso childNodes.Count > 1 AndAlso childNodes(0).Term Is nonTerminal Then
        astNode = childNodes(0)
        Dim astNode2 As AstNode = childNodes(childNodes.Count - 1)
        astNode2.Parent = astNode
        astNode.ChildNodes.Add(astNode2)
        Return astNode
      End If
      If Not flag AndAlso childNodes.Count = 1 AndAlso childNodes(0) IsNot Nothing Then
        Dim type2 As Type = If(childNodes(0).Term.NodeType, (If(defaultNodeType, GetType(AstNode))))
        If type2 Is type OrElse type2.IsSubclassOf(type) Then
          Return childNodes(0)
        End If
      End If
      astNode = Data.Grammar.CreateNode(_context, reduceAction, sourceSpan, childNodes)
      If astNode Is Nothing Then
        Dim astNodeArgs As New AstNodeArgs(nonTerminal, _context, sourceSpan, childNodes)
        If type Is GetType(AstNode) Then
          astNode = New AstNode(astNodeArgs)
        Else
          astNode = CType(Activator.CreateInstance(type, New Object() {astNodeArgs}), AstNode)
        End If
      End If
      If astNode IsNot Nothing Then
        nonTerminal.OnNodeCreated(astNode)
      End If
      Return astNode
    End Function

    ' Token: 0x040000F9 RID: 249
    Public Data As GrammarData

    ' Token: 0x040000FA RID: 250
    Public Stack As New ParserStack()

    ' Token: 0x040000FB RID: 251
    Private _context As CompilerContext

    ' Token: 0x040000FC RID: 252
    Private _caseSensitive As Boolean

    ' Token: 0x040000FD RID: 253
    Private _input As IEnumerator(Of Token)

    ' Token: 0x040000FE RID: 254
    Private _currentToken As Token

    ' Token: 0x040000FF RID: 255
    Private _currentState As ParserState

    ' Token: 0x04000100 RID: 256
    Private _currentLine As Integer

    ' Token: 0x04000101 RID: 257
    Private _tokenCount As Integer

    ' Token: 0x04000104 RID: 260
    Private _tokenArgs As New TokenEventArgs(Nothing)

    ' Token: 0x04000105 RID: 261
    Private _previewBuffer As New TokenList()
  End Class
End Namespace
