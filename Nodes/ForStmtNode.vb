Namespace Nodes

  Friend Class ForStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Private ReadOnly m_assignment As AssignStmtNode
    Private ReadOnly m_upperBound As ExpressionNode
    Private ReadOnly m_step As Integer

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      m_assignment = CType(args.ChildNodes(1), AssignStmtNode)
      m_upperBound = CType(args.ChildNodes(3), ExpressionNode)
      If args.ChildNodes.Count = 6 Then
        m_step = Integer.Parse(CType(args.ChildNodes(5), Irony.Compiler.Token).Text, Globalization.CultureInfo.InvariantCulture)
      Else
        m_step = 1
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)

      context.Indentation += 1
      textWriter.Write("for (var ")
      m_assignment.GenerateJavaScript(context, textWriter)
      textWriter.Write(" ")

      GeneratorHelper.GenerateNode(context, textWriter, m_assignment.Variable)

      If m_step > 0 Then
        textWriter.Write("<=")
      ElseIf m_step < 0 Then
        textWriter.Write(">=")
      Else
        Throw New BasicSyntaxErrorException("A step amount of 0 is not allowed.")
      End If

      m_upperBound.GenerateJavaScript(context, textWriter)
      textWriter.Write("; ")
      GeneratorHelper.GenerateNode(context, textWriter, m_assignment.Variable)
      textWriter.Write(" += " & m_step)

      textWriter.Write(") {")

    End Sub

  End Class

End Namespace