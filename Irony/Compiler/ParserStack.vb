Namespace Irony.Compiler
  ' Token: 0x02000053 RID: 83
  Public Class ParserStack
    ' Token: 0x17000045 RID: 69
    ' (get) Token: 0x060001A3 RID: 419 RVA: 0x00008C3A File Offset: 0x00006E3A
    Public ReadOnly Property Count As Integer
      <DebuggerStepThrough()>
      Get
        Return _count
      End Get
    End Property

    ' Token: 0x17000046 RID: 70
    Default Public ReadOnly Property Item(index As Integer) As ParserStackElement
      <DebuggerStepThrough()>
      Get
        Return _data(index)
      End Get
    End Property

    ' Token: 0x17000047 RID: 71
    ' (get) Token: 0x060001A5 RID: 421 RVA: 0x00008C55 File Offset: 0x00006E55
    Public ReadOnly Property Top As ParserStackElement
      <DebuggerStepThrough()>
      Get
        Return Me(Count - 1)
      End Get
    End Property

    ' Token: 0x060001A6 RID: 422 RVA: 0x00008C68 File Offset: 0x00006E68
    Public Sub Push(node As AstNode, state As ParserState)
      If _count = _data.Length Then
        ExtendData()
      End If
      _data(_count) = New ParserStackElement(node, state)
      _count += 1
    End Sub

    ' Token: 0x060001A7 RID: 423 RVA: 0x00008CB6 File Offset: 0x00006EB6
    Public Sub Pop(popCount As Integer)
      _count -= popCount
    End Sub

    ' Token: 0x060001A8 RID: 424 RVA: 0x00008CC6 File Offset: 0x00006EC6
    Public Sub Reset()
      _data = New ParserStackElement(99) {}
      _count = 0
    End Sub

    ' Token: 0x060001A9 RID: 425 RVA: 0x00008CDC File Offset: 0x00006EDC
    Private Sub ExtendData()
      Dim elements = New ParserStackElement(_data.Length + 100 - 1) {}
      Array.Copy(_data, elements, _data.Length)
      _data = elements
    End Sub

    ' Token: 0x04000116 RID: 278
    Private Const InitialSize As Integer = 100

    ' Token: 0x04000117 RID: 279
    Private Const SizeIncrement As Integer = 100

    ' Token: 0x04000118 RID: 280
    Private _data As ParserStackElement() = New ParserStackElement(99) {}

    ' Token: 0x04000119 RID: 281
    Private _count As Integer
  End Class
End Namespace
