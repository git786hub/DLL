//----------------------------------------------------------------------------+
//        Class: RIBase
//  Description: Base class for any IGTRelationshipGeometry.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                   $
//       $Date:: 08/06/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: RIBase.cs                     $
// 
// *****************  Version 1  *****************
// User: pnlella     Date: 08/06/18   Time: 11:00  Desc : Created
//----------------------------------------------------------------------------+
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
    public abstract class RIBase : IGTRelationshipGeometry
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

        public virtual void AfterEstablish()
        {
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
        }

        #endregion
    }
}
