Imports MapEditor.Item
Imports SharpDX
Namespace Item.Game
    Public Class Rail
        Inherits Item
        Public Property PointColl As New List(Of Point)
        Public ReadOnly Property PointArray As Point()
            Get
                Return PointColl.ToArray
            End Get
        End Property
        Public Property Type As RailType

        Public Enum RailType
            Line = 0
            Bezier = 1
            Catmull = 2
            PerfectCurve = 3
        End Enum

        Public Overrides Sub Draw(ByVal e As Direct2D1.RenderTarget)

        End Sub

        'Public Sub DrawControlPoint(e As PaintEventArgs)
        '    e.Graphics.DrawLines(Pens.Blue, PointArray)
        'End Sub
    End Class
End Namespace