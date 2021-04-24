Imports System.ComponentModel

Public Class NodeConsole
    Private Sub NodeConsole_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        e.Cancel = True
        Visible = False
    End Sub
End Class