Public Class MyFileWin
    Public FileNameString As String
    Public IsInput As Boolean
    Private Sub MyFileWin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'OpenFileWin.Filter = "*.xlsx"
        If IsInput Then
            If OpenFileWin.ShowDialog() = Forms.DialogResult.OK Then
                FileNameString = OpenFileWin.FileName
            End If
        Else
            If SaveFileWin.ShowDialog() = Forms.DialogResult.OK Then
                FileNameString = OpenFileWin.FileName
            End If
        End If
        Me.Close()
    End Sub
End Class