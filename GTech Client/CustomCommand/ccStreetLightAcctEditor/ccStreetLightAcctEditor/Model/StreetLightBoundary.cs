using GTechnology.Oncor.CustomAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.Model
{
    /// <summary>
    /// To Do implementaiton - JIRA#1255
    /// </summary>
    public class StreetLightBoundary
    {
        int bnd_class;
        short bnd_fno;
        int bnd_type_ano;
        string bnd_type;
        int bnd_id_ano;
        string bnd_id_G3efield;
        string bnd_id_G3eUsername;
        string bnd_id_G3eCName;
        string bnd_type_G3eField;
        string bnd_type_G3eUsername;
        string bnd_type_G3eCNname;


         EntityMode entityState= EntityMode.Add;// this is used for to identify the state of entity while performing CRUD operations.

        public int Bnd_Class { get => bnd_class; set=> bnd_class = value; }
        public short Bnd_Fno { get => bnd_fno; set { bnd_fno = value; if (entityState == EntityMode.Review) entityState = EntityMode.Update; } }
        public int Bnd_Type_Ano { get => bnd_type_ano; set { bnd_type_ano = value; if (entityState == EntityMode.Review) entityState = EntityMode.Update; } }
        public string Bnd_Type { get => bnd_type; set { bnd_type = value; if (entityState == EntityMode.Review) entityState = EntityMode.Update; } }
        public int Bnd_ID_Ano { get => bnd_id_ano; set { bnd_id_ano = value; if (entityState == EntityMode.Review) entityState = EntityMode.Update; } }

        public EntityMode EntityState { get => entityState; set => entityState = value; }
        public string Bnd_ID_G3efield { get => bnd_id_G3efield; set => bnd_id_G3efield = value; }
        public string Bnd_ID_G3eUsername { get => bnd_id_G3eUsername; set => bnd_id_G3eUsername = value; }
        public string Bnd_ID_G3eCName { get => bnd_id_G3eCName; set => bnd_id_G3eCName = value; }
        public string Bnd_Type_G3eField { get => bnd_type_G3eField; set => bnd_type_G3eField = value; }
        public string Bnd_Type_G3eUsername { get => bnd_type_G3eUsername; set => bnd_type_G3eUsername = value; }
        public string Bnd_Type_G3eCNname { get => bnd_type_G3eCNname; set => bnd_type_G3eCNname = value; }
    }
}
