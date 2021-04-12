Public Class Step2

    Dim Licence As String = <a><![CDATA[

  The intellectual owner has licensed this software as "Freeware", 
 allowing its use free of charge, for an unlimited time, 
 as well as its distribution, although subject to the following LIMITATIONS:

    -The sale, in any way, of the software is not allowed.
    -Modification, in any way, of the software is not allowed.

 The simple installation and / or execution of the software implies the total acceptance, 
 by the user, of the previous limitations and the "conditions of use agreement" detailed below, in ALL its terms, being bound by them.

 If the user does not agree to any term of this Agreement, he will not be authorized to install And/Or use the Software.                                                      
 
]]></a>.Value



    Private Sub Step2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
       
    End Sub

    Public Sub CheckBoxes()
        If Core.Helper.MainForm IsNot Nothing Then
            Core.Helper.MainForm.ContinueButton.Enabled = AcceptRadioButton.Checked
        End If
    End Sub

    Private Sub AcceptRadioButton_CheckedChanged(sender As Object) Handles AcceptRadioButton.CheckedChanged
        If Core.Helper.MainForm IsNot Nothing Then
            Core.Helper.MainForm.ContinueButton.Enabled = AcceptRadioButton.Checked
        End If
   End Sub

End Class