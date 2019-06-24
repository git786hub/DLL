// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: fiManholeNetworkManagement.cs
// 
//  Description:This interface sets default values for Network Managed and Network Restricted attributes on Manholes when initially placed.
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  26/04/2018          Sithara                  
// ======================================================
using System;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiManholeNetworkManagement : FIBase
    {
        public override void Execute()
        {
            try
            {
                fiValidation fiValidate = new fiValidation(DataContext);
                bool validation = fiValidate.ValidateFI();

                if (validation)
                {
                    ManholeOperations Operations = new ManholeOperations();
                    Operations.UpdateManholeAttributes(GetActiveComponent(), validation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of Manhole NetworkManagement FI." + Environment.NewLine + ex.Message,
                  "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
