' Syntax for defining a structure in HTML-BASIC:
' DEF STRUCT StructName(member1 AS TYPE1, member2 AS TYPE2, ...)
' The `KEY` keyword is used to define a read-only member.
Namespace Nodes

  Friend Class DefStructStmtNode
    Inherits GenericJsBasicNode

    Public Property StructName As Irony.Compiler.AstNode
    Public Property MemberList As Irony.Compiler.AstNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      ' Typical child layout: DEF STRUCT <name> ( <members> )
      StructName = If(args.ChildNodes.Count > 1, args.ChildNodes(1), Nothing)
      MemberList = If(args.ChildNodes.Count > 3, args.ChildNodes(3), Nothing)
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      ' Emit a JS constructor for the struct; initialize members to null.
      textWriter.Write(context.IndentationText)
      textWriter.Write("function ")
      If StructName IsNot Nothing Then
        GeneratorHelper.GenerateNode(context, textWriter, StructName)
      Else
        textWriter.Write("AnonymousStruct")
      End If
      textWriter.Write("() {")
      textWriter.WriteLine()

      If MemberList IsNot Nothing AndAlso MemberList.ChildNodes IsNot Nothing Then
        For Each m As Irony.Compiler.AstNode In MemberList.ChildNodes
          textWriter.Write(context.IndentationText)
          textWriter.Write(context.IndentationText)
          textWriter.Write("this.")
          ' Generate member name using GeneratorHelper
          GeneratorHelper.GenerateNode(context, textWriter, m)
          textWriter.WriteLine(" = null;")
        Next
      End If

      textWriter.Write(context.IndentationText)
      textWriter.WriteLine("}")
    End Sub

  End Class

End Namespace