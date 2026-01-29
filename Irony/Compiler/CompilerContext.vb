Namespace Irony.Compiler
  ' Token: 0x02000009 RID: 9
  Public Class CompilerContext
    ' Token: 0x06000008 RID: 8 RVA: 0x000021E8 File Offset: 0x000003E8
    Public Sub New(compiler As LanguageCompiler)
      Me.Compiler = compiler
    End Sub

    ' Token: 0x06000009 RID: 9 RVA: 0x0000220D File Offset: 0x0000040D
    Public Sub AddError(location As SourceLocation, message As String, state As ParserState)
      If Errors.Count < 20 Then
        Errors.Add(New SyntaxError(location, message, state))
      End If
    End Sub

    ' Token: 0x0600000A RID: 10 RVA: 0x00002231 File Offset: 0x00000431
    Public Sub AddError(location As SourceLocation, message As String)
      AddError(location, message, Nothing)
    End Sub

    ' Token: 0x0600000B RID: 11 RVA: 0x0000223C File Offset: 0x0000043C
    Public Shared Function CreateDummy() As CompilerContext
      Return New CompilerContext(LanguageCompiler.CreateDummy())
    End Function

    ' Token: 0x0400004F RID: 79
    Public Compiler As LanguageCompiler

    ' Token: 0x04000050 RID: 80
    Public Errors As New SyntaxErrorList()

    ' Token: 0x04000051 RID: 81
    Public Values As New Dictionary(Of String, Object)()
  End Class
End Namespace
