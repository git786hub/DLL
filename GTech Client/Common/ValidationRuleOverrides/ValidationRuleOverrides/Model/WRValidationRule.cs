using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.Model
{
    public class WRValidationRule
    {
        private string ruleID;
        private string ruleNM;
        private string ruleMsg;

        /// <summary>
        /// Validation Rule ID
        /// </summary>
        public string Rule_ID { get => ruleID; set => ruleID = value; }
        /// <summary>
        /// Validation rule Name
        /// </summary>
        public string Rule_NM { get => ruleNM; set => ruleNM = value; }

        /// <summary>
        /// validation Rule Message
        /// </summary>
        public string Rule_Msg { get => ruleMsg; set => ruleMsg = value; }
    }
}
