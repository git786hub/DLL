// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: customWorkPointCreator.cs
// 
// Description:  Custom class that holds method(s) to create work point fetaure.

//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  28/05/2018          Hari                        Initial sources
//  21/06/2018          Hari                        Adjusted code as per JIRA ONCORDEV-1761 (Work Point symbol should be offset from the associated structure)
//  16/01/2019          Pramod                      Calculated Leader Line end point 70% of the way toward the structure from center of the Work Point
//  26/03/2019          Hari                        Adjusted CU label Y-offset - ALM-1792, 1835, 1866, 1935 JIRA 2444
// ======================================================

using ADODB;
using Intergraph.GTechnology.API;
using System;

namespace GTechnology.Oncor.CustomAPI
{
    public class customWorkPointCreator
    {
        IGTDataContext m_dataContext;
        double m_locationX;
        double m_locationY;
       
        public customWorkPointCreator(IGTDataContext p_DataContext, double p_LocationX, double p_LocationY)
        {
            m_dataContext = p_DataContext;
            m_locationX = p_LocationX;
            m_locationY = p_LocationY;
        }

        /// <summary>
        /// Method to create work point feature
        /// </summary>
        /// <returns>Work point feature IGTKeyObject</returns>
        public IGTKeyObject CreateWorkPointFeature()
        {
            try
            {
                IGTKeyObject workPointFeature = m_dataContext.NewFeature(191);
                IGTComponents workPointComponents = workPointFeature.Components;
                int wpOffsetX = 0;
                int wpOffsetY = 0;
                GetWorkPointOffset(ref wpOffsetX, ref wpOffsetY);

                foreach (IGTComponent component in workPointComponents)
                {
                    Recordset tempRs = component.Recordset;
                    if (tempRs != null)
                    {
                        if (tempRs.RecordCount > 0)
                        {
                            if (component.CNO == 19102) // Work point symbol
                            {

                                IGTOrientedPointGeometry orientedPtGeometry = GTClassFactory.Create<IGTOrientedPointGeometry>();
                                IGTPoint point = GTClassFactory.Create<IGTPoint>();
                                point.X = m_locationX - wpOffsetX;
                                point.Y = m_locationY - wpOffsetY;
                                orientedPtGeometry.Origin = point;
                                component.Geometry = orientedPtGeometry;
                            }
                            if (component.CNO == 19103) // Work point Label
                            {
                                IGTTextPointGeometry gTTextPointGeometry = GTClassFactory.Create<IGTTextPointGeometry>();
                                IGTPoint gTPoint = GTClassFactory.Create<IGTPoint>();
                                gTPoint.X =  m_locationX - wpOffsetX;
                                gTPoint.Y =  m_locationY - wpOffsetY;
                                gTTextPointGeometry.Origin = gTPoint;
                                gTTextPointGeometry.Alignment = GTAlignmentConstants.gtalBottomCenter;
                                component.Geometry = gTTextPointGeometry;
                                component.Recordset.Update();
                            }
                        }
                    }
                }

                // Leader Line should end at a point 70% of the way toward the structure from center of the Work Point.
                m_locationX = m_locationX - wpOffsetX * .3;
                m_locationY = m_locationY - wpOffsetY * .3;

                CreateLeaderLine(workPointFeature,  m_locationX, m_locationY);
                return workPointFeature;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DeleteWorkPointCULabel(IGTKeyObject p_WPKeyObject)
        {
            try
            {
                IGTComponent oCUComponent = p_WPKeyObject.Components.GetComponent(19105);
                if (oCUComponent != null)
                {
                    if (oCUComponent.Recordset != null)
                    {
                        if (oCUComponent.Recordset.RecordCount > 0)
                        {
                            oCUComponent.Recordset.MoveFirst();
                            while (oCUComponent.Recordset.EOF == false)
                            {
                                oCUComponent.Recordset.Delete();
                                if (oCUComponent.Recordset.EOF == false)
                                {
                                    oCUComponent.Recordset.MoveNext();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CreateLeaderLine(IGTKeyObject p_WPKeyObject,double p_X1, double p_Y2)
        {
            try
            {
                IGTComponent oCUComponent = p_WPKeyObject.Components.GetComponent(42);

                if (oCUComponent != null)
                {
                    if (oCUComponent.Recordset != null)
                    {
                        if (oCUComponent.Recordset.RecordCount == 0)
                        {
                            oCUComponent.Recordset.AddNew();
                            oCUComponent.Recordset.Fields["G3E_FID"].Value = p_WPKeyObject.FID;
                            oCUComponent.Recordset.Fields["G3E_FNO"].Value = 191;
                            oCUComponent.Recordset.Fields["G3E_CNO"].Value = 42;

                            IGTPolylineGeometry gTPolylineGeometry = GTClassFactory.Create<IGTPolylineGeometry>();
                            IGTPoint gTPoint1 = GTClassFactory.Create<IGTPoint>();
                            IGTPoint gTPoint2 = GTClassFactory.Create<IGTPoint>();

                            gTPoint1.X = ((IGTOrientedPointGeometry)(p_WPKeyObject.Components.GetComponent(19102).Geometry)).FirstPoint.X; 
                            gTPoint1.Y = ((IGTOrientedPointGeometry)(p_WPKeyObject.Components.GetComponent(19102).Geometry)).FirstPoint.Y;

                            gTPoint2.X = p_X1;
                            gTPoint2.Y = p_Y2;
                          
                            gTPolylineGeometry.Points.Add(gTPoint1);
                            gTPolylineGeometry.Points.Add(gTPoint2);

                            oCUComponent.Geometry = gTPolylineGeometry;
                            oCUComponent.Recordset.Update();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }           
        }
        public void SynchronizeWPCuLabel(IGTKeyObject p_WPKeyObject)
        {
            try
            {
                int wpOffsetX = 0;
                int wpOffsetY = 0;
                GetWorkPointOffset(ref wpOffsetX, ref wpOffsetY);

                double Owner_X = m_locationX;
                double Owner_Y = m_locationY;

                IGTComponent CuLabelcomponent = p_WPKeyObject.Components.GetComponent(19105);

                if (CuLabelcomponent.Recordset.RecordCount == 0)
                {
                    CuLabelcomponent.Recordset.AddNew();

                    CuLabelcomponent.Recordset.Fields["G3E_FID"].Value = p_WPKeyObject.FID;
                    CuLabelcomponent.Recordset.Fields["G3E_FNO"].Value = 191;
                    CuLabelcomponent.Recordset.Fields["G3E_CNO"].Value = 19105;

                    IGTTextPointGeometry gTTextPointGeometry = GTClassFactory.Create<IGTTextPointGeometry>();
                    IGTPoint gTPoint = GTClassFactory.Create<IGTPoint>();
                    gTPoint.X = m_locationX - wpOffsetX * (1.6);
                    gTPoint.Y = Owner_Y - wpOffsetY * (1.3);

                    gTTextPointGeometry.Origin = gTPoint;
                    gTTextPointGeometry.Alignment = GTAlignmentConstants.gtalTopLeft;
                    CuLabelcomponent.Geometry = gTTextPointGeometry;
                    CuLabelcomponent.Recordset.Update();
                }               
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to get X and Y offset for the WP creation
        /// </summary>
        /// <param name="p_offsetX"></param>
        /// <param name="p_offsetY"></param>
        private void GetWorkPointOffset(ref int p_offsetX, ref int p_offsetY)
        {
            ADODB.Recordset rs = m_dataContext.OpenRecordset("Select PARAM_VALUE from SYS_GENERALPARAMETER where PARAM_NAME = 'WP_OFFSET_X' AND SUBSYSTEM_NAME = 'WorkPointCreation'", CursorTypeEnum.adOpenDynamic,LockTypeEnum.adLockOptimistic,(int) ADODB.CommandTypeEnum.adCmdText,new object[1]);
            if (rs !=null)
            {
                if (rs.RecordCount>0)
                {
                    rs.MoveFirst();
                    p_offsetX = Convert.ToInt32(rs.Fields[0].Value);
                }
            }

            rs = m_dataContext.OpenRecordset("Select PARAM_VALUE from SYS_GENERALPARAMETER where PARAM_NAME = 'WP_OFFSET_Y' AND SUBSYSTEM_NAME = 'WorkPointCreation'", CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, new object[1]);
            if (rs != null)
            {
                if (rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    p_offsetY = Convert.ToInt32(rs.Fields[0].Value);
                }
            }

        }
    }
}
