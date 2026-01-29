Namespace Irony.Compiler

  ''' <summary>
  ''' Complex64 represents a complex number using 64-bit floating point precision.
  ''' This class provides compatibility for .NET 10.0.
  ''' </summary>
  Public Structure Complex64
    Private _real As Double
    Private _imag As Double

    Public Sub New(real As Double, imag As Double)
      _real = real
      _imag = imag
    End Sub

    Public Property Real As Double
      Get
        Return _real
      End Get
      Set(value As Double)
        _real = value
      End Set
    End Property

    Public Property Imag As Double
      Get
        Return _imag
      End Get
      Set(value As Double)
        _imag = value
      End Set
    End Property

    Public Overrides Function ToString() As String
      If _imag >= 0 Then
        Return $"{_real}+{_imag}i"
      Else
        Return $"{_real}{_imag}i"
      End If
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
      If Not TypeOf obj Is Complex64 Then Return False
      Dim other = DirectCast(obj, Complex64)
      Return _real = other._real AndAlso _imag = other._imag
    End Function

    Public Overrides Function GetHashCode() As Integer
      Return HashCode.Combine(_real, _imag)
    End Function
  End Structure

End Namespace
