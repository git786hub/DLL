// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: DataAccessLayer.cs
//
//  Description:    Class to build the recordset.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  26/04/2018          Sitara                      Created
// ======================================================
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    class DataAccessLayer
    {
        #region Variables

        private IGTDataContext m_oGTDataContext;     
        

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_dataContext">The current G/Technology application object.</param>
        public DataAccessLayer(IGTDataContext p_dataContext)
        {
            this.m_oGTDataContext = p_dataContext;
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Method to build the recordset.
        /// </summary>
        /// <param name="p_strSql">SQL Query</param>
        /// <returns></returns>
        public Recordset GetRecordset(string p_strSql)
        {
            Recordset recordset = null;
            try
            {
                recordset = m_oGTDataContext.OpenRecordset(p_strSql, CursorTypeEnum.adOpenStatic,
                               LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
            }
            catch
            {
                throw;
            }
            return recordset;
        }

        public Recordset GetRecordset(string p_strSql, object p_params)
        {
            Recordset recordset = null;
            try
            {
                recordset = m_oGTDataContext.OpenRecordset(p_strSql, CursorTypeEnum.adOpenStatic,
                               LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, p_params);
            }
            catch
            {
                throw;
            }
            return recordset;
        }
        
        #endregion
    }
}
