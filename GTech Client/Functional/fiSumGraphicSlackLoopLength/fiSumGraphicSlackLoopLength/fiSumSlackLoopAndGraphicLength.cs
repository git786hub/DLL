// =======================================================================================================
//  File Name: fiSumSlackLoopAndGraphicLength.cs
// 
//  Description:  fi sum slack loop length and graphic length and update to actual length
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  30/03/2018          Pramod                      Implemented changes in Execute and delete method.                   
// ========================================================================================================

using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiSumSlackLoopAndGraphicLength : IGTFunctional
    {

        private GTArguments _gtArguments = null;
        private IGTDataContext _gtDataContext = null;
        private IGTComponents _gtComponents;
        private string _componentName;
        private string _fieldName;
        IGTApplication _gtApp;

        public GTArguments Arguments { get => _gtArguments; set => _gtArguments = value; }
        public string ComponentName { get => _componentName; set => _componentName = value;}
        public IGTComponents Components { get => _gtComponents; set => _gtComponents = value; }
        public IGTDataContext DataContext { get => _gtDataContext; set =>_gtDataContext=value; }
        public string FieldName { get => _fieldName; set => _fieldName = value; }
        public IGTFieldValue FieldValueBeforeChange { get; set; }
        public GTFunctionalTypeConstants Type { get; set; }

        public void Delete()
        {
            Recordset rs = null;
            try
            {
                var lengthFldName = _gtArguments.GetArgument(1).ToString();
                    rs = Components[_gtArguments.GetArgument(0).ToString()].Recordset;
                    if (rs != null)
                    {
                        rs.Fields[lengthFldName].Value =  Convert.ToInt32(rs.Fields[lengthFldName].Value)- GetSlackLoopLength(ComponentName, FieldName);
                        rs.Update();
                    }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in fiSumGraphicSlackLoopLength " + ex.Message);
            }
        }

        public void Execute()
        {
            Recordset rs = null;
            try
            {
              
                var lengthFldName = _gtArguments.GetArgument(1).ToString();
                rs = Components[_gtArguments.GetArgument(0).ToString()].Recordset;
                if (rs!=null)
                    {
                        rs.Fields[lengthFldName].Value =GetSlackLoopLength(ComponentName,FieldName) + Convert.ToInt32(rs.Fields[lengthFldName].Value);
                        rs.Update();
                    }
            }catch(Exception ex)
            {
                throw new Exception("Error in fiSumGraphicSlackLoopLength " + ex.Message);
            }
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;

        }

        /// <summary>
        /// sum slack loop length
        /// </summary>
        /// <param name="cName"></param>
        /// <param name="fName"></param>
        /// <returns></returns>
        private int GetSlackLoopLength(string cName,string fName)
        {
            int slackLength = 0;
            Recordset rs = Components[ComponentName].Recordset;
            if (rs != null && rs.RecordCount > 0)
            {
                slackLength = rs.Fields[FieldName].Value is DBNull ? 0: Convert.ToInt32(rs.Fields[FieldName].Value);
            }
            return slackLength;

        }
        
    }
}
