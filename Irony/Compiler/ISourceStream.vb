Namespace Irony.Compiler
  ' Token: 0x02000012 RID: 18
  Public Interface ISourceStream
    ' Token: 0x17000003 RID: 3
    ' (get) Token: 0x0600003D RID: 61
    ' (set) Token: 0x0600003E RID: 62
    Property Position As Integer

    ' Token: 0x17000004 RID: 4
    ' (get) Token: 0x0600003F RID: 63
    ReadOnly Property CurrentChar As Char

    ' Token: 0x17000005 RID: 5
    ' (get) Token: 0x06000040 RID: 64
    ReadOnly Property NextChar As Char

    ' Token: 0x06000041 RID: 65
    Function MatchSymbol(symbol As String, ignoreCase As Boolean) As Boolean

    ' Token: 0x17000006 RID: 6
    ' (get) Token: 0x06000042 RID: 66
    ReadOnly Property Text As String

    ' Token: 0x06000043 RID: 67
    Function GetLexeme() As String

    ' Token: 0x17000007 RID: 7
    ' (get) Token: 0x06000044 RID: 68
    ' (set) Token: 0x06000045 RID: 69
    Property TokenStart As SourceLocation

    ' Token: 0x17000008 RID: 8
    ' (get) Token: 0x06000046 RID: 70
    ReadOnly Property TabWidth As Integer

    ' Token: 0x06000047 RID: 71
    Function EOF() As Boolean
  End Interface
End Namespace
