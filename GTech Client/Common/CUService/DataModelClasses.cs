using System.Collections.Generic;
using System.Data;

namespace GTechnology.Oncor.CustomAPI
{
   public class PickiListData
    {
        public string KeyField { get; set; }
        public string ValueField { get; set; }      

        public PickiListData(string sKeyField, string sValueField)
        {
            KeyField = sKeyField;
            ValueField = sValueField;
        }
    }
    public class CUDataModel
    {
        public DataTable CUDataWithStandardAttributes { get; set; }
        public string Category { get; set; }
        public List<PickiListData> PickListData { get; set; }
    }
}
