Namespace Nodes

  ''' <summary>
  ''' AST node for array operations: DIM, REDIM, ERASE
  ''' </summary>
  Friend Class ArrayStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property ArrayOperation As ArrayOperationType
    Public Property ArrayName As Irony.Compiler.Token
    Public Property Dimensions As List(Of ExpressionNode)
    Public Property ArrayLiteral As ArrayLiteralNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      Dimensions = New List(Of ExpressionNode)()
      
      ' Parse the array operation type
      Dim operationToken = CType(args.ChildNodes(0), Irony.Compiler.Token)
      Select Case operationToken.Text.ToLowerInvariant()
        Case "dim"
          ArrayOperation = ArrayOperationType.Dim
        Case "redim"
          ArrayOperation = ArrayOperationType.Redim
        Case "erase"
          ArrayOperation = ArrayOperationType.Erase
      End Select
      
      ' Parse array name and dimensions
      If args.ChildNodes.Count > 1 Then
        ArrayName = CType(args.ChildNodes(1), Irony.Compiler.Token)
        
        ' Check if this is an array literal assignment
        If args.ChildNodes.Count > 3 AndAlso TypeOf args.ChildNodes(3) Is ArrayLiteralNode Then
          ArrayLiteral = CType(args.ChildNodes(3), ArrayLiteralNode)
        Else
          ' Parse dimensions (if any)
          For i As Integer = 2 To args.ChildNodes.Count - 1
            Dim child = args.ChildNodes(i)
            If TypeOf child Is ExpressionNode Then
              Dimensions.Add(CType(child, ExpressionNode))
            End If
          Next
        End If
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      Select Case ArrayOperation
        Case ArrayOperationType.Dim
          If ArrayLiteral IsNot Nothing Then
            ' Array literal assignment: DIM arr = {1, 2, 3}
            textWriter.Write($"var {ArrayName.Text} = ")
            ArrayLiteral.GenerateJavaScript(context, textWriter)
            textWriter.Write(";")
          ElseIf Dimensions.Count > 0 Then
            ' Array declaration with dimensions: DIM arr(10)
            textWriter.Write($"var {ArrayName.Text} = new Array(")
            For i As Integer = 0 To Dimensions.Count - 1
              If i > 0 Then textWriter.Write(", ")
              Dimensions(i).GenerateJavaScript(context, textWriter)
            Next
            textWriter.Write(");")
          Else
            ' Simple array declaration: DIM arr
            textWriter.Write($"var {ArrayName.Text} = [];")
          End If
          
        Case ArrayOperationType.Redim
          ' REDIM arr(20)
          textWriter.Write($"{ArrayName.Text} = new Array(")
          For i As Integer = 0 To Dimensions.Count - 1
            If i > 0 Then textWriter.Write(", ")
            Dimensions(i).GenerateJavaScript(context, textWriter)
          Next
          textWriter.Write(");")
          
        Case ArrayOperationType.Erase
          ' ERASE arr
          textWriter.Write($"{ArrayName.Text} = [];")
      End Select
    End Sub

  End Class

  ''' <summary>
  ''' AST node for array literals using brace notation: {1, 2, 3}
  ''' </summary>
  Friend Class ArrayLiteralNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property Elements As List(Of ExpressionNode)

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      Elements = New List(Of ExpressionNode)()
      
      ' Parse array elements
      For Each child In args.ChildNodes
        If TypeOf child Is ExpressionNode Then
          Elements.Add(CType(child, ExpressionNode))
        End If
      Next
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      textWriter.Write("[")
      For i As Integer = 0 To Elements.Count - 1
        If i > 0 Then textWriter.Write(", ")
        Elements(i).GenerateJavaScript(context, textWriter)
      Next
      textWriter.Write("]")
    End Sub

  End Class

  ''' <summary>
  ''' AST node for array method calls: insert, append, delete, map
  ''' </summary>
  Friend Class ArrayMethodNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property ArrayName As Irony.Compiler.Token
    Public Property MethodName As String
    Public Property Arguments As List(Of ExpressionNode)

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      Arguments = New List(Of ExpressionNode)()
      
      ' Parse array name and method call
      If args.ChildNodes.Count > 0 Then
        ArrayName = CType(args.ChildNodes(0), Irony.Compiler.Token)
        
        If args.ChildNodes.Count > 1 Then
          Dim methodToken = CType(args.ChildNodes(1), Irony.Compiler.Token)
          MethodName = methodToken.Text.ToLowerInvariant()
          
          ' Parse arguments
          For i As Integer = 2 To args.ChildNodes.Count - 1
            Dim child = args.ChildNodes(i)
            If TypeOf child Is ExpressionNode Then
              Arguments.Add(CType(child, ExpressionNode))
            End If
          Next
        End If
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      textWriter.Write($"{ArrayName.Text}.")
      
      Select Case MethodName
        Case "insert"
          textWriter.Write("splice(")
          Arguments(0).GenerateJavaScript(context, textWriter)
          textWriter.Write(", 0, ")
          Arguments(1).GenerateJavaScript(context, textWriter)
          textWriter.Write(")")
          
        Case "append"
          textWriter.Write("push(")
          Arguments(0).GenerateJavaScript(context, textWriter)
          textWriter.Write(")")
          
        Case "delete"
          textWriter.Write("splice(")
          textWriter.Write($"{ArrayName.Text}.indexOf(")
          Arguments(0).GenerateJavaScript(context, textWriter)
          textWriter.Write("), 1)")
          
        Case "map"
          textWriter.Write("map(")
          Arguments(0).GenerateJavaScript(context, textWriter)
          textWriter.Write(")")
      End Select
    End Sub

  End Class

  Public Enum ArrayOperationType As Short
    [Dim] = 0
    [Redim] = 1
    [Erase] = 2
  End Enum

End Namespace