Namespace Nodes

  ''' <summary>
  ''' AST node for M_LET member properties in structs
  ''' </summary>
  Friend Class MemberPropertyNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property StructName As Irony.Compiler.Token
    Public Property PropertyName As Irony.Compiler.Token
    Public Property InitialValue As ExpressionNode
    Public Property IsReadOnly As Boolean

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      
      ' Parse M_LET StructName.PropertyName = value
      If args.ChildNodes.Count > 1 Then
        StructName = CType(args.ChildNodes(1), Irony.Compiler.Token)
        
        If args.ChildNodes.Count > 2 Then
          Dim propertyToken = CType(args.ChildNodes(2), Irony.Compiler.Token)
          PropertyName = propertyToken
          
          ' Check if this is a KEY (read-only) property
          If args.ChildNodes.Count > 0 Then
            Dim firstToken = CType(args.ChildNodes(0), Irony.Compiler.Token)
            IsReadOnly = firstToken.Text.ToLowerInvariant() = "key"
          End If
          
          ' Parse initial value if present
          If args.ChildNodes.Count > 4 Then
            InitialValue = CType(args.ChildNodes(4), ExpressionNode)
          End If
        End If
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      ' Generate property definition in struct constructor
      textWriter.Write($"this.{PropertyName.Text} = ")
      
      If InitialValue IsNot Nothing Then
        InitialValue.GenerateJavaScript(context, textWriter)
      Else
        textWriter.Write("null")
      End If
      
      textWriter.Write(";")
      
      If IsReadOnly Then
        ' For read-only properties, make them non-writable
        textWriter.WriteLine()
        textWriter.Write(context.IndentationText)
        textWriter.Write($"Object.defineProperty(this, '{PropertyName.Text}', {{ writable: false }});")
      End If
    End Sub

  End Class

  ''' <summary>
  ''' AST node for member method definitions in structs (M_FN, M_SUB)
  ''' </summary>
  Friend Class MemberMethodNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property StructName As Irony.Compiler.Token
    Public Property MethodName As Irony.Compiler.Token
    Public Property Parameters As List(Of Irony.Compiler.Token)
    Public Property Body As StatementListNode
    Public Property IsFunction As Boolean

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      Parameters = New List(Of Irony.Compiler.Token)()
      
      ' Parse M_FN StructName.MethodName(params) = expr or M_SUB StructName.MethodName(params) body
      If args.ChildNodes.Count > 1 Then
        StructName = CType(args.ChildNodes(1), Irony.Compiler.Token)
        
        If args.ChildNodes.Count > 2 Then
          MethodName = CType(args.ChildNodes(2), Irony.Compiler.Token)
          
          ' Determine if this is a function or subroutine
          Dim firstToken = CType(args.ChildNodes(0), Irony.Compiler.Token)
          IsFunction = firstToken.Text.ToLowerInvariant() = "m_fn"
          
          ' Parse parameters
          If args.ChildNodes.Count > 3 Then
            Dim paramList = args.ChildNodes(3)
            If paramList.ChildNodes IsNot Nothing Then
              For Each param In paramList.ChildNodes
                If TypeOf param Is Irony.Compiler.Token Then
                  Parameters.Add(CType(param, Irony.Compiler.Token))
                End If
              Next
            End If
          End If
          
          ' Parse body or expression
          If args.ChildNodes.Count > 4 Then
            If IsFunction Then
              ' Single expression for M_FN
              Body = New StatementListNode(New Irony.Compiler.AstNodeArgs(Nothing, Nothing, Nothing, New Irony.Compiler.AstNodeList()))
              Body.ChildNodes.Add(args.ChildNodes(4))
            Else
              ' Statement list for M_SUB
              Body = CType(args.ChildNodes(4), StatementListNode)
            End If
          End If
        End If
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      ' Generate method definition in struct prototype
      textWriter.Write($"{StructName.Text}.prototype.{MethodName.Text} = function(")
      
      ' Generate parameters
      For i As Integer = 0 To Parameters.Count - 1
        If i > 0 Then textWriter.Write(", ")
        textWriter.Write(Parameters(i).Text)
      Next
      
      textWriter.WriteLine(") {")
      
      context.Indentation += 1
      textWriter.Write(context.IndentationText)
      
      If IsFunction Then
        textWriter.Write("return ")
      End If
      
      If Body IsNot Nothing Then
        Body.GenerateJavaScript(context, textWriter)
      End If
      
      If IsFunction Then
        textWriter.Write(";")
      End If
      
      textWriter.WriteLine()
      context.Indentation -= 1
      textWriter.Write(context.IndentationText)
      textWriter.Write("};")
    End Sub

  End Class

End Namespace