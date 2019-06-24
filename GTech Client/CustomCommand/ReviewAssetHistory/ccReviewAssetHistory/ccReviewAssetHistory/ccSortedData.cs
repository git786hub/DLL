// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: ccSortedData.cs
// 
//  Description: ccSortedData is used to as a base class to track the sorted columns on datagridview.
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  09/01/2018          Sithara                     
// ======================================================
namespace GTechnology.Oncor.CustomAPI
{
    public class ccSortedData
    {       
        public string m_SortedColumn { get; set; }

        public int m_SortedPriority { get; set; }

        public string m_SortedDirection { get; set; }

        public ccSortedData(string SortedColName,string SortedDirection,int SortPriority)
        {
            m_SortedColumn = SortedColName;
            m_SortedPriority = SortPriority;
            m_SortedDirection = SortedDirection;
        }
    }
}
