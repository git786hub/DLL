
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  19/04/2018          Pramod                     this is Shared component invoked by Custom commands to add Justiifcation Comments to Job Valdiation Errors 
// ======================================================


using ADODB;
using GTechnology.Oncor.CustomAPI.DataAccess;
using GTechnology.Oncor.CustomAPI.GUI;
using GTechnology.Oncor.CustomAPI.Model;
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class ValidationOverrides : IDisposable
    {
        ValidationRuleOverrides validationRuleOvr = null;
        List<WRValidationOverride> wrValidationOverrides = null;
        List<WRValidationRule> wrValidationRules = null;
        List<KeyValuePair<int, string>> FeatureClass = null;
        IGTApplication gtApp;


        public ValidationOverrides()
        {
            gtApp = GTClassFactory.Create<IGTApplication>();
            wrValidationOverrides = GetValidationOverrides(gtApp.DataContext.ActiveJob);
            wrValidationRules = GetValidationRules();
            FeatureClass = GetFeatureClass();
        }

        /// <summary>
        /// Show Dialog containing Job Validation Errors
        /// return recordset with user added justification comments for each error  
        /// </summary>
        /// <param name="rsValidation"></param>
        /// <returns></returns>
        public Recordset ShowValidationComments(Recordset rsValidation)
        {
            List<WRValidationOverride> wrValidationUsrComments = null;

            Recordset rsJobValidation = CopyStructure(rsValidation);
            Recordset rsJobValidationErrs = CloneValidationRS(rsValidation);
            wrValidationUsrComments = FormatValidationErrors(rsJobValidationErrs);

            validationRuleOvr = new ValidationRuleOverrides(wrValidationUsrComments);
            if (validationRuleOvr.ShowDialog(gtApp.ApplicationWindow) == DialogResult.Cancel)
            {
                rsJobValidation = null;
            }
            else
            {
                wrValidationUsrComments = validationRuleOvr.WRValidationComments;
                rsJobValidationErrs.MoveFirst();
                rsJobValidation.Open();
                //Update Job Validation Recordset with justification comments
                while (!rsJobValidationErrs.EOF)
                {
                    rsJobValidation.AddNew();
                    foreach (Field fld in rsJobValidation.Fields)
                    {
                        rsJobValidation.Fields[fld.Name].Value = rsJobValidationErrs.Fields[fld.Name].Value;
                    }
                    rsJobValidation.Fields["OVERRIDE_COMMENTS"].Value = wrValidationUsrComments.FirstOrDefault(a => a.ID == Convert.ToInt32(rsJobValidationErrs.Fields["ID"].Value)).Override_Comments;
                    rsJobValidationErrs.MoveNext();
                }
            }

            return rsJobValidation;
        }

        #region Private Methods

        /// <summary>
        /// this method Clone JOB Validation Errors recordset structure  
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        private Recordset CopyStructure(Recordset rs)
        {
            Recordset rsValidation = new Recordset();
            rsValidation.CursorLocation = CursorLocationEnum.adUseClient;
            rs.MoveFirst();

            foreach (Field fld in rs.Fields)
            {
                rsValidation.Fields.Append(fld.Name, fld.Type, fld.DefinedSize, (FieldAttributeEnum)fld.Attributes, null);
            }
            rsValidation.Fields.Append("OVERRIDE_COMMENTS", DataTypeEnum.adVarChar, 400, FieldAttributeEnum.adFldUpdatable);
            return rsValidation;
        }

        /// <summary>
        /// Clone Job Validation Error Recordset
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        private Recordset CloneValidationRS(Recordset rs)
        {
            int cnt = 1;
            Recordset rsValidation = new Recordset();
            rsValidation.CursorLocation = CursorLocationEnum.adUseClient;
            rs.MoveFirst();

            foreach (Field fld in rs.Fields)
            {
                rsValidation.Fields.Append(fld.Name, fld.Type, fld.DefinedSize, (FieldAttributeEnum)fld.Attributes, null);
            }
            // Add column ID to recordset to access each row in record set with  primary key
            rsValidation.Fields.Append("ID", DataTypeEnum.adInteger, 10, FieldAttributeEnum.adFldUpdatable);
            rsValidation.Fields.Append("OVERRIDE_COMMENTS", DataTypeEnum.adVarChar, 400, FieldAttributeEnum.adFldUpdatable);
            rsValidation.Open();
            while (!rs.EOF)
            {
                rsValidation.AddNew();
                foreach (Field fld in rs.Fields)
                {
                    rsValidation.Fields[fld.Name].Value = fld.Value;
                }

                rsValidation.Fields["ID"].Value = cnt;
                cnt += 1;
                rs.MoveNext();
            }

            return rsValidation;

        }


        /// <summary>
        /// this method foramt JOB Validation Errors to display in DataGrid
        /// </summary>
        /// <param name="rsJobValidationErrs"></param>
        /// <returns></returns>
        private List<WRValidationOverride> FormatValidationErrors(Recordset rsJobValidationErrs)
        {
            List<WRValidationOverride> wrValidationUsrComments = null;
            WRValidationOverride validationOverride = null;
            if (rsJobValidationErrs != null && rsJobValidationErrs.RecordCount > 0)
            {
                wrValidationUsrComments = new List<WRValidationOverride>();
                rsJobValidationErrs.MoveFirst();
                while (!rsJobValidationErrs.EOF)
                {
                    validationOverride = new WRValidationOverride();
                    validationOverride.ID = Convert.ToInt32(rsJobValidationErrs.Fields["ID"].Value);
                    validationOverride.G3e_Fid = Convert.ToInt32(rsJobValidationErrs.Fields["g3e_fid"].Value);
                    validationOverride.G3e_Fno = Convert.ToInt32(rsJobValidationErrs.Fields["g3e_fno"].Value);
                    //Get Feature Class name 
                    validationOverride.FeatureClass = FeatureClass.FirstOrDefault(a => a.Key == validationOverride.G3e_Fno).Value;

                    string errMsg = Convert.ToString(rsJobValidationErrs.Fields["ErrorDescription"].Value);
                    //check Justifcation comments exists for error_message 
                    var ovrComments = wrValidationOverrides.FirstOrDefault(a => a.G3e_Fid == validationOverride.G3e_Fid && a.G3e_Fno == validationOverride.G3e_Fno && (a.Error_Msg != "" && a.Error_Msg == errMsg));
                    if (ovrComments != null)
                    {
                        validationOverride.Override_Comments = ovrComments.Override_Comments;
                    }
                    //if error message starts with [ then parse ID between [] and get rule_msg from Validation Rule
                    if (errMsg.StartsWith("["))
                    {
                        var rule = wrValidationRules.FirstOrDefault(a => a.Rule_ID == errMsg.Substring(1, errMsg.IndexOf("]") - 1));
                        if (rule != null)
                        {
                            errMsg = rule.Rule_Msg;
                        }
                    }
                    validationOverride.Error_Msg = errMsg;

                    wrValidationUsrComments.Add(validationOverride);
                    rsJobValidationErrs.MoveNext();
                }
            }
            return wrValidationUsrComments;
        }

        /// <summary>
        /// Get all Validation Override Justification Comments for given Active Job Id
        /// </summary>
        /// <param name="jobID"></param>
        /// <returns></returns>
        private List<WRValidationOverride> GetValidationOverrides(string jobID)
        {
            string sqlStmt = "Select  ID ,RULE_ID,RULE_NM,G3E_IDENTIFIER,G3E_FNO,G3E_FID,ERROR_MSG,OVERRIDE_COMMENTS from WR_VALIDATION_OVERRIDE " +
                " where G3E_IDENTIFIER='{0}'";
            return CommonUtil.Execute<WRValidationOverride>(string.Format(sqlStmt, jobID));
        }

        /// <summary>
        /// Get all Validation Rules
        /// </summary>
        /// <returns></returns>
        private List<WRValidationRule> GetValidationRules()
        {
            string sqlStmt = "Select  RULE_ID,RULE_NM,RULE_MSG from WR_VALIDATION_RULE";
            return CommonUtil.Execute<WRValidationRule>(sqlStmt);
        }

        /// <summary>
        /// Return list contains Feature nane and fno
        /// </summary>
        /// <returns></returns>
        private List<KeyValuePair<int, string>> GetFeatureClass()
        {
            string sqlStmt = "Select  G3E_FNO AS kEY,G3E_USERNAME AS VALUE from G3E_FEATURES_OPTABLE ORDER BY G3E_FNO";
            return CommonUtil.ConvertRSToKeyValue<int, string>(CommonUtil.Execute(sqlStmt));
        }

        public void Dispose()
        {
            if (validationRuleOvr != null)
            {
                validationRuleOvr.Dispose();
                validationRuleOvr = null;
            }
            wrValidationOverrides = null;
            wrValidationRules = null;
            FeatureClass = null;
        }
        #endregion

    }
}
