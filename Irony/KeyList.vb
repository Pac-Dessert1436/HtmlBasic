Namespace Irony
  ' Token: 0x0200000D RID: 13
  Public Class KeyList
    Inherits List(Of String)

    ' Token: 0x0600000F RID: 15 RVA: 0x0000226D File Offset: 0x0000046D
    Public Sub New()
    End Sub

    ' Token: 0x06000010 RID: 16 RVA: 0x00002280 File Offset: 0x00000480
    Public Sub New(ParamArray keys As String())
      AddRange(keys)
    End Sub

    ' Token: 0x06000011 RID: 17 RVA: 0x0000229A File Offset: 0x0000049A
    Public Overloads Sub Add(key As String)
      If Not Contains(key) Then
        MyBase.Add(key)
        _hash.Add(key, 1)
      End If
    End Sub

    ' Token: 0x06000012 RID: 18 RVA: 0x000022BC File Offset: 0x000004BC
    Public Overloads Sub AddRange(keys As IEnumerable(Of String))
      For Each key As String In keys
        Add(key)
      Next
    End Sub

    ' Token: 0x06000013 RID: 19 RVA: 0x00002304 File Offset: 0x00000504
    Public Overloads Sub Remove(key As String)
      MyBase.Remove(key)
      _hash.Remove(key)
    End Sub

    ' Token: 0x06000014 RID: 20 RVA: 0x0000231B File Offset: 0x0000051B
    Public Overloads Function Contains(key As String) As Boolean
      Return _hash.ContainsKey(key)
    End Function

    ' Token: 0x06000015 RID: 21 RVA: 0x00002329 File Offset: 0x00000529
    Public Overloads Overrides Function ToString() As String
      Return ToString(" ")
    End Function

    ' Token: 0x06000016 RID: 22 RVA: 0x00002338 File Offset: 0x00000538
    Public Overloads Function ToString(separator As String) As String
      Dim array As String() = New String(Count - 1) {}
      CopyTo(array)
      For i As Integer = 0 To array.Length - 1
        Dim text As String = array(i)
        If text.EndsWith(vbBack) Then
          array(i) = text.Substring(0, text.Length - 1)
        End If
      Next
      Return String.Join(separator, array)
    End Function

    ' Token: 0x06000017 RID: 23 RVA: 0x00002390 File Offset: 0x00000590
    Public Overloads Sub Clear()
      MyBase.Clear()
      _hash.Clear()
    End Sub

    ' Token: 0x06000018 RID: 24 RVA: 0x000023A4 File Offset: 0x000005A4
    Public Shared Function LongerFirst(x As String, y As String) As Integer
      Try
        If x.Length > y.Length Then
          Return -1
        End If
      Catch
      End Try
      Return 0
    End Function

    ' Token: 0x04000052 RID: 82
    Private _hash As New Dictionary(Of String, Byte)()
  End Class
End Namespace
