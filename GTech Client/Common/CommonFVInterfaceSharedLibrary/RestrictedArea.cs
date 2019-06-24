// =====================================================================================================================================================================
//  File Name: RestrictedArea.cs
// 
// Description:   

// During placement or edit of a feature, check whether the primary graphic component falls within a Restricted Area boundary. 
//If so, prevent the action if the active user does not have the specified Authorized Role and the active WR does not appear in the specified list of Authorized WRs.

//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  03/09/2018          Hari                       Implemented the workflow per the description in Business rules DDD  section 3.5.2.a
// =====================================================================================================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class RestrictedArea : IProcessFV
    {
        short m_fno;
        int m_fid;
        IGTDataContext m_iGtDataContext;
        IGTKeyObjects m_features;
        Recordset m_validationErrors;
        private const short RestrictedAreaFno = 238;
        private const short RestrictedAreaAttributesCno = 23801;

        public IGTDataContext DataContext
        {
            get { return m_iGtDataContext; }
            set { m_iGtDataContext = value; }
        }

        public IGTKeyObjects Features
        {
            get { return m_features; }
            set { m_features = value; }
        }

        public Recordset ValidationErrors
        {
            get { return m_validationErrors; }
            set { m_validationErrors = value; }
        }

        //public RestrictedArea(short Fno, int Fid)
        //{
        //    m_fno = Fno;
        //    m_fid = Fid;
        //    m_iGtApplication = GTClassFactory.Create<IGTApplication>();
        //    m_iGtDataContext = m_iGtApplication.DataContext;
        //}

        /// <summary>
        /// Process Restricted Area validation
        /// </summary>
        /// <returns>true if the process is complete without errors, else returns false</returns>
        public bool Process()
        {
            IGTGeometry featureGeometry = null;
            IGTSpatialService gTSpatialService = null;
            IGTComponent restrictedAreaAttributes = null;

            try
            {

                foreach (IGTKeyObject feature in m_features)
                {
                    m_fno = feature.FNO;
                    m_fid = feature.FID;

                    IGTComponents selectedfeatureComponents = m_iGtDataContext.OpenFeature(m_fno, m_fid).Components;

                    CommonFunctions commonFunctions = new CommonFunctions();
                    short primaryCno = commonFunctions.GetPrimaryGraphicCno(m_fno, true);

                    // If the CNO is zero (not defined), then try the detail CNO
                    if (0 == primaryCno)
                    {
                        primaryCno = commonFunctions.GetPrimaryGraphicCno(m_fno, false);
                    }
                    
                    IGTComponent primaryGraphicComponent = selectedfeatureComponents.GetComponent(primaryCno);

                    if (primaryGraphicComponent != null)
                    {
                        Recordset graphicCompRs = primaryGraphicComponent.Recordset;
                        if (graphicCompRs != null && graphicCompRs.RecordCount > 0)
                        {
                            graphicCompRs.MoveFirst();
                            //  Retrieve the geometry of the primary graphic component
                            featureGeometry = primaryGraphicComponent.Geometry;

                            // Find any Restricted Area features whose area component overlaps the retrieved geometry; if none are found, return with a successful status.
                            gTSpatialService = GTClassFactory.Create<IGTSpatialService>();
                            gTSpatialService.DataContext = m_iGtDataContext;
                            gTSpatialService.Operator = GTSpatialOperatorConstants.gtsoEntirelyContainedBy;
                            gTSpatialService.FilterGeometry = featureGeometry;

                            Recordset restrictedAreaFeaturesRs = gTSpatialService.GetResultsByFNO(new short[] { RestrictedAreaFno });

                            if (restrictedAreaFeaturesRs != null && restrictedAreaFeaturesRs.RecordCount>0)
                            {
                                //Prathyusha
                                //if (restrictedAreaFeaturesRs.RecordCount == 0)
                                //{
                                //    return false;
                                //}
                                restrictedAreaFeaturesRs.MoveFirst();

                                while (!restrictedAreaFeaturesRs.EOF)
                                {
                                    int fid = Convert.ToInt32(restrictedAreaFeaturesRs.Fields["G3E_FID"].Value);
                                    restrictedAreaAttributes = m_iGtDataContext.OpenFeature(RestrictedAreaFno, fid).Components.GetComponent(RestrictedAreaAttributesCno);

                                    if (!ValidateRestrictedAreaFeature(restrictedAreaAttributes))
                                    {
                                        return false;
                                    }

                                    restrictedAreaFeaturesRs.MoveNext();
                                }
                                return true;
                            }
                        }
                    }
                }
                return true;// Prathyusha: return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool ValidateRestrictedAreaFeature(IGTComponent restrictedAreaAttributes)
        {
            Recordset restrictedAreaAttributesRs = null;
            string authorizedRole = string.Empty;
            string authorizedWrs = string.Empty;
            List<string> authorizedWrList = null;
            try
            {
                if (restrictedAreaAttributes != null)
                {
                    restrictedAreaAttributesRs = restrictedAreaAttributes.Recordset;

                    if (restrictedAreaAttributesRs != null && restrictedAreaAttributesRs.RecordCount > 0)
                    {
                        restrictedAreaAttributesRs.MoveFirst();
                        authorizedRole = Convert.ToString(restrictedAreaAttributesRs.Fields["AUTH_ROLE"].Value);
                        authorizedWrs = Convert.ToString(restrictedAreaAttributesRs.Fields["AUTH_WR_LIST"].Value);
                        if (authorizedWrs != null)
                        {
                            authorizedWrList = authorizedWrs.Split(',').ToList();
                            return CheckPermissions(authorizedRole, authorizedWrList);
                        }
                    }
                }
                return true;//Prathyusha: return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CheckPermissions(string authorizedRole, List<string> authorizedWrList)
        {
            try
            {
				//Prathyusha: Modified the condition from && to || as per document.
                if (m_iGtDataContext.IsRoleGranted(authorizedRole) || authorizedWrList.Contains(m_iGtDataContext.ActiveJob))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
