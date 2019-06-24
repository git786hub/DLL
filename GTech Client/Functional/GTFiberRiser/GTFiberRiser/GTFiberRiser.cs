// =======================================================================================================
//  File Name: GTFiberRiser.cs
// 
//  Description: Inherent fiber Branch enclosure interface to configure fiber Riser in FOW
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  30/03/2018          Pramod                      Implemented base class methods
// ====
using Intergraph.FiberManagement.GTechnology;
using Intergraph.GTechnology.API;

namespace Intergraph.Oncor.FiberManagement
{
    public class GTFiberRiser: GTFiberBranchEnclosure
    {
      
        public GTFiberRiser(GTDataProvider provider, GTFiberDataEntityIdentityDescriptor identity, IGTKeyObject key) : base(provider, identity, key)
        {
        }

        public override string Description
        {
            get { return "Fiber Riser"; }
        }
    }
}
