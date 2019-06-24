using ADODB;
using Intergraph.GTechnology.API;


namespace GTechnology.Oncor.CustomAPI
{
    public class CommonFunctions
    {
        IGTDataContext gTDataContext;
        public CommonFunctions()
        {
            IGTApplication gTApplication = (IGTApplication)GTClassFactory.Create<IGTApplication>();
            gTDataContext = gTApplication.DataContext;
        }         

        /// <summary>
        /// Returns true if activefeature is linear feature.
        /// </summary>
        /// <returns></returns>
        internal bool IsActiveFeatureIsLinear(short Fno)
        {
            string sql = "";
            Recordset rsLinear = null;
            try
            {
                sql = "SELECT * FROM G3E_COMPONENTINFO_OPTABLE WHERE G3E_CNO IN(SELECT G3E_PRIMARYGEOGRAPHICCNO FROM G3E_FEATURES_OPTABLE WHERE G3E_FNO=?) AND UPPER(G3E_GEOMETRYTYPE) LIKE '%POINT%'";
                rsLinear = gTDataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
            (int)ADODB.CommandTypeEnum.adCmdText, Fno);

                if (rsLinear.RecordCount <= 0)
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }

            return false;
        }
    }
}
