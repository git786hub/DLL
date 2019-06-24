//----------------------------------------------------------------------------+
//        Class: fkqVirtualPointAssociation
//  Description: This FKQ associate a manually-placed virtual point feature with an actual feature.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                                     $
//       $Date:: 15/04/2019                                                    $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fkqVirtualPointAssociation.cs                                 $
//----------------------------------------------------------------------------+
using System;
using ADODB;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace GTechnology.Oncor.CustomAPI
{
    public class fkqVirtualPointAssociation : IGTForeignKeyQuery
    {
        #region Private Members

        IGTKeyObject m_gtFeature;
        GTArguments m_gtArguments;
        IGTDataContext m_gtDataContext;
        IGTFieldValue m_gtOutputValue;
        string m_gtFieldName;
        string m_gtTableName;
        bool m_gtReadOnly;
        IGTApplication m_gtApplication;

        Dictionary<short, string> m_relatedFeatures;
        #endregion

        #region IGTForeignKeyQuery Members
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

        public IGTKeyObject Feature
        {
            get
            {
                return m_gtFeature;
            }
            set
            {
                m_gtFeature = value;
            }
        }

        public string FieldName
        {
            get
            {
                return m_gtFieldName;
            }
            set
            {
                m_gtFieldName = value;
            }
        }

        public IGTFieldValue OutputValue
        {
            get
            {
                return m_gtOutputValue;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return m_gtReadOnly;
            }
            set
            {
                m_gtReadOnly = value;
            }
        }

        public string TableName
        {
            get
            {
                return m_gtTableName;
            }
            set
            {
                m_gtTableName = value;
            }
        }
        public bool Execute(object InputValue)
        {
            Boolean selection = false;
           
            DataTable associatedFeaturesRs = null;
            try
            {
                m_gtApplication = GTClassFactory.Create<IGTApplication>();
                m_gtOutputValue = GTClassFactory.Create<IGTFieldValue>();

                if (ProcessVirtualPointAssociation(ref associatedFeaturesRs))
                {                   
                    DisplayFormWithAssociatedFeatures(associatedFeaturesRs, m_gtApplication);
                }

                selection = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is an error in \"Virtual Point Association\" Foreign Key Query Interface \n" + ex.Message, "G/Technology");
            }
            finally
            {

            }
            return selection;
        }

        #endregion

        #region Methods 

        private bool ProcessVirtualPointAssociation(ref DataTable p_associatedFeaturesRs)
        {
            IGTGeometry primaryGeometry = null;
            IGTGeometry tempGeometry = null;
            double locateRange = 0.0;
            bool process = false;
            try
            {
                IGTKeyObject virtualPtFeature = DataContext.OpenFeature(m_gtFeature.FNO, m_gtFeature.FID);
                m_relatedFeatures = new Dictionary<short, string>();
                DataAccess dataAccess = new DataAccess(m_gtApplication);

                dataAccess.GetPrimaryGeometry(virtualPtFeature, ref primaryGeometry);

                if (primaryGeometry != null)
                {
                    process = true;

                    dataAccess.GetRangeValueToLocateAssociateFeatures(ref locateRange);
                    dataAccess.GetListOfRelatedFeaturesOfVP(m_gtFeature.FNO, ref m_relatedFeatures);

                    VirtualPointAssociationUtility associationUtility = new VirtualPointAssociationUtility(m_gtApplication);
                    associationUtility.CreateTemporaryGeometryForVirtualPoint(primaryGeometry, locateRange, ref tempGeometry);
                    associationUtility.GetRecorsetOfAssociateFeaturesForVP(tempGeometry, m_relatedFeatures.Keys.ToArray(), ref p_associatedFeaturesRs, m_gtFeature.FNO, m_relatedFeatures);
                }
            }
            catch
            {
                throw;
            }
            return process;
        }
        private void DisplayFormWithAssociatedFeatures(DataTable p_associatedFeaturesRs, IGTApplication p_gtApplication)
        {
            try
            {
                using (VirtualPointAssociatedFeaturesForm associatedFeaturesForm = new VirtualPointAssociatedFeaturesForm(p_associatedFeaturesRs, m_gtReadOnly, p_gtApplication,m_relatedFeatures,m_gtFeature.FID))
                {
                    associatedFeaturesForm.StartPosition = FormStartPosition.CenterScreen;
                    associatedFeaturesForm.ShowDialog(p_gtApplication.ApplicationWindow);

                    if (associatedFeaturesForm.AssociatedFID != 0 && !m_gtReadOnly)
                    {
                        m_gtOutputValue.FieldValue = associatedFeaturesForm.AssociatedFID;
                    }

                }
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
