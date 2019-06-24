using System;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
  public partial class frmCreateAlternateDesign : Form
  {
    public frmCreateAlternateDesign()
    {
      InitializeComponent();
    }

    private DataProvider m_dataProvider = null;

    private void btnOK_Click(object sender, EventArgs e)
    {
      try
      {
        m_dataProvider.SaveData(lblNewAlternate.Text, chkCopyJobEdits.Checked, newDescriptionTextbox.Text);
        this.Close();
      }
      catch(Exception ex)
      {
        MessageBox.Show("Error occured in Create Alternate Design -" + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void frmCreateAlternateDesign_Load(object sender, EventArgs e)
    {

      m_dataProvider = new DataProvider();
      //load data to be displayed on form
      m_dataProvider.InitialLoadData();

      if(m_dataProvider.JobRecordData.RecordCount == 0) //if inital load could not load any data, error out.
      {
        MessageBox.Show("Form data could not be retrieved. Please see system administrator.", "G/Technology", MessageBoxButtons.OK);
        this.Close();
        return;
      }

      string jobType = Convert.ToString(m_dataProvider.JobRecordData.Fields["G3E_JOBTYPE"].Value);
      jobType = jobType.ToUpper().Trim();

      if(jobType == "NON-WR")
      {
        MessageBox.Show("This command applies only to WR jobs.", "G/Technology", MessageBoxButtons.OK);
        this.Close();
        return;
      }

      string jobStatus = Convert.ToString(m_dataProvider.JobRecordData.Fields["G3E_JOBSTATUS"].Value);
      jobStatus = jobStatus.ToUpper().Trim();

      if(jobStatus != "DESIGN")
      {
        MessageBox.Show("Job Status must be Design", "G/Technology", MessageBoxButtons.OK);
        this.Close();
        return;
      }

      lblWorkRequest.Text = Convert.ToString(m_dataProvider.JobRecordData.Fields["WR_NBR"].Value);
      lblDescription.Text = Convert.ToString(m_dataProvider.JobRecordData.Fields["G3E_DESCRIPTION"].Value);

      newDescriptionTextbox.Text = lblDescription.Text;
      chkCopyJobEdits.Checked = true;    //default to checked

      //check for existing alternate job to see if jobRenamedLabel should be displayed (red italic text)

      m_dataProvider.CheckForAlternate(lblWorkRequest.Text);

      if(m_dataProvider.HasAlternate)
      {
        jobRenamedLabel.Hide();
        string newAlternateSuffix = m_dataProvider.GetAlternateSuffix();

        if(newAlternateSuffix == "ZZ ERROR") //suffix cannot exceed "ZZ"
        {
          MessageBox.Show("WR Alternate Design cannot go past ZZ. Please see system administrator.", "G/Technology", MessageBoxButtons.OK);
          this.Close();
          return;
        }
        else
        {
          lblNewAlternate.Text = newAlternateSuffix; //set newAlternateLabel to equal the suffix that will be used for new alt design
        }
      }
      else
      {
        lblNewAlternate.Text = "B";
      }
    }

    private void frmCreateAlternateDesign_FormClosing(object sender, FormClosingEventArgs e)
    {
      m_dataProvider.Dispose();
      m_dataProvider = null;
    }
  }
}
