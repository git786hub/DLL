using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.Model
{
  public  class ParcelManual
    {
        int g3eFno;
        int g3eFid;
        string stage;
        string name;
        string address;
        string parcelMLAbstract;
        string sectionDescription;
        string additionName;
        string blockNumber;
        string lotNumber;

        public int G3eFno { get => g3eFno; set => g3eFno = value; }
        public int G3eFid { get => g3eFid; set => g3eFid = value; }
        public string Stage { get => stage; set => stage = value; }
        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public string ParcelMLAbstract { get => parcelMLAbstract; set => parcelMLAbstract = value; }
        public string SectionDescription { get => sectionDescription; set => sectionDescription = value; }
        public string AdditionName { get => additionName; set => additionName = value; }
        public string BlockNumber { get => blockNumber; set => blockNumber = value; }
        public string LotNumber { get => lotNumber; set => lotNumber = value; }
    }
}
