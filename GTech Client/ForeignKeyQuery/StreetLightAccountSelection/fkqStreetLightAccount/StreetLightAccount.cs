
namespace GTechnology.Oncor.CustomAPI
{
   public class StreetLightAccount
    {
        string esi_location = "";
        string description = "";
        string rate_schedule = "";
        string wattage = "";
        string lamp_type = "";
        string luminare_style = "";

        public string ESI_Location
        {
            get { return esi_location; }
            set { esi_location = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Rate_Schedule
        {
            get { return rate_schedule; }
            set { rate_schedule = value; }
        }

        public string Wattage
        {
            get { return wattage; }
            set { wattage = value; }
        }
        public string Lamp_Type
        {
            get { return lamp_type; }
            set { lamp_type = value; }
        }
        public string Luminare_Style
        {
            get { return luminare_style; }
            set { luminare_style = value; }
        }

    }
}
