//----------------------------------------------------------------------------+
//        Class: frmAssembly
//  Description: This form class allows user to select assembly catalogs
//----------------------------------------------------------------------------+
//     $Author::                                                              $
//       $Date::                                                              $
//   $Revision::                                                              $
//----------------------------------------------------------------------------+
//    $History::                                                              $
//----------------------------------------------------------------------------+
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class frmAssembly : Form
    {
        private IGTApplication m_GTApplication = null;
        private short m_gFno = 0;
        private int m_gFid = 0;
        private int m_Detaild = 0;
        public frmAssembly()
        {
            InitializeComponent();
        }

        #region Members
        public IGTApplication Application
        {
            get
            {
                return m_GTApplication;
            }
            set
            {
                m_GTApplication = value;
            }
        }
        public short FeatureFNO
        {
            get
            {
                return m_gFno;
            }
            set
            {
                m_gFno = value;
            }
        }
        public int FeatureIdentifier
        {
            get
            {
                return m_gFid;
            }
            set
            {
                m_gFid = value;
            }
        }

        public int DetailIdentifier
        {
            get
            {
                return m_Detaild;
            }
            set
            {
                m_Detaild = value;
            }
        }
        #endregion

        #region Events
        private void frmAssembly_Load(object sender, EventArgs e)
        {
            try
            {
                int cnt = 0;
                ADODB.Recordset rs;

                string sql = "SELECT cel.G3E_DESCRIPTION FROM CATALOGENTRYLIST cel WHERE cel.G3E_FNO=:1";
                // Eliminated obsolete join to switch gear polygon E$ table (Rich Adase, 30-APR-2019)

                rs = m_GTApplication.DataContext.Execute(sql, out cnt, (int)ADODB.CommandTypeEnum.adCmdText, FeatureFNO);
                if (rs != null)
                {
                    if (!(rs.EOF && rs.BOF))
                    {
                        rs.MoveFirst();
                        for (int i = 0; i < rs.RecordCount; i++)
                        {
                            cmbAssembly.Items.Add(rs.Fields["G3E_DESCRIPTION"].Value);
                            rs.MoveNext();
                        }
                    }
                }
                if (cmbAssembly.Items.Count > 0)
                {
                    cmbAssembly.SelectedItem = cmbAssembly.Items[0];
                    btnOK.Enabled = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                int cnt = 0;
                ADODB.Recordset rs;
                string sql = "SELECT G3E_IDENTIFIER FROM CATALOGENTRYLIST WHERE G3E_FNO=:1 AND G3E_DESCRIPTION=:2";
                rs = m_GTApplication.DataContext.Execute(sql, out cnt, (int)ADODB.CommandTypeEnum.adCmdText, FeatureFNO, cmbAssembly.SelectedItem.ToString());
                if (rs != null)
                {
                    if (!(rs.EOF && rs.BOF))
                    {
                        rs.MoveFirst();
                        m_gFid = Convert.ToInt32(rs.Fields["G3E_IDENTIFIER"].Value);
                    }
                }
                this.Close();
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
