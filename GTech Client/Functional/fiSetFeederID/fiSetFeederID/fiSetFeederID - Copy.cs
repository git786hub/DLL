//----------------------------------------------------------------------------+
//        Class: fiSetFeederID
//  Description: This interface sets the Feeder ID for a Substation Breaker by concatenating the Substation Code and Feeder Number attributes.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                                       $
//       $Date:: 18/08/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History:: fiSetFeederID.cs                                       $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 18/08/17    Time: 18:00
// User: hkonda     Date: 09/09/17    Time: 18:00  Desc : Fixed recordset issue during sanity testing
//----------------------------------------------------------------------------+


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
	public class fiSetFeederID : IGTFunctional
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
			Recordset subStationAttributesRs = null;
			Recordset subStationBreakerAttributeRs = null;
			Recordset subStationBreakerConnectivityAttributeRs = null;
			string subStationCode = string.Empty;
			int feederNumber = 0;
			try
			{
				subStationBreakerAttributeRs = Components[ComponentName].Recordset;
				//int fId = Convert.ToInt32(subStationBreakerAttributeRs.Fields["G3E_FID"].Value);
				//short fNo = Convert.ToInt16(subStationBreakerAttributeRs.Fields["G3E_FNO"].Value);
				//IGTKeyObject oActiveFeature = m_DataContext.OpenFeature(fNo,fId);

				//IGTRelationshipService service = GTClassFactory.Create<IGTRelationshipService>();
				//service.DataContext = m_DataContext;
				//service.ActiveFeature = oActiveFeature;
				//IGTKeyObjects ownerFeatures = service.GetRelatedFeatures(16);
				//foreach (IGTKeyObject owner in ownerFeatures)
				//{
				//	if (owner.FNO == 115) // if owner is SubStation
				//	{
				//		subStationAttributesRs = owner.Components.GetComponent(11501).Recordset;
				//		if (subStationAttributesRs.RecordCount > 0)
				//		{
				//			subStationAttributesRs.MoveFirst();
				//			subStationCode = Convert.ToString(subStationAttributesRs.Fields["SUBSTATION_C"].Value);
				//			break;
				//		}
				//	}
				//}

				if (subStationBreakerAttributeRs.RecordCount > 0)
				{
					subStationBreakerAttributeRs.MoveFirst();
					feederNumber = Convert.ToInt32(subStationBreakerAttributeRs.Fields["FEEDER_NBR"].Value);
				}

				subStationBreakerConnectivityAttributeRs = Components.GetComponent(11).Recordset;

				if (subStationBreakerConnectivityAttributeRs.RecordCount > 0)
				{
					subStationBreakerConnectivityAttributeRs.MoveFirst();
					subStationCode = Convert.ToString(subStationBreakerConnectivityAttributeRs.Fields["SSTA_C"].Value);
					subStationBreakerConnectivityAttributeRs.Fields["FEEDER_1_ID"].Value = subStationCode + feederNumber;
					subStationBreakerConnectivityAttributeRs.Update(System.Type.Missing, System.Type.Missing);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error during Set Feeder ID execution. " + ex.Message, "G/Technology");
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
	}
}
