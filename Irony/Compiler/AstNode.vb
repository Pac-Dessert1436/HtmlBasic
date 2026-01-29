Imports HtmlBasic.Irony.Runtime

Namespace Irony.Compiler
  ' Token: 0x02000036 RID: 54
  Public Class AstNode
    ' Token: 0x0600010F RID: 271 RVA: 0x000056F8 File Offset: 0x000038F8
    Public Sub New(args As AstNodeArgs)
      Term = args.Term
      Span = args.Span
      If args.ChildNodes Is Nothing OrElse args.ChildNodes.Count = 0 Then
        Return
      End If
      For Each astNode As AstNode In args.ChildNodes
        If astNode IsNot Nothing AndAlso Not astNode.Term.IsSet(TermOptions.IsPunctuation) Then
          ChildNodes.Add(astNode)
          astNode.Parent = Me
        End If
      Next
    End Sub

    ' Token: 0x1700002C RID: 44
    ' (get) Token: 0x06000110 RID: 272 RVA: 0x000057B0 File Offset: 0x000039B0
    Public ReadOnly Property Location As SourceLocation
      Get
        Return Span.Start
      End Get
    End Property

    ' Token: 0x1700002D RID: 45
    ' (get) Token: 0x06000111 RID: 273 RVA: 0x000057BD File Offset: 0x000039BD
    ' (set) Token: 0x06000112 RID: 274 RVA: 0x000057C5 File Offset: 0x000039C5
    Public Property Parent As AstNode
      Get
        Return _parent
      End Get
      Set(value As AstNode)
        _parent = value
      End Set
    End Property

    ' Token: 0x1700002F RID: 47
    ' (get) Token: 0x06000115 RID: 277 RVA: 0x000057DF File Offset: 0x000039DF
    ' (set) Token: 0x06000116 RID: 278 RVA: 0x000057E7 File Offset: 0x000039E7
    Public Property Tag As String
      Get
        Return _tag
      End Get
      Set(value As String)
        _tag = value
      End Set
    End Property

    ' Token: 0x17000030 RID: 48
    ' (get) Token: 0x06000117 RID: 279 RVA: 0x000057F0 File Offset: 0x000039F0
    Public ReadOnly Property Attributes As AttributeDictionary
      Get
        If _attributes Is Nothing Then
          _attributes = New AttributeDictionary()
        End If
        Return _attributes
      End Get
    End Property

    ' Token: 0x06000118 RID: 280 RVA: 0x0000580C File Offset: 0x00003A0C
    Public Overrides Function ToString() As String
      Dim text As String = String.Empty
      If Not String.IsNullOrEmpty(_tag) Then
        text = Tag + ": "
      End If
      text += Term.Name
      If ChildNodes.Count = 0 Then
        text += "(Empty)"
      End If
      Return text
    End Function

    ' Token: 0x06000119 RID: 281 RVA: 0x0000586C File Offset: 0x00003A6C
    Public Overridable Sub AcceptVisitor(visitor As IAstVisitor)
      visitor.BeginVisit(Me)
      If ChildNodes.Count > 0 Then
        For Each astNode As AstNode In ChildNodes
          astNode.AcceptVisitor(visitor)
        Next
      End If
      visitor.EndVisit(Me)
    End Sub

    ' Token: 0x0600011A RID: 282 RVA: 0x000058DC File Offset: 0x00003ADC
    Public Overridable Function Evaluate(context As EvaluationContext) As Object
      Return Nothing
    End Function

    ' Token: 0x040000B3 RID: 179
    Public Term As BnfTerm

    ' Token: 0x040000B4 RID: 180
    Public Span As SourceSpan

    ' Token: 0x040000B5 RID: 181
    Public ChildNodes As New AstNodeList()

    ' Token: 0x040000B6 RID: 182
    Private _parent As AstNode

    ' Token: 0x040000B8 RID: 184
    Private _tag As String

    ' Token: 0x040000B9 RID: 185
    Private _attributes As AttributeDictionary
  End Class
End Namespace
