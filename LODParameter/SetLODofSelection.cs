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
    internal class SetLODofSelection
	{
		private SetLODform setLODform;

		private AffectedViewRangeForm affectedRangeForm;

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UIApplication val = commandData.get_Application();
			Document val2 = val.get_ActiveUIDocument().get_Document();
			Selection val3 = val.get_ActiveUIDocument().get_Selection();
			try
			{
				ICollection<ElementId> elementIds = val3.GetElementIds();
				if (elementIds.Count == 0)
				{
					IList<Reference> refs = val3.PickObjects(1, "Select target elements for LOD change");
					elementIds = LODapp.GetElementIds(val2, refs);
				}
				View viewFromElements = GetViewFromElements(val2, elementIds);
				IList<Element> list;
				if (viewFromElements == null)
				{
					IList<Element> elements2 = LODapp.GetElements(val2, elementIds);
					list = elements2;
				}
				else
				{
					list = GetFilteredElementsFromView(val2, viewFromElements);
					if (list == null)
					{
						return 1;
					}
				}
				if (setLODform == null)
				{
					setLODform = new SetLODform();
				}
				setLODform.ShowDialog();
				if (setLODform.DialogResult != DialogResult.OK)
				{
					return 1;
				}
				string selectedLODtype = setLODform.SelectedLODtype;
				int selectedLODvalue = setLODform.SelectedLODvalue;
				Definition parameterDefinition = LODapp.GetParameterDefinition(val2, selectedLODtype);
				Transaction val4 = new Transaction(val2, "Set LOD");
				val4.Start();
				LODapp.SetParameterOfElements((IEnumerable<Element>)list, parameterDefinition, selectedLODvalue);
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

		private IList<Element> GetFilteredElementsFromView(Document doc, View view)
		{
			FilteredElementCollector val = new FilteredElementCollector(doc, view.get_Id());
			IList<Element> levels = GetLevels(doc);
			string[] array = new string[levels.Count];
			for (int i = 0; i < levels.Count; i++)
			{
				array[i] = levels[i].get_Name();
			}
			affectedRangeForm = new AffectedViewRangeForm(array, doc.GetUnits());
			affectedRangeForm.ShowDialog();
			if (affectedRangeForm.DialogResult != DialogResult.OK)
			{
				return null;
			}
			int topLevelIndex = affectedRangeForm.TopLevelIndex;
			int bottomLevelIndex = affectedRangeForm.BottomLevelIndex;
			double topOffset = affectedRangeForm.TopOffset;
			double bottomOffset = affectedRangeForm.BottomOffset;
			bool isTopUnlimited = affectedRangeForm.IsTopUnlimited;
			bool isBottomUnlimited = affectedRangeForm.IsBottomUnlimited;
			bool excludeColumns = affectedRangeForm.ExcludeColumns;
			double num;
			if (isTopUnlimited)
			{
				num = 1.7976931348623157E+308;
			}
			else
			{
				double num2 = levels[topLevelIndex].LookupParameter("Elevation").AsDouble();
				num = num2 + bottomOffset;
			}
			double num3;
			if (isBottomUnlimited)
			{
				num3 = -1.7976931348623157E+308;
			}
			else
			{
				double num4 = levels[bottomLevelIndex].LookupParameter("Elevation").AsDouble();
				num3 = num4 + bottomOffset;
			}
			XYZ val2 = new XYZ(-1.7976931348623157E+308, -1.7976931348623157E+308, num3);
			XYZ val3 = new XYZ(1.7976931348623157E+308, 1.7976931348623157E+308, num);
			Outline val4 = new Outline(val2, val3);
			BoundingBoxIntersectsFilter val5 = new BoundingBoxIntersectsFilter(val4);
			val.WherePasses(val5);
			if (excludeColumns)
			{
				ElementCategoryFilter val6 = new ElementCategoryFilter(-2001330, true);
				val.WherePasses(val6);
			}
			return val.ToElements();
		}

		private static View GetViewFromElements(Document doc, ICollection<ElementId> ids)
		{
			foreach (ElementId id in ids)
			{
				Element val = doc.GetElement(id);
				if (val is View)
				{
					return val as View;
				}
				if (val is Viewport)
				{
					Viewport val2 = val as Viewport;
					return doc.GetElement(val2.get_ViewId()) as View;
				}
			}
			if (!IsNonDetailViewSelected(doc, ids))
			{
				FilteredElementCollector val3 = new FilteredElementCollector(doc, ids);
				val3.OfCategory(-2000278);
				if (val3.GetElementCount() > 0)
				{
					Element val4 = val3.FirstElement();
					FilteredElementCollector val5 = new FilteredElementCollector(doc);
					val5.OfCategory(-2000279);
					string name = val4.get_Name();
					View val6 = null;
					foreach (Element item in val5)
					{
						if (item.get_Name() == name)
						{
							_003F val7 = item;
							val6 = val7;
							return val7;
						}
					}
				}
				return null;
			}
			return null;
		}

		private static bool IsNonDetailViewSelected(Document doc, ICollection<ElementId> selectedIds)
		{
			FilteredElementCollector val = new FilteredElementCollector(doc, selectedIds);
			ElementCategoryFilter val2 = new ElementCategoryFilter(-2000278, true);
			val.WherePasses(val2);
			return ((IEnumerable<Element>)val).Count() > 0;
		}

		private static IList<Element> GetLevels(Document doc)
		{
			FilteredElementCollector val = new FilteredElementCollector(doc);
			val.OfClass(typeof(Level));
			return val.ToElements();
		}
	}
}
