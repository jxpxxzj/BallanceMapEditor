Imports System.Drawing
Public Class Form1
    Dim instance As Bitmap
    Dim waitToWrite As Color
    Dim MapInfo(0 To 999, 0 To 999) As Item.Game.BlockType
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PictureBox1.Load(Application.StartupPath & "\Test.png")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        instance = PictureBox1.Image
        ProgressBar1.Maximum = instance.Height
        For i = 0 To instance.Height - 1
            For j = 0 To instance.Width - 1
                waitToWrite = instance.GetPixel(j, i)
                Select Case waitToWrite
                    Case Color.FromArgb(0, 0, 0) '平路
                        MapInfo(i, j) = Item.Game.BlockType.SmallFlatBorder
                    Case Color.FromArgb(128, 128, 0) '内路
                        MapInfo(i, j) = Item.Game.BlockType.SmallFlatBorderless
                    Case Color.FromArgb(49, 106, 197) '平凹转换
                        MapInfo(i, j) = Item.Game.BlockType.NormalChange_Su2Fl
                    Case Color.FromArgb(128, 0, 128) '凹
                        MapInfo(i, j) = Item.Game.BlockType.NormalSunkenFloor
                    Case Color.FromArgb(195, 195, 195) '风扇底座
                        MapInfo(i, j) = Item.Game.BlockType.Deleted
                    Case Color.FromArgb(0, 255, 255) '纸
                        MapInfo(i, j) = Item.Game.BlockType.PaperTrafo
                    Case Color.FromArgb(255, 128, 0) '木
                        MapInfo(i, j) = Item.Game.BlockType.WoodTrafo
                    Case Color.FromArgb(0, 192, 0) '石
                        MapInfo(i, j) = Item.Game.BlockType.StoneTrafo
                    Case Color.FromArgb(255, 255, 255) '空
                        MapInfo(i, j) = Item.Game.BlockType.Deleted

                        If waitToWrite = Color.FromArgb(0, 255, 255) Or waitToWrite = Color.FromArgb(255, 128, 0) Or waitToWrite = Color.FromArgb(0, 192, 0) Then

                        End If

                End Select
            Next
            ProgressBar1.Value += 1
        Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        For i = 0 To instance.Height
            For j = 0 To instance.Width
                TextBox2.Text += MapInfo(i, j).ToString
            Next
            TextBox2.Text += vbCrLf
        Next
    End Sub

    Private Sub ProgressBar1_Click(sender As Object, e As EventArgs) Handles ProgressBar1.Click

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        frmMain.Show()
    End Sub
End Class
