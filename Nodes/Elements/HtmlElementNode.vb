Imports Irony.Compiler

Namespace Nodes.Elements
    ''' <summary>
    ''' Base class for all HTML element nodes.
    ''' </summary>
    Friend MustInherit Class HtmlElementNode
        Inherits GenericJsBasicNode
        Implements IJsBasicNode

        Protected ReadOnly m_tagName As String
        Protected ReadOnly m_children As Irony.Compiler.AstNodeList

        Friend Sub New(args As Irony.Compiler.AstNodeArgs, tagName As String)
            MyBase.New(args)
            m_tagName = tagName
            m_children = args.ChildNodes
        End Sub

        Public Overrides Sub GenerateJavaScript(context As JsContext, textWriter As IO.TextWriter)
            ' Generate HTML element as JavaScript DOM creation code
            textWriter.Write($"document.createElement('{m_tagName}');")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML div element.
    ''' </summary>
    Friend Class DivNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "div")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML p element.
    ''' </summary>
    Friend Class PNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "p")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML span element.
    ''' </summary>
    Friend Class SpanNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "span")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML h1 element.
    ''' </summary>
    Friend Class H1Node
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "h1")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML h2 element.
    ''' </summary>
    Friend Class H2Node
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "h2")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML a (link) element.
    ''' </summary>
    Friend Class ANode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "a")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML img element.
    ''' </summary>
    Friend Class ImgNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "img")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML h3 element.
    ''' </summary>
    Friend Class H3Node
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "h3")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML h4 element.
    ''' </summary>
    Friend Class H4Node
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "h4")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML h5 element.
    ''' </summary>
    Friend Class H5Node
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "h5")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML h6 element.
    ''' </summary>
    Friend Class H6Node
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "h6")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML ul element.
    ''' </summary>
    Friend Class UlNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "ul")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML ol element.
    ''' </summary>
    Friend Class OlNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "ol")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML li element.
    ''' </summary>
    Friend Class LiNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "li")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML table element.
    ''' </summary>
    Friend Class TableNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "table")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML tr element.
    ''' </summary>
    Friend Class TrNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "tr")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML td element.
    ''' </summary>
    Friend Class TdNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "td")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML th element.
    ''' </summary>
    Friend Class ThNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "th")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML form element.
    ''' </summary>
    Friend Class FormNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "form")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML input element.
    ''' </summary>
    Friend Class InputNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "input")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML button element.
    ''' </summary>
    Friend Class ButtonNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "button")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML select element.
    ''' </summary>
    Friend Class SelectNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "select")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML option element.
    ''' </summary>
    Friend Class OptionNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "option")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML br element.
    ''' </summary>
    Friend Class BrNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "br")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML hr element.
    ''' </summary>
    Friend Class HrNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "hr")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML strong element.
    ''' </summary>
    Friend Class StrongNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "strong")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML em element.
    ''' </summary>
    Friend Class EmNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "em")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML code element.
    ''' </summary>
    Friend Class CodeNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "code")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML pre element.
    ''' </summary>
    Friend Class PreNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "pre")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML header element.
    ''' </summary>
    Friend Class HeaderNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "header")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML footer element.
    ''' </summary>
    Friend Class FooterNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "footer")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML nav element.
    ''' </summary>
    Friend Class NavNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "nav")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML section element.
    ''' </summary>
    Friend Class SectionNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "section")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML article element.
    ''' </summary>
    Friend Class ArticleNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "article")
        End Sub
    End Class

    ''' <summary>
    ''' Node for HTML aside element.
    ''' </summary>
    Friend Class AsideNode
        Inherits HtmlElementNode

        Friend Sub New(args As Irony.Compiler.AstNodeArgs)
            MyBase.New(args, "aside")
        End Sub
    End Class
End Namespace