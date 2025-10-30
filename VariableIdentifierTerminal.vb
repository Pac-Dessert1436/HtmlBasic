
''' <summary>
''' Matches tokens such as "a$", i.e. BASIC variables.
''' </summary>
Friend Class VariableIdentifierTerminal
  Inherits Irony.Compiler.IdentifierTerminal

  Shared ReadOnly IdentifierPattern As New Text.RegularExpressions.Regex("^[A-Za-z][A-Za-z0-9]*[$%!&#]?$")

  Public Sub New()
    MyBase.New("VARIABLE", "$%!&#", Nothing)
  End Sub

  Public Overrides Function TryMatch(context As Irony.Compiler.CompilerContext, source As Irony.Compiler.ISourceStream) As Irony.Compiler.Token
    Dim token = MyBase.TryMatch(context, source)
    Dim tokenIsNotNothing = token IsNot Nothing
    Dim isMatch = IdentifierPattern.IsMatch(token.Text)
    Dim isKeyword = token.IsKeyword
    If token IsNot Nothing AndAlso (IdentifierPattern.IsMatch(token.Text) AndAlso Not token.IsKeyword) Then
      Return token
    Else
      Return Nothing
    End If
  End Function

End Class