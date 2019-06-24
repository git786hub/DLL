//----------------------------------------------------------------------------+
//        Class: RGIBaseClass
//  Description: Base class for any IGTRelationshipGeometry.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                   $
//       $Date:: 03/05/19                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: RGIBaseClass.cs                     $
// 
// *****************  Version 1  *****************
// User: pnlella     Date: 08/06/19   Time: 11:00  Desc : Created
//----------------------------------------------------------------------------+
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class RGIBaseClass : IGTRelationshipGeometry
    {
        #region Private Members

        IGTKeyObject m_oKeyActiveFeature;
        IGTKeyObjects m_oKeyRelatedFeatures;
        GTArguments m_GTArguments;
        IGTDataContext m_GTDataContext;
        GTRelationshipGeometryProcessingModeConstants m_gProcessingMode;
        short m_iRno, m_iRelatedFNo, m_iRelatedCNo;

        #endregion

        #region IGTRelationshipGeometry Members

        public IGTKeyObject ActiveFeature
        {
            get
            {
                return m_oKeyActiveFeature;
            }
            set
            {
                m_oKeyActiveFeature = value;
            }
        }

        public IGTKeyObjects RelatedFeatures
        {
            get
            {
                return m_oKeyRelatedFeatures;
            }
            set
            {
                m_oKeyRelatedFeatures = value;
            }
        }

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

        public IGTDataContext DataContext
        {
            get
            {
                return m_GTDataContext;
            }
            set
            {
                m_GTDataContext = value;
            }
        }

        public GTRelationshipGeometryProcessingModeConstants ProcessingMode
        {
            get
            {
                return m_gProcessingMode;
            }
            set
            {
                m_gProcessingMode = value;
            }
        }

        public short RNO
        {
            get
            {
                return m_iRno;
            }
            set
            {
                m_iRno = value;
            }
        }

        public short RelatedCNO
        {
            get
            {
                return m_iRelatedCNo;
            }
            set
            {
                m_iRelatedCNo = value;
            }
        }

        public short RelatedFNO
        {
            get
            {
                return m_iRelatedFNo;
            }
            set
            {
                m_iRelatedFNo = value;
            }
        }

        public bool AdjustGeometry(IGTGeometry BeforeGeometry)
        {
            return true;
        }

        public void AfterDelete()
        {

        }

        public void AfterEstablish()
        {
            string rgiExecuted = null;
            try
            {
                if (!(ActiveFeature.FNO == 16 || ActiveFeature.FNO == 91))
                {
                    Common oCommon = new Common(this);
                    oCommon.SetValues();

                    rgiExecuted = "Common RGI:Voltage Agreement";
                    rgiVoltageAgreement oVoltageAgreement = new rgiVoltageAgreement(this, oCommon);
                    oVoltageAgreement.ProcessValidationEstablish();

                    rgiExecuted = "Common RGI:Feeder Agreement";
                    rgiFeederAgreement oFeederAgreement = new rgiFeederAgreement(this, oCommon);
                    oFeederAgreement.ProcessValidationEstablish();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error in " + rgiExecuted + "- AfterEstablish  \n" + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AfterUpdate()
        {

        }

        public bool AllowDelete(GTRelationshipGeometryDeleteActionConstants DeleteAction, IGTKeyObject RelatedFeature)
        {
            return true;
        }

        public bool AllowMerge(IGTKeyObject MergeCandidate, IGTKeyObjects MergeCandidateRelatedFeatures, out string[] MessageArray)
        {
            MessageArray = new string[0];
            return true;
        }

        public bool ApproveMerge(IGTKeyObject MergeCandidate, IGTKeyObjects MergeCandidateRelatedFeatures, out string[] MessageArray)
        {
            MessageArray = new string[0];
            return true;
        }

        public void BeforeDelete(GTRelationshipGeometryDeleteActionConstants DeleteAction, IGTKeyObject RelatedFeature)
        {

        }

        public void BeforeEstablish(IGTKeyObject EstablishToFeature, bool ProcessingBreak)
        {

        }

        public void BeforeUpdate()
        {

        }

        public void FilterCandidates(bool CanBreak, IGTKeyObjects Candidates)
        {

        }

        public void FilterLocatedCandidates(bool CanBreak, IGTDDCKeyObjects Candidates, IGTPoint gTPoint, IGTMapWindow gTMapWindow)
        {

        }

        public void Initialize()
        {

        }

        public virtual void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
            string rgiExecuted = null;
            try
            {
                List<string> m_lstErrorMessage = new List<string>();
                List<string> m_lstErrorPriority = new List<string>();

                Common oCommon = new Common(this);
                oCommon.SetValues();

                rgiExecuted = "Common RGI:Voltage Agreement";
                rgiVoltageAgreement oVoltageAgreement = new rgiVoltageAgreement(this, oCommon);
                oVoltageAgreement.Validate(out ErrorPriorityArray, out ErrorMessageArray);

                if (ErrorPriorityArray != null && ErrorMessageArray != null)
                {
                    m_lstErrorPriority.AddRange(ErrorPriorityArray);
                    m_lstErrorMessage.AddRange(ErrorMessageArray);
                }

                rgiExecuted = "Common RGI:Feeder Agreement";

                rgiFeederAgreement oFeederAgreement = new rgiFeederAgreement(this, oCommon);
                oFeederAgreement.Validate(out ErrorPriorityArray, out ErrorMessageArray);

                if (ErrorPriorityArray != null && ErrorMessageArray != null)
                {
                    m_lstErrorPriority.AddRange(ErrorPriorityArray);
                    m_lstErrorMessage.AddRange(ErrorMessageArray);
                }

                ErrorPriorityArray = m_lstErrorPriority.ToArray();
                ErrorMessageArray = m_lstErrorMessage.ToArray();
            }
            catch(Exception ex)
            {
                throw new Exception("Error in "+rgiExecuted+"-Validate: " + ex.Message);
            }

        }

        #endregion
    }
}
