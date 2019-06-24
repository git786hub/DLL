using System;
using System.Collections.Generic;
using System.Text;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using ADODB;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    class CommonRGI : IGTRelationshipGeometry
    {
        #region IGTRelationshipGeometry Members
        IGTApplication gtApplication = GTClassFactory.Create<IGTApplication>();
        private short m_relatedFNO;
        private short m_relatedCNO;
        public short m_RNO;
        private IGTKeyObject m_activeFeature;
        private IGTKeyObjects m_relatedFeatures;
        private IGTDataContext m_dataContext;
        private GTArguments m_argumentArray;

        public const int Bypass_FNO = 40;
        public const int RecloserOH_FNO = 14;
        public const int RecloserUG_FNO = 15;
        public const int VoltageRegulator_FNO = 36;
        public const int Autotransformer_FNO = 34;
        public const int PrimaryEnclosure_FNO = 5;
        public const int TransformerUG_FNO = 60;
        public const int TransformerOH_FNO = 59;
        public const int TransformerOHN_FNO = 98;
        public const int TransformerUGN_FNO = 99;
        public const int Capacitor_FNO = 4;
        public const short ConnectivityG3eCno = 11;

        public IGTKeyObject ActiveFeature
        {
            get
            {
                return m_activeFeature;
            }
            set
            {
                m_activeFeature = value;
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
            MessageArray = null;
            return true;
        }

        public bool ApproveMerge(IGTKeyObject MergeCandidate, IGTKeyObjects MergeCandidateRelatedFeatures, out string[] MessageArray)
        {
            MessageArray = null;
            return true;
        }

        public GTArguments Arguments
        {
            get
            {
                return m_argumentArray;
            }
            set
            {
                m_argumentArray = value;
            }
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
        public void FilterCandidates(bool CanBreak, IGTKeyObjects Candidates)
        {
        }
        public void FilterLocatedCandidates(bool CanBreak, IGTDDCKeyObjects Candidates, IGTPoint WorldPoint, IGTMapWindow MapWindow)
        {
            try
            {
                //Added an additional check to see if there are ANY candidate connections to check
                if (ActiveFeature.FNO == Bypass_FNO)
                {
                    if (Candidates.Count > 0)
                    {
                        for (int currentCandidateIdx = 1; currentCandidateIdx >= 0; currentCandidateIdx--)
                        {
                            Candidates.RemoveAt(currentCandidateIdx);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Initialize()
        {
            //ReadArguments();
        }

        public GTRelationshipGeometryProcessingModeConstants ProcessingMode
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public short RNO
        {
            get
            {
                return m_RNO;
            }
            set
            {
                m_RNO = value;
            }
        }

        public short RelatedCNO
        {
            get
            {
                return m_relatedCNO;
            }
            set
            {
                m_relatedCNO = value;
            }
        }

        public short RelatedFNO
        {
            get
            {
                return m_relatedFNO;
            }
            set
            {
                m_relatedFNO = value;
            }
        }

        public IGTKeyObjects RelatedFeatures
        {
            get
            {
                return m_relatedFeatures;
            }
            set
            {
                m_relatedFeatures = value;
            }
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;
        }
        #endregion
    }
}