Imports MapEditor.Item.Game
Namespace Map.Importer
    Public Class FromBMMaker
        Public Enum FloorType
            WoodTrafo = 0
            StoneTrafo = 1
            PaperTrafo = 2
            Sunken = 3
            Normal = 4
            Sunken2Normal = 5
            Borderless = 6
            Unknown = 7
        End Enum
        Dim WithEvents SolvePictureBox As PictureBox
        Dim FloorGrid(999, 999) As FloorType
        Dim FloorList As New List(Of FloorType)
        Public BlockList As New List(Of Block)
        Private Function GetPixelType(ByVal Color As Color) As FloorType
            Select Case Color
                Case Color.FromArgb(0, 0, 0) '平路
                    Return FloorType.Normal
                Case Color.FromArgb(128, 128, 0) '内路
                    Return FloorType.Borderless
                Case Color.FromArgb(49, 106, 197) '平凹转换
                    Return FloorType.Sunken2Normal
                Case Color.FromArgb(128, 0, 128) '凹
                    Return FloorType.Sunken
                Case Color.FromArgb(0, 255, 255) '纸
                    Return FloorType.PaperTrafo
                Case Color.FromArgb(255, 128, 0) '木
                    Return FloorType.WoodTrafo
                Case Color.FromArgb(0, 192, 0) '石
                    Return FloorType.StoneTrafo

                    'Case Color.FromArgb(195, 195, 195) '风扇底座
                    '    Return FloorType.Unknown
                Case Else
                    Return FloorType.Unknown
            End Select
        End Function
        Public Sub New(ByVal ImageName As String)
            SolvePictureBox = New PictureBox
            SolvePictureBox.Load(ImageName)
            SolveImage()
        End Sub
        Private Sub SolveImage()
            Dim Bitmap As Bitmap = SolvePictureBox.Image
            For i = 0 To Bitmap.Width - 1
                For j = 0 To Bitmap.Height - 1
                    FloorGrid(i, j) = GetPixelType(Bitmap.GetPixel(i, j))
                Next
            Next
            Dim Res As New Block
            For i = 0 To Bitmap.Width - 1
                For j = 0 To Bitmap.Height - 1
                    'BigBlock
                    If (FloorGrid(i, j) = FloorType.PaperTrafo) Or (FloorGrid(i, j) = FloorType.StoneTrafo) Or (FloorGrid(i, j) = FloorType.WoodTrafo) Or (FloorGrid(i, j) = FloorType.Sunken2Normal) Then
                        FloorGrid(i + 1, j) = FloorType.Unknown
                        FloorGrid(i + 1, j + 1) = FloorType.Unknown
                        FloorGrid(i, j + 1) = FloorType.Unknown
                    End If
                    Debug.Print(i & "," & j)
                    Res = ReturnBlock(FloorGrid(i, j), New Point(i, j))
                    If Res.Type <> BlockType.Deleted Then
                        BlockList.Add(Res)
                    End If
                Next
            Next
        End Sub
        Private Function ReturnBlock(ByVal Type As FloorType, ByVal Position As Point) As Block
            Select Case Type

                Case FloorType.Sunken2Normal
                    If FloorGrid(Position.X, Position.Y - 1) = FloorType.Normal Then 'Top
                        Return New Block(BlockSize.BigBlock, BlockType.NormalChange_Su2Fl, RotateDegree.Rotate180, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)
                    ElseIf FloorGrid(Position.X, Position.Y + 2) = FloorType.Normal Then 'Buttom
                        Return New Block(BlockSize.BigBlock, BlockType.NormalChange_Su2Fl, RotateDegree.NoRotate, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)
                    ElseIf FloorGrid(Position.X - 1, Position.Y) = FloorType.Normal Then 'Left
                        Return New Block(BlockSize.BigBlock, BlockType.NormalChange_Su2Fl, RotateDegree.Rotate90, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)
                    ElseIf FloorGrid(Position.X + 2, Position.Y) = FloorType.Normal Then 'Right
                        Return New Block(BlockSize.BigBlock, BlockType.NormalChange_Su2Fl, RotateDegree.Rotate270, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)
                    Else 'NoFound
                        Return New Block(BlockSize.BigBlock, BlockType.NormalChange_Su2Fl, RotateDegree.NoRotate, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)
                    End If

                Case FloorType.PaperTrafo
                    Return New Block(BlockSize.BigBlock, BlockType.PaperTrafo, RotateDegree.NoRotate, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)

                Case FloorType.WoodTrafo
                    Return New Block(BlockSize.BigBlock, BlockType.WoodTrafo, RotateDegree.NoRotate, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)

                Case FloorType.StoneTrafo
                    Return New Block(BlockSize.BigBlock, BlockType.StoneTrafo, RotateDegree.NoRotate, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)

                Case FloorType.Borderless
                    Return New Block(BlockSize.SmallBlock, BlockType.SmallFlatBorderless, RotateDegree.NoRotate, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)

                Case FloorType.Normal
                    '转角
                    '左上不存在
                    If FloorGrid(Position.X - 1, Position.Y) <> FloorType.Normal And FloorGrid(Position.X, Position.Y - 1) <> FloorType.Normal Then
                        Return New Block(BlockSize.SmallBlock, BlockType.SmallFlatTurnOut, RotateDegree.NoRotate, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)

                        '右上不存在
                    ElseIf FloorGrid(Position.X + 1, Position.Y) <> FloorType.Normal And FloorGrid(Position.X, Position.Y - 1) <> FloorType.Normal Then
                        Return New Block(BlockSize.SmallBlock, BlockType.SmallFlatTurnOut, RotateDegree.Rotate90, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)

                        '左下不存在
                    ElseIf FloorGrid(Position.X - 1, Position.Y) <> FloorType.Normal And FloorGrid(Position.X, Position.Y + 1) <> FloorType.Normal Then
                        Return New Block(BlockSize.SmallBlock, BlockType.SmallFlatTurnOut, RotateDegree.Rotate270, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)

                        '右下不存在
                    ElseIf FloorGrid(Position.X + 1, Position.Y) <> FloorType.Normal And FloorGrid(Position.X, Position.Y + 1) <> FloorType.Normal Then
                        Return New Block(BlockSize.SmallBlock, BlockType.SmallFlatTurnOut, RotateDegree.Rotate180, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)

                        '普通
                        '上下左右都存在
                    ElseIf (FloorGrid(Position.X, Position.Y - 1) = FloorType.Normal And FloorGrid(Position.X, Position.Y + 1) = FloorType.Normal) And (FloorGrid(Position.X - 1, Position.Y) = FloorType.Normal And FloorGrid(Position.X + 1, Position.Y) = FloorType.Normal) Then
                        '左上不存在
                        If FloorGrid(Position.X - 1, Position.Y - 1) = FloorType.Unknown Then
                            Return New Block(BlockSize.SmallBlock, BlockType.SmallFlatTurnIn, RotateDegree.Rotate180, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)
                            '右上不存在
                        ElseIf FloorGrid(Position.X + 1, Position.Y - 1) = FloorType.Unknown Then
                            Return New Block(BlockSize.SmallBlock, BlockType.SmallFlatTurnIn, RotateDegree.Rotate270, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)
                            '左下不存在
                        ElseIf FloorGrid(Position.X - 1, Position.Y + 1) = FloorType.Unknown Then
                            Return New Block(BlockSize.SmallBlock, BlockType.SmallFlatTurnIn, RotateDegree.Rotate90, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)
                            '右下不存在
                        ElseIf FloorGrid(Position.X + 1, Position.Y + 1) = FloorType.Unknown Then
                            Return New Block(BlockSize.SmallBlock, BlockType.SmallFlatTurnIn, RotateDegree.NoRotate, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)
                        Else '周围都存在
                            Return New Block(BlockSize.SmallBlock, BlockType.SmallFlatBorderless, RotateDegree.NoRotate, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)
                        End If
                        '左右存在
                    ElseIf (FloorGrid(Position.X - 1, Position.Y) = FloorType.Normal And FloorGrid(Position.X + 1, Position.Y) = FloorType.Normal) Then
                        '上不存在
                        If FloorGrid(Position.X, Position.Y - 1) = FloorType.Unknown Then
                            Return New Block(BlockSize.SmallBlock, BlockType.SmallFlatBorder, RotateDegree.Rotate90, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)
                        Else '下不存在
                            Return New Block(BlockSize.SmallBlock, BlockType.SmallFlatBorder, RotateDegree.Rotate270, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)
                        End If

                        '上下存在
                    ElseIf (FloorGrid(Position.X, Position.Y - 1) = FloorType.Normal And FloorGrid(Position.X, Position.Y + 1) = FloorType.Normal) Then
                        '左不存在
                        If FloorGrid(Position.X - 1, Position.Y) = FloorType.Unknown Then
                            Return New Block(BlockSize.SmallBlock, BlockType.SmallFlatBorder, RotateDegree.NoRotate, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)
                        Else '右不存在
                            Return New Block(BlockSize.SmallBlock, BlockType.SmallFlatBorder, RotateDegree.Rotate180, New Point(Position.X * 20, Position.Y * 20), frmMain._RenderTarget)
                        End If

                  
                    End If

                Case Else
                    Return New Block(BlockSize.BigBlock, BlockType.Deleted, RotateDegree.NoRotate, New Point(0, 0), frmMain._RenderTarget)
            End Select
            Return New Block(BlockSize.BigBlock, BlockType.Deleted, RotateDegree.NoRotate, New Point(0, 0), frmMain._RenderTarget)
        End Function
    End Class
End Namespace