// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: raiFeederValidation.cs
// 
//  Description:This interface sets configured attribute to indicate the feature is owned by a switch gear, so that smaller symbols are used.
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  26/04/2018          Sithara                  
//  28/05/2018          Prathyusha                  Modified the code as per the new Requirements mentioned in JIRA-1610.
// ======================================================

using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;


namespace GTechnology.Oncor.CustomAPI
{
    public class raiSwitchgearOwned : IGTRelationshipAttribute
    {
        private int _ActiveANO;
        public int ActiveANO
        {
            get { return _ActiveANO; }
            set { _ActiveANO = value; }
        }

        private string _ActiveComponentName;
        public string ActiveComponentName
        {
            get { return _ActiveComponentName; }
            set { _ActiveComponentName = value; }
        }

        private IGTComponents _ActiveComponents;
        public IGTComponents ActiveComponents
        {
            get { return _ActiveComponents; }
            set { _ActiveComponents = value; }
        }

        private string _ActiveFieldName;
        public string ActiveFieldName
        {
            get { return _ActiveFieldName; }
            set { _ActiveFieldName = value; }
        }

        private IGTFieldValue _ActiveFieldValue;
        public IGTFieldValue ActiveFieldValue
        {
            get { return _ActiveFieldValue; }
            set { _ActiveFieldValue = value; }
        }

        private GTArguments _Arguments;
        public GTArguments Arguments
        {
            get { return _Arguments; }
            set { _Arguments = value; }
        }

        private IGTDataContext _DataContext;
        public IGTDataContext DataContext
        {
            get { return _DataContext; }
            set { _DataContext = value; }
        }

        private string _Priority;
        public string Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }

        private int _RelatedANO;
        public int RelatedANO
        {
            get { return _RelatedANO; }
            set { _RelatedANO = value; }
        }
        private string _RelatedComponentName;
        public string RelatedComponentName
        {
            get { return _RelatedComponentName; }
            set { _RelatedComponentName = value; }
        }

        private IGTComponents _RelatedComponents;
        public IGTComponents RelatedComponents
        {
            get { return _RelatedComponents; }
            set { _RelatedComponents = value; }
        }

        private string _RelatedFieldName;
        public string RelatedFieldName
        {
            get { return _RelatedFieldName; }
            set { _RelatedFieldName = value; }
        }

        private IGTFieldValue _RelatedFieldValue;
        public IGTFieldValue RelatedFieldValue
        {
            get { return _RelatedFieldValue; }
            set { _RelatedFieldValue = value; }
        }
        public void AfterEstablish()
        {
            try
            {

                if (ActiveComponents[ActiveComponentName] != null && ActiveComponents[ActiveComponentName].Recordset != null &&
                    ActiveComponents[ActiveComponentName].Recordset.RecordCount > 0)
                {
                    ActiveComponents[ActiveComponentName].Recordset.MoveFirst();
                    if ((Convert.ToInt32(RelatedFieldValue.FieldValue) == 19) || (Convert.ToInt32(RelatedFieldValue.FieldValue) == 153))
                    {
                        ActiveComponents[ActiveComponentName].Recordset.Fields[ActiveFieldName].Value = "Y";
                        ActiveComponents[ActiveComponentName].Recordset.Update();
                    }
                    else
                    {
                        ActiveComponents[ActiveComponentName].Recordset.Fields[ActiveFieldName].Value = "N";
                        ActiveComponents[ActiveComponentName].Recordset.Update();
                    }
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error in Switchgear Owned Relationship Attribute Interface \n" + ex.Message, "G/Technology");
            }
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
        }
    }
}
