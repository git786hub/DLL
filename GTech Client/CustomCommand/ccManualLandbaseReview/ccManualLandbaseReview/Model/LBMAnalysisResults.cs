using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.Model
{
   public  class LBMAnalysisResults
    {

        private int g3eFno;
        private int g3eFid;
        private List<KeyValuePair<int,int>> detectedPolygons;
            
        public List<KeyValuePair<int,int>> DetectPolygons
        {
            get { return detectedPolygons; }
            set { detectedPolygons = value; }
        }


        public int G3eFid
        {
            get { return g3eFid; }
            set { g3eFid = value; }
        }

        public int G3eFno
        {
            get { return g3eFno; }
            set { g3eFno = value; }
        }

    }
}
