Namespace Irony.Compiler
  ' Token: 0x0200001C RID: 28
  Public Class StringLiteral
    Inherits CompoundTerminalBase

    ' Token: 0x06000074 RID: 116 RVA: 0x00003115 File Offset: 0x00001315
    Public Sub New(name As String, startEndSymbol As String, stringFlags As ScanFlags)
      Me.New(name, startEndSymbol, stringFlags, TermOptions.SpecialIgnoreCase)
    End Sub

    ' Token: 0x06000075 RID: 117 RVA: 0x00003125 File Offset: 0x00001325
    Public Sub New(name As String, startEndSymbol As String, stringFlags As ScanFlags, options As TermOptions)
      Me.New(name, options)
      StartEndSymbolTable.Add(startEndSymbol, stringFlags)
    End Sub

    ' Token: 0x06000076 RID: 118 RVA: 0x0000313D File Offset: 0x0000133D
    Public Sub New(name As String, options As TermOptions)
      MyBase.New(name)
      SetOption(options)
      Escapes = GetDefaultEscapes()
    End Sub

    ' Token: 0x06000077 RID: 119 RVA: 0x0000316E File Offset: 0x0000136E
    Public Sub AddStartEnd(startEndSymbol As String, stringFlags As ScanFlags)
      StartEndSymbolTable.Add(startEndSymbol, stringFlags)
    End Sub

    ' Token: 0x06000078 RID: 120 RVA: 0x00003180 File Offset: 0x00001380
    Public Overrides Sub Init(grammar As Grammar)
      MyBase.Init(grammar)
      _startEndSymbols.Clear()
      _startEndSymbols.AddRange(StartEndSymbolTable.Keys)
      _startEndSymbols.Sort(AddressOf KeyList.LongerFirst)
      _startEndFirsts = String.Empty
      For Each text As String In _startEndSymbols
        _startEndFirsts += text(0)
      Next
      If IsSet(TermOptions.SpecialIgnoreCase) Then
        _startEndFirsts = _startEndFirsts.ToLower() + _startEndFirsts.ToUpper()
      End If
    End Sub

    ' Token: 0x06000079 RID: 121 RVA: 0x00003264 File Offset: 0x00001464
    Public Overrides Function GetFirsts() As IList(Of String)
      Dim stringList As New StringList()
      stringList.AddRange(Prefixes)
      stringList.AddRange(_startEndSymbols)
      Return stringList
    End Function

    ' Token: 0x0600007A RID: 122 RVA: 0x00003290 File Offset: 0x00001490
    Protected Overrides Function ReadBody(source As ISourceStream, details As ScanDetails) As Boolean
      If Not ReadStartSymbol(source, details) Then
        Return False
      End If
      Dim flag As Boolean = Not details.IsSet(ScanFlags.HasDot)
      Dim ignoreCase As Boolean = IsSet(TermOptions.SpecialIgnoreCase)
      Dim position As Integer = source.Position
      Dim controlSymbol As String = details.ControlSymbol
      Dim symbol As String = controlSymbol + controlSymbol
      Dim num As Integer = If(details.IsSet(ScanFlags.AllowLineBreak), -1, source.Text.IndexOf(vbLf, source.Position))
      While Not source.EOF()
        Dim num2 As Integer = source.Text.IndexOf(controlSymbol, source.Position)
        Dim flag2 As Boolean = num2 < 0 OrElse (num >= 0 AndAlso num < num2)
        If flag2 Then
          If num > 0 Then
            num2 = num
          End If
          If num2 > 0 Then
            source.Position = num2 + 1
          End If
          details.[Error] = "Mal-formed  string literal - cannot find termination symbol."
          Return True
        End If
        If flag AndAlso source.Text(num2 - 1) = EscapeChar Then
          source.Position = num2 + controlSymbol.Length
        Else
          source.Position = num2
          If Not details.IsSet(ScanFlags.Octal) OrElse Not source.MatchSymbol(symbol, ignoreCase) Then
            details.Body = source.Text.Substring(position, num2 - position)
            source.Position = num2 + controlSymbol.Length
            Return True
          End If
          source.Position = num2 + controlSymbol.Length * 2
        End If
      End While
      Return False
    End Function

    ' Token: 0x0600007B RID: 123 RVA: 0x000033E0 File Offset: 0x000015E0
    Private Function ReadStartSymbol(source As ISourceStream, details As ScanDetails) As Boolean
      If _startEndFirsts.IndexOf(source.CurrentChar) < 0 Then
        Return False
      End If
      Dim ignoreCase As Boolean = IsSet(TermOptions.SpecialIgnoreCase)
      For Each text As String In _startEndSymbols
        If source.MatchSymbol(text, ignoreCase) Then
          details.ControlSymbol = text
          details.Flags = details.Flags Or StartEndSymbolTable(text)
          source.Position += text.Length
          Return True
        End If
      Next
      Return False
    End Function

    ' Token: 0x0600007C RID: 124 RVA: 0x00003494 File Offset: 0x00001694
    Protected Overrides Function ConvertValue(details As ScanDetails) As Boolean
      Dim text As String = details.Body
      Dim flag As Boolean = Not details.IsSet(ScanFlags.HasDot)
      If flag AndAlso text.Contains(EscapeChar) Then
        details.Flags = details.Flags Or ScanFlags.HasEscapes
        Dim array As String() = text.Split(New Char() {EscapeChar})
        Dim flag2 As Boolean = False
        For i As Integer = 1 To array.Length - 1
          If flag2 Then
            flag2 = False
          Else
            Dim text2 As String = array(i)
            If String.IsNullOrEmpty(text2) Then
              array(i) = "\"
              flag2 = True
            Else
              Dim key As Char = text2(0)
              Dim c As Char
              If Escapes.TryGetValue(key, c) Then
                array(i) = c + text2.Substring(1)
              Else
                array(i) = HandleSpecialEscape(array(i), details)
              End If
            End If
          End If
        Next
        text = String.Join(String.Empty, array)
      End If
      Dim controlSymbol As String = details.ControlSymbol
      If details.IsSet(ScanFlags.Octal) AndAlso text.Contains(controlSymbol, StringComparison.CurrentCulture) Then
        text = text.Replace(controlSymbol + controlSymbol, controlSymbol)
      End If
      If details.IsSet(ScanFlags.Binary) Then
        details.TypeCodes = New TypeCode() {TypeCode.Char}
      End If
      If details.TypeCodes(0) = TypeCode.Char AndAlso text.Length <> 1 Then
        details.[Error] = "Invalid length of char literal - should be 1."
        Return False
      End If
      details.Value = If((details.TypeCodes(0) = TypeCode.Char), text(0), text)
      Return True
    End Function

    ' Token: 0x0600007D RID: 125 RVA: 0x00003610 File Offset: 0x00001810
    Protected Overridable Function HandleSpecialEscape(segment As String, details As ScanDetails) As String
      If String.IsNullOrEmpty(segment) Then
        Return String.Empty
      End If
      Dim c As Char = segment(0)
      Dim c2 As Char = c
      If c2 <= "U"c Then
        Select Case c2
          Case "0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c
            If details.IsSet(ScanFlags.AllowOctalEscapes) Then
              Dim num As Integer = 0
              While num < 3 AndAlso num < segment.Length AndAlso "12345670".Contains(segment(num))
                num += 1
              End While
              Dim value As String = segment.Substring(0, num)
              Dim c3 As Char = ChrW(Convert.ToUInt32(value, 8))
              Return c3 + segment.Substring(num)
            End If
            GoTo IL_1D4
          Case Else
            If c2 <> "U"c Then
              GoTo IL_1D4
            End If
        End Select
      ElseIf c2 <> "u"c Then
        If c2 <> "x"c Then
          GoTo IL_1D4
        End If
        If Not details.IsSet(ScanFlags.AllowXEscapes) Then
          GoTo IL_1D4
        End If
        Dim num As Integer = 1
        While num < 5 AndAlso num < segment.Length AndAlso "1234567890aAbBcCdDeEfF".Contains(segment(num))
          num += 1
        End While
        If num <= 1 Then
          details.[Error] = "Invalid \x escape, at least one digit expected."
          Return segment
        End If
        Dim value As String = segment.Substring(1, num - 1)
        Dim c3 As Char = ChrW(Convert.ToUInt32(value, 16))
        Return c3 + segment.Substring(num)
      End If
      If details.IsSet(ScanFlags.HasExp) Then
        Dim num2 As Integer = If((c = "u"c), 4, 8)
        If segment.Length < num2 + 1 Then
          details.[Error] = String.Concat(New Object() {"Invalid unicode escape (", segment.Substring(num2 + 1), "), expected ", num2, " hex digits."})
          Return segment
        End If
        Dim value As String = segment.Substring(1, num2)
        Dim c3 As Char = ChrW(Convert.ToUInt32(value, 16))
        Return c3 + segment.Substring(num2 + 1)
      End If
IL_1D4:
      details.[Error] = "Invalid escape sequence: \" + segment
      Return segment
    End Function

    ' Token: 0x04000077 RID: 119
    Protected StartEndSymbolTable As New CompoundTerminalBase.ScanFlagTable()

    ' Token: 0x04000078 RID: 120
    Private _startEndFirsts As String

    ' Token: 0x04000079 RID: 121
    Private _startEndSymbols As New KeyList()
  End Class
End Namespace
