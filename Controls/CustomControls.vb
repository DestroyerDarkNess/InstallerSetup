
Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text

Namespace Controls

#Region " Helpers "

    Module DrawHelpers1
        Public Function RoundRectangle(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
            Dim P As GraphicsPath = New GraphicsPath()
            Dim ArcRectangleWidth As Integer = Curve * 2
            P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
            P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
            P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
            P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
            P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
            Return P
        End Function

        Public Function RoundRect(x!, y!, w!, h!, Optional r! = 0.3, Optional TL As Boolean = True, Optional TR As Boolean = True, Optional BR As Boolean = True, Optional BL As Boolean = True) As GraphicsPath
            Dim d! = Math.Min(w, h) * r, xw = x + w, yh = y + h
            RoundRect = New GraphicsPath
            With RoundRect
                If TL Then .AddArc(x, y, d, d, 180, 90) Else .AddLine(x, y, x, y)
                If TR Then .AddArc(xw - d, y, d, d, 270, 90) Else .AddLine(xw, y, xw, y)
                If BR Then .AddArc(xw - d, yh - d, d, d, 0, 90) Else .AddLine(xw, yh, xw, yh)
                If BL Then .AddArc(x, yh - d, d, d, 90, 90) Else .AddLine(x, yh, x, yh)
                .CloseFigure()
            End With
        End Function
    End Module
    Enum MouseState As Byte
        None = 0
        Over = 1
        Down = 2
        Block = 3
    End Enum

#End Region

    Namespace KnightTheme

        Enum MouseStateKnight
            None = 0
            Over = 1
            Down = 2
        End Enum

        Class KnightTheme
            Inherits ContainerControl

#Region " Declarations "
            Private _Header As Integer = 38
            Private _Down As Boolean = False
            Private _MousePoint As Point
#End Region

#Region " MouseStates "

            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)
                _Down = False
            End Sub

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                If e.Location.Y < _Header AndAlso e.Button = Windows.Forms.MouseButtons.Left Then
                    _Down = True
                    _MousePoint = e.Location
                End If
            End Sub

            Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
                MyBase.OnMouseMove(e)
                If _Down = True Then
                    ParentForm.Location = MousePosition - _MousePoint
                End If
            End Sub

#End Region

            Protected Overrides Sub OnCreateControl()
                MyBase.OnCreateControl()
                ParentForm.FormBorderStyle = FormBorderStyle.None
                ParentForm.TransparencyKey = Color.Fuchsia
                Dock = DockStyle.Fill
                Invalidate()
            End Sub

            Sub New()
                BackColor = Color.FromArgb(46, 49, 61)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                MyBase.OnPaint(e)
                Dim G = e.Graphics
                G.Clear(Color.FromArgb(236, 73, 99))
                G.FillRectangle(New SolidBrush(Color.FromArgb(46, 49, 61)), New Rectangle(0, _Header, Width, Height - _Header))
                G.FillRectangle(Brushes.Fuchsia, New Rectangle(0, 0, 1, 1))
                G.FillRectangle(Brushes.Fuchsia, New Rectangle(1, 0, 1, 1))
                G.FillRectangle(Brushes.Fuchsia, New Rectangle(0, 1, 1, 1))
                G.FillRectangle(Brushes.Fuchsia, New Rectangle(Width - 1, 0, 1, 1))
                G.FillRectangle(Brushes.Fuchsia, New Rectangle(Width - 2, 0, 1, 1))
                G.FillRectangle(Brushes.Fuchsia, New Rectangle(Width - 1, 1, 1, 1))
                Dim _StringF As New StringFormat
                _StringF.Alignment = StringAlignment.Center
                _StringF.LineAlignment = StringAlignment.Center
                G.DrawString(Text, New Font("Segoe UI", 12), Brushes.White, New RectangleF(0, 0, Width, _Header), _StringF)
            End Sub

        End Class

        Class KnightButton
            Inherits Control

#Region " Declarations "
            Private _State As MouseStateKnight = MouseState.None
#End Region

#Region " MouseStates "

            Protected Overrides Sub OnMouseEnter(e As EventArgs)
                MyBase.OnMouseEnter(e)
                _State = MouseStateKnight.Over
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                _State = MouseStateKnight.None
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                _State = MouseStateKnight.Down
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)
                _State = MouseStateKnight.Over
                Invalidate()
            End Sub

#End Region

#Region " Properties "
            Private _Rounded As Boolean
            Public Property RoundedCorners() As Boolean
                Get
                    Return _Rounded
                End Get
                Set(ByVal value As Boolean)
                    _Rounded = value
                End Set
            End Property
#End Region

            Sub New()
                Size = New Size(90, 30)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                MyBase.OnPaint(e)
                Dim G = e.Graphics
                G.Clear(Color.FromArgb(236, 73, 99))
                Select Case _State
                    Case MouseStateKnight.Over
                        G.FillRectangle(New SolidBrush(Color.FromArgb(25, Color.White)), New Rectangle(0, 0, Width, Height))

                    Case MouseStateKnight.Down
                        G.FillRectangle(New SolidBrush(Color.FromArgb(25, Color.Black)), New Rectangle(0, 0, Width, Height))
                End Select
                If _Rounded Then
                    G.FillRectangle(New SolidBrush(Parent.BackColor), New Rectangle(0, 0, 1, 1))
                    G.FillRectangle(New SolidBrush(Parent.BackColor), New Rectangle(Width - 1, 0, 1, 1))
                    G.FillRectangle(New SolidBrush(Parent.BackColor), New Rectangle(0, Height - 1, 1, 1))
                    G.FillRectangle(New SolidBrush(Parent.BackColor), New Rectangle(Width - 1, Height - 1, 1, 1))
                End If
                Dim _StringF As New StringFormat
                _StringF.Alignment = StringAlignment.Center
                _StringF.LineAlignment = StringAlignment.Center
                G.DrawString(Text, New Font("Segoe UI", 10), Brushes.White, New RectangleF(0, 0, Width - 1, Height - 1), _StringF)
            End Sub
        End Class

        Class KnightGroupBox
            Inherits ContainerControl

            Sub New()
                Size = New Size(200, 100)
                BackColor = Color.FromArgb(37, 39, 48)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                MyBase.OnPaint(e)
                Dim G = e.Graphics
                G.Clear(BackColor)
                G.DrawRectangle(New Pen(Color.FromArgb(236, 73, 99)), New Rectangle(0, 0, Width - 1, Height - 1))
                G.FillRectangle(New SolidBrush(Color.FromArgb(46, 49, 61)), New Rectangle(0, 0, 1, 1))
                G.FillRectangle(New SolidBrush(Color.FromArgb(46, 49, 61)), New Rectangle(Width - 1, 0, 1, 1))
                G.FillRectangle(New SolidBrush(Color.FromArgb(46, 49, 61)), New Rectangle(0, Height - 1, 1, 1))
                G.FillRectangle(New SolidBrush(Color.FromArgb(46, 49, 61)), New Rectangle(Width - 1, Height - 1, 1, 1))
                G.DrawString(Text, New Font("Segoe UI", 10), Brushes.White, New Point(7, 5))
            End Sub

        End Class

        <DefaultEvent("CheckedChanged")>
        Class KnightRadioButton
            Inherits Control

#Region " Variables"
            Private _Checked As Boolean
#End Region

#Region " Properties"
            Property Checked() As Boolean
                Get
                    Return _Checked
                End Get
                Set(value As Boolean)
                    _Checked = value
                    InvalidateControls()
                    RaiseEvent CheckedChanged(Me)
                    Invalidate()
                End Set
            End Property
            Event CheckedChanged(ByVal sender As Object)
            Protected Overrides Sub OnClick(e As EventArgs)
                If Not _Checked Then Checked = True
                MyBase.OnClick(e)
            End Sub
            Private Sub InvalidateControls()
                If Not IsHandleCreated OrElse Not _Checked Then Return
                For Each C As Control In Parent.Controls
                    If C IsNot Me AndAlso TypeOf C Is KnightRadioButton Then
                        DirectCast(C, KnightRadioButton).Checked = False
                        Invalidate()
                    End If
                Next
            End Sub
            Protected Overrides Sub OnCreateControl()
                MyBase.OnCreateControl()
                InvalidateControls()
            End Sub


            Protected Overrides Sub OnResize(e As EventArgs)
                MyBase.OnResize(e)
                Height = 16
            End Sub

#End Region

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                MyBase.OnPaint(e)
                Dim G = e.Graphics
                G.SmoothingMode = 2
                G.TextRenderingHint = 5
                G.Clear(Parent.BackColor)
                If Parent.BackColor = Color.FromArgb(46, 49, 61) Then
                    G.FillEllipse(New SolidBrush(Color.FromArgb(37, 39, 48)), New Rectangle(0, 0, 15, 15))
                Else
                    G.FillEllipse(New SolidBrush(Color.FromArgb(24, 25, 31)), New Rectangle(0, 0, 15, 15))
                End If
                If Checked Then
                    G.FillEllipse(New SolidBrush(Color.FromArgb(236, 73, 99)), New Rectangle(4, 4, 7, 7))
                End If
                G.DrawString(Text, New Font("Segoe UI", 10), Brushes.White, New Point(18, -2))
            End Sub

        End Class

        <DefaultEvent("CheckedChanged")>
        Class KnightCheckBox
            Inherits Control

#Region "Variables"
            Private _Checked As Boolean
#End Region

#Region " Properties"
            Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
                MyBase.OnTextChanged(e)
                Invalidate()
            End Sub

            Property Checked() As Boolean
                Get
                    Return _Checked
                End Get
                Set(ByVal value As Boolean)
                    _Checked = value
                    Invalidate()
                End Set
            End Property

            Event CheckedChanged(ByVal sender As Object)
            Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
                If Not _Checked Then
                    Checked = True
                Else
                    Checked = False
                End If
                RaiseEvent CheckedChanged(Me)
                MyBase.OnClick(e)
            End Sub

            Protected Overrides Sub OnResize(e As EventArgs)
                MyBase.OnResize(e)
                Height = 16
            End Sub

#End Region


            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                MyBase.OnPaint(e)
                Dim G = e.Graphics
                G.Clear(BackColor)

                If Parent.BackColor = Color.FromArgb(46, 49, 61) Then
                    G.FillRectangle(New SolidBrush(Color.FromArgb(37, 39, 48)), New Rectangle(0, 0, 15, 15))
                Else
                    G.FillRectangle(New SolidBrush(Color.FromArgb(24, 25, 31)), New Rectangle(0, 0, 15, 15))
                End If
                If Checked Then
                    G.FillRectangle(New SolidBrush(Color.FromArgb(236, 73, 99)), New Rectangle(4, 4, 7, 7))
                End If

                G.DrawString(Text, New Font("Segoe UI", 10), Brushes.White, New Point(18, -2))

            End Sub
        End Class

        <DefaultEvent("TextChanged")>
        Class KnightTextBox
            Inherits Control

#Region " Variables"
            Private WithEvents _TextBox As Windows.Forms.TextBox
#End Region

#Region " Properties"

#Region " TextBox Properties"

            Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
            <Category("Options")> _
            Property TextAlign() As HorizontalAlignment
                Get
                    Return _TextAlign
                End Get
                Set(ByVal value As HorizontalAlignment)
                    _TextAlign = value
                    If _TextBox IsNot Nothing Then
                        _TextBox.TextAlign = value
                    End If
                End Set
            End Property
            Private _MaxLength As Integer = 32767
            <Category("Options")> _
            Property MaxLength() As Integer
                Get
                    Return _MaxLength
                End Get
                Set(ByVal value As Integer)
                    _MaxLength = value
                    If _TextBox IsNot Nothing Then
                        _TextBox.MaxLength = value
                    End If
                End Set
            End Property
            Private _ReadOnly As Boolean
            <Category("Options")> _
            Property [ReadOnly]() As Boolean
                Get
                    Return _ReadOnly
                End Get
                Set(ByVal value As Boolean)
                    _ReadOnly = value
                    If _TextBox IsNot Nothing Then
                        _TextBox.ReadOnly = value
                    End If
                End Set
            End Property
            Private _UseSystemPasswordChar As Boolean
            <Category("Options")> _
            Property UseSystemPasswordChar() As Boolean
                Get
                    Return _UseSystemPasswordChar
                End Get
                Set(ByVal value As Boolean)
                    _UseSystemPasswordChar = value
                    If _TextBox IsNot Nothing Then
                        _TextBox.UseSystemPasswordChar = value
                    End If
                End Set
            End Property
            Private _Multiline As Boolean
            <Category("Options")> _
            Property Multiline() As Boolean
                Get
                    Return _Multiline
                End Get
                Set(ByVal value As Boolean)
                    _Multiline = value
                    If _TextBox IsNot Nothing Then
                        _TextBox.Multiline = value

                        If value Then
                            _TextBox.Height = Height - 11
                        Else
                            Height = _TextBox.Height + 11
                        End If

                    End If
                End Set
            End Property
            <Category("Options")> _
            Overrides Property Text As String
                Get
                    Return MyBase.Text
                End Get
                Set(ByVal value As String)
                    MyBase.Text = value
                    If _TextBox IsNot Nothing Then
                        _TextBox.Text = value
                    End If
                End Set
            End Property
            <Category("Options")> _
            Overrides Property Font As Font
                Get
                    Return MyBase.Font
                End Get
                Set(ByVal value As Font)
                    MyBase.Font = value
                    If _TextBox IsNot Nothing Then
                        _TextBox.Font = value
                        _TextBox.Location = New Point(3, 5)
                        _TextBox.Width = Width - 6

                        If Not _Multiline Then
                            Height = _TextBox.Height + 11
                        End If
                    End If
                End Set
            End Property

            Protected Overrides Sub OnCreateControl()
                MyBase.OnCreateControl()
                If Not Controls.Contains(_TextBox) Then
                    Controls.Add(_TextBox)
                End If
            End Sub
            Private Sub OnBaseTextChanged(ByVal s As Object, ByVal e As EventArgs)
                Text = _TextBox.Text
            End Sub
            Private Sub OnBaseKeyDown(ByVal s As Object, ByVal e As KeyEventArgs)
                If e.Control AndAlso e.KeyCode = Keys.A Then
                    _TextBox.SelectAll()
                    e.SuppressKeyPress = True
                End If
                If e.Control AndAlso e.KeyCode = Keys.C Then
                    _TextBox.Copy()
                    e.SuppressKeyPress = True
                End If
            End Sub
            Protected Overrides Sub OnResize(ByVal e As EventArgs)
                _TextBox.Location = New Point(5, 5)
                _TextBox.Width = Width - 10

                If _Multiline Then
                    _TextBox.Height = Height - 11
                Else
                    Height = _TextBox.Height + 11
                End If
                MyBase.OnResize(e)
            End Sub

#End Region
#End Region
            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                         ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                         ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True

                BackColor = Color.Transparent

                _TextBox = New Windows.Forms.TextBox
                _TextBox.Font = New Font("Segoe UI", 10)
                _TextBox.Text = Text
                _TextBox.BackColor = Color.FromArgb(37, 39, 48)
                _TextBox.ForeColor = Color.White
                _TextBox.MaxLength = _MaxLength
                _TextBox.Multiline = _Multiline
                _TextBox.ReadOnly = _ReadOnly
                _TextBox.UseSystemPasswordChar = _UseSystemPasswordChar
                _TextBox.BorderStyle = BorderStyle.None
                _TextBox.Location = New Point(5, 5)
                _TextBox.Width = Width - 10

                _TextBox.Cursor = Cursors.IBeam

                If _Multiline Then
                    _TextBox.Height = Height - 11
                Else
                    Height = _TextBox.Height + 11
                End If

                AddHandler _TextBox.TextChanged, AddressOf OnBaseTextChanged
                AddHandler _TextBox.KeyDown, AddressOf OnBaseKeyDown
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                MyBase.OnPaint(e)
                Dim G = e.Graphics
                G.Clear(Color.FromArgb(37, 39, 48))
                G.DrawRectangle(New Pen(Color.FromArgb(236, 73, 99)), New Rectangle(0, 0, Width - 1, Height - 1))
            End Sub
        End Class

        Class KnightClose
            Inherits Control

#Region " Declarations "
            Private _State As MouseStateKnight
#End Region

#Region " MouseStates "
            Protected Overrides Sub OnMouseEnter(e As EventArgs)
                MyBase.OnMouseEnter(e)
                _State = MouseStateKnight.Over
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                _State = MouseStateKnight.None
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                _State = MouseStateKnight.Down
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)
                _State = MouseStateKnight.Over
                Invalidate()
            End Sub

            Protected Overrides Sub OnClick(e As EventArgs)
                MyBase.OnClick(e)
                Environment.Exit(0)
            End Sub
#End Region

            Protected Overrides Sub OnResize(e As EventArgs)
                MyBase.OnResize(e)
                Size = New Size(12, 12)
            End Sub

            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                        ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
                DoubleBuffered = True
                Size = New Size(12, 12)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                MyBase.OnPaint(e)
                Dim G = e.Graphics

                G.Clear(Color.FromArgb(236, 73, 99))

                Dim _StringF As New StringFormat
                _StringF.Alignment = StringAlignment.Center
                _StringF.LineAlignment = StringAlignment.Center

                G.DrawString("r", New Font("Marlett", 11), Brushes.White, New RectangleF(0, 0, Width, Height), _StringF)

                Select Case _State
                    Case MouseStateKnight.Over
                        G.DrawString("r", New Font("Marlett", 11), New SolidBrush(Color.FromArgb(25, Color.White)), New RectangleF(0, 0, Width, Height), _StringF)

                    Case MouseStateKnight.Down
                        G.DrawString("r", New Font("Marlett", 11), New SolidBrush(Color.FromArgb(40, Color.Black)), New RectangleF(0, 0, Width, Height), _StringF)

                End Select

            End Sub

        End Class

        Class KnightMini
            Inherits Control

#Region " Declarations "
            Private _State As MouseStateKnight
#End Region

#Region " MouseStates "
            Protected Overrides Sub OnMouseEnter(e As EventArgs)
                MyBase.OnMouseEnter(e)
                _State = MouseStateKnight.Over
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                _State = MouseStateKnight.None
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                _State = MouseStateKnight.Down
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)
                _State = MouseStateKnight.Over
                Invalidate()
            End Sub

            Protected Overrides Sub OnClick(e As EventArgs)
                MyBase.OnClick(e)
                FindForm.WindowState = FormWindowState.Minimized
            End Sub
#End Region

            Protected Overrides Sub OnResize(e As EventArgs)
                MyBase.OnResize(e)
                Size = New Size(12, 12)
            End Sub

            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                        ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
                DoubleBuffered = True
                Size = New Size(12, 12)
            End Sub
            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                MyBase.OnPaint(e)
                Dim G = e.Graphics

                G.Clear(Color.FromArgb(236, 73, 99))

                Dim _StringF As New StringFormat
                _StringF.Alignment = StringAlignment.Center
                _StringF.LineAlignment = StringAlignment.Center

                G.DrawString("0", New Font("Marlett", 11), Brushes.White, New RectangleF(0, 0, Width, Height), _StringF)

                Select Case _State
                    Case MouseStateKnight.Over
                        G.DrawString("0", New Font("Marlett", 11), New SolidBrush(Color.FromArgb(25, Color.White)), New RectangleF(0, 0, Width, Height), _StringF)

                    Case MouseStateKnight.Down
                        G.DrawString("0", New Font("Marlett", 11), New SolidBrush(Color.FromArgb(40, Color.Black)), New RectangleF(0, 0, Width, Height), _StringF)

                End Select

            End Sub

        End Class
    End Namespace

    Namespace CustomRichTextLabel

        ' Examples:
        ' RichTextLabel1.AppendText("My ", Color.White, , New Font("Arial", 12, FontStyle.Bold))
        ' RichTextLabel1.AppendText("RichText-", Color.White, , New Font("Arial", 12, FontStyle.Bold))
        ' RichTextLabel1.AppendText("Label", Color.YellowGreen, Color.Black, New Font("Lucida console", 16, FontStyle.Italic))


        Public Class RichTextLabel : Inherits RichTextBox

            Public Sub New()
                MyBase.Enabled = False
                MyBase.Size = New Point(200, 20)
                MyBase.BorderStyle = Windows.Forms.BorderStyle.None
            End Sub

#Region " Overrided Properties "

            ''' <summary>
            ''' Turn the control backcolor to transparent.
            ''' </summary>
            Protected Overrides ReadOnly Property CreateParams() As CreateParams
                Get
                    Dim cp As CreateParams = MyBase.CreateParams
                    cp.ExStyle = (cp.ExStyle Or 32)
                    Return cp
                End Get
            End Property

#End Region

#Region " Shadowed Properties "

            ' AcceptsTab
            ' Just hidden from the designer and editor.
            <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
            Public Shadows Property AcceptsTab() As Boolean
                Get
                    Return MyBase.AcceptsTab
                End Get
                Set(value As Boolean)
                    MyBase.AcceptsTab = False
                End Set
            End Property

            ' AutoWordSelection
            ' Just hidden from the designer and editor.
            <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
            Public Shadows Property AutoWordSelection() As Boolean
                Get
                    Return MyBase.AutoWordSelection
                End Get
                Set(value As Boolean)
                    MyBase.AutoWordSelection = False
                End Set
            End Property

            ' BackColor
            ' Not hidden, but little hardcoded 'cause the createparams transparency.
            <Browsable(True), EditorBrowsable(EditorBrowsableState.Always)>
            Public Shadows Property BackColor() As Color
                Get
                    Return MyBase.BackColor
                End Get
                Set(value As Color)
                    MyBase.SelectionStart = 0
                    MyBase.SelectionLength = MyBase.TextLength
                    MyBase.SelectionBackColor = value
                    MyBase.BackColor = value
                End Set
            End Property

            ' BorderStyle
            ' Just hidden from the designer and editor.
            <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
            Public Shadows Property BorderStyle() As BorderStyle
                Get
                    Return MyBase.BorderStyle
                End Get
                Set(value As BorderStyle)
                    MyBase.BorderStyle = BorderStyle.None
                End Set
            End Property

            ' Cursor
            ' Hidden from the designer and editor,
            ' because while the control is disabled the cursor always be the default even if changed.
            <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
            Public Shadows Property Cursor() As Cursor
                Get
                    Return MyBase.Cursor
                End Get
                Set(value As Cursor)
                    MyBase.Cursor = Cursors.Default
                End Set
            End Property

            ' Enabled
            ' Hidden from the but not from the editor,
            ' because to prevent exceptions when doing loops over a control collection to disable/enable controls.
            <Browsable(False), EditorBrowsable(EditorBrowsableState.Always)>
            Public Shadows Property Enabled() As Boolean
                Get
                    Return MyBase.Enabled
                End Get
                Set(value As Boolean)
                    MyBase.Enabled = False
                End Set
            End Property

            ' HideSelection
            ' Just hidden from the designer and editor.
            <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
            Public Shadows Property HideSelection() As Boolean
                Get
                    Return MyBase.HideSelection
                End Get
                Set(value As Boolean)
                    MyBase.HideSelection = True
                End Set
            End Property

            ' MaxLength
            ' Just hidden from the designer and editor.
            <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
            Public Shadows Property MaxLength() As Integer
                Get
                    Return MyBase.MaxLength
                End Get
                Set(value As Integer)
                    MyBase.MaxLength = 2147483646
                End Set
            End Property

            ' ReadOnly
            ' Just hidden from the designer and editor.
            <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
            Public Shadows Property [ReadOnly]() As Boolean
                Get
                    Return MyBase.ReadOnly
                End Get
                Set(value As Boolean)
                    MyBase.ReadOnly = True
                End Set
            End Property

            ' ScrollBars
            ' Just hidden from the designer and editor.
            <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
            Public Shadows Property ScrollBars() As RichTextBoxScrollBars
                Get
                    Return MyBase.ScrollBars
                End Get
                Set(value As RichTextBoxScrollBars)
                    MyBase.ScrollBars = RichTextBoxScrollBars.None
                End Set
            End Property

            ' ShowSelectionMargin
            ' Just hidden from the designer and editor.
            <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
            Public Shadows Property ShowSelectionMargin() As Boolean
                Get
                    Return MyBase.ShowSelectionMargin
                End Get
                Set(value As Boolean)
                    MyBase.ShowSelectionMargin = False
                End Set
            End Property

            ' TabStop
            ' Just hidden from the designer and editor.
            <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
            Public Shadows Property TabStop() As Boolean
                Get
                    Return MyBase.TabStop
                End Get
                Set(value As Boolean)
                    MyBase.TabStop = False
                End Set
            End Property

#End Region

#Region " Funcs & Procs "

            ''' <summary>
            ''' Append text to the current text.
            ''' </summary>
            ''' <param name="text">The text to append</param>
            ''' <param name="forecolor">The font color</param>
            ''' <param name="backcolor">The Background color</param>
            ''' <param name="font">The font of the appended text</param>
            Public Overloads Sub AppendText(ByVal text As String, _
                                  ByVal forecolor As Color, _
                                  Optional ByVal backcolor As Color = Nothing, _
                                  Optional ByVal font As Font = Nothing)

                Dim index As Int32 = MyBase.TextLength
                MyBase.AppendText(text)
                MyBase.SelectionStart = index
                MyBase.SelectionLength = MyBase.TextLength - index
                MyBase.SelectionColor = forecolor

                If Not backcolor = Nothing _
                Then MyBase.SelectionBackColor = backcolor _
                Else MyBase.SelectionBackColor = DefaultBackColor

                If font IsNot Nothing Then MyBase.SelectionFont = font

                ' Reset selection
                MyBase.SelectionStart = MyBase.TextLength
                MyBase.SelectionLength = 0

            End Sub

#End Region

        End Class

    End Namespace

    Namespace XylosThemeMod
        Friend Module HelpersXylos

            Enum RoundingStyle As Byte
                All = 0
                Top = 1
                Bottom = 2
                Left = 3
                Right = 4
                TopRight = 5
                BottomRight = 6
            End Enum

            Public Sub CenterString(G As Graphics, T As String, F As Font, C As Color, R As Rectangle)
                Dim TS As SizeF = G.MeasureString(T, F)

                Using B As New SolidBrush(C)
                    G.DrawString(T, F, B, New Point(R.Width / 2 - (TS.Width / 2), R.Height / 2 - (TS.Height / 2)))
                End Using
            End Sub

            Public Function ColorFromHex(Hex As String) As Color
                Return Color.FromArgb(Long.Parse(String.Format("FFFFFFFFFF{0}", Hex.Substring(1)), Globalization.NumberStyles.HexNumber))
            End Function

            Public Function FullRectangle(S As Size, Subtract As Boolean) As Rectangle

                If Subtract Then
                    Return New Rectangle(0, 0, S.Width - 1, S.Height - 1)
                Else
                    Return New Rectangle(0, 0, S.Width, S.Height)
                End If

            End Function

            Public Function RoundRect1(ByVal Rect As Rectangle, ByVal Rounding As Integer, Optional ByVal Style As RoundingStyle = RoundingStyle.All) As Drawing2D.GraphicsPath

                Dim GP As New Drawing2D.GraphicsPath()
                Dim AW As Integer = Rounding * 2

                GP.StartFigure()

                If Rounding = 0 Then
                    GP.AddRectangle(Rect)
                    GP.CloseAllFigures()
                    Return GP
                End If

                Select Case Style
                    Case RoundingStyle.All
                        GP.AddArc(New Rectangle(Rect.X, Rect.Y, AW, AW), -180, 90)
                        GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90)
                        GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90)
                        GP.AddArc(New Rectangle(Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 90, 90)
                    Case RoundingStyle.Top
                        GP.AddArc(New Rectangle(Rect.X, Rect.Y, AW, AW), -180, 90)
                        GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90)
                        GP.AddLine(New Point(Rect.X + Rect.Width, Rect.Y + Rect.Height), New Point(Rect.X, Rect.Y + Rect.Height))
                    Case RoundingStyle.Bottom
                        GP.AddLine(New Point(Rect.X, Rect.Y), New Point(Rect.X + Rect.Width, Rect.Y))
                        GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90)
                        GP.AddArc(New Rectangle(Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 90, 90)
                    Case RoundingStyle.Left
                        GP.AddArc(New Rectangle(Rect.X, Rect.Y, AW, AW), -180, 90)
                        GP.AddLine(New Point(Rect.X + Rect.Width, Rect.Y), New Point(Rect.X + Rect.Width, Rect.Y + Rect.Height))
                        GP.AddArc(New Rectangle(Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 90, 90)
                    Case RoundingStyle.Right
                        GP.AddLine(New Point(Rect.X, Rect.Y + Rect.Height), New Point(Rect.X, Rect.Y))
                        GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90)
                        GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90)
                    Case RoundingStyle.TopRight
                        GP.AddLine(New Point(Rect.X, Rect.Y + 1), New Point(Rect.X, Rect.Y))
                        GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90)
                        GP.AddLine(New Point(Rect.X + Rect.Width, Rect.Y + Rect.Height - 1), New Point(Rect.X + Rect.Width, Rect.Y + Rect.Height))
                        GP.AddLine(New Point(Rect.X + 1, Rect.Y + Rect.Height), New Point(Rect.X, Rect.Y + Rect.Height))
                    Case RoundingStyle.BottomRight
                        GP.AddLine(New Point(Rect.X, Rect.Y + 1), New Point(Rect.X, Rect.Y))
                        GP.AddLine(New Point(Rect.X + Rect.Width - 1, Rect.Y), New Point(Rect.X + Rect.Width, Rect.Y))
                        GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90)
                        GP.AddLine(New Point(Rect.X + 1, Rect.Y + Rect.Height), New Point(Rect.X, Rect.Y + Rect.Height))
                End Select

                GP.CloseAllFigures()

                Return GP

            End Function

        End Module

        Public Class XylosTabControl
            Inherits TabControl

            Private G As Graphics
            Private Rect As Rectangle
            Private _OverIndex As Integer = -1

            Public Property FirstHeaderBorder As Boolean

            Private Property OverIndex As Integer
                Get
                    Return _OverIndex
                End Get
                Set(value As Integer)
                    _OverIndex = value
                    Invalidate()
                End Set
            End Property

            Sub New()
                DoubleBuffered = True
                Alignment = TabAlignment.Left
                SizeMode = TabSizeMode.Fixed
                ItemSize = New Size(40, 180)
            End Sub

            Protected Overrides Sub OnCreateControl()
                MyBase.OnCreateControl()
                SetStyle(ControlStyles.UserPaint, True)
            End Sub

            Protected Overrides Sub OnControlAdded(e As ControlEventArgs)
                MyBase.OnControlAdded(e)
                e.Control.BackColor = Color.White
                e.Control.ForeColor = HelpersXylos.ColorFromHex("#7C858E")
                e.Control.Font = New Font("Segoe UI", 9)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                G = e.Graphics
                G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

                MyBase.OnPaint(e)

                G.Clear(HelpersXylos.ColorFromHex("#FFFFFF"))

                For I As Integer = 0 To TabPages.Count - 1

                    Rect = GetTabRect(I)

                    If String.IsNullOrEmpty(TabPages(I).Tag) Then

                        If SelectedIndex = I Then

                            Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#3375ED")), TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#BECCD9")), TextFont As New Font("Segoe UI semibold", 9)
                                G.FillRectangle(Background, New Rectangle(Rect.X - 5, Rect.Y + 1, Rect.Width + 7, Rect.Height))
                                G.DrawString(TabPages(I).Text, TextFont, TextColor, New Point(Rect.X + 50 + (ItemSize.Height - 180), Rect.Y + 12))
                            End Using

                        Else

                            Using TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#919BA6")), TextFont As New Font("Segoe UI semibold", 9)
                                G.DrawString(TabPages(I).Text, TextFont, TextColor, New Point(Rect.X + 50 + (ItemSize.Height - 180), Rect.Y + 12))
                            End Using

                        End If

                        If Not OverIndex = -1 And Not SelectedIndex = OverIndex Then

                            Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#2F3338")), TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#919BA6")), TextFont As New Font("Segoe UI semibold", 9)
                                G.FillRectangle(Background, New Rectangle(GetTabRect(OverIndex).X - 5, GetTabRect(OverIndex).Y + 1, GetTabRect(OverIndex).Width + 7, GetTabRect(OverIndex).Height))
                                G.DrawString(TabPages(OverIndex).Text, TextFont, TextColor, New Point(GetTabRect(OverIndex).X + 50 + (ItemSize.Height - 180), GetTabRect(OverIndex).Y + 12))
                            End Using

                            If Not IsNothing(ImageList) Then
                                If Not TabPages(OverIndex).ImageIndex < 0 Then
                                    G.DrawImage(ImageList.Images(TabPages(OverIndex).ImageIndex), New Rectangle(GetTabRect(OverIndex).X + 25 + (ItemSize.Height - 180), GetTabRect(OverIndex).Y + ((GetTabRect(OverIndex).Height / 2) - 9), 16, 16))
                                End If
                            End If

                        End If


                        If Not IsNothing(ImageList) Then
                            If Not TabPages(I).ImageIndex < 0 Then
                                G.DrawImage(ImageList.Images(TabPages(I).ImageIndex), New Rectangle(Rect.X + 25 + (ItemSize.Height - 180), Rect.Y + ((Rect.Height / 2) - 9), 16, 16))
                            End If
                        End If

                    Else

                        Using TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#6A7279")), TextFont As New Font("Segoe UI", 7, FontStyle.Bold), Border As New Pen(HelpersXylos.ColorFromHex("#2B2F33"))

                            If FirstHeaderBorder Then
                                G.DrawLine(Border, New Point(Rect.X - 5, Rect.Y + 1), New Point(Rect.Width + 7, Rect.Y + 1))
                            Else
                                If Not I = 0 Then
                                    G.DrawLine(Border, New Point(Rect.X - 5, Rect.Y + 1), New Point(Rect.Width + 7, Rect.Y + 1))
                                End If
                            End If

                            G.DrawString(TabPages(I).Text.ToUpper, TextFont, TextColor, New Point(Rect.X + 25 + (ItemSize.Height - 180), Rect.Y + 16))

                        End Using

                    End If

                Next

            End Sub

            Protected Overrides Sub OnSelecting(e As TabControlCancelEventArgs)
                MyBase.OnSelecting(e)

                If Not IsNothing(e.TabPage) Then
                    If Not String.IsNullOrEmpty(e.TabPage.Tag) Then
                        e.Cancel = True
                    Else
                        OverIndex = -1
                    End If
                End If

            End Sub

            Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
                MyBase.OnMouseMove(e)

                For I As Integer = 0 To TabPages.Count - 1
                    If GetTabRect(I).Contains(e.Location) And Not SelectedIndex = I And String.IsNullOrEmpty(TabPages(I).Tag) Then
                        OverIndex = I
                        Exit For
                    Else
                        OverIndex = -1
                    End If
                Next

            End Sub

            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                OverIndex = -1
            End Sub

        End Class

        Public Class XylosCheckBox
            Inherits Control

            Public Event CheckedChanged(sender As Object, e As EventArgs)

            Private _Checked As Boolean
            Private _EnabledCalc As Boolean
            Private G As Graphics

            Private B64Enabled As String = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAA00lEQVQ4T6WTwQ2CMBSG30/07Ci6gY7gxZoIiYADuAIrsIDpQQ/cHMERZBOuXHimDSWALYL01EO/L//724JmLszk6S+BCOIExFsmL50sEH4kAZxVciYuJgnacD16Plpgg8tFtYMILntQdSXiZ3aXqa1UF/yUsoDw4wKglQaZZPa4RW3JEKzO4RjEbyJaN1BL8gvWgsMp3ADeq0lRJ2FimLZNYWpmFbudUJdolXTLyG2wTmDODUiccEfgSDIIfwmMxAMStS+XHPZn7l/z6Ifk+nSzBR8zi2d9JmVXSgAAAABJRU5ErkJggg=="
            Private B64Disabled As String = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAA1UlEQVQ4T6WTzQ2CQBCF56EnLpaiXvUAJBRgB2oFtkALdEAJnoVEMIGzdEIFjNkFN4DLn+xpD/N9efMWQAsPFvL0lyBMUg8MiwzyZwuiJAuI6CyTMxezBC24EuSTBTp4xaaN6JWdqKQbge6udfB1pfbBjrMvEMZZAdCm3ilw7eO1KRmCxRyiOH0TsFUQs5KMwVLweKY7ALFKUZUTECD6qdquCxM7i9jNhLJEraQ5xZzrYJngO9crGYBbAm2SEfhHoCQGeeK+Ls1Ld+fuM0/+kPp+usWCD10idEOGa4QuAAAAAElFTkSuQmCC"

            Public Property Checked As Boolean
                Get
                    Return _Checked
                End Get
                Set(value As Boolean)
                    _Checked = value
                    Invalidate()
                End Set
            End Property

            Public Shadows Property Enabled As Boolean
                Get
                    Return EnabledCalc
                End Get
                Set(value As Boolean)
                    _EnabledCalc = value

                    If Enabled Then
                        Cursor = Cursors.Hand
                    Else
                        Cursor = Cursors.Default
                    End If

                    Invalidate()
                End Set
            End Property


            Public Property EnabledCalc As Boolean
                Get
                    Return _EnabledCalc
                End Get
                Set(value As Boolean)
                    Enabled = value
                    Invalidate()
                End Set
            End Property

            Sub New()
                DoubleBuffered = True
                Enabled = True
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                G = e.Graphics
                G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

                MyBase.OnPaint(e)

                G.Clear(Color.White)

                If Enabled Then

                    Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#F3F4F7")), Border As New Pen(HelpersXylos.ColorFromHex("#D0D5D9")), TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#7C858E")), TextFont As New Font("Segoe UI", 9)
                        G.FillPath(Background, HelpersXylos.RoundRect1(New Rectangle(0, 0, 16, 16), 3))
                        G.DrawPath(Border, HelpersXylos.RoundRect1(New Rectangle(0, 0, 16, 16), 3))
                        G.DrawString(Text, TextFont, TextColor, New Point(25, 0))
                    End Using

                    If Checked Then

                        Using I As Image = Image.FromStream(New IO.MemoryStream(Convert.FromBase64String(B64Enabled)))
                            G.DrawImage(I, New Rectangle(3, 3, 11, 11))
                        End Using

                    End If

                Else

                    Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#F5F5F8")), Border As New Pen(HelpersXylos.ColorFromHex("#E1E1E2")), TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#D0D3D7")), TextFont As New Font("Segoe UI", 9)
                        G.FillPath(Background, HelpersXylos.RoundRect1(New Rectangle(0, 0, 16, 16), 3))
                        G.DrawPath(Border, HelpersXylos.RoundRect1(New Rectangle(0, 0, 16, 16), 3))
                        G.DrawString(Text, TextFont, TextColor, New Point(25, 0))
                    End Using

                    If Checked Then

                        Using I As Image = Image.FromStream(New IO.MemoryStream(Convert.FromBase64String(B64Disabled)))
                            G.DrawImage(I, New Rectangle(3, 3, 11, 11))
                        End Using

                    End If

                End If

            End Sub

            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)

                If Enabled Then
                    Checked = Not Checked
                    RaiseEvent CheckedChanged(Me, e)
                End If

            End Sub

            Protected Overrides Sub OnResize(e As EventArgs)
                MyBase.OnResize(e)
                Size = New Size(Width, 18)
            End Sub

        End Class

        Public Class XylosRadioButton
            Inherits Control

            Public Event CheckedChanged(sender As Object, e As EventArgs)

            Private _Checked As Boolean
            Private _EnabledCalc As Boolean
            Private G As Graphics

            Public Property Checked As Boolean
                Get
                    Return _Checked
                End Get
                Set(value As Boolean)
                    _Checked = value
                    Invalidate()
                End Set
            End Property

            Public Shadows Property Enabled As Boolean
                Get
                    Return EnabledCalc
                End Get
                Set(value As Boolean)
                    _EnabledCalc = value

                    If Enabled Then
                        Cursor = Cursors.Hand
                    Else
                        Cursor = Cursors.Default
                    End If

                    Invalidate()
                End Set
            End Property

            Public Property EnabledCalc As Boolean
                Get
                    Return _EnabledCalc
                End Get
                Set(value As Boolean)
                    Enabled = value
                    Invalidate()
                End Set
            End Property

            Sub New()
                DoubleBuffered = True
                Enabled = True
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                G = e.Graphics
                G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

                MyBase.OnPaint(e)

                G.Clear(Color.White)

                If Enabled Then

                    Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#F3F4F7")), Border As New Pen(HelpersXylos.ColorFromHex("#D0D5D9")), TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#7C858E")), TextFont As New Font("Segoe UI", 9)
                        G.FillEllipse(Background, New Rectangle(0, 0, 16, 16))
                        G.DrawEllipse(Border, New Rectangle(0, 0, 16, 16))
                        G.DrawString(Text, TextFont, TextColor, New Point(25, 0))
                    End Using

                    If Checked Then

                        Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#575C62"))
                            G.FillEllipse(Background, New Rectangle(4, 4, 8, 8))
                        End Using

                    End If

                Else

                    Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#F5F5F8")), Border As New Pen(HelpersXylos.ColorFromHex("#E1E1E2")), TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#D0D3D7")), TextFont As New Font("Segoe UI", 9)
                        G.FillEllipse(Background, New Rectangle(0, 0, 16, 16))
                        G.DrawEllipse(Border, New Rectangle(0, 0, 16, 16))
                        G.DrawString(Text, TextFont, TextColor, New Point(25, 0))
                    End Using

                    If Checked Then

                        Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#BCC1C6"))
                            G.FillEllipse(Background, New Rectangle(4, 4, 8, 8))
                        End Using

                    End If

                End If

            End Sub

            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)

                If Enabled Then

                    For Each C As Control In Parent.Controls
                        If TypeOf C Is XylosRadioButton Then
                            DirectCast(C, XylosRadioButton).Checked = False
                        End If
                    Next

                    Checked = Not Checked
                    RaiseEvent CheckedChanged(Me, e)
                End If

            End Sub

            Protected Overrides Sub OnResize(e As EventArgs)
                MyBase.OnResize(e)
                Size = New Size(Width, 18)
            End Sub

        End Class

        Public Class XylosNotice
            Inherits TextBox

            Private G As Graphics
            Private B64 As String = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAABL0lEQVQ4T5VT0VGDQBB9e2cBdGBSgTIDEr9MCw7pI0kFtgB9yFiC+KWMmREqMOnAAuDWOfAiudzhyA/svtvH7Xu7BOv5eH2atVKtwbwk0LWGGVyDqLzoRB7e3u/HJTQOdm+PGYjWNuk4ZkIW36RbkzsS7KqiBnB1Usw49DHh8oQEXMfJKhwgAM4/Mw7RIp0NeLG3ScCcR4vVhnTPnVCf9rUZeImTdKnz71VREnBnn5FKzMnX95jA2V6vLufkBQFESTq0WBXsEla7owmcoC6QJMKW2oCUePY5M0lAjK0iBAQ8TBGc2/d7+uvnM/AQNF4Rp4bpiGkRfTb2Gigx12+XzQb3D9JfBGaQzHWm7HS000RJ2i/av5fJjPDZMplErwl1GxDpMTbL1YC5lCwze52/AQFekh7wKBpGAAAAAElFTkSuQmCC"

            Sub New()
                DoubleBuffered = True
                Enabled = False
                [ReadOnly] = True
                BorderStyle = BorderStyle.None
                Multiline = True
                Cursor = Cursors.Default
            End Sub

            Protected Overrides Sub OnCreateControl()
                MyBase.OnCreateControl()
                SetStyle(ControlStyles.UserPaint, True)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                G = e.Graphics
                G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

                MyBase.OnPaint(e)

                G.Clear(Color.White)

                Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#FFFDE8")), MainBorder As New Pen(HelpersXylos.ColorFromHex("#F2F3F7")), TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#B9B595")), TextFont As New Font("Segoe UI", 9)
                    G.FillPath(Background, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
                    G.DrawPath(MainBorder, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
                    G.DrawString(Text, TextFont, TextColor, New Point(30, 6))
                End Using

                Using I As Image = Image.FromStream(New IO.MemoryStream(Convert.FromBase64String(B64)))
                    G.DrawImage(I, New Rectangle(8, Height / 2 - 8, 16, 16))
                End Using

            End Sub

            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)

            End Sub

        End Class

        Public Class XylosTextBox
            Inherits Control

            Enum MouseState As Byte
                None = 0
                Over = 1
                Down = 2
            End Enum

            Private WithEvents TB As New TextBox
            Private G As Graphics
            Private State As MouseState
            Private IsDown As Boolean

            Private _EnabledCalc As Boolean
            Private _allowpassword As Boolean = False
            Private _maxChars As Integer = 32767
            Private _textAlignment As HorizontalAlignment
            Private _multiLine As Boolean = False
            Private _readOnly As Boolean = False

            Public Shadows Property Enabled As Boolean
                Get
                    Return EnabledCalc
                End Get
                Set(value As Boolean)
                    TB.Enabled = value
                    _EnabledCalc = value
                    Invalidate()
                End Set
            End Property

            Public Property EnabledCalc As Boolean
                Get
                    Return _EnabledCalc
                End Get
                Set(value As Boolean)
                    Enabled = value
                    Invalidate()
                End Set
            End Property

            Public Shadows Property UseSystemPasswordChar() As Boolean
                Get
                    Return _allowpassword
                End Get
                Set(ByVal value As Boolean)
                    TB.UseSystemPasswordChar = UseSystemPasswordChar
                    _allowpassword = value
                    Invalidate()
                End Set
            End Property

            Public Shadows Property MaxLength() As Integer
                Get
                    Return _maxChars
                End Get
                Set(ByVal value As Integer)
                    _maxChars = value
                    TB.MaxLength = MaxLength
                    Invalidate()
                End Set
            End Property

            Public Shadows Property TextAlign() As HorizontalAlignment
                Get
                    Return _textAlignment
                End Get
                Set(ByVal value As HorizontalAlignment)
                    _textAlignment = value
                    Invalidate()
                End Set
            End Property

            Public Shadows Property MultiLine() As Boolean
                Get
                    Return _multiLine
                End Get
                Set(ByVal value As Boolean)
                    _multiLine = value
                    TB.Multiline = value
                    OnResize(EventArgs.Empty)
                    Invalidate()
                End Set
            End Property

            Public Shadows Property [ReadOnly]() As Boolean
                Get
                    Return _readOnly
                End Get
                Set(ByVal value As Boolean)
                    _readOnly = value
                    If TB IsNot Nothing Then
                        TB.ReadOnly = value
                    End If
                End Set
            End Property

            Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
                MyBase.OnTextChanged(e)
                Invalidate()
            End Sub

            Protected Overrides Sub OnBackColorChanged(ByVal e As EventArgs)
                MyBase.OnBackColorChanged(e)
                Invalidate()
            End Sub

            Protected Overrides Sub OnForeColorChanged(ByVal e As EventArgs)
                MyBase.OnForeColorChanged(e)
                TB.ForeColor = ForeColor
                Invalidate()
            End Sub

            Protected Overrides Sub OnFontChanged(ByVal e As EventArgs)
                MyBase.OnFontChanged(e)
                TB.Font = Font
            End Sub

            Protected Overrides Sub OnGotFocus(ByVal e As EventArgs)
                MyBase.OnGotFocus(e)
                TB.Focus()
            End Sub

            Private Sub TextChangeTb() Handles TB.TextChanged
                Text = TB.Text
            End Sub

            Private Sub TextChng() Handles MyBase.TextChanged
                TB.Text = Text
            End Sub

            Public Sub NewTextBox()
                With TB
                    .Text = String.Empty
                    .BackColor = Me.BackColor
                    .ForeColor = HelpersXylos.ColorFromHex("#7C858E")
                    .TextAlign = HorizontalAlignment.Left
                    .BorderStyle = BorderStyle.None
                    .Location = New Point(3, 3)
                    .Font = New Font("Segoe UI", 9)
                    .Size = New Size(Width - 3, Height - 3)
                    .UseSystemPasswordChar = UseSystemPasswordChar
                End With
            End Sub

            Sub New()
                MyBase.New()
                NewTextBox()
                Controls.Add(TB)
                SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                TextAlign = HorizontalAlignment.Left
                ForeColor = HelpersXylos.ColorFromHex("#7C858E")
                Font = New Font("Segoe UI", 9)
                Size = New Size(130, 29)
                Enabled = True
            End Sub

            Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

                G = e.Graphics
                G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

                MyBase.OnPaint(e)
                PaintTransparentBackground(Me, e)
                ' G.Clear(Color.White)

                If Enabled Then

                    TB.ForeColor = HelpersXylos.ColorFromHex("#7C858E")
                    TB.BackColor = Me.BackColor
                    If State = MouseState.Down Then

                        Using Border As New Pen(HelpersXylos.ColorFromHex("#78B7E6"))
                            G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 12))
                        End Using

                    Else

                        Using Border As New Pen(HelpersXylos.ColorFromHex("#D0D5D9"))
                            G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 12))
                        End Using

                    End If

                Else

                    TB.ForeColor = HelpersXylos.ColorFromHex("#7C858E")

                    Using Border As New Pen(HelpersXylos.ColorFromHex("#E1E1E2")) '
                        G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 12))
                    End Using

                End If

                TB.TextAlign = TextAlign
                TB.UseSystemPasswordChar = UseSystemPasswordChar

            End Sub

            Private Sub PaintTransparentBackground(ByVal c As Control, ByVal e As PaintEventArgs)
                If c.Parent Is Nothing OrElse Not Application.RenderWithVisualStyles Then Return
                ButtonRenderer.DrawParentBackground(e.Graphics, c.ClientRectangle, c)
            End Sub

            Protected Overrides Sub OnResize(e As EventArgs)
                MyBase.OnResize(e)
                If Not MultiLine Then
                    Dim tbheight As Integer = TB.Height
                    TB.Location = New Point(10, CType(((Height / 2) - (tbheight / 2) - 0), Integer))
                    TB.Size = New Size(Width - 20, tbheight)
                Else
                    TB.Location = New Point(10, 10)
                    TB.Size = New Size(Width - 20, Height - 20)
                End If
            End Sub

            Protected Overrides Sub OnEnter(e As EventArgs)
                MyBase.OnEnter(e)
                State = MouseState.Down : Invalidate()
            End Sub

            Protected Overrides Sub OnLeave(e As EventArgs)
                MyBase.OnLeave(e)
                State = MouseState.None : Invalidate()
            End Sub

        End Class

        Public Class XylosProgressBar
            Inherits Control

#Region " Drawing "

            Private _Val As Integer = 0
            Private _Min As Integer = 0
            Private _Max As Integer = 100

            Public Property Stripes As Color = Color.DarkGreen
            Public Property BackgroundColor As Color = Color.Green


            Public Property Value As Integer
                Get
                    Return _Val
                End Get
                Set(value As Integer)
                    _Val = value
                    Invalidate()
                End Set
            End Property

            Public Property Minimum As Integer
                Get
                    Return _Min
                End Get
                Set(value As Integer)
                    _Min = value
                    Invalidate()
                End Set
            End Property

            Public Property Maximum As Integer
                Get
                    Return _Max
                End Get
                Set(value As Integer)
                    _Max = value
                    Invalidate()
                End Set
            End Property

            Sub New()
                DoubleBuffered = True
                Maximum = 100
                Minimum = 0
                Value = 0
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)

                Dim G As Graphics = e.Graphics
                G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

                MyBase.OnPaint(e)
                PaintTransparentBackground(Me, e)
                '    G.Clear(Color.White)

                Using Border As New Pen(HelpersXylos.ColorFromHex("#D0D5D9"))
                    G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 6))
                End Using

                If Not Value = 0 Then

                    Using Background As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.LightUpwardDiagonal, Stripes, BackgroundColor)
                        G.FillPath(Background, HelpersXylos.RoundRect1(New Rectangle(0, 0, Value / Maximum * Width - 1, Height - 1), 6))
                    End Using

                End If


            End Sub

            Private Sub PaintTransparentBackground(ByVal c As Control, ByVal e As PaintEventArgs)
                If c.Parent Is Nothing OrElse Not Application.RenderWithVisualStyles Then Return
                ButtonRenderer.DrawParentBackground(e.Graphics, c.ClientRectangle, c)
            End Sub

#End Region

        End Class

        Public Class XylosCombobox
            Inherits ComboBox

            Private G As Graphics
            Private Rect As Rectangle
            Private _EnabledCalc As Boolean

            Public Shadows Property Enabled As Boolean
                Get
                    Return EnabledCalc
                End Get
                Set(value As Boolean)
                    _EnabledCalc = value
                    Invalidate()
                End Set
            End Property

            Public Property EnabledCalc As Boolean
                Get
                    Return _EnabledCalc
                End Get
                Set(value As Boolean)
                    MyBase.Enabled = value
                    Enabled = value
                    Invalidate()
                End Set
            End Property

            Sub New()
                DoubleBuffered = True
                DropDownStyle = ComboBoxStyle.DropDownList
                Cursor = Cursors.Hand
                Enabled = True
                DrawMode = DrawMode.OwnerDrawFixed
                ItemHeight = 20
            End Sub

            Protected Overrides Sub OnCreateControl()
                MyBase.OnCreateControl()
                SetStyle(ControlStyles.UserPaint, True)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                G = e.Graphics
                G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

                MyBase.OnPaint(e)

                G.Clear(Color.White)

                If Enabled Then

                    Using Border As New Pen(HelpersXylos.ColorFromHex("#D0D5D9")), TriangleColor As New SolidBrush(HelpersXylos.ColorFromHex("#7C858E")), TriangleFont As New Font("Marlett", 13)
                        G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 6))
                        G.DrawString("6", TriangleFont, TriangleColor, New Point(Width - 22, 3))
                    End Using

                Else

                    Using Border As New Pen(HelpersXylos.ColorFromHex("#E1E1E2")), TriangleColor As New SolidBrush(HelpersXylos.ColorFromHex("#D0D3D7")), TriangleFont As New Font("Marlett", 13)
                        G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 6))
                        G.DrawString("6", TriangleFont, TriangleColor, New Point(Width - 22, 3))
                    End Using

                End If

                If Not IsNothing(Items) Then

                    Using ItemsFont As New Font("Segoe UI", 9), ItemsColor As New SolidBrush(HelpersXylos.ColorFromHex("#7C858E"))

                        If Enabled Then

                            If Not SelectedIndex = -1 Then
                                G.DrawString(GetItemText(Items(SelectedIndex)), ItemsFont, ItemsColor, New Point(7, 4))
                            Else
                                Try
                                    G.DrawString(GetItemText(Items(0)), ItemsFont, ItemsColor, New Point(7, 4))
                                Catch
                                End Try
                            End If

                        Else

                            Using DisabledItemsColor As New SolidBrush(HelpersXylos.ColorFromHex("#D0D3D7"))

                                If Not SelectedIndex = -1 Then
                                    G.DrawString(GetItemText(Items(SelectedIndex)), ItemsFont, DisabledItemsColor, New Point(7, 4))
                                Else
                                    G.DrawString(GetItemText(Items(0)), ItemsFont, DisabledItemsColor, New Point(7, 4))
                                End If

                            End Using

                        End If

                    End Using

                End If

            End Sub

            Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
                MyBase.OnDrawItem(e)
                G = e.Graphics
                G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

                If Enabled Then
                    e.DrawBackground()
                    Rect = e.Bounds

                    Try

                        Using ItemsFont As New Font("Segoe UI", 9), Border As New Pen(HelpersXylos.ColorFromHex("#D0D5D9"))

                            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then

                                Using ItemsColor As New SolidBrush(Color.White), Itembackground As New SolidBrush(HelpersXylos.ColorFromHex("#78B7E6"))
                                    G.FillRectangle(Itembackground, Rect)
                                    G.DrawString(GetItemText(Items(e.Index)), New Font("Segoe UI", 9), Brushes.White, New Point(Rect.X + 5, Rect.Y + 1))
                                End Using

                            Else
                                Using ItemsColor As New SolidBrush(HelpersXylos.ColorFromHex("#7C858E"))
                                    G.FillRectangle(Brushes.White, Rect)
                                    G.DrawString(GetItemText(Items(e.Index)), New Font("Segoe UI", 9), ItemsColor, New Point(Rect.X + 5, Rect.Y + 1))
                                End Using

                            End If

                        End Using

                    Catch
                    End Try

                End If

            End Sub

            Protected Overrides Sub OnSelectedItemChanged(ByVal e As EventArgs)
                MyBase.OnSelectedItemChanged(e)
                Invalidate()
            End Sub

        End Class

        Public Class XylosSeparator
            Inherits Control

            Private G As Graphics

            Sub New()
                DoubleBuffered = True
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                G = e.Graphics
                G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

                MyBase.OnPaint(e)

                Using C As New Pen(HelpersXylos.ColorFromHex("#EBEBEC"))
                    G.DrawLine(C, New Point(0, 0), New Point(Width, 0))
                End Using

            End Sub

            Protected Overrides Sub OnResize(e As EventArgs)
                MyBase.OnResize(e)
                Size = New Size(Width, 2)
            End Sub

        End Class

        Public Class XylosButton
            Inherits Control

            Enum MouseState As Byte
                None = 0
                Over = 1
                Down = 2
            End Enum

            Private G As Graphics
            Private State As MouseState

            Private _EnabledCalc As Boolean

            Public Shadows Event Click(sender As Object, e As EventArgs)

            Sub New()
                DoubleBuffered = True
                Enabled = True
            End Sub

            Public Shadows Property Enabled As Boolean
                Get
                    Return EnabledCalc
                End Get
                Set(value As Boolean)
                    _EnabledCalc = value
                    Invalidate()
                End Set
            End Property

            Public Property EnabledCalc As Boolean
                Get
                    Return _EnabledCalc
                End Get
                Set(value As Boolean)
                    Enabled = value
                    Invalidate()
                End Set
            End Property

            Private _MauseOverColor As Color = Color.Black
            Public Property MauseOverColor As Color
                Get
                    Return _MauseOverColor
                End Get
                Set(value As Color)
                    _MauseOverColor = value
                    Invalidate()
                End Set
            End Property

            Private _MauseDownColor As Color = Color.FromArgb(44, 51, 67)
            Public Property MauseDownColor As Color
                Get
                    Return _MauseDownColor
                End Get
                Set(value As Color)
                    _MauseDownColor = value
                    Invalidate()
                End Set
            End Property

            Private _BorderColor As Color = Color.FromArgb(59, 68, 88)
            Public Property BorderColor As Color
                Get
                    Return _BorderColor
                End Get
                Set(value As Color)
                    _BorderColor = value
                    Invalidate()
                End Set
            End Property

            Protected Overrides Sub OnPaint(e As PaintEventArgs)

                G = e.Graphics
                G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit
                PaintTransparentBackground(Me, e)
                MyBase.OnPaint(e)

                If Enabled Then

                    Select Case State

                        Case MouseState.Over

                            Using Background As New SolidBrush(_MauseOverColor) 'HelpersXylos.ColorFromHex("#FDFDFD")
                                G.FillPath(Background, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
                            End Using

                        Case MouseState.Down

                            Using Background As New SolidBrush(_MauseDownColor)
                                G.FillPath(Background, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
                            End Using

                        Case Else

                            Using Background As New SolidBrush(Me.BackColor) 'HelpersXylos.ColorFromHex("#F6F6F6")
                                G.FillPath(Background, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
                            End Using

                    End Select

                    Using ButtonFont As New Font("Segoe UI", 9), Border As New Pen(BorderColor) 'HelpersXylos.ColorFromHex("#C3C3C3")
                        G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
                        HelpersXylos.CenterString(G, Text, ButtonFont, Me.ForeColor, HelpersXylos.FullRectangle(Size, False)) 'HelpersXylos.ColorFromHex("#7C858E")
                    End Using

                Else

                    Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#F3F4F7")), Border As New Pen(HelpersXylos.ColorFromHex("#DCDCDC")), ButtonFont As New Font("Segoe UI", 9)
                        G.FillPath(Background, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
                        G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
                        HelpersXylos.CenterString(G, Text, ButtonFont, HelpersXylos.ColorFromHex("#D0D3D7"), HelpersXylos.FullRectangle(Size, False))
                    End Using

                End If

            End Sub

            Private Sub PaintTransparentBackground(ByVal c As Control, ByVal e As PaintEventArgs)
                If c.Parent Is Nothing OrElse Not Application.RenderWithVisualStyles Then Return
                ButtonRenderer.DrawParentBackground(e.Graphics, c.ClientRectangle, c)
            End Sub

            Private Sub buttonBorderRadius(ByRef buttonObj As Object, ByVal borderRadiusINT As Integer)
                Dim p As New Drawing2D.GraphicsPath()
                p.StartFigure()
                'TOP LEFT CORNER
                p.AddArc(New Rectangle(0, 0, borderRadiusINT, borderRadiusINT), 180, 90)
                p.AddLine(40, 0, buttonObj.Width - borderRadiusINT, 0)
                'TOP RIGHT CORNER
                p.AddArc(New Rectangle(buttonObj.Width - borderRadiusINT, 0, borderRadiusINT, borderRadiusINT), -90, 90)
                p.AddLine(buttonObj.Width, 40, buttonObj.Width, buttonObj.Height - borderRadiusINT)
                'BOTTOM RIGHT CORNER
                p.AddArc(New Rectangle(buttonObj.Width - borderRadiusINT, buttonObj.Height - borderRadiusINT, borderRadiusINT, borderRadiusINT), 0, 90)
                p.AddLine(buttonObj.Width - borderRadiusINT, buttonObj.Height, borderRadiusINT, buttonObj.Height)
                'BOTTOM LEFT CORNER
                p.AddArc(New Rectangle(0, buttonObj.Height - borderRadiusINT, borderRadiusINT, borderRadiusINT), 90, 90)
                p.CloseFigure()
                buttonObj.Region = New Region(p)
            End Sub


            Protected Overrides Sub OnMouseEnter(e As EventArgs)
                MyBase.OnMouseEnter(e)
                State = MouseState.Over : Invalidate()
            End Sub

            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                State = MouseState.None : Invalidate()
            End Sub

            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)

                If Enabled Then
                    RaiseEvent Click(Me, e)
                End If

                State = MouseState.Over : Invalidate()
            End Sub

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                State = MouseState.Down : Invalidate()
            End Sub

        End Class
    End Namespace

    Namespace LogInLabelTheme


        ''     DO NOT REMOVE CREDITS! IF YOU USE PLEASE CREDIT!     ''

        ''' <summary>
        ''' LogIn GDI+ Theme
        ''' Creator: Xertz (HF)
        ''' Version: 1.4
        ''' Control Count: 28
        ''' Date Created: 18/12/2013
        ''' Date Changed: 07/09/2014
        ''' UID: 1602992
        ''' For any bugs / errors, PM me.
        ''' </summary>
        ''' <remarks></remarks>

        Module DrawHelpers

#Region "Functions"


            Public Function RoundRect(x!, y!, w!, h!, Optional r! = 0.3, Optional TL As Boolean = True, Optional TR As Boolean = True, Optional BR As Boolean = True, Optional BL As Boolean = True) As GraphicsPath
                Dim d! = Math.Min(w, h) * r, xw = x + w, yh = y + h
                RoundRect = New GraphicsPath
                With RoundRect
                    If TL Then .AddArc(x, y, d, d, 180, 90) Else .AddLine(x, y, x, y)
                    If TR Then .AddArc(xw - d, y, d, d, 270, 90) Else .AddLine(xw, y, xw, y)
                    If BR Then .AddArc(xw - d, yh - d, d, d, 0, 90) Else .AddLine(xw, yh, xw, yh)
                    If BL Then .AddArc(x, yh - d, d, d, 90, 90) Else .AddLine(x, yh, x, yh)
                    .CloseFigure()
                End With
            End Function

            Enum MouseState As Byte
                None = 0
                Over = 1
                Down = 2
                Block = 3
            End Enum

#End Region

        End Module

        Public Class LogInThemeContainer
            Inherits ContainerControl

#Region "Declarations"
            Private _CloseChoice As __CloseChoice = __CloseChoice.Form
            Private _FontSize As Integer = 12
            Private ReadOnly _Font As Font = New Font("Segoe UI", _FontSize)
            Private MouseXLoc As Integer
            Private MouseYLoc As Integer
            Private CaptureMovement As Boolean = False
            Private Const MoveHeight As Integer = 35
            Private MouseP As Point = New Point(0, 0)
            Private _FontColour As Color = Color.FromArgb(255, 255, 255)
            Private _BaseColour As Color = Color.FromArgb(35, 35, 35)
            Private _ContainerColour As Color = Color.FromArgb(54, 54, 54)
            Private _BorderColour As Color = Color.FromArgb(60, 60, 60)
            Private _HoverColour As Color = Color.FromArgb(42, 42, 42)
#End Region

#Region "Size Handling"

            Private _LockWidth As Integer
            Protected Property LockWidth() As Integer
                Get
                    Return _LockWidth
                End Get
                Set(ByVal value As Integer)
                    _LockWidth = value
                    If Not LockWidth = 0 AndAlso IsHandleCreated Then Width = LockWidth
                End Set
            End Property

            Private _LockHeight As Integer
            Protected Property LockHeight() As Integer
                Get
                    Return _LockHeight
                End Get
                Set(ByVal value As Integer)
                    _LockHeight = value
                    If Not LockHeight = 0 AndAlso IsHandleCreated Then Height = LockHeight
                End Set
            End Property

            Private Frame As Rectangle
            Protected NotOverridable Overrides Sub OnSizeChanged(ByVal e As EventArgs)
                If _Movable AndAlso Not _ControlMode Then
                    Frame = New Rectangle(7, 7, Width - 14, 35)
                End If

                Invalidate()

                MyBase.OnSizeChanged(e)
            End Sub

            Protected Overrides Sub SetBoundsCore(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal specified As BoundsSpecified)
                If Not _LockWidth = 0 Then width = _LockWidth
                If Not _LockHeight = 0 Then height = _LockHeight
                MyBase.SetBoundsCore(x, y, width, height, specified)
            End Sub

#End Region

#Region "State Handling"

            Private State As MouseState = MouseState.None
            Private Sub SetState(ByVal current As MouseState)
                State = current
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
                If Not (_IsParentForm AndAlso ParentForm.WindowState = FormWindowState.Maximized) Then
                    If _Sizable AndAlso Not _ControlMode Then InvalidateMouse()
                End If
                MyBase.OnMouseMove(e)
                SetState(MouseState.Over)
            End Sub

            Protected Overrides Sub OnEnabledChanged(ByVal e As EventArgs)
                If Enabled Then SetState(MouseState.None) Else SetState(MouseState.Block)
                MyBase.OnEnabledChanged(e)
            End Sub

            Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
                SetState(MouseState.Over)
                MyBase.OnMouseEnter(e)
            End Sub

            Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
                SetState(MouseState.Over)
                MyBase.OnMouseUp(e)
            End Sub

            Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
                SetState(MouseState.None)
                If GetChildAtPoint(PointToClient(MousePosition)) IsNot Nothing Then
                    If _Sizable AndAlso Not _ControlMode Then
                        Cursor = Cursors.Default
                        Previous = 0
                    End If
                End If
                MyBase.OnMouseLeave(e)
            End Sub

            Private GetMouseLocation As Point
            Private OldSize As Size
            Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
                If e.Button = Windows.Forms.MouseButtons.Left Then SetState(MouseState.Down)
                If Not (_IsParentForm AndAlso ParentForm.WindowState = FormWindowState.Maximized OrElse _ControlMode) Then
                    If _Movable AndAlso Frame.Contains(e.Location) Then
                        Capture = False
                        WM_LMBUTTONDOWN = True
                        DefWndProc(Messages(0))
                    ElseIf _Sizable AndAlso Not Previous = 0 Then
                        Capture = False
                        WM_LMBUTTONDOWN = True
                        DefWndProc(Messages(Previous))
                    End If
                End If
                GetMouseLocation = PointToClient(MousePosition)
                If GetMouseLocation.X > Width - 39 AndAlso GetMouseLocation.X < Width - 16 AndAlso GetMouseLocation.Y < 22 Then
                    If _AllowClose Then
                        If _CloseChoice = __CloseChoice.Application Then
                            Environment.Exit(0)
                        Else
                            ParentForm.Close()
                        End If
                    End If
                ElseIf GetMouseLocation.X > Width - 64 AndAlso GetMouseLocation.X < Width - 41 AndAlso GetMouseLocation.Y < 22 Then
                    If _AllowMaximize Then
                        Select Case FindForm.WindowState
                            Case FormWindowState.Maximized
                                FindForm.WindowState = FormWindowState.Normal
                            Case FormWindowState.Normal
                                OldSize = Size
                                FindForm.WindowState = FormWindowState.Maximized
                        End Select
                    End If
                ElseIf GetMouseLocation.X > Width - 89 AndAlso GetMouseLocation.X < Width - 66 AndAlso GetMouseLocation.Y < 22 Then
                    If _AllowMinimize Then
                        Select Case FindForm.WindowState
                            Case FormWindowState.Normal
                                OldSize = Size
                                FindForm.WindowState = FormWindowState.Minimized
                            Case FormWindowState.Maximized
                                FindForm.WindowState = FormWindowState.Minimized
                        End Select
                    End If
                End If
                MyBase.OnMouseDown(e)
            End Sub

            Private Messages(8) As Message
            Private Sub InitializeMessages()
                Messages(0) = Message.Create(Parent.Handle, 161, New IntPtr(2), IntPtr.Zero)
                For I As Integer = 1 To 8
                    Messages(I) = Message.Create(Parent.Handle, 161, New IntPtr(I + 9), IntPtr.Zero)
                Next
            End Sub

            Private GetIndexPoint As Point
            Private B1, B2, B3, B4 As Boolean
            Private Function GetMouseIndex() As Integer
                GetIndexPoint = PointToClient(MousePosition)
                B1 = GetIndexPoint.X < 6
                B2 = GetIndexPoint.X > Width - 6
                B3 = GetIndexPoint.Y < 6
                B4 = GetIndexPoint.Y > Height - 6
                If B1 AndAlso B3 Then Return 4
                If B1 AndAlso B4 Then Return 7
                If B2 AndAlso B3 Then Return 5
                If B2 AndAlso B4 Then Return 8
                If B1 Then Return 1
                If B2 Then Return 2
                If B3 Then Return 3
                If B4 Then Return 6
                Return 0
            End Function

            Private Current, Previous As Integer
            Private Sub InvalidateMouse()
                Current = GetMouseIndex()
                If Current = Previous Then Return
                Previous = Current
                Select Case Previous
                    Case 0
                        Cursor = Cursors.Default
                    Case 1, 2
                        Cursor = Cursors.SizeWE
                    Case 3, 6
                        Cursor = Cursors.SizeNS
                    Case 4, 8
                        Cursor = Cursors.SizeNWSE
                    Case 5, 7
                        Cursor = Cursors.SizeNESW
                End Select
            End Sub

            Private WM_LMBUTTONDOWN As Boolean
            Protected Overrides Sub WndProc(ByRef m As Message)
                MyBase.WndProc(m)

                If WM_LMBUTTONDOWN AndAlso m.Msg = 513 Then
                    WM_LMBUTTONDOWN = False

                    SetState(MouseState.Over)
                    If Not _SmartBounds Then Return

                    If IsParentMdi Then
                        CorrectBounds(New Rectangle(Point.Empty, Parent.Parent.Size))
                    Else
                        Try : CorrectBounds(Screen.FromControl(Parent).WorkingArea) : Catch : End Try
                    End If
                End If
            End Sub

            Private Sub CorrectBounds(ByVal bounds As Rectangle)
                If Parent.Width > bounds.Width Then Parent.Width = bounds.Width
                If Parent.Height > bounds.Height Then Parent.Height = bounds.Height
                Dim X As Integer = Parent.Location.X
                Dim Y As Integer = Parent.Location.Y
                If X < bounds.X Then X = bounds.X
                If Y < bounds.Y Then Y = bounds.Y
                Dim Width As Integer = bounds.X + bounds.Width
                Dim Height As Integer = bounds.Y + bounds.Height
                If X + Parent.Width > Width Then X = Width - Parent.Width
                If Y + Parent.Height > Height Then Y = Height - Parent.Height
                ''Weird allows proper full screen
                '  Parent.Size = New Size(Width, Height)
                If FindForm.WindowState = FormWindowState.Maximized Or FindForm.WindowState = FormWindowState.Minimized Then
                    Parent.Size = OldSize
                End If
            End Sub

            Protected NotOverridable Overrides Sub OnHandleCreated(ByVal e As EventArgs)
                If Not _LockWidth = 0 Then Width = _LockWidth
                If Not _LockHeight = 0 Then Height = _LockHeight
                If Not _ControlMode Then MyBase.Dock = DockStyle.Fill
            End Sub

            Protected NotOverridable Overrides Sub OnParentChanged(ByVal e As EventArgs)
                MyBase.OnParentChanged(e)
                If Parent Is Nothing Then Return
                _IsParentForm = TypeOf Parent Is Form
                If Not _ControlMode Then
                    InitializeMessages()
                    Parent.BackColor = BackColor
                End If
            End Sub

#End Region

#Region "Properties"

            Enum __CloseChoice
                Form
                Application
            End Enum
            Public Property CloseChoice As __CloseChoice
                Get
                    Return _CloseChoice
                End Get
                Set(value As __CloseChoice)
                    _CloseChoice = value
                End Set
            End Property

            Private _Movable As Boolean = True
            Property Movable() As Boolean
                Get
                    Return _Movable
                End Get
                Set(ByVal value As Boolean)
                    _Movable = value
                End Set
            End Property

            Private _Sizable As Boolean = True
            Property Sizable() As Boolean
                Get
                    Return _Sizable
                End Get
                Set(ByVal value As Boolean)
                    _Sizable = value
                End Set
            End Property

            Private _ControlMode As Boolean
            Protected Property ControlMode() As Boolean
                Get
                    Return _ControlMode
                End Get
                Set(ByVal v As Boolean)
                    _ControlMode = v

                    Invalidate()
                End Set
            End Property

            Private _SmartBounds As Boolean = True
            Property SmartBounds() As Boolean
                Get
                    Return _SmartBounds
                End Get
                Set(ByVal value As Boolean)
                    _SmartBounds = value
                End Set
            End Property

            Private _IsParentForm As Boolean
            Protected ReadOnly Property IsParentForm As Boolean
                Get
                    Return _IsParentForm
                End Get
            End Property

            Protected ReadOnly Property IsParentMdi As Boolean
                Get
                    If Parent Is Nothing Then Return False
                    Return Parent.Parent IsNot Nothing
                End Get
            End Property

            <Category("Control")>
            Public Property FontSize As Integer
                Get
                    Return _FontSize
                End Get
                Set(value As Integer)
                    _FontSize = value
                End Set
            End Property

            Private _AllowMinimize As Boolean = True
            <Category("Control")>
            Public Property AllowMinimize As Boolean
                Get
                    Return _AllowMinimize
                End Get
                Set(value As Boolean)
                    _AllowMinimize = value
                End Set
            End Property

            Private _AllowMaximize As Boolean = True
            <Category("Control")>
            Public Property AllowMaximize As Boolean
                Get
                    Return _AllowMaximize
                End Get
                Set(value As Boolean)
                    _AllowMaximize = value
                End Set
            End Property

            Private _ShowIcon As Boolean = True
            <Category("Control")>
            Public Property ShowIcon As Boolean
                Get
                    Return _ShowIcon
                End Get
                Set(value As Boolean)
                    _ShowIcon = value
                    Invalidate()
                End Set
            End Property

            Private _AllowClose As Boolean = True
            <Category("Control")>
            Public Property AllowClose As Boolean
                Get
                    Return _AllowClose
                End Get
                Set(value As Boolean)
                    _AllowClose = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property HoverColour As Color
                Get
                    Return _HoverColour
                End Get
                Set(value As Color)
                    _HoverColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BaseColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property ContainerColour As Color
                Get
                    Return _ContainerColour
                End Get
                Set(value As Color)
                    _ContainerColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property FontColour As Color
                Get
                    Return _FontColour
                End Get
                Set(value As Color)
                    _FontColour = value
                End Set
            End Property

#End Region

#Region "Draw Control"

            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                        ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
                Me.DoubleBuffered = True
                Me.BackColor = _BaseColour
                Me.Dock = DockStyle.Fill
            End Sub

            Protected Overrides Sub OnCreateControl()
                MyBase.OnCreateControl()
                ParentForm.FormBorderStyle = FormBorderStyle.None
                ParentForm.AllowTransparency = False
                ParentForm.TransparencyKey = Color.Fuchsia
                ParentForm.FindForm.StartPosition = FormStartPosition.CenterScreen
                Dock = DockStyle.Fill
                Invalidate()
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)

                Dim G = e.Graphics
                With G
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(0, 0, Width, Height))
                    .FillRectangle(New SolidBrush(_ContainerColour), New Rectangle(2, 35, Width - 4, Height - 37))
                    .DrawRectangle(New Pen(_BorderColour), New Rectangle(0, 0, Width, Height))
                    Dim ControlBoxPoints() As Point = {New Point(Width - 90, 0), New Point(Width - 90, 22), New Point(Width - 15, 22), New Point(Width - 15, 0)}
                    .DrawLines(New Pen(_BorderColour), ControlBoxPoints)
                    .DrawLine(New Pen(_BorderColour), Width - 65, 0, Width - 65, 22)
                    GetMouseLocation = PointToClient(MousePosition)
                    Select Case State
                        Case MouseState.Over
                            If GetMouseLocation.X > Width - 39 AndAlso GetMouseLocation.X < Width - 16 AndAlso GetMouseLocation.Y < 22 Then
                                .FillRectangle(New SolidBrush(_HoverColour), New Rectangle(Width - 39, 0, 23, 22))
                            ElseIf GetMouseLocation.X > Width - 64 AndAlso GetMouseLocation.X < Width - 41 AndAlso GetMouseLocation.Y < 22 Then
                                .FillRectangle(New SolidBrush(_HoverColour), New Rectangle(Width - 64, 0, 23, 22))
                            ElseIf GetMouseLocation.X > Width - 89 AndAlso GetMouseLocation.X < Width - 66 AndAlso GetMouseLocation.Y < 22 Then
                                .FillRectangle(New SolidBrush(_HoverColour), New Rectangle(Width - 89, 0, 23, 22))
                            End If
                    End Select
                    .DrawLine(New Pen(_BorderColour), Width - 40, 0, Width - 40, 22)
                    ''Close Button
                    .DrawLine(New Pen(_FontColour, 2), Width - 33, 6, Width - 22, 16)
                    .DrawLine(New Pen(_FontColour, 2), Width - 33, 16, Width - 22, 6)
                    ''Minimize Button
                    .DrawLine(New Pen(_FontColour), Width - 83, 16, Width - 72, 16)
                    ''Maximize Button
                    .DrawLine(New Pen(_FontColour), Width - 58, 16, Width - 47, 16)
                    .DrawLine(New Pen(_FontColour), Width - 58, 16, Width - 58, 6)
                    .DrawLine(New Pen(_FontColour), Width - 47, 16, Width - 47, 6)
                    .DrawLine(New Pen(_FontColour), Width - 58, 6, Width - 47, 6)
                    .DrawLine(New Pen(_FontColour), Width - 58, 7, Width - 47, 7)
                    If _ShowIcon Then
                        .DrawIcon(FindForm.Icon, New Rectangle(6, 6, 22, 22))
                        .DrawString(Text, _Font, New SolidBrush(_FontColour), New RectangleF(31, 0, Width - 110, 35), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
                    Else
                        .DrawString(Text, _Font, New SolidBrush(_FontColour), New RectangleF(4, 0, Width - 110, 35), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
                    End If
                    .InterpolationMode = CType(7, InterpolationMode)
                End With
            End Sub

#End Region

        End Class

        <DefaultEvent("TextChanged")>
        Public Class LogInUserTextBox
            Inherits Control

#Region "Declarations"
            Private State As MouseState = MouseState.None
            Private WithEvents TB As Windows.Forms.TextBox
            Private _BaseColour As Color = Color.FromArgb(42, 42, 42)
            Private _TextColour As Color = Color.FromArgb(255, 255, 255)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
#End Region

#Region "TextBox Properties"

            Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
            <Category("Options")>
            Property TextAlign() As HorizontalAlignment
                Get
                    Return _TextAlign
                End Get
                Set(ByVal value As HorizontalAlignment)
                    _TextAlign = value
                    If TB IsNot Nothing Then
                        TB.TextAlign = value
                    End If
                End Set
            End Property
            Private _MaxLength As Integer = 32767
            <Category("Options")>
            Property MaxLength() As Integer
                Get
                    Return _MaxLength
                End Get
                Set(ByVal value As Integer)
                    _MaxLength = value
                    If TB IsNot Nothing Then
                        TB.MaxLength = value
                    End If
                End Set
            End Property
            Private _ReadOnly As Boolean
            <Category("Options")>
            Property [ReadOnly]() As Boolean
                Get
                    Return _ReadOnly
                End Get
                Set(ByVal value As Boolean)
                    _ReadOnly = value
                    If TB IsNot Nothing Then
                        TB.ReadOnly = value
                    End If
                End Set
            End Property
            Private _UseSystemPasswordChar As Boolean
            <Category("Options")>
            Property UseSystemPasswordChar() As Boolean
                Get
                    Return _UseSystemPasswordChar
                End Get
                Set(ByVal value As Boolean)
                    _UseSystemPasswordChar = value
                    If TB IsNot Nothing Then
                        TB.UseSystemPasswordChar = value
                    End If
                End Set
            End Property
            Private _Multiline As Boolean
            <Category("Options")>
            Property Multiline() As Boolean
                Get
                    Return _Multiline
                End Get
                Set(ByVal value As Boolean)
                    _Multiline = value
                    If TB IsNot Nothing Then
                        TB.Multiline = value

                        If value Then
                            TB.Height = Height - 11
                        Else
                            Height = TB.Height + 11
                        End If

                    End If
                End Set
            End Property
            <Category("Options")>
            Overrides Property Text As String
                Get
                    Return MyBase.Text
                End Get
                Set(ByVal value As String)
                    MyBase.Text = value
                    If TB IsNot Nothing Then
                        TB.Text = value
                    End If
                End Set
            End Property
            <Category("Options")>
            Overrides Property Font As Font
                Get
                    Return MyBase.Font
                End Get
                Set(ByVal value As Font)
                    MyBase.Font = value
                    If TB IsNot Nothing Then
                        TB.Font = value
                        TB.Location = New Point(3, 5)
                        TB.Width = Width - 35

                        If Not _Multiline Then
                            Height = TB.Height + 11
                        End If
                    End If
                End Set
            End Property

            Protected Overrides Sub OnCreateControl()
                MyBase.OnCreateControl()
                If Not Controls.Contains(TB) Then
                    Controls.Add(TB)
                End If
            End Sub
            Private Sub OnBaseTextChanged(ByVal s As Object, ByVal e As EventArgs)
                Text = TB.Text
            End Sub
            Private Sub OnBaseKeyDown(ByVal s As Object, ByVal e As KeyEventArgs)
                If e.Control AndAlso e.KeyCode = Keys.A Then
                    TB.SelectAll()
                    e.SuppressKeyPress = True
                End If
                If e.Control AndAlso e.KeyCode = Keys.C Then
                    TB.Copy()
                    e.SuppressKeyPress = True
                End If
            End Sub
            Protected Overrides Sub OnResize(ByVal e As EventArgs)
                TB.Location = New Point(5, 5)
                TB.Width = Width - 35

                If _Multiline Then
                    TB.Height = Height - 11
                Else
                    Height = TB.Height + 11
                End If

                MyBase.OnResize(e)
            End Sub

#End Region

#Region "Colour Properties"

            <Category("Colours")>
            Public Property BackgroundColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property TextColour As Color
                Get
                    Return _TextColour
                End Get
                Set(value As Color)
                    _TextColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

#End Region

#Region "Mouse States"

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                State = MouseState.Down : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)
                State = MouseState.Over : TB.Focus() : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                State = MouseState.None : Invalidate()
            End Sub

#End Region

#Region "Draw Control"
            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                         ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                         ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                BackColor = Color.Transparent
                TB = New Windows.Forms.TextBox
                TB.Height = 190
                TB.Font = New Font("Segoe UI", 10)
                TB.Text = Text
                TB.BackColor = Color.FromArgb(42, 42, 42)
                TB.ForeColor = Color.FromArgb(255, 255, 255)
                TB.MaxLength = _MaxLength
                TB.Multiline = False
                TB.ReadOnly = _ReadOnly
                TB.UseSystemPasswordChar = _UseSystemPasswordChar
                TB.BorderStyle = BorderStyle.None
                TB.Location = New Point(5, 5)
                TB.Width = Width - 35
                AddHandler TB.TextChanged, AddressOf OnBaseTextChanged
                AddHandler TB.KeyDown, AddressOf OnBaseKeyDown
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)

                Dim G = e.Graphics
                Dim GP As GraphicsPath
                Dim Base As New Rectangle(0, 0, Width, Height)
                With G
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(BackColor)
                    TB.BackColor = Color.FromArgb(42, 42, 42)
                    TB.ForeColor = Color.FromArgb(255, 255, 255)
                    GP = RoundRectangle(Base, 6)
                    .FillPath(New SolidBrush(Color.FromArgb(42, 42, 42)), GP)
                    .DrawPath(New Pen(New SolidBrush(Color.FromArgb(35, 35, 35)), 2), GP)
                    GP.Dispose()
                    .FillPie(New SolidBrush(FindForm.BackColor), New Rectangle(Width - 25, Height - 23, Height + 25, Height + 25), 180, 90)
                    .DrawPie(New Pen(Color.FromArgb(35, 35, 35), 2), New Rectangle(Width - 25, Height - 23, Height + 25, Height + 25), 180, 90)
                    .InterpolationMode = CType(7, InterpolationMode)
                End With
            End Sub



#End Region

        End Class

        <DefaultEvent("TextChanged")>
        Public Class LogInPassTextBox
            Inherits Control

#Region "Declarations"
            Private State As MouseState = MouseState.None
            Private WithEvents TB As Windows.Forms.TextBox
            Private _BaseColour As Color = Color.FromArgb(255, 255, 255)
            Private _TextColour As Color = Color.FromArgb(50, 50, 50)
            Private _BorderColour As Color = Color.FromArgb(180, 187, 205)
#End Region

#Region "TextBox Properties"

            Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
            <Category("Options")>
            Property TextAlign() As HorizontalAlignment
                Get
                    Return _TextAlign
                End Get
                Set(ByVal value As HorizontalAlignment)
                    _TextAlign = value
                    If TB IsNot Nothing Then
                        TB.TextAlign = value
                    End If
                End Set
            End Property
            Private _MaxLength As Integer = 32767
            <Category("Options")>
            Property MaxLength() As Integer
                Get
                    Return _MaxLength
                End Get
                Set(ByVal value As Integer)
                    _MaxLength = value
                    If TB IsNot Nothing Then
                        TB.MaxLength = value
                    End If
                End Set
            End Property
            Private _ReadOnly As Boolean
            <Category("Options")>
            Property [ReadOnly]() As Boolean
                Get
                    Return _ReadOnly
                End Get
                Set(ByVal value As Boolean)
                    _ReadOnly = value
                    If TB IsNot Nothing Then
                        TB.ReadOnly = value
                    End If
                End Set
            End Property
            Private _UseSystemPasswordChar As Boolean
            <Category("Options")>
            Property UseSystemPasswordChar() As Boolean
                Get
                    Return _UseSystemPasswordChar
                End Get
                Set(ByVal value As Boolean)
                    _UseSystemPasswordChar = value
                    If TB IsNot Nothing Then
                        TB.UseSystemPasswordChar = value
                    End If
                End Set
            End Property
            Private _Multiline As Boolean
            <Category("Options")>
            Property Multiline() As Boolean
                Get
                    Return _Multiline
                End Get
                Set(ByVal value As Boolean)
                    _Multiline = value
                    If TB IsNot Nothing Then
                        TB.Multiline = value

                        If value Then
                            TB.Height = Height - 11
                        Else
                            Height = TB.Height + 11
                        End If

                    End If
                End Set
            End Property
            <Category("Options")>
            Overrides Property Text As String
                Get
                    Return MyBase.Text
                End Get
                Set(ByVal value As String)
                    MyBase.Text = value
                    If TB IsNot Nothing Then
                        TB.Text = value
                    End If
                End Set
            End Property
            <Category("Options")>
            Overrides Property Font As Font
                Get
                    Return MyBase.Font
                End Get
                Set(ByVal value As Font)
                    MyBase.Font = value
                    If TB IsNot Nothing Then
                        TB.Font = value
                        TB.Location = New Point(3, 5)
                        TB.Width = Width - 35

                        If Not _Multiline Then
                            Height = TB.Height + 11
                        End If
                    End If
                End Set
            End Property

            Protected Overrides Sub OnCreateControl()
                MyBase.OnCreateControl()
                If Not Controls.Contains(TB) Then
                    Controls.Add(TB)
                End If
            End Sub
            Private Sub OnBaseTextChanged(ByVal s As Object, ByVal e As EventArgs)
                Text = TB.Text
            End Sub
            Private Sub OnBaseKeyDown(ByVal s As Object, ByVal e As KeyEventArgs)
                If e.Control AndAlso e.KeyCode = Keys.A Then
                    TB.SelectAll()
                    e.SuppressKeyPress = True
                End If
                If e.Control AndAlso e.KeyCode = Keys.C Then
                    TB.Copy()
                    e.SuppressKeyPress = True
                End If
            End Sub
            Protected Overrides Sub OnResize(ByVal e As EventArgs)
                TB.Location = New Point(5, 5)
                TB.Width = Width - 35

                If _Multiline Then
                    TB.Height = Height - 11
                Else
                    Height = TB.Height + 11
                End If

                MyBase.OnResize(e)
            End Sub
#End Region

#Region "Colour Properties"

            <Category("Colours")>
            Public Property BackgroundColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property TextColour As Color
                Get
                    Return _TextColour
                End Get
                Set(value As Color)
                    _TextColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

#End Region

#Region "Mouse States"

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                State = MouseState.Down : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)
                State = MouseState.Over : TB.Focus() : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                State = MouseState.None : Invalidate()
            End Sub

#End Region

#Region "Draw Control"
            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                         ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                         ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                BackColor = Color.Transparent
                TB = New Windows.Forms.TextBox
                TB.Height = 190
                TB.Font = New Font("Segoe UI", 10)
                TB.Text = Text
                TB.BackColor = Color.FromArgb(42, 42, 42)
                TB.ForeColor = Color.FromArgb(255, 255, 255)
                TB.MaxLength = _MaxLength
                TB.Multiline = False
                TB.ReadOnly = _ReadOnly
                TB.UseSystemPasswordChar = _UseSystemPasswordChar
                TB.BorderStyle = BorderStyle.None
                TB.Location = New Point(5, 5)
                TB.Width = Width - 35
                AddHandler TB.TextChanged, AddressOf OnBaseTextChanged
                AddHandler TB.KeyDown, AddressOf OnBaseKeyDown
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                Dim G = e.Graphics
                Dim GP As GraphicsPath
                Dim Base As New Rectangle(0, 0, Width, Height)
                With G
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(BackColor)
                    TB.BackColor = Color.FromArgb(42, 42, 42)
                    TB.ForeColor = Color.FromArgb(255, 255, 255)
                    GP = RoundRectangle(Base, 6)
                    .FillPath(New SolidBrush(Color.FromArgb(42, 42, 42)), GP)
                    .DrawPath(New Pen(New SolidBrush(Color.FromArgb(35, 35, 35)), 2), GP)
                    GP.Dispose()
                    .FillPie(New SolidBrush(FindForm.BackColor), New Rectangle(Width - 25, Height - 60, Height + 25, Height + 25), 90, 90)
                    .DrawPie(New Pen(Color.FromArgb(35, 35, 35), 2), New Rectangle(Width - 25, Height - 60, Height + 25, Height + 25), 90, 90)
                    .FillEllipse(New SolidBrush(_TextColour), New Rectangle(10, 5, 10, 7))
                    .InterpolationMode = CType(7, InterpolationMode)
                End With
            End Sub
#End Region

        End Class

        Public Class LogInLogButton
            Inherits Control

#Region "Declarations"
            Private State As MouseState = MouseState.None
            Private _ArcColour As Color = Color.FromArgb(43, 43, 43)
            Private _ArrowColour As Color = Color.FromArgb(235, 233, 234)
            Private _ArrowBorderColour As Color = Color.FromArgb(170, 170, 170)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
            Private _HoverColour As Color = Color.FromArgb(0, 130, 169)
            Private _PressedColour As Color = Color.FromArgb(0, 145, 184)
            Private _NormalColour As Color = Color.FromArgb(0, 160, 199)
#End Region

#Region "Mouse States"

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                State = MouseState.Down : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)
                State = MouseState.Over : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseEnter(e As EventArgs)
                MyBase.OnMouseEnter(e)
                State = MouseState.Over : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                State = MouseState.None : Invalidate()
            End Sub

#End Region

#Region "Colour Properties"
            <Category("Colours")>
            Public Property ArcColour As Color
                Get
                    Return _ArcColour
                End Get
                Set(value As Color)
                    _ArcColour = value
                End Set
            End Property
            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property
            <Category("Colours")>
            Public Property ArrowColour As Color
                Get
                    Return _ArrowColour
                End Get
                Set(value As Color)
                    _ArrowColour = value
                End Set
            End Property
            <Category("Colours")>
            Public Property ArrowBorderColour As Color
                Get
                    Return _ArrowBorderColour
                End Get
                Set(value As Color)
                    _ArrowBorderColour = value
                End Set
            End Property
            <Category("Colours")>
            Public Property HoverColour As Color
                Get
                    Return _HoverColour
                End Get
                Set(value As Color)
                    _HoverColour = value
                End Set
            End Property
            <Category("Colours")>
            Public Property PressedColour As Color
                Get
                    Return _PressedColour
                End Get
                Set(value As Color)
                    _PressedColour = value
                End Set
            End Property
            <Category("Colours")>
            Public Property NormalColour As Color
                Get
                    Return _NormalColour
                End Get
                Set(value As Color)
                    _NormalColour = value
                End Set
            End Property

            Protected Overrides Sub OnResize(e As EventArgs)
                MyBase.OnResize(e)
                Size = New Size(50, 50)
            End Sub

#End Region

#Region "Draw Control"

            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                        ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                        ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                Size = New Size(50, 50)
                BackColor = Color.FromArgb(54, 54, 54)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)

                Dim G = e.Graphics
                Dim GP, GP1 As New GraphicsPath
                With G
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(BackColor)
                    Dim P() As Point = {New Point(18, 22), New Point(28, 22), New Point(28, 18), New Point(34, 25), New Point(28, 32), New Point(28, 28), New Point(18, 28)}
                    Select Case State
                        Case MouseState.None
                            .FillEllipse(New SolidBrush(Color.FromArgb(56, 56, 56)), New Rectangle(CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 3, Height - 3 - 3))
                            .DrawArc(New Pen(New SolidBrush(_ArcColour), 1 + 3), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 3, Height - 3 - 3, -90, 360)
                            .DrawEllipse(New Pen(_BorderColour), New Rectangle(1, 1, Height - 3, Height - 3))
                            .FillEllipse(New SolidBrush(_NormalColour), New Rectangle(CInt(3 / 2) + 3, CInt(3 / 2) + 3, Height - 11, Height - 11))
                            .FillPolygon(New SolidBrush(_ArrowColour), P)
                            .DrawPolygon(New Pen(_ArrowBorderColour), P)
                        Case MouseState.Over
                            .DrawArc(New Pen(New SolidBrush(_ArcColour), 1 + 3), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 3, Height - 3 - 3, -90, 360)
                            .DrawEllipse(New Pen(_BorderColour), New Rectangle(1, 1, Height - 3, Height - 3))
                            .FillEllipse(New SolidBrush(_HoverColour), New Rectangle(6, 6, Height - 13, Height - 13))
                            .FillPolygon(New SolidBrush(_ArrowColour), P)
                            .DrawPolygon(New Pen(_ArrowBorderColour), P)
                        Case MouseState.Down
                            .DrawArc(New Pen(New SolidBrush(_ArcColour), 1 + 3), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 3, Height - 3 - 3, -90, 360)
                            .DrawEllipse(New Pen(_BorderColour), New Rectangle(1, 1, Height - 3, Height - 3))
                            .FillEllipse(New SolidBrush(_PressedColour), New Rectangle(6, 6, Height - 13, Height - 13))
                            .FillPolygon(New SolidBrush(_ArrowColour), P)
                            .DrawPolygon(New Pen(_ArrowBorderColour), P)
                    End Select
                    GP.Dispose()
                    GP1.Dispose()
                    .InterpolationMode = InterpolationMode.HighQualityBicubic
                End With
            End Sub

#End Region

        End Class

        <DefaultEvent("CheckedChanged")>
        Public Class LogInCheckBox
            Inherits Control

#Region "Declarations"
            Private _Checked As Boolean
            Private State As MouseState = MouseState.None
            Private _CheckedColour As Color = Color.FromArgb(173, 173, 174)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
            Private _BackColour As Color = Color.FromArgb(42, 42, 42)
            Private _TextColour As Color = Color.FromArgb(255, 255, 255)
#End Region

#Region "Colour & Other Properties"

            <Category("Colours")>
            Public Property BaseColour As Color
                Get
                    Return _BackColour
                End Get
                Set(value As Color)
                    _BackColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property CheckedColour As Color
                Get
                    Return _CheckedColour
                End Get
                Set(value As Color)
                    _CheckedColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property FontColour As Color
                Get
                    Return _TextColour
                End Get
                Set(value As Color)
                    _TextColour = value
                End Set
            End Property

            Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
                MyBase.OnTextChanged(e)
                Invalidate()
            End Sub

            Property Checked() As Boolean
                Get
                    Return _Checked
                End Get
                Set(ByVal value As Boolean)
                    _Checked = value
                    Invalidate()
                End Set
            End Property

            Event CheckedChanged(ByVal sender As Object)
            Protected Overrides Sub OnClick(ByVal e As EventArgs)
                _Checked = Not _Checked
                RaiseEvent CheckedChanged(Me)
                MyBase.OnClick(e)
            End Sub

            Protected Overrides Sub OnResize(e As EventArgs)
                MyBase.OnResize(e)
                Height = 22
            End Sub
#End Region

#Region "Mouse States"

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                State = MouseState.Down : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)
                State = MouseState.Over : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseEnter(e As EventArgs)
                MyBase.OnMouseEnter(e)
                State = MouseState.Over : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                State = MouseState.None : Invalidate()
            End Sub

#End Region

#Region "Draw Control"
            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                           ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                Cursor = Cursors.Hand
                Size = New Size(100, 22)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                Dim g = e.Graphics
                Dim Base As New Rectangle(0, 0, 20, 20)
                With g
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(Color.FromArgb(54, 54, 54))
                    .FillRectangle(New SolidBrush(_BackColour), Base)
                    .DrawRectangle(New Pen(_BorderColour), New Rectangle(1, 1, 18, 18))
                    Select Case State
                        Case MouseState.Over
                            .FillRectangle(New SolidBrush(Color.FromArgb(50, 49, 51)), Base)
                            .DrawRectangle(New Pen(_BorderColour), New Rectangle(1, 1, 18, 18))
                    End Select
                    If Checked Then
                        Dim P() As Point = {New Point(4, 11), New Point(6, 8), New Point(9, 12), New Point(15, 3), New Point(17, 6), New Point(9, 16)}
                        .FillPolygon(New SolidBrush(_CheckedColour), P)
                    End If
                    .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(24, 1, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
                    .InterpolationMode = CType(7, InterpolationMode)
                End With
            End Sub
#End Region

        End Class

        Public Class LogInNormalTextBox
            Inherits Control

#Region "Declarations"
            Private State As MouseState = MouseState.None
            Private WithEvents TB As Windows.Forms.TextBox
            Private _BaseColour As Color = Color.FromArgb(42, 42, 42)
            Private _TextColour As Color = Color.FromArgb(255, 255, 255)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
            Private _Style As Styles = Styles.NotRounded
            Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
            Private _MaxLength As Integer = 32767
            Private _ReadOnly As Boolean
            Private _UseSystemPasswordChar As Boolean
            Private _Multiline As Boolean
#End Region

#Region "TextBox Properties"

            Enum Styles
                Rounded
                NotRounded
            End Enum

            <Category("Options")>
            Property TextAlign() As HorizontalAlignment
                Get
                    Return _TextAlign
                End Get
                Set(ByVal value As HorizontalAlignment)
                    _TextAlign = value
                    If TB IsNot Nothing Then
                        TB.TextAlign = value
                    End If
                End Set
            End Property

            <Category("Options")>
            Property MaxLength() As Integer
                Get
                    Return _MaxLength
                End Get
                Set(ByVal value As Integer)
                    _MaxLength = value
                    If TB IsNot Nothing Then
                        TB.MaxLength = value
                    End If
                End Set
            End Property

            <Category("Options")>
            Property [ReadOnly]() As Boolean
                Get
                    Return _ReadOnly
                End Get
                Set(ByVal value As Boolean)
                    _ReadOnly = value
                    If TB IsNot Nothing Then
                        TB.ReadOnly = value
                    End If
                End Set
            End Property

            <Category("Options")>
            Property UseSystemPasswordChar() As Boolean
                Get
                    Return _UseSystemPasswordChar
                End Get
                Set(ByVal value As Boolean)
                    _UseSystemPasswordChar = value
                    If TB IsNot Nothing Then
                        TB.UseSystemPasswordChar = value
                    End If
                End Set
            End Property

            <Category("Options")>
            Property Multiline() As Boolean
                Get
                    Return _Multiline
                End Get
                Set(ByVal value As Boolean)
                    _Multiline = value
                    If TB IsNot Nothing Then
                        TB.Multiline = value

                        If value Then
                            TB.Height = Height - 11
                        Else
                            Height = TB.Height + 11
                        End If

                    End If
                End Set
            End Property

            <Category("Options")>
            Overrides Property Text As String
                Get
                    Return MyBase.Text
                End Get
                Set(ByVal value As String)
                    MyBase.Text = value
                    If TB IsNot Nothing Then
                        TB.Text = value
                    End If
                End Set
            End Property

            <Category("Options")>
            Overrides Property Font As Font
                Get
                    Return MyBase.Font
                End Get
                Set(ByVal value As Font)
                    MyBase.Font = value
                    If TB IsNot Nothing Then
                        TB.Font = value
                        TB.Location = New Point(3, 5)
                        TB.Width = Width - 6

                        If Not _Multiline Then
                            Height = TB.Height + 11
                        End If
                    End If
                End Set
            End Property

            Protected Overrides Sub OnCreateControl()
                MyBase.OnCreateControl()
                If Not Controls.Contains(TB) Then
                    Controls.Add(TB)
                End If
            End Sub

            Private Sub OnBaseTextChanged(ByVal s As Object, ByVal e As EventArgs)
                Text = TB.Text
            End Sub

            Private Sub OnBaseKeyDown(ByVal s As Object, ByVal e As KeyEventArgs)
                If e.Control AndAlso e.KeyCode = Keys.A Then
                    TB.SelectAll()
                    e.SuppressKeyPress = True
                End If
                If e.Control AndAlso e.KeyCode = Keys.C Then
                    TB.Copy()
                    e.SuppressKeyPress = True
                End If
            End Sub

            Protected Overrides Sub OnResize(ByVal e As EventArgs)
                TB.Location = New Point(5, 5)
                TB.Width = Width - 10

                If _Multiline Then
                    TB.Height = Height - 11
                Else
                    Height = TB.Height + 11
                End If

                MyBase.OnResize(e)
            End Sub

            Public Property Style As Styles
                Get
                    Return _Style
                End Get
                Set(value As Styles)
                    _Style = value
                End Set
            End Property

            Public Sub SelectAll()
                TB.Focus()
                TB.SelectAll()
            End Sub


#End Region

#Region "Colour Properties"

            <Category("Colours")>
            Public Property BackgroundColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property TextColour As Color
                Get
                    Return _TextColour
                End Get
                Set(value As Color)
                    _TextColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

#End Region

#Region "Mouse States"

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                State = MouseState.Down : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)
                State = MouseState.Over : TB.Focus() : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                State = MouseState.None : Invalidate()
            End Sub

#End Region

#Region "Draw Control"
            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                         ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                         ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                BackColor = Color.Transparent
                TB = New Windows.Forms.TextBox
                TB.Height = 190
                TB.Font = New Font("Segoe UI", 10)
                TB.Text = Text
                TB.BackColor = Color.FromArgb(42, 42, 42)
                TB.ForeColor = Color.FromArgb(255, 255, 255)
                TB.MaxLength = _MaxLength
                TB.Multiline = False
                TB.ReadOnly = _ReadOnly
                TB.UseSystemPasswordChar = _UseSystemPasswordChar
                TB.BorderStyle = BorderStyle.None
                TB.Location = New Point(5, 5)
                TB.Width = Width - 35
                AddHandler TB.TextChanged, AddressOf OnBaseTextChanged
                AddHandler TB.KeyDown, AddressOf OnBaseKeyDown
            End Sub


            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                Dim g = e.Graphics
                Dim GP As GraphicsPath
                Dim Base As New Rectangle(0, 0, Width, Height)
                With g
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(BackColor)
                    TB.BackColor = Color.FromArgb(42, 42, 42)
                    TB.ForeColor = Color.FromArgb(255, 255, 255)
                    Select Case _Style
                        Case Styles.Rounded
                            GP = RoundRectangle(Base, 6)
                            .FillPath(New SolidBrush(Color.FromArgb(42, 42, 42)), GP)
                            .DrawPath(New Pen(New SolidBrush(Color.FromArgb(35, 35, 35)), 2), GP)
                            GP.Dispose()
                        Case Styles.NotRounded
                            .FillRectangle(New SolidBrush(Color.FromArgb(42, 42, 42)), New Rectangle(0, 0, Width - 1, Height - 1))
                            .DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(35, 35, 35)), 2), New Rectangle(0, 0, Width, Height))
                    End Select
                    .InterpolationMode = CType(7, InterpolationMode)
                End With
            End Sub

#End Region

        End Class

        Public Class LogInRadialProgressBar
            Inherits Control

#Region "Declarations"
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
            Private _BaseColour As Color = Color.FromArgb(42, 42, 42)
            Private _ProgressColour As Color = Color.FromArgb(23, 119, 151)
            Private _Value As Integer = 0
            Private _Maximum As Integer = 100
            Private _StartingAngle As Integer = 110
            Private _RotationAngle As Integer = 255
            Private ReadOnly _Font As Font = New Font("Segoe UI", 20)
#End Region

#Region "Properties"

            <Category("Control")>
            Public Property Maximum() As Integer
                Get
                    Return _Maximum
                End Get
                Set(V As Integer)
                    Select Case V
                        Case Is < _Value
                            _Value = V
                    End Select
                    _Maximum = V
                    Invalidate()
                End Set
            End Property

            <Category("Control")>
            Public Property Value() As Integer
                Get
                    Select Case _Value
                        Case 0
                            Return 0
                        Case Else
                            Return _Value
                    End Select
                End Get

                Set(V As Integer)
                    Select Case V
                        Case Is > _Maximum
                            V = _Maximum
                            Invalidate()
                    End Select
                    _Value = V
                    Invalidate()
                End Set
            End Property

            Public Sub Increment(ByVal Amount As Integer)
                Value += Amount
            End Sub

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property ProgressColour As Color
                Get
                    Return _ProgressColour
                End Get
                Set(value As Color)
                    _ProgressColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BaseColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Control")>
            Public Property StartingAngle As Integer
                Get
                    Return _StartingAngle
                End Get
                Set(value As Integer)
                    _StartingAngle = value
                End Set
            End Property

            <Category("Control")>
            Public Property RotationAngle As Integer
                Get
                    Return _RotationAngle
                End Get
                Set(value As Integer)
                    _RotationAngle = value
                End Set
            End Property

#End Region

#Region "Draw Control"
            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                        ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                        ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                Size = New Size(78, 78)
                BackColor = Color.FromArgb(54, 54, 54)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                Dim G = e.Graphics
                With G
                    .TextRenderingHint = TextRenderingHint.AntiAliasGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(BackColor)
                    Select Case _Value
                        Case 0
                            .DrawArc(New Pen(New SolidBrush(_BorderColour), 1 + 5), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle - 3, _RotationAngle + 5)
                            .DrawArc(New Pen(New SolidBrush(_BaseColour), 1 + 3), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle, _RotationAngle)
                            .DrawString(CStr(_Value), _Font, Brushes.White, New Point(CInt(Width / 2), CInt(Height / 2 - 1)), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                        Case _Maximum
                            .DrawArc(New Pen(New SolidBrush(_BorderColour), 1 + 5), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle - 3, _RotationAngle + 5)
                            .DrawArc(New Pen(New SolidBrush(_BaseColour), 1 + 3), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle, _RotationAngle)
                            .DrawArc(New Pen(New SolidBrush(_ProgressColour), 1 + 3), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle, _RotationAngle)
                            .DrawString(CStr(_Value), _Font, Brushes.White, New Point(CInt(Width / 2), CInt(Height / 2 - 1)), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                        Case Else
                            .DrawArc(New Pen(New SolidBrush(_BorderColour), 1 + 5), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle - 3, _RotationAngle + 5)
                            .DrawArc(New Pen(New SolidBrush(_BaseColour), 1 + 3), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle, _RotationAngle)
                            .DrawArc(New Pen(New SolidBrush(_ProgressColour), 1 + 3), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle, CInt((_RotationAngle / _Maximum) * _Value))
                            .DrawString(CStr(_Value), _Font, Brushes.White, New Point(CInt(Width / 2), CInt(Height / 2 - 1)), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    End Select
                    .InterpolationMode = InterpolationMode.HighQualityBicubic
                End With
            End Sub
#End Region

        End Class

        <DefaultEvent("CheckedChanged")>
        Public Class LogInRadioButton
            Inherits Control

#Region "Declarations"
            Private _Checked As Boolean
            Private State As MouseState = MouseState.None
            Private _HoverColour As Color = Color.FromArgb(50, 49, 51)
            Private _CheckedColour As Color = Color.FromArgb(173, 173, 174)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
            Private _BackColour As Color = Color.FromArgb(54, 54, 54)
            Private _TextColour As Color = Color.FromArgb(255, 255, 255)
#End Region

#Region "Colour & Other Properties"

            <Category("Colours")>
            Public Property HighlightColour As Color
                Get
                    Return _HoverColour
                End Get
                Set(value As Color)
                    _HoverColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BaseColour As Color
                Get
                    Return _BackColour
                End Get
                Set(value As Color)
                    _BackColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property CheckedColour As Color
                Get
                    Return _CheckedColour
                End Get
                Set(value As Color)
                    _CheckedColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property FontColour As Color
                Get
                    Return _TextColour
                End Get
                Set(value As Color)
                    _TextColour = value
                End Set
            End Property

            Event CheckedChanged(ByVal sender As Object)
            Property Checked() As Boolean
                Get
                    Return _Checked
                End Get
                Set(value As Boolean)
                    _Checked = value
                    InvalidateControls()
                    RaiseEvent CheckedChanged(Me)
                    Invalidate()
                End Set
            End Property

            Protected Overrides Sub OnClick(e As EventArgs)
                If Not _Checked Then Checked = True
                MyBase.OnClick(e)
            End Sub
            Private Sub InvalidateControls()
                If Not IsHandleCreated OrElse Not _Checked Then Return
                For Each C As Control In Parent.Controls
                    If C IsNot Me AndAlso TypeOf C Is LogInRadioButton Then
                        DirectCast(C, LogInRadioButton).Checked = False
                        Invalidate()
                    End If
                Next
            End Sub
            Protected Overrides Sub OnCreateControl()
                MyBase.OnCreateControl()
                InvalidateControls()
            End Sub
            Protected Overrides Sub OnResize(e As EventArgs)
                MyBase.OnResize(e)
                Height = 22
            End Sub
#End Region

#Region "Mouse States"

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                State = MouseState.Down : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)
                State = MouseState.Over : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseEnter(e As EventArgs)
                MyBase.OnMouseEnter(e)
                State = MouseState.Over : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                State = MouseState.None : Invalidate()
            End Sub

#End Region

#Region "Draw Control"
            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                           ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                Cursor = Cursors.Hand
                Size = New Size(100, 22)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                Dim G = e.Graphics
                Dim Base As New Rectangle(1, 1, Height - 2, Height - 2)
                Dim Circle As New Rectangle(6, 6, Height - 12, Height - 12)
                With G
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(_BackColour)
                    .FillEllipse(New SolidBrush(_BackColour), Base)
                    .DrawEllipse(New Pen(_BorderColour, 2), Base)
                    If Checked Then
                        Select Case State
                            Case MouseState.Over
                                .FillEllipse(New SolidBrush(_HoverColour), New Rectangle(2, 2, Height - 4, Height - 4))
                        End Select
                        .FillEllipse(New SolidBrush(_CheckedColour), Circle)
                    Else
                        Select Case State
                            Case MouseState.Over
                                .FillEllipse(New SolidBrush(_HoverColour), New Rectangle(2, 2, Height - 4, Height - 4))
                        End Select
                    End If
                    .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(24, 3, Width, Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})
                    .InterpolationMode = CType(7, InterpolationMode)
                End With
            End Sub
#End Region

        End Class

        Public Class LogInLabel
            Inherits Label

#Region "Declaration"
            Private _FontColour As Color = Color.FromArgb(255, 255, 255)
#End Region

#Region "Property & Event"

            <Category("Colours")>
            Public Property FontColour As Color
                Get
                    Return _FontColour
                End Get
                Set(value As Color)
                    _FontColour = value
                End Set
            End Property

            Protected Overrides Sub OnTextChanged(e As EventArgs)
                MyBase.OnTextChanged(e) : Invalidate()
            End Sub

#End Region

#Region "Draw Control"

            Sub New()
                SetStyle(ControlStyles.SupportsTransparentBackColor, True)
                Font = New Font("Segoe UI", 9)
                ForeColor = _FontColour
                BackColor = Color.Transparent
                Text = Text
            End Sub

#End Region

        End Class

        Public Class LogInButton
            Inherits Control

#Region "Declarations"
            Private ReadOnly _Font As New Font("Segoe UI", 9)
            Private _ProgressColour As Color = Color.FromArgb(0, 191, 255)
            Private _BorderColour As Color = Color.FromArgb(25, 25, 25)
            Private _FontColour As Color = Color.FromArgb(255, 255, 255)
            Private _MainColour As Color = Color.FromArgb(42, 42, 42)
            Private _HoverColour As Color = Color.FromArgb(52, 52, 52)
            Private _PressedColour As Color = Color.FromArgb(47, 47, 47)
            Private State As New MouseState
#End Region

#Region "Mouse States"

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                State = MouseState.Down : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)
                State = MouseState.Over : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseEnter(e As EventArgs)
                MyBase.OnMouseEnter(e)
                State = MouseState.Over : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                State = MouseState.None : Invalidate()
            End Sub

#End Region

#Region "Properties"

            <Category("Colours")>
            Public Property ProgressColour As Color
                Get
                    Return _ProgressColour
                End Get
                Set(value As Color)
                    _ProgressColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property FontColour As Color
                Get
                    Return _FontColour
                End Get
                Set(value As Color)
                    _FontColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BaseColour As Color
                Get
                    Return _MainColour
                End Get
                Set(value As Color)
                    _MainColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property HoverColour As Color
                Get
                    Return _HoverColour
                End Get
                Set(value As Color)
                    _HoverColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property PressedColour As Color
                Get
                    Return _PressedColour
                End Get
                Set(value As Color)
                    _PressedColour = value
                End Set
            End Property

#End Region

#Region "Draw Control"
            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                      ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                      ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                Size = New Size(75, 30)
                BackColor = Color.Transparent
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                Dim G = e.Graphics
                With G
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .InterpolationMode = CType(7, InterpolationMode)
                    .Clear(BackColor)
                    Select Case State
                        Case MouseState.None
                            .FillRectangle(New SolidBrush(_MainColour), New Rectangle(0, 0, Width, Height))
                            .DrawRectangle(New Pen(_BorderColour, 2), New Rectangle(0, 0, Width, Height))
                            .DrawString(Text, _Font, Brushes.White, New Point(CInt(Width / 2), CInt(Height / 2)), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                        Case MouseState.Over
                            .FillRectangle(New SolidBrush(_HoverColour), New Rectangle(0, 0, Width, Height))
                            .DrawRectangle(New Pen(_BorderColour, 1), New Rectangle(1, 1, Width - 2, Height - 2))
                            .DrawString(Text, _Font, Brushes.White, New Point(CInt(Width / 2), CInt(Height / 2)), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                        Case MouseState.Down
                            .FillRectangle(New SolidBrush(_PressedColour), New Rectangle(0, 0, Width, Height))
                            .DrawRectangle(New Pen(_BorderColour, 1), New Rectangle(1, 1, Width - 2, Height - 2))
                            .DrawString(Text, _Font, Brushes.White, New Point(CInt(Width / 2), CInt(Height / 2)), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    End Select
                End With
            End Sub

#End Region

        End Class

        Public Class LogInButtonWithProgress
            Inherits Control

#Region "Declarations"
            Private _Value As Integer = 0
            Private _Maximum As Integer = 100
            Private _Font As New Font("Segoe UI", 9)
            Private _ProgressColour As Color = Color.FromArgb(0, 191, 255)
            Private _BorderColour As Color = Color.FromArgb(25, 25, 25)
            Private _FontColour As Color = Color.FromArgb(255, 255, 255)
            Private _MainColour As Color = Color.FromArgb(42, 42, 42)
            Private _HoverColour As Color = Color.FromArgb(52, 52, 52)
            Private _PressedColour As Color = Color.FromArgb(47, 47, 47)
            Private State As New MouseState
#End Region

#Region "Mouse States"

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                State = MouseState.Down : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)
                State = MouseState.Over : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseEnter(e As EventArgs)
                MyBase.OnMouseEnter(e)
                State = MouseState.Over : Invalidate()
            End Sub
            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                State = MouseState.None : Invalidate()
            End Sub

#End Region

#Region "Properties"

            <Category("Colours")>
            Public Property ProgressColour As Color
                Get
                    Return _ProgressColour
                End Get
                Set(value As Color)
                    _ProgressColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property FontColour As Color
                Get
                    Return _FontColour
                End Get
                Set(value As Color)
                    _FontColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BaseColour As Color
                Get
                    Return _MainColour
                End Get
                Set(value As Color)
                    _MainColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property HoverColour As Color
                Get
                    Return _HoverColour
                End Get
                Set(value As Color)
                    _HoverColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property PressedColour As Color
                Get
                    Return _PressedColour
                End Get
                Set(value As Color)
                    _PressedColour = value
                End Set
            End Property

            <Category("Control")>
            Public Property Maximum() As Integer
                Get
                    Return _Maximum
                End Get
                Set(V As Integer)
                    Select Case V
                        Case Is < _Value
                            _Value = V
                    End Select
                    _Maximum = V
                    Invalidate()
                End Set
            End Property

            <Category("Control")>
            Public Property Value() As Integer
                Get
                    Select Case _Value
                        Case 0
                            Return 0

                        Case Else
                            Return _Value

                    End Select
                End Get
                Set(V As Integer)
                    Select Case V
                        Case Is > _Maximum
                            V = _Maximum
                            Invalidate()
                    End Select
                    _Value = V
                    Invalidate()
                End Set
            End Property

            Public Sub Increment(ByVal Amount As Integer)
                Value += Amount
            End Sub

#End Region

#Region "Draw Control"
            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                      ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                      ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                Size = New Size(75, 30)
                BackColor = Color.Transparent
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                Dim g = e.Graphics
                With g
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(BackColor)
                    Select Case State
                        Case MouseState.None
                            .FillRectangle(New SolidBrush(_MainColour), New Rectangle(0, 0, Width, Height - 4))
                            .DrawRectangle(New Pen(_BorderColour, 2), New Rectangle(0, 0, Width, Height - 4))
                            .DrawString(Text, _Font, Brushes.White, New Point(CInt(Width / 2), CInt(Height / 2 - 2)), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                        Case MouseState.Over
                            .FillRectangle(New SolidBrush(_HoverColour), New Rectangle(0, 0, Width, Height - 4))
                            .DrawRectangle(New Pen(_BorderColour, 1), New Rectangle(1, 1, Width - 2, Height - 5))
                            .DrawString(Text, _Font, Brushes.White, New Point(CInt(Width / 2), CInt(Height / 2 - 2)), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                        Case MouseState.Down
                            .FillRectangle(New SolidBrush(_PressedColour), New Rectangle(0, 0, Width, Height - 4))
                            .DrawRectangle(New Pen(_BorderColour, 1), New Rectangle(1, 1, Width - 2, Height - 5))
                            .DrawString(Text, _Font, Brushes.White, New Point(CInt(Width / 2), CInt(Height / 2 - 2)), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    End Select
                    Select Case _Value
                        Case 0
                        Case _Maximum
                            .FillRectangle(New SolidBrush(_ProgressColour), New Rectangle(0, Height - 4, Width, Height - 4))
                            .DrawRectangle(New Pen(_BorderColour, 2), New Rectangle(0, 0, Width, Height))
                        Case Else
                            .FillRectangle(New SolidBrush(_ProgressColour), New Rectangle(0, Height - 4, CInt(Width / _Maximum * _Value), Height - 4))
                            .DrawRectangle(New Pen(_BorderColour, 2), New Rectangle(0, 0, Width, Height))
                    End Select
                    .InterpolationMode = CType(7, InterpolationMode)
                End With
            End Sub

#End Region

        End Class

        Public Class LogInGroupBox
            Inherits ContainerControl

#Region "Declarations"
            Private _MainColour As Color = Color.FromArgb(47, 47, 47)
            Private _HeaderColour As Color = Color.FromArgb(42, 42, 42)
            Private _TextColour As Color = Color.FromArgb(255, 255, 255)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
#End Region

#Region "Properties"

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property TextColour As Color
                Get
                    Return _TextColour
                End Get
                Set(value As Color)
                    _TextColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property HeaderColour As Color
                Get
                    Return _HeaderColour
                End Get
                Set(value As Color)
                    _HeaderColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property MainColour As Color
                Get
                    Return _MainColour
                End Get
                Set(value As Color)
                    _MainColour = value
                End Set
            End Property

#End Region

#Region "Draw Control"
            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                       ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                       ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                Size = New Size(160, 110)
                Font = New Font("Segoe UI", 10, FontStyle.Bold)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                Dim g = e.Graphics
                With g
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(Color.FromArgb(54, 54, 54))
                    .FillRectangle(New SolidBrush(_MainColour), New Rectangle(0, 28, Width, Height))
                    .FillRectangle(New SolidBrush(_HeaderColour), New Rectangle(0, 0, CInt(.MeasureString(Text, Font).Width + 7), 28))
                    .DrawString(Text, Font, New SolidBrush(_TextColour), New Point(5, 5))
                    Dim P() As Point = {New Point(0, 0), New Point(CInt(.MeasureString(Text, Font).Width + 7), 0), New Point(CInt(.MeasureString(Text, Font).Width + 7), 28), _
                                        New Point(Width - 1, 28), New Point(Width - 1, Height - 1), New Point(1, Height - 1), New Point(1, 1)}
                    .DrawLines(New Pen(_BorderColour), P)
                    .DrawLine(New Pen(_BorderColour, 2), New Point(0, 28), New Point(CInt(.MeasureString(Text, Font).Width + 7), 28))
                    .InterpolationMode = CType(7, InterpolationMode)
                End With
            End Sub
#End Region

        End Class

        Public Class LogInSeperator
            Inherits Control

#Region "Declarations"
            Private _SeperatorColour As Color = Color.FromArgb(35, 35, 35)
            Private _Alignment As Style = Style.Horizontal
            Private _Thickness As Single = 1
#End Region

#Region "Properties"

            Enum Style
                Horizontal
                Verticle
            End Enum

            <Category("Control")>
            Public Property Thickness As Single
                Get
                    Return _Thickness
                End Get
                Set(value As Single)
                    _Thickness = value
                End Set
            End Property

            <Category("Control")>
            Public Property Alignment As Style
                Get
                    Return _Alignment
                End Get
                Set(value As Style)
                    _Alignment = value
                End Set
            End Property

            <Category("Colours")>
            Public Property SeperatorColour As Color
                Get
                    Return _SeperatorColour
                End Get
                Set(value As Color)
                    _SeperatorColour = value
                End Set
            End Property

#End Region

#Region "Draw Control"
            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                         ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                         ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                BackColor = Color.Transparent
                Size = New Size(20, 20)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                Dim G = e.Graphics
                Dim Base As New Rectangle(0, 0, Width - 1, Height - 1)
                With G
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    Select Case _Alignment
                        Case Style.Horizontal
                            .DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(0, CInt(Height / 2)), New Point(Width, CInt(Height / 2)))
                        Case Style.Verticle
                            .DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(CInt(Width / 2), 0), New Point(CInt(Width / 2), Height))
                    End Select
                    .InterpolationMode = CType(7, InterpolationMode)
                End With
            End Sub
#End Region

        End Class

        Public Class LogInNumeric
            Inherits Control

#Region "Variables"

            Private State As MouseState = MouseState.None
            Private MouseXLoc, MouseYLoc As Integer
            Private _Value As Long
            Private _Minimum As Long = 0
            Private _Maximum As Long = 9999999
            Private BoolValue As Boolean
            Private _BaseColour As Color = Color.FromArgb(42, 42, 42)
            Private _ButtonColour As Color = Color.FromArgb(47, 47, 47)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
            Private _SecondBorderColour As Color = Color.FromArgb(0, 191, 255)
            Private _FontColour As Color = Color.FromArgb(255, 255, 255)

#End Region

#Region "Properties & Events"

            Public Property Value As Long
                Get
                    Return _Value
                End Get
                Set(value As Long)
                    If value <= _Maximum And value >= _Minimum Then _Value = value
                    Invalidate()
                End Set
            End Property

            Public Property Maximum As Long
                Get
                    Return _Maximum
                End Get
                Set(value As Long)
                    If value > _Minimum Then _Maximum = value
                    If _Value > _Maximum Then _Value = _Maximum
                    Invalidate()
                End Set
            End Property

            Public Property Minimum As Long
                Get
                    Return _Minimum
                End Get
                Set(value As Long)
                    If value < _Maximum Then _Minimum = value
                    If _Value < _Minimum Then _Value = Minimum
                    Invalidate()
                End Set
            End Property

            Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
                MyBase.OnMouseMove(e)
                MouseXLoc = e.Location.X
                MouseYLoc = e.Location.Y
                Invalidate()
                If e.X < Width - 47 Then Cursor = Cursors.IBeam Else Cursor = Cursors.Hand
            End Sub

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                If MouseXLoc > Width - 47 AndAlso MouseXLoc < Width - 3 Then
                    If MouseXLoc < Width - 23 Then
                        If (Value + 1) <= _Maximum Then _Value += 1
                    Else
                        If (Value - 1) >= _Minimum Then _Value -= 1
                    End If
                Else
                    BoolValue = Not BoolValue
                    Focus()
                End If
                Invalidate()
            End Sub

            Protected Overrides Sub OnKeyPress(e As KeyPressEventArgs)
                MyBase.OnKeyPress(e)
                Try
                    If BoolValue Then _Value = CLng(CStr(CStr(_Value) & e.KeyChar.ToString()))
                    If _Value > _Maximum Then _Value = _Maximum
                    Invalidate()
                Catch : End Try
            End Sub

            Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
                MyBase.OnKeyDown(e)
                If e.KeyCode = Keys.Back Then
                    Value = 0
                End If
            End Sub

            Protected Overrides Sub OnResize(e As EventArgs)
                MyBase.OnResize(e)
                Height = 24
            End Sub

            <Category("Colours")> _
            Public Property BaseColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property ButtonColour As Color
                Get
                    Return _ButtonColour
                End Get
                Set(value As Color)
                    _ButtonColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property SecondBorderColour As Color
                Get
                    Return _SecondBorderColour
                End Get
                Set(value As Color)
                    _SecondBorderColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property FontColour As Color
                Get
                    Return _FontColour
                End Get
                Set(value As Color)
                    _FontColour = value
                End Set
            End Property

#End Region

#Region "Draw Control"

            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                Font = New Font("Segoe UI", 10)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                Dim g = e.Graphics
                Dim Base As New Rectangle(0, 0, Width, Height)
                Dim CenterSF As New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center}
                With g
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .Clear(BackColor)
                    .FillRectangle(New SolidBrush(_BaseColour), Base)
                    .FillRectangle(New SolidBrush(_ButtonColour), New Rectangle(Width - 48, 0, 48, Height))
                    .DrawRectangle(New Pen(_BorderColour, 2), Base)
                    .DrawLine(New Pen(_SecondBorderColour), New Point(Width - 48, 1), New Point(Width - 48, Height - 2))
                    .DrawLine(New Pen(_BorderColour), New Point(Width - 24, 1), New Point(Width - 24, Height - 2))
                    .DrawLine(New Pen(_FontColour), New Point(Width - 36, 7), New Point(Width - 36, 17))
                    .DrawLine(New Pen(_FontColour), New Point(Width - 31, 12), New Point(Width - 41, 12))
                    .DrawLine(New Pen(_FontColour), New Point(Width - 17, 13), New Point(Width - 7, 13))
                    .DrawString(CStr(Value), Font, New SolidBrush(_FontColour), New Rectangle(5, 1, Width, Height), New StringFormat() With {.LineAlignment = StringAlignment.Center})
                    .InterpolationMode = InterpolationMode.HighQualityBicubic
                End With
            End Sub

#End Region

        End Class

        Public Class LogInColourTable
            Inherits ProfessionalColorTable

#Region "Declarations"

            Private _BackColour As Color = Color.FromArgb(42, 42, 42)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
            Private _SelectedColour As Color = Color.FromArgb(47, 47, 47)

#End Region

#Region "Properties"

            <Category("Colours")>
            Public Property SelectedColour As Color
                Get
                    Return _SelectedColour
                End Get
                Set(value As Color)
                    _SelectedColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BackColour As Color
                Get
                    Return _BackColour
                End Get
                Set(value As Color)
                    _BackColour = value
                End Set
            End Property

            Public Overrides ReadOnly Property ButtonSelectedBorder() As Color
                Get
                    Return _BackColour
                End Get
            End Property

            Public Overrides ReadOnly Property CheckBackground() As Color
                Get
                    Return _BackColour
                End Get
            End Property

            Public Overrides ReadOnly Property CheckPressedBackground() As Color
                Get
                    Return _BackColour
                End Get
            End Property

            Public Overrides ReadOnly Property CheckSelectedBackground() As Color
                Get
                    Return _BackColour
                End Get
            End Property

            Public Overrides ReadOnly Property ImageMarginGradientBegin() As Color
                Get
                    Return _BackColour
                End Get
            End Property

            Public Overrides ReadOnly Property ImageMarginGradientEnd() As Color
                Get
                    Return _BackColour
                End Get
            End Property

            Public Overrides ReadOnly Property ImageMarginGradientMiddle() As Color
                Get
                    Return _BackColour
                End Get
            End Property

            Public Overrides ReadOnly Property MenuBorder() As Color
                Get
                    Return _BorderColour
                End Get
            End Property

            Public Overrides ReadOnly Property MenuItemBorder() As Color
                Get
                    Return _BackColour
                End Get
            End Property

            Public Overrides ReadOnly Property MenuItemSelected() As Color
                Get
                    Return _SelectedColour
                End Get
            End Property

            Public Overrides ReadOnly Property SeparatorDark() As Color
                Get
                    Return _BorderColour
                End Get
            End Property

            Public Overrides ReadOnly Property ToolStripDropDownBackground() As Color
                Get
                    Return _BackColour
                End Get
            End Property

#End Region

        End Class

        Public Class LogInListBox
            Inherits Control

#Region "Variables"

            Private WithEvents ListB As New ListBox
            Private _Items As String() = {""}
            Private _BaseColour As Color = Color.FromArgb(42, 42, 42)
            Private _SelectedColour As Color = Color.FromArgb(55, 55, 55)
            Private _ListBaseColour As Color = Color.FromArgb(47, 47, 47)
            Private _TextColour As Color = Color.FromArgb(255, 255, 255)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)

#End Region

#Region "Properties"

            <Category("Control")> _
            Public Property Items As String()
                Get
                    Return _Items
                End Get
                Set(value As String())
                    _Items = value
                    ListB.Items.Clear()
                    ListB.Items.AddRange(value)
                    Invalidate()
                End Set
            End Property

            <Category("Colours")> _
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property SelectedColour As Color
                Get
                    Return _SelectedColour
                End Get
                Set(value As Color)
                    _SelectedColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property BaseColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property ListBaseColour As Color
                Get
                    Return _ListBaseColour
                End Get
                Set(value As Color)
                    _ListBaseColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property TextColour As Color
                Get
                    Return _TextColour
                End Get
                Set(value As Color)
                    _TextColour = value
                End Set
            End Property

            Public ReadOnly Property SelectedItem() As String
                Get
                    Return CStr(ListB.SelectedItem)
                End Get
            End Property

            Public ReadOnly Property SelectedIndex() As Integer
                Get
                    Return ListB.SelectedIndex
                    If ListB.SelectedIndex < 0 Then Exit Property
                End Get
            End Property

            Public Sub Clear()
                ListB.Items.Clear()
            End Sub

            Public Sub ClearSelected()
                For i As Integer = (ListB.SelectedItems.Count - 1) To 0 Step -1
                    ListB.Items.Remove(ListB.SelectedItems(i))
                Next
            End Sub

            Protected Overrides Sub OnCreateControl()
                MyBase.OnCreateControl()
                If Not Controls.Contains(ListB) Then
                    Controls.Add(ListB)
                End If
            End Sub

            Sub AddRange(ByVal items As Object())
                ListB.Items.Remove("")
                ListB.Items.AddRange(items)
            End Sub

            Sub AddItem(ByVal item As Object)
                ListB.Items.Remove("")
                ListB.Items.Add(item)
            End Sub

#End Region

#Region "Draw Control"

            Sub Drawitem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ListB.DrawItem
                If e.Index < 0 Then Exit Sub
                e.DrawBackground()
                e.DrawFocusRectangle()
                With e.Graphics
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .InterpolationMode = InterpolationMode.HighQualityBicubic
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    If InStr(e.State.ToString, "Selected,") > 0 Then
                        .FillRectangle(New SolidBrush(_SelectedColour), New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 1))
                        .DrawString(" " & ListB.Items(e.Index).ToString(), New Font("Segoe UI", 9, FontStyle.Bold), New SolidBrush(_TextColour), e.Bounds.X, e.Bounds.Y + 2)
                    Else
                        .FillRectangle(New SolidBrush(_ListBaseColour), New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height))
                        .DrawString(" " & ListB.Items(e.Index).ToString(), New Font("Segoe UI", 8), New SolidBrush(_TextColour), e.Bounds.X, e.Bounds.Y + 2)
                    End If
                    .Dispose()
                End With
            End Sub

            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                    ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
                DoubleBuffered = True
                ListB.DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
                ListB.ScrollAlwaysVisible = False
                ListB.HorizontalScrollbar = False
                ListB.BorderStyle = BorderStyle.None
                ListB.BackColor = _BaseColour
                ListB.Location = New Point(3, 3)
                ListB.Font = New Font("Segoe UI", 8)
                ListB.ItemHeight = 20
                ListB.Items.Clear()
                ListB.IntegralHeight = False
                Size = New Size(130, 100)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                Dim G = e.Graphics
                Dim Base As New Rectangle(0, 0, Width, Height)
                With G
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .Clear(BackColor)
                    ListB.Size = New Size(Width - 6, Height - 5)
                    .FillRectangle(New SolidBrush(_BaseColour), Base)
                    .DrawRectangle(New Pen(_BorderColour, 3), New Rectangle(0, 0, Width, Height - 1))
                    .InterpolationMode = InterpolationMode.HighQualityBicubic
                End With
            End Sub

#End Region

        End Class

        Public Class LogInTitledListBox
            Inherits Control

#Region "Variables"

            Private WithEvents ListB As New ListBox
            Private _Items As String() = {""}
            Private _BaseColour As Color = Color.FromArgb(42, 42, 42)
            Private _SelectedColour As Color = Color.FromArgb(55, 55, 55)
            Private _ListBaseColour As Color = Color.FromArgb(47, 47, 47)
            Private _TextColour As Color = Color.FromArgb(255, 255, 255)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
            Private _TitleFont As New Font("Segeo UI", 10, FontStyle.Bold)

#End Region

#Region "Properties"

            <Category("Control")>
            Public Property TitleFont As Font
                Get
                    Return _TitleFont
                End Get
                Set(value As Font)
                    _TitleFont = value
                End Set
            End Property

            <Category("Control")> _
            Public Property Items As String()
                Get
                    Return _Items
                End Get
                Set(value As String())
                    _Items = value
                    ListB.Items.Clear()
                    ListB.Items.AddRange(value)
                    Invalidate()
                End Set
            End Property

            <Category("Colours")> _
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property SelectedColour As Color
                Get
                    Return _SelectedColour
                End Get
                Set(value As Color)
                    _SelectedColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property BaseColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property ListBaseColour As Color
                Get
                    Return _ListBaseColour
                End Get
                Set(value As Color)
                    _ListBaseColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property TextColour As Color
                Get
                    Return _TextColour
                End Get
                Set(value As Color)
                    _TextColour = value
                End Set
            End Property

            Public ReadOnly Property SelectedItem() As String
                Get
                    Return CStr(ListB.SelectedItem)
                End Get
            End Property

            Public ReadOnly Property SelectedIndex() As Integer
                Get
                    Return ListB.SelectedIndex
                    If ListB.SelectedIndex < 0 Then Exit Property
                End Get
            End Property

            Public Sub Clear()
                ListB.Items.Clear()
            End Sub

            Public Sub ClearSelected()
                For i As Integer = (ListB.SelectedItems.Count - 1) To 0 Step -1
                    ListB.Items.Remove(ListB.SelectedItems(i))
                Next
            End Sub

            Protected Overrides Sub OnCreateControl()
                MyBase.OnCreateControl()
                If Not Controls.Contains(ListB) Then
                    Controls.Add(ListB)
                End If
            End Sub

            Sub AddRange(ByVal items As Object())
                ListB.Items.Remove("")
                ListB.Items.AddRange(items)
            End Sub

            Sub AddItem(ByVal item As Object)
                ListB.Items.Remove("")
                ListB.Items.Add(item)
            End Sub

#End Region

#Region "Draw Control"

            Sub Drawitem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ListB.DrawItem
                If e.Index < 0 Then Exit Sub
                e.DrawBackground()
                e.DrawFocusRectangle()
                With e.Graphics
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .InterpolationMode = InterpolationMode.HighQualityBicubic
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    If InStr(e.State.ToString, "Selected,") > 0 Then
                        .FillRectangle(New SolidBrush(_SelectedColour), New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 1))
                        .DrawString(" " & ListB.Items(e.Index).ToString(), New Font("Segoe UI", 9, FontStyle.Bold), New SolidBrush(_TextColour), e.Bounds.X, e.Bounds.Y + 2)
                    Else
                        .FillRectangle(New SolidBrush(_ListBaseColour), New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height))
                        .DrawString(" " & ListB.Items(e.Index).ToString(), New Font("Segoe UI", 8), New SolidBrush(_TextColour), e.Bounds.X, e.Bounds.Y + 2)
                    End If
                    .Dispose()
                End With
            End Sub

            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                    ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
                DoubleBuffered = True
                ListB.DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
                ListB.ScrollAlwaysVisible = False
                ListB.HorizontalScrollbar = False
                ListB.BorderStyle = BorderStyle.None
                ListB.BackColor = BaseColour
                ListB.Location = New Point(3, 28)
                ListB.Font = New Font("Segoe UI", 8)
                ListB.ItemHeight = 20
                ListB.Items.Clear()
                ListB.IntegralHeight = False
                Size = New Size(130, 100)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)

                Dim G = e.Graphics
                Dim Base As New Rectangle(0, 0, Width, Height)
                With G
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .Clear(BackColor)
                    ListB.Size = New Size(Width - 6, Height - 30)
                    .FillRectangle(New SolidBrush(BaseColour), Base)
                    .DrawRectangle(New Pen((_BorderColour), 3), New Rectangle(0, 0, Width, Height - 1))
                    .DrawLine(New Pen((_BorderColour), 2), New Point(0, 27), New Point(Width - 1, 27))
                    .DrawString(Text, _TitleFont, New SolidBrush(_TextColour), New Rectangle(2, 5, Width - 5, 20), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    .InterpolationMode = InterpolationMode.HighQualityBicubic
                End With
            End Sub

#End Region

        End Class

        Public Class LogInContextMenu
            Inherits ContextMenuStrip

#Region "Declarations"

            Private _FontColour As Color = Color.FromArgb(55, 255, 255)

#End Region

#Region "Properties"

            Public Property FontColour As Color
                Get
                    Return _FontColour
                End Get
                Set(value As Color)
                    _FontColour = value
                End Set
            End Property

#End Region

#Region "Draw Control"

            Sub New()
                Renderer = New ToolStripProfessionalRenderer(New LogInColourTable())
                ShowCheckMargin = False
                ShowImageMargin = False
                ForeColor = Color.FromArgb(255, 255, 255)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                MyBase.OnPaint(e)
            End Sub

#End Region

        End Class

        Public Class LogInProgressBar
            Inherits Control

#Region "Declarations"
            Private _ProgressColour As Color = Color.FromArgb(0, 160, 199)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
            Private _BaseColour As Color = Color.FromArgb(42, 42, 42)
            Private _FontColour As Color = Color.FromArgb(50, 50, 50)
            Private _SecondColour As Color = Color.FromArgb(0, 145, 184)
            Private _Value As Integer = 0
            Private _Maximum As Integer = 100
            Private _TwoColour As Boolean = True
#End Region

#Region "Properties"

            Public Property SecondColour As Color
                Get
                    Return _SecondColour
                End Get
                Set(value As Color)
                    _SecondColour = value
                End Set
            End Property

            <Category("Control")>
            Public Property TwoColour As Boolean
                Get
                    Return _TwoColour
                End Get
                Set(value As Boolean)
                    _TwoColour = value
                End Set
            End Property

            <Category("Control")>
            Public Property Maximum() As Integer
                Get
                    Return _Maximum
                End Get
                Set(V As Integer)
                    Select Case V
                        Case Is < _Value
                            _Value = V
                    End Select
                    _Maximum = V
                    Invalidate()
                End Set
            End Property

            <Category("Control")>
            Public Property Value() As Integer
                Get
                    Select Case _Value
                        Case 0
                            Return 0
                            Invalidate()
                        Case Else
                            Return _Value
                            Invalidate()
                    End Select
                End Get
                Set(V As Integer)
                    Select Case V
                        Case Is > _Maximum
                            V = _Maximum
                            Invalidate()
                    End Select
                    _Value = V
                    Invalidate()
                End Set
            End Property

            <Category("Colours")>
            Public Property ProgressColour As Color
                Get
                    Return _ProgressColour
                End Get
                Set(value As Color)
                    _ProgressColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BaseColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property FontColour As Color
                Get
                    Return _FontColour
                End Get
                Set(value As Color)
                    _FontColour = value
                End Set
            End Property

#End Region

#Region "Events"

            Protected Overrides Sub OnResize(e As EventArgs)
                MyBase.OnResize(e)
                Height = 25
            End Sub

            Protected Overrides Sub CreateHandle()
                MyBase.CreateHandle()
                Height = 25
            End Sub

            Public Sub Increment(ByVal Amount As Integer)
                Value += Amount
            End Sub

#End Region

#Region "Draw Control"
            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                         ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
                DoubleBuffered = True
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                Dim G = e.Graphics
                Dim Base As New Rectangle(0, 0, Width, Height)
                With G
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(BackColor)
                    Dim ProgVal As Integer = CInt(_Value / _Maximum * Width)
                    Select Case Value
                        Case 0
                            .FillRectangle(New SolidBrush(_BaseColour), Base)
                            .FillRectangle(New SolidBrush(_ProgressColour), New Rectangle(0, 0, ProgVal - 1, Height))
                            .DrawRectangle(New Pen(_BorderColour, 3), Base)
                        Case _Maximum
                            .FillRectangle(New SolidBrush(_BaseColour), Base)
                            .FillRectangle(New SolidBrush(_ProgressColour), New Rectangle(0, 0, ProgVal - 1, Height))
                            If _TwoColour Then
                                G.SetClip(New Rectangle(0, -10, CInt(Width * _Value / _Maximum - 1), Height - 5))
                                For i = 0 To (Width - 1) * _Maximum / _Value Step 25
                                    G.DrawLine(New Pen(New SolidBrush(_SecondColour), 7), New Point(CInt(i), 0), New Point(CInt(i - 15), Height))
                                Next
                                G.ResetClip()
                            Else
                            End If
                            .DrawRectangle(New Pen(_BorderColour, 3), Base)
                        Case Else
                            .FillRectangle(New SolidBrush(_BaseColour), Base)
                            .FillRectangle(New SolidBrush(_ProgressColour), New Rectangle(0, 0, ProgVal - 1, Height))
                            If _TwoColour Then
                                .SetClip(New Rectangle(0, 0, CInt(Width * _Value / _Maximum - 1), Height - 1))
                                For i = 0 To (Width - 1) * _Maximum / _Value Step 25
                                    .DrawLine(New Pen(New SolidBrush(_SecondColour), 7), New Point(CInt(i), 0), New Point(CInt(i - 10), Height))
                                Next
                                .ResetClip()
                            Else
                            End If
                            .DrawRectangle(New Pen(_BorderColour, 3), Base)
                    End Select
                    .InterpolationMode = CType(7, InterpolationMode)
                End With
            End Sub

#End Region

        End Class

        Public Class LogInRichTextBox
            Inherits Control

#Region "Declarations"
            Private WithEvents TB As New RichTextBox
            Private _BaseColour As Color = Color.FromArgb(42, 42, 42)
            Private _TextColour As Color = Color.FromArgb(255, 255, 255)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
#End Region

#Region "Properties"

            <Category("Colours")>
            Public Property BaseColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property TextColour As Color
                Get
                    Return _TextColour
                End Get
                Set(value As Color)
                    _TextColour = value
                End Set
            End Property

#End Region

#Region "Events"

            Public Sub AppendText(ByVal AppendingText As String)
                TB.Focus()
                TB.AppendText(AppendingText)
                Invalidate()
            End Sub

            Overrides Property Text As String
                Get
                    Return TB.Text
                End Get
                Set(value As String)
                    TB.Text = value
                    Invalidate()
                End Set
            End Property

            Protected Overrides Sub OnBackColorChanged(ByVal e As System.EventArgs)
                MyBase.OnBackColorChanged(e)
                TB.BackColor = BackColor
                Invalidate()
            End Sub

            Protected Overrides Sub OnForeColorChanged(ByVal e As System.EventArgs)
                MyBase.OnForeColorChanged(e)
                TB.ForeColor = ForeColor
                Invalidate()
            End Sub

            Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
                MyBase.OnSizeChanged(e)
                TB.Size = New Size(Width - 10, Height - 11)
            End Sub

            Protected Overrides Sub OnFontChanged(ByVal e As System.EventArgs)
                MyBase.OnFontChanged(e)
                TB.Font = Font
            End Sub

            Sub TextChanges() Handles MyBase.TextChanged
                TB.Text = Text
            End Sub

#End Region

#Region "Draw Control"

            Sub New()
                With TB
                    .Multiline = True
                    .BackColor = _BaseColour
                    .ForeColor = _TextColour
                    .Text = String.Empty
                    .BorderStyle = BorderStyle.None
                    .Location = New Point(5, 5)
                    .Font = New Font("Segeo UI", 9)
                    .Size = New Size(Width - 10, Height - 10)
                End With
                Controls.Add(TB)
                Size = New Size(135, 35)
                DoubleBuffered = True
            End Sub

            Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
                Dim g = e.Graphics
                Dim Base As New Rectangle(0, 0, Width - 1, Height - 1)
                With g
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(_BaseColour)
                    .DrawRectangle(New Pen(_BorderColour, 2), ClientRectangle)
                    .InterpolationMode = CType(7, InterpolationMode)
                End With
            End Sub

#End Region

        End Class

        Public Class LogInStatusBar
            Inherits Control

#Region "Variables"
            Private _BaseColour As Color = Color.FromArgb(42, 42, 42)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
            Private _TextColour As Color = Color.White
            Private _RectColour As Color = Color.FromArgb(21, 117, 149)
            Private _ShowLine As Boolean = True
            Private _LinesToShow As LinesCount = LinesCount.One
            Private _Alignment As Alignments = Alignments.Left
            Private _ShowBorder As Boolean = True
#End Region

#Region "Properties"

            <Category("Colours")>
            Public Property BaseColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property TextColour As Color
                Get
                    Return _TextColour
                End Get
                Set(value As Color)
                    _TextColour = value
                End Set
            End Property

            Enum LinesCount As Integer
                One = 1
                Two = 2
            End Enum

            Enum Alignments
                Left
                Center
                Right
            End Enum

            <Category("Control")>
            Public Property Alignment As Alignments
                Get
                    Return _Alignment
                End Get
                Set(value As Alignments)
                    _Alignment = value
                End Set
            End Property

            <Category("Control")>
            Public Property LinesToShow As LinesCount
                Get
                    Return _LinesToShow
                End Get
                Set(value As LinesCount)
                    _LinesToShow = value
                End Set
            End Property

            Public Property ShowBorder As Boolean
                Get
                    Return _ShowBorder
                End Get
                Set(value As Boolean)
                    _ShowBorder = value
                End Set
            End Property

            Protected Overrides Sub CreateHandle()
                MyBase.CreateHandle()
                Dock = DockStyle.Bottom
            End Sub

            Protected Overrides Sub OnTextChanged(e As EventArgs)
                MyBase.OnTextChanged(e) : Invalidate()
            End Sub

            <Category("Colours")> _
            Public Property RectangleColor As Color
                Get
                    Return _RectColour
                End Get
                Set(value As Color)
                    _RectColour = value
                End Set
            End Property

            Public Property ShowLine As Boolean
                Get
                    Return _ShowLine
                End Get
                Set(value As Boolean)
                    _ShowLine = value
                End Set
            End Property

#End Region

#Region "Draw Control"

            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                         ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
                DoubleBuffered = True
                Font = New Font("Segoe UI", 9)
                ForeColor = Color.White
                Size = New Size(Width, 20)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                Dim G = e.Graphics
                Dim Base As New Rectangle(0, 0, Width, Height)
                With G
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .Clear(BaseColour)
                    .FillRectangle(New SolidBrush(BaseColour), Base)
                    If _ShowLine = True Then
                        Select Case _LinesToShow
                            Case LinesCount.One
                                If _Alignment = Alignments.Left Then
                                    .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(22, 2, Width, Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})
                                ElseIf _Alignment = Alignments.Center Then
                                    .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(0, 0, Width, Height), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                                Else
                                    .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(0, 0, Width - 5, Height), New StringFormat With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center})
                                End If
                                .FillRectangle(New SolidBrush(_RectColour), New Rectangle(5, 9, 14, 3))
                            Case LinesCount.Two
                                If _Alignment = Alignments.Left Then
                                    .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(22, 2, Width, Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})
                                ElseIf _Alignment = Alignments.Center Then
                                    .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(0, 0, Width, Height), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                                Else
                                    .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(0, 0, Width - 22, Height), New StringFormat With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center})
                                End If
                                .FillRectangle(New SolidBrush(_RectColour), New Rectangle(5, 9, 14, 3))
                                .FillRectangle(New SolidBrush(_RectColour), New Rectangle(Width - 20, 9, 14, 3))
                        End Select
                    Else
                        .DrawString(Text, Font, Brushes.White, New Rectangle(5, 2, Width, Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})
                    End If
                    If _ShowBorder Then
                        .DrawLine(New Pen(_BorderColour, 2), New Point(0, 0), New Point(Width, 0))
                    Else
                    End If
                    .InterpolationMode = InterpolationMode.HighQualityBicubic
                End With
            End Sub

#End Region

        End Class

        <DefaultEvent("ToggleChanged")>
        Public Class LogInOnOffSwitch
            Inherits Control

#Region "Declarations"

            Event ToggleChanged(ByVal sender As Object)
            Private _Toggled As Toggles = Toggles.NotToggled
            Private MouseXLoc As Integer
            Private ToggleLocation As Integer = 0
            Private _BaseColour As Color = Color.FromArgb(42, 42, 42)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
            Private _TextColour As Color = Color.FromArgb(255, 255, 255)
            Private _NonToggledTextColour As Color = Color.FromArgb(125, 125, 125)
            Private _ToggledColour As Color = Color.FromArgb(23, 119, 151)

#End Region

#Region "Properties & Events"

            <Category("Colours")>
            Public Property BaseColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                    Invalidate()
                End Set
            End Property

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                    Invalidate()
                End Set
            End Property

            <Category("Colours")>
            Public Property TextColour As Color
                Get
                    Return _TextColour
                End Get
                Set(value As Color)
                    _TextColour = value
                    Invalidate()
                End Set
            End Property

            <Category("Colours")>
            Public Property NonToggledTextColourderColour As Color
                Get
                    Return _NonToggledTextColour
                End Get
                Set(value As Color)
                    _NonToggledTextColour = value
                    Invalidate()
                End Set
            End Property

            <Category("Colours")>
            Public Property ToggledColour As Color
                Get
                    Return _ToggledColour
                End Get
                Set(value As Color)
                    _ToggledColour = value
                    Invalidate()
                End Set
            End Property

            Enum Toggles
                Toggled
                NotToggled
            End Enum

            Event ToggledChanged()

            Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
                MyBase.OnMouseMove(e)
                MouseXLoc = e.Location.X
                Invalidate()
                If e.X < Width - 40 AndAlso e.X > 40 Then Cursor = Cursors.IBeam Else Cursor = Cursors.Arrow
            End Sub

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                If MouseXLoc > Width - 39 Then
                    _Toggled = Toggles.Toggled
                    ToggledValue()
                ElseIf MouseXLoc < 39 Then
                    _Toggled = Toggles.NotToggled
                    ToggledValue()
                End If
                Invalidate()
            End Sub

            Public Property Toggled() As Toggles
                Get
                    Return _Toggled
                End Get
                Set(ByVal value As Toggles)
                    _Toggled = value
                    Invalidate()
                End Set
            End Property

            Private Sub ToggledValue()
                If CBool(_Toggled) Then
                    If ToggleLocation < 100 Then
                        ToggleLocation += 10
                    End If
                Else
                    If ToggleLocation > 0 Then
                        ToggleLocation -= 10
                    End If
                End If
                Invalidate()
            End Sub

#End Region

#Region "Draw Control"

            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                        ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
                BackColor = Color.FromArgb(54, 54, 54)
            End Sub

            Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)

                Dim G As Graphics = e.Graphics
                With G
                    .Clear(BackColor)
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .InterpolationMode = InterpolationMode.HighQualityBicubic
                    .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(0, 0, 39, Height))
                    .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(Width - 40, 0, Width, Height))
                    .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(38, 9, Width - 40, 5))
                    Dim P As Point() = {New Point(0, 0), New Point(39, 0), New Point(39, 9), New Point(Width - 40, 9), New Point(Width - 40, 0), _
                                       New Point(Width - 2, 0), New Point(Width - 2, Height - 1), New Point(Width - 40, Height - 1), _
                                        New Point(Width - 40, 14), New Point(39, 14), New Point(39, Height - 1), New Point(0, Height - 1), New Point()}
                    .DrawLines(New Pen(_BorderColour, 2), P)
                    If _Toggled = Toggles.Toggled Then
                        .FillRectangle(New SolidBrush(_ToggledColour), New Rectangle(CInt(Width / 2), 10, CInt(Width / 2 - 38), 3))
                        .FillRectangle(New SolidBrush(_ToggledColour), New Rectangle(Width - 39, 2, 36, Height - 5))
                        .DrawString("ON", New Font("Microsoft Sans Serif", 7, FontStyle.Bold), New SolidBrush(_TextColour), New Rectangle(2, -1, CInt(Width - 20 + 20 / 3), Height), New StringFormat() With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center})
                        .DrawString("OFF", New Font("Microsoft Sans Serif", 7, FontStyle.Bold), New SolidBrush(_NonToggledTextColour), New Rectangle(CInt(20 - 20 / 3 - 6), -1, CInt(Width - 20 + 20 / 3), Height), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
                    ElseIf _Toggled = Toggles.NotToggled Then
                        .DrawString("OFF", New Font("Microsoft Sans Serif", 7, FontStyle.Bold), New SolidBrush(_TextColour), New Rectangle(CInt(20 - 20 / 3 - 6), -1, CInt(Width - 20 + 20 / 3), Height), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
                        .DrawString("ON", New Font("Microsoft Sans Serif", 7, FontStyle.Bold), New SolidBrush(_NonToggledTextColour), New Rectangle(2, -1, CInt(Width - 20 + 20 / 3), Height), New StringFormat() With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center})
                    End If
                    .DrawLine(New Pen(_BorderColour, 2), New Point(CInt(Width / 2), 0), New Point(CInt(Width / 2), Height))

                End With
            End Sub

#End Region

        End Class

        Public Class LogInComboBox
            Inherits ComboBox

#Region "Declarations"
            Private _StartIndex As Integer = 0
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
            Private _BaseColour As Color = Color.FromArgb(42, 42, 42)
            Private _FontColour As Color = Color.FromArgb(255, 255, 255)
            Private _LineColour As Color = Color.FromArgb(23, 119, 151)
            Private _SqaureColour As Color = Color.FromArgb(47, 47, 47)
            Private _ArrowColour As Color = Color.FromArgb(30, 30, 30)
            Private _SqaureHoverColour As Color = Color.FromArgb(52, 52, 52)
            Private State As MouseState = MouseState.None
#End Region

#Region "Properties & Events"

            <Category("Colours")>
            Public Property LineColour As Color
                Get
                    Return _LineColour
                End Get
                Set(value As Color)
                    _LineColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property SqaureColour As Color
                Get
                    Return _SqaureColour
                End Get
                Set(value As Color)
                    _SqaureColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property ArrowColour As Color
                Get
                    Return _ArrowColour
                End Get
                Set(value As Color)
                    _ArrowColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property SqaureHoverColour As Color
                Get
                    Return _SqaureHoverColour
                End Get
                Set(value As Color)
                    _SqaureHoverColour = value
                End Set
            End Property

            Protected Overrides Sub OnMouseEnter(e As EventArgs)
                MyBase.OnMouseEnter(e)
                State = MouseState.Over : Invalidate()
            End Sub

            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                State = MouseState.None : Invalidate()
            End Sub

            <Category("Colours")>
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property BaseColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")>
            Public Property FontColour As Color
                Get
                    Return _FontColour
                End Get
                Set(value As Color)
                    _FontColour = value
                End Set
            End Property

            Public Property StartIndex As Integer
                Get
                    Return _StartIndex
                End Get
                Set(ByVal value As Integer)
                    _StartIndex = value
                    Try
                        MyBase.SelectedIndex = value
                    Catch
                    End Try
                    Invalidate()
                End Set
            End Property

            Protected Overrides Sub OnTextChanged(e As System.EventArgs)
                MyBase.OnTextChanged(e)
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
                Invalidate()
                OnMouseClick(e)
            End Sub

            Protected Overrides Sub OnMouseUp(e As System.Windows.Forms.MouseEventArgs)
                Invalidate()
                MyBase.OnMouseUp(e)
            End Sub

#End Region

#Region "Draw Control"

            Sub ReplaceItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
                e.DrawBackground()
                e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                Dim Rect As New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 1, e.Bounds.Height + 1)
                Try
                    With e.Graphics
                        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                            .FillRectangle(New SolidBrush(_SqaureColour), Rect)
                            .DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), Font, New SolidBrush(_FontColour), 1, e.Bounds.Top + 2)
                        Else
                            .FillRectangle(New SolidBrush(_BaseColour), Rect)
                            .DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), Font, New SolidBrush(_FontColour), 1, e.Bounds.Top + 2)
                        End If
                    End With
                Catch
                End Try
                e.DrawFocusRectangle()
                Invalidate()

            End Sub

            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                       ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                       ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                BackColor = Color.Transparent
                DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
                DropDownStyle = ComboBoxStyle.DropDownList
                Width = 163
                Font = New Font("Segoe UI", 10)
            End Sub

            Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
                Dim g = e.Graphics
                With g
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(BackColor)
                    Try
                        Dim Square As New Rectangle(Width - 25, 0, Width, Height)
                        .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(0, 0, Width - 25, Height))
                        Select Case State
                            Case MouseState.None
                                .FillRectangle(New SolidBrush(_SqaureColour), Square)
                            Case MouseState.Over
                                .FillRectangle(New SolidBrush(_SqaureHoverColour), Square)
                        End Select
                        .DrawLine(New Pen(_LineColour, 2), New Point(Width - 26, 1), New Point(Width - 26, Height - 1))
                        Try
                            .DrawString(Text, Font, New SolidBrush(_FontColour), New Rectangle(3, 0, Width - 20, Height), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
                        Catch : End Try
                        .DrawRectangle(New Pen(_BorderColour, 2), New Rectangle(0, 0, Width, Height))
                        Dim P() As Point = {New Point(Width - 17, 11), New Point(Width - 13, 5), New Point(Width - 9, 11)}
                        .FillPolygon(New SolidBrush(_BorderColour), P)
                        .DrawPolygon(New Pen(_ArrowColour), P)
                        Dim P1() As Point = {New Point(Width - 17, 15), New Point(Width - 13, 21), New Point(Width - 9, 15)}
                        .FillPolygon(New SolidBrush(_BorderColour), P1)
                        .DrawPolygon(New Pen(_ArrowColour), P1)
                    Catch
                    End Try
                    .InterpolationMode = CType(7, InterpolationMode)
                End With

            End Sub

#End Region

        End Class

        Public Class LogInTabControl
            Inherits TabControl

#Region "Declarations"

            Private _TextColour As Color = Color.FromArgb(255, 255, 255)
            Private _BackTabColour As Color = Color.FromArgb(54, 54, 54)
            Private _BaseColour As Color = Color.FromArgb(35, 35, 35)
            Private _ActiveColour As Color = Color.FromArgb(47, 47, 47)
            Private _BorderColour As Color = Color.FromArgb(30, 30, 30)
            Private _UpLineColour As Color = Color.FromArgb(0, 160, 199)
            Private _HorizLineColour As Color = Color.FromArgb(23, 119, 151)
            Private CenterSF As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}

#End Region

#Region "Properties"

            <Category("Colours")> _
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property UpLineColour As Color
                Get
                    Return _UpLineColour
                End Get
                Set(value As Color)
                    _UpLineColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property HorizontalLineColour As Color
                Get
                    Return _HorizLineColour
                End Get
                Set(value As Color)
                    _HorizLineColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property TextColour As Color
                Get
                    Return _TextColour
                End Get
                Set(value As Color)
                    _TextColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property BackTabColour As Color
                Get
                    Return _BackTabColour
                End Get
                Set(value As Color)
                    _BackTabColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property BaseColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property ActiveColour As Color
                Get
                    Return _ActiveColour
                End Get
                Set(value As Color)
                    _ActiveColour = value
                End Set
            End Property

            Protected Overrides Sub CreateHandle()
                MyBase.CreateHandle()
                Alignment = TabAlignment.Top
            End Sub

#End Region

#Region "Draw Control"

            Sub New()
                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                         ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
                DoubleBuffered = True
                Font = New Font("Segoe UI", 10)
                SizeMode = TabSizeMode.Normal
                ItemSize = New Size(240, 32)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                Dim g = e.Graphics
                With g
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .Clear(_BaseColour)
                    Try : SelectedTab.BackColor = _BackTabColour : Catch : End Try
                    Try : SelectedTab.BorderStyle = BorderStyle.FixedSingle : Catch : End Try
                    .DrawRectangle(New Pen(_BorderColour, 2), New Rectangle(0, 0, Width, Height))
                    For i = 0 To TabCount - 1
                        Dim Base As New Rectangle(New Point(GetTabRect(i).Location.X, GetTabRect(i).Location.Y), New Size(GetTabRect(i).Width, GetTabRect(i).Height))
                        Dim BaseSize As New Rectangle(Base.Location, New Size(Base.Width, Base.Height))
                        If i = SelectedIndex Then
                            .FillRectangle(New SolidBrush(_BaseColour), BaseSize)
                            .FillRectangle(New SolidBrush(_ActiveColour), New Rectangle(Base.X + 1, Base.Y - 3, Base.Width, Base.Height + 5))
                            .DrawString(TabPages(i).Text, Font, New SolidBrush(_TextColour), New Rectangle(Base.X + 7, Base.Y, Base.Width - 3, Base.Height), CenterSF)
                            .DrawLine(New Pen(_HorizLineColour, 2), New Point(Base.X + 3, CInt(Base.Height / 2 + 2)), New Point(Base.X + 9, CInt(Base.Height / 2 + 2)))
                            .DrawLine(New Pen(_UpLineColour, 2), New Point(Base.X + 3, Base.Y - 3), New Point(Base.X + 3, Base.Height + 5))
                        Else
                            .DrawString(TabPages(i).Text, Font, New SolidBrush(_TextColour), BaseSize, CenterSF)
                        End If
                    Next
                    .InterpolationMode = InterpolationMode.HighQualityBicubic
                End With
            End Sub

#End Region

        End Class

        Public Class LogInTrackBar
            Inherits Control

#Region "Declaration"
            Private _Maximum As Integer = 10
            Private _Value As Integer = 0
            Private CaptureMovement As Boolean = False
            Private Bar As Rectangle = New Rectangle(0, 10, Width - 21, Height - 21)
            Private Track As Size = New Size(25, 14)
            Private _TextColour As Color = Color.FromArgb(255, 255, 255)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
            Private _BarBaseColour As Color = Color.FromArgb(47, 47, 47)
            Private _StripColour As Color = Color.FromArgb(42, 42, 42)
            Private _StripAmountColour As Color = Color.FromArgb(23, 119, 151)
#End Region

#Region "Properties"

            <Category("Colours")> _
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property BarBaseColour As Color
                Get
                    Return _BarBaseColour
                End Get
                Set(value As Color)
                    _BarBaseColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property StripColour As Color
                Get
                    Return _StripColour
                End Get
                Set(value As Color)
                    _StripColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property TextColour As Color
                Get
                    Return _TextColour
                End Get
                Set(value As Color)
                    _TextColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property StripAmountColour As Color
                Get
                    Return _StripAmountColour
                End Get
                Set(value As Color)
                    _StripAmountColour = value
                End Set
            End Property

            Public Property Maximum() As Integer
                Get
                    Return _Maximum
                End Get
                Set(ByVal value As Integer)
                    If value > 0 Then _Maximum = value
                    If value < _Value Then _Value = value
                    Invalidate()
                End Set
            End Property

            Event ValueChanged()

            Public Property Value() As Integer
                Get
                    Return _Value
                End Get
                Set(ByVal value As Integer)
                    Select Case value
                        Case Is = _Value
                            Exit Property
                        Case Is < 0
                            _Value = 0
                        Case Is > _Maximum
                            _Value = _Maximum
                        Case Else
                            _Value = value
                    End Select
                    Invalidate()
                    RaiseEvent ValueChanged()
                End Set
            End Property



            Protected Overrides Sub OnMouseDown(ByVal e As Windows.Forms.MouseEventArgs)
                MyBase.OnMouseDown(e)
                Dim MovementPoint As New Rectangle(New Point(e.Location.X, e.Location.Y), New Size(1, 1))
                Dim Bar As New Rectangle(10, 10, Width - 21, Height - 21)
                If New Rectangle(New Point(Bar.X + CInt(Bar.Width * (Value / Maximum)) - CInt(Track.Width / 2 - 1), 0), New Size(Track.Width, Height)).IntersectsWith(MovementPoint) Then
                    CaptureMovement = True
                End If
            End Sub

            Protected Overrides Sub OnMouseUp(ByVal e As Windows.Forms.MouseEventArgs)
                MyBase.OnMouseUp(e)
                CaptureMovement = False
            End Sub

            Protected Overrides Sub OnMouseMove(ByVal e As Windows.Forms.MouseEventArgs)
                MyBase.OnMouseMove(e)
                If CaptureMovement Then
                    Dim MovementPoint As New Point(e.X, e.Y)
                    Dim Bar As New Rectangle(10, 10, Width - 21, Height - 21)
                    Value = CInt(Maximum * ((MovementPoint.X - Bar.X) / Bar.Width))
                End If
            End Sub

            Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
                MyBase.OnMouseLeave(e) : CaptureMovement = False
            End Sub

#End Region

#Region "Draw Control"

            Sub New()
                SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                            ControlStyles.UserPaint Or ControlStyles.Selectable Or _
                            ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                BackColor = Color.FromArgb(54, 54, 54)
            End Sub

            Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
                Dim g = e.Graphics
                With g
                    .SmoothingMode = SmoothingMode.AntiAlias
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    Bar = New Rectangle(13, 11, Width - 27, Height - 21)
                    .Clear(BackColor)
                    .TextRenderingHint = TextRenderingHint.AntiAliasGridFit
                    .FillRectangle(New SolidBrush(_StripColour), New Rectangle(3, CInt((Height / 2) - 4), Width - 5, 8))
                    .DrawRectangle(New Pen(_BorderColour, 2), New Rectangle(4, CInt((Height / 2) - 4), Width - 5, 8))
                    .FillRectangle(New SolidBrush(_StripAmountColour), New Rectangle(4, CInt((Height / 2) - 4), CInt(Bar.Width * (Value / Maximum)) + CInt(Track.Width / 2), 8))
                    .FillRectangle(New SolidBrush(_BarBaseColour), Bar.X + CInt(Bar.Width * (Value / Maximum)) - CInt(Track.Width / 2), Bar.Y + CInt((Bar.Height / 2)) - CInt(Track.Height / 2), Track.Width, Track.Height)
                    .DrawRectangle(New Pen(_BorderColour, 2), Bar.X + CInt(Bar.Width * (Value / Maximum)) - CInt(Track.Width / 2), Bar.Y + CInt((Bar.Height / 2)) - CInt(Track.Height / 2), Track.Width, Track.Height)
                    .DrawString(CStr(_Value), New Font("Segoe UI", 6.5, FontStyle.Regular), New SolidBrush(_TextColour), New Rectangle(Bar.X + CInt(Bar.Width * (Value / Maximum)) - CInt(Track.Width / 2), Bar.Y + CInt((Bar.Height / 2)) - CInt(Track.Height / 2), Track.Width - 1, Track.Height), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    .InterpolationMode = InterpolationMode.HighQualityBicubic
                End With

            End Sub

#End Region

        End Class

        <DefaultEvent("Scroll")>
        Public Class LogInVerticalScrollBar
            Inherits Control

#Region "Declarations"

            Private ThumbMovement As Integer
            Private TSA As Rectangle
            Private BSA As Rectangle
            Private Shaft As Rectangle
            Private Thumb As Rectangle
            Private ShowThumb As Boolean
            Private ThumbPressed As Boolean
            Private _ThumbSize As Integer = 24
            Public _Minimum As Integer = 0
            Public _Maximum As Integer = 100
            Public _Value As Integer = 0
            Public _SmallChange As Integer = 1
            Private _ButtonSize As Integer = 16
            Public _LargeChange As Integer = 10
            Private _ThumbBorder As Color = Color.FromArgb(35, 35, 35)
            Private _LineColour As Color = Color.FromArgb(23, 119, 151)
            Private _ArrowColour As Color = Color.FromArgb(37, 37, 37)
            Private _BaseColour As Color = Color.FromArgb(47, 47, 47)
            Private _ThumbColour As Color = Color.FromArgb(55, 55, 55)
            Private _ThumbSecondBorder As Color = Color.FromArgb(65, 65, 65)
            Private _FirstBorder As Color = Color.FromArgb(55, 55, 55)
            Private _SecondBorder As Color = Color.FromArgb(35, 35, 35)

#End Region

#Region "Properties & Events"

            <Category("Colours")> _
            Public Property ThumbBorder As Color
                Get
                    Return _ThumbBorder
                End Get
                Set(value As Color)
                    _ThumbBorder = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property LineColour As Color
                Get
                    Return _LineColour
                End Get
                Set(value As Color)
                    _LineColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property ArrowColour As Color
                Get
                    Return _ArrowColour
                End Get
                Set(value As Color)
                    _ArrowColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property BaseColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property ThumbColour As Color
                Get
                    Return _ThumbColour
                End Get
                Set(value As Color)
                    _ThumbColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property ThumbSecondBorder As Color
                Get
                    Return _ThumbSecondBorder
                End Get
                Set(value As Color)
                    _ThumbSecondBorder = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property FirstBorder As Color
                Get
                    Return _FirstBorder
                End Get
                Set(value As Color)
                    _FirstBorder = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property SecondBorder As Color
                Get
                    Return _SecondBorder
                End Get
                Set(value As Color)
                    _SecondBorder = value
                End Set
            End Property

            Event Scroll(ByVal sender As Object)

            Property Minimum() As Integer
                Get
                    Return _Minimum
                End Get
                Set(ByVal value As Integer)
                    _Minimum = value
                    If value > _Value Then _Value = value
                    If value > _Maximum Then _Maximum = value
                    InvalidateLayout()
                End Set
            End Property

            Property Maximum() As Integer
                Get
                    Return _Maximum
                End Get
                Set(ByVal value As Integer)
                    If value < _Value Then _Value = value
                    If value < _Minimum Then _Minimum = value
                End Set
            End Property

            Property Value() As Integer
                Get
                    Return _Value
                End Get
                Set(ByVal value As Integer)
                    Select Case value
                        Case Is = _Value
                            Exit Property
                        Case Is < _Minimum
                            _Value = _Minimum
                        Case Is > _Maximum
                            _Value = _Maximum
                        Case Else
                            _Value = value
                    End Select
                    InvalidatePosition()
                    RaiseEvent Scroll(Me)
                End Set
            End Property

            Public Property SmallChange() As Integer
                Get
                    Return _SmallChange
                End Get
                Set(ByVal value As Integer)
                    Select Case value
                        Case Is < 1
                        Case Is >
                            CInt(_SmallChange = value)
                    End Select
                End Set
            End Property

            Public Property LargeChange() As Integer
                Get
                    Return _LargeChange
                End Get
                Set(ByVal value As Integer)
                    Select Case value
                        Case Is < 1
                        Case Else
                            _LargeChange = value
                    End Select
                End Set
            End Property

            Public Property ButtonSize As Integer
                Get
                    Return _ButtonSize
                End Get
                Set(value As Integer)
                    Select Case value
                        Case Is < 16
                            _ButtonSize = 16
                        Case Else
                            _ButtonSize = value
                    End Select
                End Set
            End Property

            Protected Overrides Sub OnSizeChanged(e As EventArgs)
                InvalidateLayout()
            End Sub

            Private Sub InvalidateLayout()
                TSA = New Rectangle(0, 1, Width, 0)
                Shaft = New Rectangle(0, TSA.Bottom - 1, Width, Height - 3)
                ShowThumb = CBool(((_Maximum - _Minimum)))
                If ShowThumb Then
                    Thumb = New Rectangle(1, 0, Width - 3, _ThumbSize)
                End If
                RaiseEvent Scroll(Me)
                InvalidatePosition()
            End Sub

            Private Sub InvalidatePosition()
                Thumb.Y = CInt(((_Value - _Minimum) / (_Maximum - _Minimum)) * (Shaft.Height - _ThumbSize) + 1)
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
                If e.Button = Windows.Forms.MouseButtons.Left AndAlso ShowThumb Then
                    If TSA.Contains(e.Location) Then
                        ThumbMovement = _Value - _SmallChange
                    ElseIf BSA.Contains(e.Location) Then
                        ThumbMovement = _Value + _SmallChange
                    Else
                        If Thumb.Contains(e.Location) Then
                            ThumbPressed = True
                            Return
                        Else
                            If e.Y < Thumb.Y Then
                                ThumbMovement = _Value - _LargeChange
                            Else
                                ThumbMovement = _Value + _LargeChange
                            End If
                        End If
                    End If
                    Value = Math.Min(Math.Max(ThumbMovement, _Minimum), _Maximum)
                    InvalidatePosition()
                End If
            End Sub

            Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
                If ThumbPressed AndAlso ShowThumb Then
                    Dim ThumbPosition As Integer = e.Y - TSA.Height - (_ThumbSize \ 2)
                    Dim ThumbBounds As Integer = Shaft.Height - _ThumbSize
                    ThumbMovement = CInt((ThumbPosition / ThumbBounds) * (_Maximum - _Minimum)) + _Minimum
                    Value = Math.Min(Math.Max(ThumbMovement, _Minimum), _Maximum)
                    InvalidatePosition()
                End If
            End Sub

            Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
                ThumbPressed = False
            End Sub

#End Region

#Region "Draw Control"

            Sub New()
                SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                                    ControlStyles.UserPaint Or ControlStyles.Selectable Or _
                                    ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                Size = New Size(24, 50)
            End Sub

            Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
                Dim g = e.Graphics
                With g
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(_BaseColour)
                    Dim P() As Point = {New Point(CInt(Width / 2), 5), New Point(CInt(Width / 4), 13), New Point(CInt(Width / 2 - 2), 13), New Point(CInt(Width / 2 - 2), Height - 13), _
                                        New Point(CInt(Width / 4), Height - 13), New Point(CInt(Width / 2), Height - 5), New Point(CInt(Width - Width / 4 - 1), Height - 13), New Point(CInt(Width / 2 + 2), Height - 13), _
                                        New Point(CInt(Width / 2 + 2), 13), New Point(CInt(Width - Width / 4 - 1), 13)}
                    .FillPolygon(New SolidBrush(_ArrowColour), P)
                    .FillRectangle(New SolidBrush(_ThumbColour), Thumb)
                    .DrawRectangle(New Pen(_ThumbBorder), Thumb)
                    .DrawRectangle(New Pen(_ThumbSecondBorder), Thumb.X + 1, Thumb.Y + 1, Thumb.Width - 2, Thumb.Height - 2)
                    .DrawLine(New Pen(_LineColour, 2), New Point(CInt(Thumb.Width / 2 + 1), Thumb.Y + 4), New Point(CInt(Thumb.Width / 2 + 1), Thumb.Bottom - 4))
                    .DrawRectangle(New Pen(_FirstBorder), 0, 0, Width - 1, Height - 1)
                    .DrawRectangle(New Pen(_SecondBorder), 1, 1, Width - 3, Height - 3)
                    .InterpolationMode = InterpolationMode.HighQualityBicubic
                End With

            End Sub

#End Region

        End Class

        <DefaultEvent("Scroll")> _
        Public Class LogInHorizontalScrollBar
            Inherits Control

#Region "Declarations"

            Private ThumbMovement As Integer
            Private LSA As Rectangle
            Private RSA As Rectangle
            Private Shaft As Rectangle
            Private Thumb As Rectangle
            Private ShowThumb As Boolean
            Private ThumbPressed As Boolean
            Private _ThumbSize As Integer = 24
            Private _Minimum As Integer = 0
            Private _Maximum As Integer = 100
            Private _Value As Integer = 0
            Private _SmallChange As Integer = 1
            Private _ButtonSize As Integer = 16
            Private _LargeChange As Integer = 10
            Private _ThumbBorder As Color = Color.FromArgb(35, 35, 35)
            Private _LineColour As Color = Color.FromArgb(23, 119, 151)
            Private _ArrowColour As Color = Color.FromArgb(37, 37, 37)
            Private _BaseColour As Color = Color.FromArgb(47, 47, 47)
            Private _ThumbColour As Color = Color.FromArgb(55, 55, 55)
            Private _ThumbSecondBorder As Color = Color.FromArgb(65, 65, 65)
            Private _FirstBorder As Color = Color.FromArgb(55, 55, 55)
            Private _SecondBorder As Color = Color.FromArgb(35, 35, 35)
            Private ThumbDown As Boolean = False

#End Region

#Region "Properties & Events"

            <Category("Colours")> _
            Public Property ThumbBorder As Color
                Get
                    Return _ThumbBorder
                End Get
                Set(value As Color)
                    _ThumbBorder = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property LineColour As Color
                Get
                    Return _LineColour
                End Get
                Set(value As Color)
                    _LineColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property ArrowColour As Color
                Get
                    Return _ArrowColour
                End Get
                Set(value As Color)
                    _ArrowColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property BaseColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property ThumbColour As Color
                Get
                    Return _ThumbColour
                End Get
                Set(value As Color)
                    _ThumbColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property ThumbSecondBorder As Color
                Get
                    Return _ThumbSecondBorder
                End Get
                Set(value As Color)
                    _ThumbSecondBorder = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property FirstBorder As Color
                Get
                    Return _FirstBorder
                End Get
                Set(value As Color)
                    _FirstBorder = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property SecondBorder As Color
                Get
                    Return _SecondBorder
                End Get
                Set(value As Color)
                    _SecondBorder = value
                End Set
            End Property

            Event Scroll(ByVal sender As Object)

            Property Minimum() As Integer
                Get
                    Return _Minimum
                End Get
                Set(ByVal value As Integer)
                    _Minimum = value
                    If value > _Value Then _Value = value
                    If value > _Maximum Then _Maximum = value
                    InvalidateLayout()
                End Set
            End Property

            Property Maximum() As Integer
                Get
                    Return _Maximum
                End Get
                Set(ByVal value As Integer)
                    If value < _Value Then _Value = value
                    If value < _Minimum Then _Minimum = value
                End Set
            End Property

            Property Value() As Integer
                Get
                    Return _Value
                End Get
                Set(ByVal value As Integer)
                    Select Case value
                        Case Is = _Value
                            Exit Property
                        Case Is < _Minimum
                            _Value = _Minimum
                        Case Is > _Maximum
                            _Value = _Maximum
                        Case Else
                            _Value = value
                    End Select
                    InvalidatePosition()
                    RaiseEvent Scroll(Me)
                End Set
            End Property

            Public Property SmallChange() As Integer
                Get
                    Return _SmallChange
                End Get
                Set(ByVal value As Integer)
                    Select Case value
                        Case Is < 1
                        Case Is >
                            CInt(_SmallChange = value)
                    End Select
                End Set
            End Property

            Public Property LargeChange() As Integer
                Get
                    Return _LargeChange
                End Get
                Set(ByVal value As Integer)
                    Select Case value
                        Case Is < 1
                        Case Else
                            _LargeChange = value
                    End Select
                End Set
            End Property

            Public Property ButtonSize As Integer
                Get
                    Return _ButtonSize
                End Get
                Set(value As Integer)
                    Select Case value
                        Case Is < 16
                            _ButtonSize = 16
                        Case Else
                            _ButtonSize = value
                    End Select
                End Set
            End Property

            Protected Overrides Sub OnSizeChanged(e As EventArgs)
                InvalidateLayout()
            End Sub

            Private Sub InvalidateLayout()
                LSA = New Rectangle(0, 1, 0, Height)
                Shaft = New Rectangle(LSA.Right + 1, 0, Width - 3, Height)
                ShowThumb = CBool(((_Maximum - _Minimum)))
                Thumb = New Rectangle(0, 1, _ThumbSize, Height - 3)
                RaiseEvent Scroll(Me)
                InvalidatePosition()
            End Sub

            Private Sub InvalidatePosition()
                Thumb.X = CInt(((_Value - _Minimum) / (_Maximum - _Minimum)) * (Shaft.Width - _ThumbSize) + 1)
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
                If e.Button = Windows.Forms.MouseButtons.Left AndAlso ShowThumb Then
                    If LSA.Contains(e.Location) Then
                        ThumbMovement = _Value - _SmallChange
                    ElseIf RSA.Contains(e.Location) Then
                        ThumbMovement = _Value + _SmallChange
                    Else
                        If Thumb.Contains(e.Location) Then
                            ThumbDown = True
                            Return
                        Else
                            If e.X < Thumb.X Then
                                ThumbMovement = _Value - _LargeChange
                            Else
                                ThumbMovement = _Value + _LargeChange
                            End If
                        End If
                    End If
                    Value = Math.Min(Math.Max(ThumbMovement, _Minimum), _Maximum)
                    InvalidatePosition()
                End If
            End Sub

            Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
                If ThumbDown AndAlso ShowThumb Then
                    Dim ThumbPosition As Integer = e.X - LSA.Width - (_ThumbSize \ 2)
                    Dim ThumbBounds As Integer = Shaft.Width - _ThumbSize

                    ThumbMovement = CInt((ThumbPosition / ThumbBounds) * (_Maximum - _Minimum)) + _Minimum

                    Value = Math.Min(Math.Max(ThumbMovement, _Minimum), _Maximum)
                    InvalidatePosition()
                End If
            End Sub

            Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
                ThumbDown = False
            End Sub

#End Region

#Region "Draw Control"

            Sub New()
                SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                                   ControlStyles.UserPaint Or ControlStyles.Selectable Or _
                                   ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                Height = 18
            End Sub

            Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
                Dim g = e.Graphics
                With g
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(Color.FromArgb(47, 47, 47))
                    Dim P() As Point = {New Point(5, CInt(Height / 2)), New Point(13, CInt(Height / 4)), New Point(13, CInt(Height / 2 - 2)), New Point(Width - 13, CInt(Height / 2 - 2)), _
                            New Point(Width - 13, CInt(Height / 4)), New Point(Width - 5, CInt(Height / 2)), New Point(Width - 13, CInt(Height - Height / 4 - 1)), New Point(Width - 13, CInt(Height / 2 + 2)), _
                                       New Point(13, CInt(Height / 2 + 2)), New Point(13, CInt(Height - Height / 4 - 1))}
                    .FillPolygon(New SolidBrush(_ArrowColour), P)
                    .FillRectangle(New SolidBrush(_ThumbColour), Thumb)
                    .DrawRectangle(New Pen(_ThumbBorder), Thumb)
                    .DrawRectangle(New Pen(_ThumbSecondBorder), Thumb.X + 1, Thumb.Y + 1, Thumb.Width - 2, Thumb.Height - 2)
                    .DrawLine(New Pen((_LineColour), 2), New Point(Thumb.X + 4, (CInt(Thumb.Height / 2 + 1))), New Point(Thumb.Right - 4, (CInt(Thumb.Height / 2 + 1))))
                    .DrawRectangle(New Pen(_FirstBorder), 0, 0, Width - 1, Height - 1)
                    .DrawRectangle(New Pen(_SecondBorder), 1, 1, Width - 3, Height - 3)
                    .InterpolationMode = InterpolationMode.HighQualityBicubic
                End With
            End Sub

#End Region

        End Class

        Public Class LogInTitledListBoxWBuiltInScrollBar
            Inherits Control

#Region "Declarations"

            Private _Items As New List(Of LogInListBoxItem)
            Private ReadOnly _SelectedItems As New List(Of LogInListBoxItem)
            Private _MultiSelect As Boolean = True
            Private ItemHeight As Integer = 24
            Private ReadOnly VerticalScrollbar As LogInVerticalScrollBar
            Private _BaseColour As Color = Color.FromArgb(55, 55, 55)
            Private _SelectedItemColour As Color = Color.FromArgb(50, 50, 50)
            Private _NonSelectedItemColour As Color = Color.FromArgb(47, 47, 47)
            Private _TitleAreaColour As Color = Color.FromArgb(42, 42, 42)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
            Private _TextColour As Color = Color.FromArgb(255, 255, 255)
            Private _SelectedHeight As Integer = 1

#End Region

#Region "Properties"

            <Category("Colours")> _
            Public Property TitleAreaColour As Color
                Get
                    Return _TitleAreaColour
                End Get
                Set(value As Color)
                    _TitleAreaColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property TextColour As Color
                Get
                    Return _TextColour
                End Get
                Set(value As Color)
                    _TextColour = value
                End Set
            End Property

            <Category("Control")> _
            Public Property SelectedHeight As Integer
                Get
                    Return _SelectedHeight
                End Get
                Set(value As Integer)
                    If value < 1 Then
                        _SelectedHeight = Height
                    Else
                        _SelectedHeight = value
                    End If
                    InvalidateScroll()
                End Set
            End Property

            <Category("Colours")> _
            Public Property BaseColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property SelectedItemColour As Color
                Get
                    Return _SelectedItemColour
                End Get
                Set(value As Color)
                    _SelectedItemColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property NonSelectedItemColour As Color
                Get
                    Return _NonSelectedItemColour
                End Get
                Set(value As Color)
                    _NonSelectedItemColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property


            Private Sub HandleScroll(ByVal sender As Object)
                Invalidate()
            End Sub

            Private Sub InvalidateScroll()
                Debug.Print(CStr(Height))
                If CInt(Math.Round(((_Items.Count) * ItemHeight) / _SelectedHeight)) < CDbl((((_Items.Count) * ItemHeight) / _SelectedHeight)) Then
                    VerticalScrollbar._Maximum = CInt(Math.Ceiling(((_Items.Count) * ItemHeight) / _SelectedHeight))
                ElseIf CInt(Math.Round(((_Items.Count) * ItemHeight) / _SelectedHeight)) = 0 Then
                    VerticalScrollbar._Maximum = 1
                Else
                    VerticalScrollbar._Maximum = CInt(Math.Round(((_Items.Count) * ItemHeight) / _SelectedHeight))
                End If
                Invalidate()
            End Sub

            Private Sub InvalidateLayout()
                VerticalScrollbar.Location = New Point(Width - VerticalScrollbar.Width - 2, 2)
                VerticalScrollbar.Size = New Size(18, Height - 4)
                Invalidate()
            End Sub

            Public Class LogInListBoxItem
                Property Text As String
                Public Overrides Function ToString() As String
                    Return Text
                End Function
            End Class

            <System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)> _
            Public Property Items() As LogInListBoxItem()
                Get
                    Return _Items.ToArray()
                End Get
                Set(ByVal value As LogInListBoxItem())
                    _Items = New List(Of LogInListBoxItem)(value)
                    Invalidate()
                    InvalidateScroll()
                End Set
            End Property

            Public ReadOnly Property SelectedItems() As LogInListBoxItem()
                Get
                    Return _SelectedItems.ToArray()
                End Get
            End Property

            Public Property MultiSelect() As Boolean
                Get
                    Return _MultiSelect
                End Get
                Set(ByVal value As Boolean)
                    _MultiSelect = value

                    If _SelectedItems.Count > 1 Then
                        _SelectedItems.RemoveRange(1, _SelectedItems.Count - 1)
                    End If

                    Invalidate()
                End Set
            End Property

            Public Overrides Property Font As Font
                Get
                    Return MyBase.Font
                End Get
                Set(ByVal value As Font)
                    ItemHeight = CInt(Graphics.FromHwnd(Handle).MeasureString("@", Font).Height)
                    If VerticalScrollbar IsNot Nothing Then
                        VerticalScrollbar._SmallChange = 1
                        VerticalScrollbar._LargeChange = 1

                    End If
                    MyBase.Font = value
                    InvalidateLayout()
                End Set
            End Property

            Public Sub AddItem(ByVal Items As String)
                Dim Item As New LogInListBoxItem()
                Item.Text = Items
                _Items.Add(Item)
                Invalidate()
                InvalidateScroll()
            End Sub

            Public Sub AddItems(ByVal Items() As String)
                For Each I In Items
                    Dim Item As New LogInListBoxItem()
                    Item.Text = I
                    _Items.Add(Item)
                Next
                Invalidate()
                InvalidateScroll()
            End Sub

            Public Sub RemoveItemAt(ByVal index As Integer)
                _Items.RemoveAt(index)
                Invalidate()
                InvalidateScroll()
            End Sub

            Public Sub RemoveItem(ByVal item As LogInListBoxItem)
                _Items.Remove(item)
                Invalidate()
                InvalidateScroll()
            End Sub

            Public Sub RemoveItems(ByVal items As LogInListBoxItem())
                For Each I As LogInListBoxItem In items
                    _Items.Remove(I)
                Next
                Invalidate()
                InvalidateScroll()
            End Sub

            Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
                _SelectedHeight = Height
                InvalidateScroll()
                InvalidateLayout()
                MyBase.OnSizeChanged(e)
            End Sub

            Private Sub Vertical_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
                Focus()
            End Sub

            Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
                Focus()
                If e.Button = MouseButtons.Left Then
                    Dim Offset As Integer = CInt(VerticalScrollbar.Value * (VerticalScrollbar.Maximum + (Height - (ItemHeight))))

                    Dim Index As Integer = ((e.Y + Offset) \ ItemHeight)

                    If Index > _Items.Count - 1 Then Index = -1

                    If Not Index = -1 Then

                        If ModifierKeys = Keys.Control AndAlso _MultiSelect Then
                            If _SelectedItems.Contains(_Items(Index)) Then
                                _SelectedItems.Remove(_Items(Index))
                            Else
                                _SelectedItems.Add(_Items(Index))
                            End If
                        Else
                            _SelectedItems.Clear()
                            _SelectedItems.Add(_Items(Index))
                        End If
                        Debug.Print(CStr(_SelectedItems(0).Text))
                    End If

                    Invalidate()
                End If
                MyBase.OnMouseDown(e)
            End Sub

            Protected Overrides Sub OnMouseWheel(ByVal e As MouseEventArgs)
                Dim Move As Integer = -((e.Delta * SystemInformation.MouseWheelScrollLines \ 120) * (2 \ 2))
                Dim Value As Integer = Math.Max(Math.Min(VerticalScrollbar.Value + Move, VerticalScrollbar.Maximum), VerticalScrollbar.Minimum)
                VerticalScrollbar.Value = Value
                MyBase.OnMouseWheel(e)
            End Sub

#End Region

#Region "Draw Control"

            Sub New()
                SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                            ControlStyles.UserPaint Or ControlStyles.Selectable Or _
                            ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                VerticalScrollbar = New LogInVerticalScrollBar
                VerticalScrollbar.SmallChange = 1
                VerticalScrollbar.LargeChange = 1
                AddHandler VerticalScrollbar.Scroll, AddressOf HandleScroll
                AddHandler VerticalScrollbar.MouseDown, AddressOf Vertical_MouseDown
                Controls.Add(VerticalScrollbar)
                InvalidateLayout()
            End Sub

            Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
                Dim G = e.Graphics
                With G
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(_BaseColour)
                    Dim AllItems As LogInListBoxItem
                    Dim Offset As Integer = CInt(VerticalScrollbar.Value * (VerticalScrollbar.Maximum + (Height - (ItemHeight))))
                    Dim StartIndex As Integer
                    If Offset = 0 Then StartIndex = 0 Else StartIndex = CInt(Offset \ ItemHeight \ VerticalScrollbar.Maximum)
                    Dim EndIndex As Integer = Math.Min(StartIndex + (Height \ ItemHeight), _Items.Count - 1)

                    For I As Integer = StartIndex To _Items.Count - 1
                        AllItems = Items(I)
                        Dim Y As Integer = (ItemHeight + (I * ItemHeight) + 1 - Offset) + CInt((ItemHeight / 2) - 8)
                        If _SelectedItems.Contains(AllItems) Then
                            .FillRectangle(New SolidBrush(_SelectedItemColour), New Rectangle(0, ItemHeight + (I * ItemHeight) + 1 - Offset, Width - 19, ItemHeight - 1))
                        Else
                            .FillRectangle(New SolidBrush(_NonSelectedItemColour), New Rectangle(0, ItemHeight + (I * ItemHeight) + 1 - Offset, Width - 19, ItemHeight - 1))
                        End If
                        .DrawLine(New Pen(_BorderColour), 0, (ItemHeight + (I * ItemHeight) + 1 - Offset) + ItemHeight - 1, Width - 18, (ItemHeight + (I * ItemHeight) + 1 - Offset) + ItemHeight - 1)
                        .DrawString(AllItems.Text, New Font("Segoe UI", 8), New SolidBrush(_TextColour), 9, Y)
                        .ResetClip()
                    Next
                    .FillRectangle(New SolidBrush(_TitleAreaColour), New Rectangle(0, 0, Width, ItemHeight))
                    .DrawRectangle(New Pen(Color.FromArgb(35, 35, 35)), 1, 1, Width - 3, ItemHeight - 2)
                    .DrawString(Text, New Font("Segoe UI", 10, FontStyle.Bold), New SolidBrush(_TextColour), New Rectangle(0, 0, Width, ItemHeight + 2), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    .DrawRectangle(New Pen(Color.FromArgb(35, 35, 35), 2), 1, 0, Width - 2, Height - 1)
                    .DrawLine(New Pen(_BorderColour), 0, ItemHeight, Width, ItemHeight)
                    .DrawLine(New Pen(_BorderColour, 2), VerticalScrollbar.Location.X - 1, 0, VerticalScrollbar.Location.X - 1, Height)
                    .InterpolationMode = CType(7, InterpolationMode)
                End With
            End Sub

#End Region

        End Class

        Public Class LogInListBoxWBuiltInScrollBar
            Inherits Control

#Region "Declarations"

            Private _Items As New List(Of LogInListBoxItem)
            Private ReadOnly _SelectedItems As New List(Of LogInListBoxItem)
            Private _MultiSelect As Boolean = True
            Private ItemHeight As Integer = 24
            Private ReadOnly VerticalScrollbar As LogInVerticalScrollBar
            Private _BaseColour As Color = Color.FromArgb(55, 55, 55)
            Private _SelectedItemColour As Color = Color.FromArgb(50, 50, 50)
            Private _NonSelectedItemColour As Color = Color.FromArgb(47, 47, 47)
            Private _BorderColour As Color = Color.FromArgb(35, 35, 35)
            Private _TextColour As Color = Color.FromArgb(255, 255, 255)
            Private _SelectedHeight As Integer = 1

#End Region

#Region "Properties"

            <Category("Colours")> _
            Public Property TextColour As Color
                Get
                    Return _TextColour
                End Get
                Set(value As Color)
                    _TextColour = value
                End Set
            End Property

            <Category("Control")> _
            Public Property SelectedHeight As Integer
                Get
                    Return _SelectedHeight
                End Get
                Set(value As Integer)
                    If value < 1 Then
                        _SelectedHeight = Height
                    Else
                        _SelectedHeight = value
                    End If
                    InvalidateScroll()
                End Set
            End Property

            <Category("Colours")> _
            Public Property BaseColour As Color
                Get
                    Return _BaseColour
                End Get
                Set(value As Color)
                    _BaseColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property SelectedItemColour As Color
                Get
                    Return _SelectedItemColour
                End Get
                Set(value As Color)
                    _SelectedItemColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property NonSelectedItemColour As Color
                Get
                    Return _NonSelectedItemColour
                End Get
                Set(value As Color)
                    _NonSelectedItemColour = value
                End Set
            End Property

            <Category("Colours")> _
            Public Property BorderColour As Color
                Get
                    Return _BorderColour
                End Get
                Set(value As Color)
                    _BorderColour = value
                End Set
            End Property


            Private Sub HandleScroll(ByVal sender As Object)
                Invalidate()
            End Sub

            Private Sub InvalidateScroll()
                Debug.Print(CStr(Height))
                If CInt(Math.Round(((_Items.Count) * ItemHeight) / _SelectedHeight)) < CDbl((((_Items.Count) * ItemHeight) / _SelectedHeight)) Then
                    VerticalScrollbar._Maximum = CInt(Math.Ceiling(((_Items.Count) * ItemHeight) / _SelectedHeight))
                ElseIf CInt(Math.Round(((_Items.Count) * ItemHeight) / _SelectedHeight)) = 0 Then
                    VerticalScrollbar._Maximum = 1
                Else
                    VerticalScrollbar._Maximum = CInt(Math.Round(((_Items.Count) * ItemHeight) / _SelectedHeight))
                End If
                Invalidate()
            End Sub

            Private Sub InvalidateLayout()
                VerticalScrollbar.Location = New Point(Width - VerticalScrollbar.Width - 2, 2)
                VerticalScrollbar.Size = New Size(18, Height - 4)
                Invalidate()
            End Sub

            Public Class LogInListBoxItem
                Property Text As String
                Public Overrides Function ToString() As String
                    Return Text
                End Function
            End Class

            <System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)> _
            Public Property Items() As LogInListBoxItem()
                Get
                    Return _Items.ToArray()
                End Get
                Set(ByVal value As LogInListBoxItem())
                    _Items = New List(Of LogInListBoxItem)(value)
                    Invalidate()
                    InvalidateScroll()
                End Set
            End Property

            Public ReadOnly Property SelectedItems() As LogInListBoxItem()
                Get
                    Return _SelectedItems.ToArray()
                End Get
            End Property

            Public Property MultiSelect() As Boolean
                Get
                    Return _MultiSelect
                End Get
                Set(ByVal value As Boolean)
                    _MultiSelect = value

                    If _SelectedItems.Count > 1 Then
                        _SelectedItems.RemoveRange(1, _SelectedItems.Count - 1)
                    End If

                    Invalidate()
                End Set
            End Property

            Public Overrides Property Font As Font
                Get
                    Return MyBase.Font
                End Get
                Set(ByVal value As Font)
                    ItemHeight = CInt(Graphics.FromHwnd(Handle).MeasureString("@", Font).Height)
                    If VerticalScrollbar IsNot Nothing Then
                        VerticalScrollbar._SmallChange = 1
                        VerticalScrollbar._LargeChange = 1

                    End If
                    MyBase.Font = value
                    InvalidateLayout()
                End Set
            End Property

            Public Sub AddItem(ByVal Items As String)
                Dim Item As New LogInListBoxItem()
                Item.Text = Items
                _Items.Add(Item)
                Invalidate()
                InvalidateScroll()
            End Sub

            Public Sub AddItems(ByVal Items() As String)
                For Each I In Items
                    Dim Item As New LogInListBoxItem()
                    Item.Text = I
                    _Items.Add(Item)
                Next
                Invalidate()
                InvalidateScroll()
            End Sub

            Public Sub RemoveItemAt(ByVal index As Integer)
                _Items.RemoveAt(index)
                Invalidate()
                InvalidateScroll()
            End Sub

            Public Sub RemoveItem(ByVal item As LogInListBoxItem)
                _Items.Remove(item)
                Invalidate()
                InvalidateScroll()
            End Sub

            Public Sub RemoveItems(ByVal items As LogInListBoxItem())
                For Each I As LogInListBoxItem In items
                    _Items.Remove(I)
                Next
                Invalidate()
                InvalidateScroll()
            End Sub

            Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
                _SelectedHeight = Height
                InvalidateScroll()
                InvalidateLayout()
                MyBase.OnSizeChanged(e)
            End Sub

            Private Sub Vertical_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
                Focus()
            End Sub

            Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
                Focus()
                If e.Button = MouseButtons.Left Then
                    Dim Offset As Integer = CInt(VerticalScrollbar.Value * (VerticalScrollbar.Maximum + (Height - (ItemHeight))))

                    Dim Index As Integer = ((e.Y + Offset) \ ItemHeight)

                    If Index > _Items.Count - 1 Then Index = -1

                    If Not Index = -1 Then

                        If ModifierKeys = Keys.Control AndAlso _MultiSelect Then
                            If _SelectedItems.Contains(_Items(Index)) Then
                                _SelectedItems.Remove(_Items(Index))
                            Else
                                _SelectedItems.Add(_Items(Index))
                            End If
                        Else
                            _SelectedItems.Clear()
                            _SelectedItems.Add(_Items(Index))
                        End If
                        Debug.Print(CStr(_SelectedItems(0).Text))
                    End If

                    Invalidate()
                End If
                MyBase.OnMouseDown(e)
            End Sub

            Protected Overrides Sub OnMouseWheel(ByVal e As MouseEventArgs)
                Dim Move As Integer = -((e.Delta * SystemInformation.MouseWheelScrollLines \ 120) * (2 \ 2))
                Dim Value As Integer = Math.Max(Math.Min(VerticalScrollbar.Value + Move, VerticalScrollbar.Maximum), VerticalScrollbar.Minimum)
                VerticalScrollbar.Value = Value
                MyBase.OnMouseWheel(e)
            End Sub

#End Region

#Region "Draw Control"

            Sub New()
                SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                            ControlStyles.UserPaint Or ControlStyles.Selectable Or _
                            ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                VerticalScrollbar = New LogInVerticalScrollBar
                VerticalScrollbar._SmallChange = 1
                VerticalScrollbar._LargeChange = 1
                AddHandler VerticalScrollbar.Scroll, AddressOf HandleScroll
                AddHandler VerticalScrollbar.MouseDown, AddressOf Vertical_MouseDown
                Controls.Add(VerticalScrollbar)
                InvalidateLayout()
            End Sub

            Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
                Dim g = e.Graphics
                With g
                    .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                    .SmoothingMode = SmoothingMode.HighQuality
                    .PixelOffsetMode = PixelOffsetMode.HighQuality
                    .Clear(_BaseColour)
                    Dim AllItems As LogInListBoxItem
                    Dim Offset As Integer = CInt(VerticalScrollbar.Value * (VerticalScrollbar.Maximum + (Height - (ItemHeight))))
                    Dim StartIndex As Integer
                    If Offset = 0 Then StartIndex = 0 Else StartIndex = CInt(Offset \ ItemHeight \ VerticalScrollbar.Maximum)
                    Dim EndIndex As Integer = Math.Min(StartIndex + (Height \ ItemHeight), _Items.Count - 1)
                    .DrawLine(New Pen(_BorderColour, 2), VerticalScrollbar.Location.X - 1, 0, VerticalScrollbar.Location.X - 1, Height)

                    For I As Integer = StartIndex To _Items.Count - 1
                        AllItems = Items(I)
                        Dim Y As Integer = ((I * ItemHeight) + 1 - Offset) + CInt((ItemHeight / 2) - 8)
                        If _SelectedItems.Contains(AllItems) Then
                            .FillRectangle(New SolidBrush(_SelectedItemColour), New Rectangle(0, (I * ItemHeight) + 1 - Offset, Width - 19, ItemHeight - 1))
                        Else
                            .FillRectangle(New SolidBrush(_NonSelectedItemColour), New Rectangle(0, (I * ItemHeight) + 1 - Offset, Width - 19, ItemHeight - 1))
                        End If
                        .DrawLine(New Pen(_BorderColour), 0, ((I * ItemHeight) + 1 - Offset) + ItemHeight - 1, Width - 18, ((I * ItemHeight) + 1 - Offset) + ItemHeight - 1)
                        .DrawString(AllItems.Text, New Font("Segoe UI", 8), New SolidBrush(_TextColour), 9, Y)
                        .ResetClip()
                    Next
                    .DrawRectangle(New Pen(Color.FromArgb(35, 35, 35), 2), 1, 1, Width - 2, Height - 2)
                    '   .DrawLine(New Pen(_BorderColour), 0, ItemHeight, Width, ItemHeight)
                    .DrawLine(New Pen(_BorderColour, 2), VerticalScrollbar.Location.X - 1, 0, VerticalScrollbar.Location.X - 1, Height)
                    .InterpolationMode = CType(7, InterpolationMode)
                End With

            End Sub

#End Region

        End Class

        <DefaultEvent("SelectedIndexChanged")> _
        Public Class LogInPaginator
            Inherits Control

#Region "Declarations"
            Private GP1, GP2 As GraphicsPath

            Private R1 As Rectangle

            Private SZ1 As Size
            Private PT1 As Point

            Private P1, P2, P3 As Pen
            Private B1, B2 As SolidBrush
#End Region

#Region "Functions"
            Public Function RoundRectangle(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
                Dim P As GraphicsPath = New GraphicsPath()
                Dim ArcRectangleWidth As Integer = Curve * 2
                P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
                P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
                P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
                P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
                P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
                Return P
            End Function

            Public Function RoundRect(x!, y!, w!, h!, Optional r! = 0.3, Optional TL As Boolean = True, Optional TR As Boolean = True, Optional BR As Boolean = True, Optional BL As Boolean = True) As GraphicsPath
                Dim d! = Math.Min(w, h) * r, xw = x + w, yh = y + h
                RoundRect = New GraphicsPath
                With RoundRect
                    If TL Then .AddArc(x, y, d, d, 180, 90) Else .AddLine(x, y, x, y)
                    If TR Then .AddArc(xw - d, y, d, d, 270, 90) Else .AddLine(xw, y, xw, y)
                    If BR Then .AddArc(xw - d, yh - d, d, d, 0, 90) Else .AddLine(xw, yh, xw, yh)
                    If BL Then .AddArc(x, yh - d, d, d, 90, 90) Else .AddLine(x, yh, x, yh)
                    .CloseFigure()
                End With
            End Function
#End Region

#Region "Properties & Events"
            Public Event SelectedIndexChanged(sender As Object, e As EventArgs)

            Private _SelectedIndex As Integer
            Public Property SelectedIndex() As Integer
                Get
                    Return _SelectedIndex
                End Get
                Set(ByVal value As Integer)
                    _SelectedIndex = Math.Max(Math.Min(value, MaximumIndex), 0)
                    Invalidate()
                End Set
            End Property

            Private _NumberOfPages As Integer
            Public Property NumberOfPages() As Integer
                Get
                    Return _NumberOfPages
                End Get
                Set(ByVal value As Integer)
                    _NumberOfPages = value
                    _SelectedIndex = Math.Max(Math.Min(_SelectedIndex, MaximumIndex), 0)
                    Invalidate()
                End Set
            End Property

            Public ReadOnly Property MaximumIndex As Integer
                Get
                    Return NumberOfPages - 1
                End Get
            End Property

            Private ItemWidth As Integer
            Public Overrides Property Font As Font
                Get
                    Return MyBase.Font
                End Get
                Set(value As Font)
                    MyBase.Font = value
                    Invalidate()
                End Set
            End Property

            Private Sub InvalidateItems(ByVal e As PaintEventArgs)
                Dim S As Size = e.Graphics.MeasureString("000 ..", Font).ToSize()
                ItemWidth = S.Width + 10
            End Sub

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                If e.Button = Windows.Forms.MouseButtons.Left Then
                    Dim NewIndex As Integer
                    Dim OldIndex As Integer = _SelectedIndex
                    If _SelectedIndex < 4 Then
                        NewIndex = (e.X \ ItemWidth)
                    ElseIf _SelectedIndex > 3 AndAlso _SelectedIndex < (MaximumIndex - 3) Then
                        NewIndex = (e.X \ ItemWidth)
                        Select Case NewIndex
                            Case 2
                                NewIndex = OldIndex
                            Case Is < 2
                                NewIndex = OldIndex - (2 - NewIndex)
                            Case Is > 2
                                NewIndex = OldIndex + (NewIndex - 2)
                        End Select
                    Else
                        NewIndex = MaximumIndex - (4 - (e.X \ ItemWidth))
                    End If
                    If (NewIndex < _NumberOfPages) AndAlso (Not NewIndex = OldIndex) Then
                        SelectedIndex = NewIndex
                        RaiseEvent SelectedIndexChanged(Me, Nothing)
                    End If
                End If
                MyBase.OnMouseDown(e)
            End Sub

#End Region

#Region "Draw Control"

            Sub New()
                SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                          ControlStyles.UserPaint Or ControlStyles.Selectable Or _
                          ControlStyles.SupportsTransparentBackColor, True)
                DoubleBuffered = True
                BackColor = Color.FromArgb(54, 54, 54)
                Size = New Size(202, 26)
                B1 = New SolidBrush(Color.FromArgb(50, 50, 50))
                B2 = New SolidBrush(Color.FromArgb(55, 55, 55))
                P1 = New Pen(Color.FromArgb(35, 35, 35))
                P2 = New Pen(Color.FromArgb(23, 119, 151))
                P3 = New Pen(Color.FromArgb(35, 35, 35))
            End Sub

            Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
                InvalidateItems(e)
                Dim g = e.Graphics
                With g
                    .Clear(BackColor)
                    .SmoothingMode = SmoothingMode.AntiAlias
                    .TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit
                    Dim LeftEllipse, RightEllipse As Boolean
                    If _SelectedIndex < 4 Then
                        For I As Integer = 0 To Math.Min(MaximumIndex, 4)
                            RightEllipse = (I = 4) AndAlso (MaximumIndex > 4)
                            DrawBox(I * ItemWidth, I, False, RightEllipse, g)
                        Next
                    ElseIf _SelectedIndex > 3 AndAlso _SelectedIndex < (MaximumIndex - 3) Then
                        For I As Integer = 0 To 4
                            LeftEllipse = (I = 0)
                            RightEllipse = (I = 4)
                            DrawBox(I * ItemWidth, _SelectedIndex + I - 2, LeftEllipse, RightEllipse, g)
                        Next
                    Else
                        For I As Integer = 0 To 4
                            LeftEllipse = (I = 0) AndAlso (MaximumIndex > 4)
                            DrawBox(I * ItemWidth, MaximumIndex - (4 - I), LeftEllipse, False, g)
                        Next
                    End If
                End With
            End Sub

            Private Sub DrawBox(ByVal x As Integer, ByVal index As Integer, ByVal leftEllipse As Boolean, ByVal rightEllipse As Boolean, ByVal g As Graphics)
                R1 = New Rectangle(x, 0, ItemWidth - 4, Height - 1)
                GP1 = RoundRectangle(R1, 4)
                GP2 = RoundRectangle(New Rectangle(R1.X + 1, R1.Y + 1, R1.Width - 2, R1.Height - 2), 4)
                Dim T As String = CStr(index + 1)
                If leftEllipse Then T = ".. " & T
                If rightEllipse Then T = T & " .."
                SZ1 = g.MeasureString(T, Font).ToSize()
                PT1 = New Point(R1.X + (R1.Width \ 2 - SZ1.Width \ 2), R1.Y + (R1.Height \ 2 - SZ1.Height \ 2))
                If index = _SelectedIndex Then
                    g.FillPath(B1, GP1)
                    Dim F As New Font(Font, FontStyle.Underline)
                    g.DrawString(T, F, Brushes.Black, PT1.X + 1, PT1.Y + 1)
                    g.DrawString(T, F, Brushes.White, PT1)
                    F.Dispose()
                    g.DrawPath(P1, GP2)
                    g.DrawPath(P2, GP1)
                Else
                    g.FillPath(B2, GP1)
                    g.DrawString(T, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1)
                    g.DrawString(T, Font, Brushes.White, PT1)
                    g.DrawPath(P3, GP2)
                    g.DrawPath(P1, GP1)
                End If
            End Sub

#End Region

        End Class

    End Namespace

End Namespace

