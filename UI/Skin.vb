Imports SharpDX
Imports WIC = SharpDX.WIC
Namespace UI
    Public Module Skin
        Private WithEvents SkinList As New List(Of Direct2D1.Bitmap)

        Public _renderTarget As Direct2D1.RenderTarget = frmMain._RenderTarget
        Public _imagingFactory As New WIC.ImagingFactory

        Public SkinFolder As String = Application.StartupPath & "\Texture"
        Public Enum Texture
            Deleted = 0

            Ball_Paper = 1
            Ball_Stone = 2
            Ball_Wood = 3

            Floor_Top_Borderless = 4
            Floor_Top_Flat = 5
            Floor_Top_Profil = 6
            Floor_Top_ProfilFlat = 7

            NormalFlatTurn = 8
            NormalBorderTurn = 9

            NormalSunkenTurn = 10
            Floor_Top_Border = 11

            SmallBorderTurn = 12
            SmallFlatBorder = 13
            SmallFlatBorderless = 14
            SmallFlatTurnIn = 15
            SmallSunkenFloor = 16
            SmallSunkenTurnIn = 17
            SmallSunkenTurnOut = 18
        End Enum
        Public Enum ImageType
            bmp = 0
            png = 1
        End Enum
        Public Sub AddTextrue(ByVal Name As Texture, Optional ByVal isBigTexture As Boolean = True, Optional ByVal ImageType As ImageType = ImageType.bmp)
            If isBigTexture Then
                SkinList.Add(LoadBitmap(SkinFolder & "\" & Name.ToString & "." & ImageType.ToString))
            Else
                SkinList.Add(LoadBitmap(SkinFolder & "\1x1Texture\" & Name.ToString & "." & ImageType.ToString))
            End If
        End Sub

        Public Sub LoadSkin()
            Try
                AddTextrue(Texture.Deleted, True, ImageType.png)

                AddTextrue(Texture.Ball_Paper)
                AddTextrue(Texture.Ball_Stone)
                AddTextrue(Texture.Ball_Wood)

                AddTextrue(Texture.Floor_Top_Borderless)
                AddTextrue(Texture.Floor_Top_Flat)
                AddTextrue(Texture.Floor_Top_Profil)
                AddTextrue(Texture.Floor_Top_ProfilFlat)

                AddTextrue(Texture.NormalFlatTurn)
                AddTextrue(Texture.NormalBorderTurn)
                AddTextrue(Texture.NormalSunkenTurn)
                AddTextrue(Texture.Floor_Top_Border)

                AddTextrue(Texture.SmallBorderTurn, False)
                AddTextrue(Texture.SmallFlatBorder, False)
                AddTextrue(Texture.SmallFlatBorderless, False)
                AddTextrue(Texture.SmallFlatTurnIn, False)

                AddTextrue(Texture.SmallSunkenFloor, False)
                AddTextrue(Texture.SmallSunkenTurnIn, False)
                AddTextrue(Texture.SmallSunkenTurnOut, False)
            Catch ex As Exception
                MsgBox("贴图加载失败,程序将自动退出. 以下是错误信息:" & vbCrLf & ex.Message & vbCrLf & ex.StackTrace)
                End
            End Try
        End Sub

        Public Function GetSkin(ByVal Texture As Texture) As Direct2D1.Bitmap
            Return (SkinList(Texture))
        End Function
        Public Function LoadBitmap(File As String, Optional FrameIndex As Integer = 0) As Direct2D1.Bitmap
            Return LoadBitmap(_renderTarget, File, FrameIndex)
        End Function
        Private Function LoadBitmap(Render As Direct2D1.RenderTarget, File As String, FrameIndex As Integer) As Direct2D1.Bitmap
            Dim Decoder As New WIC.BitmapDecoder(_imagingFactory, File, SharpDX.IO.NativeFileAccess.Read, WIC.DecodeOptions.CacheOnLoad)

            If FrameIndex > Decoder.FrameCount - 1 OrElse FrameIndex < 0 Then FrameIndex = 0

            Dim Source As WIC.BitmapFrameDecode = Decoder.GetFrame(FrameIndex)

            Dim Converter As New WIC.FormatConverter(_imagingFactory)
            Converter.Initialize(Source, WIC.PixelFormat.Format32bppPBGRA)

            Return Direct2D1.Bitmap.FromWicBitmap(Render, Converter)
        End Function
    End Module
End Namespace

