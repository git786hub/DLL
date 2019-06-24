using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ADODB;
using Intergraph.CoordSystems;
using Intergraph.CoordSystems.Interop;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
//using Intergraph.CoordSystems;

namespace GTechnology.Oncor.CustomAPI
{
	public class fiStructureLocation : IGTFunctional
	{
		private GTArguments m_Arguments = null;
		private IGTDataContext m_DataContext = null;
		private IGTComponents m_components;
		private string m_ComponentName;
		private string m_FieldName;

		public GTArguments Arguments
		{
			get { return m_Arguments; }
			set { m_Arguments = value; }
		}

		public string ComponentName
		{
			get { return m_ComponentName; }
			set { m_ComponentName = value; }
		}

		public IGTComponents Components
		{
			get { return m_components; }
			set { m_components = value; }
		}

		public IGTDataContext DataContext
		{
			get { return m_DataContext; }
			set { m_DataContext = value; }
		}

		public void Delete()
		{

		}

		public void Execute()
		{
			
			double dXoord = 0.0;
			double dYoord = 0.0;
			double dZoord = 0.0;
			IGTGeometry geometry = null;
			  IGTDDCKeyObjects oDDCKeyObjects = default(IGTDDCKeyObjects);
   
			IGTApplication igtApp = GTClassFactory.Create<IGTApplication>();
			oDDCKeyObjects = igtApp.SelectedObjects.GetObjects();

			foreach (IGTDDCKeyObject item in oDDCKeyObjects)
			{
				geometry= item.Geometry;
			}

			if (geometry != null)
			{
				dXoord = ((IGTPointGeometry)geometry).FirstPoint.X;
				dYoord = ((IGTPointGeometry)geometry).FirstPoint.Y;


				IGTPoint point = GTClassFactory.Create<IGTPoint>();
				point.X = dXoord;
				point.Y = dYoord;
				point.Z = 0.0;

				IGTComponent commonComponent = Components.GetComponent(1);
				if (commonComponent != null)
				{
					if (commonComponent.Recordset.RecordCount > 0)
					{
						commonComponent.Recordset.MoveFirst();
						commonComponent.Recordset.Fields["OGGX_H"].Value = point.X;
						commonComponent.Recordset.Fields["OGGY_H"].Value = point.Y;
					}
				}
				//ICoordSystemsMgr oCoordSystemsMgr;
				//oCoordSystemsMgr = (ICoordSystemsMgr)m_oDataContext.CoordSystemsMgr;
				//ICoordSystem oCoordSystem;
				//oCoordSystem = oCoordSystemsMgr.BaseCoordSystem;
				//double dMeters2Database = 1.0 / oCoordSystem.StorageToProjectionScale;




				//string strCurrDir = System.IO.Directory.GetCurrentDirectory();	
				//ICoordSystemsMgr iCSM = new Intergraph.CoordSystems.CoordSystemsMgr();
				//ICoordSystem iCS = iCSM.BaseCoordSystem;
				//iCS.LoadFromFile(strCurrDir + "\\StatePlane.csf");

				//ICoordSystem iTargCS = new Intergraph.CoordSystems.CoordSystem();
				
				//	//iTargCS.LoadFromFile(strCurrDir + "\\EPSG4326.csf");  
				//iTargCS.LoadFromFile(strCurrDir + "\\0061_WGS84.csf");  
				//IAltCoordSystemPath iACSP = iCSM.CreateNamedPath("CSFFileConvert");
				//ILinkableTransformation iLink = iACSP.CreateLinkableTransformation("CSFFileConvert", Intergraph.CoordSystems.CSTransformLinkConstants.cstlCoordinateSystem);
				//ICoordSystem altCS = (ICoordSystem)iLink;
				//((ICopyable<ICoordSystem>)iTargCS).CopyInto(altCS);
				//iACSP.AddChainLink(iLink, true, Intergraph.CoordSystems.CSTransDirectionConstants.cstdForward);
				//ILinkableTransformation iTrans = (ILinkableTransformation)iACSP;
				//iTrans.TransformPoint(Intergraph.CoordSystems.CSPointConstants.cspENU, 1, Intergraph.CoordSystems.CSPointConstants.cspLLG, 2, ref dXoord, ref dYoord, ref dZoord);
				//MessageBox.Show(dXoord + "   " + dYoord);

				//---------------------------------------------------------------------------------------------------------------------------

				Intergraph.CoordSystems.Interop.CoordSystemClass coords = new CoordSystemClass();
				int outrec = 0;
                ADODB.Recordset rs = DataContext.Execute("select c.*  from g3e_dataconnection_optable d , gcoordsystemtable  c where " +
                    " d.g3e_username ='" + DataContext.ConfigurationName + "' and d.g3e_csname=c.name", out outrec, (int)ADODB.CommandTypeEnum.adCmdText, new object[0]);
                rs.MoveFirst();
                object[] rowformat = new object[rs.Fields.Count];
                for (int ifld = 0; ifld < rs.Fields.Count; ifld++)
                {
                    rowformat[ifld] = rs.Fields[ifld].Value;
                }
                coords.LoadFromGCoordSystemTableRowFormat(rowformat);


				coords.TransformPoint(Intergraph.CoordSystems.Interop.CSPointConstants.cspUOR, (int)Intergraph.CoordSystems.CSTransformLinkConstants.cstlDatumTransformation,
									Intergraph.CoordSystems.Interop.CSPointConstants.cspLLO, (int)Intergraph.CoordSystems.CSTransformLinkConstants.cstlDatumTransformation,
									ref dXoord, ref dYoord, ref dZoord);

				dXoord = dXoord * 180 / (4 * Math.Atan(1));
				dYoord = dYoord * 180 / (4 * Math.Atan(1));
				
			}

		}

		public string FieldName
		{
			get { return m_FieldName; }
			set { m_FieldName = value; }
		}

		public IGTFieldValue FieldValueBeforeChange
		{
			get;
			set;

		}

		public GTFunctionalTypeConstants Type
		{
			get;
			set;
		}

		public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
		{
			ErrorMessageArray = null;
			ErrorPriorityArray = null;
		}

		private string GetToCoordinatePath()
		{
			try
			{
				string strCurrDir = System.IO.Directory.GetCurrentDirectory();
				return strCurrDir + "\\EPSG4326.csf";
			}
			catch (Exception ex)
			{

				throw;
			}
		}
	}
}


