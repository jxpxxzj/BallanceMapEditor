Imports System.ComponentModel
Imports SharpDX

Namespace Item.Game
    Public Class Block
        Inherits Item
        Dim typ As BlockType
        Dim rot As RotateDegree
        <[ReadOnly](True)> <Category("外观")>
        <BrowsableAttribute(False)> <DescriptionAttribute("方块尺寸")>
        Public Property Size As BlockSize

        Dim BitmapBrush As Direct2D1.BitmapBrush
        Dim Rec As SharpDX.RectangleF

        <Category("属性")> <DescriptionAttribute("方块类型")>
        Public Property Type As BlockType
            Get
                Return typ
            End Get
            Set(ByVal value As BlockType)
                If value >= 12 And value <= 18 Then
                    Size = BlockSize.SmallBlock
                Else
                    Size = BlockSize.BigBlock
                End If
                typ = value
                Image = UI.Skin.GetSkin(value)
                'Rotate = 0
            End Set
        End Property
        <Category("外观")> <DescriptionAttribute("方块旋转角度")>
        Public Property Rotate As RotateDegree
            Get
                Return rot
            End Get
            Set(ByVal value As RotateDegree)
                rot = value

            End Set
        End Property
        <BrowsableAttribute(False)>
        Public Property Image As Direct2D1.Bitmap
        <BrowsableAttribute(False)>
        Public Property Opacity As Double = 1

        <BrowsableAttribute(False)> <Category("属性")> <DescriptionAttribute("方块文本标识符")>
        Public ReadOnly Property Text As String
            Get
                Return (Type.ToString())
            End Get
        End Property
        <Category("外观")> <DescriptionAttribute("方块宽度")>
        Public ReadOnly Property Height As Integer
            Get
                If Size = BlockSize.BigBlock Then
                    Return 40
                Else
                    Return 20
                End If
            End Get
        End Property
        <Category("外观")> <DescriptionAttribute("方块长度")>
        Public ReadOnly Property Width As Integer
            Get
                If Size = BlockSize.BigBlock Then
                    Return 40
                Else
                    Return 20
                End If
            End Get
        End Property
        Public Sub New()

        End Sub
        Public Sub New(ByVal Siz As BlockSize, ByVal Typ As BlockType, ByVal Rotat As Integer, ByVal loca As System.Drawing.Point, e As Direct2D1.RenderTarget)
            Size = Siz
            Rotate = Rotat
            Type = Typ
            Position = loca
            Image = UI.GetSkin(Type)
        End Sub
        Public Overrides Sub Draw(ByVal e As Direct2D1.RenderTarget)
            BitmapBrush = New Direct2D1.BitmapBrush(e, Image)
            BitmapBrush.Transform = SharpDX.Matrix3x2.Scaling(0.3125, 0.3125, New Vector2(0, 0)) * SharpDX.Matrix3x2.Translation(New Vector2(Position.X, Position.Y)) * SharpDX.Matrix3x2.Rotation(Math.PI / 180 * Rotate, New Vector2(Position.X + Width / 2, Position.Y + Height / 2))
            BitmapBrush.Opacity = Opacity
            Rec = New SharpDX.RectangleF(Position.X, Position.Y, Width, Height)
            e.FillRectangle(Rec, BitmapBrush)
        End Sub
    End Class
    Public Enum RotateDegree
        NoRotate = 0
        Rotate90 = 90
        Rotate180 = 180
        Rotate270 = 270
    End Enum
    Public Enum BlockSize
        BigBlock = 0
        SmallBlock = 1
    End Enum
    Public Enum BlockType
        Deleted = 0

        PaperTrafo = 1
        StoneTrafo = 2
        WoodTrafo = 3

        NormalFlatBorderless = 4
        NormalFlatFloor = 5
        NormalSunkenFloor = 6
        NormalChange_Su2Fl = 7
        NormalFlatTurn = 8
        NormalFlatBorderTurn = 9
        NormalSunkenTurn = 10
        NormalFlatBorder = 11

        SmallFlatTurnOut = 12
        SmallFlatBorder = 13
        SmallFlatBorderless = 14
        SmallFlatTurnIn = 15
        SmallSunkenFloor = 16
        SmallSunkenTurnIn = 17
        SmallSunkenTurnOut = 18

    End Enum
End Namespace