Namespace Nodes

  ''' <summary>
  ''' AST node for ON ERROR GOTO error handling statements
  ''' </summary>
  Friend Class OnErrorStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property ErrorHandlerLine As Integer
    Public Property ErrorHandlerType As ErrorHandlerType

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      
      ' Parse ON ERROR GOTO <line> or ON ERROR RESUME NEXT
      If args.ChildNodes.Count > 2 Then
        Dim errorCommand = CType(args.ChildNodes(2), Irony.Compiler.Token)
        
        If errorCommand.Text.ToLowerInvariant() = "goto" Then
          ErrorHandlerType = ErrorHandlerType.GotoLine
          If args.ChildNodes.Count > 3 Then
            Dim lineToken = CType(args.ChildNodes(3), Irony.Compiler.Token)
            ErrorHandlerLine = Integer.Parse(lineToken.Text)
          End If
        ElseIf errorCommand.Text.ToLowerInvariant() = "resume" Then
          ErrorHandlerType = ErrorHandlerType.ResumeNext
        End If
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      Select Case ErrorHandlerType
        Case ErrorHandlerType.GotoLine
          textWriter.Write($"_errorHandler = function() {{ return line{ErrorHandlerLine}; }};")
        Case ErrorHandlerType.ResumeNext
          textWriter.Write("_errorHandler = function() { return _nextLine; };")
      End Select
    End Sub

  End Class

  ''' <summary>
  ''' AST node for RESUME NEXT statement
  ''' </summary>
  Friend Class ResumeStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      textWriter.Write("return _nextLine;")
    End Sub

  End Class

  ''' <summary>
  ''' AST node for ERR variable (error information)
  ''' </summary>
  Friend Class ErrVariableNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      textWriter.Write("_lastError")
    End Sub

  End Class

  Public Enum ErrorHandlerType
    GotoLine
    ResumeNext
  End Enum

End Namespace