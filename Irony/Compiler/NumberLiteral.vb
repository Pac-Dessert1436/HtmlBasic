Imports System.Globalization
Imports System.Numerics

Namespace Irony.Compiler
  ' Token: 0x02000059 RID: 89
  Public Class NumberLiteral
    Inherits CompoundTerminalBase

    ' Token: 0x06000258 RID: 600 RVA: 0x0000BD06 File Offset: 0x00009F06
    Public Sub New(name As String, options As TermOptions)
      Me.New(name)
      SetOption(options)
    End Sub

    ' Token: 0x06000259 RID: 601 RVA: 0x0000BD18 File Offset: 0x00009F18
    Public Sub New(name As String)
      MyBase.New(name)
      MatchMode = TokenMatchMode.ByType
    End Sub

    ' Token: 0x0600025A RID: 602 RVA: 0x0000BD61 File Offset: 0x00009F61
    Public Sub New(name As String, displayName As String)
      Me.New(name)
      Me.DisplayName = displayName
    End Sub

    ' Token: 0x0600025B RID: 603 RVA: 0x0000BD74 File Offset: 0x00009F74
    Public Overrides Sub Init(grammar As Grammar)
      MyBase.Init(grammar)
      If String.IsNullOrEmpty(QuickParseTerminators) Then
        QuickParseTerminators = grammar.WhitespaceChars + grammar.Delimiters
      End If
      _defaultFloatTypes = New TypeCode() {DefaultFloatType}
    End Sub

    ' Token: 0x0600025C RID: 604 RVA: 0x0000BDC4 File Offset: 0x00009FC4
    Public Overrides Function GetFirsts() As IList(Of String)
      Dim stringList As New StringList()
      stringList.AddRange(Prefixes)
      stringList.AddRange(New String() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"})
      If IsSet(TermOptions.NumberAllowStartEndDot) Then
        stringList.Add(DecimalSeparator.ToString())
      End If
      Return stringList
    End Function

    ' Token: 0x0600025D RID: 605 RVA: 0x0000BE64 File Offset: 0x0000A064
    Protected Overrides Function QuickParse(context As CompilerContext, source As ISourceStream) As Token
      Dim currentChar As Char = source.CurrentChar
      If Char.IsDigit(currentChar) AndAlso QuickParseTerminators.Contains(source.NextChar) Then
        Dim num As Integer = AscW(currentChar) - 48
        Dim value As Object
        Select Case DefaultIntTypes(0)
          Case TypeCode.[SByte]
            value = CSByte(num)
          Case TypeCode.[Byte]
            value = CByte(num)
          Case TypeCode.Int16
            value = CShort(num)
          Case TypeCode.UInt16
            value = CUShort(num)
          Case TypeCode.Int32
            value = num
          Case TypeCode.UInt32
            value = CUInt(num)
          Case Else
            Return Nothing
        End Select
        Dim result As Token = Token.Create(Me, context, source.TokenStart, currentChar.ToString(), value)
        source.Position += 1
        Return result
      End If
      Return Nothing
    End Function

    ' Token: 0x0600025E RID: 606 RVA: 0x0000BF31 File Offset: 0x0000A131
    Protected Overrides Sub ReadPrefix(source As ISourceStream, details As ScanDetails)
      If source.CurrentChar = "0"c AndAlso source.NextChar = "."c Then
        Return
      End If
      MyBase.ReadPrefix(source, details)
    End Sub

    ' Token: 0x0600025F RID: 607 RVA: 0x0000BF50 File Offset: 0x0000A150
    Protected Overrides Sub ReadSuffix(source As ISourceStream, details As ScanDetails)
      MyBase.ReadSuffix(source, details)
      If String.IsNullOrEmpty(details.Suffix) Then
        details.TypeCodes = If(details.IsSet(ScanFlags.HasDotOrExp), _defaultFloatTypes, DefaultIntTypes)
      End If
    End Sub

    ' Token: 0x06000260 RID: 608 RVA: 0x0000BF88 File Offset: 0x0000A188
    Protected Overrides Function ReadBody(source As ISourceStream, details As ScanDetails) As Boolean
      Dim position As Integer = source.Position
      Dim digits As String = GetDigits(details)
      Dim flag As Boolean = Not details.IsSet(ScanFlags.NonDecimal)
      Dim flag2 As Boolean = Not IsSet(TermOptions.NumberIntOnly)
      While Not source.EOF()
        Dim currentChar As Char = source.CurrentChar
        If digits.Contains(currentChar) Then
          source.Position += 1
        ElseIf currentChar = DecimalSeparator AndAlso flag2 Then
          If details.IsSet(ScanFlags.HasDotOrExp) OrElse (digits.IndexOf(source.NextChar) < 0 AndAlso Not IsSet(TermOptions.NumberAllowStartEndDot)) Then
            Exit While
          End If
          details.Flags = details.Flags Or ScanFlags.HasDot
          source.Position += 1
        Else
          If Not flag2 OrElse Not flag OrElse details.ControlSymbol IsNot Nothing OrElse ExponentSymbols.IndexOf(currentChar) < 0 Then
            Exit While
          End If
          Dim nextChar As Char = source.NextChar
          Dim flag3 As Boolean = nextChar = "-"c OrElse nextChar = "+"c
          Dim flag4 As Boolean = digits.Contains(nextChar)
          If Not flag3 AndAlso Not flag4 Then
            Exit While
          End If
          details.ControlSymbol = currentChar.ToString()
          details.Flags = details.Flags Or ScanFlags.HasExp
          source.Position += 1
          If flag3 Then
            source.Position += 1
          End If
        End If
      End While
      Dim position2 As Integer = source.Position
      details.Body = source.Text.Substring(position, position2 - position)
      Return True
    End Function

    ' Token: 0x06000261 RID: 609 RVA: 0x0000C104 File Offset: 0x0000A304
    Protected Overrides Function ConvertValue(details As ScanDetails) As Boolean
      If String.IsNullOrEmpty(details.Body) Then
        details.[Error] = "Invalid number."
        Return False
      End If
      If MyBase.ConvertValue(details) Then
        Return True
      End If
      Dim typeCode As TypeCode = details.TypeCodes(0)
      If typeCode <> TypeCode.Int32 Then
        If typeCode = TypeCode.[Double] Then
          If QuickConvertToDouble(details) Then
            Return True
          End If
        End If
      ElseIf QuickConvertToInt32(details) Then
        Return True
      End If
      details.Value = Nothing
      Dim typeCodes As TypeCode() = details.TypeCodes
      Dim i As Integer = 0
      While i < typeCodes.Length
        Dim typeCode2 As TypeCode = typeCodes(i)
        Dim typeCode3 As TypeCode = typeCode2
        Select Case typeCode3
          Case TypeCode.[SByte], TypeCode.[Byte], TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32, TypeCode.Int64, TypeCode.UInt64
            If details.Value Is Nothing Then
              TryConvertToUlong(details)
            End If
            If TryCastToIntegerType(typeCode2, details) Then
              Return True
            End If
          Case TypeCode.[Single], TypeCode.[Double], TypeCode.[Decimal]
            GoTo IL_B7
          Case Else
            Select Case typeCode3
              Case CType(30, TypeCode)
                If ConvertToBigInteger(details) Then
                  Return True
                End If
              Case CType(31, TypeCode)
                GoTo IL_B7
            End Select
        End Select
        i += 1
        Continue While
IL_B7:
        Return ConvertToFloat(typeCode2, details)
      End While
      Return False
    End Function

    ' Token: 0x06000262 RID: 610 RVA: 0x0000C214 File Offset: 0x0000A414
    Private Function QuickConvertToInt32(details As ScanDetails) As Boolean
      Dim typeCode As TypeCode = details.TypeCodes(0)
      Dim radix As Integer = GetRadix(details)
      If radix = 10 AndAlso details.Body.Length > 10 Then
        Return False
      End If
      Dim result As Boolean
      Try
        If radix = 10 Then
          details.Value = Convert.ToInt32(details.Body, CultureInfo.InvariantCulture)
        Else
          details.Value = Convert.ToInt32(details.Body, radix)
        End If
        result = True
      Catch
        result = False
      End Try
      Return result
    End Function

    ' Token: 0x06000263 RID: 611 RVA: 0x0000C29C File Offset: 0x0000A49C
    Private Function QuickConvertToDouble(details As ScanDetails) As Boolean
      If details.IsSet(CType(43, ScanFlags)) Then
        Return False
      End If
      If DecimalSeparator <> "."c Then
        Return False
      End If
      Dim num As Double
      If Not Double.TryParse(details.Body, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, num) Then
        Return False
      End If
      details.Value = num
      Return True
    End Function

    ' Token: 0x06000264 RID: 612 RVA: 0x0000C2E8 File Offset: 0x0000A4E8
    Private Function ConvertToFloat(typeCode As TypeCode, details As ScanDetails) As Boolean
      If details.IsSet(ScanFlags.NonDecimal) Then
        details.[Error] = "Invalid number."
        Return False
      End If
      Dim text As String = details.Body
      If details.IsSet(ScanFlags.HasExp) AndAlso details.ControlSymbol.ToUpper() <> "E" Then
        text = text.Replace(details.ControlSymbol, "E")
      End If
      If details.IsSet(ScanFlags.HasDot) AndAlso DecimalSeparator <> "."c Then
        text = text.Replace(DecimalSeparator, "."c)
      End If
      Select Case typeCode
        Case TypeCode.[Single]
          Dim num As Single
          If Not Single.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, num) Then
            Return False
          End If
          details.Value = num
          Return True
        Case TypeCode.[Double]
        Case TypeCode.[Decimal]
          Dim num2 As Decimal
          If Not Decimal.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, num2) Then
            Return False
          End If
          details.Value = num2
          Return True
        Case Else
          If typeCode <> CType(31, TypeCode) Then
            Return False
          End If
      End Select
      Dim num3 As Double
      If Not Double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, num3) Then
        Return False
      End If
      If typeCode = CType(31, TypeCode) Then
        details.Value = New Complex64(0.0, num3)
      Else
        details.Value = num3
      End If
      Return True
    End Function

    ' Token: 0x06000265 RID: 613 RVA: 0x0000C41C File Offset: 0x0000A61C
    Private Shared Function TryCastToIntegerType(typeCode As TypeCode, details As ScanDetails) As Boolean
      If details.Value Is Nothing Then
        Return False
      End If
      Dim result As Boolean
      Try
        If typeCode <> TypeCode.UInt64 Then
          details.Value = Convert.ChangeType(details.Value, typeCode, CultureInfo.InvariantCulture)
        End If
        result = True
      Catch ex As Exception
        Trace.WriteLine(String.Concat(New Object() {"Error converting to integer: text=[", details.Body, "], type=", typeCode, ", error: ", ex.Message}))
        result = False
      End Try
      Return result
    End Function

    ' Token: 0x06000266 RID: 614 RVA: 0x0000C4B0 File Offset: 0x0000A6B0
    Private Function TryConvertToUlong(details As ScanDetails) As Boolean
      Dim result As Boolean
      Try
        Dim radix As Integer = GetRadix(details)
        If radix = 10 Then
          details.Value = Convert.ToUInt64(details.Body, CultureInfo.InvariantCulture)
        Else
          details.Value = Convert.ToUInt64(details.Body, radix)
        End If
        result = True
      Catch ex As OverflowException
        result = False
      End Try
      Return result
    End Function

    ' Token: 0x06000267 RID: 615 RVA: 0x0000C518 File Offset: 0x0000A718
    Private Function ConvertToBigInteger(details As ScanDetails) As Boolean
      details.Body = details.Body.TrimStart(New Char() {"0"c})
      Dim length As Integer = details.Body.Length
      Dim radix As Integer = GetRadix(details)
      Dim num As Integer = GetSafeWordLength(details)
      Dim sectionCount As Integer = GetSectionCount(length, num)
      Dim array As ULong() = New ULong(sectionCount - 1) {}
      Try
        Dim num2 As Integer = details.Body.Length - num
        For i As Integer = sectionCount - 1 To 0 Step -1
          If num2 < 0 Then
            num += num2
            num2 = 0
          End If
          If radix = 10 Then
            array(i) = Convert.ToUInt64(details.Body.Substring(num2, num))
          Else
            array(i) = Convert.ToUInt64(details.Body.Substring(num2, num), radix)
          End If
          num2 -= num
        Next
      Catch
        details.[Error] = "Invalid number."
        Return False
      End Try
      Dim safeWordRadix As ULong = GetSafeWordRadix(details)
      Dim bigInteger As BigInteger = array(0)
      For j As Integer = 1 To sectionCount - 1
        bigInteger = bigInteger * safeWordRadix + array(j)
      Next
      details.Value = bigInteger
      Return True
    End Function

    ' Token: 0x06000268 RID: 616 RVA: 0x0000C654 File Offset: 0x0000A854
    Private Shared Function GetRadix(details As ScanDetails) As Integer
      If details.IsSet(ScanFlags.Hex) Then
        Return 16
      End If
      If details.IsSet(ScanFlags.Octal) Then
        Return 8
      End If
      If details.IsSet(ScanFlags.Binary) Then
        Return 2
      End If
      Return 10
    End Function

    ' Token: 0x06000269 RID: 617 RVA: 0x0000C67A File Offset: 0x0000A87A
    Private Shared Function GetDigits(details As ScanDetails) As String
      If details.IsSet(ScanFlags.Hex) Then
        Return "1234567890aAbBcCdDeEfF"
      End If
      If details.IsSet(ScanFlags.Octal) Then
        Return "12345670"
      End If
      If details.IsSet(ScanFlags.Binary) Then
        Return "01"
      End If
      Return "1234567890"
    End Function

    ' Token: 0x0600026A RID: 618 RVA: 0x0000C6AE File Offset: 0x0000A8AE
    Private Shared Function GetSafeWordLength(details As ScanDetails) As Integer
      If details.IsSet(ScanFlags.Hex) Then
        Return 15
      End If
      If details.IsSet(ScanFlags.Octal) Then
        Return 21
      End If
      If details.IsSet(ScanFlags.Binary) Then
        Return 63
      End If
      Return 19
    End Function

    ' Token: 0x0600026B RID: 619 RVA: 0x0000C6D8 File Offset: 0x0000A8D8
    Private Shared Function GetSectionCount(stringLength As Integer, safeWordLength As Integer) As Integer
      Dim num2 As Integer
      Dim num As Integer = Math.DivRem(stringLength, safeWordLength, num2)
      If num2 <> 0 Then
        Return num + 1
      End If
      Return num
    End Function

    ' Token: 0x0600026C RID: 620 RVA: 0x0000C6F8 File Offset: 0x0000A8F8
    Private Shared Function GetSafeWordRadix(details As ScanDetails) As ULong
      If details.IsSet(ScanFlags.Hex) Then
        Return 1152921504606846976UL
      End If
      If details.IsSet(ScanFlags.Octal) Then
        Return 9223372036854775808UL
      End If
      If details.IsSet(ScanFlags.Binary) Then
        Return 9223372036854775808UL
      End If
      Return 10000000000000000000UL
    End Function

    ' Token: 0x0600026D RID: 621 RVA: 0x0000C747 File Offset: 0x0000A947
    Private Shared Function IsIntegerCode(code As TypeCode) As Boolean
      Return code >= TypeCode.[SByte] AndAlso code <= TypeCode.UInt64
    End Function

    ' Token: 0x0400013C RID: 316
    Public Const TypeCodeBigInt As TypeCode = CType(30, TypeCode)

    ' Token: 0x0400013D RID: 317
    Public Const TypeCodeImaginary As TypeCode = CType(31, TypeCode)

    ' Token: 0x0400013E RID: 318
    Public QuickParseTerminators As String

    ' Token: 0x0400013F RID: 319
    Public ExponentSymbols As String = "eE"

    ' Token: 0x04000140 RID: 320
    Public DecimalSeparator As Char = "."c

    ' Token: 0x04000141 RID: 321
    Public DefaultIntTypes As TypeCode() = New TypeCode() {TypeCode.Int32}

    ' Token: 0x04000142 RID: 322
    Public DefaultFloatType As TypeCode = TypeCode.[Double]

    ' Token: 0x04000143 RID: 323
    Private _defaultFloatTypes As TypeCode()
  End Class
End Namespace
