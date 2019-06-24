//----------------------------------------------------------------------------+
//        Class: GridSelectionHelper
//  Description: This class is used to store the adjacent row details for the current selected row
//
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 20/03/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: GridSelectionHelper.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 20/03/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+
using OncorTicketCreation;


namespace GTechnology.Oncor.CustomAPI
{
    public class GridSelectionHelper
    {
        public TicketStepType SelectedRow;
        public int SelectedRowIndex;
        public TicketStepType RowAbove;
        public int AboveRowIndex;
        public TicketStepType RowBelow;
        public int BelowRowIndex;
    }
}
