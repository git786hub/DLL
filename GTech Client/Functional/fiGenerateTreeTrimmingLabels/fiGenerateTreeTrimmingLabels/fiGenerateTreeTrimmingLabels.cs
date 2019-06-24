//----------------------------------------------------------------------------+
//        Class: fiGenerateTreeTrimmingLabels
//  Description: This interface will generate Tree-Trimming Work Label components at all Work Points for the active job contained by the boundary, and delete any labels at Work Points which are no longer within the boundary.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                                       $
//       $Date:: 31/07/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiGenerateTreeTrimmingLabels.cs                              $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 31/07/17    Time: 18:00  Desc: Created
// User: hkonda     Date: 12/09/17    Time: 18:00  Desc: Removed aligment for tree-trimming work point label.
//----------------------------------------------------------------------------+

using System;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
	public class fiGenerateTreeTrimmingLabels : IGTFunctional
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
			int fId = 0;
			short fNo = 0;
			short treeTrimmingGraphicCno = 19002;
			short workPointFno = 191;
			short treeTrimmingFno = 190;
			short workPointGcNo = 19102;
			IGTOrientedPointGeometry workPointGeometry = null; ;
			IGTKeyObject treeTrimmingFeature = null;
			IGTKeyObject workPointFeature = null;
			IGTComponent mainComponent = null;
			short cid = 0;

			try
			{
				IGTSpatialService spatialService = GTClassFactory.Create<IGTSpatialService>();
				spatialService.DataContext = m_DataContext;
				spatialService.Operator = GTSpatialOperatorConstants.gtsoEntirelyContains;
				spatialService.FilterGeometry = Components.GetComponent(treeTrimmingGraphicCno).Geometry;
				Recordset rs = spatialService.GetResultsByFNO(new short[] { workPointFno });
				if (rs != null)
				{

					IGTComponent tTVGraphic = Components.GetComponent(treeTrimmingGraphicCno);
					int tTVFid = Convert.ToInt32(tTVGraphic.Recordset.Fields["G3E_FID"].Value);
					treeTrimmingFeature = DataContext.OpenFeature(treeTrimmingFno, tTVFid);
					DeleteWorkLabelGeometry(treeTrimmingFeature);

					if (rs.RecordCount > 0)
					{
						rs.MoveFirst();
						while (!rs.EOF)
						{
							fId = Convert.ToInt32(rs.Fields["G3E_FID"].Value.ToString());
							workPointFeature = DataContext.OpenFeature(workPointFno, fId);
							mainComponent = workPointFeature.Components.GetComponent(workPointGcNo);
							mainComponent.Recordset.MoveFirst();
							workPointGeometry = (IGTOrientedPointGeometry)mainComponent.Geometry;
							cid = (short)(cid + 1);
							CreateWorkLabelGeometry(workPointGeometry, treeTrimmingFeature, cid);
							rs.MoveNext();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error during Tree Trimming Labels execution." + ex.Message, "G/Technology");
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

		/// <summary>
		/// Method to create Work Label geometry
		/// </summary>
		/// <param name="wpGeometry"> WorkPoint geometry</param>
		/// <param name="tTVfeature"> Tree trimming feature </param>
		/// <param name="cid">cid</param>
		private void CreateWorkLabelGeometry(IGTOrientedPointGeometry wpGeometry, IGTKeyObject tTVfeature, short cid)
		{
			try
			{
				IGTTextPointGeometry iGtTxtPointGeometry = GTClassFactory.Create<IGTTextPointGeometry>();
				IGTPoint textPoint = GTClassFactory.Create<IGTPoint>();
				textPoint.X = wpGeometry.Origin.X + Convert.ToDouble(m_Arguments.GetArgument(0));
				textPoint.Y = wpGeometry.Origin.Y + Convert.ToDouble(m_Arguments.GetArgument(1));
				textPoint.Z = 0;
				iGtTxtPointGeometry.Origin = textPoint;

				IGTComponent igtComponent = tTVfeature.Components.GetComponent(19006);

				if (igtComponent.Recordset.RecordCount != 0)
				{
					igtComponent.Recordset.MoveLast();
				}

				igtComponent.Recordset.AddNew("G3E_FID", tTVfeature.FID);
				igtComponent.Recordset.Fields["G3E_FNO"].Value = 190;
				igtComponent.Recordset.Fields["G3E_CNO"].Value = 19006;
				igtComponent.Recordset.Fields["G3E_CID"].Value = cid;
				igtComponent.Geometry = iGtTxtPointGeometry;
				igtComponent.Recordset.Update(System.Type.Missing, System.Type.Missing);
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Method to delete work label geometry
		/// </summary>
		/// <param name="currentFeature"> Tree trimming feature</param>
		/// <returns></returns>
		private bool DeleteWorkLabelGeometry(IGTKeyObject currentFeature)
		{
			Recordset rs = null;
			try
			{
				IGTComponent igtComp = currentFeature.Components.GetComponent(19006);
				if (igtComp != null)
				{
					rs = currentFeature.Components.GetComponent(19006).Recordset;
					if (rs != null && rs.RecordCount > 0)
					{
						rs.MoveFirst();
						while (!rs.EOF)
						{
							rs.Delete();
							rs.MoveNext();
						}
					}
				}
				return true;
			}
			catch (Exception)
			{
				throw;
			}
		}

	}
}
