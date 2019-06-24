using GTechnology.Oncor.CustomAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.Model
{
    public class StreetLightAccount
    {
        string esi_location="";
        bool billable;
        string description_id="";
        string owner_code;
        string wattage="";
        string lamp_type="";
        string luminare_style="";
        string rate_schedule="";
        string rate_code;
        int previous_count;
        int current_count;
        int threshold_percent;
        string threshold_state="NEW";
        bool threshold_override;
        string run_date;
        string modified_by;
        string modified_date;
        string creation_date;
        string boundary_class;
        string boundary_id;
        bool restricted;
        int misc_struct_fid;
        int bndryFid;
        short bndryFno;
        EntityMode entityState= EntityMode.Add;// this is used for to identify the state of entity while performing CRUD operations.

        public string ESI_LOCATION { get => esi_location; set { esi_location = value;} }
        public bool Billable { get => billable; set { billable = value; UpdateEntityState(); } }
        public string Description { get => description_id; set { description_id = value; UpdateEntityState(); } }
        public string OWNER_CODE { get => owner_code; set { owner_code = value; UpdateEntityState(); } }
        public string Wattage { get => wattage; set => wattage = value; }
        public string LAMP_TYPE { get => lamp_type; set => lamp_type = value; }
        public string LUMINARE_STYLE { get => luminare_style; set => luminare_style = value; }
        public string RATE_SCHEDULE { get => rate_schedule; set => rate_schedule = value; }
        public string RATE_CODE { get => rate_code; set => rate_code = value; }
        public int PREVIOUS_COUNT { get => previous_count; set => previous_count = value; }
        public int CURRENT_COUNT { get => current_count; set => current_count = value; }
        public int THRESHOLD_PERCENT { get => threshold_percent; set { threshold_percent = value; UpdateEntityState(); } }
        public string THRESHOLD_STATE { get => threshold_state; set => threshold_state = value; }
        public bool THRESHOLD_OVERRIDE { get => threshold_override; set { threshold_override = value; UpdateEntityState(); } }
        public string RUN_DATE { get => run_date; set => run_date = value; }
        public string MODIFIED_BY { get => modified_by; set => modified_by = value; }
        public string MODIFIED_DATE { get => modified_date; set => modified_date = value; }
        public string CREATION_DATE { get => creation_date; set => creation_date = value; }
        public string BOUNDARY_CLASS { get => boundary_class; set{ boundary_class = value; UpdateEntityState(); } }
        public string BOUNDARY_ID { get => boundary_id; set => boundary_id = value; }
        public string Boundary { get { if (string.IsNullOrEmpty(boundary_id)) return null; else return (boundary_class + "-" + boundary_id); } }
        public bool Restricted { get => restricted; set { restricted = value; UpdateEntityState(); } }
        public int MiscStructFid { get => misc_struct_fid; set {misc_struct_fid = value; } }
        public EntityMode EntityState { get => entityState; set => entityState = value; }
        public int BndryFid { get => bndryFid; set => bndryFid = value; }
        public short BndryFno { get => bndryFno; set => bndryFno = value; }

        private void UpdateEntityState()
        {
            if (this.EntityState == EntityMode.Review)
            {
                this.EntityState = EntityMode.Update;
            }

        }
    }
}
