using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccNamedViewCommon
    {
        public short m_ViewId { get; set; }

        public string m_ViewName { get; set; }

        public string m_FilterColumn { get; set; }

        public string m_FilterValue { get; set; }

        public string m_SortColumn { get; set; }

        public short m_SortPriority { get; set; }

        public string m_SortDirection { get; set; }
    }
}
