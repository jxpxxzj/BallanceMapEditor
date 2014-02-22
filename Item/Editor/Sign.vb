Imports System.ComponentModel
Imports SharpDX

Namespace Item.Editor
    Public Class Sign
        Inherits Item
        <Category("外观")> <DescriptionAttribute("标记文字")>
        Public Property Name As String
        <Category("外观")> <DescriptionAttribute("标记颜色")>
        Public Property BrushColor As SharpDX.Color4 = SharpDX.Color.Red
        <Category("外观")> <DescriptionAttribute("标记文字字体")>
        Private Property Font As Font = New Font(New FontFamily("宋体"), 15, FontStyle.Bold)
        Public Sub New()

        End Sub
        Public Sub New(ByVal nam As String, ByVal loca As System.Drawing.Point)
            Name = nam
            Position = loca
        End Sub
        Private _textFactory As New DirectWrite.Factory
        Public Overrides Sub Draw(ByVal e As Direct2D1.RenderTarget)
            Dim mBrush As New Direct2D1.SolidColorBrush(e, BrushColor)
            Dim mTextFormat = New DirectWrite.TextFormat(_textFactory, Font.FontFamily.Name, Font.Size + 5, DirectWrite.FontStyle.Normal, Font.Size)
            e.FillEllipse(New Direct2D1.Ellipse(New Vector2(Position.X, Position.Y), 8, 8), mBrush)
            e.DrawText(Name & " [X:" & Position.X & " Y:" & Position.Y & "]", mTextFormat, New SharpDX.RectangleF(Position.X + 12, Position.Y - 10, 1000, 100), mBrush)
        End Sub
    End Class
End Namespace