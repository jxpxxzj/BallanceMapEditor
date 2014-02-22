Imports System.ComponentModel
Imports SharpDX

Namespace UI
    Public Class GridView
        Public Pen As Pen = New Pen(Brushes.Black, 1)
        <Category("外观")> <DescriptionAttribute("显示网格")>
        Public Property DrawGrid As Boolean = True
        <Category("外观")> <DescriptionAttribute("网格颜色")>
        Public Property PenColor As System.Drawing.Color
            Get
                Return Pen.Color
            End Get
            Set(ByVal value As System.Drawing.Color)
                Pen = New Pen(value, 1)
            End Set
        End Property


        <BrowsableAttribute(False)>
        Public Property Grid As Size

        <Category("属性")> <DescriptionAttribute("画布格数")>
        Public Property CanvasCount As Size

        <Category("外观")> <DescriptionAttribute("绘制网格编号")>
        Public Property DrawGridNumber As Boolean = True
        <Category("外观")> <DescriptionAttribute("网格编号字体")>
        Public Property GridNumberFont As Font = New Font(New FontFamily("宋体"), 12, FontStyle.Regular)

        Public Sub New(ByVal co As System.Drawing.Color, ByVal drs As Size, ByVal canv As Size, ByVal gn As Boolean)
            Pen = New Pen(co, 1)
            Grid = drs
            CanvasCount = canv
            DrawGridNumber = gn

        End Sub
        Private _textFactory As New DirectWrite.Factory
        Public Sub Draw(ByRef e As Direct2D1.RenderTarget)

            If DrawGrid = True Then
                Dim mBrush As New Direct2D1.SolidColorBrush(_renderTarget, New Color4(New Color3(Pen.Color.R / 255, Pen.Color.G / 255, Pen.Color.B / 255)))
                Dim mTextFormat = New DirectWrite.TextFormat(_textFactory, GridNumberFont.FontFamily.Name, GridNumberFont.Size)

                For i = 0 To CanvasCount.Height + 1
                    e.DrawLine(New Vector2(i * Grid.Height, 0), New Vector2(i * Grid.Height, CanvasCount.Width * Grid.Height + Grid.Height), mBrush, 1)

                Next
                For i = 0 To CanvasCount.Width + 1
                    e.DrawLine(New Vector2(0, i * Grid.Width), New Vector2(CanvasCount.Height * Grid.Width + Grid.Width, i * Grid.Width), mBrush, 1)
                Next
                If DrawGridNumber Then
                    For i = 0 To CanvasCount.Height
                        e.DrawText(i, mTextFormat, New SharpDX.RectangleF(i * Grid.Height + 2, 0, i * Grid.Height + 100, 0), mBrush)
                    Next
                    For i = 0 To CanvasCount.Width
                        e.DrawText(i, mTextFormat, New SharpDX.RectangleF(2, i * Grid.Width, 100, i * Grid.Width), mBrush)
                    Next
                End If
            End If
        End Sub
    End Class
End Namespace
