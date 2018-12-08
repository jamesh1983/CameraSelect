Public Class IP_Input_Form
    Public IP_Return_Value As Integer   '0:confirm 1:add   2:cancel
    Public Return_IP_String, Return_Account_String, Return_Password_String As String
    Private Sub IP_Cancel_Click(sender As Object, e As RoutedEventArgs)
        Return_IP_String = ""
        Return_Account_String = ""
        Return_Password_String = ""
        IP_Return_Value = 2
        Me.Close()
    End Sub

    Private Sub Add_Click(sender As Object, e As RoutedEventArgs)
        Return_IP_String = TB_IP.Text
        Return_Account_String = TB_ID.Text
        Return_Password_String = TB_PW.Text
        IP_Return_Value = 1
        Me.Close()
    End Sub

    Private Sub IP_Confirm_Click(sender As Object, e As RoutedEventArgs)
        Return_IP_String = TB_IP.Text
        Return_Account_String = TB_ID.Text
        Return_Password_String = TB_PW.Text
        IP_Return_Value = 0
        Me.Close()
    End Sub
End Class
