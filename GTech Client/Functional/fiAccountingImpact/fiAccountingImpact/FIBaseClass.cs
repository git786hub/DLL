// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: ICCFIBase.cs
// 
//  Description: Base class for any FI.
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  01/02/2018         Sithara                  
// ======================================================
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
    public abstract class FIBaseClass : IGTFunctional
    {
        #region IGTFunctional Members

        private GTArguments _arguments;
        public virtual GTArguments Arguments
        {
            get { return _arguments; }
            set { _arguments = value; }
        }

        private string _componentName;
        public virtual string ComponentName
        {
            get { return _componentName; }
            set
            {
                _componentName = value;
            }
        }

        private IGTComponents _components;
        public virtual IGTComponents Components
        {
            get { return _components; }
            set
            {
                _components = value;
            }
        }

        private IGTDataContext _dataContext;
        public virtual IGTDataContext DataContext
        {
            get { return _dataContext; }
            set { _dataContext = value; }
        }
        private IGTComponent _activeComponent;
        public virtual IGTComponent ActiveComponent
        {
            get { return _activeComponent; }
            set { _activeComponent = value; }
        }

        private string _fieldName;
        public virtual string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        private IGTFieldValue _fieldValueBeforeChange;
        public virtual IGTFieldValue FieldValueBeforeChange
        {
            get { return _fieldValueBeforeChange; }
            set { _fieldValueBeforeChange = value; }
        }

        private GTFunctionalTypeConstants _type;
        public virtual GTFunctionalTypeConstants Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public virtual void Delete() { }

        public virtual void Execute() { }

        public virtual void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
        }

        #endregion

        public IGTComponent GetActiveComponent()
        {

            foreach (IGTComponent component in _components)
            {
                if (component.Name == ComponentName)
                {
                    ActiveComponent = component;
                    break;
                }
            }
            return ActiveComponent;
        }

        public IGTKeyObject ActiveKeyObject
        {
            get
            {
                IGTApplication _gtApp = (IGTApplication)GTClassFactory.Create<IGTApplication>();

                IGTComponent comp = GetActiveComponent();

                if (comp != null)
                {
                    short FNO = short.Parse(comp.Recordset.Fields["G3E_FNO"].Value.ToString());
                    int FID = int.Parse(comp.Recordset.Fields["G3E_FID"].Value.ToString());
                    return _gtApp.DataContext.OpenFeature(FNO, FID);
                }
                return null;
            }
        }



    }
}
