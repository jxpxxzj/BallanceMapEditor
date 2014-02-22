Imports MapEditor.Item.Game
Imports MapEditor.Item.Editor
Public Class Form3
    Dim nowBlock As Block
    Private Function GetSignString(ByVal Sign As Sign) As String
        Dim waitToReturn As String = Nothing
        waitToReturn += Sign.Name
        waitToReturn += " [X:" & Sign.Position.X & " Y:" & Sign.Position.Y & "]"
        Return (waitToReturn)
    End Function
    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        nowBlock = frmMain.nowBlock
        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()

        For i = 0 To frmMain.nowMap.SignColl.Count - 1
            ComboBox1.Items.Add(GetSignString(frmMain.nowMap.SignColl(i)))
            ComboBox2.Items.Add(GetSignString(frmMain.nowMap.SignColl(i)))
        Next
    End Sub

    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    Call FillBlock(New Point(Val(TextBox1.Text), Val(TextBox2.Text)), New Point(Val(TextBox4.Text), Val(TextBox3.Text)), nowBlock, sender, e)
    '    Me.Close()
    'End Sub
    'Public Sub FillBlock(ByVal StartPosition As Point, ByVal EndPosition As Point, ByVal Block As Block)
    '    Dim waitToFill As Block
    '    With waitToFill
    '        .BlockRotate = Val(TextBox5.Text)
    '        .BlockImage = Block.BlockImage
    '        .BlockString = Block.BlockString
    '        .BlockSize.Width = Block.BlockSize.Width
    '        .BlockSize.Height = Block.BlockSize.Height



    '        For i = StartPosition.X To EndPosition.X - Block.BlockSize.Width Step Block.BlockSize.Width
    '            For j = StartPosition.Y To EndPosition.Y - Block.BlockSize.Height Step Block.BlockSize.Height
    '                Form2.nowImageCount += 1
    '                .BlockSize.X = i
    '                .BlockSize.Y = j
    '                Debug.Print(Form2.nowImageCount)
    '                Form2.Canv(Form2.nowImageCount) = waitToFill
    '                Form2.Canvas(i, j) = Form2.nowImageCount
    '            Next
    '        Next
    '    End With
    '    With Form2
    '        Call .SaveFile()
    '        .ComboBox1.Items.Clear()
    '        Call .OpenFile(Application.StartupPath & "\Temp.bme")
    '        .ComboBox1.SelectedIndex = .nowSelect - 1
    '    End With
    'End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        TextBox1.Text = frmMain.nowMap.SignColl(ComboBox1.SelectedIndex).Position.X
        TextBox2.Text = frmMain.nowMap.SignColl(ComboBox1.SelectedIndex).Position.Y
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        TextBox4.Text = frmMain.nowMap.SignColl(ComboBox2.SelectedIndex).Position.X
        TextBox3.Text = frmMain.nowMap.SignColl(ComboBox2.SelectedIndex).Position.Y
    End Sub
End Class