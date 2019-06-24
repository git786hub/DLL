using System;
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
        ///  Method to get the primary graphic cno for fno
        /// </summary>
        /// <param name="fNo"></param>
        /// <param name="Geo">true to return Geo CNO, false to return Detail CNO</param>
        /// <returns>cno or 0</returns>
        internal short GetPrimaryGraphicCno(short fNo, bool Geo)
        {
            short primaryGraphicCno = 0;
            Recordset tempRs = null;
            try
            {
                tempRs = gTDataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + fNo);

                if (tempRs != null && tempRs.RecordCount > 0)
                {
                    tempRs.MoveFirst();
                    string field = Geo ? "G3E_PRIMARYGEOGRAPHICCNO" : "G3E_PRIMARYDETAILCNO";

                    //if(System.DBNull.Value != tempRs.Fields[field].Value)
                    if (!Convert.IsDBNull(tempRs.Fields[field].Value))
                    {
                        primaryGraphicCno = Convert.ToInt16(tempRs.Fields[field].Value);
                    }
                }
                return primaryGraphicCno;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (tempRs != null)
                {
                    tempRs.Close();
                    tempRs = null;
                }
            }
        }

        internal bool ISPrimaryGraphicComponentExist(short fNo, int fid)
        {
            bool exist = false;
            try
            {
                // Get the Primary Geo Graphic CNO
                short primaryCno = GetPrimaryGraphicCno(fNo, true);

                // If the CNO is zero (not defined), then try the detail CNO
                if (0 == primaryCno)
                {
                    primaryCno = GetPrimaryGraphicCno(fNo, false);
                }

                // If we have a CNO (and we should for either Geo or Detail at this point), then proceed to validate the existence of the component recordset
                if (0 != primaryCno)
                {
                    IGTComponent gTPrimaryComponent = gTDataContext.OpenFeature(fNo, fid).Components.GetComponent(primaryCno);

                    if (gTPrimaryComponent != null && gTPrimaryComponent.Recordset != null && gTPrimaryComponent.Recordset.RecordCount > 0)
                    {
                        exist = true;
                    }
                }

            }
            catch
            {
                throw;
            }

            return exist;
        }

        /// <summary>
        /// Get Workpoint Number.
        /// </summary>
        /// <returns></returns>
        internal int GetWorkPointNumber()
        {
            int WPnumber = 0;
            string sql = "";
            Recordset tempRs = null;
            try
            {
                sql = "select max(wp_nbr) from WORKPOINT_N where wr_nbr=:1";
                tempRs = gTDataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
            (int)ADODB.CommandTypeEnum.adCmdText, gTDataContext.ActiveJob);

                if (tempRs.RecordCount > 0)
                {
                    tempRs.MoveFirst();
                    if (!string.IsNullOrEmpty(Convert.ToString(tempRs.Fields[0].Value)))
                    {
                        WPnumber = Convert.ToInt32(tempRs.Fields[0].Value) + 1;
                    }
                    else
                    {
                        WPnumber = 1;
                    }
                }
                else
                {
                    WPnumber = 1;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (tempRs != null)
                {
                    tempRs.Close();
                    tempRs = null;
                }
            }

            return WPnumber;
        }

        /// <summary>
        /// Get Workpoint Instruction.
        /// </summary>
        /// <param name="cuCode"> CU code of the Active component.</param>
        /// <returns></returns>
        internal string GetWorkInstruction(string cuCode)
        {
            string sql = "";
            Recordset tempRs = null;
            try
            {
                sql = "select SHORT_WORK_INSTRUCTION from CULIB_UNIT where CU_ID=:1";
                tempRs = gTDataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
            (int)ADODB.CommandTypeEnum.adCmdText, cuCode);

                if (tempRs.RecordCount > 0)
                {
                    tempRs.MoveFirst();
                    if (!string.IsNullOrEmpty(Convert.ToString(tempRs.Fields[0].Value)))
                    {
                        return Convert.ToString(tempRs.Fields[0].Value);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (tempRs != null)
                {
                    tempRs.Close();
                    tempRs = null;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the next available Work Management CU sequence for the active job
        /// </summary>
        internal int NextWorkMgmtSeq
        {
            get
            {
                try
                {
                    int seq = 1;
                    string sql = "select cu.wm_seq from workpoint_n wp join workpoint_cu_n cu on wp.g3e_fid=cu.g3e_fid where wp.wr_nbr=? order by 1 desc";
                    Recordset rs = gTDataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, gTDataContext.ActiveJob);

                    if (null != rs & 0 < rs.RecordCount)
                    {
                        rs.MoveFirst();

                        //if (DBNull.Value != rs.Fields[0].Value)
                        if (!Convert.IsDBNull(rs.Fields[0].Value))
                        {
                            string curSeq = rs.Fields[0].Value.ToString();
                            curSeq = curSeq.Substring(curSeq.Length - 4);
                            seq = Convert.ToInt32(curSeq) + 1;
                        }

                        rs.Close();
                        rs = null;
                    }

                    return seq;

                }
                catch (Exception ex)
                {
                    {
                        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
                        throw new Exception(exMsg);
                    }
                }
            }
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
