using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public static class AccessRelativeComponent
    {
        #region Private Members 
        private const int ALL_OCCTYPE = 0;
        private const int LASTPLACED_OCCTYPE = 1;
        private const int SAMEASACTIVE_OCCTYPE = -1;
        #endregion

        #region public Methods
        public static IGTGraphicComponent RetrieveRelativeComponent(string relativeComponentArgument, IGTGraphicComponents graphicComponents, IGTGraphicComponent graphicComponentBeingPlaced)
        {
            IGTGraphicComponent relativeComponent = null;
            char[] delimiterChars = { '/' };

            //Convert the placement technique argument that contains the relative component from
            //a variant to a string
            if (String.IsNullOrEmpty(relativeComponentArgument))
            {
                MessageBox.Show("Error: Unable to get relative component information. Possible cause: Relative component is not defined.", "G / Technology");
            }
            else
            {
                //Parse the relative component argument into 4 distinct key ids.  The format of the
                //argument is FNO/FIDX/CNO/Occurrence type.  To parse it, use the split function and the
                //"/" delimiter.  Split will return an array substrings
                string[] relativeComponentArgumentIDs = relativeComponentArgument.Split(delimiterChars);

                //Convert the substrings to integers and save as key ids for a later comparison.
                int FNO = Convert.ToInt32(relativeComponentArgumentIDs[0]);
                int FID = Convert.ToInt32(relativeComponentArgumentIDs[1]);
                int CNO = Convert.ToInt32(relativeComponentArgumentIDs[2]);
                int occurenceType = Convert.ToInt32(relativeComponentArgumentIDs[3]);
                int lastCID = -1;

                //Use the key ids saved above to find this relative component in the GraphicComponents
                //collection.
                foreach (IGTGraphicComponent graphicComponent in graphicComponents)
                {
                    //First look for a match on FNO, FeatureIndex, and CNO
                    if ((graphicComponent.FNO == FNO) &&
                      (graphicComponent.CNO == CNO))
                    {
                        //Now that we've found a match, find the specified occurrence type.  The
                        //occurrence type identifies which instance to use when multiple instances
                        //of a CNO exist.
                        switch (occurenceType)
                        {
                            case ALL_OCCTYPE:
                                //Not applicable for VBAStartLinearAtPointSPT
                                MessageBox.Show("Error in AccessRelativeComponent.RetrieveRelativeComponent: Occurrence type of 'All' not supported by this placement technique", "G / Technology");
                                break;
                            case LASTPLACED_OCCTYPE:
                                //This finds the occurrence with the greatest CID value.
                                if (graphicComponent.CID > lastCID)
                                {
                                    //store last cid value
                                    lastCID = graphicComponent.CID;
                                    relativeComponent = graphicComponent;
                                }
                                break;
                            case SAMEASACTIVE_OCCTYPE:
                                //This finds the occurrence with the same CID value as the CID of
                                //the component a technique is currently placing.
                                if (graphicComponent.CID == graphicComponentBeingPlaced.CID)
                                {
                                    relativeComponent = graphicComponent;
                                }
                                break;
                            default:
                                relativeComponent = graphicComponent;
                                break;
                        }
                    }
                }
                if (relativeComponent == null)
                {
                    MessageBox.Show("Error in AccessRelativeComponent.RetrieveRelativeComponent: Failed to find the relative component " + relativeComponentArgument, "G/Technology");
                }
            }
            return relativeComponent;
        }

        public static IGTPoint RetrieveRelativePoint(IGTGraphicComponent relativeComponent)
        {
            //initialize
            IGTGeometry relativeGeometry = relativeComponent.Geometry;
            IGTPoint relativePoint = GTClassFactory.Create<IGTPoint>();
            relativePoint.X = ((IGTOrientedPointGeometry)relativeGeometry).Origin.X;
            relativePoint.Y = ((IGTOrientedPointGeometry)relativeGeometry).Origin.Y;
            relativePoint.Z = ((IGTOrientedPointGeometry)relativeGeometry).Origin.Z;
            return relativePoint;
        }
        #endregion
    }
}

