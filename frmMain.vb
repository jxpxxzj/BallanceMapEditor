Imports System.IO
Imports MapEditor.Item.Game
Imports MapEditor.Item.Editor
Imports MapEditor.Item
Imports MapEditor.Map
Imports SharpDX

Public Class frmMain

#Region "Varient"

#Region "Object"
    Public nowMap As New CMap
    Public nowDrawSign As New Sign
    Public nowBlock As New Block
    Dim nowLayerID As Integer
#End Region

#Region "Editor"
    Dim nowMouse As Boolean = True
    Dim nowSelect As Integer
    Dim nowMoveOn As Boolean = False
    Dim nowX, nowY As Integer
    Dim ObjectOnLoad As Boolean = False

    Dim old_time, new_time, frame_time As Date
    Dim TotalTime As TimeSpan
    Dim Frame As Integer
#End Region

#End Region

#Region "Canvas"

#Region "Control"
    Private Function ReturnBlockID(ByVal pos As System.Drawing.Point) As Integer
        For i = 0 To nowMap.LayerColl(nowLayerID).BlockColl.Count - 1
            Dim Rec As System.Drawing.Rectangle = New System.Drawing.Rectangle(nowMap.LayerColl(nowLayerID).BlockColl(i).Position, New Size(nowMap.LayerColl(nowLayerID).BlockColl(i).Width, nowMap.LayerColl(nowLayerID).BlockColl(i).Height))
            If (pos.X > Rec.X) And (pos.Y > Rec.Y) And (pos.X < Rec.X + Rec.Width) And (pos.Y < Rec.Y + Rec.Height) Then
                Return i
            End If
        Next
        Return -1
    End Function
    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint
        MainLoop()
    End Sub
    Private Sub Panel2_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Panel2.MouseDown
        If e.Button = System.Windows.Forms.MouseButtons.Right Then
            Dim id = ReturnBlockID(e.Location)
            If id <> -1 Then
                DeleteItem(id)
                RefreshCombobox()
            End If
            Exit Sub
        End If
        If nowMouse = False Then
            If ReturnBlockID(e.Location) = -1 Then
                Dim pos As System.Drawing.Point = New System.Drawing.Point(e.X + 1 - (e.X Mod 20), e.Y + 2 - (e.Y Mod 20) - 1)
                nowMap.LayerColl(nowLayerID).BlockColl.Add(New Block(nowBlock.Size, nowBlock.Type, 0, pos, _RenderTarget))
                nowMap.LayerColl(nowLayerID).BlockColl(nowMap.LayerColl(nowLayerID).BlockColl.Count - 1).Rotate = Val(ComboBox2.Text)
                nowSelect = nowMap.LayerColl(nowLayerID).BlockColl.Count - 1
                ComboBox1.Items.Add(nowMap.LayerColl(nowLayerID).BlockColl.Count - 1 & ":" & nowMap.LayerColl(nowLayerID).BlockColl(nowMap.LayerColl(nowLayerID).BlockColl.Count - 1).Text)
                ComboBox1.SelectedIndex = nowMap.LayerColl(nowLayerID).BlockColl.Count - 1
            End If
        Else
            If e.Button = System.Windows.Forms.MouseButtons.Left Then
                Dim BlockID As Integer = ReturnBlockID(e.Location)
                If BlockID <> -1 Then
                    PropertyGrid1.SelectedObject = nowMap.LayerColl(nowLayerID).BlockColl(BlockID)
                    ComboBox1.SelectedIndex = BlockID
                End If
            End If
        End If
    End Sub
    Private Sub DeleteItem(ByVal Index As Integer)
        If Index <> -1 Then
            nowMap.LayerColl(nowLayerID).BlockColl.RemoveAt(Index)
            RefreshCombobox()
            nowSelect = 0
            ComboBox1.SelectedIndex = Index - 1
        End If
    End Sub

    Private Sub Panel2_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel2.MouseLeave
        nowMoveOn = False
    End Sub

    Private Sub Panel2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel2.MouseMove
        ToolStripStatusLabel2.Text = "X:" & e.X & " Y:" & e.Y
        nowMoveOn = True
        If nowBlock.Text <> "Deleted" Then
            nowX = e.X
            nowY = e.Y
        End If
    End Sub
#End Region

#Region "Draw"
    Private Sub DrawCanvas()
        Try
            _RenderTarget.BeginDraw()
            nowMap.Draw(_RenderTarget)
            Dim mBrush As New Direct2D1.SolidColorBrush(_RenderTarget, SharpDX.Color.Red)
            If nowMap.LayerColl(nowLayerID).BlockColl.Count <> 0 Then
                With nowMap.LayerColl(nowLayerID).BlockColl(nowSelect)
                    _RenderTarget.DrawLine(New SharpDX.Vector2(.Position.X - 1, .Position.Y - 1), New SharpDX.Vector2(.Position.X - 1, .Position.Y + .Height - 1), mBrush, 1)
                    _RenderTarget.DrawLine(New SharpDX.Vector2(.Position.X, .Position.Y - 1), New SharpDX.Vector2(.Position.X + .Width - 1, .Position.Y - 1), mBrush, 1)
                    _RenderTarget.DrawLine(New SharpDX.Vector2(.Position.X + .Width - 1, .Position.Y - 1), New SharpDX.Vector2(.Position.X + .Width - 1, .Position.Y + .Height - 1), mBrush, 1)
                    _RenderTarget.DrawLine(New SharpDX.Vector2(.Position.X, .Position.Y + .Height - 1), New SharpDX.Vector2(.Position.X + .Width - 1, .Position.Y + .Height - 1), mBrush, 1)
                End With
            End If
            If (nowBlock.Type <> BlockType.Deleted) And (nowMoveOn = True) Then
                DrawNowBlock()
            End If
            _RenderTarget.EndDraw()
        Catch ex As Exception
            MsgBox(ex.Message & ex.StackTrace)
        End Try
    End Sub
    Private Sub DrawNowBlock()
        Dim mBrush As New Direct2D1.SolidColorBrush(_RenderTarget, SharpDX.Color.Red)
        nowBlock.Rotate = Val(ComboBox2.Text)
        Dim BitmapBrush As Direct2D1.BitmapBrush
        BitmapBrush = New Direct2D1.BitmapBrush(_RenderTarget, nowBlock.Image)
        BitmapBrush.Transform = Matrix3x2.Scaling(0.3125, 0.3125, New Vector2(0, 0)) * Matrix3x2.Translation(New Vector2(nowX + 1 - (nowX Mod 20), nowY + 1 - (nowY Mod 20))) * Matrix3x2.Rotation(Math.PI / 180 * nowBlock.Rotate, New Vector2(nowX + 1 - (nowX Mod 20) + nowBlock.Width / 2, nowY + 1 - (nowY Mod 20) + nowBlock.Height / 2))
        Dim Rec As RectangleF = New SharpDX.RectangleF(nowX + 1 - (nowX Mod 20), nowY + 1 - (nowY Mod 20), nowBlock.Width, nowBlock.Height)
        _RenderTarget.FillRectangle(Rec, BitmapBrush)
    End Sub
#End Region

#End Region

#Region "FormEvents"
    Private Sub Form2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        退出ToolStripMenuItem_Click(sender, e)
    End Sub
    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CreateDeviceResource(Me.Panel2)
        UI.Skin.LoadSkin()

        OpenFileDialog1.FileName = ""
        SaveFileDialog1.FileName = ""
        'PropertyGrid2.SelectedObject = nowMap.Grid
        ComboBox2.SelectedIndex = 0

        nowMap.LayerColl.Add(New CLayer("New Layer"))
        ComboBox3.Items.Add(nowMap.LayerColl(0).Name)
        ComboBox3.SelectedIndex = 0
        TextBox1.Text = nowMap.Title
        TextBox2.Text = nowMap.Creator
        Me.Text = "Ballance Map Editor - " & nowMap.Title
        Panel2.AutoSize = True
        old_time = Now
        SharpDX.Windows.RenderLoop.Run(Me, AddressOf MainLoop, True)
        Me.Focus()
    End Sub

    Private Sub MainLoop()
        frame_time = Now
        DrawCanvas()
        frame += 1
        new_time = Now
        totaltime += (new_time - frame_time)
        If (new_time - old_time).Seconds > 1 Then
            old_time = new_time
            ToolStripStatusLabel4.Text = "FPS:" & Math.Round(1 / (totaltime.TotalSeconds / frame))
            totaltime = New TimeSpan
            frame = 0
        End If
    End Sub
#End Region

#Region "LayerControl"
    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ObjectOnLoad = False Then
            ToolStripStatusLabel1.Text = "正在进行层切换... 目标层:" & nowMap.LayerColl(ComboBox3.SelectedIndex).Name
            Application.DoEvents()
            For Each l In nowMap.LayerColl
                l.Inactive()
            Next
            PropertyGrid3.SelectedObject = nowMap.LayerColl(ComboBox3.SelectedIndex)
            nowLayerID = ComboBox3.SelectedIndex
            nowMap.LayerColl(nowLayerID).Active()
            nowSelect = 0

            RefreshCombobox()
            ToolStripStatusLabel1.Text = "就绪"
            ToolStripStatusLabel3.Text = "当前层:" & nowMap.LayerColl(nowLayerID).Name
        End If
    End Sub
    Private Sub PropertyGrid3_PropertyValueChanged(sender As Object, e As PropertyValueChangedEventArgs) Handles PropertyGrid3.PropertyValueChanged
        ComboBox3.Items.Clear()
        ObjectOnLoad = True
        For i = 0 To nowMap.LayerColl.Count - 1
            ComboBox3.Items.Add(nowMap.LayerColl(i).Name)
        Next
        ObjectOnLoad = False
        ComboBox3.SelectedIndex = nowLayerID
    End Sub

    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles Button24.Click
        nowMap.LayerColl.Add(New CLayer(TextBox3.Text))
        ComboBox3.Items.Add(nowMap.LayerColl(nowMap.LayerColl.Count - 1).Name)
        ComboBox3.SelectedIndex = nowMap.LayerColl.Count - 1
    End Sub

    Private Sub Button25_Click(sender As Object, e As EventArgs) Handles Button25.Click
        If nowMap.LayerColl.Count > 1 Then
            ComboBox3.SelectedIndex = 0
            nowMap.LayerColl.RemoveAt(nowLayerID)
            nowLayerID = 0
            ComboBox3.Items.Clear()
            For i = 0 To nowMap.LayerColl.Count - 1
                ComboBox3.Items.Add(nowMap.LayerColl(i).Name)
            Next
            ComboBox3.SelectedIndex = 0
        Else
            MsgBox("不能删除这一层!", vbOKOnly, "提示")
        End If
    End Sub
#End Region

#Region "SetBlock"
    Private Sub SetNowBlock(ByVal nowType As BlockType, ByVal SetText As String, ByVal nowSize As BlockSize)
        nowBlock = New Block(nowSize, nowType, Val(ComboBox2.Text), New System.Drawing.Point(0, 0), _RenderTarget)
        nowMouse = False
        ToolStripStatusLabel1.Text = "当前选择:" & SetText
    End Sub
    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        nowMouse = True
        nowBlock = New Block
        ToolStripStatusLabel1.Text = "就绪"
    End Sub

    Private Sub Button5_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        SetNowBlock(BlockType.StoneTrafo, Button5.Text, BlockSize.BigBlock)
    End Sub

    Private Sub Button6_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        SetNowBlock(BlockType.PaperTrafo, Button6.Text, BlockSize.BigBlock)
    End Sub

    Private Sub Button4_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        SetNowBlock(BlockType.WoodTrafo, Button4.Text, BlockSize.BigBlock)
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        SetNowBlock(BlockType.NormalFlatFloor, Button7.Text, BlockSize.BigBlock)
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        SetNowBlock(BlockType.NormalFlatTurn, Button11.Text, BlockSize.BigBlock)
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        SetNowBlock(BlockType.NormalChange_Su2Fl, Button8.Text, BlockSize.BigBlock)
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        SetNowBlock(BlockType.NormalSunkenFloor, Button9.Text, BlockSize.BigBlock)
    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        SetNowBlock(BlockType.NormalFlatBorderTurn, Button13.Text, BlockSize.BigBlock)
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        SetNowBlock(BlockType.NormalFlatBorderless, Button14.Text, BlockSize.BigBlock)
    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        SetNowBlock(BlockType.NormalFlatBorder, Button15.Text, BlockSize.BigBlock)
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        SetNowBlock(BlockType.NormalSunkenTurn, Button16.Text, BlockSize.BigBlock)
    End Sub

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
        SetNowBlock(BlockType.SmallFlatTurnOut, Button19.Text, BlockSize.SmallBlock)
    End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        SetNowBlock(BlockType.SmallFlatBorder, Button18.Text, BlockSize.SmallBlock)
    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        SetNowBlock(BlockType.SmallFlatTurnIn, Button17.Text, BlockSize.SmallBlock)
    End Sub

    Private Sub Button22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button22.Click
        SetNowBlock(BlockType.SmallSunkenTurnOut, Button22.Text, BlockSize.SmallBlock)
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        SetNowBlock(BlockType.SmallSunkenFloor, Button21.Text, BlockSize.SmallBlock)
    End Sub

    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button20.Click
        SetNowBlock(BlockType.SmallSunkenTurnIn, Button20.Text, BlockSize.SmallBlock)
    End Sub

    Private Sub Button23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button23.Click
        SetNowBlock(BlockType.SmallFlatBorderless, Button23.Text, BlockSize.SmallBlock)
    End Sub
#End Region

#Region "ControlEvent"
    Private Sub RefreshCombobox()
        ComboBox1.Items.Clear()
        For i = 0 To nowMap.LayerColl(nowLayerID).BlockColl.Count - 1
            ComboBox1.Items.Add(i & ":" & nowMap.LayerColl(nowLayerID).BlockColl(i).Text)
        Next
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        nowSelect = ComboBox1.SelectedIndex
        PropertyGrid1.SelectedObject = nowMap.LayerColl(nowLayerID).BlockColl(nowSelect)

    End Sub
#End Region

#Region "Menu"
    Public Sub 新建ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 新建ToolStripMenuItem.Click
        nowMap = New CMap
        nowMouse = True
        nowBlock = New Block
        nowLayerID = 0
        nowSelect = 0
        'PictureBox1.Refresh()
        PropertyGrid1.SelectedObject = Nothing
        PropertyGrid1.SelectedObject = Nothing
        OpenFileDialog1.FileName = ""
        SaveFileDialog1.FileName = ""
        ComboBox1.Items.Clear()
        ComboBox3.Items.Clear()
        nowMap.LayerColl.Add(New CLayer("New Layer"))
        ComboBox3.Items.Add(nowMap.LayerColl(0).Name)
        ComboBox3.SelectedIndex = 0
        ToolStripStatusLabel1.Text = "就绪"
        Me.Text = "Ballance Map Editor - 未命名"
    End Sub

    Private Sub 打开ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 打开ToolStripMenuItem.Click
        On Error GoTo err
        Dim MapPath As String
        OpenFileDialog1.Filter = "Ballance地图文件|*.bme"
        OpenFileDialog1.RestoreDirectory = True
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Title = "打开"

        Dim result As MsgBoxResult
        result = MsgBox("文件尚未保存,是否保存?", MsgBoxStyle.YesNoCancel, "提示")
        If result = MsgBoxResult.Cancel Then
            Exit Sub
        End If
        If result = MsgBoxResult.Yes Then
            保存ToolStripMenuItem_Click(sender, e)
        End If

        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            Dim timer As New Stopwatch
            MapPath = OpenFileDialog1.FileName
            timer.Start()
            Call 新建ToolStripMenuItem_Click(sender, e)
            ComboBox3.Items.Clear()
            nowMap.LayerColl.Clear()
            ToolStripStatusLabel1.Text = "正在加载文件..."
            Application.DoEvents()

            ObjectOnLoad = True
            nowMap.OpenFile(MapPath)
            For i = 0 To nowMap.LayerColl.Count - 1
                ComboBox3.Items.Add(nowMap.LayerColl(i).Name)
            Next
            ComboBox3.SelectedIndex = 0
            ObjectOnLoad = False

            PropertyGrid2.SelectedObject = nowMap.LayerColl(0)
            ToolStripStatusLabel3.Text = "当前层:" & nowMap.LayerColl(0).Name


            RefreshCombobox()
            timer.Stop()

            Select Case nowMap.Result
                Case CMap.OpenResult.Successful
                    ToolStripStatusLabel1.Text = "文件加载完成,用时:" & Math.Round(timer.Elapsed.TotalSeconds, 2) & "s."
                Case CMap.OpenResult.Updated
                    ToolStripStatusLabel1.Text = "文件成功升级并已保存,用时:" & Math.Round(timer.Elapsed.TotalSeconds, 2) & "s."
                Case CMap.OpenResult.Failed
err:
                    ObjectOnLoad = False
                    Call 新建ToolStripMenuItem_Click(sender, e)
                    ToolStripStatusLabel1.Text = "文件打开失败."
                    Exit Sub
            End Select

        Else
            Exit Sub
        End If
        Me.Text = "Ballance Map Editor - " & MapPath
    End Sub

    Private Sub 保存ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 保存ToolStripMenuItem.Click
        SaveFileDialog1.Filter = "Ballance地图文件|*.bme"
        If OpenFileDialog1.FileName = "" Then
            SaveFileDialog1.FileName = "未命名"
        Else
            SaveFileDialog1.FileName = OpenFileDialog1.FileName
        End If

        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            nowMap.SaveFile(SaveFileDialog1.FileName)
            Me.Text = "Ballance Map Editor - " & SaveFileDialog1.FileName
            ToolStripStatusLabel1.Text = "文件已保存."
        End If
    End Sub
    Private Sub 退出ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 退出ToolStripMenuItem.Click
        Dim result As MsgBoxResult
        result = MsgBox("文件尚未保存,是否保存?", MsgBoxStyle.YesNoCancel, "提示")
        If result = MsgBoxResult.Cancel Then
            Exit Sub
        End If
        If result = MsgBoxResult.No Then
            End
        End If
        If result = MsgBoxResult.Yes Then
            保存ToolStripMenuItem_Click(sender, e)
        End If
    End Sub
    Private Sub 另存为ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 另存为ToolStripMenuItem.Click
        SaveFileDialog1.Filter = "Ballance地图文件|*.bme"
        SaveFileDialog1.FileName = "未命名"
        SaveFileDialog1.FileName = OpenFileDialog1.FileName
        If SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            nowMap.SaveFile(SaveFileDialog1.FileName)
            Me.Text = "Ballance Map Editor - " & SaveFileDialog1.FileName
        End If
    End Sub
    Private Sub 标记编辑器SToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 标记编辑器SToolStripMenuItem.Click
        Form4.Show()
    End Sub

    Private Sub 填充区块FToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 填充区块FToolStripMenuItem.Click
        MsgBox("暂不可用.", MsgBoxStyle.OkOnly, "提示")
        'If nowBlock.Text = "" Then
        '    MsgBox("选择一个方块后在再使用此工具。")
        'Else
        '    Form3.Show()
        '    Form3.Label3.Text = ToolStripStatusLabel1.Text
        'End If
    End Sub
    Private Sub BMMaker的bmpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BMMaker的bmpToolStripMenuItem.Click
        Dim test As New Importer.FromBMMaker(Application.StartupPath & "/Test.png")
        For Each s In test.BlockList
            nowMap.LayerColl(nowLayerID).BlockColl.Add(s)
        Next
        RefreshCombobox()
    End Sub
    Private Sub 编译为ms文件ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 编译为ms文件ToolStripMenuItem.Click
        nowMap.Build()
    End Sub

    Private Sub 删除DToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 删除DToolStripMenuItem.Click
        DeleteItem(nowSelect)
    End Sub
#End Region

#Region "Direct2D"
    Public _D2DFactory As Direct2D1.Factory
    Public _RenderTarget As Direct2D1.WindowRenderTarget
    Public _ImagingFactory As WIC.ImagingFactory
    Public _TextFactory As DirectWrite.Factory
    Public Sub CreateDeviceResource(Target As Control)
        Try
            _D2DFactory = New Direct2D1.Factory
            _ImagingFactory = New WIC.ImagingFactory
            _TextFactory = New DirectWrite.Factory
            Dim P As New Direct2D1.PixelFormat(DXGI.Format.B8G8R8A8_UNorm, Direct2D1.AlphaMode.Ignore)

            Dim H As New Direct2D1.HwndRenderTargetProperties
            H.Hwnd = Target.Handle
            H.PixelSize = New Size2(Target.Width, Target.Height)
            H.PresentOptions = SharpDX.Direct2D1.PresentOptions.None

            Dim R As New Direct2D1.RenderTargetProperties(Direct2D1.RenderTargetType.Default, _
                                                                      P, 0, 0, _
                                                                      Direct2D1.RenderTargetUsage.None, _
                                                                      Direct2D1.FeatureLevel.Level_DEFAULT)
            _RenderTarget = New Direct2D1.WindowRenderTarget(_D2DFactory, R, H)
        Catch ex As Exception
            MsgBox("Direct2D初始化失败,程序将自动退出. 以下是错误信息:" & vbCrLf & ex.Message & vbCrLf & ex.StackTrace)
            End
        End Try
    End Sub
#End Region


End Class