using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LODParameter
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	public class FilterByLOD
	{
		private LODfilterForm lodFilterForm;

		private static string[] filterNames = new string[4]
		{
			"LOD Equals 200",
			"LOD Equals 300",
			"LOD Equals 350",
			"LOD Equals 400"
		};

		private static int[] lodValues = new int[4]
		{
			200,
			300,
			350,
			400
		};

		private static Color[] lodFilterColors = (Color[])new Color[4]
		{
			new Color(byte.MaxValue, (byte)128, (byte)64),
			new Color((byte)0, (byte)128, (byte)64),
			new Color((byte)0, (byte)64, (byte)128),
			new Color((byte)64, (byte)0, (byte)128)
		};

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UIApplication val = commandData.get_Application();
			Document val2 = val.get_ActiveUIDocument().get_Document();
			Selection val3 = val.get_ActiveUIDocument().get_Selection();
			try
			{
				if (lodFilterForm == null)
				{
					lodFilterForm = new LODfilterForm();
				}
				lodFilterForm.ShowDialog();
				if (lodFilterForm.DialogResult != DialogResult.OK)
				{
					return 1;
				}
				bool[] fillColorEnabled = lodFilterForm.GetFillColorEnabled();
				bool[] lineColorEnabled = lodFilterForm.GetLineColorEnabled();
				bool[] visibilitesEnabled = lodFilterForm.GetVisibilitesEnabled();
				int[] transparencies = lodFilterForm.GetTransparencies();
				CreateFiltersIfMissing(val2);
				IList<ElementId> list = ApplyLODfiltersToView(val2, val2.get_ActiveView());
				Transaction val4 = new Transaction(val2, "Apply LOD filters");
				val4.Start();
				View val5 = val2.get_ActiveView();
				for (int i = 0; i < filterNames.Length; i++)
				{
					OverrideGraphicSettings val6 = new OverrideGraphicSettings();
					if (lineColorEnabled[i])
					{
						val6.SetCutLineColor(lodFilterColors[i]);
						val6.SetProjectionLineColor(lodFilterColors[i]);
					}
					ElementId solidFillId = GetSolidFillId(val2);
					if (fillColorEnabled[i])
					{
						val6.SetCutForegroundPatternColor(lodFilterColors[i]);
						val6.SetCutBackgroundPatternColor(lodFilterColors[i]);
						val6.SetSurfaceForegroundPatternColor(lodFilterColors[i]);
						val6.SetSurfaceBackgroundPatternColor(lodFilterColors[i]);
						val6.SetSurfaceForegroundPatternId(solidFillId);
						val6.SetSurfaceBackgroundPatternId(solidFillId);
					}
					val6.SetSurfaceTransparency(transparencies[i]);
					val5.SetFilterOverrides(list[i], val6);
					val5.SetFilterVisibility(list[i], visibilitesEnabled[i]);
				}
				val4.Commit();
			}
			catch (OperationCanceledException)
			{
				return 1;
			}
			catch (Exception ex)
			{
				message = ex.Message;
				return -1;
			}
			return 0;
		}

		private IList<ElementId> CreateFiltersIfMissing(Document doc)
		{
			bool[] array = new bool[4];
			FilteredElementCollector val = new FilteredElementCollector(doc);
			val.OfClass(typeof(ParameterFilterElement));
			ICollection<ElementId> collection = val.ToElementIds();
			foreach (ElementId item2 in collection)
			{
				Element val2 = doc.GetElement(item2);
				for (int i = 0; i < filterNames.Length; i++)
				{
					if (val2.get_Name() == filterNames[i])
					{
						array[i] = true;
					}
				}
			}
			if (array.Any((bool b) => !b))
			{
				ElementId val3 = LODapp.GetLODparameter(doc, "Current_LOD").get_Id();
				IList<ElementId> list = (from cat in LODapp.lodCatArray
				where (int)cat != -2000220
				select new ElementId(cat)).ToList();
				Transaction val4 = new Transaction(doc, "Create LOD filters");
				try
				{
					val4.Start();
					for (int j = 0; j < filterNames.Length; j++)
					{
						if (!array[j])
						{
							ParameterFilterElement val5 = ParameterFilterElement.Create(doc, filterNames[j], (ICollection<ElementId>)list);
							FilterRule item = ParameterFilterRuleFactory.CreateEqualsRule(val3, lodValues[j]);
							val5.SetRules((IList<FilterRule>)new List<FilterRule>(1)
							{
								item
							});
						}
					}
					val4.Commit();
				}
				catch (Exception innerException)
				{
					val4.RollBack();
					throw new Exception("Failed to create filters.", innerException);
				}
			}
			return GetLODfilters(doc);
		}

		private IList<ElementId> ApplyLODfiltersToView(Document doc, View view)
		{
			bool[] array = new bool[4];
			ICollection<ElementId> filters = view.GetFilters();
			foreach (ElementId item in filters)
			{
				Element val = doc.GetElement(item);
				for (int i = 0; i < filterNames.Length; i++)
				{
					if (val.get_Name() == filterNames[i])
					{
						array[i] = true;
					}
				}
			}
			IList<ElementId> lODfilters = GetLODfilters(doc);
			Transaction val2 = new Transaction(doc, "Add LOD filters to view");
			try
			{
				val2.Start();
				for (int j = 0; j < filterNames.Length; j++)
				{
					if (!array[j])
					{
						view.AddFilter(lODfilters[j]);
					}
				}
				val2.Commit();
			}
			catch (Exception innerException)
			{
				val2.RollBack();
				throw new Exception("Failed to add LOD filters to view", innerException);
			}
			return lODfilters;
		}

		private IList<ElementId> GetLODfilters(Document doc)
		{
			FilteredElementCollector val = new FilteredElementCollector(doc);
			val.OfClass(typeof(ParameterFilterElement));
			ICollection<ElementId> source = val.ToElementIds();
			IList<ElementId> list = (from id in source
			where doc.GetElement(id).get_Name().StartsWith("LOD Equals ")
			select id).ToList();
			if (list.Count < 4)
			{
				throw new InvalidOperationException("Must create LOD filters before querying them");
			}
			return list;
		}

		private ElementId GetSolidFillId(Document doc)
		{
			FilteredElementCollector val = new FilteredElementCollector(doc);
			val.OfClass(typeof(FillPatternElement));
			foreach (Element item in val)
			{
				if (item.get_Name() == "<Solid fill>")
				{
					return item.get_Id();
				}
			}
			throw new Exception("Could not find Solid fill Element in document");
		}
	}
}
