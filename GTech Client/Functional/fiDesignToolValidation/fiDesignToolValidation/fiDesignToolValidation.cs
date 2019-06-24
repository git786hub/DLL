using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiDesignToolValidation : IGTFunctional
    {
        #region Private Variables
        private GTArguments m_GTArguments = null;
        private string m_componentName = string.Empty;
        private IGTComponents m_components = null;
        private IGTDataContext m_dataContext = null;
        private string m_fieldName = string.Empty;
        private IGTFieldValue m_fieldValueBeforeChange = null;
        private GTFunctionalTypeConstants m_type;

        #endregion

        #region Properities
        public GTArguments Arguments
        {
            get
            {
                return m_GTArguments;
            }

            set
            {
                m_GTArguments = value;
            }
        }

        public string ComponentName
        {
            get
            {
                return m_componentName;
            }

            set
            {
                m_componentName = value;
            }
        }

        public IGTComponents Components
        {
            get
            {
                return m_components;
            }

            set
            {
                m_components = value;
            }
        }

        public IGTDataContext DataContext
        {
            get
            {
                return m_dataContext;
            }

            set
            {
                m_dataContext = value;
            }
        }

        public string FieldName
        {
            get
            {
                return m_fieldName;
            }

            set
            {
                m_fieldName = value;
            }
        }

        public IGTFieldValue FieldValueBeforeChange
        {
            get
            {
                return m_fieldValueBeforeChange;
            }

            set
            {
                m_fieldValueBeforeChange = value;
            }
        }

        public GTFunctionalTypeConstants Type
        {
            get
            {
                return m_type;
            }

            set
            {
                m_type = value;
            }
        }

        #endregion

        public void Delete()
        {
            return;
        }

        public void Execute()
        {
            return;
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
        }
    }
}
