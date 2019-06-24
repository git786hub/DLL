//----------------------------------------------------------------------------+
//        Class: WriteBackCompletedEventArgs
//  Description: This contains the event argument class necessary to process Writeback.
//        Class: UpdateStatusCompleteEventArgs
//  Description: This contains the event argument class necessary to process Update Job Status.
//                                                                  
//----------------------------------------------------------------------------+
//          $Author:: Shubham Agarwal                                       $
//          $Date:: 25/03/18                                                $
//          $Revision:: 1                                                   $
//----------------------------------------------------------------------------+
//    $History:: EventArgs.cs                     $
// 
// *****************  Version 1  *****************
// User: sagarwal     Date: 25/03/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using System;
using System.ComponentModel;

namespace CustomWriteBackLibrary
{

    /// <summary>
    /// This event argument class is responsible for passing the information back to the caller.
    /// The valid values for the Status are FAILURE or SUCCESS
    /// </summary>
    public class WriteBackCompletedEventArgs :
          AsyncCompletedEventArgs
    {
        public string Status { get; set; }

        public WriteBackCompletedEventArgs(
            string status,
            Exception e,
            bool canceled,
            object state)
            : base(e, canceled, state)
        {
            this.Status = status;
        }
    }

   /// This event argument class is responsible for passing the information back to the caller.
    /// The valid values for the Status are FAILURE or SUCCESS
    public class UpdateStatusCompleteEventArgs :
         AsyncCompletedEventArgs
    {
        public string Status { get; set; }

        public UpdateStatusCompleteEventArgs(
            string status,
            Exception e,
            bool canceled,
            object state)
            : base(e, canceled, state)
        {
            this.Status = status;
        }
    }
}
