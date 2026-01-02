' Syntax for defining a function or a subroutine in HTML-BASIC:
' DEF FN FunctionName(param1 AS TYPE1, param2 AS TYPE2, ...) AS ReturnType
' DEF SUB SubroutineName(param1 AS TYPE1, param2 AS TYPE2, ...)
' ReturnType can be omitted for functions that implicitly return a Variant type.
Namespace Nodes

  Friend Class DefFnStmtNode
    Inherits GenericJsBasicNode

    Public Property IsSub As Boolean
    Public Property FnName As Irony.Compiler.AstNode
    Public Property ParamList As Irony.Compiler.AstNode
    Public Property ReturnExpr As Irony.Compiler.AstNode
    Public Property Body As Irony.Compiler.AstNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      
      ' Determine if this is a function or subroutine
      For Each node In args.ChildNodes
        If TypeOf node Is Irony.Compiler.Token Then
          Dim token = CType(node, Irony.Compiler.Token)
          If token.Text.ToLower() = "sub" Then
            IsSub = True
            Exit For
          End If
        End If
      Next
      
      ' Parse function/subroutine name and parameters
      For i As Integer = 0 To args.ChildNodes.Count - 1
        If TypeOf args.ChildNodes(i) Is Irony.Compiler.Token Then
          Dim token = CType(args.ChildNodes(i), Irony.Compiler.Token)
          If token.Text.ToLower() = "fn" Or token.Text.ToLower() = "sub" Then
            ' Next node should be the name
            If i + 1 < args.ChildNodes.Count Then
              FnName = args.ChildNodes(i + 1)
            End If
            
            ' Find parameters (look for '(' and ')')
            For j As Integer = i + 2 To args.ChildNodes.Count - 1
              If TypeOf args.ChildNodes(j) Is Irony.Compiler.Token Then
                Dim paramToken = CType(args.ChildNodes(j), Irony.Compiler.Token)
                If paramToken.Text = "(" AndAlso j + 1 < args.ChildNodes.Count Then
                  ParamList = args.ChildNodes(j + 1)
                  Exit For
                End If
              End If
            Next
            
            ' Check for single-line function (with '=')
            For j As Integer = i + 2 To args.ChildNodes.Count - 1
              If TypeOf args.ChildNodes(j) Is Irony.Compiler.Token Then
                Dim eqToken = CType(args.ChildNodes(j), Irony.Compiler.Token)
                If eqToken.Text = "=" AndAlso j + 1 < args.ChildNodes.Count Then
                  ReturnExpr = args.ChildNodes(j + 1)
                  Exit For
                End If
              End If
            Next
            
            ' If no '=', look for body (multi-line function)
            If ReturnExpr Is Nothing Then
              For j As Integer = i + 3 To args.ChildNodes.Count - 1
                If Not (TypeOf args.ChildNodes(j) Is Irony.Compiler.Token AndAlso 
                       (CType(args.ChildNodes(j), Irony.Compiler.Token).Text = "end" Or 
                        CType(args.ChildNodes(j), Irony.Compiler.Token).Text = "def")) Then
                  Body = args.ChildNodes(j)
                  Exit For
                End If
              Next
            End If
            
            Exit For
          End If
        End If
      Next
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      textWriter.Write(context.IndentationText)
      textWriter.Write("function ")
      
      If FnName IsNot Nothing Then
        GeneratorHelper.GenerateNode(context, textWriter, FnName)
      Else
        textWriter.Write("AnonymousFn")
      End If
      
      textWriter.Write("(")
      
      If ParamList IsNot Nothing Then
        Dim first As Boolean = True
        For Each p As Irony.Compiler.AstNode In ParamList.ChildNodes
          If Not first Then textWriter.Write(", ")
          first = False
          ' Generate parameter name using GeneratorHelper
          GeneratorHelper.GenerateNode(context, textWriter, p)
        Next
      End If
      
      textWriter.WriteLine(") {")
      context.Indentation += 1
      
      If ReturnExpr IsNot Nothing Then
        ' Single-line function
        textWriter.Write(context.IndentationText)
        textWriter.Write("return ")
        GeneratorHelper.GenerateNode(context, textWriter, ReturnExpr)
        textWriter.WriteLine(";")
      ElseIf Body IsNot Nothing Then
        ' Multi-line function or subroutine
        GeneratorHelper.GenerateNode(context, textWriter, Body)
      End If
      
      context.Indentation -= 1
      textWriter.Write(context.IndentationText)
      textWriter.WriteLine("}")
    End Sub

  End Class

End Namespace