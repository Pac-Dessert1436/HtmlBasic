Namespace Irony.Compiler
  ' Token: 0x0200003A RID: 58
  Public Class ParserState
    ' Token: 0x06000130 RID: 304 RVA: 0x00005B3F File Offset: 0x00003D3F
    Public Sub New(name As String, item As LRItem)
      Me.Name = name
      Items.Add(item)
    End Sub

    ' Token: 0x06000131 RID: 305 RVA: 0x00005B70 File Offset: 0x00003D70
    Public Sub New(name As String, coreItems As LR0ItemList)
      Me.Name = name
      For Each core As LR0Item In coreItems
        Items.Add(New LRItem(Me, core))
      Next
    End Sub

    ' Token: 0x06000132 RID: 306 RVA: 0x00005BEC File Offset: 0x00003DEC
    Public Overrides Function ToString() As String
      Return Name
    End Function

    ' Token: 0x040000CC RID: 204
    Public Name As String

    ' Token: 0x040000CD RID: 205
    Public Actions As New ActionRecordTable()

    ' Token: 0x040000CE RID: 206
    Public Items As New LRItemList()
  End Class
End Namespace
