using GTechnology.Oncor.CustomAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.Model
{
    public class StreetLightOwner
    {
        string ownerCode;
        string ownerName;
        string prevOwnerCodeVal;
        EntityMode entityState = EntityMode.Add;// this is used for to identify the state of entity while performing CRUD operations.

        public string OwnerCode
        {
            get => ownerCode;
            set { if (ownerCode != value && entityState!=EntityMode.Update) { prevOwnerCodeVal = ownerCode; ownerCode = value; } if (entityState == EntityMode.Review) entityState = EntityMode.Update; }

        }
        public string OwnerName { get => ownerName; set { ownerName = value; if (entityState == EntityMode.Review) entityState = EntityMode.Update; } }
        public string PrevOwnerCodeVal { get => prevOwnerCodeVal; }
        public EntityMode EntityState { get => entityState; set => entityState = value; }

    }
}
