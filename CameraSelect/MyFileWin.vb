Public Class MyFileWin
    Public FileNameString As String
    Private Sub MyFileWin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'OpenFileWin.Filter = "*.xlsx"
        If OpenFileWin.ShowDialog() = Forms.DialogResult.OK Then
            FileNameString = OpenFileWin.FileName
        End If
        Me.Close()
    End Sub
End Class