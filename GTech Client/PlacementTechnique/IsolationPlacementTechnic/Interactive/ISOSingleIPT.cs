//----------------------------------------------------------------------------+
//  Class: ISOSingleIPT
//  Description: Interactive Placement technic for Isosingle,IsoDual,ByPass and ELBOW Scenario
//----------------------------------------------------------------------------+
//   $Author:: Uma Avote                              $
//   $Date:: 15/04/18                                 $
//   $Revision:: 1                                    $
//----------------------------------------------------------------------------+
//    $History:: ISOSingleIPT.cs                     $
// 
// *****************  Version 1  *****************
// User: Uma Avote     Date: 15/04/18   Time: 10:00  Desc : Created
//     : Pramod        07-Sept-2018     Add property to the GTApplication session to hold the located linear G3eFno value and property used ny ByPassSPT Placement Technique
//----------------------------------------------------------------------------+
using System;
using System.Windows.Forms;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using System.Linq;
using System.Collections.Generic;

namespace GTechnology.Oncor.CustomAPI
{
    public class ISOSingleIPT : CommonIPTCode
    {
        #region Private Members       
        IGTOrientedPointGeometry m_gtPointGeometry = null;
        bool bSilent = false;
        bool flag = false;
        List<int> linearFNOs;
        #endregion

        #region IGTPlacementTechnique Members  
        public override void MouseClick(IGTMapWindow MapWindow, Intergraph.GTechnology.API.IGTPoint UserPoint, int Button, int ShiftState, Intergraph.GTechnology.API.IGTDDCKeyObjects LocatedObjects, Intergraph.GTechnology.API.IGTPlacementTechniqueEventMode EventMode)
        {
            try
            {
                if (!bSilent)
                {
                    bool pointSelected = false;
                    bool lineSelected = false;
                    IGTPoint m_gtOrientedPoint = null;
                    //verify if the located object is point feature
                    for (int i = 0; i < LocatedObjects.Count; i++)
                    {
                        if (LocatedObjects[i].Geometry.Type == "OrientedPointGeometry")
                        {
                            pointSelected = true;
                            break;
                        }
                        else
                        {
                            for (int indx = 0; indx < linearFNOs.Count; indx++)
                            {
                                if (linearFNOs[indx] == LocatedObjects[i].FNO)
                                {
                                    if (!lineSelected)
                                    {
                                        //Get point and angle of the located linear feature
                                        m_gtOrientedPoint = GetPointAndAngle(LocatedObjects[i].Geometry, UserPoint);
                                        lineSelected = true;
                                        //Add Property "LinearFno" to Session to hold value located Linear fno and used by ByPassSPT Placement Technique
                                        //ByPassSPT Placement Technique get located Linear Fno 
                                        AddProperty("LinearFno", linearFNOs[indx]);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if ((!pointSelected) && (m_gtOrientedPoint != null))
                    {
                        pointSelected = true;
                        m_GComp.Geometry.FirstPoint.X = m_gtOrientedPoint.X;
                        m_GComp.Geometry.FirstPoint.Y = m_gtOrientedPoint.Y;
                        m_gtPointGeometry = (IGTOrientedPointGeometry)m_GComp.Geometry;
                        m_gtPointGeometry.Origin = m_gtOrientedPoint;
                        //set the orientation of the point feature same as located linear feature
                        m_gtPointGeometry.Orientation = VectorByAngle(ang);
                        m_PTHelper.SetGeometry(m_gtPointGeometry);
                        m_PTHelper.EndPlacement();
                        flag = true;
                    }
                    else
                    {
                        MessageBox.Show("Please click on related Primary Conductor feature.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please click on related Primary Conductor feature. \n" + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AbortPlacement();
            }
            finally
            {
                if (flag)
                {
                    Exitcommand();
                }
            }
        }
        public override void StartPlacement(IGTPlacementTechniqueHelper PTHelper, Intergraph.GTechnology.API.IGTKeyObject KeyObject, Intergraph.GTechnology.API.IGTKeyObjects KeyObjectCollection)
        {
            try
            {
                m_PTHelper = PTHelper;
                IGTKeyObject m_KeyObject = KeyObject;
                IGTKeyObjects m_KeyObjectCollection = KeyObjectCollection;
                if (bSilent)
                {
                    m_PTHelper.StatusBarPromptsEnabled = false;
                    m_PTHelper.StartPlacement(m_KeyObject, m_KeyObjectCollection);
                }
                else
                {
                    //Get the Argument value
                    linearFNOs = new List<int>(Array.ConvertAll(m_GComp.Arguments.GetArgument(0).ToString().Split(','), int.Parse));
                    m_PTHelper.StatusBarPromptsEnabled = true;
                    m_PTHelper.StartPlacement(m_KeyObject, m_KeyObjectCollection);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Isolation Scenario PlacementTechnic Interface - StartPlacement  \n" + ex.Message, "G /Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    #endregion
}
