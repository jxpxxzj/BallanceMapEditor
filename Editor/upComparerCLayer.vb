Imports MapEditor.Item
Namespace Editor.Plugin
    Public Class upComparerCLayer
        Implements IComparer(Of CLayer)
        Public Function Compare(ByVal x As CLayer, ByVal y As CLayer) As Integer Implements System.Collections.Generic.IComparer(Of CLayer).Compare
            If x.Height >= y.Height Then
                Return 1
            Else
                Return -1
            End If
        End Function
    End Class
End Namespace