Imports System.Globalization

Namespace Irony.Compiler
  ' Token: 0x02000049 RID: 73
  Public Class IdentifierTerminal
    Inherits CompoundTerminalBase

    ' Token: 0x0600016B RID: 363 RVA: 0x00007A20 File Offset: 0x00005C20
    Public Sub New(name As String, extraChars As String, extraFirstChars As String)
      MyBase.New(name)
      AllFirstChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz" + extraFirstChars
      AllChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890" + extraChars
      MatchMode = TokenMatchMode.ByValueThenByType
    End Sub

    ' Token: 0x0600016C RID: 364 RVA: 0x00007A89 File Offset: 0x00005C89
    Public Sub New(name As String)
      Me.New(name, "_", "_")
    End Sub

    ' Token: 0x0600016D RID: 365 RVA: 0x00007A9C File Offset: 0x00005C9C
    Public Sub AddKeywords(ParamArray keywords As String())
      Me.Keywords.AddRange(keywords)
    End Sub

    ' Token: 0x0600016E RID: 366 RVA: 0x00007AAC File Offset: 0x00005CAC
    Public Sub AddKeywordList(keywordList As String)
      Dim array As String() = keywordList.Split(New Char() {" "c, ","c, ";"c, vbLf, vbCr, vbTab})
      For Each text As String In array
        Dim text2 As String = text.Trim()
        If Not String.IsNullOrEmpty(text2) Then
          Keywords.Add(text2)
        End If
      Next
    End Sub

    ' Token: 0x0600016F RID: 367 RVA: 0x00007B20 File Offset: 0x00005D20
    Public Overrides Sub Init(grammar As Grammar)
      MyBase.Init(grammar)
      _terminators = grammar.WhitespaceChars + grammar.Delimiters
      _keywordHash = New StringDictionary()
      For Each text As String In Keywords
        If grammar.CaseSensitive Then
          _keywordHash.Add(text, String.Empty)
        Else
          _keywordHash.Add(text.ToLower(), String.Empty)
        End If
      Next
      If StartCharCategories.Count > 0 AndAlso Not grammar.FallbackTerminals.Contains(Me) Then
        grammar.FallbackTerminals.Add(Me)
      End If
    End Sub

    ' Token: 0x06000170 RID: 368 RVA: 0x00007BF0 File Offset: 0x00005DF0
    Protected Overrides Function CreateToken(context As CompilerContext, source As ISourceStream, details As ScanDetails) As Token
      If details.IsSet(ScanFlags.Binary) AndAlso Not String.IsNullOrEmpty(details.Prefix) Then
        details.Value = details.Prefix + details.Body
      End If
      Dim token As Token = MyBase.CreateToken(context, source, details)
      If details.IsSet(ScanFlags.Octal) Then
        Return token
      End If
      Dim text As String = token.Text
      If Not Grammar.CaseSensitive Then
        text = text.ToLower()
      End If
      If _keywordHash.ContainsKey(text) Then
        token.IsKeyword = True
      End If
      Return token
    End Function

    ' Token: 0x06000171 RID: 369 RVA: 0x00007C70 File Offset: 0x00005E70
    Protected Overrides Function QuickParse(context As CompilerContext, source As ISourceStream) As Token
      If AllFirstChars.IndexOf(source.CurrentChar) < 0 Then
        Return Nothing
      End If
      source.Position += 1
      While AllChars.Contains(source.CurrentChar) AndAlso Not source.EOF()
        source.Position += 1
      End While
      If _terminators.IndexOf(source.CurrentChar) < 0 Then
        Return Nothing
      End If
      Dim lexeme As String = source.GetLexeme()
      Return Token.Create(Me, context, source.TokenStart, lexeme)
    End Function

    ' Token: 0x06000172 RID: 370 RVA: 0x00007CF8 File Offset: 0x00005EF8
    Protected Overrides Function ReadBody(source As ISourceStream, details As ScanDetails) As Boolean
      Dim position As Integer = source.Position
      Dim flag As Boolean = Not details.IsSet(ScanFlags.HasDot)
      Dim charList As New CharList()
      While Not source.EOF()
        Dim c As Char = source.CurrentChar
        If _terminators.Contains(c) Then
          Exit While
        End If
        If flag AndAlso c = EscapeChar Then
          c = ReadUnicodeEscape(source, details)
          source.Position -= 1
          If details.HasError() Then
            Return False
          End If
        End If
        If Not CharOk(c, source.Position = position) Then
          Exit While
        End If
        Dim unicodeCategory As UnicodeCategory = Char.GetUnicodeCategory(c)
        If Not CharsToRemoveCategories.Contains(unicodeCategory) Then
          charList.Add(c)
        End If
        source.Position += 1
      End While
      If charList.Count = 0 Then
        Return False
      End If
      details.Body = New String(charList.ToArray())
      Return Not String.IsNullOrEmpty(details.Body)
    End Function

    ' Token: 0x06000173 RID: 371 RVA: 0x00007DD8 File Offset: 0x00005FD8
    Private Function CharOk(ch As Char, first As Boolean) As Boolean
      Dim text As String = If(first, AllFirstChars, AllChars)
      If text.Contains(ch) Then
        Return True
      End If
      Dim unicodeCategory As UnicodeCategory = Char.GetUnicodeCategory(ch)
      Dim unicodeCategoryList As UnicodeCategoryList = If(first, StartCharCategories, CharCategories)
      Return unicodeCategoryList.Contains(unicodeCategory)
    End Function

    ' Token: 0x06000174 RID: 372 RVA: 0x00007E28 File Offset: 0x00006028
    Private Shared Function ReadUnicodeEscape(source As ISourceStream, details As ScanDetails) As Char
      source.Position += 1
      Dim currentChar As Char = source.CurrentChar
      Dim num As Integer
      If currentChar <> "U"c Then
        If currentChar <> "u"c Then
          details.[Error] = "Invalid escape symbol, expected 'u' or 'U' only."
          Return vbNullChar
        End If
        num = 4
      Else
        num = 8
      End If
      If source.Position + num > source.Text.Length Then
        details.[Error] = "Invalid escape symbol"
        Return vbNullChar
      End If
      source.Position += 1
      Dim value As String = source.Text.Substring(source.Position, num)
      Dim result As Char = ChrW(Convert.ToUInt32(value, 16))
      source.Position += num
      details.Flags = details.Flags Or ScanFlags.HasEscapes
      Return result
    End Function

    ' Token: 0x06000175 RID: 373 RVA: 0x00007ED7 File Offset: 0x000060D7
    Protected Overrides Function ConvertValue(details As ScanDetails) As Boolean
      If details.IsSet(ScanFlags.Binary) Then
        details.Value = details.Prefix + details.Body
      Else
        details.Value = details.Body
      End If
      Return True
    End Function

    ' Token: 0x06000176 RID: 374 RVA: 0x00007F08 File Offset: 0x00006108
    Public Overrides Function GetFirsts() As IList(Of String)
      Dim keyList As New KeyList()
      keyList.AddRange(Prefixes)
      If String.IsNullOrEmpty(AllFirstChars) Then
        Return keyList
      End If
      Dim array As Char() = AllFirstChars.ToCharArray()
      For Each c As Char In array
        keyList.Add(c.ToString())
      Next
      If IsSet(TermOptions.CanStartWithEscape) Then
        keyList.Add(EscapeChar.ToString())
      End If
      Return keyList
    End Function

    ' Token: 0x040000E9 RID: 233
    Public AllChars As String

    ' Token: 0x040000EA RID: 234
    Public AllFirstChars As String

    ' Token: 0x040000EB RID: 235
    Private _terminators As String

    ' Token: 0x040000EC RID: 236
    Private _keywordHash As StringDictionary

    ' Token: 0x040000ED RID: 237
    Public Keywords As New KeyList()

    ' Token: 0x040000EE RID: 238
    Public StartCharCategories As New UnicodeCategoryList()

    ' Token: 0x040000EF RID: 239
    Public CharCategories As New UnicodeCategoryList()

    ' Token: 0x040000F0 RID: 240
    Public CharsToRemoveCategories As New UnicodeCategoryList()
  End Class
End Namespace
