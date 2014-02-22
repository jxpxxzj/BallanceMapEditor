Imports System.ComponentModel
Imports SharpDX

Namespace Item
    Public MustInherit Class Item
        <Category("属性")> <DescriptionAttribute("物件位置")>
        Public Property Position As System.Drawing.Point
        Public MustOverride Sub Draw(ByVal e As Direct2D1.RenderTarget)
    End Class
End Namespace
