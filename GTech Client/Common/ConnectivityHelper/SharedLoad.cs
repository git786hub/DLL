// ===================================================
//  Copyright 2018 Hexagon
//  File Name: SharedLoads.cs
// 
// Description:   
//  This class is responsible to get all the Shared Loads of an Afftected feature

//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  28/06/2018          Shubham                     Initial Creation
// ======================================================
using System;
using System.Collections.Generic;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
  public class SharedLoads : RelatedFeatureTypes
  {
    public SharedLoads()
    {
    }

    private IGTKeyObject m_oAffectedObject = null;
    public override IGTKeyObject AffectedFeature { get { return m_oAffectedObject; } set { m_oAffectedObject = value; } }

    public override List<IGTKeyObject> RelatedFeaturesList { get => throw new NotImplementedException(); }

    /// <summary>
    /// Method to get all the Shared Loads w.r.t.affected feature
    /// </summary>
    /// <returns></returns>
    public override List<IGTKeyObject> GetRelatedFeaturesForFeatureTypes()
    {
      try
      {
        List<IGTKeyObject> oSharedLoads = new List<IGTKeyObject>();

        FeatureConnectivityAttributes oAffectedConnectivity = GetConnectivityAttributes(AffectedFeature);
        FeatureConnectivityAttributes oRelatedConnectivity = null;

        NodeType Node = oAffectedConnectivity.UpstreamNode == 1 ? NodeType.Node2 : NodeType.Node1;

        // If affected downstream node is not set, then there is no load to share; return empty list.
        if(oAffectedConnectivity.DownStreamNodeValue == 0)
        {
          return oSharedLoads;
        }

        // Iterate through related features to identify shared loads.
        List<IGTKeyObject> oRelated = GetAllRelatedFeatures(Node);
        foreach(IGTKeyObject item in oRelated)
        {
          oRelatedConnectivity = GetConnectivityAttributes(item);

          // If downstream node is not set, then it can't be a shared load.
          if(oRelatedConnectivity.DownStreamNodeValue == 0) { continue; }

          // If nodes are equal (i.e.- single-node feature) then treat as downstream, not shared load.
          if(oRelatedConnectivity.DownStreamNodeValue == oRelatedConnectivity.UpstreamNodeValue) { continue; }

          if(oRelatedConnectivity.DownStreamNodeValue == oAffectedConnectivity.DownStreamNodeValue
              && oRelatedConnectivity.UpstreamNodeValue != oAffectedConnectivity.UpstreamNodeValue)
          {
            oSharedLoads.Add(item);
          }
        }

        return oSharedLoads;
      }
      catch(Exception)
      {
        throw;
      }
    }

    public override List<char> GetRelatedPhases()
    {
      throw new NotImplementedException();
    }

    public override bool IsAffectedDownstreamOfRelated(IGTKeyObject p_relatedObject)
    {
      throw new NotImplementedException();
    }
  }
}
