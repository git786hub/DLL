using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.Model
{
   public class StreetCenterLine
    {
        int g3eFno;
        int g3eFid;
        string stage;
        string owner;
        string material;
        string name;
        string type;
        string width;

        public int G3eFno { get => g3eFno; set => g3eFno = value; }
        public int G3eFid { get => g3eFid; set => g3eFid = value; }
        public string Stage { get => stage; set => stage = value; }
        public string Owner { get => owner; set => owner = value; }
        public string Material { get => material; set => material = value; }
        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public string Width { get => width; set => width = value; }
    }
}
