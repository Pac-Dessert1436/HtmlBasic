Imports System.Text

Namespace Irony.Compiler
  ' Token: 0x02000024 RID: 36
  Public Module TextUtils
    ' Token: 0x0600008C RID: 140 RVA: 0x00003B1C File Offset: 0x00001D1C
    Public Function GetDefaultEscapes() As EscapeTable
      Return New EscapeTable() From {{"a"c, ChrW(7)}, {"b"c, vbBack}, {"t"c, vbTab}, {"n"c, vbLf}, {"v"c, vbVerticalTab}, {"f"c, vbFormFeed}, {"r"c, vbCr}, {""""c, """"c}, {"'"c, "'"c}, {"\"c, "\"c}, {" "c, " "c}, {vbLf, vbLf}}
    End Function

    ' Token: 0x0600008D RID: 141 RVA: 0x00003BA8 File Offset: 0x00001DA8
    Public Function TerminalsToText(terminals As TerminalList) As String
      Dim stringBuilder As New StringBuilder()
      For Each terminal As Terminal In terminals
        stringBuilder.Append(terminal.ToString())
        stringBuilder.AppendLine()
      Next
      Return stringBuilder.ToString()
    End Function

    ' Token: 0x0600008E RID: 142 RVA: 0x00003C10 File Offset: 0x00001E10
    Public Function NonTerminalsToText(nonTerminals As NonTerminalList) As String
      Dim stringBuilder As New StringBuilder()
      For Each nonTerminal As NonTerminal In nonTerminals
        stringBuilder.Append(nonTerminal.Name)
        stringBuilder.Append(If(nonTerminal.Nullable, "  (Nullable) ", ""))
        stringBuilder.AppendLine()
        For Each production As Production In nonTerminal.Productions
          stringBuilder.Append("   ")
          stringBuilder.AppendLine(production.ToString())
        Next
        stringBuilder.Append("  FIRSTS: ")
        stringBuilder.AppendLine(nonTerminal.Firsts.ToString(" "))
        stringBuilder.AppendLine()
      Next
      Return stringBuilder.ToString()
    End Function

    ' Token: 0x0600008F RID: 143 RVA: 0x00003D18 File Offset: 0x00001F18
    Public Function StateListToText(states As ParserStateList) As String
      Dim stringBuilder As New StringBuilder()
      For Each parserState As ParserState In states
        stringBuilder.Append("State ")
        stringBuilder.AppendLine(parserState.Name)
        For Each lritem As LRItem In parserState.Items
          stringBuilder.Append("    ")
          stringBuilder.AppendLine(lritem.ToString())
        Next
        stringBuilder.Append("      TRANSITIONS: ")
        For Each text As String In parserState.Actions.Keys
          Dim actionRecord As ActionRecord = parserState.Actions(text)
          If actionRecord.NewState IsNot Nothing Then
            Dim value As String = If(text.EndsWith(vbBack), text.Substring(0, text.Length - 1), text)
            stringBuilder.Append(value)
            stringBuilder.Append("->")
            stringBuilder.Append(actionRecord.NewState.Name)
            stringBuilder.Append("; ")
          End If
        Next
        stringBuilder.AppendLine()
        stringBuilder.AppendLine()
      Next
      Return stringBuilder.ToString()
    End Function

    ' Token: 0x0400008C RID: 140
    Public Const AllLatinLetters As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"

    ' Token: 0x0400008D RID: 141
    Public Const DecimalDigits As String = "1234567890"

    ' Token: 0x0400008E RID: 142
    Public Const OctalDigits As String = "12345670"

    ' Token: 0x0400008F RID: 143
    Public Const HexDigits As String = "1234567890aAbBcCdDeEfF"

    ' Token: 0x04000090 RID: 144
    Public Const BinaryDigits As String = "01"
  End Module
End Namespace
