using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.Model
{
   public class BuildingManual
    {
        int g3eFno;
        int g3eFid;
        string stage;
        string name;
        string location;

        public int G3eFno { get => g3eFno; set => g3eFno = value; }
        public int G3eFid { get => g3eFid; set => g3eFid = value; }
        public string Stage { get => stage; set => stage = value; }
        public string Name { get => name; set => name = value; }
        public string Location { get => location; set => location = value; }
    }
}
