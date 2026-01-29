Imports HtmlBasic.Irony.Runtime

Namespace Irony.Compiler
  ' Token: 0x02000037 RID: 55
  Public Class Token
    Inherits AstNode

    ' Token: 0x0600011B RID: 283 RVA: 0x000058DF File Offset: 0x00003ADF
    Protected Sub New(args As AstNodeArgs)
      MyBase.New(args)
    End Sub

    ' Token: 0x17000031 RID: 49
    ' (get) Token: 0x0600011C RID: 284 RVA: 0x000058E8 File Offset: 0x00003AE8
    Public ReadOnly Property Terminal As Terminal
      <DebuggerStepThrough()>
      Get
        Return TryCast(Term, Terminal)
      End Get
    End Property

    ' Token: 0x17000032 RID: 50
    ' (get) Token: 0x0600011D RID: 285 RVA: 0x000058F5 File Offset: 0x00003AF5
    Public ReadOnly Property Symbol As SymbolTerminal
      <DebuggerStepThrough()>
      Get
        Return TryCast(Term, SymbolTerminal)
      End Get
    End Property

    ' Token: 0x17000033 RID: 51
    ' (get) Token: 0x0600011E RID: 286 RVA: 0x00005902 File Offset: 0x00003B02
    Public ReadOnly Property Category As TokenCategory
      <DebuggerStepThrough()>
      Get
        Return Terminal.Category
      End Get
    End Property

    ' Token: 0x17000034 RID: 52
    ' (get) Token: 0x0600011F RID: 287 RVA: 0x0000590F File Offset: 0x00003B0F
    ' (set) Token: 0x06000120 RID: 288 RVA: 0x00005917 File Offset: 0x00003B17
    Public Property Text As String
      <DebuggerStepThrough()>
      Get
        Return _text
      End Get
      Set(value As String)
        _text = value
      End Set
    End Property

    ' Token: 0x17000035 RID: 53
    ' (get) Token: 0x06000121 RID: 289 RVA: 0x00005920 File Offset: 0x00003B20
    ' (set) Token: 0x06000122 RID: 290 RVA: 0x00005928 File Offset: 0x00003B28
    Public Property Value As Object
      <DebuggerStepThrough()>
      Get
        Return _value
      End Get
      Set(value As Object)
        _value = value
        _valueString = If((_value Is Nothing), String.Empty, _value.ToString())
      End Set
    End Property

    ' Token: 0x17000036 RID: 54
    ' (get) Token: 0x06000123 RID: 291 RVA: 0x00005951 File Offset: 0x00003B51
    Public ReadOnly Property ValueString As String
      Get
        Return _valueString
      End Get
    End Property

    ' Token: 0x06000124 RID: 292 RVA: 0x00005959 File Offset: 0x00003B59
    <DebuggerStepThrough()>
    Public Function IsError() As Boolean
      Return Category = TokenCategory.[Error]
    End Function

    ' Token: 0x06000125 RID: 293 RVA: 0x00005964 File Offset: 0x00003B64
    <DebuggerStepThrough()>
    Public Function IsMultiToken() As Boolean
      Return ChildNodes.Count > 0
    End Function

    ' Token: 0x17000037 RID: 55
    ' (get) Token: 0x06000126 RID: 294 RVA: 0x00005974 File Offset: 0x00003B74
    Public ReadOnly Property Length As Integer
      <DebuggerStepThrough()>
      Get
        If _text IsNot Nothing Then
          Return _text.Length
        End If
        Return 0
      End Get
    End Property

    ' Token: 0x17000038 RID: 56
    ' (get) Token: 0x06000127 RID: 295 RVA: 0x0000598B File Offset: 0x00003B8B
    Public ReadOnly Property MatchByValue As Boolean
      Get
        Return IsKeyword OrElse (Text IsNot Nothing AndAlso (Terminal.MatchMode And TokenMatchMode.ByValue) <> CType(0, TokenMatchMode))
      End Get
    End Property

    ' Token: 0x17000039 RID: 57
    ' (get) Token: 0x06000128 RID: 296 RVA: 0x000059B4 File Offset: 0x00003BB4
    Public ReadOnly Property MatchByType As Boolean
      Get
        Return Not IsKeyword AndAlso (Terminal.MatchMode And TokenMatchMode.ByType) <> CType(0, TokenMatchMode)
      End Get
    End Property

    ' Token: 0x06000129 RID: 297 RVA: 0x000059D3 File Offset: 0x00003BD3
    Public Overrides Function Evaluate(context As EvaluationContext) As Object
      Return Value
    End Function

    ' Token: 0x0600012A RID: 298 RVA: 0x000059DC File Offset: 0x00003BDC
    <DebuggerStepThrough()>
    Public Overrides Function ToString() As String
      If TypeOf Terminal Is SymbolTerminal Then
        Return _text + " [Symbol]"
      End If
      If IsKeyword Then
        Return _text + " [Keyword]"
      End If
      Return ValueString + " " + Terminal.ToString()
    End Function

    ' Token: 0x0600012B RID: 299 RVA: 0x00005A3B File Offset: 0x00003C3B
    Public Shared Function Create(term As Terminal, context As CompilerContext, location As SourceLocation, text As String) As Token
      Return Create(term, context, location, text, text)
    End Function

    ' Token: 0x0600012C RID: 300 RVA: 0x00005A48 File Offset: 0x00003C48
    Public Shared Function Create(term As Terminal, context As CompilerContext, location As SourceLocation, text As String, value As Object) As Token
      Dim length As Integer = If((text Is Nothing), 0, text.Length)
      Dim span As New SourceSpan(location, length)
      Dim args As New AstNodeArgs(term, context, span, Nothing)
      Return New Token(args) With {.Text = text, .Value = value}
    End Function

    ' Token: 0x0600012D RID: 301 RVA: 0x00005A90 File Offset: 0x00003C90
    Public Shared Function CreateMultiToken(term As Terminal, context As CompilerContext, tokens As TokenList) As Token
      Dim args As New AstNodeArgs(term, context, Nothing, Nothing)
      Dim token As New Token(args)
      token.ChildNodes.AddRange(tokens.ToArray())
      Return token
    End Function

    ' Token: 0x040000BA RID: 186
    Private _text As String

    ' Token: 0x040000BB RID: 187
    Private _value As Object

    ' Token: 0x040000BC RID: 188
    Private _valueString As String

    ' Token: 0x040000BD RID: 189
    Public Details As ScanDetails

    ' Token: 0x040000BE RID: 190
    Public IsKeyword As Boolean
  End Class
End Namespace
