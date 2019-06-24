// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: ManholeOperations.cs
// 
//  Description:Updates Manhole Network Managed and Network Restricted attributes on .
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  26/04/2018          Sithara                  
// ======================================================
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class ManholeOperations
    {
        public void UpdateManholeAttributes(IGTComponent p_gTComponent,bool p_PassValidation)
        {
            try
            {
                p_gTComponent.Recordset.MoveFirst();
                if (p_PassValidation)
                {
                    p_gTComponent.Recordset.Fields["NETWORK_MANAGED_YN"].Value = "Y";
                    p_gTComponent.Recordset.Fields["NETWORK_RESTRICTED_YN"].Value = "Y";
                }
                else
                {
                    p_gTComponent.Recordset.Fields["NETWORK_MANAGED_YN"].Value = "N";
                    p_gTComponent.Recordset.Fields["NETWORK_RESTRICTED_YN"].Value = "N";
                }

                p_gTComponent.Recordset.Update();                
            }
            catch
            {
                throw;
            }
        }
    }
}
