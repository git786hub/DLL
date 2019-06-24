// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: DataAccess.cs
//
//  Description:    Class to build the recordset.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  15/03/2018          Prathyusha                  Created 
// ======================================================
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    class DataAccess
    {
        #region Variables

        private IGTApplication m_oGTApp;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_application">The current G/Technology application object.</param>
        public DataAccess(IGTApplication p_oGTApp)
        {
            this.m_oGTApp = p_oGTApp;
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
                recordset = m_oGTApp.DataContext.OpenRecordset(p_strSql, CursorTypeEnum.adOpenStatic,
                               LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
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
