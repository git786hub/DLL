using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.Model
{
    public class WRValidationOverride
    {
        private int id;
        private string ruleID;
        private string ruleNm;
        private int g3eFno;
        private int g3eFid;
        private string featureClass;
        private string errorMsg;
        private string overrideComments;

        /// <summary>
        /// Validation Overrides ID unique identifier
        /// </summary>
        public int ID { get => id; set => id = value; }
        /// <summary>
        /// Validation Rule ID
        /// </summary>
        public string Rule_ID { get => ruleID; set => ruleID = value; }
        /// <summary>
        /// Validation Rule Name
        /// </summary>
        public string Rule_NM { get => ruleNm; set => ruleNm = value; }

        /// <summary>
        /// Feature Number
        /// </summary>
        public int G3e_Fno { get => g3eFno; set => g3eFno = value; }

        /// <summary>
        /// Feature UserName
        /// </summary>
        public string FeatureClass { get => featureClass; set => featureClass = value; }

        /// <summary>
        /// Feature Instance G3e_Fid
        /// </summary>
        public int G3e_Fid { get => g3eFid; set => g3eFid = value; }

        /// <summary>
        /// Job Validation Error Description
        /// </summary>
        public string Error_Msg { get => errorMsg; set => errorMsg = value; }

        /// <summary>
        /// Justification for Validation Errors
        /// </summary>
        public string Override_Comments { get => overrideComments; set => overrideComments = value; }
    }
}