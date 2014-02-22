Imports MapEditor.Item.Game
Imports System.ComponentModel
Imports SharpDX


Namespace Item
    Public Class CLayer
        <Browsable(False)>
        Public Property BlockColl As New List(Of Block)
        <Browsable(False)>
        Public Property RailColl As New List(Of Rail)

        <Category("属性")> <DescriptionAttribute("层名称")>
        Public Property Name As String
        <Category("属性")> <DescriptionAttribute("生成高度")>
        Public Property Height As Double
        <Category("属性")> <DescriptionAttribute("可见性")>
        Public Property Visible As Boolean = True
        Public Sub New()

        End Sub
        Public Sub New(_name As String)
            Name = _name
        End Sub

        Public Sub New(_name As String, _height As Double, _visible As Boolean)
            Name = _name
            Height = _height
            Visible = _visible
        End Sub

        Public Sub Draw(ByVal e As Direct2D1.RenderTarget)
            If Visible = True Then
                For Each i In BlockColl
                    i.Draw(e)
                Next
            End If
        End Sub

        Public Sub Active()
            For Each i In BlockColl
                i.Opacity = 1
            Next
        End Sub

        Public Sub Inactive()
            For Each i In BlockColl
                i.Opacity = 0.3
            Next
        End Sub

    End Class
End Namespace