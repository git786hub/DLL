using Intergraph.GTechnology.API;
using System;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    internal class ProcessDEIS
    {
        private IGTDataContext m_DataContext = null;
        private IGTKeyObject transformerKO = null;
        private IGTKeyObject structureKO = null;

        public ProcessDEIS(IGTDataContext dataContext)
        {
            m_DataContext = dataContext;
        }

        /// <summary>
        /// The main function to that is called to process the transaction.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        internal bool Process()
        {
            bool returnValue = false;

            try
            {
                // Call appropriate activity method to process the transaction.
                switch (TransactionDEIS.TransactionCode)
                {
                    case "I":
                        if (!InstallTransformer())
                        {
                            return false;
                        }
                        break;
                    case "R":
                        if (!RemoveTransformer())
                        {
                            return false;
                        }
                        break;
                    case "C":
                        if (!ChangeoutTransformer())
                        {
                            return false;
                        }
                        break;
                    case "D":
                        if (TransactionDEIS.TransformerProperties.FID != 0)
                        {
                            if (!UpdateTransformer())
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!UpdateVoltageRegulator())
                            {
                                return false;
                            }
                        }
                        
                        break;
                    default:
                        break;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error processing transaction: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Performs the steps for an install transaction.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool InstallTransformer()
        {
            bool returnValue = false;

            try
            {
                if (TransactionDEIS.TransformerProperties.FID != 0)
                {                    
                    transformerKO = m_DataContext.OpenFeature(TransactionDEIS.TransformerProperties.FNO, TransactionDEIS.TransformerProperties.FID);

                    if (TransactionDEIS.TransformerProperties.FNO == TransactionDEIS.FNO_TRANSFORMER_OH || TransactionDEIS.TransformerProperties.FNO == TransactionDEIS.FNO_TRANSFORMER_OH_NETWORK)
                    {
                        // Add unit to existing Transformer if unit record doesn't exist for Company Number
                        if (TransactionDEIS.InsTrf1CompanyNumber.Length > 0)
                        {
                            if (!AddTransformerUnit(TransactionDEIS.InsTrf1CompanyNumber, TransactionDEIS.InsTrf1PhaseCode))
                            {
                                return false;
                            }
                        }
                        if (TransactionDEIS.InsTrf2CompanyNumber.Length > 0)
                        {
                            if (!AddTransformerUnit(TransactionDEIS.InsTrf2CompanyNumber, TransactionDEIS.InsTrf2PhaseCode))
                            {
                                return false;
                            }
                        }
                        if (TransactionDEIS.InsTrf3CompanyNumber.Length > 0)
                        {
                            if (!AddTransformerUnit(TransactionDEIS.InsTrf3CompanyNumber, TransactionDEIS.InsTrf3PhaseCode))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        // UG Transformer. Expecting non-repeating record, so update Company Number
                        ADODB.Recordset unitRS = transformerKO.Components.GetComponent(TransactionDEIS.TransformerProperties.UnitCNO).Recordset;
                        if (unitRS.RecordCount > 0)
                        {
                            unitRS.MoveFirst();
                            // Update Company Number
                            unitRS.Fields["COMPANY_ID"].Value = TransactionDEIS.InsTrf1CompanyNumber;
                        }
                    }

                    // Call method to set the Transformer attributes
                    if (!SetTransformerAttributes())
                    {
                        return false;
                    }
                }
                else
                {
                    // Install a new Transformer
                    if (!AddTransformer())
                    {
                        return false;
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error installing Transformer: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Adds a new Transformer feature.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool AddTransformer()
        {
            bool returnValue = false;

            try
            {
                // Create a new Transformer instance
                transformerKO = m_DataContext.NewFeature(TransactionDEIS.TransformerProperties.FNO, false);
                TransactionDEIS.TransformerProperties.FID = transformerKO.FID;
                ADODB.Recordset symbolRS = transformerKO.Components.GetComponent(TransactionDEIS.TransformerProperties.PrimaryGraphicCNO).Recordset;
                ADODB.Recordset labelRS = transformerKO.Components.GetComponent(TransactionDEIS.TransformerProperties.LabelCNO).Recordset;

                // Check if symbol component has been created. If metadata is defined with alternate required component then symbol record will need to be added.
                if (symbolRS.RecordCount == 0)
                {
                    symbolRS.AddNew();
                    symbolRS.Fields["G3E_FNO"].Value = transformerKO.FNO;
                    symbolRS.Fields["G3E_FID"].Value = transformerKO.FID;
                }

                // Check if label component has been created. If metadata is defined with alternate required component then label record will need to be added.
                if (labelRS.RecordCount == 0)
                {
                    labelRS.AddNew();
                    labelRS.Fields["G3E_FNO"].Value = transformerKO.FNO;
                    labelRS.Fields["G3E_FID"].Value = transformerKO.FID;
                }

                // Set the symbol geometry to the location of the conductor
                structureKO = m_DataContext.OpenFeature(TransactionDEIS.StructureProperties.FNO, TransactionDEIS.StructureProperties.FID);
                IGTKeyObject conductorKO = m_DataContext.OpenFeature(TransactionDEIS.PrimaryProperties.FNO, TransactionDEIS.PrimaryProperties.FID);
                IGTOrientedPointGeometry xfmrPt = GTClassFactory.Create<IGTOrientedPointGeometry>();
                Recordset FacilityGeometryInformation = m_DataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + conductorKO.FNO);
                short connectingPrimaryGraphicCNO = Convert.ToInt16(FacilityGeometryInformation.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value.ToString());
                xfmrPt.Origin = DetermineClosestPoint(conductorKO.Components.GetComponent(connectingPrimaryGraphicCNO).Geometry, structureKO.Components.GetComponent(TransactionDEIS.StructureProperties.PrimaryGraphicCNO).Geometry.FirstPoint, true);

                transformerKO.Components.GetComponent(TransactionDEIS.TransformerProperties.PrimaryGraphicCNO).Geometry = xfmrPt;

                // Set label geometry at offset from symbol. Use offset values from metadata.
                string sql = "select param_value from sys_generalparameter where subsystem_name = ? and subsystem_component = ? and param_name = ?";
                double xOffset = 0;
                double yOffset = 0;

                ADODB.Recordset metadataRS = m_DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockReadOnly, -1, "GISAUTO_DEIS", "Placement", "XFMR_LABEL_XY_OFFSET");

                if (metadataRS.RecordCount > 0)
                {
                    metadataRS.MoveFirst();
                    string[] paramValue = metadataRS.Fields["PARAM_VALUE"].Value.ToString().Split(',');

                    xOffset = Convert.ToDouble(paramValue[0].Trim());
                    yOffset = Convert.ToDouble(paramValue[1].Trim());
                }

                IGTPoint point = GTClassFactory.Create<IGTPoint>();
                point.X = structureKO.Components.GetComponent(TransactionDEIS.StructureProperties.PrimaryGraphicCNO).Geometry.FirstPoint.X + xOffset;
                point.Y = structureKO.Components.GetComponent(TransactionDEIS.StructureProperties.PrimaryGraphicCNO).Geometry.FirstPoint.Y + yOffset;

                IGTTextPointGeometry labelPoint = GTClassFactory.Create<IGTTextPointGeometry>();
                labelPoint.Origin = point;
                transformerKO.Components.GetComponent(TransactionDEIS.TransformerProperties.LabelCNO).Geometry = labelPoint;

                ADODB.Recordset unitRS = transformerKO.Components.GetComponent(TransactionDEIS.TransformerProperties.UnitCNO).Recordset;
                
                // Create a unit and cu record for each Transformer to install.
                if (TransactionDEIS.InsTrf1CompanyNumber.Length > 0)
                {
                    if (unitRS.RecordCount > 0)
                    {
                        // Unit record is required, so one record will be created when the Transformer is created. Use this record if exists.
                        unitRS.MoveFirst();
                        unitRS.Fields["COMPANY_ID"].Value = TransactionDEIS.InsTrf1CompanyNumber;
                    }
                    else
                    {
                        if (!AddTransformerUnit(TransactionDEIS.InsTrf1CompanyNumber, TransactionDEIS.InsTrf1PhaseCode))
                        {
                            return false;
                        }                        
                    }
                }
                if (TransactionDEIS.InsTrf2CompanyNumber.Length > 0)
                {
                    if (!AddTransformerUnit(TransactionDEIS.InsTrf2CompanyNumber, TransactionDEIS.InsTrf2PhaseCode))
                    {
                        return false;
                    }
                }
                if (TransactionDEIS.InsTrf3CompanyNumber.Length > 0)
                {
                    if (!AddTransformerUnit(TransactionDEIS.InsTrf3CompanyNumber, TransactionDEIS.InsTrf3PhaseCode))
                    {
                        return false;
                    }
                }

                // Call method to set the default tab attributes
                string message = string.Empty;
                if (!SetAttributeDefaults(transformerKO, out message))
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                    TransactionDEIS.TransactionMessage = message;
                    returnValue = false;
                }

                // Call method to set the Transformer attributes
                if (!SetTransformerAttributes())
                {
                    return false;
                }

                // Set secondary voltage
                ADODB.Recordset connectivityRS = transformerKO.Components.GetComponent(TransactionDEIS.CNO_CONNECTIVITY_ATTRIBUTES).Recordset;
                if (connectivityRS.RecordCount > 0)
                {
                    connectivityRS.MoveFirst();
                    connectivityRS.Fields["VOLT_2_Q"].Value = TransactionDEIS.InsTrf1SecVoltValue;
                }

                // Establish the ownership and connectivity relationships
                if (!EstablishOwnership(transformerKO, structureKO))
                {
                    return false;
                }

                if (TransactionDEIS.TransactionCode != "C" && TransactionDEIS.TransformerProperties.Node1 == 0 && TransactionDEIS.TransformerProperties.Node2 == 0)
                {
                    //// Add Isolation Point
                    //IGTKeyObject isolationPointKO = m_DataContext.NewFeature(TransactionDEIS.FNO_ISOLATION_POINT);

                    //// Check if symbol component has been created. If metadata is defined with alternate required component then symbol record will need to be added.
                    //symbolRS = isolationPointKO.Components.GetComponent(TransactionDEIS.CNO_ISOLATION_POINT_SYMBOL).Recordset;
                    //if (symbolRS.RecordCount == 0)
                    //{
                    //    symbolRS.AddNew();
                    //    symbolRS.Fields["G3E_FNO"].Value = isolationPointKO.FNO;
                    //    symbolRS.Fields["G3E_FID"].Value = isolationPointKO.FID;
                    //}

                    //// Call method to update Isolation Point attributes
                    //if (!SetIsolationPointAttributes(isolationPointKO))
                    //{
                    //    returnValue = false;
                    //}

                    //isolationPointKO.Components.GetComponent(TransactionDEIS.CNO_ISOLATION_POINT_SYMBOL).Geometry = transformerKO.Components.GetComponent(TransactionDEIS.TransformerProperties.PrimaryGraphicCNO).Geometry;
                    
                    // Add Secondary Conductor Node
                    IGTKeyObject secondaryNodeKO = m_DataContext.NewFeature(TransactionDEIS.FNO_SECONDARY_CONDNODE);

                    // Call method to update Secondary Conductor Node attributes
                    if (!SetConductorNodeAttributes(secondaryNodeKO))
                    {
                        returnValue = false;
                    }

                    secondaryNodeKO.Components.GetComponent(TransactionDEIS.CNO_SECONDARY_CONDNODE_SYMBOL).Geometry = transformerKO.Components.GetComponent(TransactionDEIS.TransformerProperties.PrimaryGraphicCNO).Geometry;

                    // Establish the ownership between the Secondary Conductor Node and Structure.
                    if (!EstablishOwnership(secondaryNodeKO, structureKO))
                    {
                        return false;
                    }

                    // Connect the Primary Conductor, Transformer, and Secondary Conductor Node
                    if (TransactionDEIS.PrimaryProperties.ConnectNode == 0)
                    {
                        // Determine the Primary Conductor Node at which to connect the Isolation Point.
                        TransactionDEIS.PrimaryProperties.ConnectNode = 2;
                        ADODB.Recordset structureRS = structureKO.Components.GetComponent(TransactionDEIS.CNO_COMMON_ATTRIBUTES).Recordset;
                        int structureG3EID = 0;
                        if (structureRS.RecordCount > 0)
                        {
                            structureRS.MoveFirst();
                            structureG3EID = Convert.ToInt32(structureRS.Fields["G3E_ID"].Value);
                        }

                        ADODB.Recordset conductorRS = conductorKO.Components.GetComponent(TransactionDEIS.CNO_COMMON_ATTRIBUTES).Recordset;

                        if (conductorRS.RecordCount > 0)
                        {
                            int conductorOwnerID = 0;
                            conductorRS.MoveFirst();
                            if (!Convert.IsDBNull(conductorRS.Fields["OWNER1_ID"].Value))
                            {
                                conductorOwnerID = Convert.ToInt32(conductorRS.Fields["OWNER1_ID"].Value);
                                if (conductorOwnerID == structureG3EID)
                                {
                                    TransactionDEIS.PrimaryProperties.ConnectNode = 1;
                                }
                            }
                        }
                    }
                    
                    // Connect the Transformer to the Primary Conductor
                    if (!EstablishConnectivity(transformerKO, conductorKO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, (GTRelationshipOrdinalConstants)TransactionDEIS.PrimaryProperties.ConnectNode))
                    {
                        returnValue = false;
                    }

                    //// Connect the Isolation Point to the Transformer
                    //if (!EstablishConnectivity(isolationPointKO, transformerKO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1))
                    //{
                    //    returnValue = false;
                    //}

                    // Connect the Secondary Conductor Node to the Transformer
                    if (!EstablishConnectivity(secondaryNodeKO, transformerKO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2))
                    {
                        returnValue = false;
                    }
                }
                else
                {
                    if (connectivityRS.RecordCount > 0)
                    {
                        connectivityRS.MoveFirst();
                        connectivityRS.Fields["NODE_1_ID"].Value = TransactionDEIS.TransformerProperties.Node1;
                        connectivityRS.Fields["NODE_2_ID"].Value = TransactionDEIS.TransformerProperties.Node2;
                    }
                }
                

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error adding new Transformer: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

    /// <summary>
    /// Determine which point of the passed in geometry is closest to the target point. 
    /// </summary>
    /// <param name="sourceGeometry">The IGTGeometry of the connecting facility</param>
    /// <param name="targetPt">The IGTPoint to use for comparison</param>
    /// <param name="returnClosestPt">Boolean indicating which point to return</param>
    /// <returns>returnClosestPt = true then return the IGTPoint closest to the target point, otherwise return the IGTPoint furthest away from the target point</returns>
    internal IGTPoint DetermineClosestPoint(IGTGeometry sourceGeometry, IGTPoint targetPt, bool returnClosestPt)
    {
      IGTPoint gtPt = sourceGeometry.FirstPoint;

      if (sourceGeometry.FirstPoint.X != sourceGeometry.LastPoint.X || sourceGeometry.FirstPoint.Y != sourceGeometry.LastPoint.Y)
      {
        double distanceX = targetPt.X - sourceGeometry.FirstPoint.X;
        double distanceY = targetPt.Y - sourceGeometry.FirstPoint.Y;
        double pt1Distance = Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY));

        distanceX = targetPt.X - sourceGeometry.LastPoint.X;
        distanceY = targetPt.Y - sourceGeometry.LastPoint.Y;
        double pt2Distance = Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY));

        if (pt2Distance < pt1Distance)
        {
          if (returnClosestPt)
          {
            gtPt = sourceGeometry.LastPoint;
          }
        }
        else
        {
          if (!returnClosestPt)
          {
            gtPt = sourceGeometry.LastPoint;
          }
        }
      }
      else
      {
        if (!returnClosestPt)
        {
          gtPt = sourceGeometry.LastPoint;
        }
      }

      return gtPt;
    }

    /// <summary>
    /// Adds a new Transformer unit to an existing Transformer if Company Number doesn't exist.
    /// </summary>
    ///  <param name="companyNumber">Transaction Company Number</param>
    ///   <param name="phase">Transaction phase</param>
    /// <returns>Boolean indicating status</returns>
    private bool AddTransformerUnit(string companyNumber, string phase)
        {
            bool returnValue = false;

            try
            {                
                ADODB.Recordset unitRS = transformerKO.Components.GetComponent(TransactionDEIS.TransformerProperties.UnitCNO).Recordset;
                unitRS.Filter = "COMPANY_ID='" + companyNumber + "'";
                if (unitRS.RecordCount == 0)
                {
                    // Check if record exists with phase value matching transaction phase
                    unitRS.Filter = "PHASE_C='" + phase + "'";
                    if (unitRS.RecordCount == 0)
                    {
                        // Create a new Transformer unit record
                        unitRS.Filter = "";
                        unitRS.AddNew();
                        unitRS.Fields["G3E_FNO"].Value = transformerKO.FNO;
                        unitRS.Fields["G3E_FID"].Value = transformerKO.FID;
                        unitRS.Fields["COMPANY_ID"].Value = companyNumber;

                        // Create a new CU record
                        ADODB.Recordset cuRS = transformerKO.Components.GetComponent(TransactionDEIS.CNO_CU_ATTRIBUTES).Recordset;
                        cuRS.AddNew();
                        cuRS.Fields["G3E_FNO"].Value = transformerKO.FNO;
                        cuRS.Fields["G3E_FID"].Value = transformerKO.FID;
                    }
                    else
                    {
                        // Update Company Number
                        unitRS.Fields["COMPANY_ID"].Value = companyNumber;
                    }                    
                }

                unitRS.Filter = "";

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error adding new Transformer unit: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Establishes the connectivity relationship for the input features.
        /// </summary>
        /// <param name="activeKO">Active feature to connect.</param>
        /// <param name="relatedKO">Related feature to connect.</param>
        /// <param name="activeRelationshipOrdinal">Node on the active feature to connect.</param>
        /// <param name="relatedRelationshipOrdinal">Node on the related feature to connect.</param>
        /// <returns>Boolean indicating status</returns>
        private bool EstablishConnectivity(IGTKeyObject activeKO, IGTKeyObject relatedKO, GTRelationshipOrdinalConstants activeRelationshipOrdinal, GTRelationshipOrdinalConstants relatedRelationshipOrdinal)
        {
            bool returnValue = false;
            IGTRelationshipService relationshipService = null;

            try
            {
                relationshipService = GTClassFactory.Create<IGTRelationshipService>();
                relationshipService.DataContext = m_DataContext;
                relationshipService.ActiveFeature = activeKO;

                if (relationshipService.AllowSilentEstablish(relatedKO))
                {
                    relationshipService.SilentEstablish(TransactionDEIS.RNO_CONNECTIVITY, relatedKO, activeRelationshipOrdinal, relatedRelationshipOrdinal);
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error establishing connectivity: " + ex.Message;
                returnValue = false;
            }

            relationshipService.Dispose();

            return returnValue;
        }

        /// <summary>
        /// Establishes the ownership relationship for the input features.
        /// </summary>
        /// <param name="activeKO">Active feature to be owned.</param>
        /// <param name="ownerKO">Owning feature.</param>
        /// <returns>Boolean indicating status</returns>
        private bool EstablishOwnership(IGTKeyObject activeKO, IGTKeyObject ownerKO)
        {
            bool returnValue = false;
            IGTRelationshipService relationshipService = null;

            try
            {
                relationshipService = GTClassFactory.Create<IGTRelationshipService>();
                relationshipService.DataContext = m_DataContext;
                relationshipService.ActiveFeature = activeKO;

                if (relationshipService.AllowSilentEstablish(ownerKO))
                {
                    relationshipService.SilentEstablish(TransactionDEIS.RNO_OWNERSHIP_CHILD, ownerKO);
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error establishing ownership: " + ex.Message;
                returnValue = false;
            }

            relationshipService.Dispose();

            return returnValue;
        }

        /// <summary>
        /// Performs the steps for a removal transaction.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool RemoveTransformer()
        {
            bool returnValue = false;

            try
            {
                transformerKO = m_DataContext.OpenFeature(TransactionDEIS.TransformerProperties.FNO, TransactionDEIS.TransformerProperties.FID);
                
                if (TransactionDEIS.TransformerProperties.RemoveBank)
                {
                    // Call method to delete the Transformer
                    if (!RemoveTransformerBank())
                    {
                        return false;
                    }
                }
                else
                {
                    // Call method to delete the Transformer Unit(s)
                    if (!RemoveTransformerUnit())
                    {
                        return false;
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error removing Transformer: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Performs the steps for a changeout transaction.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool ChangeoutTransformer()
        {
            bool returnValue = false;

            try
            {
                // Call method to process the existing Transformer if one was found
                if (TransactionDEIS.TransformerProperties.FID != 0)
                {
                    transformerKO = m_DataContext.OpenFeature(TransactionDEIS.TransformerProperties.FNO, TransactionDEIS.TransformerProperties.FID);
                    if (TransactionDEIS.TransformerProperties.FeatureState == "PPX" || TransactionDEIS.TransformerProperties.FeatureState == "ABX")
                    {
                        // Delete the records where CU Activity = Remove, but don't delete the feature.
                        ADODB.Recordset unitRS = transformerKO.Components.GetComponent(TransactionDEIS.TransformerProperties.UnitCNO).Recordset;
                        ADODB.Recordset cuRS = transformerKO.Components.GetComponent(TransactionDEIS.CNO_CU_ATTRIBUTES).Recordset;
                        int cid = 0;
                        
                        cuRS.Filter = "activity_c = 'R'";
                        if (cuRS.RecordCount > 0)
                        {
                            cuRS.MoveFirst();
                            while(!cuRS.EOF)
                            {
                                if (!Convert.IsDBNull(cuRS.Fields["UNIT_CID"].Value))
                                {
                                    cid = Convert.ToInt32(cuRS.Fields["UNIT_CID"].Value);
                                }
                                else
                                {
                                    cid = Convert.ToInt32(cuRS.Fields["G3E_CID"].Value);
                                }

                                cuRS.Delete();

                                unitRS.Filter = "g3e_cid = " + cid;

                                if (unitRS.RecordCount > 0)
                                {
                                    unitRS.Delete();
                                }                                

                                cuRS.Filter = "activity_c = 'R'";
                                if (cuRS.RecordCount > 0)
                                {
                                    cuRS.MoveFirst();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        cuRS.Filter = "";
                        unitRS.Filter = "";
                    }
                    else if (!RemoveTransformerBank())
                    {
                        return false;
                    }
                }

                int transformerFID = TransactionDEIS.TransformerProperties.FID;
                string featureState = TransactionDEIS.TransformerProperties.FeatureState;

                // Reset data structures
                TransactionDEIS.InitializeDataStructures();

                if (featureState == "PPX" || featureState == "ABX")
                {
                    // Use the same transformer as the removal
                    TransactionDEIS.TransformerProperties.FID = transformerFID;
                    TransactionDEIS.TransformerProperties.FeatureState = featureState;
                }

                // Install validation needs to be done after removal. Call method to validate install.
                ValidateDEIS validateDEIS = new ValidateDEIS(m_DataContext);
                if (!validateDEIS.ChangeoutValidationTransformer(false))
                {
                    return false;
                }

                if (!InstallTransformer())
                {
                    return false;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error changing out Transformer: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Performs the steps for an update Transformer transaction.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool UpdateTransformer()
        {
            bool returnValue = false;

            try
            {                
                transformerKO = m_DataContext.OpenFeature(TransactionDEIS.TransformerProperties.FNO, TransactionDEIS.TransformerProperties.FID);

                // Call method to update Transformer attributes
                if (!SetTransformerAttributes())
                {
                    return false;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error updating Transformer: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Performs the steps for an update Voltage Regulator transaction.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool UpdateVoltageRegulator()
        {
            bool returnValue = false;

            try
            {
                IGTKeyObject voltageRegulatorKO = m_DataContext.OpenFeature(TransactionDEIS.FNO_VOLTAGE_REGULATOR, TransactionDEIS.VoltageRegulatorFID);

                ADODB.Recordset voltageRegulatorRS = voltageRegulatorKO.Components.GetComponent(TransactionDEIS.CNO_VOLTAGE_REGULATOR_ATTRIBUTES).Recordset;

                if (voltageRegulatorRS.RecordCount > 0)
                {
                    voltageRegulatorRS.MoveFirst();
                    voltageRegulatorRS.Fields["COMPANY_ID"].Value = TransactionDEIS.InsTrf1CompanyNumber;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error updating Voltage Regulator: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Updates the Transformer attributes.
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool SetTransformerAttributes()
        {
            bool returnValue = false;

            try
            {                
                ADODB.Recordset unitRS = transformerKO.Components.GetComponent(TransactionDEIS.TransformerProperties.UnitCNO).Recordset;
                ADODB.Recordset cuRS = transformerKO.Components.GetComponent(TransactionDEIS.CNO_CU_ATTRIBUTES).Recordset;                

                // Set unit and cu attributes.
                if (unitRS.RecordCount > 0)
                {
                    int cuG3eCid = 0;
                    string cu = string.Empty;
                    string phase = string.Empty;
                    string tsn = string.Empty;
                    bool foundRecord = false;

                    unitRS.MoveFirst();

                    while (!unitRS.EOF)
                    {
                        // Need to determine cu record to update using company number and unit_cid/g3e_cid values
                        if (TransactionDEIS.InsTrf1CompanyNumber.Length > 0 && TransactionDEIS.InsTrf1CompanyNumber == unitRS.Fields["COMPANY_ID"].Value.ToString())
                        {
                            cu = TransactionDEIS.InsTrf1CU;
                            phase = TransactionDEIS.InsTrf1PhaseCode;
                            tsn = TransactionDEIS.InsTrf1TSN;
                            foundRecord = true;
                        }
                        else if (TransactionDEIS.InsTrf2CompanyNumber.Length > 0 && TransactionDEIS.InsTrf2CompanyNumber == unitRS.Fields["COMPANY_ID"].Value.ToString())
                        {
                            cu = TransactionDEIS.InsTrf2CU;
                            phase = TransactionDEIS.InsTrf2PhaseCode;
                            tsn = TransactionDEIS.InsTrf2TSN;
                            foundRecord = true;
                        }
                        else if (TransactionDEIS.InsTrf3CompanyNumber.Length > 0 && TransactionDEIS.InsTrf3CompanyNumber == unitRS.Fields["COMPANY_ID"].Value.ToString())
                        {
                            cu = TransactionDEIS.InsTrf3CU;
                            phase = TransactionDEIS.InsTrf3PhaseCode;
                            tsn = TransactionDEIS.InsTrf3TSN;
                            foundRecord = true;
                        }

                        if (foundRecord)
                        {
                            // Set Unit attributes
                            unitRS.Fields["TSN_ID"].Value = tsn;

                            if (TransactionDEIS.TransformerProperties.FNO != TransactionDEIS.FNO_TRANSFORMER_UG && TransactionDEIS.TransformerProperties.FNO != TransactionDEIS.FNO_TRANSFORMER_UG_NETWORK)
                            {
                                unitRS.Fields["PHASE_C"].Value = phase;
                            }
                            else
                            {
                                unitRS.Fields["CONFIG_PRI_C"].Value = TransactionDEIS.TransformerProperties.PrimaryWiringConfiguration;
                                unitRS.Fields["CONFIG_SEC_C"].Value = TransactionDEIS.TransformerProperties.SecondaryWiringConfiguration;
                            }

                            // Set CU attributes
                            cuG3eCid = Convert.ToInt32(unitRS.Fields["G3E_CID"].Value);
                            
                            if (cuRS.RecordCount > 0)
                            {
                                cuRS.MoveFirst();

                                if (!Convert.IsDBNull(cuRS.Fields["UNIT_CID"].Value))
                                {
                                    cuRS.Filter = "unit_cid = " + cuG3eCid;
                                }
                                else
                                {
                                    cuRS.Filter = "g3e_cid = " + cuG3eCid;
                                }
                                
                                if (cuRS.RecordCount > 0)
                                {
                                    // Update CU record
                                    cuRS.Fields["CU_C"].Value = cu;
                                    cuRS.Fields["ACTIVITY_C"].Value = "";
                                    if (TransactionDEIS.TransactionCode != "D")
                                    {
                                        cuRS.Fields["QTY_LENGTH_Q"].Value = 1;
                                    }
                                }
                            }                            
                        }

                        foundRecord = false;
                        unitRS.MoveNext();
                    }
                }

                unitRS.Filter = "";
                cuRS.Filter = "";

                if (TransactionDEIS.TransactionCode != "D")
                {
                    // Set PROTECTIVE_DEVICE_FID = 0 if connected Primary Conductor's PROTECTIVE_DEVICE_FID is null
                    if (TransactionDEIS.PrimaryProperties.ProtectiveDeviceID == 0)
                    {
                        ADODB.Recordset connRS = transformerKO.Components.GetComponent(TransactionDEIS.CNO_CONNECTIVITY_ATTRIBUTES).Recordset;
                        if (connRS.RecordCount > 0)
                        {
                            connRS.MoveFirst();
                            connRS.Fields["PROTECTIVE_DEVICE_FID"].Value = 0;
                        }
                    }

                    // Set Bank attributes for OH
                    if (TransactionDEIS.TransformerProperties.FNO == TransactionDEIS.FNO_TRANSFORMER_OH || TransactionDEIS.TransformerProperties.FNO == TransactionDEIS.FNO_TRANSFORMER_OH_NETWORK)
                    {
                        ADODB.Recordset bankRS = transformerKO.Components.GetComponent(TransactionDEIS.TransformerProperties.BankCNO).Recordset;
                        if (bankRS.RecordCount > 0)
                        {
                            bankRS.MoveFirst();
                            bankRS.Fields["CONFIG_PRI_C"].Value = TransactionDEIS.TransformerProperties.PrimaryWiringConfiguration;
                            bankRS.Fields["CONFIG_SEC_C"].Value = TransactionDEIS.TransformerProperties.SecondaryWiringConfiguration;
                        }
                    }

                    // Set Bank attributes for Autotransformer
                    if (TransactionDEIS.TransformerProperties.FNO == TransactionDEIS.FNO_AUTOTRANSFORMER)
                    {
                        ADODB.Recordset bankRS = transformerKO.Components.GetComponent(TransactionDEIS.TransformerProperties.BankCNO).Recordset;
                        if (bankRS.RecordCount > 0)
                        {
                            bankRS.MoveFirst();
                            bankRS.Fields["BANK_CONFIG_C"].Value = TransactionDEIS.TransformerProperties.PrimaryWiringConfiguration;
                        }
                    }

                    // Set feature state
                    if (TransactionDEIS.TransformerProperties.FeatureState != "CLS")
                    {
                        ADODB.Recordset commonRS = transformerKO.Components.GetComponent(TransactionDEIS.CNO_COMMON_ATTRIBUTES).Recordset;
                        if (commonRS.RecordCount > 0)
                        {
                            commonRS.MoveFirst();
                            commonRS.Fields["FEATURE_STATE_C"].Value = "CLS";
                        }
                    }
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error updating Transformer attributes: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Removes the Transformer Unit(s).
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool RemoveTransformerUnit()
        {
            bool returnValue = false;

            try
            {
                // Remove the transformer unit records from the located transformer using the input company number(s)
                ADODB.Recordset unitRS = transformerKO.Components.GetComponent(TransactionDEIS.TransformerProperties.UnitCNO).Recordset;
                ADODB.Recordset cuRS = transformerKO.Components.GetComponent(TransactionDEIS.CNO_CU_ATTRIBUTES).Recordset;

                if (unitRS.RecordCount > 0)
                {
                    unitRS.MoveFirst();

                    string currentCompanyNumber = string.Empty;
                    int currentUnitCID = 0;
                    int cuCID = 0;

                    while (!unitRS.EOF)
                    {
                        currentCompanyNumber = unitRS.Fields["COMPANY_ID"].Value.ToString();
                        currentUnitCID = Convert.ToInt32(unitRS.Fields["G3E_CID"].Value);
                        if (currentCompanyNumber == TransactionDEIS.RemTrf1CompanyNumber || currentCompanyNumber == TransactionDEIS.RemTrf2CompanyNumber || currentCompanyNumber == TransactionDEIS.RemTrf3CompanyNumber)
                        {
                            unitRS.Delete();

                            // Delete corresponding CU record.
                            if (cuRS.RecordCount > 0)
                            {
                                cuRS.MoveFirst();

                                while (!cuRS.EOF)
                                {
                                    if (!Convert.IsDBNull(cuRS.Fields["UNIT_CID"].Value))
                                    {
                                        cuCID = Convert.ToInt32(cuRS.Fields["UNIT_CID"].Value);
                                    }
                                    else
                                    {
                                        cuCID = Convert.ToInt32(cuRS.Fields["G3E_CID"].Value);
                                    }

                                    if (currentUnitCID == cuCID)
                                    {
                                        cuRS.Delete();
                                        break;
                                    }
                                    cuRS.MoveNext();
                                }
                            }
                        }
                        unitRS.MoveNext();
                    }
                }

                unitRS.Filter = "";
                cuRS.Filter = "";

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error removing Transformer Unit(s): " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Removes the Transformer Bank (feature).
        /// </summary>
        /// <returns>Boolean indicating status</returns>
        private bool RemoveTransformerBank()
        {
            bool returnValue = false;
            string message = string.Empty;

            try
            {
                if (TransactionDEIS.TransactionCode == "R")
                {
                    // Check if secondary conductor or service line is connected to transformer.
                    string sqlStatement = "select conn2.g3e_fno, conn2.g3e_fid, nvl(conn1.node_1_id,0) xfmrNode1, nvl(conn1.node_2_id,0) xfmrNode2, nvl(conn2.node_1_id,0) secNode1, nvl(conn2.node_2_id,0) secNode2 " +
                                   "from connectivity_n conn1, connectivity_n conn2 where conn1.g3e_fid = ? " +
                                   "and ((nvl(conn1.node_1_id,0) <> 0 and (conn2.node_1_id = conn1.node_1_id or conn2.node_2_id = conn1.node_1_id)) or (nvl(conn1.node_2_id,0) <> 0 and (conn2.node_1_id = conn1.node_2_id or conn2.node_2_id = conn1.node_2_id))) " +
                                   "and conn2.g3e_fno in (53,54,63,96,97)";
                    int recordsAffected = 0;
                    ADODB.Recordset transformerConnRS = m_DataContext.Execute(sqlStatement, out recordsAffected, (int)CommandTypeEnum.adCmdText, TransactionDEIS.TransformerProperties.FID);

                    if (transformerConnRS.RecordCount > 0)
                    {
                        transformerConnRS.MoveFirst();

                        TransactionDEIS.SecondaryProperties.FNO = Convert.ToInt16(transformerConnRS.Fields["G3E_FNO"].Value);
                        TransactionDEIS.SecondaryProperties.FID = Convert.ToInt32(transformerConnRS.Fields["G3E_FID"].Value);

                        // Secondary exists. Put secondary node at connect point to not break connectivity. Log a warning.
                        IGTKeyObject secondaryNodeKO = m_DataContext.NewFeature(TransactionDEIS.FNO_SECONDARY_CONDNODE);

                        // Call method to update Secondary Conductor Node attributes
                        if (!SetConductorNodeAttributes(secondaryNodeKO))
                        {
                            returnValue = false;
                        }

                        IGTKeyObject secondaryKO = m_DataContext.OpenFeature(TransactionDEIS.SecondaryProperties.FNO, TransactionDEIS.SecondaryProperties.FID);

                        secondaryNodeKO.Components.GetComponent(TransactionDEIS.CNO_SECONDARY_CONDNODE_SYMBOL).Geometry = transformerKO.Components.GetComponent(TransactionDEIS.TransformerProperties.PrimaryGraphicCNO).Geometry;

                        // Determine which node on the Secondary Conductor to connect.
                        int xfmrNode1 = Convert.ToInt32(transformerConnRS.Fields["xfmrNode1"].Value);
                        int xfmrNode2 = Convert.ToInt32(transformerConnRS.Fields["xfmrNode2"].Value);
                        int secNode2 = Convert.ToInt32(transformerConnRS.Fields["secNode2"].Value);

                        GTRelationshipOrdinalConstants secConnectNode = GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1;

                        if (secNode2 == xfmrNode1 || secNode2 == xfmrNode2)
                        {
                            secConnectNode = GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal2;
                        }

                        // Connect the Secondary Conductor Node to the Secondary Conductor
                        if (!EstablishConnectivity(secondaryNodeKO, secondaryKO, GTRelationshipOrdinalConstants.gtrelRelationshipOrdinal1, secConnectNode))
                        {
                            returnValue = false;
                        }

                        // Establish the ownership between the Secondary Conductor Node and Structure.
                        structureKO = m_DataContext.OpenFeature(TransactionDEIS.StructureProperties.FNO, TransactionDEIS.StructureProperties.FID);
                        if (!EstablishOwnership(secondaryNodeKO, structureKO))
                        {
                            return false;
                        }

                        TransactionDEIS.TransactionStatus = TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_WARNING;
                        TransactionDEIS.TransactionMessage = "Secondary exists at removed Transformer";
                    }
                }
                
                // Remove Transformer feature
                if (!DeleteFeature(TransactionDEIS.TransformerProperties.FID, TransactionDEIS.TransformerProperties.FNO, out message))
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                    TransactionDEIS.TransactionMessage = message;
                    returnValue = false;
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error removing Transformer Bank: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Sets the attribute defaults for the passed in Secondary Conductor Node feature
        /// </summary>
        /// <param name="gtKeyObject">Feature key object to apply defaults.</param>
        /// <returns>Boolean indicating status</returns>
        private bool SetConductorNodeAttributes(IGTKeyObject gtKeyObject)
        {
            bool returnValue = false;
            string message = "";

            try
            {
                // Set tab attribute default values for Secondary Node
                if (!SetAttributeDefaults(gtKeyObject, out message))
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                    TransactionDEIS.TransactionMessage = message;
                    returnValue = false;
                }

                // Set Secondary Conductor Node type attribute to "DEADEND"
                ADODB.Recordset secCondNodeRS = gtKeyObject.Components.GetComponent(TransactionDEIS.CNO_SECONDARY_CONDNODE_ATTRIBUTES).Recordset;
                if (secCondNodeRS.RecordCount > 0)
                {
                    secCondNodeRS.MoveFirst();
                    secCondNodeRS.Fields["TYPE_C"].Value = "DEADEND";
                }

                // Set Secondary Conductor Node feature state attribute to "CLS".
                secCondNodeRS = gtKeyObject.Components.GetComponent(TransactionDEIS.CNO_COMMON_ATTRIBUTES).Recordset;
                if (secCondNodeRS.RecordCount > 0)
                {
                    secCondNodeRS.MoveFirst();
                    secCondNodeRS.Fields["FEATURE_STATE_C"].Value = "CLS";
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error setting Secondary Conductor Node attributes: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Sets the attribute defaults for the passed in Isolation Point feature
        /// </summary>
        /// <param name="gtKeyObject">Feature key object to apply defaults.</param>
        /// <returns>Boolean indicating status</returns>
        private bool SetIsolationPointAttributes(IGTKeyObject gtKeyObject)
        {
            bool returnValue = false;
            string message = "";

            try
            {
                // Set tab attribute default values for Isolation Point
                if (!SetAttributeDefaults(gtKeyObject, out message))
                {
                    TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                    TransactionDEIS.TransactionMessage = message;
                    returnValue = false;
                }

                // Set Isolation Point permanent attribute to "Y"
                ADODB.Recordset isolationPtRS = gtKeyObject.Components.GetComponent(TransactionDEIS.CNO_ISOLATION_POINT_ATTRIBUTES).Recordset;
                if (isolationPtRS.RecordCount > 0)
                {
                    isolationPtRS.MoveFirst();
                    isolationPtRS.Fields["ASSOCIATED_FID"].Value = TransactionDEIS.TransformerProperties.FID;
                    isolationPtRS.Fields["PERMANENT_YN"].Value = "Y";
                }

                // Set Isolation Point feature state attribute to "CLS".
                isolationPtRS = gtKeyObject.Components.GetComponent(TransactionDEIS.CNO_COMMON_ATTRIBUTES).Recordset;
                if (isolationPtRS.RecordCount > 0)
                {
                    isolationPtRS.MoveFirst();
                    isolationPtRS.Fields["FEATURE_STATE_C"].Value = "CLS";
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                TransactionDEIS.TransactionStatus = TransactionDEIS.TRANS_STATUS_FAILED;
                TransactionDEIS.TransactionMessage = "Error setting Isolation Point attributes: " + ex.Message;
                returnValue = false;
            }

            return returnValue;
        }

        // TODO: copied this method from the Service Line plug-in and modified sort by g3e_deleteordinal and to delete all records in recordset. Need to add code to a common library.
        /// <summary>
        /// Method to delete a feature. 
        /// </summary>
        /// <param name="FID">G3E_FID of the feature to delete</param>
        /// <param name="FNO">G3E_FNO of the feature to delete</param>
        /// <param name="message">Message returned if there is an error</param>
        /// <returns>Boolean indicating status</returns>
        internal bool DeleteFeature(int FID, short FNO, out string message)
        {
            try
            {
                message = "";
                IGTKeyObject removeObject = m_DataContext.OpenFeature(FNO, FID);
                Recordset deleteOrder = m_DataContext.MetadataRecordset("G3E_FEATURECOMPS_OPTABLE", "G3E_FNO = " + FNO);
                deleteOrder.Sort = "g3e_deleteordinal";
                deleteOrder.MoveFirst();
                while (!deleteOrder.EOF && !deleteOrder.BOF)
                {
                    for (int i = 0; i < removeObject.Components.Count; i++)
                    {
                        if (removeObject.Components[i].CNO == Convert.ToInt16(deleteOrder.Fields["G3E_CNO"].Value))
                        {
                            if (!removeObject.Components[i].Recordset.EOF && !removeObject.Components[i].Recordset.BOF)
                            {
                                removeObject.Components[i].Recordset.MoveFirst();
                                //removeObject.Components[i].Recordset.Delete();

                                while (!removeObject.Components[i].Recordset.EOF)
                                {
                                    removeObject.Components[i].Recordset.Delete();
                                    removeObject.Components[i].Recordset.MoveNext();
                                }
                            }
                        }
                    }
                    deleteOrder.MoveNext();
                }
                return true;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }
        }

        // TODO: copied this method from the Service Line plug-in. Need to add code to a common library.
        /// <summary>
        /// Gets and sets the attribute defaults for the passed in feature
        /// </summary>
        /// <param name="gtKeyObject">Feature key object to apply defaults.</param>
        /// <returns>Boolean indicating status</returns>
        private bool SetAttributeDefaults(IGTKeyObject gtKeyObject, out string message)
        {
            bool returnValue = false;

            try
            {
                message = "";
                ADODB.Recordset componentRS = null;

                string sql = "select tabattr.g3e_cno, tabattr.g3e_field, tabattr.g3e_default " +
                             "from g3e_dialogattributes_optable tabattr, g3e_dialogs_optable dialog " +
                             "where dialog.g3e_fno = ? and tabattr.g3e_dtno = dialog.g3e_dtno " +
                             "and dialog.g3e_type = 'Placement' and tabattr.g3e_default is not null " +
                             "order by tabattr.g3e_cno";

                Recordset attrDefaultsRS = m_DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, gtKeyObject.FNO);

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
    }
}
