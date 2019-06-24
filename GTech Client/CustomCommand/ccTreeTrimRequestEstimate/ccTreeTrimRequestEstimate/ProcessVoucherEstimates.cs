// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: ProcessVoucherEstimates.cs
//
//  Description:    Class to Process the Voucher Estimates.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  15/03/2018          Prathyusha                  Created 
// ======================================================
using System;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    class ProcessVoucherEstimates
    {
        #region Methods
        /// <summary>
        /// Method to Update the Vouchers attributes of the placed TreeTrimming feature
        /// </summary>
        /// <param name="p_newTreeTrimmingfeature">Current placed feature</param>
        /// <param name="p_voucherAcctValue">Prime account and Sub account values selected by user from form</param>
        public void UpdateVoucherAttributes(IGTKeyObject p_newTreeTrimmingfeature, string p_voucherAcctValue)
        {
            try
            {
                Recordset voucherAttributesRs = p_newTreeTrimmingfeature.Components.GetComponent(19001).Recordset;
                if (voucherAttributesRs != null && voucherAttributesRs.RecordCount > 0)
                {
                    voucherAttributesRs.MoveFirst();
                    if (!String.IsNullOrEmpty(p_voucherAcctValue))
                    {
                        voucherAttributesRs.Fields["FERC_PRIME_ACCT"].Value = p_voucherAcctValue.Split(',')[0];
                        voucherAttributesRs.Fields["FERC_SUB_ACCT"].Value = p_voucherAcctValue.Split(',')[1];
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to process mail to the Recipients using the local email client
        /// </summary>
        /// <param name="p_recipientName">Name of the recipient, keyed in by user in form.</param>
        /// <param name="p_recipientEmail">Email address of the recipient, keyed in by user in form</param>
        /// <param name="p_vegMngSheetTemplate">Name of the copy of the Vegetation management HTML sheet template</param>
        /// <param name="p_plotPDFName">Exported Plot PDF Name</param>
        /// <param name="p_activeJob">Active job</param>
        public void ProcessEmail(string p_recipientName, string p_recipientEmail, string p_vegMngSheetTemplate, string p_plotPDFName, string p_activeJob)
        {
            try
            {
                MailUtility.MailToVegetationManagement(p_recipientName, p_recipientEmail, p_vegMngSheetTemplate, p_plotPDFName, p_activeJob);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
