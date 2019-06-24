using System;
using System.Linq;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using ADODB;

//----------------------------------------------------------------------------+
//  Class: AttributeEditControl
//  Description: This interface controls the enabling of certain attributes 
//----------------------------------------------------------------------------+
//  $Author::   Shubham Agarwal                                               $
//  $Date::     8 July 2017                                                     $
//  $Revision::      1                                                        $
//----------------------------------------------------------------------------+
//  $History::                                                                $
//----------------------------------------------------------------------------+


namespace GTechnology.Oncor.CustomAPI
{
    public class aecRestrictByJobAttriute : IGTAttributeEditControl
    {
        private IGTDataContext m_oDataContext = null;
        private string m_sComponentName = string.Empty;
        private string m_sFieldName = string.Empty;
        private IGTFieldValue m_fieldValue = null;

        private IGTComponents m_componentCollection = null;
        private GTArguments m_GTArguments = null;
        private int m_sjobANO = 0;
        private string m_sCompareValue = string.Empty;
        private int m_iRestrictOnMatch = 0;
        private string m_configuredComponentName = string.Empty;

        public GTArguments Arguments
        {
            get
            {
                return m_GTArguments;
            }

            set
            {
                m_GTArguments = value;

                if (m_GTArguments.Count < 4 || m_GTArguments.Count > 4)
                {
                    throw new Exception("The configured arguments for\"Restrict By Job Attribute\" Attribute Edit Control Interface should be 4");
                }
                m_sjobANO = Convert.ToInt32(m_GTArguments.GetArgument(0));          //jobANO - Attribute number of the job attribute that controls access.

                m_sCompareValue = m_GTArguments.GetArgument(1).ToString();          //compareValue - Value to compare to the specified job attribute; multiple 
                                                                                    //values may be provided in a comma-delimited string.  Values preceded by # are 
                                                                                    //interpreted as attribute identifiers (G3E_ANO), which must refer to an attribute 
                                                                                    //of the same component for which this interface is configured.

                m_iRestrictOnMatch = Convert.ToInt32(m_GTArguments.GetArgument(2)); //restrictOnMatch - Controls comparison behavior                                                                                    
                                                                                    //	1 = Restrict access if a match is found in compareValue
                                                                                    //	0 = Restrict access if no match is found in compareValue

                m_configuredComponentName = Convert.ToString(m_GTArguments.GetArgument(3)); //Component name of the attribute on which FI is configured
            }
        }

        public string ComponentName
        {
            get
            {
                return m_sComponentName;
            }

            set
            {
                m_sComponentName = value;
            }
        }

        public IGTComponents Components
        {
            get
            {
                return m_componentCollection;
            }

            set
            {
                m_componentCollection = value;
            }
        }

        public IGTDataContext DataContext
        {
            get
            {
                return m_oDataContext;
            }

            set
            {
                m_oDataContext = value;
            }
        }

        public string FieldName
        {
            get
            {
                return m_sFieldName;
            }

            set
            {
                m_sFieldName = value;
            }
        }

        public IGTFieldValue FieldValue
        {
            get
            {
                return m_fieldValue;
            }

            set
            {
                m_fieldValue = value;
            }
        }

        public bool IsAttributeEditable(int ANO)
        {          
            if (string.IsNullOrEmpty(m_sComponentName)) m_sComponentName = m_configuredComponentName;

            string sFieldName = string.Empty;
            string sql = string.Empty;
            string sJobAttributeValue = string.Empty;
            bool bdefaultAttributeControlSetting = true;
            int sFNO = 0;
            string sReadOnly = string.Empty;
            bool bCompareMatchFound = false;
            bool bEditControl = true;
            Recordset rs = null;
            string sCompNameForFNO = m_sComponentName;

            try
            {
                IGTApplication oApp = GTClassFactory.Create<IGTApplication>();

                string jobAttributeValue = oApp.DataContext.ActiveJob;
                bool mainComponentRecordEmpty = false;

                //First store the default setting of g3e_readonly value of tab attribute configured against the ANO (g3e_Ano of the field which triggered this interface)
                //Store the default setting of g3e_readonly for the enabling and disabling of the tab attribute in case of no action scenarios

                //The m_sComponentName may not have a recordset at the time of the initialization of the tabs, so look for a component that has the record that may
                // not be necessarily m_sComponentName

                if (m_componentCollection != null)
                {
                    if (m_componentCollection[m_sComponentName].Recordset.RecordCount == 0)
                    {
                        mainComponentRecordEmpty = true;
                        for (int i = 0; i < m_componentCollection.Count; i++)
                        {
                            if (m_componentCollection[i].Recordset.RecordCount > 0)
                            {
                                sCompNameForFNO = m_componentCollection[i].Name;
                                m_componentCollection[sCompNameForFNO].Recordset.MoveFirst();
                                break;
                            }
                        }
                    }
                }
                if (m_componentCollection!=null && m_componentCollection[sCompNameForFNO].Recordset!=null)
                {
                    sFNO = m_componentCollection[sCompNameForFNO].Recordset.Fields["G3E_FNO"].Value.GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(m_componentCollection[sCompNameForFNO].Recordset.Fields["G3E_FNO"].Value);
                }
                sql = "select distinct(g3e_readonly) from G3E_DIALOGATTRIBUTES_OPTABLE a, g3e_dialogs_optable b where a.g3E_ano = " + ANO + " and a.g3E_dtno = b.g3e_dtno and b.g3e_type <> 'Review' and b.g3e_type <> 'G3E_JOBENVIRONMENT' and b.g3E_fno = " + sFNO;
                rs = DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, new object[1]);

                if (rs != null)
                {
                    if (!(rs.EOF && rs.BOF))
                    {
                        rs.MoveFirst();
                        sReadOnly = Convert.ToString(rs.Fields["g3e_readonly"].Value);
                    }
                }

                bdefaultAttributeControlSetting = sReadOnly == "1" ? false : true;
                bEditControl = bdefaultAttributeControlSetting;


                if (string.IsNullOrEmpty(oApp.DataContext.ActiveJob)) //Return without any action if no active job exists
                {
                    return bEditControl;
                }

                if (mainComponentRecordEmpty)
                {
                    return bEditControl;
                }
                //Know the field name of Job Attribute configured in the argument to control access to the tab attribute
                rs = oApp.DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE");
                rs.Filter = "G3E_ANO = " + m_sjobANO + " AND G3E_NAME = 'G3E_JOB'";
                string sPickListTable = string.Empty;
                string sKeyField = string.Empty;
                string sValueField = string.Empty;
                string sComparedJobKeyField = string.Empty;

                if (rs != null)
                {
                    if (!(rs.EOF && rs.BOF))
                    {
                        rs.MoveFirst();
                        sFieldName = Convert.ToString(rs.Fields["G3E_FIELD"].Value);
                        sPickListTable = Convert.ToString(rs.Fields["G3E_PICKLISTTABLE"].Value);
                        sKeyField = Convert.ToString(rs.Fields["G3E_KEYFIELD"].Value);
                        sValueField = Convert.ToString(rs.Fields["G3E_VALUEFIELD"].Value);

                    }
                    else
                    {
                        oApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Error - The configured job attribute that controls access does not specify a valid configuration...");
                        return bEditControl;
                    }
                }

             //   Recordset rsTemp = null;

                //Store value of the job attribute configured in the interface argument
                if (!string.IsNullOrEmpty(sFieldName))
                {
                    sql = "select " + sFieldName + " from g3e_job where G3E_IDENTIFIER = '" + DataContext.ActiveJob + "'";
                    rs = DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, new object[1]);
                    if (rs != null)
                    {
                        if (!(rs.EOF && rs.BOF))
                        {
                            rs.MoveFirst();
                            sJobAttributeValue = Convert.ToString(rs.Fields[0].Value);
                        }
                    }
                }

                if (m_sCompareValue.Contains("#"))
                {
                    string sLocalANO = m_sCompareValue.Remove(0, 1);
                    string sLocalFieldName = string.Empty;

                    rs = DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE");

                    rs.Filter = "g3e_ano = " + sLocalANO;

                    if (rs != null)
                    {
                        if (!(rs.EOF && rs.BOF))
                        {
                            rs.MoveFirst();
                            sLocalFieldName = Convert.ToString(rs.Fields["G3E_FIELD"].Value);
                            if (m_componentCollection[ComponentName].Recordset != null && m_componentCollection[ComponentName].Recordset.RecordCount > 0)
                            {
                                try
                                {
                                    m_sCompareValue = Convert.ToString(m_componentCollection[ComponentName].Recordset.Fields[sLocalFieldName].Value);
                                }
                                catch (Exception)
                                {
                                    throw new Exception("The " + sLocalFieldName + " is not a valid field on the component" + ComponentName);
                                }

                                bCompareMatchFound = m_sCompareValue.Equals(sJobAttributeValue) == true ? true : false;
                            }
                        }
                    }
                }

                else
                {
                    if (m_sCompareValue.Contains(","))
                    {
                        string[] sApp = m_sCompareValue.Split(',');
                        if (sApp.ToList<string>().Find(p => p.Equals(sJobAttributeValue)) != null)
                        {
                            bCompareMatchFound = true;
                        }
                        else
                        {
                            bCompareMatchFound = false;
                        }
                    }
                    else
                    {
                        bCompareMatchFound = m_sCompareValue.Equals(sJobAttributeValue) == true ? true : false;
                    }
                }

                if (m_iRestrictOnMatch == 1)
                {
                    if (bCompareMatchFound) bEditControl = false;
                }
                else
                {
                    if (!bCompareMatchFound) bEditControl = false;
                }

                return bEditControl;

            }
            catch (Exception ex)
            {
                throw new Exception("There is an error in \"Restrict By Job Attribute\" Attribute Edit Control Interface \n" + ex.Message);
            }
        }

    }
}
