using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.Diagnostics;


namespace GTechnology.Oncor.CustomAPI
{
	public class StreetLightHistory : IGTCustomCommandModeless
	{
		#region Properties

		#region GTech Properties

		IGTApplication oApp = null;
		IGTMapWindow oMap = null;
		IGTDataContext oDC = null;
		IGTCustomCommandHelper oCustomCmdHelper = null;
		IGTDisplayService oDisplay = null;
		IGTLocateService oLocate = null;
		IGTRelationshipService oRelationService = null;
		IGTTransactionManager oTransManager = null;

		private GTDiagnostics oDiag = null;



		private bool _canTerminate;

		#endregion GTech Properties

		#region Tracker Properties

		string sDisplayPathName = "StreetLight Location Tracking";
		private GTSysGenParam oGenParams = null;
		private IGTGeometry oLocationCriteria = null;

		private List<GTActiveStreetLight> oActiveList = null;
		private List<GTActiveStructure> oActiveStructureList = null;
		private List<AssetHistory> oHistory = null;

		//Structure_ID, AssetHistory
		Dictionary<string, List<AssetHistory>> oSID = null;
		//Streetlight FID, AssetHistory
		Dictionary<int, List<AssetHistory>> oStreetlight = null;
		//Structure FID, AssetHistory
		Dictionary<int, List<AssetHistory>> oStructure = null;

		#endregion Tracker Properties

		#endregion Properties

		#region IGTCustomCommandModeless Implementation
		public bool CanTerminate
		{
			get { return _canTerminate; }
			set { _canTerminate = value; }
		}

		public IGTTransactionManager TransactionManager { set { oTransManager = value; } }
		public void Activate(IGTCustomCommandHelper CustomCommandHelper)
		{
			try
			{
				this.oCustomCmdHelper = CustomCommandHelper;
				oDiag = new GTDiagnostics(GTDiagSS.IDotNetCustomCmd, GTDiagMaskWord.IDotNetCustomCmd, "StreetLightHistory.cs");

				InitializeAppResources();

				SetupLocationCriteria();

				DoActiveMapWindowByStreetlight();
				DoActiveMapWindowByStructure();

				DoAssetHistory();

				DoEvaluation();

				DoTempGeometries();

				DoExit();
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "Activate", ex);
				DoExit();
			}
		}

		public void Pause()
		{
		}

		public void Resume()
		{
		}

		public void Terminate()
		{
		}

		#endregion IGTCustomCommandModeless Implementation

		#region Setup and Initialization

		private void InitializeAppResources()
		{

			try
			{


				this.oApp = GTClassFactory.Create<IGTApplication>();

				if(this.oApp == null)
				{
					CommandUtilities.LogMessage(oDiag, "GTAssetTracker.InitializeResources", "IGTApplicationError", "Cannot initialize IGTApplication.");
					DoExit();
				}

				SetStatusBarText("Initializing...");

				this.oMap = oApp.ActiveMapWindow;
				if(this.oMap == null)
				{
					CommandUtilities.LogMessage(oDiag, "GTAssetTracker.InitializeResources", "MapWindowError", "There is no active map window.");
					throw new Exception("IGTMapWindow");
				}

				oDisplay = oMap.DisplayService;
				if(oDisplay == null)
				{
					CommandUtilities.LogMessage(oDiag, "GTAssetTracker.InitializeResources", "DisplayService", "No display service for the active map window.");
					throw new Exception("IGTDisplayService");
				}

				this.oDC = oApp.DataContext;
				if(this.oDC == null)
				{
					CommandUtilities.LogMessage(oDiag, "GTAssetTracker.InitializeResources", "DataContextError", "No data context was obtained from IGTApplication.");
					throw new Exception("IGTDataContext");
				}

				this.oLocate = this.oMap.LocateService;
				if(this.oLocate == null)
				{
					CommandUtilities.LogMessage(oDiag, "GTAssetTracker.InitializeResources", "IGTLocateService", "No IGTLocateService was obtained from IGTMapWindow.");
					throw new Exception("IGTLocateService");
				}

				this.oRelationService = GTClassFactory.Create<IGTRelationshipService>();
				this.oRelationService.DataContext = this.oDC;

				this.oGenParams = new GTSysGenParam();
				if(!CommandUtilities.GetSysGeneralParameters(this.oDC, this.oDiag, ref this.oGenParams))
				{
					throw new Exception("No General Parameters have been defined.");
				}

				this.oActiveList = new List<GTActiveStreetLight>();
				this.oActiveStructureList = new List<GTActiveStructure>();

				SetStatusBarText("Initialized.");
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "GTAssetTracker.InitializeResources", ex);
				throw ex;
			}
		}

		private void SetStatusBarText(string sDisplayText)
		{
			try
			{
				oApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, " ");
				oApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sDisplayText);
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "SetStatusBarText", ex);
				throw ex;
			}
		}
		private void DoExit()
		{
			try
			{
				SetStatusBarText("Exiting Street Light History...");

				// This is not in the design; however, if nothing is found to display,
				// the system appears to have done nothing at all and this
				// at least gives some feedback in that case.
				if(0 == this.oActiveList.Count)
				{
					MessageBox.Show("No historical Street Light information was found for the features in the active map window.", "G/Technology", System.Windows.Forms.MessageBoxButtons.OK);
				}

				if(this.oCustomCmdHelper != null)
				{
					this.oDC = null;
					this.oMap = null;
					this.oApp = null;
					this.oCustomCmdHelper.Complete();
				}
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "StreetLightHistory.DoExit", ex);
				throw ex;
			}
		}

		#endregion Setup and Initialization

		#region Map Window

		private void SetupLocationCriteria()
		{
			try
			{
				if(this.oMap == null)
				{
					throw new Exception("Map object is null.");
				}

				IGTWorldRange oMapExtents = this.oMap.GetRange();

				IGTPolygonGeometry m_oPoly = GTClassFactory.Create<IGTPolygonGeometry>();
				IGTPoint m_ulPoint = GTClassFactory.Create<IGTPoint>();
				m_ulPoint.X = oMapExtents.BottomLeft.X;
				m_ulPoint.Y = oMapExtents.TopRight.Y;

				IGTPoint m_urPoint = GTClassFactory.Create<IGTPoint>();
				m_urPoint.X = oMapExtents.TopRight.X;
				m_urPoint.Y = oMapExtents.TopRight.Y;

				IGTPoint m_lrPoint = GTClassFactory.Create<IGTPoint>();
				m_lrPoint.X = oMapExtents.TopRight.X;
				m_lrPoint.Y = oMapExtents.BottomLeft.Y;

				IGTPoint m_llPoint = GTClassFactory.Create<IGTPoint>();
				m_llPoint.X = oMapExtents.BottomLeft.X;
				m_llPoint.Y = oMapExtents.BottomLeft.Y;

				m_oPoly.Points.Add(m_ulPoint);
				m_oPoly.Points.Add(m_urPoint);
				m_oPoly.Points.Add(m_lrPoint);
				m_oPoly.Points.Add(m_llPoint);

				this.oLocationCriteria = (IGTGeometry)m_oPoly;

				if(this.oLocationCriteria == null) throw new Exception("Map extents are null.");
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "GTStreetLight.SetupLocationCriteria", ex);
				throw ex;
			}
		}

		private void DoActiveMapWindowByStreetlight()
		{
			try
			{
				if(this.oLocate == null) throw new Exception("IGTLocateService object is null.");

				if(this.oLocationCriteria == null) throw new Exception("The map extents for the active map window is null.");

				if(this.oRelationService == null) throw new Exception("IGTRelationshipService is null.");

				this.oLocate.FenceMode = GTFenceMode.gtfmInside;
				IGTDDCKeyObjects activeSLightDDCList = this.oLocate.Locate(this.oLocationCriteria, 0, -1, GTSelectionTypeConstants.gtmwstSelectAll, new short[] { 56 }); // 56=Street Light

				if(activeSLightDDCList == null)
				{
					CommandUtilities.LogMessage(this.oDiag, "LoadActiveDDCObjects", "IGTLocateService", "There are no street lights in the active map window.");
					//throw new Exception("No features were located in the active map window");
					return;
				}


				if(activeSLightDDCList.Count == 0)
				{
					CommandUtilities.LogMessage(this.oDiag, "LoadActiveDDCObjects", "IGTLocateService", "There are no street lights in the active map window.");
					//throw new Exception("No features were located in the active map window");
					return;
				}

				ProcessActiveStreetLightDDC(activeSLightDDCList);
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "DoActiveMapWindow", ex);
				throw ex;
			}
		}

		private void ProcessActiveStreetLightDDC(IGTDDCKeyObjects ddcStreetLightList)
		{
			try
			{
				foreach(IGTDDCKeyObject ddcobj in ddcStreetLightList)
				{
					if((ddcobj.Geometry.Type == GTGeometryTypeConstants.gtgtOrientedPointGeometry) || (ddcobj.Geometry.Type == GTGeometryTypeConstants.gtgtPointGeometry))
					{
						IGTKeyObject koStreetLight = oDC.OpenFeature(ddcobj.FNO, ddcobj.FID);

						oRelationService.ActiveFeature = koStreetLight;

						IGTKeyObjects koOwnedByList = oRelationService.GetRelatedFeatures(3); // Owned By

						if(koOwnedByList == null) continue;

						if(koOwnedByList.Count == 0) continue;

						//Should only be one IGTKeyObject that owns a streetlight
						//Need Common_N data on both the structure and the streetlight
						IGTKeyObject koStructure = koOwnedByList[0];

						GTActiveStreetLight objStreetLight = CommandUtilities.GetCommonNActiveFeature(koStreetLight, koStructure, oDiag);

						if(objStreetLight == null) continue;

						objStreetLight.G3E_FID = ddcobj.FID;
						objStreetLight.G3E_FNO = ddcobj.FNO;
						objStreetLight.Structure_FID = koStructure.FID;
						objStreetLight.StructureFNO = koStructure.FNO;

						this.oActiveList.Add(objStreetLight);

					}
				}
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "ProcessActiveStreetLightDDC", ex);
				throw ex;
			}
		}

		private void DoActiveMapWindowByStructure()
		{
			try
			{
				short[] fnoToLocate = new short[3];
				fnoToLocate[0] = 107; //Streetlight Standard
				fnoToLocate[1] = 110; //pole
				fnoToLocate[2] = 114; //Misc Structure


				if(this.oLocate == null) throw new Exception("IGTLocateService object is null.");

				if(this.oLocationCriteria == null) throw new Exception("The map extents for the active map window is null.");

				if(this.oRelationService == null) throw new Exception("IGTRelationshipService is null.");

				this.oLocate.FenceMode = GTFenceMode.gtfmInside;
				IGTDDCKeyObjects activeStructureDDCList = this.oLocate.Locate(this.oLocationCriteria, 0, -1, GTSelectionTypeConstants.gtmwstSelectAll, fnoToLocate);

				if(activeStructureDDCList == null)
				{
					CommandUtilities.LogMessage(this.oDiag, "DoActiveMapWindowByStructure", "IGTLocateService", "There are no structures or street lights in the active map window.");
					return;
				}


				if(activeStructureDDCList.Count == 0)
				{
					CommandUtilities.LogMessage(this.oDiag, "LoadActiveDDCObjects", "IGTLocateService", "There are no street lights in the active map window.");
					//throw new Exception("No features were located in the active map window");
					return;
				}

				ProcessActiveStructureDDC(activeStructureDDCList);
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "DoActiveMapWindow", ex);
				throw ex;
			}
		}

		private void ProcessActiveStructureDDC(IGTDDCKeyObjects ddcStructureList)
		{
			try
			{
				foreach(IGTDDCKeyObject ddcobj in ddcStructureList)
				{
					if((ddcobj.Geometry.Type == GTGeometryTypeConstants.gtgtOrientedPointGeometry) || (ddcobj.Geometry.Type == GTGeometryTypeConstants.gtgtPointGeometry))
					{
						IGTKeyObject koStructure = oDC.OpenFeature(ddcobj.FNO, ddcobj.FID);

						oRelationService.ActiveFeature = koStructure;

						IGTKeyObjects koOwnsList = oRelationService.GetRelatedFeatures(2);

						if(koOwnsList == null) continue;

						if(koOwnsList.Count == 0) continue;

						GTActiveStructure oStructure = new GTActiveStructure();
						oStructure.G3E_FID = ddcobj.FID;
						oStructure.G3E_FNO = ddcobj.FNO;
						IGTOrientedPointGeometry ogeom = ddcobj.Geometry as IGTOrientedPointGeometry;

						oStructure.OGG_X1 = ogeom.Origin.X;
						oStructure.OGG_Y1 = ogeom.Origin.Y;
						oStructure.OGG_Z1 = ogeom.Origin.Z;
						string sid = CommandUtilities.GetCommonNStructureID(koStructure, oDiag);

						if(koOwnsList != null)
						{
							if(koOwnsList.Count > 0)
							{
								foreach(IGTKeyObject koObj in koOwnsList)
								{
									if(koObj.FNO == 56)
									{
										oStructure.StreetlightKOList.Add(koObj);
									}
								}
							}
						}

						this.oActiveStructureList.Add(oStructure);
					}
				}
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "ProcessActiveStreetLightDDC", ex);
				throw ex;
			}
		}

		#endregion Map Window

		#region AssetHistory NOT USED
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_obj"></param>
		/// <param name="_history">Contains every assethistory record directly and indirectly associated to current active streetlight object.</param>
		/*private List<GTStreetLightHistory> ProcessStreetLightAssetHistory(GTActiveStreetLight _obj, List<AssetHistory> _history)
    {
      List<GTStreetLightHistory> oLifeCycle = new List<GTStreetLightHistory>();

      try
      {
        List<AssetHistory> streetLightHistory = null;
        List<AssetHistory> structureHistory = null;
        

        if (_history.Any(x=>x.G3E_FNO == 56))
        {
          streetLightHistory = _history.Where(x => x.G3E_FNO == 56).OrderByDescending(x => x.ChangeDate).ToList();
        }

        if (_history.Any(x => x.G3E_FNO == 107 || x.G3E_FNO == 110 || x.G3E_FNO == 114))
        {
          structureHistory = _history.Where(x => x.G3E_FNO == 107 || x.G3E_FNO == 110 || x.G3E_FNO == 114).OrderByDescending(x => x.ChangeDate).ToList();
        }

        //Order each list with one create and delete and multiple updates
        List<AssetHistory> templist1 = CreateDistinctAssetHistoryRecords(streetLightHistory);
        if (templist1 == null) return null;
        //oRderedHistoryList.AddRange(templist);

        List<AssetHistory> templist2 = CreateDistinctAssetHistoryRecords(structureHistory);
        if (templist2 == null) return null;
        //oRderedHistoryList.AddRange(templist);

        templist1 = templist1.OrderByDescending(x => x.ChangeDate).ToList();
        templist2 = templist1.OrderByDescending(x => x.ChangeDate).ToList();

        
        DateTime? streetlightChangeDTG = null;

        GTStreetLightHistory objHistory = null;
        for(int i = templist1.Count - 1; i >= 0; i--)
        {
          if (i == templist1.Count - 1)
          {
            streetlightChangeDTG = templist1[i].ChangeDate;
            if (templist2.Any(x=>x.ChangeDate <= streetlightChangeDTG))
            {
              //Get the structure that was used first
              var minResults = templist2.Where(x => x.ChangeDate <= streetlightChangeDTG).OrderByDescending(x => x.ChangeDate).ToList();
              var minResult = minResults[0];
              objHistory = new GTStreetLightHistory();
              objHistory.OwningStructure = minResult;
              objHistory.StreetLight = templist1[i];
              oLifeCycle.Add(objHistory);
            }
          }
          else if (i == 0)
          {
            streetlightChangeDTG = templist1[i].ChangeDate;
            if (templist2.Any(x => x.ChangeDate <= streetlightChangeDTG))
            {
              var maxResults = templist2.Where(x => x.ChangeDate <= streetlightChangeDTG).OrderByDescending(x => x.ChangeDate).ToList();
              var maxResult = maxResults[0];
              objHistory = new GTStreetLightHistory();
              objHistory.OwningStructure = maxResult;
              objHistory.StreetLight = templist1[0];
              oLifeCycle.Add(objHistory);
            }
          }
          else
          {
            streetlightChangeDTG = templist1[i].ChangeDate;
            
            if (templist2.Any(x=>x.ChangeDate <= streetlightChangeDTG))
            {
              var otherResults = templist2.Where(x => x.ChangeDate <= streetlightChangeDTG).OrderByDescending(x => x.ChangeDate).ToList();
              var otherResult = otherResults[0];
              objHistory = new GTStreetLightHistory();
              objHistory.OwningStructure = otherResult;
              objHistory.StreetLight = templist1[i];
              oLifeCycle.Add(objHistory);
            }
          }
        }
      }
      catch(Exception ex)
      {
        CommandUtilities.LogException(oDiag, "ProcessStreetLightAssetHistory", ex);
        throw ex;
      }

      return oLifeCycle;
    }
    

    private List<AssetHistory> GetAllHistoryBySIDList(List<string> _sidList)
    {
      List<AssetHistory> olist = null;

      try
      {
        olist = this.oHistory.Where(x => _sidList.Contains(x.StructureID_1)).OrderByDescending(x => x.ChangeDate).ToList();

        if (olist == null) { return null; }

        if (olist.Count == 0) { return null; }
      }
      catch (Exception ex)
      {
        CommandUtilities.LogException(oDiag, "GetAllHistoryBySIDList", ex);
        throw ex;
      }

      return olist;
    }
    */
		/// <summary>
		/// Gets every FID and SID directly and indirectly associated to the current streetlight structures and streetlights
		/// </summary>
		/// <param name="_iStreetLightFID"></param>
		/// <param name="_sidList"></param>
		/// <returns></returns>
		/*private List<AssetHistory> GetStreetLightHistory(int _iStreetLightFID, List<string> _sidList)
    {
      List<AssetHistory> _list = null;

      try
      {
        bool bHistoryLoop = true;
        List<int> tempFIDList = new List<int>();
        List<string> tempSIDList = new List<string>();
        bool bFIDList = false;
        bool bSIDList = false;
        
        while (bHistoryLoop)
        {
          if ((bFIDList) && (bSIDList))
          {
            _list = GetAllHistoryBySIDList(tempSIDList);
            break;
          }
          //Get all the fids associated to those in the sid list
          var fids = GetFIDinSID(_sidList);
          if (fids == null) break;
          if (fids.Count == 0) break;

          if (!CommandUtilities.CompareLists(fids, tempFIDList))
          {
            tempFIDList.AddRange(fids);
            tempFIDList = tempFIDList.Distinct().ToList();

            fids = null;
          }
          else
          {
            bFIDList = true;
          }
          //Get any additional SID associated to FID list
          var sids = GetSIDinFID(tempFIDList);
          if (sids == null) break;
          if (sids.Count == 0) break;

          if (!CommandUtilities.CompareLists(sids, tempSIDList))
          {
            tempSIDList.AddRange(sids);

            tempSIDList = tempSIDList.Distinct().ToList();
            _sidList = GetSIDinFID(tempFIDList);

            
            sids = null;
          }
          else
          {
            bSIDList = true;
          }
        }
      }
      catch(Exception ex)
      {
        CommandUtilities.LogException(oDiag, "DoAssetHistory", ex);
        throw ex;
      }

      return _list;
    }

    private List<int> GetFIDinSID(List<string> _sidlist)
    {
      List<int> fidList = null;

      try
      {
        var results = this.oHistory.Where(x => _sidlist.Contains(x.StructureID_1) && x.G3E_FNO == 56).GroupBy(x=>x.G3E_FID).Select(x=>x.First()).Select(x=>x.G3E_FID).ToList();
        if (results.Count == 0) return null;

        fidList = new List<int>();
        fidList = results;
      }
      catch(Exception ex)
      {
        CommandUtilities.LogException(oDiag, "GetFIDinSID", ex);
        throw ex;
      }

      return fidList;
    }

    private List<string> GetSIDinFID(List<int> _fidlist)
    {
      List<string> sidList = null;

      try
      {
        var results = this.oHistory.Where(x => _fidlist.Contains(x.G3E_FID) && x.G3E_FNO == 56).GroupBy(x => x.StructureID_1).Select(x => x.First()).Select(x => x.StructureID_1).ToList();
        if (results.Count == 0) return null;

        sidList = new List<string>();
        sidList = results;
      }
      catch (Exception ex)
      {
        CommandUtilities.LogException(oDiag, "GetSIDinFID", ex);
        throw ex;
      }

      return sidList;
    }

    private List<AssetHistory> CreateDistinctAssetHistoryRecords(List<AssetHistory> _list)
    {
      List<AssetHistory> olist = new List<AssetHistory>();

      try
      {
        #region Create Objects

        if (_list.Any(x => x.ChangeOperation == "CREATE"))
        {
          //Get the one Create object out of the four
          var objCreate = _list.Where(x => x.ChangeOperation == "CREATE").GroupBy(x => x.ChangeOperation).Select(x => x.First()).ToList();
          if (objCreate.Count > 0)
          {
            olist.AddRange(objCreate);
          }
        }


        #endregion Create Objects

        #region Update Objects

        if (_list.Any(x => x.ChangeOperation == "UPDATED" && x.G3E_ANO == this.oGenParams.ANO_X || x.G3E_ANO == this.oGenParams.ANO_Y))
        {
          var objUpdateList = _list.Where((x => x.ChangeOperation == "UPDATED" && x.G3E_ANO == this.oGenParams.ANO_X || x.G3E_ANO == this.oGenParams.ANO_Y)).ToList();
          if (objUpdateList.Count > 0)
          {
            //Get location change updates only
            if (objUpdateList.Any(x => x.G3E_ANO == this.oGenParams.ANO_X || x.G3E_ANO == this.oGenParams.ANO_Y))
            {
              var uniqueLocationUpdates = objUpdateList.GroupBy(y => new { y.G3E_FID, y.G3E_FNO, y.OGG_X1, y.OGG_Y1 }).Select(y => y.First()).ToList();
              if (uniqueLocationUpdates.Count > 0)
              {
                olist.AddRange(uniqueLocationUpdates);
              }
            }
          }
        }
        #endregion Update Objects

        #region Delete Objects

        if (_list.Any(x => x.ChangeOperation == "DELETE"))
        {
          var objDelete = _list.Where(x => x.ChangeOperation == "DELETE").GroupBy(x => new { x.G3E_FID, x.G3E_FNO }).Select(x => x.First()).ToList();
          if (objDelete.Count > 0)
          {
            olist.AddRange(objDelete);
          }
        }

        #endregion Delete Objects

        if (olist.Count == 0) return null;

        olist = olist.OrderByDescending(x => x.ChangeDate).ToList();
      }
      catch(Exception ex)
      {
        CommandUtilities.LogException(oDiag, "CreateDistinctAssetHistoryRecords", ex);
        throw ex;
      }

      return olist;
    }

    */
		#endregion AssetHIstory NOT USED


		#region AssetHistory

		private void DoAssetHistory()
		{
			try
			{
				if(this.oActiveList == null) return;

				GetAllHistory();

				GetAssetHistoryBySID();
				GetStructureHistory();
				GetAssetHistoryByFID();

			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "DoAssetHistory", ex);
				throw ex;
			}
		}
		//1
		private void GetAllHistory()
		{
			try
			{
				this.oHistory = CommandUtilities.GetAllAssetHistory(oDC, oDiag);

				if(null != oHistory && 0 == this.oHistory.Count)
				{
					this.oHistory = null;
				}

			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "DoAssetHistory", ex);
				throw ex;
			}
		}
		//1
		private void GetAssetHistoryBySID()
		{
			try
			{
				//The oHistory list must be populated
				if(this.oHistory == null) return;

				oSID = new Dictionary<string, List<AssetHistory>>();

				var sidList = this.oHistory.OrderByDescending(x => x.ChangeDate).GroupBy(x => x.StructureID_1).Select(x => x.First()).Select(x => x.StructureID_1).ToList();
				if(sidList.Count == 0) return;

				foreach(string sid in sidList)
				{
					var sidHistoryTracks = this.oHistory.Where(y => y.StructureID_1 == sid).OrderByDescending(y => y.ChangeDate).ToList();
					List<AssetHistory> tempList = new List<AssetHistory>();
					tempList = sidHistoryTracks;

					var uniqueSIDTracks = sidHistoryTracks.GroupBy(b => new { b.ChangeOperation, b.ChangeDate }).Select(b => b.First()).ToList();
					oSID.Add(sid, uniqueSIDTracks);
				}
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "GetAssetHistoryBySID", ex);
				throw ex;
			}
		}

		//1
		private void GetAssetHistoryByFID()
		{
			try
			{
				DateTime? startDTG = null;
				DateTime? endDTG = null;
				List<AssetHistory> templist = null;
				//The oHistory list must be populated
				if(this.oHistory == null) return;

				this.oStreetlight = new Dictionary<int, List<AssetHistory>>();

				var fidList = this.oHistory.Where(x => x.G3E_FNO == 56).OrderByDescending(x => x.ChangeDate).GroupBy(x => x.G3E_FID).Select(x => x.First()).Select(x => x.G3E_FID).ToList();
				if(fidList.Count == 0) return;

				foreach(int fid in fidList)
				{
					templist = new List<AssetHistory>();
					var fidHistoryTracks = this.oHistory.Where(y => y.G3E_FID == fid).OrderByDescending(y => y.ChangeDate).ToList();

					var uniqueStreetlightHistory = fidHistoryTracks.GroupBy(b => new { b.ChangeOperation, b.ChangeDate }).Select(b => b.First()).ToList();
					if(uniqueStreetlightHistory.Count > 0)
					{
						templist.AddRange(uniqueStreetlightHistory);
					}

					var sidList = fidHistoryTracks.GroupBy(x => x.StructureID_1).Select(x => x.First()).Select(x => x.StructureID_1).ToList();

					foreach(string sid in sidList)
					{
						var dateList = fidHistoryTracks.Where(x => x.StructureID_1 == sid).OrderByDescending(x => x.ChangeDate).ToList();
						startDTG = dateList[dateList.Count - 1].ChangeDate;
						endDTG = dateList[0].ChangeDate;
						if(this.oSID.ContainsKey(sid))
						{
							var suppList = this.oSID[sid].Where(x => x.ChangeDate < startDTG && x.G3E_FNO == 56).OrderByDescending(x => x.ChangeDate).ToList();
							if(suppList.Count > 0)
							{
								var uniqueSuppHistory = suppList.GroupBy(b => new { b.ChangeOperation, b.ChangeDate }).Select(b => b.First()).ToList();
								templist.AddRange(uniqueSuppHistory);
							}
						}
					}

					if(templist.Count == 0) continue;

					oStreetlight.Add(fid, templist);
				}
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "GetAssetHistoryBySID", ex);
				throw ex;
			}
		}

		private void GetStructureHistory()
		{
			try
			{
				if(this.oHistory == null) return;

				this.oStructure = new Dictionary<int, List<AssetHistory>>();

				var structureList = this.oHistory.Where(x => x.G3E_FNO == 107 || x.G3E_FNO == 110 || x.G3E_FNO == 114).GroupBy(x => x.G3E_FID).Select(x => x.First()).Select(x => x.G3E_FID).ToList();
				if(structureList.Count == 0) return;

				foreach(int sfid in structureList)
				{
					var structureHistory = this.oHistory.Where(a => a.G3E_FID == sfid).OrderByDescending(a => a.ChangeDate).ToList();
					if(structureHistory.Count == 0) continue;

					//Get distinct records based on changeoperation and changedayte
					var uniqueStructureHistory = structureHistory.GroupBy(b => new { b.ChangeOperation, b.ChangeDate }).Select(b => b.First()).ToList();
					if(uniqueStructureHistory.Count == 0) continue;

					this.oStructure.Add(sfid, uniqueStructureHistory);
				}
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "GetStructureHistory", ex);
				throw ex;
			}
		}

		private void DoEvaluation()
		{
			try
			{
				foreach(GTActiveStreetLight obj in this.oActiveList)
				{
					if(this.oStreetlight.ContainsKey(obj.G3E_FID))
					{
						List<AssetHistory> temp = this.oStreetlight[obj.G3E_FID].OrderByDescending(x => x.ChangeDate).ToList();

						if(null != temp && 0 < temp.Count)
						{
							obj.GeometrySource = this.oStreetlight[obj.G3E_FID].OrderByDescending(x => x.ChangeDate).ToList();
						}
						else
						{
							obj.GeometrySource = null;
						}
					}
				}
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "DoEvaluation", ex);
				throw ex;
			}
		}

		#region By Structure


		#endregion By Structure

		#endregion AssetHistory

		#region Temporary Geometries

		private void DoSymbolGeometryByFID(List<AssetHistory> objHistList)
		{
			try
			{
				IGTGeometry[] arrSymbolGeoms = new IGTGeometry[objHistList.Count];
				object[] arrStyleIDs = new object[objHistList.Count];
				object[] arrToolTips = new object[objHistList.Count];
				string sDisplayName = string.Empty;

				IGTOrientedPointGeometry oPointGeom = GTClassFactory.Create<IGTOrientedPointGeometry>();

				int iSymbol = 0;
				foreach(AssetHistory objToUse in objHistList)
				{
					IGTPoint apoint = GTClassFactory.Create<IGTPoint>();
					apoint.X = objToUse.OGG_X1;
					apoint.Y = objToUse.OGG_Y1;
					apoint.Z = objToUse.OGG_Z1;

					oPointGeom.Origin = apoint;

					arrSymbolGeoms = new IGTGeometry[1];
					arrSymbolGeoms[0] = (IGTGeometry)oPointGeom;

					arrStyleIDs = new object[1];
					arrStyleIDs[0] = this.oGenParams.HistoricalSymbol;

					string sToolTip = "FID " + objToUse.G3E_FID.ToString();
					arrToolTips = new object[1];
					arrToolTips[0] = sToolTip;

					sDisplayName = sToolTip + " " + iSymbol.ToString();


					this.oDisplay.AppendTemporaryGeometries(this.sDisplayPathName, sDisplayName, arrSymbolGeoms, arrStyleIDs, arrToolTips, true, true);
					iSymbol += 1;
				}

				this.oApp.RefreshWindows();
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "DoSymbolGeometry", ex);
				throw ex;
			}

		}

		private void DoLineGeometryByFID(List<AssetHistory> objHistList, int iFID)
		{
			try
			{
				IGTGeometry[] arrGeoms;
				object[] arrStyles;
				object[] arrToolTips;
				string sDisplayName = string.Empty;

				IGTPolylineGeometry oPolyLine = GTClassFactory.Create<IGTPolylineGeometry>();

				string sFID = iFID.ToString();


				foreach(AssetHistory objToUse in objHistList)
				{
					IGTPoint apoint = GTClassFactory.Create<IGTPoint>();

					apoint.X = objToUse.OGG_X1;
					apoint.Y = objToUse.OGG_Y1;
					apoint.Z = objToUse.OGG_Z1;

					oPolyLine.Points.Add(apoint);
				}

				if(oPolyLine.Points.Count > 0)
				{
					arrGeoms = new IGTGeometry[1];
					arrStyles = new object[1];
					arrToolTips = new object[1];

					arrGeoms[0] = (IGTGeometry)oPolyLine;
					arrStyles[0] = this.oGenParams.HistoricalSymbol;
					arrToolTips[0] = "FID " + sFID;
					sDisplayName = "Line FID " + sFID;


					this.oDisplay.AppendTemporaryGeometries(sDisplayPathName, sDisplayName, arrGeoms, arrStyles, arrToolTips, true, true);
				}

				this.oApp.RefreshWindows();
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "CCStreetLightTracker.DoLineGeometry", ex);
				throw ex;
			}

		}

		private int GetTotalDistance(GTActiveStreetLight objSL)
		{
			int iTotalDistance = 0;

			try
			{
				int icounter = 0;
				for(int i = 0;i < objSL.GeometrySource.Count;i++)
				{
					if(icounter == objSL.GeometrySource.Count - 1)
					{
						break;
					}

					CommandUtilities.LogDistanceCalculationPoints(icounter, objSL, oDiag);
					iTotalDistance = iTotalDistance + CommandUtilities.GetDistance(objSL.GeometrySource[i].OGG_X1, objSL.GeometrySource[i].OGG_Y1, objSL.GeometrySource[i].OGG_Z1,
						objSL.GeometrySource[i + 1].OGG_X1, objSL.GeometrySource[i + 1].OGG_Y1, objSL.GeometrySource[i + 1].OGG_Z1, oDiag);

					icounter += 1;
				}
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "GetTotalDistance", ex);
				throw ex;
			}

			return iTotalDistance;
		}
		private void DoTempGeometries()
		{
			try
			{
				if(this.oActiveList.Count == 0)
				{
					return;
				}


				if(this.oDisplay == null)
				{
					throw new Exception("IGTDisplayService is not running.");
				}

				foreach(GTActiveStreetLight obj in this.oActiveList)
				{
					if(null == obj.GeometrySource)
					{
						continue;
					}

					if(GetTotalDistance(obj) > this.oGenParams.MovementThreshold)
					{
						DoSymbolGeometryByFID(obj.GeometrySource.OrderByDescending(d => d.ChangeDate).ToList());
						DoLineGeometryByFID(obj.GeometrySource.OrderByDescending(d => d.ChangeDate).ToList(), obj.G3E_FID);
					}
				}
			}
			catch(Exception ex)
			{
				CommandUtilities.LogException(oDiag, "DoTempGeometries", ex);
				throw ex;
			}
		}

		#endregion Temporary Geometries
	}
}
