//----------------------------------------------------------------------------+
//        Class: PlotBoundaryAttributes
//  Description: This class stores information related to Plotting Boundary.
//                                                                  
//----------------------------------------------------------------------------+
//          $Author     :: Shubham Agarwal                                       $
//          $Date       :: 10/01/2019                                               $
//          $Revision   :: 1                                                   $
//----------------------------------------------------------------------------+
//    $History:: PlotBoundaryAttributes.cs                     $
// 
// *****************  Version 1  *****************
// User: sagarwal     Date: 10/01/2019   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+


namespace Intergraph.GTechnology.API
{
   public class PlotBoundaryAttributes
    {
        public short FNO { get; set; }
        public int FID { get; set; }
        public string ProductType { get; set; }
        public string SheetNumber { get; set; }
        public string PlotTemplateName { get; set; }

    }
}
