
namespace GTechnology.Oncor.CustomAPI
{
    /// <summary>
    /// Class to hold CU related information. Used for processing selected and target features
    /// </summary>
    public class CuInformation
    {
        public string CuCode { get; set; }
        public string Phase { get; set; }
        public string Activity { get; set; }
        public short TargetCid { get; set; }
        public short SourceCid { get; set; }
        public bool? isNeutral { get; set; }
        public int ReplacedCID { get; set; }
        public int UnitCID { get; set; }
        public bool Processed { get; set; }
        public string PhasePosition { get; set; }

    }
}
