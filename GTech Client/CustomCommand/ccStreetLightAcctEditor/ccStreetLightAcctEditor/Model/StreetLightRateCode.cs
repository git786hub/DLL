using GTechnology.Oncor.CustomAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.Model
{
    public class StreetLightRateCode
    {
        private string rateCode;
        private string prevRateCodeVal;// this is used for while updating Rate code
        EntityMode entityState = EntityMode.Add;// this is used for to identify the state of entity while performing CRUD operations.

        public string RateCode { get => rateCode; set { if (rateCode != value && entityState != EntityMode.Update) { prevRateCodeVal = rateCode; rateCode = value; } if (entityState == EntityMode.Review) entityState = EntityMode.Update; } }
        public string PrevRateCodeVal { get => prevRateCodeVal; }
        public EntityMode EntityState { get => entityState; set => entityState = value; }

    }
}
