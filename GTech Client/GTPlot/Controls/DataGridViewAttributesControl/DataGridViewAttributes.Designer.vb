<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DataGridViewAttributes
  Inherits System.Windows.Forms.UserControl

  'UserControl1 overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container
    Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
    Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
    Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
    Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DataGridViewAttributes))
    Me.DataGridView1 = New System.Windows.Forms.DataGridView
    Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn
    Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn
    Me.oComboBox = New System.Windows.Forms.ComboBox
    Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
    CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'DataGridView1
    '
    Me.DataGridView1.AllowUserToAddRows = False
    Me.DataGridView1.AllowUserToDeleteRows = False
    Me.DataGridView1.AllowUserToResizeRows = False
    Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
    DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
    DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
    DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
    DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
    DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
    DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
    Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
    Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
    Me.DataGridView1.ColumnHeadersVisible = False
    Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
    DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
    DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
    DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
    DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
    DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
    DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
    Me.DataGridView1.DefaultCellStyle = DataGridViewCellStyle3
    Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.DataGridView1.Location = New System.Drawing.Point(0, 0)
    Me.DataGridView1.MultiSelect = False
    Me.DataGridView1.Name = "DataGridView1"
    DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
    DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
    DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
    DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
    DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
    DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
    Me.DataGridView1.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
    Me.DataGridView1.RowHeadersVisible = False
    Me.DataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
    Me.DataGridView1.Size = New System.Drawing.Size(279, 317)
    Me.DataGridView1.TabIndex = 0
    '
    'Column1
    '
    Me.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
    DataGridViewCellStyle2.BackColor = System.Drawing.Color.White
    Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
    Me.Column1.HeaderText = "Name"
    Me.Column1.Name = "Column1"
    Me.Column1.ReadOnly = True
    Me.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
    '
    'Column2
    '
    Me.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
    Me.Column2.HeaderText = "Value"
    Me.Column2.Name = "Column2"
    Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
    '
    'oComboBox
    '
    Me.oComboBox.FormattingEnabled = True
    Me.oComboBox.Location = New System.Drawing.Point(140, 3)
    Me.oComboBox.Name = "oComboBox"
    Me.oComboBox.Size = New System.Drawing.Size(136, 21)
    Me.oComboBox.TabIndex = 1
    Me.oComboBox.Visible = False
    '
    'ImageList1
    '
    Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
    Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
    Me.ImageList1.Images.SetKeyName(0, "CheckBox.bmp")
    Me.ImageList1.Images.SetKeyName(1, "CheckBox-Checked.bmp")
    Me.ImageList1.Images.SetKeyName(2, "CheckBox-Neutral.bmp")
    Me.ImageList1.Images.SetKeyName(3, "DropDownList.bmp")
    '
    'DataGridViewAttributes
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.oComboBox)
    Me.Controls.Add(Me.DataGridView1)
    Me.Name = "DataGridViewAttributes"
    Me.Size = New System.Drawing.Size(279, 317)
    CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents oComboBox As System.Windows.Forms.ComboBox
  Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
  Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
  Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
  Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
