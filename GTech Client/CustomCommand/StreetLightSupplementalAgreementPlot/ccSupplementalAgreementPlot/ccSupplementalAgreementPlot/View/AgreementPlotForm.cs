using System;
using System.Windows.Forms;
using GTechnology.Oncor.CustomAPI.Presenter;

namespace GTechnology.Oncor.CustomAPI.View
{
    public partial class AgreementPlotForm : Form
    {
        SupplementalAgreementPlotPresenter m_plotPresenter;
        public AgreementPlotForm(SupplementalAgreementPlotPresenter plotPresenter)
        {
            InitializeComponent();
            m_plotPresenter = plotPresenter;

            txtPlotName.Text = plotPresenter.m_PlotName;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.FormClosing -= AgreementPlotForm_FormClosing;
                this.Close();
                m_plotPresenter.CompleteCC();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error,
                          MessageBoxDefaultButton.Button1);
                m_plotPresenter.CompleteCC();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_plotPresenter.GeneratePlot(txtPlotName.Text, txtCity.Text, txtSubdivision.Text))
                {
                    this.Close();
                    m_plotPresenter.CompleteCC();
                }
               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
                m_plotPresenter.CompleteCC();
            }
        }

        private void AgreementPlotForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.FormClosing -= AgreementPlotForm_FormClosing;
            this.Close();
            m_plotPresenter.CompleteCC();
        }        
    }
}
