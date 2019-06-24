// ===================================================
//  Copyright 2018 Hexagon.
//  File Name: UserPreferenceForm.cs
// 
//  Description: This command allows users to identify multiple transformers as being bussed together.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  24/05/2018          Shubham                     Created 
// ======================================================
using System;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public enum ButtonClicked
    {
        Ok, Cancel, Additional
    }
    public partial class UserPreferenceForm : Form
    {
        //public bool OKButton { get; set; }
        public bool ShowAdditionalButton { get; set; }
        public string OkButtonCaption { get; set; }
        public string AdditionalButtonCaption { get; set; }
        public string PromptText { get; set; }
        public bool ShowCancelButton { get; set; }

        public ButtonClicked ButtonClicked { get; set; }
        public UserPreferenceForm()
        {
            InitializeComponent();
        }

        private void UserPreferenceForm_Load(object sender, EventArgs e)
        {
            ButtonClicked = ButtonClicked.Cancel;
            btnAdditonal.Visible = ShowAdditionalButton == true ? true : false;
            btnCancel.Visible = ShowCancelButton == true ? true : false;

            btnAdditonal.Text = AdditionalButtonCaption;
            btnOK.Text = OkButtonCaption;
            label1.Text = PromptText;
        }

        private void btnAdditonal_Click(object sender, EventArgs e)
        {
            ButtonClicked = ButtonClicked.Additional;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ButtonClicked = ButtonClicked.Ok;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ButtonClicked = ButtonClicked.Cancel;
            this.Close();
        }
    }
}
