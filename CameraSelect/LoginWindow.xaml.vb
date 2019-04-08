Imports System.Data.OleDb

Public Class LoginWindow
    Public bResult As Boolean = False

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim ID_Table As New Data.DataSet()
        Dim ComfirmationString As String
        'Dim MyConnectionString As String ' = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + "istanbul.xlsx;Extended Properties='Excel 8.0;HDR=False;IMEX=1'"
        ComfirmationString = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + "CameraSelect.accdb"
        Dim UPConn As New OleDbConnection(ComfirmationString)
        UPConn.Open()
        Dim MyIDSQL As String = "SELECT User, Password FROM User_Table"
        Dim MyIDAdapter As New OleDbDataAdapter(MyIDSQL, UPConn)
        MyIDAdapter.Fill(ID_Table)
        UPConn.Close()
        If (username.Text = ID_Table.Tables(0).Rows(0).Item(0).ToString()) And (password.Password = ID_Table.Tables(0).Rows(0).Item(1).ToString()) Then
            bResult = True
            Me.Close()
        Else
            MsgBox("请输入正确的用户名和密码！", , "提示")
        End If
    End Sub

    Private Sub LoginWin_Close_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
End Class
