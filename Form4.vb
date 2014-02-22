Public Class Form4
    Dim nowSign As New Item.Editor.Sign
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        With frmMain
            .nowMap.SignColl.Add(New Item.Editor.Sign(nowSign.Name, nowSign.Position))
            Call RefreshComboBox()
            '.PictureBox1.Refresh()
        End With
    End Sub
    Private Sub RefreshComboBox()
        ComboBox1.Items.Clear()
        With frmMain
            For i = 0 To .nowMap.SignColl.Count - 1
                If .nowMap.SignColl(i).Name <> "" Then
                    ComboBox1.Items.Add(.nowMap.SignColl(i).Name)
                End If
            Next
        End With
    End Sub

    Private Sub Form4_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        With frmMain
            .nowDrawSign.Position = New Point(0, 0)
            .nowDrawSign.Name = ""
            .Panel2.Refresh()
        End With
    End Sub
    Private Sub Form4_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call RefreshComboBox()
        PropertyGrid1.SelectedObject = nowSign
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        PropertyGrid1.SelectedObject = frmMain.nowMap.SignColl(ComboBox1.SelectedIndex)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        On Error GoTo err
        ComboBox1.SelectedIndex = 0
        With frmMain
            .nowMap.SignColl.RemoveAt(ComboBox1.SelectedIndex)
            .Panel2.Refresh()
        End With
        RefreshComboBox()

err:
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim nowSign As New Item.Editor.Sign
        PropertyGrid1.SelectedObject = nowSign
    End Sub
End Class