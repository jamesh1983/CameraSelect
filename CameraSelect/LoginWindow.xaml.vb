Public Class LoginWindow
    Public bResult As Boolean = False

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        If username.Text = "admin" And password.Password = "admin" Then
            bResult = True
            Me.Close()
        Else
            MsgBox("请输入正确的用户名和密码！", , "提示")
        End If
    End Sub
End Class
