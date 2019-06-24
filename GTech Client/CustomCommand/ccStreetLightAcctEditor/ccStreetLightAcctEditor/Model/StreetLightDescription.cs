using GTechnology.Oncor.CustomAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.Model
{
    public class StreetLightDescription
    {
        int description_id;
        string description;
        DateTime msla_date;
        EntityMode entityState = EntityMode.Add;// this is used for to identify the state of entity while performing CRUD operations.

        public int DescriptionID { get => description_id; set => description_id = value; }
        public string DESCRIPTION { get => description; set { description = value; UpdateEntityState(); } }
        public DateTime MSLA_Date { get => msla_date; set { msla_date = value; UpdateEntityState(); } }

        public EntityMode EntityState { get => entityState; set => entityState = value; }

        private void UpdateEntityState()
        {
            if (this.EntityState == EntityMode.Review)
            {
                this.EntityState = EntityMode.Update;
            }

        }
    }
}
