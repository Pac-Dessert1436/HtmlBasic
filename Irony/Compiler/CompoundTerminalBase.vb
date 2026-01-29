Namespace Irony.Compiler
  ' Token: 0x02000019 RID: 25
  Public MustInherit Class CompoundTerminalBase
    Inherits Terminal

    ' Token: 0x06000062 RID: 98 RVA: 0x00002BCC File Offset: 0x00000DCC
    Public Sub New(name As String, options As TermOptions)
      MyBase.New(name)
      SetOption(options)
      Escapes = GetDefaultEscapes()
    End Sub

    ' Token: 0x06000063 RID: 99 RVA: 0x00002C31 File Offset: 0x00000E31
    Public Sub New(name As String)
      Me.New(name, TermOptions.None)
    End Sub

    ' Token: 0x06000064 RID: 100 RVA: 0x00002C3B File Offset: 0x00000E3B
    Public Sub AddPrefixFlag(prefix As String, flags As ScanFlags)
      PrefixFlags.Add(prefix, flags)
      Prefixes.Add(prefix)
    End Sub

    ' Token: 0x06000065 RID: 101 RVA: 0x00002C56 File Offset: 0x00000E56
    Public Sub AddSuffixCodes(suffix As String, ParamArray codes As TypeCode())
      SuffixTypeCodes.Add(suffix, codes)
      Suffixes.Add(suffix)
    End Sub

    ' Token: 0x14000001 RID: 1
    ' (add) Token: 0x06000066 RID: 102 RVA: 0x00002C71 File Offset: 0x00000E71
    ' (remove) Token: 0x06000067 RID: 103 RVA: 0x00002C8A File Offset: 0x00000E8A
    Public Event ConvertingValue As EventHandler(Of ScannerConvertingValueEventArgs)

    ' Token: 0x06000068 RID: 104 RVA: 0x00002CA4 File Offset: 0x00000EA4
    Public Overrides Sub Init(grammar As Grammar)
      MyBase.Init(grammar)
      _defaultTypes = New TypeCode() {DefaultType}
      Prefixes.Sort(AddressOf KeyList.LongerFirst)
      _prefixesFirsts = String.Empty
      For Each text As String In Prefixes
        _prefixesFirsts += text(0)
      Next
      Suffixes.Sort(AddressOf KeyList.LongerFirst)
      _suffixesFirsts = String.Empty
      For Each text2 As String In Suffixes
        _suffixesFirsts += text2(0)
      Next
      If IsSet(TermOptions.SpecialIgnoreCase) Then
        _prefixesFirsts = _prefixesFirsts.ToLower() + _prefixesFirsts.ToUpper()
        _suffixesFirsts = _suffixesFirsts.ToLower() + _suffixesFirsts.ToUpper()
      End If
    End Sub

    ' Token: 0x06000069 RID: 105 RVA: 0x00002E18 File Offset: 0x00001018
    Public Overrides Function GetFirsts() As IList(Of String)
      Return Prefixes
    End Function

    ' Token: 0x0600006A RID: 106 RVA: 0x00002E20 File Offset: 0x00001020
    Public Overrides Function TryMatch(context As CompilerContext, source As ISourceStream) As Token
      If IsSet(TermOptions.EnableQuickParse) Then
        Dim token As Token = QuickParse(context, source)
        If token IsNot Nothing Then
          Return token
        End If
      End If
      source.Position = source.TokenStart.Position
      Dim scanDetails As New ScanDetails()
      scanDetails.Flags = DefaultFlags
      scanDetails.TypeCodes = _defaultTypes
      ReadPrefix(source, scanDetails)
      If Not ReadBody(source, scanDetails) Then
        Return Nothing
      End If
      If scanDetails.[Error] IsNot Nothing Then
        Return Grammar.CreateSyntaxErrorToken(context, source.TokenStart, scanDetails.[Error], New Object(-1) {})
      End If
      ReadSuffix(source, scanDetails)
      If Not ConvertValue(scanDetails) Then
        Return Grammar.CreateSyntaxErrorToken(context, source.TokenStart, "Failed to convert the value: " + scanDetails.[Error], New Object(-1) {})
      End If
      Return CreateToken(context, source, scanDetails)
    End Function

    ' Token: 0x0600006B RID: 107 RVA: 0x00002EF0 File Offset: 0x000010F0
    Protected Overridable Function CreateToken(context As CompilerContext, source As ISourceStream, details As ScanDetails) As Token
      Dim lexeme As String = source.GetLexeme()
      Dim token As Token = Token.Create(Me, context, source.TokenStart, lexeme, details.Value)
      token.Details = details
      Return token
    End Function

    ' Token: 0x0600006C RID: 108 RVA: 0x00002F21 File Offset: 0x00001121
    Protected Overridable Function QuickParse(context As CompilerContext, source As ISourceStream) As Token
      Return Nothing
    End Function

    ' Token: 0x0600006D RID: 109 RVA: 0x00002F24 File Offset: 0x00001124
    Protected Overridable Sub ReadPrefix(source As ISourceStream, details As ScanDetails)
      If _prefixesFirsts.IndexOf(source.CurrentChar) < 0 Then
        Return
      End If
      Dim ignoreCase As Boolean = IsSet(TermOptions.SpecialIgnoreCase)
      For Each text As String In Prefixes
        If source.MatchSymbol(text, ignoreCase) Then
          details.Prefix = text
          source.Position += text.Length
          Dim scanFlags As ScanFlags
          If Not String.IsNullOrEmpty(details.Prefix) AndAlso PrefixFlags.TryGetValue(details.Prefix, scanFlags) Then
            details.Flags = details.Flags Or scanFlags
          End If
          Exit For
        End If
      Next
    End Sub

    ' Token: 0x0600006E RID: 110 RVA: 0x00002FE8 File Offset: 0x000011E8
    Protected Overridable Sub ReadSuffix(source As ISourceStream, details As ScanDetails)
      If _suffixesFirsts.IndexOf(source.CurrentChar) < 0 Then
        Return
      End If
      Dim ignoreCase As Boolean = IsSet(TermOptions.SpecialIgnoreCase)
      For Each text As String In Suffixes
        If source.MatchSymbol(text, ignoreCase) Then
          details.Suffix = text
          source.Position += text.Length
          Dim typeCodes As TypeCode() = Nothing
          If Not String.IsNullOrEmpty(details.Suffix) AndAlso SuffixTypeCodes.TryGetValue(details.Suffix, typeCodes) Then
            details.TypeCodes = typeCodes
          End If
          Exit For
        End If
      Next
    End Sub

    ' Token: 0x0600006F RID: 111 RVA: 0x000030A4 File Offset: 0x000012A4
    Protected Overridable Function ReadBody(source As ISourceStream, details As ScanDetails) As Boolean
      Return False
    End Function

    ' Token: 0x06000070 RID: 112 RVA: 0x000030A8 File Offset: 0x000012A8
    Protected Overridable Function ConvertValue(details As ScanDetails) As Boolean
      details.Value = details.Body
      Return OnConvertingValue(details)
    End Function

    ' Token: 0x06000071 RID: 113 RVA: 0x000030D4 File Offset: 0x000012D4
    Protected Overridable Function OnConvertingValue(details As ScanDetails) As Boolean
      Dim scannerConvertingValueEventArgs As New ScannerConvertingValueEventArgs(details)
      RaiseEvent ConvertingValue(Me, scannerConvertingValueEventArgs)
      Return scannerConvertingValueEventArgs.Converted
    End Function

    ' Token: 0x0400006B RID: 107
    Public EscapeChar As Char = "\"c

    ' Token: 0x0400006C RID: 108
    Public Escapes As New EscapeTable()

    ' Token: 0x0400006D RID: 109
    Public DefaultFlags As ScanFlags

    ' Token: 0x0400006E RID: 110
    Public DefaultType As TypeCode

    ' Token: 0x0400006F RID: 111
    Protected PrefixFlags As New CompoundTerminalBase.ScanFlagTable()

    ' Token: 0x04000070 RID: 112
    Protected SuffixTypeCodes As New CompoundTerminalBase.TypeCodeTable()

    ' Token: 0x04000071 RID: 113
    Protected Prefixes As New KeyList()

    ' Token: 0x04000072 RID: 114
    Protected Suffixes As New KeyList()

    ' Token: 0x04000073 RID: 115
    Private _prefixesFirsts As String

    ' Token: 0x04000074 RID: 116
    Private _suffixesFirsts As String

    ' Token: 0x04000075 RID: 117
    Private _defaultTypes As TypeCode()

    ' Token: 0x0200001A RID: 26
    Public Class ScanFlagTable
      Inherits Dictionary(Of String, ScanFlags)

    End Class

    ' Token: 0x0200001B RID: 27
    Public Class TypeCodeTable
      Inherits Dictionary(Of String, TypeCode())

    End Class
  End Class
End Namespace
