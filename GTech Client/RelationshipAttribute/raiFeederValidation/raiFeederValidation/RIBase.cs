// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: FIBase.cs
// 
//  Description: Base class for any IGTRelationshipAttribute.
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  30/04/2018          Sithara                  
// ======================================================
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
    public abstract class RIBase : IGTRelationshipAttribute
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

        public virtual void AfterEstablish()
        {

        }

        public virtual void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
        }
    }
}
