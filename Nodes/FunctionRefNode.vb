Namespace Nodes

  ''' <summary>
  ''' AST node for first-class function references with @ suffix
  ''' </summary>
  Friend Class FunctionRefNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property FunctionName As Irony.Compiler.Token

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      If args.ChildNodes.Count > 0 Then
        FunctionName = CType(args.ChildNodes(0), Irony.Compiler.Token)
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      ' Remove the @ suffix for JavaScript
      Dim jsFunctionName = FunctionName.Text.TrimEnd("@"c)
      textWriter.Write(jsFunctionName)
    End Sub

  End Class

  ''' <summary>
  ''' AST node for lambda expressions (single-line anonymous functions)
  ''' </summary>
  Friend Class LambdaExprNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property Parameters As List(Of Irony.Compiler.Token)
    Public Property Body As ExpressionNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      Parameters = New List(Of Irony.Compiler.Token)()
      
      ' Parse lambda expression: FN(x) x * 2
      If args.ChildNodes.Count > 1 Then
        ' Parse parameters (inside parentheses)
        Dim paramList = args.ChildNodes(1)
        If paramList.ChildNodes IsNot Nothing Then
          For Each param In paramList.ChildNodes
            If TypeOf param Is Irony.Compiler.Token Then
              Parameters.Add(CType(param, Irony.Compiler.Token))
            End If
          Next
        End If
        
        ' Parse body expression
        If args.ChildNodes.Count > 2 Then
          Body = CType(args.ChildNodes(2), ExpressionNode)
        End If
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      textWriter.Write("(")
      
      ' Generate parameters
      For i As Integer = 0 To Parameters.Count - 1
        If i > 0 Then textWriter.Write(", ")
        textWriter.Write(Parameters(i).Text)
      Next
      
      textWriter.Write(") => ")
      
      ' Generate body
      If Body IsNot Nothing Then
        Body.GenerateJavaScript(context, textWriter)
      End If
    End Sub

  End Class

  ''' <summary>
  ''' AST node for function calls with function references
  ''' </summary>
  Friend Class FunctionCallNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property FunctionRef As IJsBasicNode
    Public Property Arguments As List(Of ExpressionNode)

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      Arguments = New List(Of ExpressionNode)()
      
      ' Parse function reference and arguments
      If args.ChildNodes.Count > 0 Then
        FunctionRef = CType(args.ChildNodes(0), IJsBasicNode)
        
        ' Parse arguments
        If args.ChildNodes.Count > 2 Then
          Dim argList = args.ChildNodes(2)
          If argList.ChildNodes IsNot Nothing Then
            For Each arg In argList.ChildNodes
              If TypeOf arg Is ExpressionNode Then
                Arguments.Add(CType(arg, ExpressionNode))
              End If
            Next
          End If
        End If
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      ' Generate function reference
      FunctionRef.GenerateJavaScript(context, textWriter)
      
      textWriter.Write("(")
      
      ' Generate arguments
      For i As Integer = 0 To Arguments.Count - 1
        If i > 0 Then textWriter.Write(", ")
        Arguments(i).GenerateJavaScript(context, textWriter)
      Next
      
      textWriter.Write(")")
    End Sub

  End Class

End Namespace