// ==============================================================================================================================================================================
//  File Name: AccountImpact.cs
// 
// Description:   

// Set the writeback needed flag in the job records.
// Synchronize that CU components with the corresponding Work Point(creating or deleting it if necessary) ,If Workpoint is not there Validation FI creates the Workpoint feature .

//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  01/02/2018          Sithara                    Implemented  Business Rule as per JIRA 1294
//  03/09/2018          Hari                       Ported logic from fvAccountingImpact.cs  
// ==============================================================================================================================================================================

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ADODB;
using GTechnology.Oncor.CustomAPI;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    class AccountImpact : IProcessFV
    {
        private IGTKeyObjects m_features;
        private Recordset m_ValidationErrors;
        private CommonFunctions m_gtcommonFunctions = new CommonFunctions();
        private IGTDataContext m_iGtDataContext;  
        private WorkPointOperations oWorkPointOperations = null;
        public IGTDataContext DataContext
        {
            get { return m_iGtDataContext; }
            set { m_iGtDataContext = value; }
        }

        public IGTKeyObjects Features
        {
            get { return m_features; }
            set { m_features = value; }
        }

        public Recordset ValidationErrors
        {
            get { return m_ValidationErrors; }
            set { m_ValidationErrors = value; }
        }

        /// <summary>
        /// Process Accounting Impact validation
        /// </summary>
        /// <returns>true if the process is complete without errors, else returns false</returns>
        public bool Process()
        {
            try
            {
                Dictionary<IGTKeyObject, IGTComponents> oDicFeatureCollection = new Dictionary<IGTKeyObject, IGTComponents>();
                oDicFeatureCollection = GetComponentsChangedCollection(Features);

                if (oDicFeatureCollection.Count > 0)
                {
                    oWorkPointOperations = new WorkPointOperations(null, null, DataContext, oDicFeatureCollection);
                    oWorkPointOperations.DoWorkpointOperations();

                    if (oWorkPointOperations.CUCompModified)
                    {
                        UpdateWritebackFlagAndAlternateDesigns();
                    }

                    RemoveCorrectionModeProperty();
                }
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error during execution of Accounting Impact FV." + Environment.NewLine + ex.Message,
                // "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw ex;
            }
            
        }
         
        public Dictionary<IGTKeyObject,IGTComponents> GetComponentsChangedCollection(IGTKeyObjects p_KeyObjects)
        {
            Dictionary<IGTKeyObject, IGTComponents> dicReturn = new Dictionary<IGTKeyObject, IGTComponents>();
            IGTComponents gTCUComponents = GTClassFactory.Create<IGTComponents>();

            try
            {
                foreach (IGTKeyObject oKeyObject in p_KeyObjects)
                {
                    if (oKeyObject.Components.GetComponent(21) != null)
                    {
                        gTCUComponents.Add(oKeyObject.Components.GetComponent(21));
                    }

                    if (oKeyObject.Components.GetComponent(22) != null)
                    {
                        gTCUComponents.Add(oKeyObject.Components.GetComponent(22));
                    }
                    IGTComponents oCompCollection = SetComponentsChanged(gTCUComponents);
                    if (oCompCollection.Count > 0)
                    {
                        dicReturn.Add(oKeyObject, oCompCollection);
                    }
                }                
            }
            catch
            {
                throw;
            }
            return dicReturn;
        }

        public IGTComponents SetComponentsChanged(IGTComponents gTChangedComponents)
        {
            Recordset rs = null;
            IGTComponents m_gtAllCUComponents = GTClassFactory.Create<IGTComponents>();

            try
            {
                foreach (IGTComponent gtComp in gTChangedComponents)
                {
                    rs = gtComp.Recordset;
                    if (rs != null && rs.RecordCount > 0)
                    {
                        rs.MoveFirst();
                        while (!rs.EOF)
                        {
                            foreach (Field field in rs.Fields)
                            {
                                if (field.OriginalValue.ToString() != field.Value.ToString())
                                {
                                    if (!m_gtAllCUComponents.Contains(gtComp))//Added by Prathyusha
                                    {
                                        m_gtAllCUComponents.Add(gtComp);
                                    }
                                    break;
                                }
                            }
                            rs.MoveNext();
                        }
                    }

                }
            }
            catch
            {
                throw;
            }
            return m_gtAllCUComponents;
        }
        private void RemoveCorrectionModeProperty()
        {
            IGTApplication m_oApp = GTClassFactory.Create<IGTApplication>();
            if (m_oApp.Properties.Count > 0)
            {
                try
                {
                    m_oApp.Properties.Remove("CorrectionsMode");
                }
                catch (Exception)
                {
                    
                }
            }

            string mapCaption = m_oApp.Application.ActiveMapWindow.Caption.Replace("CORRECTIONS MODE - ", "");
            m_oApp.Application.ActiveMapWindow.Caption = mapCaption;
        }

        /// <summary>
        /// UpdateWritebackFlagAndAlternateDesigns : Updates Writeback Needed flag in the job table to Y,If alternate designs exist set the flag to Y for all versions..
        /// </summary>
        /// <returns></returns>
        private void UpdateWritebackFlagAndAlternateDesigns()
        {
            Recordset rsJob = null;
            int count = 0;
            int wrNbr = 0;
            try
            {
                #region WritebackFlag

                string sql = "select WR_NBR from g3e_job where G3E_IDENTIFIER=:1";
                rsJob = DataContext.Execute(sql, out count, (int)ADODB.CommandTypeEnum.adCmdText, DataContext.ActiveJob.ToString());
                rsJob.MoveFirst();

                if (!string.IsNullOrEmpty(Convert.ToString(rsJob.Fields["WR_NBR"].Value)))
                {
                    wrNbr = Convert.ToInt32(rsJob.Fields["WR_NBR"].Value);
                }
                
                ExecuteCommand(string.Format("update g3e_job set WRITEBACK_NEEDED_YN = 'Y' where g3e_identifier = '{0}'", DataContext.ActiveJob.ToString()));

                #endregion

                #region AlternateDesigns

                if (wrNbr > 0)
                {
                    ExecuteCommand(string.Format("update g3e_job set WRITEBACK_NEEDED_YN = 'Y' where WR_NBR={0}", wrNbr));
                }

                #endregion

            }
            catch
            {
                throw;
            }
            finally
            {
                if (rsJob != null)
                {
                    rsJob.Close();
                    rsJob = null;
                }
            }
        }

        /// <summary>
        /// Method to execute sql query and return the result record set
        /// </summary>
        /// <param name="sqlString">Query string</param>
        /// <returns></returns>
        private Recordset ExecuteCommand(string sqlString)
        {
            int outRecords = 0;
            try
            {
                ADODB.Command command = new ADODB.Command();
                command.CommandText = sqlString;
                ADODB.Recordset results = DataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
