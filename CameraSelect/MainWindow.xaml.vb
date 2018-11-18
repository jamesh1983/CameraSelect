Imports System.Data.OleDb
Imports System.Windows.Forms

Class MainWindow
    Private Const V As Boolean = True
    Private Sub MainWindow_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized
        Dim address As Uri = New Uri(System.AppDomain.CurrentDomain.BaseDirectory + "main_map.jpg")
        'address = New Uri("http://Admin:123456@88.248.108.72/ie.html")
        web_browser.Source = address
        Me.Height = My.Computer.Screen.Bounds.Height
        Me.Width = My.Computer.Screen.Bounds.Width
        Me.WindowState = WindowState.Maximized
    End Sub

    Private Sub Web_browser_Navigating(sender As Object, e As NavigatingCancelEventArgs) Handles web_browser.Navigating

    End Sub

    Private Sub Web_browser_Navigated(sender As Object, e As NavigationEventArgs) Handles web_browser.Navigated

    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim loginFrm As LoginWindow = New LoginWindow
        '显示登录对话框
        'loginFrm.ShowDialog()
        '合法进入系统
        'If loginFrm.AcceptButton = True Then
        '    Dim frm As FrmMainWindows = New FrmMainWindows
        '    Application.Run(FrmMainWindows)
        'End If
        'If loginFrm.bResult = False Then
        '    Me.Close()
        'End If
    End Sub
    Private Function MenuItem_Click() As Object
        Throw New NotImplementedException()
    End Function

    Private Sub mydatagrid_GotMouseCapture(sender As Object, e As Input.MouseEventArgs) Handles mydatagrid.GotMouseCapture

    End Sub

    Private Sub AS_1_Click(sender As Object, e As RoutedEventArgs) Handles AS_1.Click
        Try
            Dim MyConnectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + "istanbul.xlsx;Extended Properties='Excel 8.0;HDR=False;IMEX=1'"
            Dim OleConn As New OleDbConnection(MyConnectionString)
            'OleConn.Open()
            Dim MySQL As String = "SELECT * FROM DataTable"     '注意这里用到真实的名称
            Dim MyTable As New Data.DataSet()
            Dim MyAdapter As New OleDbDataAdapter(MySQL, OleConn)
            MyAdapter.Fill(MyTable)
            OleConn.Close()
            If MyTable.Tables.Item(0).Rows.Count = 0 Then
                MessageBox.Show("导入Excel失败!失败原因：选择的Excel中没有数据", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'GeneralCommon.Gp_MsgBoxDisplay("导入Excel失败!失败原因：选择的Excel中没有数据", "W", "错误提示")
            Else
                mydatagrid.ItemsSource = MyTable.Tables(0).DefaultView
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub mydatagrid_GotFocus(sender As Object, e As RoutedEventArgs) Handles mydatagrid.GotFocus
        Dim str As String = mydatagrid.CurrentItem.Row(0)
        Dim address As Uri
        str = "http://Admin:123456@" + str + "/ie.html"
        address = New Uri(str)
        web_browser.Source = address
    End Sub
End Class
