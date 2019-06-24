using Intergraph.GTechnology.API;
using System;
using System.Windows.Forms;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    class IsoCommon
    {
        public const double AUTO_XFMR_ISO_PT1_OFFSET_X = -3.5;
        public const double AUTO_XFMR_ISO_PT1_OFFSET_Y = -1.3;
        public const double AUTO_XFMR_ISO_PT2_OFFSET_X = 3.5;
        public const double AUTO_XFMR_ISO_PT2_OFFSET_Y = -1.3;

        public const double CAPACITOR_ISO_PT_OFFSET_X = 0;
        public const double CAPACITOR_ISO_PT_OFFSET_Y = -1.2;

        public const double ENCLOSURE_ISO_PT1_OFFSET_X = -3.5;
        //Alm-1725 - Changed Y offset value
        public const double ENCLOSURE_ISO_PT1_OFFSET_Y = -3.5;
        public const double ENCLOSURE_ISO_PT2_OFFSET_X = 3.5;
        //Alm-1725 - Changed Y offset value
        public const double ENCLOSURE_ISO_PT2_OFFSET_Y = -3.5;

        public const double PPOD_ISO_PT_OFFSET_X = 0;
        public const double PPOD_ISO_PT_OFFSET_Y = -1.3;

        public const double RECLOSER_ISO_PT1_OFFSET_X = -12;
        public const double RECLOSER_ISO_PT1_OFFSET_Y = 0;
        public const double RECLOSER_ISO_PT2_OFFSET_X = 12;
        public const double RECLOSER_ISO_PT2_OFFSET_Y = 0;
        public const double RECLOSER_ISO_PT3_OFFSET_X = 0;
        public const double RECLOSER_ISO_PT3_OFFSET_Y = 12;

        public const double XFMR_ISO_PT_OFFSET_X = 0;
        public const double XFMR_ISO_PT_OFFSET_Y = -1.3;

        public const double XFMR_LOOP_ISO_PT1_OFFSET_X = -3.5;
        public const double XFMR_LOOP_ISO_PT1_OFFSET_Y = -1.6;
        public const double XFMR_LOOP_ISO_PT2_OFFSET_X = 3.5;
        public const double XFMR_LOOP_ISO_PT2_OFFSET_Y = -1.6;

        public const double VOLT_REG_ISO_PT1_OFFSET_X = -12;
        public const double VOLT_REG_ISO_PT1_OFFSET_Y = 0;
        public const double VOLT_REG_ISO_PT2_OFFSET_X = 12;
        public const double VOLT_REG_ISO_PT2_OFFSET_Y = 0;
        public const double VOLT_REG_ISO_PT3_OFFSET_X = 0;
        public const double VOLT_REG_ISO_PT3_OFFSET_Y = 12;

        public bool InteractiveMode = false;
        public short ActiveFNO = 0;
        public Int32 ActiveFID = 0;
        public int detailID = 0;

        public IGTDataContext DataContext;

        public double Vector2Angle(IGTVector vector)
        {
            double angle = 0;
            double x;
            double y;

            x = vector.I;
            y = vector.J;

            if (x != 0)
            {
                angle = Math.Atan(y / x) * 180 / Math.PI;

                if (x < 0)
                {
                    angle = angle + 180;
                }
                else if (y < 0)
                {
                    angle = angle + 360;
                }

                if (angle == 360)
                {
                    angle = 0;
                }
            }
            else
            {
                if (y >= 0)
                {
                    angle = 90;
                }
                else
                {
                    angle = 270;
                }
            }

            return angle;
        }

        public IGTPoint GetOffsetPoint(IGTPoint inputPt, double offsetX, double offsetY, IGTVector vector)
        {
            double angle = Vector2Angle(vector);

            double rotatedOffsetX = (Math.Cos(angle * Math.PI / 180) * offsetX) - (Math.Sin(angle * Math.PI / 180) * offsetY);
            double rotatedOffsetY = (Math.Sin(angle * Math.PI / 180) * offsetX) + (Math.Cos(angle * Math.PI / 180) * offsetY);

            IGTPoint gtPt = GTClassFactory.Create<IGTPoint>();
            gtPt.X = inputPt.X + rotatedOffsetX;
            gtPt.Y = inputPt.Y + rotatedOffsetY;

            return gtPt;
        }

        /// <summary>
        /// Establishes the connectivity relationship for the input features.
        /// </summary>
        /// <param name="activeKO">Active feature to connect.</param>
        /// <param name="relatedKO">Related feature to connect.</param>
        /// <param name="activeRelationshipOrdinal">Node on the active feature to connect.</param>
        /// <param name="relatedRelationshipOrdinal">Node on the related feature to connect.</param>
        /// <returns>Boolean indicating status</returns>
        public bool EstablishConnectivity(IGTKeyObject activeKO, IGTKeyObject relatedKO, GTRelationshipOrdinalConstants activeRelationshipOrdinal, GTRelationshipOrdinalConstants relatedRelationshipOrdinal)
        {
            bool returnValue = false;
            IGTRelationshipService relationshipService = null;
            Int32 node1 = 0;
            Int32 node2 = 0;
            Recordset connRS = null;

            try
            {
                // Get current connectivity for active feature.
                // This is needed to reset the connectivity if establish connectivity fails.
                // Establish connectivity may fail if data violates the rule defined in metadata.
                //     For example, Feature A is connected to Node 1 of the Transformer. 
                //     When the FI fires and creates an Isolation Point at the Transformer, 
                //     then all features currently at node 1 of the Transformer are connected to node 1 of the Isolation Point 
                //     and the Transformer is connected to node 2 of the Isolation Point. 
                //     The FI will attempt to connect Feature A to the Isolation Point which may not be allowed in metadata. This would throw an error.
                // There are many other reasons why the establish could fail, so best to catch any errors and reset the connectivity.
                connRS = relatedKO.Components.GetComponent(11).Recordset;
                if (connRS.RecordCount > 0)
                {
                    connRS.MoveFirst();
                    node1 = Convert.ToInt32(connRS.Fields["NODE_1_ID"].Value);
                    node2 = Convert.ToInt32(connRS.Fields["NODE_2_ID"].Value);
                }

                relationshipService = GTClassFactory.Create<IGTRelationshipService>();
                relationshipService.DataContext = DataContext;
                relationshipService.ActiveFeature = activeKO;

                if (relationshipService.AllowSilentEstablish(relatedKO))
                {
                    //relationshipService.SilentDelete(14, activeRelationshipOrdinal);
                    relationshipService.SilentEstablish(14, relatedKO, activeRelationshipOrdinal, relatedRelationshipOrdinal);
                }

                returnValue = true;
            }
            catch
            {
                //returnValue = false;
                returnValue = true;

                // Reset connectivity
                if (connRS != null)
                {
                    if (connRS.RecordCount > 0)
                    {
                        connRS.MoveFirst();
                        connRS.Fields["NODE_1_ID"].Value = node1;
                        connRS.Fields["NODE_2_ID"].Value = node2;
                    }
                }

                //if (InteractiveMode)
                //{
                //    MessageBox.Show("Error in Isolation Scenario FI:EstablishConnectivity - " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}
            }

            relationshipService.Dispose();

            return returnValue;
        }

        /// <summary>
        /// Determines which node of the related feature is connected to the active feature.
        /// </summary>
        /// <param name="activeKO">Active feature to connect.</param>
        /// <param name="relatedKO">Related feature to connect.</param>
        /// <param name="relatedRelationshipOrdinal">Node on the related feature to connect.</param>
        /// <returns>Boolean indicating status</returns>
        public bool DetermineConnectivityNode(IGTKeyObject activeKO, IGTKeyObject relatedKO, ref GTRelationshipOrdinalConstants relatedRelationshipOrdinal)
        {
            bool returnValue = false;
            IGTRelationshipService relationshipService = null;
            IGTKeyObjects m_relatedFeatures = null;
            bool foundNode = false;

            try
            {
                relationshipService = GTClassFactory.Create<IGTRelationshipService>();
                relationshipService.DataContext = DataContext;
                relationshipService.ActiveFeature = relatedKO;

                m_relatedFeatures = relationshipService.GetRelatedFeatures(14, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1);

                foreach (IGTKeyObject feature in m_relatedFeatures)
                {
                    if (feature.FID == activeKO.FID)
                    {
                        relatedRelationshipOrdinal = GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1;
                        foundNode = true;
                        break;
                    }
                }

                if (!foundNode)
                {
                    relatedRelationshipOrdinal = GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
                if (InteractiveMode)
                {
                    MessageBox.Show("Error in Isolation Scenario FI:DetermineConnectivityNode - " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            relationshipService.Dispose();

            return returnValue;
        }

        /// <summary>
        /// Gets and sets the attribute defaults for the passed in feature
        /// </summary>
        /// <param name="gtKeyObject">Feature key object to apply defaults.</param>
        /// <returns>Boolean indicating status</returns>
        public bool SetAttributeDefaults(IGTKeyObject gtKeyObject, out string message)
        {
            bool returnValue = false;

            try
            {
                message = "";
                Recordset componentRS = null;

                string sql = "select tabattr.g3e_cno, tabattr.g3e_field, tabattr.g3e_default " +
                             "from g3e_dialogattributes_optable tabattr, g3e_dialogs_optable dialog " +
                             "where dialog.g3e_fno = ? and tabattr.g3e_dtno = dialog.g3e_dtno " +
                             "and dialog.g3e_type = 'Placement' and tabattr.g3e_default is not null " +
                             "order by tabattr.g3e_cno";

                Recordset attrDefaultsRS = DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic,
                          LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, gtKeyObject.FNO);

                if (attrDefaultsRS.RecordCount > 0)
                {
                    short cno;
                    string field;
                    string defaultValue;
                    attrDefaultsRS.MoveFirst();

                    while (!attrDefaultsRS.EOF)
                    {
                        cno = Convert.ToInt16(attrDefaultsRS.Fields["G3E_CNO"].Value);
                        field = attrDefaultsRS.Fields["G3E_FIELD"].Value.ToString();
                        defaultValue = attrDefaultsRS.Fields["G3E_DEFAULT"].Value.ToString();

                        componentRS = gtKeyObject.Components.GetComponent(cno).Recordset;
                        componentRS.Fields[field].Value = defaultValue;

                        attrDefaultsRS.MoveNext();
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                returnValue = false;
            }
            return returnValue;
        }

        /// <summary>
        /// Sets attributes for the virtual point to keep in sync with the associated feature
        /// </summary>
        /// <param name="isoScenarioFeature">The associated feature from which to get attribute values.</param>
        /// <param name="gtKeyObject">Feature key object to validate and set attribute values.</param>
        /// <returns>Boolean indicating status</returns>
        public bool SetVirtualPointAttributes(IsoScenarioFeature isoScenarioFeature, IGTKeyObject gtKeyObject, out string message)
        {
            bool returnValue = false;

            try
            {
                message = "";

                Recordset isoPtRS = gtKeyObject.Components.GetComponent(4).Recordset;
                if (isoPtRS.RecordCount > 0)
                {
                    isoPtRS.MoveFirst();

                    string associatedFID = isoPtRS.Fields["ASSOCIATED_FID"].Value.ToString();
                    if (associatedFID != isoScenarioFeature.GtKeyObject.FID.ToString())
                    {
                        isoPtRS.Fields["ASSOCIATED_FID"].Value = isoScenarioFeature.GtKeyObject.FID;
                    }
                }

                string commFeatureState = string.Empty;

                isoPtRS = gtKeyObject.Components.GetComponent(1).Recordset;
                if (isoPtRS.RecordCount > 0)
                {
                    isoPtRS.MoveFirst();

                    commFeatureState = isoPtRS.Fields["FEATURE_STATE_C"].Value.ToString();
                    if (commFeatureState != isoScenarioFeature.FeatureState)
                    {
                        isoPtRS.Fields["FEATURE_STATE_C"].Value = isoScenarioFeature.FeatureState;
                        commFeatureState = isoScenarioFeature.FeatureState;
                    }
                }

                isoPtRS = gtKeyObject.Components.GetComponent(11).Recordset;
                if (isoPtRS.RecordCount > 0)
                {
                    isoPtRS.MoveFirst();

                    string phase = isoPtRS.Fields["PHASE_ALPHA"].Value.ToString();
                    if (phase != isoScenarioFeature.Phase)
                    {
                        isoPtRS.Fields["PHASE_ALPHA"].Value = isoScenarioFeature.Phase;
                    }

                    string connFeatureState = isoPtRS.Fields["FEATURE_STATE_C"].Value.ToString();
                    if (connFeatureState != commFeatureState)
                    {
                        isoPtRS.Fields["FEATURE_STATE_C"].Value = commFeatureState;
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                returnValue = false;
            }
            return returnValue;
        }

        public void PopulateProtectionDeviceIDForIsoPt(IGTKeyObject gTKeyObject,IGTKeyObject isoPtKeyObject)
        {
            try
            {
                Recordset connecRS = gTKeyObject.Components.GetComponent(11).Recordset;

                if (connecRS != null && connecRS.RecordCount > 0)
                {
                    connecRS.MoveFirst();

                    Recordset isoPtConnecRS = isoPtKeyObject.Components.GetComponent(11).Recordset;

                    if (isoPtConnecRS != null && isoPtConnecRS.RecordCount > 0)
                    {
                        isoPtConnecRS.MoveFirst();

                        isoPtConnecRS.Fields["PROTECTIVE_DEVICE_FID"].Value = connecRS.Fields["PROTECTIVE_DEVICE_FID"].Value;
                        isoPtConnecRS.Fields["PP_PROTECTIVE_DEVICE_FID"].Value = connecRS.Fields["PP_PROTECTIVE_DEVICE_FID"].Value;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Fix for ALM-1586
        /// Sets the Normal and As-Operated status to the values per business rules. 
        /// </summary>
        /// <param name="gtKeyObject"></param>
        /// <param name="isoPtNumber"></param>
        /// <returns></returns>
        public void SetNormalAndAsOperatedStatus(IGTKeyObject gtKeyObject, int isoPtNumber = -1)
        {
            Recordset componentRS = null;
            string defaultNormalStatus = string.Empty;
            string defaultAsOperatedStatus = string.Empty;
            try
            {

                componentRS = gtKeyObject.Components.GetComponent(1).Recordset;

                if (componentRS != null && componentRS.RecordCount > 0)
                {
                    componentRS.MoveFirst();

                    string featureState = Convert.ToString(componentRS.Fields["FEATURE_STATE_C"].Value);
                    componentRS = gtKeyObject.Components.GetComponent(11).Recordset;

                    if (featureState.Equals("PPI") || featureState.Equals("ABI")) // Feature is placed with PPI or ABI
                    {
                        if (isoPtNumber != -1)
                        {
                            if (isoPtNumber == 1 || isoPtNumber == 2)
                            {
                                defaultNormalStatus = "CLOSED";
                                defaultAsOperatedStatus = "OPEN";
                            }
                        }
                        else
                        {
                            defaultNormalStatus = "OPEN";
                            defaultAsOperatedStatus = "CLOSED";
                        }

                        if (!string.IsNullOrEmpty(defaultNormalStatus) && !string.IsNullOrEmpty(defaultAsOperatedStatus))
                        {
                            componentRS.Fields["STATUS_NORMAL_C"].Value = defaultNormalStatus;
                            componentRS.Fields["STATUS_OPERATED_C"].Value = defaultAsOperatedStatus;
                        }

                    }

                    else if (featureState.Equals("INI")) // Feature is placed in INI state (Job type - WR-MAPCOR)
                    {
                        if (isoPtNumber != -1)
                        {
                            if (isoPtNumber == 1 || isoPtNumber == 2)
                            {
                                defaultNormalStatus = "CLOSED";
                                defaultAsOperatedStatus = "OPEN";
                            }
                        }
                        else
                        {
                            defaultNormalStatus = "OPEN";
                            defaultAsOperatedStatus = "CLOSED";
                        }
                        componentRS = gtKeyObject.Components.GetComponent(11).Recordset;

                        if (!defaultAsOperatedStatus.Equals(defaultNormalStatus))
                        {
                            componentRS.Fields["STATUS_OPERATED_C"].Value = defaultNormalStatus;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EstablishOwnerShip(IGTKeyObject activeKO, IGTKeyObject isolationPoint)
        {
            IGTRelationshipService relationshipService = null;            
            try
            {
                relationshipService = GTClassFactory.Create<IGTRelationshipService>();
                relationshipService.DataContext = DataContext;
                relationshipService.ActiveFeature = activeKO;
                IGTKeyObjects m_relatedFeatures = relationshipService.GetRelatedFeatures(3);

                foreach (IGTKeyObject ownerFeature in m_relatedFeatures)
                {
                    relationshipService.ActiveFeature = isolationPoint;

                    if (relationshipService.AllowSilentEstablish(ownerFeature))
                    {
                        try
                        {
                            relationshipService.SilentEstablish(3, ownerFeature);
                        }
                        catch
                        {

                        }
                    }
                }
            }
            catch
            {

            }
            finally
            {
                relationshipService.Dispose();
                relationshipService = null;
            }
        }
        public bool CheckAssociatedVirtualPoint(IGTKeyObject feature,int isolationscenarioFeatureFID)
        {
            bool associated = false;
            try
            {
                String sql = "select G3E_FID,G3E_FNO from VIRTUALPT_N where ASSOCIATED_FID = ?";

                Recordset virtualRS = DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText,
                                                               isolationscenarioFeatureFID);
                if (virtualRS != null && virtualRS.RecordCount > 0)
                {
                    virtualRS.MoveFirst();

                    while (!virtualRS.EOF)
                    {
                        if (Convert.ToInt32(virtualRS.Fields["g3e_fid"].Value) == feature.FID)
                        {
                            associated = true;
                            break;
                        }
                        virtualRS.MoveNext();
                    }
                }
                virtualRS.Close();
                virtualRS = null;
            }
            catch
            {
                throw;
            }
           return associated;
        }

        public bool IsDetailMapWindow()
        {
            try
            {
                IGTApplication gTApplication = GTClassFactory.Create<IGTApplication>();
                detailID = gTApplication.ActiveMapWindow.DetailID;

                if(detailID==0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
