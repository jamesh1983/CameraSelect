Public Class Personal_Setup
    Public PS_Return_Value As Boolean
    Private Sub Setup_Confirm_Click(sender As Object, e As RoutedEventArgs)
        PS_Return_Value = True
        Me.Close()
    End Sub

    Private Sub Setup_Cancel_Click(sender As Object, e As RoutedEventArgs)
        PS_Return_Value = False
        Me.Close()
    End Sub
End Class
