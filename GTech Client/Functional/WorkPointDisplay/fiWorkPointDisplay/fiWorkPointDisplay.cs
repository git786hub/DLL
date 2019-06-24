//----------------------------------------------------------------------------+
//        Class: fiWorkPointDisplay
//  Description: This FI redisplay Work Points for the active WR.
//----------------------------------------------------------------------------+
//     $Author:: Prathyusha Lella (pnlella)                                                      $
//       $Date:: 6/12/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History::                                         $
// 
// *****************  Version 1  *****************
// User: pnlella     Date: 6/12/17    Time: 15:00
// User: pnlella     Date: 18/01/18    Time: 11:00 Modified the code as per code review comments of Barry.
//----------------------------------------------------------------------------+

using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    /// <summary>
    /// fiWorkPointDisplay
    /// </summary>
    public class fiWorkPointDisplay : IGTFunctional
    {
        #region Private Members
        GTArguments m_gtArguments = null;
        string m_gtComponent = null;
        IGTComponents m_gtComponentCollection = null;
        IGTDataContext m_gtDataContext = null;
        string m_sFieldName = string.Empty;
        IGTFieldValue m_gtFieldValue = null;
        GTFunctionalTypeConstants m_gtFunctionalTypeConstant;

        #endregion

        #region IGTFunctional Members
        /// <summary>
        /// Arguments
        /// </summary>
        public GTArguments Arguments
        {
            get
            {
                return m_gtArguments;
            }

            set
            {
                m_gtArguments = value;
            }
        }
        /// <summary>
        /// ComponentName
        /// </summary>
        public string ComponentName
        {
            get
            {
                return m_gtComponent;
            }

            set
            {
                m_gtComponent = value;
            }
        }
        /// <summary>
        /// Components
        /// </summary>
        public IGTComponents Components
        {
            get
            {
                return m_gtComponentCollection;
            }

            set
            {
                m_gtComponentCollection = value;
            }
        }
        /// <summary>
        /// DataContext
        /// </summary>
        public IGTDataContext DataContext
        {
            get
            {
                return m_gtDataContext;
            }

            set
            {
                m_gtDataContext = value;
            }
        }
        /// <summary>
        /// FieldName
        /// </summary>
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
        /// <summary>
        /// FieldValueBeforeChange
        /// </summary>
        public IGTFieldValue FieldValueBeforeChange
        {
            get
            {
                return m_gtFieldValue;
            }

            set
            {
                m_gtFieldValue = value;
            }
        }
        /// <summary>
        /// Type
        /// </summary>
        public GTFunctionalTypeConstants Type
        {
            get
            {
                return m_gtFunctionalTypeConstant;
            }

            set
            {
                m_gtFunctionalTypeConstant = value;
            }
        }
        /// <summary>
        /// Delete
        /// </summary>
        public void Delete()
        {
            WorkPointDisplay(true);
        }
        /// <summary>
        /// Execute
        /// </summary>
        public void Execute()
        {
            WorkPointDisplay(false);
        }
        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="ErrorPriorityArray"></param>
        /// <param name="ErrorMessageArray"></param>
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
        }
        #endregion

        #region Methods

        /// <summary>
        /// This method runs a query for all Work Point features and attach/detach the results to the display control.
        /// </summary>
        /// <param name="iDelete">This parameter determines whether the current operation is delete or execute</param>
        public void WorkPointDisplay(bool iDelete)
        {
            CommonWorkPointDisplayQuery objcmmnWorkPointDisplayQuery = null;
            IGTApplication gtApplication = null;
            try
            {
                gtApplication = GTClassFactory.Create<IGTApplication>();
                objcmmnWorkPointDisplayQuery = new CommonWorkPointDisplayQuery(gtApplication, Components[ComponentName].Recordset,iDelete);//This Recordset needs to be passed for the Workpoint attribute component that is under transaction and is not yet commited to the DB. In the cases where FI uses this shared component, the Workpoint attribute component recordset that fired the FI needs to be passed.
                objcmmnWorkPointDisplayQuery.RedisplayWorkPoints();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is an error in \"Work Point Display\" Functional Interface \n" + ex.Message, "G/Technology");
            }
            finally
            {
                objcmmnWorkPointDisplayQuery = null;
            }
        }
        #endregion
    }
}
