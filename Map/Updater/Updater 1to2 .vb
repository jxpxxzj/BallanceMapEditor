Imports System.IO

Namespace Map.Updater
    Public Class Updater_1to2
        Public Enum UpdateResult
            Successful = 0
            Failed = 1
        End Enum
        Private Function GetText(ByVal strFile As String, ByVal strStart As String, ByVal strEnd As String) As String
            Return Strings.Mid(strFile, ((Strings.InStr(strFile, strStart) + strStart.Length) + 2), ((Strings.InStr(strFile, strEnd) - Strings.InStr(strFile, strStart)) - (strStart.Length + 4)))
        End Function

        Public Function FileUpdate(ByVal Filetext() As String, ByVal FileName As String) As UpdateResult
            Dim TList As New List(Of String)
            TList = Filetext.ToList
            If TList(0).Split("v")(1) = 1 Then
                TList(0) = "ballance map format v2"
                Dim i As Integer
                For i = 0 To TList.Count - 1
                    If TList(i) = "[Block]" Then
                        TList.Insert(i - 1, "Default,0,True")
                        TList.Insert(i - 1, "[Layer]")
                        TList.Insert(i - 1, "")
                        Exit For
                    End If
                Next
                For j = i + 4 To TList.Count - 1
                    If TList(j) <> "" Then
                        TList(j) = TList(j) + ",0"
                    End If
                Next
                SaveFile(TList, FileName)
                Return UpdateResult.Successful
            End If
            Return UpdateResult.Failed
        End Function

        Private Sub SaveFile(ByVal strList As List(Of String), ByVal FileName As String)
            Dim i As Integer
            Dim strFilePath As String = FileName
            Dim sw As StreamWriter = New StreamWriter(strFilePath, False) 'true是指以追加的方式打开指定文件
            For i = 0 To strList.Count - 1
                sw.WriteLine(strList(i))
            Next
            sw.Close()
        End Sub
    End Class
End Namespace