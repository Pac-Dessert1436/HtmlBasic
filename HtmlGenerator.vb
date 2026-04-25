Imports HtmlBasic.Nodes

''' <summary>
''' Generates complete HTML pages from HTML-BASIC source code.
''' </summary>
Public Module HtmlGenerator

  ''' <summary>
  ''' Generates a complete HTML page from BASIC source code.
  ''' </summary>
  Public Function GenerateHtmlPage(sourceCode As String, Optional title As String = "HTML-BASIC Application") As String
    ' First, generate the JavaScript code
    Dim jsResult = JavaScriptGenerator.Generate(sourceCode)
    
    If Not jsResult.IsSuccessful Then
      Throw New Exception("Failed to compile BASIC code: " & jsResult.ResultMessage)
    End If
    
    ' Create the HTML page
    Dim html As New Text.StringBuilder()
    
    ' HTML header
    html.AppendLine("<!DOCTYPE html>")
    html.AppendLine("<html>")
    html.AppendLine("<head>")
    html.AppendLine("  <meta charset='UTF-8'>")
    html.AppendLine("  <meta name='viewport' content='width=device-width, initial-scale=1.0'>")
    html.AppendLine($"  <title>{title}</title>")
    html.AppendLine("  <style>")
    html.AppendLine("    body {")
    html.AppendLine("      font-family: Arial, sans-serif;")
    html.AppendLine("      margin: 20px;")
    html.AppendLine("    }")
    html.AppendLine("    /* Console output styles */")
    html.AppendLine("    #console-output {")
    html.AppendLine("      background-color: #f0f0f0;")
    html.AppendLine("      border: 1px solid #ccc;")
    html.AppendLine("      padding: 10px;")
    html.AppendLine("      margin: 10px 0;")
    html.AppendLine("      font-family: 'Courier New', monospace;")
    html.AppendLine("      white-space: pre-wrap;")
    html.AppendLine("    }")
    html.AppendLine("  </style>")
    html.AppendLine("</head>")
    html.AppendLine("<body>")
    html.AppendLine($"  <h1>{title}</h1>")
    html.AppendLine("  <div id='console-output'></div>")
    html.AppendLine()
    html.AppendLine("  <script>")
    
    ' Add console helper functions
    html.AppendLine("    // Console output functions")
    html.AppendLine("    const consoleOutput = document.getElementById('console-output');")
    html.AppendLine("    ")
    html.AppendLine("    function console.println(...args) {")
    html.AppendLine("      const output = args.map(arg => {")
    html.AppendLine("        if (typeof arg === 'string') return arg;")
    html.AppendLine("        if (typeof arg === 'number') return arg.toString();")
    html.AppendLine("        return String(arg);")
    html.AppendLine("      }).join(' ');")
    html.AppendLine("      consoleOutput.textContent += output + '\\n';")
    html.AppendLine("      console.log(output);")
    html.AppendLine("    }")
    html.AppendLine("    ")
    html.AppendLine("    function console.input(prompt) {")
    html.AppendLine("      return prompt(prompt || '');")
    html.AppendLine("    }")
    html.AppendLine("    ")
    html.AppendLine("    function console.setCursorPos(row, col) {")
    html.AppendLine("      console.log(`Cursor position: ${row}, ${col}`);")
    html.AppendLine("    }")
    html.AppendLine("    ")
    html.AppendLine("    // Boolean to integer conversion")
    html.AppendLine("    function _boolToInt(expression) {")
    html.AppendLine("      return expression ? -1 : 0;")
    html.AppendLine("    }")
    html.AppendLine("    ")
    
    ' Add the generated JavaScript code
    html.AppendLine("    // Generated JavaScript code")
    html.AppendLine("    " & jsResult.JavaScript.Replace(vbLf, vbLf & "    "))
    html.AppendLine()
    
    ' Start the program
    html.AppendLine("    // Start the program")
    html.AppendLine($"    {jsResult.StartFunction}();")
    html.AppendLine("  </script>")
    html.AppendLine("</body>")
    html.AppendLine("</html>")
    
    Return html.ToString()
  End Function

  ''' <summary>
  ''' Generates an HTML page and saves it to a file.
  ''' </summary>
  Public Sub GenerateHtmlFile(sourceCode As String, outputPath As String, Optional title As String = "HTML-BASIC Application")
    Dim htmlContent = GenerateHtmlPage(sourceCode, title)
    IO.File.WriteAllText(outputPath, htmlContent, System.Text.Encoding.UTF8)
  End Sub

End Module