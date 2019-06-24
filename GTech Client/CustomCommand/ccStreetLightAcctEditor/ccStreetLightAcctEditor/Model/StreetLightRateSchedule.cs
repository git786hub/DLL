using GTechnology.Oncor.CustomAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.Model
{
    public class StreetLightRateSchedule
    {
        private string rateSchedule;
        private string prevRateScheduleVal;// this is used for while updating Rate Schedule
        EntityMode entityState = EntityMode.Add;// this is used for to identify the state of entity while performing CRUD operations.

        public string RateSchedule
        {
            get => rateSchedule;
            set
            {
                if (rateSchedule != value && entityState != EntityMode.Update)
                { prevRateScheduleVal = rateSchedule; rateSchedule = value; }
                if (entityState == EntityMode.Review) entityState = EntityMode.Update;
            }
        }
        public string PrevRateScheduleVal { get => prevRateScheduleVal; }
        public EntityMode EntityState { get => entityState; set => entityState = value; }

    }
}
