
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  05/03/2018          Pramod                     Implemented Street Light Boundary 
// ======================================================

using ADODB;
using GTechnology.Oncor.CustomAPI.DataAccess;
using GTechnology.Oncor.CustomAPI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI.GUI
{
    public partial class StreetLightBndry : Form
    {
        List<StreetLightBoundary> streetLightBndrys = null;
        bool isInValidExists = false;
        string _selectedBndClass;
        string _selectedIDValue;
        int _selectedBndryG3eFid;//this is useful when click on show Boundary instead of querying DB table to get boundary g3efid
        string InValidMsg = "The attribute (or attributes) used to define the highlighted boundaries causes the query to return ambiguous results.\n Please review this boundary definition before selecting a boundary from it";


        /// <summary>
        /// Selected Boundary Definition Class
        /// </summary>
        public string SelectedBndClass { get => _selectedBndClass; }

        /// <summary>
        /// Selected Boundary Attribute Value
        /// </summary>
        public string SelectedIDValue { get => _selectedIDValue; }

        /// <summary>
        /// Selected Boundary Feature Instance (G3E_FID)
        /// </summary>
        public int SelectedBndryG3eFid { get => _selectedBndryG3eFid; }

        public StreetLightBndry()
        {
            InitializeComponent();
            btnSelect.Enabled = false;
            treeViewBndry.AfterSelect += TreeViewBndry_AfterSelect;
            btnSelect.DialogResult = DialogResult.OK;
            btnSelect.Click += btnSelect_Click;
            this.Shown += StreetLightBndry_Shown;
        }

        private void StreetLightBndry_Shown(object sender, EventArgs e)
        {
            if (isInValidExists)
            {
                MessageBox.Show(InValidMsg, "Street Light Boundary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }


        public StreetLightBndry(List<StreetLightBoundary> streetLightBndrys) : this()
        {
            this.streetLightBndrys = streetLightBndrys.OrderBy(a => a.Bnd_Fno).ToList();
            AddBoundarystoTreeView();
        }


        #region events
        private void btnSelect_Click(object sender, EventArgs e)
        {
            //Get Bounary Class, attribute value and Boundary G3eFid for selected node in the Tree View
            _selectedBndClass = treeViewBndry.SelectedNode.Name;
            _selectedIDValue = treeViewBndry.SelectedNode.Text;
            _selectedBndryG3eFid = Convert.ToInt32(treeViewBndry.SelectedNode.Tag);
        }

        private void TreeViewBndry_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //disable select button when root node having InValid records or NoRecords 
            // btnSelect.Enabled = (e.Node.GetNodeCount(true) == 0 && (e.Node.Name != "NoRecords" && e.Node.Name != "InValid" && e.Node.Parent.Name != "InValid")) ? true : false;
            btnSelect.Enabled = (e.Node.GetNodeCount(true) == 0 && (e.Node.Name != "NoRecords" && e.Node.Parent.Name != "InValid")) ? true : false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add Boundary Definition to Tree View
        /// </summary>
        private void AddBoundarystoTreeView()
        {
            TreeNode node;
            foreach (StreetLightBoundary bndry in streetLightBndrys)
            {
                //check bounary Type exists for boundary definition
                if (string.IsNullOrEmpty(bndry.Bnd_Type))
                {
                    node = GetBoundaryNode(bndry);
                }
                else
                {
                    node = GetBoundaryNodeByType(bndry);
                }
                //Add Bounary to Tree view
                if (!treeViewBndry.Nodes.Contains(node)) { treeViewBndry.Nodes.Add(node); }
            }

        }

        /// <summary>
        /// Add Boundary to Tree node 
        /// </summary>
        /// <param name="stltBndry"></param>
        /// <returns></returns>
        private TreeNode GetBoundaryNode(StreetLightBoundary stltBndry)
        {
            string sqlStmt = "Select G3E_FID as Key,{0} as Value from {1} where {0} is not null order by {0}";
            TreeNode childNode = null;
            Recordset rs = null;
            TreeNode parentNode = new TreeNode();
            //Assign boundary g3e_fno
            parentNode.Tag = stltBndry.Bnd_Fno;
            //show Feature name for given Boundary Fno
            parentNode.Text = CommonUtil.GetUsernameByFno(stltBndry.Bnd_Fno);
            //concat boundary Fno,boundary ID ano and assign to Node name- this is for to indentify node and combine all subclasses
            parentNode.Name = stltBndry.Bnd_Fno + "-" + stltBndry.Bnd_ID_Ano;
            if (!String.IsNullOrEmpty(stltBndry.Bnd_ID_G3efield) && !string.IsNullOrEmpty(stltBndry.Bnd_ID_G3eCName))
            {
                rs = CommonUtil.Execute(string.Format(sqlStmt, stltBndry.Bnd_ID_G3efield, stltBndry.Bnd_ID_G3eCName));
            }
            else
            {
                parentNode.BackColor = Color.Yellow;
                parentNode.Name = "InValid";
                isInValidExists = true;
            }
            if (rs != null && rs.RecordCount > 0)
            {
                var bndrys = CommonUtil.ConvertRSToKeyValue(rs);
                var bndryValues = bndrys
                       .GroupBy(b => b.Value)
                       .Select(s => new { kValue = s.Key, count = s.Count() }).ToList().Where(c => c.count > 1);

                /*
                 * Check if there are any duplicate value if exits then make that root node as Invalid 
                 and set root node back ground color to yellow
                 */
                if (bndryValues.Count() > 0)
                {
                    parentNode.BackColor = Color.Yellow;
                    parentNode.Name = "InValid";
                  //  childNode.Name = "InValid";
                    isInValidExists = true;
                }
                    foreach (var bndry in bndrys)
                {
                    childNode = new TreeNode(bndry.Value);
                    //Assign boundary class to Treenode name
                    childNode.Name = stltBndry.Bnd_Class.ToString();
                    //assign Boundary g3e_fid  to childnode 
                    childNode.Tag = bndry.Key;
                    parentNode.Nodes.Add(childNode);
                }
            }
            else
            {
                parentNode.Nodes.Add("NoRecords", "<<No records found for this boundary definition>>");
            }

            return parentNode;
        }

        /// <summary>
        /// Combine all sub-classes for the same feature in the same root node  and add boundary to treenode
        /// </summary>
        /// <param name="stltBndry"></param>
        /// <returns>return treenode</returns>
        private TreeNode GetBoundaryNodeByType(StreetLightBoundary stltBndry)
        {
            string sqlStmt = "SELECT a.G3E_FID as key, a.{0} as Value FROM {1} a where a.{2}='{3}' and a.{0} is not null order by a.{0}";
            TreeNode childNode = null;
            Recordset rs = null;

            //check tree node exist with combination of FNO+TYPE_Ano to combine all sub classes for the same feature
            TreeNode parentNode = FindNodeByName(stltBndry.Bnd_Fno + "-" + stltBndry.Bnd_Type_Ano) ?? new TreeNode();
            //assign Boundary g3e_fid  to childnode 
            parentNode.Tag = stltBndry.Bnd_Fno;
            parentNode.Text = CommonUtil.GetUsernameByFno(stltBndry.Bnd_Fno);
            //concat boundary Fno and boundary ID ano and assign to Node name- this is for to indentify node and combine all subclasses
            parentNode.Name = stltBndry.Bnd_Fno + "-" + stltBndry.Bnd_Type_Ano;
            TreeNode typeNode = new TreeNode(stltBndry.Bnd_Type);

            if (!String.IsNullOrEmpty(stltBndry.Bnd_ID_G3efield) && !string.IsNullOrEmpty(stltBndry.Bnd_ID_G3eCName) && !string.IsNullOrEmpty(stltBndry.Bnd_Type_G3eField))
            {
                rs = CommonUtil.Execute(string.Format(sqlStmt, stltBndry.Bnd_ID_G3efield, stltBndry.Bnd_ID_G3eCName, stltBndry.Bnd_Type_G3eField, stltBndry.Bnd_Type));
            }
            else
            {
                parentNode.BackColor = Color.Yellow;
                typeNode.BackColor = Color.Yellow;
                typeNode.Name = "InValid";
                isInValidExists = true;
            }

          
            if (rs != null && rs.RecordCount > 0)
            {
                var bndrys = CommonUtil.ConvertRSToKeyValue(rs);
               
                var bndryValues = bndrys
                            .GroupBy(b => b.Value)
                            .Select(s => new { kValue = s.Key, count = s.Count() }).ToList().Where(c => c.count > 1);

                /*
                 * Check if there are any duplicate value if exits then make that root node as Invalid 
                 and set root node back ground color to yellow
                 */
                if(bndryValues.Count()>0)
                {
                    parentNode.BackColor = Color.Yellow;
                    typeNode.BackColor = Color.Yellow;
                    typeNode.Name = "InValid";
                    isInValidExists = true;
                }
                foreach (var bndry in bndrys)
                {
                    childNode = new TreeNode(bndry.Value);
                    //Assign boundary class to Treenode name
                    childNode.Name = stltBndry.Bnd_Class.ToString();
                    //assign Boundary g3e_fid  to Treenode.Tag 
                    childNode.Tag = bndry.Key;
                    typeNode.Nodes.Add(childNode);
                }
            }
            else
            {
                typeNode.Nodes.Add("NoRecords", "<<No records found for this boundary definition>>");
            }
            parentNode.Nodes.Add(typeNode);
            return parentNode;
        }


        /// <summary>
        /// Traverse entire root node check node with given input node name exists if exists returns matching treenode
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns>Return TreeNode</returns>
        private TreeNode FindNodeByName(string nodeName)
        {
            TreeNode node = null;
            foreach (TreeNode tNode in treeViewBndry.Nodes)
            {
                if (tNode.Name == nodeName)
                {
                    node = tNode;
                    break;
                }
            }
            return node;
        }
        #endregion
    }
}
