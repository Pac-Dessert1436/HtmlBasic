Namespace Irony.Compiler
  ' Token: 0x02000007 RID: 7
  Public Enum TermOptions
    ' Token: 0x0400001A RID: 26
    None
    ' Token: 0x0400001B RID: 27
    IsOperator
    ' Token: 0x0400001C RID: 28
    IsGrammarSymbol
    ' Token: 0x0400001D RID: 29
    IsOpenBrace = 4
    ' Token: 0x0400001E RID: 30
    IsCloseBrace = 8
    ' Token: 0x0400001F RID: 31
    IsBrace = 12
    ' Token: 0x04000020 RID: 32
    IsConstant = 16
    ' Token: 0x04000021 RID: 33
    IsPunctuation = 32
    ' Token: 0x04000022 RID: 34
    IsDelimiter = 64
    ' Token: 0x04000023 RID: 35
    IsList = 128
    ' Token: 0x04000024 RID: 36
    IsNonGrammar = 256
    ' Token: 0x04000025 RID: 37
    SpecialIgnoreCase = 65536
    ' Token: 0x04000026 RID: 38
    EnableQuickParse = 131072
    ' Token: 0x04000027 RID: 39
    CanStartWithEscape = 262144
    ' Token: 0x04000028 RID: 40
    NumberAllowStartEndDot = 1048576
    ' Token: 0x04000029 RID: 41
    NumberIntOnly = 2097152
  End Enum
End Namespace
