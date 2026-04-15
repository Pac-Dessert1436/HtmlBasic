Namespace Nodes

  ''' <summary>
  ''' AST node for HTML element creation statements: SET btn = NEW Button
  ''' </summary>
  Friend Class HtmlElementStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property ElementType As String
    Public Property VariableName As Irony.Compiler.Token

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      
      ' Parse SET variable = NEW ElementType
      If args.ChildNodes.Count > 3 Then
        VariableName = CType(args.ChildNodes(1), Irony.Compiler.Token)
        
        Dim elementTypeToken = CType(args.ChildNodes(3), Irony.Compiler.Token)
        ElementType = elementTypeToken.Text
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      textWriter.Write($"var {VariableName.Text} = document.createElement('")
      
      ' Map HTML-BASIC element types to HTML tag names
      Select Case ElementType.ToLowerInvariant()
        Case "button"
          textWriter.Write("button")
        Case "label"
          textWriter.Write("label")
        Case "div"
          textWriter.Write("div")
        Case "h1", "h2", "h3", "h4", "h5", "h6"
          textWriter.Write(ElementType.ToLowerInvariant())
        Case "p"
          textWriter.Write("p")
        Case "span"
          textWriter.Write("span")
        Case "input"
          textWriter.Write("input")
        Case "form"
          textWriter.Write("form")
        Case "select"
          textWriter.Write("select")
        Case "option"
          textWriter.Write("option")
        Case "ul", "ol", "li"
          textWriter.Write(ElementType.ToLowerInvariant())
        Case "table", "tr", "td", "th"
          textWriter.Write(ElementType.ToLowerInvariant())
        Case "header", "footer", "nav", "section", "article", "aside"
          textWriter.Write(ElementType.ToLowerInvariant())
        Case "a"
          textWriter.Write("a")
        Case "img"
          textWriter.Write("img")
        Case "br"
          textWriter.Write("br")
        Case "hr"
          textWriter.Write("hr")
        Case "strong", "em", "code", "pre"
          textWriter.Write(ElementType.ToLowerInvariant())
        Case Else
          textWriter.Write("div") ' Default fallback
      End Select
      
      textWriter.Write("');")
      textWriter.WriteLine()
      textWriter.Write(context.IndentationText)
      textWriter.Write($"document.body.appendChild({VariableName.Text});")
    End Sub

  End Class

  ''' <summary>
  ''' AST node for element positioning: element.locate x, y
  ''' </summary>
  Friend Class LocateElementStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property ElementName As Irony.Compiler.Token
    Public Property XPosition As ExpressionNode
    Public Property YPosition As ExpressionNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      
      ' Parse element.locate x, y
      If args.ChildNodes.Count > 2 Then
        ElementName = CType(args.ChildNodes(0), Irony.Compiler.Token)
        XPosition = CType(args.ChildNodes(2), ExpressionNode)
        YPosition = CType(args.ChildNodes(4), ExpressionNode)
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      textWriter.Write($"{ElementName.Text}.style.position = 'absolute';")
      textWriter.WriteLine()
      textWriter.Write(context.IndentationText)
      textWriter.Write($"{ElementName.Text}.style.left = ")
      XPosition.GenerateJavaScript(context, textWriter)
      textWriter.Write(" + 'px';")
      textWriter.WriteLine()
      textWriter.Write(context.IndentationText)
      textWriter.Write($"{ElementName.Text}.style.top = ")
      YPosition.GenerateJavaScript(context, textWriter)
      textWriter.Write(" + 'px';")
    End Sub

  End Class

  ''' <summary>
  ''' AST node for element property setting: element.set_property value
  ''' </summary>
  Friend Class SetPropertyStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property ElementName As Irony.Compiler.Token
    Public Property PropertyName As String
    Public Property Value As ExpressionNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      
      ' Parse element.set_property value
      If args.ChildNodes.Count > 1 Then
        ElementName = CType(args.ChildNodes(0), Irony.Compiler.Token)
        
        Dim propertyToken = CType(args.ChildNodes(1), Irony.Compiler.Token)
        PropertyName = propertyToken.Text.ToLowerInvariant().Replace("set_", "")
        
        If args.ChildNodes.Count > 2 Then
          Value = CType(args.ChildNodes(2), ExpressionNode)
        End If
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      Select Case PropertyName
        Case "text"
          textWriter.Write($"{ElementName.Text}.textContent = ")
          Value.GenerateJavaScript(context, textWriter)
          textWriter.Write(";")
          
        Case "bgcolor", "backgroundcolor"
          textWriter.Write($"{ElementName.Text}.style.backgroundColor = ")
          Value.GenerateJavaScript(context, textWriter)
          textWriter.Write(";")
          
        Case "color"
          textWriter.Write($"{ElementName.Text}.style.color = ")
          Value.GenerateJavaScript(context, textWriter)
          textWriter.Write(";")
          
        Case "value"
          textWriter.Write($"{ElementName.Text}.value = ")
          Value.GenerateJavaScript(context, textWriter)
          textWriter.Write(";")
          
        Case Else
          ' Generic property setting
          textWriter.Write($"{ElementName.Text}.{PropertyName} = ")
          Value.GenerateJavaScript(context, textWriter)
          textWriter.Write(";")
      End Select
    End Sub

  End Class

  ''' <summary>
  ''' AST node for event handling: element.on_event SUB() ... END SUB
  ''' </summary>
  Friend Class EventHandlerStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property ElementName As Irony.Compiler.Token
    Public Property EventName As String
    Public Property HandlerBody As StatementListNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      
      ' Parse element.on_event SUB() ... END SUB
      If args.ChildNodes.Count > 1 Then
        ElementName = CType(args.ChildNodes(0), Irony.Compiler.Token)
        
        Dim eventToken = CType(args.ChildNodes(1), Irony.Compiler.Token)
        EventName = eventToken.Text.ToLowerInvariant().Replace("on_", "")
        
        If args.ChildNodes.Count > 2 Then
          HandlerBody = CType(args.ChildNodes(2), StatementListNode)
        End If
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      ' Map HTML-BASIC event names to JavaScript event names
      Dim jsEventName As String
      Select Case EventName
        Case "click"
          jsEventName = "click"
        Case "hover"
          jsEventName = "mouseover"
        Case "leave"
          jsEventName = "mouseout"
        Case "change"
          jsEventName = "change"
        Case "focus"
          jsEventName = "focus"
        Case "blur"
          jsEventName = "blur"
        Case Else
          jsEventName = EventName
      End Select
      
      textWriter.Write($"{ElementName.Text}.addEventListener('{jsEventName}', function(e) {{")
      textWriter.WriteLine()
      
      context.Indentation += 1
      textWriter.Write(context.IndentationText)
      
      If HandlerBody IsNot Nothing Then
        HandlerBody.GenerateJavaScript(context, textWriter)
      End If
      
      context.Indentation -= 1
      textWriter.Write(context.IndentationText)
      textWriter.Write("});")
    End Sub

  End Class

  ''' <summary>
  ''' AST node for MSGBOX function
  ''' </summary>
  Friend Class MsgBoxStmtNode
    Inherits GenericJsBasicNode
    Implements IJsBasicNode

    Public Property Message As ExpressionNode

    Public Sub New(args As Irony.Compiler.AstNodeArgs)
      MyBase.New(args)
      
      If args.ChildNodes.Count > 1 Then
        Message = CType(args.ChildNodes(1), ExpressionNode)
      End If
    End Sub

    Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
      textWriter.Write("alert(")
      Message.GenerateJavaScript(context, textWriter)
      textWriter.Write(");")
    End Sub

  End Class

End Namespace