using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.Model
{
  public class PipeLine
    {
        int g3eFno;
        int g3eFid;
        string stage;
        string addressRange;
        string prefix;
        string preType;
        string name;
        string type;
        string suffix;

        public int G3eFno { get => g3eFno; set => g3eFno = value; }
        public int G3eFid { get => g3eFid; set => g3eFid = value; }
        public string Stage { get => stage; set => stage = value; }
        public string AddressRange { get => addressRange; set => addressRange = value; }
        public string Prefix { get => prefix; set => prefix = value; }
        public string PreType { get => preType; set => preType = value; }
        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public string Suffix { get => suffix; set => suffix = value; }
    }
}
