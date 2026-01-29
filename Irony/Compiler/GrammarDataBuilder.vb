Imports System.Text

Namespace Irony.Compiler
  ' Token: 0x02000045 RID: 69
  Public Class GrammarDataBuilder
    ' Token: 0x0600014A RID: 330 RVA: 0x00006025 File Offset: 0x00004225
    Public Sub New(grammar As Grammar)
      _grammar = grammar
      Data = New GrammarData()
      Data.Grammar = _grammar
    End Sub

    ' Token: 0x0600014B RID: 331 RVA: 0x00006050 File Offset: 0x00004250
    Public Sub Build()
      Try
        Data.ScannerRecoverySymbols = _grammar.WhitespaceChars + _grammar.Delimiters
        If _grammar.Root Is Nothing Then
          Cancel("Root property of the grammar is not set.")
        End If
        Data.AugmentedRoot = New NonTerminal(_grammar.Root.Name + "'", New BnfExpression(_grammar.Root))
        CollectAllElements()
        If Not _grammar.CaseSensitive Then
          AdjustCaseForSymbols()
        End If
        CreateProductions()
        CalculateNullability()
        CalculateFirsts()
        CalculateTailFirsts()
        CreateParserStates()
        PropagateLookaheads()
        CreateReduceActions()
        CheckActionConflicts()
        InitAll()
        BuildTerminalsLookupTable()
        ValidateAll()
      Catch ex As GrammarErrorException
        Data.Errors.Add(ex.Message)
        Data.AnalysisCanceled = True
      End Try
    End Sub

    ' Token: 0x0600014C RID: 332 RVA: 0x00006168 File Offset: 0x00004368
    Private Shared Sub Cancel(msg As String)
      If msg Is Nothing Then
        msg = "Grammar analysis canceled."
      End If
      Throw New GrammarErrorException(msg)
    End Sub

    ' Token: 0x0600014D RID: 333 RVA: 0x0000617C File Offset: 0x0000437C
    Private Sub CollectAllElements()
      Data.NonTerminals.Clear()
      Data.Terminals.Clear()
      For Each terminal As Terminal In _grammar.NonGrammarTerminals
        terminal.SetOption(TermOptions.IsNonGrammar)
        Data.Terminals.Add(terminal)
      Next
      _unnamedCount = 0
      CollectAllElementsRecursive(Data.AugmentedRoot)
      Data.Terminals.Sort(AddressOf Terminal.ByName)
      If Data.AnalysisCanceled Then
        Cancel(Nothing)
      End If
    End Sub

    ' Token: 0x0600014E RID: 334 RVA: 0x00006254 File Offset: 0x00004454
    Private Sub CollectAllElementsRecursive(element As BnfTerm)
      Dim terminal As Terminal = TryCast(element, Terminal)
      If terminal IsNot Nothing AndAlso Not Data.Terminals.Contains(terminal) AndAlso terminal.[GetType]() IsNot GetType(Terminal) Then
        Data.Terminals.Add(terminal)
        Return
      End If
      Dim nonTerminal As NonTerminal = TryCast(element, NonTerminal)
      If nonTerminal Is Nothing OrElse Data.NonTerminals.Contains(nonTerminal) Then
        Return
      End If
      If nonTerminal.Name Is Nothing Then
        If nonTerminal.Rule IsNot Nothing AndAlso Not String.IsNullOrEmpty(nonTerminal.Rule.Name) Then
          nonTerminal.Name = nonTerminal.Rule.Name
        Else
          Dim bnfTerm As BnfTerm = nonTerminal
          Dim arg As Object = "NT"
          Dim unnamedCount As Integer = _unnamedCount
          Dim num As Integer = unnamedCount
          _unnamedCount = unnamedCount + 1
          bnfTerm.Name = arg + num
        End If
      End If
      Data.NonTerminals.Add(nonTerminal)
      If nonTerminal.Rule Is Nothing Then
        AddError("Non-terminal {0} has uninitialized Rule property.", New Object() {nonTerminal.Name})
        Data.AnalysisCanceled = True
        Return
      End If
      For Each bnfTermList As BnfTermList In nonTerminal.Rule.Data
        For i As Integer = 0 To bnfTermList.Count - 1
          Dim bnfTerm2 As BnfTerm = bnfTermList(i)
          If bnfTerm2 Is Nothing Then
            AddError("Rule for NonTerminal {0} contains null as an operand in position {1} in one of productions.", New Object() {nonTerminal, i})
          Else
            Dim bnfExpression As BnfExpression = TryCast(bnfTerm2, BnfExpression)
            If bnfExpression IsNot Nothing Then
              bnfTerm2 = New NonTerminal(Nothing, bnfExpression)
              bnfTermList(i) = bnfTerm2
            End If
            CollectAllElementsRecursive(bnfTerm2)
          End If
        Next
      Next
    End Sub

    ' Token: 0x0600014F RID: 335 RVA: 0x00006414 File Offset: 0x00004614
    Private Sub AdjustCaseForSymbols()
      If _grammar.CaseSensitive Then
        Return
      End If
      For Each terminal As Terminal In Data.Terminals
        If TypeOf terminal Is SymbolTerminal Then
          terminal.Key = terminal.Key.ToLower()
        End If
      Next
    End Sub

    ' Token: 0x06000150 RID: 336 RVA: 0x0000648C File Offset: 0x0000468C
    Private Sub BuildTerminalsLookupTable()
      Data.TerminalsLookup.Clear()
      Data.FallbackTerminals.AddRange(Data.Grammar.FallbackTerminals)
      For Each terminal As Terminal In Data.Terminals
        Dim firsts As IList(Of String) = terminal.GetFirsts()
        If firsts Is Nothing OrElse firsts.Count = 0 Then
          If Not Data.FallbackTerminals.Contains(terminal) Then
            Data.FallbackTerminals.Add(terminal)
          End If
        Else
          For Each text As String In firsts
            If Not String.IsNullOrEmpty(text) Then
              Dim c As Char = text(0)
              If Not _grammar.CaseSensitive Then
                c = Char.ToLower(c)
              End If
              Dim terminalList As TerminalList = Nothing
              If Not Data.TerminalsLookup.TryGetValue(c, terminalList) Then
                terminalList = New TerminalList()
                Data.TerminalsLookup(c) = terminalList
              End If
              terminalList.Add(terminal)
            End If
          Next
        End If
      Next
      For Each terminalList2 As TerminalList In Data.TerminalsLookup.Values
        If terminalList2.Count > 1 Then
          terminalList2.Sort(AddressOf Terminal.ByPriorityReverse)
        End If
      Next
    End Sub

    ' Token: 0x06000151 RID: 337 RVA: 0x00006648 File Offset: 0x00004848
    Private Sub CreateProductions()
      Data.Productions.Clear()
      LR0Item._maxID = 0
      For Each nonTerminal As NonTerminal In Data.NonTerminals
        nonTerminal.Productions.Clear()
        Dim bnfExpressionData As New BnfExpressionData()
        bnfExpressionData.AddRange(nonTerminal.Rule.Data)
        If nonTerminal.ErrorRule IsNot Nothing Then
          bnfExpressionData.AddRange(nonTerminal.ErrorRule.Data)
        End If
        For Each rvalues As BnfTermList In bnfExpressionData
          Dim isInitial As Boolean = nonTerminal Is Data.AugmentedRoot
          Dim item As New Production(isInitial, nonTerminal, rvalues)
          nonTerminal.Productions.Add(item)
          Data.Productions.Add(item)
        Next
      Next
    End Sub

    ' Token: 0x06000152 RID: 338 RVA: 0x00006760 File Offset: 0x00004960
    Private Sub CalculateNullability()
      Dim nonTerminalList As NonTerminalList = Data.NonTerminals
      While nonTerminalList.Count > 0
        Dim nonTerminalList2 As New NonTerminalList()
        For Each nonTerminal As NonTerminal In nonTerminalList
          If Not CalculateNullability(nonTerminal, nonTerminalList) Then
            nonTerminalList2.Add(nonTerminal)
          End If
        Next
        If nonTerminalList.Count = nonTerminalList2.Count Then
          Return
        End If
        nonTerminalList = nonTerminalList2
      End While
    End Sub

    ' Token: 0x06000153 RID: 339 RVA: 0x000067E8 File Offset: 0x000049E8
    Private Shared Function CalculateNullability(nonTerminal As NonTerminal, undecided As NonTerminalList) As Boolean
      For Each production As Production In nonTerminal.Productions
        If Not production.HasTerminals Then
          If production.IsEmpty() Then
            nonTerminal.Nullable = True
            Return True
          End If
          Dim flag As Boolean = True
          For Each bnfTerm As BnfTerm In production.RValues
            Dim nonTerminal2 As NonTerminal = TryCast(bnfTerm, NonTerminal)
            If nonTerminal2 IsNot Nothing Then
              flag = flag And nonTerminal2.Nullable
            End If
          Next
          If flag Then
            nonTerminal.Nullable = True
            Return True
          End If
        End If
      Next
      Return False
    End Function

    ' Token: 0x06000154 RID: 340 RVA: 0x000068B8 File Offset: 0x00004AB8
    Private Sub CalculateFirsts()
      For Each production As Production In Data.Productions
        For Each bnfTerm As BnfTerm In production.RValues
          If TypeOf bnfTerm Is Terminal Then
            production.LValue.Firsts.Add(bnfTerm.Key)
            Exit For
          End If
          Dim nonTerminal As NonTerminal = TryCast(bnfTerm, NonTerminal)
          If Not nonTerminal.PropagateFirstsTo.Contains(production.LValue) Then
            nonTerminal.PropagateFirstsTo.Add(production.LValue)
          End If
          If Not nonTerminal.Nullable Then
            Exit For
          End If
        Next
      Next
      Dim nonTerminalList As NonTerminalList = Data.NonTerminals
      While nonTerminalList.Count > 0
        Dim nonTerminalList2 As New NonTerminalList()
        For Each nonTerminal2 As NonTerminal In nonTerminalList
          For Each nonTerminal3 As NonTerminal In nonTerminal2.PropagateFirstsTo
            For Each key As String In nonTerminal2.Firsts
              If Not nonTerminal3.Firsts.Contains(key) Then
                nonTerminal3.Firsts.Add(key)
                If Not nonTerminalList2.Contains(nonTerminal3) Then
                  nonTerminalList2.Add(nonTerminal3)
                End If
              End If
            Next
          Next
        Next
        nonTerminalList = nonTerminalList2
      End While
    End Sub

    ' Token: 0x06000155 RID: 341 RVA: 0x00006AB4 File Offset: 0x00004CB4
    Private Sub CalculateTailFirsts()
      For Each production As Production In Data.Productions
        Dim keyList As New KeyList()
        Dim tailIsNullable As Boolean = True
        For i As Integer = production.LR0Items.Count - 1 To 0 Step -1
          Dim lr0Item As LR0Item = production.LR0Items(i)
          If i >= production.LR0Items.Count - 2 Then
            lr0Item.TailIsNullable = True
            lr0Item.TailFirsts.Clear()
          Else
            Dim bnfTerm As BnfTerm = production.RValues(lr0Item.Position + 1)
            Dim nonTerminal As NonTerminal = TryCast(bnfTerm, NonTerminal)
            If nonTerminal Is Nothing OrElse Not nonTerminal.Nullable Then
              keyList.Clear()
              tailIsNullable = False
              lr0Item.TailIsNullable = False
              If nonTerminal Is Nothing Then
                lr0Item.TailFirsts.Add(bnfTerm.Key)
                keyList.Add(bnfTerm.Key)
              Else
                lr0Item.TailFirsts.AddRange(nonTerminal.Firsts)
                keyList.AddRange(nonTerminal.Firsts)
              End If
            Else
              keyList.AddRange(nonTerminal.Firsts)
              lr0Item.TailFirsts.AddRange(keyList)
              lr0Item.TailIsNullable = tailIsNullable
            End If
          End If
        Next
      Next
    End Sub

    ' Token: 0x06000156 RID: 342 RVA: 0x00006C1C File Offset: 0x00004E1C
    Private Sub CreateInitialAndFinalStates()
      Dim lr0ItemList As New LR0ItemList()
      lr0ItemList.Add(Data.AugmentedRoot.Productions(0).LR0Items(0))
      Data.InitialState = FindOrCreateState(lr0ItemList)
      Data.InitialState.Items(0).NewLookaheads.Add(Grammar.Eof.Key)
      lr0ItemList.Clear()
      lr0ItemList.Add(Data.AugmentedRoot.Productions(0).LR0Items(1))
      Data.FinalState = FindOrCreateState(lr0ItemList)
      Data.InitialState.Actions(Data.AugmentedRoot.Key) = New ActionRecord(Data.AugmentedRoot.Key, ParserActionType.Shift, Data.FinalState, Nothing)
    End Sub

    ' Token: 0x06000157 RID: 343 RVA: 0x00006D18 File Offset: 0x00004F18
    Private Sub CreateParserStates()
      Data.States.Clear()
      _stateHash = New ParserStateTable()
      CreateInitialAndFinalStates()
      Dim key As String = Data.AugmentedRoot.Key
      For i As Integer = 0 To Data.States.Count - 1
        Dim parserState As ParserState = Data.States(i)
        AddClosureItems(parserState)
        Dim stateShifts As GrammarDataBuilder.ShiftTable = GetStateShifts(parserState)
        For Each key2 As String In stateShifts.Keys
          Dim lr0ItemList As LR0ItemList = stateShifts(key2)
          Dim parserState2 As ParserState = FindOrCreateState(lr0ItemList)
          parserState.Actions(key2) = New ActionRecord(key2, ParserActionType.Shift, parserState2, Nothing)
          For Each lr0Item As LR0Item In lr0ItemList
            Dim lritem As LRItem = FindItem(parserState, lr0Item.Production, lr0Item.Position - 1)
            Dim item As LRItem = FindItem(parserState2, lr0Item.Production, lr0Item.Position)
            If Not lritem.PropagateTargets.Contains(item) Then
              lritem.PropagateTargets.Add(item)
            End If
          Next
        Next
      Next
      Data.FinalState = Data.InitialState.Actions(Data.AugmentedRoot.Key).NewState
    End Sub

    ' Token: 0x06000158 RID: 344 RVA: 0x00006EC8 File Offset: 0x000050C8
    Private Function AdjustCase(key As String) As String
      If Not _grammar.CaseSensitive Then
        Return key.ToLower()
      End If
      Return key
    End Function

    ' Token: 0x06000159 RID: 345 RVA: 0x00006EE0 File Offset: 0x000050E0
    Private Shared Function TryFindItem(state As ParserState, core As LR0Item) As LRItem
      For Each lritem As LRItem In state.Items
        If lritem.Core Is core Then
          Return lritem
        End If
      Next
      Return Nothing
    End Function

    ' Token: 0x0600015A RID: 346 RVA: 0x00006F3C File Offset: 0x0000513C
    Private Shared Function FindItem(state As ParserState, production As Production, position As Integer) As LRItem
      For Each lritem As LRItem In state.Items
        If lritem.Core.Production Is production AndAlso lritem.Core.Position = position Then
          Return lritem
        End If
      Next
      Dim message As String = String.Format("Failed to find an LRItem in state {0} by production [{1}] and position {2}. ", state, production.ToString(), position.ToString())
      Throw New IronyException(message)
    End Function

    ' Token: 0x0600015B RID: 347 RVA: 0x00006FCC File Offset: 0x000051CC
    Private Shared Function GetStateShifts(state As ParserState) As GrammarDataBuilder.ShiftTable
      Dim shiftTable As New GrammarDataBuilder.ShiftTable()
      For Each lritem As LRItem In state.Items
        Dim nextElement As BnfTerm = lritem.Core.NextElement
        If nextElement IsNot Nothing Then
          Dim item As LR0Item = lritem.Core.Production.LR0Items(lritem.Core.Position + 1)
          Dim lr0ItemList As LR0ItemList = Nothing
          If Not shiftTable.TryGetValue(nextElement.Key, lr0ItemList) Then
            Dim dictionary As Dictionary(Of String, LR0ItemList) = shiftTable
            Dim key As String = nextElement.Key
            Dim lr0ItemList2 As New LR0ItemList()
            lr0ItemList = lr0ItemList2
            dictionary(key) = lr0ItemList2
          End If
          lr0ItemList.Add(item)
        End If
      Next
      Return shiftTable
    End Function

    ' Token: 0x0600015C RID: 348 RVA: 0x00007080 File Offset: 0x00005280
    Private Function FindOrCreateState(lr0Items As LR0ItemList) As ParserState
      Dim key As String = CalcItemListKey(lr0Items)
      Dim parserState As ParserState = Nothing
      If _stateHash.TryGetValue(key, parserState) Then
        Return parserState
      End If
      parserState = New ParserState("S" + Data.States.Count, lr0Items)
      Data.States.Add(parserState)
      _stateHash(key) = parserState
      Return parserState
    End Function

    ' Token: 0x0600015D RID: 349 RVA: 0x000070EC File Offset: 0x000052EC
    Private Function AddClosureItems(state As ParserState) As Boolean
      Dim result As Boolean = False
      For i As Integer = 0 To state.Items.Count - 1
        Dim lritem As LRItem = state.Items(i)
        Dim nonTerminal As NonTerminal = TryCast(lritem.Core.NextElement, NonTerminal)
        If nonTerminal IsNot Nothing Then
          For Each production As Production In nonTerminal.Productions
            Dim core As LR0Item = production.LR0Items(0)
            Dim lritem2 As LRItem = TryFindItem(state, core)
            If lritem2 Is Nothing Then
              lritem2 = New LRItem(state, core)
              state.Items.Add(lritem2)
              result = True
            End If
            lritem2.NewLookaheads.AddRange(lritem.Core.TailFirsts)
            If lritem.Core.TailIsNullable AndAlso Not lritem.PropagateTargets.Contains(lritem2) Then
              lritem.PropagateTargets.Add(lritem2)
            End If
          Next
        End If
      Next
      Return result
    End Function

    ' Token: 0x0600015E RID: 350 RVA: 0x000071FC File Offset: 0x000053FC
    Private Shared Function CalcItemListKey(items As LR0ItemList) As String
      items.Sort(AddressOf ById)
      If items.Count = 0 Then
        Return ""
      End If
      If items.Count = 1 AndAlso items(0).IsKernel Then
        Return items(0).ID.ToString()
      End If
      Dim stringBuilder As New StringBuilder(1024)
      For Each lr0Item As LR0Item In items
        If lr0Item.IsKernel Then
          stringBuilder.Append(lr0Item.ID)
          stringBuilder.Append(",")
        End If
      Next
      Return stringBuilder.ToString()
    End Function

    ' Token: 0x0600015F RID: 351 RVA: 0x000072C0 File Offset: 0x000054C0
    Private Shared Function ById(x As LR0Item, y As LR0Item) As Integer
      If x.ID < y.ID Then
        Return -1
      End If
      If x.ID = y.ID Then
        Return 0
      End If
      Return 1
    End Function

    ' Token: 0x06000160 RID: 352 RVA: 0x000072E4 File Offset: 0x000054E4
    Private Sub PropagateLookaheads()
      Dim lritemList As New LRItemList()
      Dim enumerator As List(Of ParserState).Enumerator = Data.States.GetEnumerator()
      Using enumerator
        While enumerator.MoveNext()
          Dim parserState As ParserState = enumerator.Current
          lritemList.AddRange(parserState.Items)
        End While
        GoTo IL_116
      End Using
IL_4A:
      Dim lritemList2 As New LRItemList()
      For Each lritem As LRItem In lritemList
        If lritem.NewLookaheads.Count <> 0 Then
          Dim count As Integer = lritem.Lookaheads.Count
          lritem.Lookaheads.AddRange(lritem.NewLookaheads)
          If lritem.Lookaheads.Count <> count Then
            For Each lritem2 As LRItem In lritem.PropagateTargets
              lritem2.NewLookaheads.AddRange(lritem.NewLookaheads)
              lritemList2.Add(lritem2)
            Next
          End If
          lritem.NewLookaheads.Clear()
        End If
      Next
      lritemList = lritemList2
IL_116:
      If lritemList.Count <= 0 Then
        Return
      End If
      GoTo IL_4A
    End Sub

    ' Token: 0x06000161 RID: 353 RVA: 0x0000743C File Offset: 0x0000563C
    Private Sub CreateReduceActions()
      For Each parserState As ParserState In Data.States
        For Each lritem As LRItem In parserState.Items
          If lritem.Core.NextElement Is Nothing Then
            For Each key As String In lritem.Lookaheads
              Dim actionRecord As ActionRecord = Nothing
              If parserState.Actions.TryGetValue(key, actionRecord) Then
                actionRecord.ReduceProductions.Add(lritem.Core.Production)
              Else
                parserState.Actions(key) = New ActionRecord(key, ParserActionType.Reduce, Nothing, lritem.Core.Production)
              End If
            Next
          End If
        Next
      Next
    End Sub

    ' Token: 0x06000162 RID: 354 RVA: 0x00007568 File Offset: 0x00005768
    Private Sub CheckActionConflicts()
      Dim stringDictionary As New StringDictionary()
      For Each parserState As ParserState In Data.States
        For Each actionRecord As ActionRecord In parserState.Actions.Values
          If actionRecord.NewState Is Nothing OrElse actionRecord.ReduceProductions.Count <> 0 Then
            If actionRecord.NewState Is Nothing AndAlso actionRecord.ReduceProductions.Count = 1 Then
              actionRecord.ActionType = ParserActionType.Reduce
            Else
              If actionRecord.NewState IsNot Nothing AndAlso actionRecord.ReduceProductions.Count > 0 Then
                Dim symbol As SymbolTerminal = SymbolTerminal.GetSymbol(actionRecord.Key)
                If symbol IsNot Nothing AndAlso symbol.IsSet(TermOptions.IsOperator) Then
                  actionRecord.ActionType = ParserActionType.[Operator]
                Else
                  AddErrorForInput(stringDictionary, actionRecord.Key, "Shift-reduce conflict in state {0}, reduce production: {1}", New Object() {parserState, actionRecord.ReduceProductions(0)})
                End If
              End If
              If actionRecord.ReduceProductions.Count > 1 Then
                AddErrorForInput(stringDictionary, actionRecord.Key, "Reduce-reduce conflict in state {0} in productions: {1} ; {2}", New Object() {parserState, actionRecord.ReduceProductions(0), actionRecord.ReduceProductions(1)})
              End If
            End If
          End If
        Next
      Next
      For Each text As String In stringDictionary.Keys
        Data.Errors.Add(text + " on inputs: " + stringDictionary(text))
      Next
    End Sub

    ' Token: 0x06000163 RID: 355 RVA: 0x00007784 File Offset: 0x00005984
    Private Shared Sub AddErrorForInput(errors As StringDictionary, input As String, template As String, ParamArray args As Object())
      Dim key As String = String.Format(template, args)
      Dim str As String = Nothing
      errors.TryGetValue(key, str)
      errors(key) = str + input + " "
    End Sub

    ' Token: 0x06000164 RID: 356 RVA: 0x000077B8 File Offset: 0x000059B8
    Private Shared Function ContainsProduction(productions As ProductionList, nonTerminal As NonTerminal) As Boolean
      For Each production As Production In productions
        If production.LValue Is nonTerminal Then
          Return True
        End If
      Next
      Return False
    End Function

    ' Token: 0x06000165 RID: 357 RVA: 0x00007810 File Offset: 0x00005A10
    Private Sub InitAll()
      For Each terminal As Terminal In Data.Terminals
        terminal.Init(_grammar)
      Next
      For Each nonTerminal As NonTerminal In Data.NonTerminals
        nonTerminal.Init(_grammar)
      Next
      For Each tokenFilter As TokenFilter In _grammar.TokenFilters
        tokenFilter.Init(_grammar)
      Next
    End Sub

    ' Token: 0x06000166 RID: 358 RVA: 0x00007908 File Offset: 0x00005B08
    Private Sub ValidateAll()
      Dim keyList As New KeyList()
      For Each nonTerminal As NonTerminal In Data.NonTerminals
        If nonTerminal IsNot Data.AugmentedRoot Then
          Dim data As BnfExpressionData = nonTerminal.Rule.Data
          If data.Count = 1 AndAlso data(0).Count = 1 AndAlso TypeOf data(0)(0) Is NonTerminal Then
            keyList.Add(nonTerminal.Name)
          End If
        End If
      Next
      If keyList.Count > 0 Then
        AddError("Warning: Possible non-terminal duplication. The following non-terminals have rules containing a single non-terminal: " & vbCrLf & " {0}. " & vbCrLf & "Consider merging two non-terminals; you may need to use 'nt1 = nt2;' instead of 'nt1.Rule=nt2'.", New Object() {keyList.ToString(", ")})
      End If
    End Sub

    ' Token: 0x06000167 RID: 359 RVA: 0x000079E0 File Offset: 0x00005BE0
    Private Sub AddError(message As String, ParamArray args As Object())
      If args IsNot Nothing AndAlso args.Length > 0 Then
        message = String.Format(message, args)
      End If
      Data.Errors.Add(message)
    End Sub

    ' Token: 0x040000E5 RID: 229
    Private _stateHash As ParserStateTable

    ' Token: 0x040000E6 RID: 230
    Public Data As GrammarData

    ' Token: 0x040000E7 RID: 231
    Private _grammar As Grammar

    ' Token: 0x040000E8 RID: 232
    Private _unnamedCount As Integer

    ' Token: 0x02000046 RID: 70
    Private Class ShiftTable
      Inherits Dictionary(Of String, LR0ItemList)

    End Class
  End Class
End Namespace
