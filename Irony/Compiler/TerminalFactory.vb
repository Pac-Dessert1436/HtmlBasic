Imports System.Globalization

Namespace Irony.Compiler
  ' Token: 0x0200005C RID: 92
  Public Module TerminalFactory
    ' Token: 0x06000270 RID: 624 RVA: 0x0000C768 File Offset: 0x0000A968
    Public Function CreateCSharpString(name As String) As StringLiteral
      Dim stringLiteral As New StringLiteral(name, TermOptions.None)
      stringLiteral.AddStartEnd("""", ScanFlags.AllowAllEscapes)
      stringLiteral.AddPrefixFlag("@", CType(22, ScanFlags))
      Return stringLiteral
    End Function

    ' Token: 0x06000271 RID: 625 RVA: 0x0000C79C File Offset: 0x0000A99C
    Public Function CreateCSharpChar(name As String) As StringLiteral
      Dim stringLiteral As New StringLiteral(name, TermOptions.None)
      stringLiteral.AddStartEnd("'", ScanFlags.Binary)
      Return stringLiteral
    End Function

    ' Token: 0x06000272 RID: 626 RVA: 0x0000C7C0 File Offset: 0x0000A9C0
    Public Function CreateVbString(name As String) As StringLiteral
      Dim stringLiteral As New StringLiteral(name, TermOptions.SpecialIgnoreCase)
      stringLiteral.AddStartEnd("""", CType(18, ScanFlags))
      stringLiteral.AddSuffixCodes("$", New TypeCode() {TypeCode.[String]})
      stringLiteral.AddSuffixCodes("c", New TypeCode() {TypeCode.Char})
      Return stringLiteral
    End Function

    ' Token: 0x06000273 RID: 627 RVA: 0x0000C818 File Offset: 0x0000AA18
    Public Function CreatePythonString(name As String) As StringLiteral
      Dim stringLiteral As New StringLiteral(name, TermOptions.SpecialIgnoreCase)
      stringLiteral.AddStartEnd("'", ScanFlags.AllowAllEscapes)
      stringLiteral.AddStartEnd("'''", CType(228, ScanFlags))
      stringLiteral.AddStartEnd("""", ScanFlags.AllowAllEscapes)
      stringLiteral.AddStartEnd("""""""", CType(228, ScanFlags))
      stringLiteral.AddPrefixFlag("u", ScanFlags.AllowAllEscapes)
      stringLiteral.AddPrefixFlag("r", ScanFlags.HasDot)
      stringLiteral.AddPrefixFlag("ur", ScanFlags.HasDot)
      Return stringLiteral
    End Function

    ' Token: 0x06000274 RID: 628 RVA: 0x0000C89C File Offset: 0x0000AA9C
    Public Function CreateCSharpNumber(name As String) As NumberLiteral
      Dim numberLiteral As New NumberLiteral(name, CType(196608, TermOptions))
      numberLiteral.DefaultIntTypes = New TypeCode() {TypeCode.Int32, TypeCode.UInt32, TypeCode.Int64, TypeCode.UInt64}
      numberLiteral.DefaultFloatType = TypeCode.[Double]
      numberLiteral.AddPrefixFlag("0x", ScanFlags.Hex)
      numberLiteral.AddSuffixCodes("u", New TypeCode() {TypeCode.UInt32, TypeCode.UInt64})
      numberLiteral.AddSuffixCodes("l", New TypeCode() {TypeCode.Int64, TypeCode.UInt64})
      numberLiteral.AddSuffixCodes("ul", New TypeCode() {TypeCode.UInt64})
      numberLiteral.AddSuffixCodes("f", New TypeCode() {TypeCode.[Single]})
      numberLiteral.AddSuffixCodes("d", New TypeCode() {TypeCode.[Double]})
      numberLiteral.AddSuffixCodes("m", New TypeCode() {TypeCode.[Decimal]})
      Return numberLiteral
    End Function

    ' Token: 0x06000275 RID: 629 RVA: 0x0000C994 File Offset: 0x0000AB94
    Public Function CreateVbNumber(name As String) As NumberLiteral
      Dim numberLiteral As New NumberLiteral(name, CType(196608, TermOptions))
      numberLiteral.DefaultIntTypes = New TypeCode() {TypeCode.Int32, TypeCode.Int64}
      numberLiteral.AddPrefixFlag("&H", ScanFlags.Hex)
      numberLiteral.AddPrefixFlag("&O", ScanFlags.Octal)
      numberLiteral.AddSuffixCodes("S", New TypeCode() {TypeCode.Int16})
      numberLiteral.AddSuffixCodes("I", New TypeCode() {TypeCode.Int32})
      numberLiteral.AddSuffixCodes("%", New TypeCode() {TypeCode.Int32})
      numberLiteral.AddSuffixCodes("L", New TypeCode() {TypeCode.Int64})
      numberLiteral.AddSuffixCodes("&", New TypeCode() {TypeCode.Int64})
      numberLiteral.AddSuffixCodes("D", New TypeCode() {TypeCode.[Decimal]})
      numberLiteral.AddSuffixCodes("@", New TypeCode() {TypeCode.[Decimal]})
      numberLiteral.AddSuffixCodes("F", New TypeCode() {TypeCode.[Single]})
      numberLiteral.AddSuffixCodes("!", New TypeCode() {TypeCode.[Single]})
      numberLiteral.AddSuffixCodes("R", New TypeCode() {TypeCode.[Double]})
      numberLiteral.AddSuffixCodes("#", New TypeCode() {TypeCode.[Double]})
      numberLiteral.AddSuffixCodes("US", New TypeCode() {TypeCode.UInt16})
      numberLiteral.AddSuffixCodes("UI", New TypeCode() {TypeCode.UInt32})
      numberLiteral.AddSuffixCodes("UL", New TypeCode() {TypeCode.UInt64})
      Return numberLiteral
    End Function

    ' Token: 0x06000276 RID: 630 RVA: 0x0000CB50 File Offset: 0x0000AD50
    Public Function CreatePythonNumber(name As String) As NumberLiteral
      Dim numberLiteral As New NumberLiteral(name, CType(1245184, TermOptions))
      numberLiteral.DefaultIntTypes = New TypeCode() {TypeCode.Int32, TypeCode.Int64, CType(30, TypeCode)}
      numberLiteral.AddPrefixFlag("0x", ScanFlags.Hex)
      numberLiteral.AddPrefixFlag("0", ScanFlags.Octal)
      numberLiteral.AddSuffixCodes("L", New TypeCode() {TypeCode.Int64, CType(30, TypeCode)})
      numberLiteral.AddSuffixCodes("J", New TypeCode() {CType(31, TypeCode)})
      Return numberLiteral
    End Function

    ' Token: 0x06000277 RID: 631 RVA: 0x0000CBD4 File Offset: 0x0000ADD4
    Public Function CreateCSharpIdentifier(name As String) As IdentifierTerminal
      Dim identifierTerminal As New IdentifierTerminal(name)
      identifierTerminal.SetOption(TermOptions.CanStartWithEscape)
      Dim keywordList As String = "abstract as base bool break byte case catch char checked class" & vbTab & "const" & vbTab & "continue decimal default delegate  do double else enum event explicit extern false finally fixed float for foreach goto if implicit in int interface internal is lock long namespace new null object operator out override params private protected public readonly ref return sbyte sealed short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using virtual void volatile while"
      identifierTerminal.AddKeywordList(keywordList)
      identifierTerminal.AddPrefixFlag("@", CType(18, ScanFlags))
      identifierTerminal.StartCharCategories.AddRange(New UnicodeCategory() {UnicodeCategory.UppercaseLetter, UnicodeCategory.LowercaseLetter, UnicodeCategory.TitlecaseLetter, UnicodeCategory.ModifierLetter, UnicodeCategory.OtherLetter, UnicodeCategory.LetterNumber})
      identifierTerminal.CharCategories.AddRange(identifierTerminal.StartCharCategories)
      identifierTerminal.CharCategories.AddRange(New UnicodeCategory() {UnicodeCategory.DecimalDigitNumber, UnicodeCategory.ConnectorPunctuation, UnicodeCategory.SpacingCombiningMark, UnicodeCategory.NonSpacingMark, UnicodeCategory.Format})
      identifierTerminal.CharsToRemoveCategories.Add(UnicodeCategory.Format)
      Return identifierTerminal
    End Function

    ' Token: 0x06000278 RID: 632 RVA: 0x0000CC80 File Offset: 0x0000AE80
    Public Function CreatePythonIdentifier(name As String) As IdentifierTerminal
      Dim identifierTerminal As New IdentifierTerminal("Identifier")
      identifierTerminal.AddKeywords(New String() {"and", "del", "from", "not", "while", "as", "elif", "global", "or", "with", "assert", "else", "if", "pass", "yield", "break", "except", "import", "print", "class", "exec", "in", "raise", "continue", "finally", "is", "return", "def", "for", "lambda", "try"})
      Return identifierTerminal
    End Function
  End Module
End Namespace
