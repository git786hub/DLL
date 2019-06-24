//----------------------------------------------------------------------------+
//  Class: ValidateFeatureConditions
//  Description: 
//		Class to carry out checks for Upstream and DownStream side of affected and related feature and validate
//      if the Protective Device ID matches per the properties of the features 
//----------------------------------------------------------------------------+
//  $Author::   Shubham Agarwal                                                       $
//  $Date::     30/07/17                                                                $
//  $Revision:: 1                                                                   $
//----------------------------------------------------------------------------+
//  $History:: ValidateFeatureConditions.cs                                             $
// 
// *****************  Version 1  *****************
//  User: sagarwal     Date: 30/04/18    Initial Creation
//----------------------------------------------------------------------------+

namespace GTechnology.Oncor.CustomAPI
{
public  class ValidateFeatureConditions
    {
        /// <summary>
        /// Method to return true or false depending upon whether Related Feature is Down Stream of the affected feature
        /// </summary>
        /// <param name="p_affectedFeature"></param>
        /// <param name="p_relatedFeature"></param>
        /// <returns></returns>
        public bool? IsRelatedFeatureDownstream(FeatureProperties p_affectedFeature, FeatureProperties p_relatedFeature)
        {
            bool? bReturn = null;

            if ((p_relatedFeature.UpstreamNode == p_affectedFeature.UpstreamNode) || (p_relatedFeature.DownStreamNode == p_affectedFeature.DownStreamNode)) //This is the case of parallel branches where both are connected to U/S node
            {
                bReturn = null;
            }
          
            else if (p_relatedFeature.DownStreamNode != 0 && p_relatedFeature.DownStreamNode == p_affectedFeature.UpstreamNode) //Affected Feature is D/S of related feature
            {
                bReturn = false;
            }
            else if (p_relatedFeature.UpstreamNode != 0 && p_relatedFeature.UpstreamNode == p_affectedFeature.DownStreamNode) //Affected Feature is U/S of related feature
            {
                bReturn = true;
            }
                           
            return bReturn;
        }     
    }
}
