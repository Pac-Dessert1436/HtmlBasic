Friend Module Utilities

    ''' <summary>
    ''' An iterator to to a depth-first traversal of an Abstract-Syntax Tree.
    ''' Very useful when combined with LINQ-to-Objects.
    ''' </summary>
    <Runtime.CompilerServices.Extension()>
    Public Iterator Function DepthFirstTraversal(startNode As Irony.Compiler.AstNode) As IEnumerable(Of Irony.Compiler.AstNode)
      Yield startNode
      Dim childBearer = TryCast(startNode, GenericJsBasicNode)
      If childBearer IsNot Nothing Then
        For Each child In childBearer.ChildNodes
          If TypeOf child Is GenericJsBasicNode Then
            For Each item In DepthFirstTraversal(child)
              Yield item
            Next
          Else
            Yield child
          End If
        Next
      End If
    End Function

  End Module

  ''' <summary>
  ''' An error in some BASIC source code.
  ''' </summary>
  Public Class BasicSyntaxErrorException
    Inherits Exception

    Public Sub New()
    End Sub

    Public Sub New(message As String)
      MyBase.New(message)
    End Sub

    Public Sub New(message As String, inner As Exception)
      MyBase.New(message, inner)
    End Sub

    Protected Sub New(info As Runtime.Serialization.SerializationInfo,
                      context As Runtime.Serialization.StreamingContext)
      MyBase.New(info, context)
    End Sub

End Class