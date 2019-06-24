using GTechnology.Oncor.CustomAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.Model
{
	public class StreetLightNonLocated
	{


		string stltIdentifier;
		string connectionStatus;
		string disconnectDate = DateTime.Now.ToShortDateString();
		string connectDate = DateTime.Now.ToShortDateString();
		string location;
		string additionalLocation;
		int g3eFid;
		short g3efno;

		string esi_location = "";
		string wattage = "";
		string lamp_type = "";
		string luminare_style = "";
		string rate_schedule = "";
		string rate_code;
		string cu = "";
		EntityMode entityState = EntityMode.Add;// this is used for to identify the state of entity while performing CRUD operations.

		public string StltIdentifier { get => stltIdentifier; set { stltIdentifier = value; this.entityState = EntityMode.Add; } }
		public string ConnectionStatus { get => connectionStatus; set => connectionStatus = value; }
		public string DisconnectDate { get => disconnectDate; set => disconnectDate = value; }
		public string ConnectDate { get => connectDate; set => connectDate = value; }
		public string Location { get => location; set => location = value; }
		public string AdditionalLocation { get => additionalLocation; set => additionalLocation = value; }
		public EntityMode EntityState { get => entityState; set => entityState = value; }
		public int G3eFid { get => g3eFid; set => g3eFid = value; }
		public short G3efno { get => g3efno; set => g3efno = value; }

		public string ESI_LOCATION { get => esi_location; set { esi_location = value; } }
		public string Wattage { get => wattage; set => wattage = value; }
		public string LAMP_TYPE { get => lamp_type; set => lamp_type = value; }
		public string LUMINARE_STYLE { get => luminare_style; set => luminare_style = value; }
		public string RATE_SCHEDULE { get => rate_schedule; set => rate_schedule = value; }
		public string RATE_CODE { get => rate_code; set => rate_code = value; }
		public string CU { get => cu; set => cu = value; }
	}
}
