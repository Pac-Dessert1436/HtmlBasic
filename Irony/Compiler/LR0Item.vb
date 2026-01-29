Namespace Irony.Compiler
  ' Token: 0x02000043 RID: 67
  Public Class LR0Item
    ' Token: 0x06000145 RID: 325 RVA: 0x00005F68 File Offset: 0x00004168
    Public Sub New(production As Production, position As Integer)
      Me.Production = production
      Me.Position = position
      Dim maxID As Integer = _maxID
      _maxID = maxID + 1
      ID = maxID
      _toString = Me.Production.ToString(Me.Position)
    End Sub

    ' Token: 0x1700003D RID: 61
    ' (get) Token: 0x06000146 RID: 326 RVA: 0x00005FBE File Offset: 0x000041BE
    Public ReadOnly Property NextElement As BnfTerm
      Get
        If Position < Production.RValues.Count Then
          Return Production.RValues(Position)
        End If
        Return Nothing
      End Get
    End Property

    ' Token: 0x1700003E RID: 62
    ' (get) Token: 0x06000147 RID: 327 RVA: 0x00005FF0 File Offset: 0x000041F0
    Public ReadOnly Property IsKernel As Boolean
      Get
        Return Position > 0 OrElse (Production.IsInitial AndAlso Position = 0)
      End Get
    End Property

    ' Token: 0x06000148 RID: 328 RVA: 0x00006015 File Offset: 0x00004215
    Public Overrides Function ToString() As String
      Return _toString
    End Function

    ' Token: 0x040000DE RID: 222
    Public Production As Production

    ' Token: 0x040000DF RID: 223
    Public Position As Integer

    ' Token: 0x040000E0 RID: 224
    Public TailFirsts As New KeyList()

    ' Token: 0x040000E1 RID: 225
    Public TailIsNullable As Boolean

    ' Token: 0x040000E2 RID: 226
    Friend ID As Integer

    ' Token: 0x040000E3 RID: 227
    Friend Shared _maxID As Integer

    ' Token: 0x040000E4 RID: 228
    Private _toString As String
  End Class
End Namespace
