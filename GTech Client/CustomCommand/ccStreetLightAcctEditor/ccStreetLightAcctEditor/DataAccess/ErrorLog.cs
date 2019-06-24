using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.DataAccess
{
    /// <summary>
    /// this is to log Validation Errors encounter while persisting Street Light Account and Value Lists 
    /// </summary>
   public class ErrorLog
    {
        public string ErrorIn { get; set; }
        public string ErrorMessage { get; set; }
    }
}
