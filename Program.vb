Imports System.IO

Public Module Program
  Private Const BASIC_CODE = "
10 DEF ENUM Elements{WOOD, FIRE, EARTH, METAL, WATER}
20
30 DEF STRUCT Vf2d(x!, y!)
40
50 DEF FN add#(a#, b#) = a + b
60
70 DEF FN element_name$(element%)
80 IF element = Elements.WOOD THEN RETURN ""wood""
90 IF element = Elements.FIRE THEN RETURN ""fire""
100 IF element = Elements.EARTH THEN RETURN ""earth""
110 IF element = Elements.METAL THEN RETURN ""metal""
120 IF element = Elements.WATER THEN RETURN ""water""
130 RETURN ""unknown""
140 END DEF
150
160 DEF SUB print_hello()
170 PRINT ""Hello, world!""
180 END DEF
190
200 PRINT add(2.5, 3.5)
210 PRINT element_name(Elements.WATER)
220 CALL print_hello()            
"

  Public Sub Main()
    Try
      ' Read the test BASIC file

      Console.WriteLine("BASIC Code:")
      Console.WriteLine(New String("="c, 50))
      Console.WriteLine(BASIC_CODE)
      Console.WriteLine(New String("="c, 50))
      Console.WriteLine()

      ' Use the JavaScriptGenerator to parse and generate JavaScript
      Dim result = JavaScriptGenerator.Generate(BASIC_CODE)

      If result.IsSuccessful Then
        Console.WriteLine("Generated JavaScript:")
        Console.WriteLine(New String("="c, 50))
        Console.WriteLine(result.ResultMessage)
        Console.WriteLine(New String("="c, 50))
        Console.WriteLine("Test completed successfully!")
      Else
        Console.WriteLine("Error:")
        Console.WriteLine(result.ResultMessage)
      End If

    Catch ex As Exception
      Console.WriteLine("Error: " & ex.Message)
      Console.WriteLine(ex.StackTrace)
    End Try

    Console.WriteLine()
    Console.Write("Press any key to continue...")
    Console.ReadKey()
  End Sub

End Module