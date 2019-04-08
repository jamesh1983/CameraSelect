Imports System.Data.OleDb
Imports System.Windows.Forms

Class MainWindow
    Private Const V As Boolean = True
    Public tempstring As String
    Private Myloading_window As LoadingWindow = New LoadingWindow
    Public MyID As String
    Public MyPassword As String
    Private Enum FoucedWebBrowser
        WebBrowser1
        WebBrowser2
        WebBrowser3
        WebBrowser4
    End Enum
    Private BrowserNow As FoucedWebBrowser
    Private Enum ScreenState
        FullScreen
        SplitScreen
    End Enum
    Private ScreenStatus As ScreenState

    Private Sub Web_browser_Navigating(sender As Object, e As NavigatingCancelEventArgs) Handles web_browser.Navigating, web_browser_2.Navigating, web_browser_3.Navigating, web_browser_4.Navigating
        Myloading_window.Visibility = Visibility.Visible
        mydatagrid.IsEnabled = False
    End Sub

    Private Sub Web_browser_Navigated(sender As Object, e As NavigationEventArgs) Handles web_browser.Navigated, web_browser_2.Navigated, web_browser_3.Navigated, web_browser_4.Navigated
        Myloading_window.Visibility = Visibility.Hidden
        mydatagrid.IsEnabled = True
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim MapItem As Controls.MenuItem
        Dim MapItem2 As Controls.MenuItem
        Dim MyTable As New Data.DataSet()
        Dim MapTable As New Data.DataSet()
        Dim MyConnectionString As String
        Dim OleConn As OleDbConnection
        Dim MySQL As String
        Dim MyAdapter As OleDbDataAdapter
        Dim MapAdapter As OleDbDataAdapter
        Dim RowCount As Int16
        Dim RowCount2 As Int16
        Dim i As Int16
        Dim j As Int16
        Dim address As Uri
        address = New Uri(System.AppDomain.CurrentDomain.BaseDirectory + "Resource/main_map.jpg")
        web_browser.Source = address
        Me.Height = My.Computer.Screen.Bounds.Height
        Me.Width = My.Computer.Screen.Bounds.Width
        Me.WindowState = WindowState.Maximized
        'Myloading_window.Show()
        Try
            MyConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + "Resource/CameraSelect.accdb"
            OleConn = New OleDbConnection(MyConnectionString)
            OleConn.Open()
            MySQL = "SELECT NationName FROM NationName"     '注意这里用到真实的名称
            MyAdapter = New OleDbDataAdapter(MySQL, OleConn)
            MyTable = New Data.DataSet()
            MyAdapter.Fill(MyTable)
            OleConn.Close()
            If MyTable.Tables.Item(0).Rows.Count = 0 Then
                MessageBox.Show("导入Excel失败!失败原因：选择的Excel中没有数据", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
        RowCount = MyTable.Tables(0).Rows.Count
        For i = 0 To RowCount - 1
            MapItem = New Controls.MenuItem With {
                .Height = 30,
                .Header = MyTable.Tables(0).Rows(i).Item(0)
            }
            Try
                MyConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + "Resource/CameraSelect.accdb"
                OleConn = New OleDbConnection(MyConnectionString)
                OleConn.Open()
                MySQL = "SELECT DistrictName FROM MapName WHERE NationName = '" + MapItem.Header + "'"  '注意这里用到真实的名称
                MapAdapter = New OleDbDataAdapter(MySQL, OleConn)
                MapTable = New Data.DataSet()
                MapAdapter.Fill(MapTable)
                OleConn.Close()
                RowCount2 = MapTable.Tables(0).Rows.Count
            Catch ex As Exception
                MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
            For j = 0 To RowCount2 - 1
                MapItem2 = New Controls.MenuItem With {
                    .Height = 30,
                    .Header = MapTable.Tables(0).Rows(j).Item(0)
                }
                MapItem.Items.Add(MapItem2)
                AddHandler MapItem2.Click, AddressOf MenuClickEvent
            Next
            MapMenu.Items.Add(MapItem)
        Next
        MapItem = New Controls.MenuItem With {
                .Height = 30,
                .Header = "全部IP"
            }
        MapMenu.Items.Add(MapItem)
        AddHandler MapItem.Click, AddressOf Menu_ALL_ClickEvent
        Me.MyGridColumn2.Width = New GridLength(720, GridUnitType.Star)
        Me.MyGridColumn3.Width = New GridLength(0, GridUnitType.Star)
        Me.MyGridRow2.Height = New GridLength(120, GridUnitType.Star)
        Me.MyGridRow3.Height = New GridLength(0, GridUnitType.Star)
        ScreenStatus = ScreenState.FullScreen
        BrowserNow = FoucedWebBrowser.WebBrowser1
        '显示登录对话框
        Dim loginFrm As LoginWindow = New LoginWindow
        loginFrm.ShowDialog()
        If loginFrm.bResult = False Then
            Myloading_window.Close()
            Me.Close()
        End If
    End Sub

    Private Sub Menu_ALL_ClickEvent(sender As Object, e As RoutedEventArgs)
        Dim Temp_MenuItem As Controls.MenuItem
        Dim address As Uri
        Temp_MenuItem = CType(sender, Controls.MenuItem)
        address = New Uri(System.AppDomain.CurrentDomain.BaseDirectory + "main_map.jpg")
        web_browser.Source = address
        Try
            Dim MyConnectionString As String
            MyConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + "Resource/CameraSelect.accdb"
            Dim OleConn As New OleDbConnection(MyConnectionString)
            OleConn.Open()
            Dim MySQL As String = "SELECT IP, description FROM IP_Table"
            Dim MyTable As New Data.DataSet()
            Dim MyAdapter As New OleDbDataAdapter(MySQL, OleConn)
            MyAdapter.Fill(MyTable)
            OleConn.Close()
            If MyTable.Tables(0).Rows.Count = 0 Then
                MessageBox.Show("该地区无对应IP内容", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                mydatagrid.ItemsSource = MyTable.Tables(0).DefaultView
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub MenuClickEvent(sender As Object, e As RoutedEventArgs)
        Dim Temp_MenuItem As Controls.MenuItem
        Dim address As Uri
        Temp_MenuItem = CType(sender, Controls.MenuItem)
        address = New Uri(System.AppDomain.CurrentDomain.BaseDirectory + "Resource\背景\地图\" + Temp_MenuItem.Header + ".jpg")
        web_browser.Source = address
        Try
            Dim MyConnectionString As String
            MyConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + "Resource/CameraSelect.accdb"
            Dim OleConn As New OleDbConnection(MyConnectionString)
            OleConn.Open()
            Dim MySQL As String = "SELECT IP, description FROM IP_Table WHERE city = '" + Temp_MenuItem.Header + "'"
            Dim MyTable As New Data.DataSet()
            Dim MyAdapter As New OleDbDataAdapter(MySQL, OleConn)
            MyAdapter.Fill(MyTable)
            OleConn.Close()
            If MyTable.Tables(0).Rows.Count = 0 Then
                MessageBox.Show("该地区无对应IP内容", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                mydatagrid.ItemsSource = ""
            Else
                mydatagrid.ItemsSource = MyTable.Tables(0).DefaultView
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub Mydatagrid_GotMouseCapture(sender As Object, e As Input.MouseEventArgs) Handles mydatagrid.GotMouseCapture

    End Sub

    Private Sub Mydatagrid_GotFocus(sender As Object, e As RoutedEventArgs) Handles mydatagrid.GotFocus
        Dim str As String = mydatagrid.CurrentItem.Row(0)
        Dim address As Uri
        str = "http://Admin:123456@" + str + "/ie.html"
        address = New Uri(str)
        Try
            Select Case ScreenStatus
                Case ScreenState.FullScreen
                    web_browser.Source = address
                Case ScreenState.SplitScreen
                    Select Case BrowserNow
                        Case FoucedWebBrowser.WebBrowser1
                            web_browser.Source = address
                            BrowserNow = FoucedWebBrowser.WebBrowser2
                        Case FoucedWebBrowser.WebBrowser2
                            web_browser_2.Source = address
                            BrowserNow = FoucedWebBrowser.WebBrowser3
                        Case FoucedWebBrowser.WebBrowser3
                            web_browser_3.Source = address
                            BrowserNow = FoucedWebBrowser.WebBrowser4
                        Case FoucedWebBrowser.WebBrowser4
                            web_browser_4.Source = address
                            BrowserNow = FoucedWebBrowser.WebBrowser1
                    End Select
            End Select
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub MenuItem_Close_Click(sender As Object, e As RoutedEventArgs)
        Myloading_window.Close()
        Me.Close()
    End Sub

    Private Sub MI_Open_Click(sender As Object, e As RoutedEventArgs)
        Dim IP_Frm As IP_Input_Form = New IP_Input_Form
        Dim str, str1, str2, str3 As String
        Dim address As Uri
        IP_Frm.ShowDialog()
        str1 = IP_Frm.Return_IP_String
        str2 = IP_Frm.Return_Account_String
        str3 = IP_Frm.Return_Password_String
        Select Case IP_Frm.IP_Return_Value
            Case 0
                str = "http://" + str2 + ":" + str3 + "@" + str1 + "/ie.html"
                address = New Uri(str)
                web_browser.Source = address
            Case 1
                Try
                    Dim MyConnectionString As String = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source="
                    MyConnectionString += System.AppDomain.CurrentDomain.BaseDirectory + "Resource/CameraSelect.accdb"
                    Dim OleConn As New OleDbConnection(MyConnectionString)
                    OleConn.Open()
                    Dim MySQL As String = "INSERT INTO [IP_Table] ([IP], [description]) VALUES('" + str1 + "','" + str2 + str3 + "')"
                    Dim SQL_Comm As New OleDbCommand(MySQL, OleConn)
                    SQL_Comm.ExecuteNonQuery()
                    OleConn.Close()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try
            Case 2
        End Select
    End Sub

    Private Sub MI_Setup_Click(sender As Object, e As RoutedEventArgs)
        Dim Setup_Frm As Personal_Setup = New Personal_Setup
        Dim Temp_str1, Temp_str2, Temp_str3, Temp_str4 As String
        Setup_Frm.ShowDialog()
        Temp_str1 = Setup_Frm.TB_Account.Text
        Temp_str2 = Setup_Frm.TB_Old_Password.Password
        Temp_str3 = Setup_Frm.TB_New_Password.Password
        Temp_str4 = Setup_Frm.TB_comfirm_Password.Password
        If Setup_Frm.PS_Return_Value Then
            Try
                Dim MyConnectionString As String
                MyConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + "Resource/CameraSelect.accdb"
                Dim OleConn As New OleDbConnection(MyConnectionString)
                OleConn.Open()
                Dim MySQL As String = "SELECT User, Password FROM User_Table WHERE User = '" + Temp_str1 + "'"
                Dim MyDataSet As New Data.DataSet()
                Dim MyAdapter As New OleDbDataAdapter(MySQL, OleConn)
                MyAdapter.Fill(MyDataSet)
                OleConn.Close()
                If MyDataSet.Tables(0).Rows.Count = 0 Then
                    MessageBox.Show("该用户不存在", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    If Temp_str2 = MyDataSet.Tables(0).Rows(0).Item(1).ToString() Then
                        If Temp_str3 = Temp_str4 Then
                            Try
                                Dim UpdateConnString As String = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source="
                                UpdateConnString += System.AppDomain.CurrentDomain.BaseDirectory + "Resource/CameraSelect.accdb"
                                Dim UpdConn As New OleDbConnection(UpdateConnString)
                                UpdConn.Open()
                                Dim UpdSQL As String = "UPDATE User_Table SET [Password] = '" + Temp_str3 + "' WHERE [User] = '" + Temp_str1 + "'"
                                Dim SQL_Comm As New OleDbCommand(UpdSQL, UpdConn)
                                SQL_Comm.ExecuteNonQuery()
                                UpdConn.Close()
                            Catch ex As Exception
                                MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End Try
                        Else
                            MessageBox.Show("密码确认错误", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    Else
                        MessageBox.Show("原始密码错误", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        End If
    End Sub

    Private Sub My_About_Box_Click(sender As Object, e As RoutedEventArgs)
        Dim MyAboutWindow As MyAboutBox = New MyAboutBox()
        MyAboutWindow.ShowDialog()
    End Sub

    Private Sub InPutFile_Click(sender As Object, e As RoutedEventArgs)
        Dim MyInputWindow As MyFileWin = New MyFileWin()
        Dim FileNameStr As String
        MyInputWindow.IsInput = True
        If MyInputWindow.ShowDialog() = Forms.DialogResult.OK Then
            FileNameStr = MyInputWindow.FileNameString
            Try
                Dim MyConnectionString As String = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source="
                MyConnectionString += FileNameStr
                Dim OleConn As New OleDbConnection(MyConnectionString)
                OleConn.Open()
                Dim MySQL As String = "SELECT IP, description FROM IP_Table"
                Dim MyDataSet As New Data.DataSet()
                Dim Original_DataAdapter As New OleDbDataAdapter(MySQL, OleConn)
                'Dim MySQLCommandBuilder As New OleDbCommandBuilder(MyAdapter)
                Original_DataAdapter.Fill(MyDataSet)
                OleConn.Close()
                Dim NewConnStr As String = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source="
                NewConnStr += System.AppDomain.CurrentDomain.BaseDirectory + "Resource/CameraSelect.accdb"
                Dim NewOleConn As New OleDbConnection(NewConnStr)
                NewOleConn.Open()
                Dim NewDataAdapter As New OleDbDataAdapter(MySQL, NewOleConn)
                Dim MySQLCommandBuilder As New OleDbCommandBuilder(NewDataAdapter)
                NewDataAdapter.Update(MyDataSet.Tables(0))
                NewOleConn.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
            MessageBox.Show("输入完成！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub My_Output_Click(sender As Object, e As RoutedEventArgs)
        Dim MyOutputWindow As MyFileWin = New MyFileWin()
        Dim FileNameStr As String
        Dim xlApp As Microsoft.Office.Interop.Excel.Application '定义EXCEL类 
        Dim xlBook As Microsoft.Office.Interop.Excel.Workbook '定义工件簿类 
        Dim xlsheet As Microsoft.Office.Interop.Excel.Worksheet '定义工作表类 
        Dim xlTable As Microsoft.Office.Interop.Excel.DataTable
        FileNameStr = MyOutputWindow.FileNameString
        MyOutputWindow.IsInput = False
        If MyOutputWindow.ShowDialog() = Forms.DialogResult.OK Then
            Dim MyConnectionString As String = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source="
            MyConnectionString += System.AppDomain.CurrentDomain.BaseDirectory + "Resource/CameraSelect.accdb"
            Dim OleConn As New OleDbConnection(MyConnectionString)
            OleConn.Open()
            Dim MySQL As String = "SELECT IP, description FROM IP_Table"
            Dim MyDataSet As New Data.DataSet()
            Dim Original_DataAdapter As New OleDbDataAdapter(MySQL, OleConn)
            'Dim MySQLCommandBuilder As New OleDbCommandBuilder(MyAdapter)
            Original_DataAdapter.Fill(MyDataSet)
            OleConn.Close()

            xlApp = New Microsoft.Office.Interop.Excel.Application
            xlApp = CreateObject("Excel.Application")
            xlApp.Visible = False
            xlBook = xlApp.Workbooks.Add
            xlsheet = xlBook.Worksheets(1)
            xlsheet.Name = "IP_Table"
            xlTable = xlsheet.QueryTables
            xlTable = MyDataSet.Tables(0)
            xlBook.SaveAs(FileNameStr)
            xlBook.Close()
            xlApp.Quit()
            MessageBox.Show("输出完成！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub FullScreenBT(sender As Object, e As RoutedEventArgs)
        Me.MyGridColumn2.Width = New GridLength(720, GridUnitType.Star)
        Me.MyGridColumn3.Width = New GridLength(0, GridUnitType.Star)
        Me.MyGridRow2.Height = New GridLength(120, GridUnitType.Star)
        Me.MyGridRow3.Height = New GridLength(0, GridUnitType.Star)
        ScreenStatus = ScreenState.FullScreen
    End Sub

    Private Sub SplitScreenBT(sender As Object, e As RoutedEventArgs)
        Me.MyGridColumn2.Width = New GridLength(360, GridUnitType.Star)
        Me.MyGridColumn3.Width = New GridLength(360, GridUnitType.Star)
        Me.MyGridRow2.Height = New GridLength(60, GridUnitType.Star)
        Me.MyGridRow3.Height = New GridLength(60, GridUnitType.Star)
        ScreenStatus = ScreenState.SplitScreen
    End Sub

    Private Sub SearchBT_Click(sender As Object, e As RoutedEventArgs) Handles SearchBt.Click
        Dim TempSearchStr As String
        TempSearchStr = SearchingInput.Text
        Try
            Dim MyConnectionString As String
            MyConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + "Resource/CameraSelect.accdb"
            Dim OleConn As New OleDbConnection(MyConnectionString)
            OleConn.Open()
            Dim MySQL As String = "SELECT IP, description FROM IP_Table WHERE IP LIKE '" + TempSearchStr + "'" + " or description LIKE '" + TempSearchStr + "'"
            Dim MyTable As New Data.DataSet()
            Dim MyAdapter As New OleDbDataAdapter(MySQL, OleConn)
            MyAdapter.Fill(MyTable)
            OleConn.Close()
            If MyTable.Tables(0).Rows.Count = 0 Then
                MessageBox.Show("无对应内容", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                mydatagrid.ItemsSource = ""
            Else
                mydatagrid.ItemsSource = MyTable.Tables(0).DefaultView
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub Communication_Info_Click(sender As Object, e As RoutedEventArgs)
        MessageBox.Show("请访问系统目录下CameraSelect.accdb数据库获取相应信息。", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub WebMouseClick(sender As Object, e As MouseButtonEventArgs) Handles web_browser.MouseLeftButtonUp
        If ScreenStatus = ScreenState.FullScreen Then
            Me.MyGridColumn2.Width = New GridLength(360, GridUnitType.Star)
            Me.MyGridColumn3.Width = New GridLength(360, GridUnitType.Star)
            Me.MyGridRow2.Height = New GridLength(60, GridUnitType.Star)
            Me.MyGridRow3.Height = New GridLength(60, GridUnitType.Star)
            ScreenStatus = ScreenState.SplitScreen
        ElseIf ScreenStatus = ScreenState.SplitScreen Then
            Me.MyGridColumn2.Width = New GridLength(720, GridUnitType.Star)
            Me.MyGridColumn3.Width = New GridLength(0, GridUnitType.Star)
            Me.MyGridRow2.Height = New GridLength(120, GridUnitType.Star)
            Me.MyGridRow3.Height = New GridLength(0, GridUnitType.Star)
            ScreenStatus = ScreenState.FullScreen
        End If
    End Sub
End Class
