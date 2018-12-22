using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LODParameter
{
	public class LODParameterUpdater
	{
		private readonly int[] VALID_CURRENT_LOD_VALUES = new int[5]
		{
			100,
			200,
			300,
			350,
			400
		};

		private const bool BLANK_ALLOWED_CURRENT_LOD = false;

		private readonly int[] VALID_TARGET_LOD_VALUES = new int[5]
		{
			100,
			200,
			300,
			350,
			400
		};

		private const bool BLANK_ALLOWED_TARGET_LOD = true;

		private const int FALLBACK_CURRENT_LOD = 200;

		private const int FALLBACK_TARGET_LOD = 200;

		private static UpdaterId m_updaterId;

		private ElementFilter curLODfilter;

		private ElementFilter tarLODfilter;

		public LODParameterUpdater(AddInId id)
		{
			m_updaterId = new UpdaterId(id, new Guid("FBFBF6B2-4C07-42e7-97C1-D1B4EB593EFF"));
		}

		public void Execute(UpdaterData data)
		{
			Document val = data.GetDocument();
			ICollection<ElementId> modifiedElementIds = data.GetModifiedElementIds();
			Definition curLODdef = LODapp.GetParameterDefinition(val, "Current_LOD");
			Definition tarLODdef = LODapp.GetParameterDefinition(val, "Target_LOD");
			CreateFiltersIfMissing(val);
			IEnumerable<Element> enumerable = new FilteredElementCollector(val, modifiedElementIds).WherePasses(curLODfilter).ToElements();
			bool flag = false;
			IEnumerable<Element> source = new FilteredElementCollector(val, modifiedElementIds).WherePasses(tarLODfilter).ToElements();
			bool flag2 = true;
			source = from e in source
			where e.get_Parameter(tarLODdef).get_HasValue()
			select e;
			if (enumerable.Count() > 0)
			{
				int num = enumerable.First().get_Parameter(curLODdef).AsInteger();
				string text = "Invalid Current_LOD Value: " + num;
				TaskDialog.Show("Invalid Value", text);
				foreach (Element item in enumerable)
				{
					item.get_Parameter(curLODdef).Set(200);
				}
			}
			if (source.Count() > 0)
			{
				int num2 = source.First().get_Parameter(tarLODdef).AsInteger();
				string text2 = "Invalid Target_LOD Value: " + num2;
				TaskDialog.Show("Invalid Value", text2);
				foreach (Element item2 in source)
				{
					item2.get_Parameter(tarLODdef).Set(200);
				}
			}
		}

		public string GetAdditionalInformation()
		{
			return "LOD Parameter Validator: ensures LOD parameters are set to a legal value";
		}

		public ChangePriority GetChangePriority()
		{
			return 16;
		}

		public UpdaterId GetUpdaterId()
		{
			return m_updaterId;
		}

		public string GetUpdaterName()
		{
			return "LOD Parameter Validator";
		}

		private bool CreateFiltersIfMissing(Document doc)
		{
			if (curLODfilter != null && tarLODfilter != null)
			{
				return false;
			}
			ElementId val = LODapp.GetLODparameter(doc, "Current_LOD").get_Id();
			ElementId val2 = LODapp.GetLODparameter(doc, "Target_LOD").get_Id();
			ParameterValueProvider val3 = new ParameterValueProvider(val);
			ParameterValueProvider val4 = new ParameterValueProvider(val2);
			FilterNumericRuleEvaluator val5 = new FilterNumericEquals();
			ElementParameterFilter[] array = (ElementParameterFilter[])new ElementParameterFilter[VALID_CURRENT_LOD_VALUES.Length];
			for (int i = 0; i < VALID_CURRENT_LOD_VALUES.Length; i++)
			{
				FilterIntegerRule val6 = new FilterIntegerRule(val3, val5, VALID_CURRENT_LOD_VALUES[i]);
				array[i] = new ElementParameterFilter(val6, true);
			}
			curLODfilter = new LogicalAndFilter((IList<ElementFilter>)array);
			ElementParameterFilter[] array2 = (ElementParameterFilter[])new ElementParameterFilter[VALID_TARGET_LOD_VALUES.Length];
			for (int j = 0; j < VALID_TARGET_LOD_VALUES.Length; j++)
			{
				FilterIntegerRule val7 = new FilterIntegerRule(val4, val5, VALID_TARGET_LOD_VALUES[j]);
				array2[j] = new ElementParameterFilter(val7, true);
			}
			tarLODfilter = new LogicalAndFilter((IList<ElementFilter>)array2);
			return true;
		}
	}
}
