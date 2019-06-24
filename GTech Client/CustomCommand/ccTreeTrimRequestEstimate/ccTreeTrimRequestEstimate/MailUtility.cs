// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: MailUtility.cs
//
//  Description:   Sends mail Vegetation Management WR Activity Sheet and exported PDF to Recipient given by user.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  15/03/2018          Prathyusha                  Created 
// ======================================================
using System;
using System.Reflection;
using System.Net.Mail;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace GTechnology.Oncor.CustomAPI
{ 
    public static class MailUtility
    {
        #region Methods
        //Extension method for MailMessage to save to a file on disk
        private static void Save(this MailMessage p_message, string p_filename, bool p_addUnsentHeader = true)
        {
            using (var filestream = File.Open(p_filename, FileMode.Create))
            {
                if (p_addUnsentHeader)
                {
                    var binaryWriter = new BinaryWriter(filestream);
                    //Write the Unsent header to the file so the mail client knows this mail must be presented in "New message" mode
                    binaryWriter.Write(System.Text.Encoding.UTF8.GetBytes("X-Unsent: 1" + Environment.NewLine));
                }

                var assembly = typeof(SmtpClient).Assembly;
                var mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");

                // Get reflection info for MailWriter contructor
                var mailWriterContructor = mailWriterType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(System.IO.Stream) }, null);

                // Construct MailWriter object with our FileStream
                var mailWriter = mailWriterContructor.Invoke(new object[] { filestream });

                // Get reflection info for Send() method on MailMessage
                var sendMethod = typeof(MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic);

                sendMethod.Invoke(p_message, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { mailWriter, true, true }, null);

                // Finally get reflection info for Close() method on our MailWriter
                var closeMethod = mailWriter.GetType().GetMethod("Close", BindingFlags.Instance | BindingFlags.NonPublic);

                // Call close method
                closeMethod.Invoke(mailWriter, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { }, null);
            }
        }
        /// <summary>
        /// Sends mail Vegetation Management WR Activity Sheet and exported PDF to Recipient given by user.
        /// </summary>
        /// <param name="p_recipientName">Name of the recipient, keyed in by user in form.</param>
        /// <param name="p_recipientEmail">Email address of the recipient, keyed in by user in form</param>
        /// <param name="p_recipientEmail">Email address of the recipient, keyed in by user in form</param>
        /// <param name="p_vegMngSheetTemplate">Name of the copy of the Vegetation management HTML sheet template</param>
        /// <param name="p_plotPDFName">Exported Plot PDF Name</param>
        /// <param name="p_activeJob">Active job</param>
        public static void MailToVegetationManagement(string p_recipientName, string p_recipientEmail,string p_vegMngSheetTemplate,string p_plotPDFName,string p_activeJob)
        {
            try
            {
                var mailMessage = new MailMessage(System.DirectoryServices.AccountManagement.UserPrincipal.Current.EmailAddress, p_recipientEmail);
                mailMessage.Subject = "Request for Tree Trimming Estimate - Work Request " + p_activeJob;
                mailMessage.IsBodyHtml = false;
                mailMessage.Body = p_recipientName + ",";

                if (p_vegMngSheetTemplate == null ? false : File.Exists(p_vegMngSheetTemplate))
                {
                    mailMessage.Attachments.Add(new Attachment(p_vegMngSheetTemplate));

                    DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetTempPath());
                    var files = directoryInfo.GetFiles("*.png").Where(p => p.Name.StartsWith("VegetationManagementEstimate_Sheet"));
                    foreach (FileInfo s in files)
                    {
                        mailMessage.Attachments.Add(new Attachment(s.FullName));
                    }
                }
                if (p_plotPDFName == null ? false : File.Exists(Path.Combine(Path.GetTempPath(), p_plotPDFName)))
                {
                    mailMessage.Attachments.Add(new Attachment(Path.Combine(Path.GetTempPath(), p_plotPDFName)));
                }
                var filename = Path.Combine(Path.GetTempPath(), "VegetationManagement.eml");
                mailMessage.Save(filename);
                Process.Start(filename);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
