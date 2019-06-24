using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI
{
    public class ChildFeatureInfo
    {
        public bool Owner1IdExists { get; set; }
        public bool Owner2IdExists { get; set; }
        //public int FId { get; set; }

        public IGTKeyObject GtKeyObject;
    }
}
