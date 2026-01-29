Namespace Irony.Compiler
  ' Token: 0x02000008 RID: 8
  Public Enum ScanFlags
    ' Token: 0x0400002B RID: 43
    None
    ' Token: 0x0400002C RID: 44
    Binary
    ' Token: 0x0400002D RID: 45
    Octal
    ' Token: 0x0400002E RID: 46
    Hex = 8
    ' Token: 0x0400002F RID: 47
    NonDecimal = 11
    ' Token: 0x04000030 RID: 48
    HasDot = 16
    ' Token: 0x04000031 RID: 49
    HasExp = 32
    ' Token: 0x04000032 RID: 50
    HasDotOrExp = 48
    ' Token: 0x04000033 RID: 51
    IsChar = 1
    ' Token: 0x04000034 RID: 52
    AllowDoubledQuote
    ' Token: 0x04000035 RID: 53
    AllowLineBreak = 4
    ' Token: 0x04000036 RID: 54
    LineBreakEscaped = 8
    ' Token: 0x04000037 RID: 55
    DisableEscapes = 16
    ' Token: 0x04000038 RID: 56
    AllowUEscapes = 32
    ' Token: 0x04000039 RID: 57
    AllowXEscapes = 64
    ' Token: 0x0400003A RID: 58
    AllowOctalEscapes = 128
    ' Token: 0x0400003B RID: 59
    AllowAllEscapes = 224
    ' Token: 0x0400003C RID: 60
    HasEscapes = 256
    ' Token: 0x0400003D RID: 61
    IncludePrefix = 1
    ' Token: 0x0400003E RID: 62
    IsNotKeyword
    ' Token: 0x0400003F RID: 63
    Bit0 = 1
    ' Token: 0x04000040 RID: 64
    Bit1
    ' Token: 0x04000041 RID: 65
    Bit2 = 4
    ' Token: 0x04000042 RID: 66
    Bit3 = 8
    ' Token: 0x04000043 RID: 67
    Bit4 = 16
    ' Token: 0x04000044 RID: 68
    Bit5 = 32
    ' Token: 0x04000045 RID: 69
    Bit6 = 64
    ' Token: 0x04000046 RID: 70
    Bit7 = 128
    ' Token: 0x04000047 RID: 71
    Bit8 = 256
    ' Token: 0x04000048 RID: 72
    Bit9 = 512
    ' Token: 0x04000049 RID: 73
    Bit10 = 1024
    ' Token: 0x0400004A RID: 74
    Bit11 = 2048
    ' Token: 0x0400004B RID: 75
    Bit12 = 4096
    ' Token: 0x0400004C RID: 76
    Bit13 = 8192
    ' Token: 0x0400004D RID: 77
    Bit14 = 16384
    ' Token: 0x0400004E RID: 78
    Bit15 = 32768
  End Enum
End Namespace
