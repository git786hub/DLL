using ADODB;
using GTechnology.Oncor.CustomAPI.Model;
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTechnology.Oncor.CustomAPI.DataAccess
{
	/// <summary>
	/// Methods in the class to manage Non Located Street Lights
	/// </summary>
	public class ManageNonLocatedSTLTContext : IDisposable
	{
		IGTApplication _gtApp = null;
		IGTRelationshipService _gtRelationShipService = null;

		int miscStructG3eFid;

		public int MiscStructG3eFid { get => miscStructG3eFid; set => miscStructG3eFid = value; }

		public ManageNonLocatedSTLTContext()
		{
			_gtApp = GTClassFactory.Create<IGTApplication>();
			_gtRelationShipService = GTClassFactory.Create<IGTRelationshipService>();
			_gtRelationShipService.DataContext = _gtApp.DataContext;
        }

		#region Public Methods

		/// <summary>
		/// Get all Street Light feature instance for given input esilocation
		/// </summary>
		/// <param name="esiLocation"></param>
		/// <returns></returns>
		public BindingList<StreetLightNonLocated> GetStreetLightByAccountID(string esiLocation)
		{
			string sqlStmt = "SELECT S.G3E_FID AS G3EFID,S.G3E_FNO AS G3EFNO,S.CO_IDENTIFIER AS StltIdentifier,S.CONNECTION_STATUS_C AS ConnectionStatus" +
					",S.CONNECT_D AS ConnectDate,S.DISCONNECT_D AS DisconnectDate,C.LOCATION AS Location,S.LOCATION_ADDL AS AdditionalLocation " +
					"FROM STREETLIGHT_N S,COMMON_N C WHERE S.G3E_FID=C.G3E_FID AND S.ACCOUNT_ID='{0}'";

			BindingList<StreetLightNonLocated> streetLights = new BindingList<StreetLightNonLocated>();
			Recordset rs = CommonUtil.Execute(string.Format(sqlStmt, esiLocation));
			if(rs != null && rs.RecordCount > 0)
			{
				streetLights = CommonUtil.ConvertRSToEntity<StreetLightNonLocated>(rs);
			}
			return streetLights;
		}


		/// <summary>
		/// Place Miscellaneous Structure at center of Boundary
		/// </summary>
		/// <param name="gtKeyObject"></param>
		/// <param name="bndryG3eFid"></param>
		/// <param name="bndryG3eFno"></param>
		public void PlaceMiscStrucAtBndryCenter(IGTKeyObject gtKeyObject, int bndryG3eFid, short bndryG3eFno)
		{
			IGTGeometry bndryGeom = _gtApp.DataContext.GetDDCKeyObjects(bndryG3eFno, bndryG3eFid, GTComponentGeometryConstants.gtddcgPrimaryGeographic)[0].Geometry;
			IGTPoint gtOrigin = CalculateCentroid(bndryGeom);
			IGTPointGeometry gtPointGeom = GTClassFactory.Create<IGTPointGeometry>();
			gtPointGeom.Origin = gtOrigin;
			gtKeyObject.Components.GetComponent(CommonUtil.MiscStructGeoCno).Geometry = gtPointGeom;
		}

		/// <summary>
		/// perform add/delete Street light feature instance
		/// </summary>
		/// <param name="streetLight"></param>
		public void SaveStreetLight(StreetLightNonLocated streetLight)
		{
			if(streetLight.EntityState == EntityMode.Add) { AddStreetLight(streetLight); }
			if(streetLight.EntityState == EntityMode.Delete) { DeleteFeature(streetLight.G3efno, streetLight.G3eFid); }
		}

		/// <summary>
		/// Update newly placed Misc Structure Fid to Street light Account
		/// </summary>
		/// <param name="miscStructG3eFid"></param>
		/// <param name="esiLocation"></param>
		public void UpdateStreetLightAcct(int miscStructG3eFid, string esiLocation)
		{

			string sqlStmt = "Update STLT_ACCOUNT SET MISC_STRUCT_FID={0} WHERE ESI_LOCATION='{1}'";
			CommonUtil.Execute(String.Format(sqlStmt, miscStructG3eFid, esiLocation));
		}

		/// <summary>
		/// Create Misc Structure Feature components and return IGTKeyObject
		/// </summary>
		/// <returns></returns>
		public IGTKeyObject GetNewMiscStructure()
		{
			IGTKeyObject gtKeyObj = _gtApp.DataContext.NewFeature(CommonUtil.MiscStructG3eFno);
			//Add Misc structure Graphic Component
			IGTComponent gtGeoComponent = gtKeyObj.Components.GetComponent(CommonUtil.MiscStructGeoCno);
			gtGeoComponent.Recordset.Fields["g3e_fno"].Value = CommonUtil.MiscStructG3eFno;
			gtGeoComponent.Recordset.Fields["g3e_fid"].Value = gtKeyObj.FID;
			gtGeoComponent.Recordset.Fields["g3e_cno"].Value = CommonUtil.MiscStructGeoCno;

			//Add Misc structure attribute Component
			IGTComponent gtComponent = gtKeyObj.Components.GetComponent(CommonUtil.MiscStructAttributeCno);
			gtComponent.Recordset.Fields["g3e_fno"].Value = CommonUtil.MiscStructG3eFno;
			gtComponent.Recordset.Fields["g3e_fid"].Value = gtKeyObj.FID;
			gtComponent.Recordset.Fields["g3e_cno"].Value = CommonUtil.MiscStructAttributeCno;
			gtComponent.Recordset.Fields["TYPE_C"].Value = "SPNL";

			return gtKeyObj;

		}

        /// <summary>
        /// GET Street Light Connection Status Value List
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetConnectionStatusVL()
        {
            string sqlStmt = "select VL_KEY as key,VL_VALUE as value from VL_STLT_CONN_STATUS";
            ADODB.Recordset rs = CommonUtil.Execute(sqlStmt);
            var connStatus = CommonUtil.ConvertRSToKeyValue(rs);
            return connStatus;
        }
        #endregion


        #region Private Methods


        /// <summary>
        /// Create new feature Street Light feature instance and its feature components.
        /// Establish Ownership relationship with Miscellaneous Structure
        /// </summary>
        /// <param name="streetLight"></param>
        private void AddStreetLight(StreetLightNonLocated streetLight)
		{

			IGTKeyObject gtKeyStreetLight = _gtApp.DataContext.NewFeature(CommonUtil.StreetLightG3eFno);
			IGTGeometry gtMiscStructGeometry = _gtApp.DataContext.GetDDCKeyObjects(CommonUtil.MiscStructG3eFno, miscStructG3eFid, GTComponentGeometryConstants.gtddcgPrimaryGeographic)[0].Geometry;

			//Add Street Light At Misc Structure
			AddPointGeometry(gtKeyStreetLight, CommonUtil.StreetLightGeoCno, gtMiscStructGeometry.FirstPoint);

			//Update Street Light component attribut
			IGTComponent gtComponent = gtKeyStreetLight.Components.GetComponent(CommonUtil.StreetLightAttributeCno);
			gtComponent.Recordset.Fields["ACCOUNT_ID"].Value = streetLight.ESI_LOCATION;
			gtComponent.Recordset.Fields["CO_IDENTIFIER"].Value = streetLight.StltIdentifier;
			gtComponent.Recordset.Fields["LAMP_TYPE_C"].Value = streetLight.LAMP_TYPE;
			gtComponent.Recordset.Fields["LUMIN_STYL_C"].Value = streetLight.LUMINARE_STYLE;
			gtComponent.Recordset.Fields["WATT_Q"].Value = streetLight.Wattage;
			gtComponent.Recordset.Fields["LOCATION_ADDL"].Value = streetLight.AdditionalLocation;
			gtComponent.Recordset.Fields["CONNECTION_STATUS_C"].Value = streetLight.ConnectionStatus;
			gtComponent.Recordset.Fields["RATE_CODE_C"].Value = streetLight.RATE_CODE;
			gtComponent.Recordset.Fields["RATE_SCHEDULE_C"].Value = streetLight.RATE_SCHEDULE;
			gtComponent.Recordset.Fields["CONNECT_D"].Value = streetLight.ConnectDate;
			gtComponent.Recordset.Fields["DISCONNECT_D"].Value = streetLight.DisconnectDate;
            //ALM - 2041 - Set attribute default value to "N"
            gtComponent.Recordset.Fields["LOCATABLE_YN"].Value = "N";
            gtComponent.Recordset.Update();

			//Update Location column in common component
			IGTComponent gtCommonComponent = gtKeyStreetLight.Components.GetComponent(CommonUtil.CommonAttributeCno);
			gtCommonComponent.Recordset.Fields["LOCATION"].Value = streetLight.Location;
			gtCommonComponent.Recordset.Update();

			//ALM 2046 - Set the two attributes to their expected default values.
			IGTComponent gtConnectivityComponent = gtKeyStreetLight.Components.GetComponent(CommonUtil.ConnectivityAttributeCno);
			gtConnectivityComponent.Recordset.Fields["STATUS_NORMAL_C"].Value = "CLOSED";
			gtConnectivityComponent.Recordset.Fields["STATUS_OPERATED_C"].Value = "CLOSED";
			gtConnectivityComponent.Recordset.Update();

			//ALM 2044 - Set the CU for the current street light.
			IGTComponent gtCUComponent = gtKeyStreetLight.Components.GetComponent(CommonUtil.CUAttributeCno);
			gtCUComponent.Recordset.Fields["CU_C"].Value = streetLight.CU;
			gtCUComponent.Recordset.Update();


			_gtRelationShipService.ActiveFeature = gtKeyStreetLight;
			IGTKeyObject gtKeyMiscStruct = _gtApp.DataContext.OpenFeature(CommonUtil.MiscStructG3eFno, miscStructG3eFid);

			//Establish child to parent ownership with Miscellaneous Structure
			if(_gtRelationShipService.AllowSilentEstablish(gtKeyMiscStruct))
			{
				_gtRelationShipService.SilentEstablish(3, gtKeyMiscStruct);
			}
		}

		/// <summary>
		/// Delete Street Light feature instance for given g3efid
		/// </summary>
		/// <param name="g3efno"></param>
		/// <param name="g3eFid"></param>
		private void DeleteFeature(short g3efno, int g3eFid)
		{

			IGTKeyObject gtKeyObject = _gtApp.DataContext.OpenFeature(g3efno, g3eFid);
			foreach(IGTComponent gtComp in gtKeyObject.Components)
			{
				if(gtComp.Recordset != null && gtComp.Recordset.RecordCount > 0)
				{
					gtComp.Recordset.Delete();
					gtComp.Recordset.Update();
				}
			}
		}

		/// <summary>
		/// Add point Geometry to feaure 
		/// </summary>
		/// <param name="gtKeyObject"></param>
		/// <param name="g3eCno"></param>
		/// <param name="gtPoint"></param>
		private void AddPointGeometry(IGTKeyObject gtKeyObject, short g3eCno, IGTPoint gtPoint)
		{
			IGTComponent gtComponent = gtKeyObject.Components.GetComponent(g3eCno);
            IGTPointGeometry gtPointGeom = GTClassFactory.Create<IGTPointGeometry>();
			gtPointGeom.Origin = gtPoint;
			gtComponent.Geometry = gtPointGeom;
			gtComponent.Recordset.Update();
		}

		/// <summary>
		/// Calculate Centroid for given boundayr Geometry
		/// </summary>
		/// <param name="gtGeometry"></param>
		/// <returns></returns>
		private IGTPoint CalculateCentroid(IGTGeometry gtGeometry)
		{
			IGTPoint gtCenterPt = GTClassFactory.Create<IGTPoint>();
			gtCenterPt.X = 0;
			gtCenterPt.Y = 0;
			gtCenterPt.Z = 0;
			if(gtGeometry.KeypointCount > 1)
			{
				for(int indx = 0;indx < gtGeometry.KeypointCount - 1;indx++)
				{
					gtCenterPt.X += gtGeometry.GetKeypointPosition(indx).X;
					gtCenterPt.Y += gtGeometry.GetKeypointPosition(indx).Y;
					gtCenterPt.Z += gtGeometry.GetKeypointPosition(indx).Z;
				}
				gtCenterPt.X /= gtGeometry.KeypointCount - 1;
				gtCenterPt.Y /= gtGeometry.KeypointCount - 1;
				gtCenterPt.Z /= gtGeometry.KeypointCount - 1;
			}
			else
			{
				gtCenterPt = gtGeometry.FirstPoint;
			}
			return gtCenterPt;
		}

		#endregion

		#region Dispose
		public void Dispose()
		{
			if(_gtRelationShipService != null)
			{
				_gtRelationShipService.Dispose();
			}
			_gtRelationShipService = null;
		}


		#endregion
	}
}
