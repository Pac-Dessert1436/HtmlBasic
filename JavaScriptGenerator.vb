Imports HtmlBasic.Nodes

''' <summary>
''' Generates JavaScript code from BASIC source code.
''' </summary>
Public Module JavaScriptGenerator

    ''' <summary>
    ''' Generates JavaScript based on BASIC source code, and returns a
    ''' CopmileResult object containing the compiled source code if
    ''' successful, or otherwise error messages.
    ''' </summary>
    Public Function Generate(sourceCode As String) As CompileResult

      ' Create a BASIC compiler
      Dim basicGrammer = New BasicGrammar
      Dim compiler = New Irony.Compiler.LanguageCompiler(basicGrammer)

      ' Compile the source code into an Abstract Syntax Tree.
      Dim rootNode As ProgramNode
      Try
        rootNode = CType(compiler.Parse(sourceCode & Environment.NewLine), ProgramNode)
      Catch bsee As BasicSyntaxErrorException
        rootNode = Nothing
        compiler.Context.Errors.Add(New Irony.Compiler.SyntaxError(New Irony.Compiler.SourceLocation, bsee.Message, Nothing))
      End Try
      If rootNode Is Nothing OrElse compiler.Context.Errors.Count > 0 Then
        ' Didn't compile.  Generate an error message.
        Dim [error] = compiler.Context.Errors(0)
        Dim location = String.Empty
        If [error].Location.Line > 0 AndAlso [error].Location.Column > 0 Then
          location = $"Line {[error].Location.Line + 1}, column {[error].Location.Column + 1}"
        End If
        Dim message = $"{location}: { [error].Message }:{Environment.NewLine}{sourceCode.Split(vbLf)([error].Location.Line)}"
        ' Return failure.
        Return New CompileResult() With {.IsSuccessful = False, .ResultMessage = message}
      End If

      ' Set up the types of lines (e.g. whether a given line is the first, last,
      ' or internal line of a function) and get the starting function to call.
      Dim firstFunctionName = SetLineTypes(rootNode)

      ' Now generate JavaScript from an abstract syntax tree.
      Dim code As String
      Dim sb = New Text.StringBuilder
      Using tw = New IO.StringWriter(sb, Globalization.CultureInfo.InvariantCulture)
        rootNode.GenerateJavaScript(New JsContext, tw)
        code = sb.ToString()
      End Using

      ' Return the JavaScript code.
      Return New CompileResult() With {.IsSuccessful = True,
                                       .ResultMessage = "Successfully compiled in " & compiler.CompileTime & "ms",
                                       .JavaScript = code,
                                       .StartFunction = firstFunctionName}

    End Function

    Private Function SetLineTypes(rootNode As ProgramNode) As String

      ' Get all the destination line numbers of Branch statements (goto, gosub, and return statements).
      Dim branchLineNumbers = From p In rootNode.DepthFirstTraversal()
                              Where TypeOf p Is BranchStmtNode
                              Select CType(p, BranchStmtNode).DestinationLine

      ' Get all the Line nodes.
      Dim allLines = From p In rootNode.DepthFirstTraversal()
                     Where TypeOf p Is LineNode
                     Select CType(p, LineNode)

      ' Figure out the branching, i.e. creating javascript 'return' statements.
      Dim previous As LineNode = Nothing
      For Each line In allLines
        If branchLineNumbers.Contains(line.LineNumber) Then
          line.LineTypes = line.LineTypes Xor LineTypes.InternalLine
          line.LineTypes = line.LineTypes Or LineTypes.FunctionStart
          If previous IsNot Nothing Then
            previous.LineTypes = previous.LineTypes Xor LineTypes.InternalLine
            previous.LineTypes = previous.LineTypes Or LineTypes.FunctionEnd
            ' add a return statement to the current node from the previous node, unless the
            ' statement on the previous line was a goto.
            Dim branchQuery = From p In previous.DepthFirstTraversal()
                              Where TypeOf p Is BranchStmtNode
                              Select CType(p, BranchStmtNode)
            Dim previousStmt = branchQuery.FirstOrDefault()
            If previousStmt Is Nothing OrElse previousStmt.BranchType = BranchType.[Gosub] Then
              previous.ReturnText = $"return line{line.LineNumber};"
            End If
          End If
        End If
        previous = line
      Next
      If previous IsNot Nothing Then
        previous.LineTypes = previous.LineTypes Xor LineTypes.InternalLine
        previous.LineTypes = previous.LineTypes Or LineTypes.FunctionEnd
      End If

      ' Get the first line, which corresponds to the first function to call.
      Dim firstLine = allLines.First()
      firstLine.LineTypes = firstLine.LineTypes Or LineTypes.FunctionStart
      allLines.Last().LineTypes = allLines.Last().LineTypes Or LineTypes.FunctionEnd

      Return $"line{firstLine.LineNumber}"

    End Function

  End Module

  Public Class CompileResult

    Public Property IsSuccessful As Boolean
    Public Property JavaScript As String
    Public Property StartFunction As String
    Public Property ResultMessage As String

End Class