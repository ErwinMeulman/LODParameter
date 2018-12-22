using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LODParameter
{
	public class ZoneModifiedUpdater
	{
		private class EqualUniqueId : EqualityComparer<Element>
		{
			public override bool Equals(Element x, Element y)
			{
				return x.get_UniqueId().Equals(y.get_UniqueId());
			}

			public override int GetHashCode(Element obj)
			{
				return obj.get_UniqueId().GetHashCode();
			}
		}

		private static UpdaterId m_updaterId;

		public bool Paused
		{
			get;
			set;
		} = false;


		public ZoneModifiedUpdater(AddInId id)
		{
			m_updaterId = new UpdaterId(id, new Guid("FBFDF6B2-4C10-42e7-97C1-D1B7EB593EFF"));
		}

		public void Execute(UpdaterData data)
		{
			if (!Paused)
			{
				Document doc = data.GetDocument();
				List<ElementId> list = data.GetAddedElementIds().ToList();
				list.AddRange(data.GetModifiedElementIds());
				IList<Element> list2 = (from ElementId id in list
				select doc.GetElement(id)).ToList();
				Definition parameterDefinition = LODapp.GetParameterDefinition(doc, "Zone");
				ElementId val = LODapp.GetLODparameter(doc, parameterDefinition).get_Id();
				ParameterValueProvider val2 = new ParameterValueProvider(val);
				FilterStringRuleEvaluator val3 = new FilterStringContains();
				FilterStringRuleEvaluator val4 = new FilterStringEquals();
				FilterStringRuleEvaluator val5 = new FilterStringBeginsWith();
				foreach (Element item in list2)
				{
					string text = item.LookupParameter("Name").AsString();
					if (!string.IsNullOrWhiteSpace(text))
					{
						FilterRule[] array = (FilterRule[])new FilterRule[3]
						{
							new FilterStringRule(val2, val4, text, true),
							new FilterStringRule(val2, val5, text + ", ", true),
							new FilterStringRule(val2, val3, ", " + text, true)
						};
						ElementParameterFilter val6 = new ElementParameterFilter((IList<FilterRule>)array);
						IList<Element> list3 = new FilteredElementCollector(doc).WhereElementIsNotElementType().WherePasses(val6).ToElements();
						BoundingBoxIntersectsFilter val7 = new BoundingBoxIntersectsFilter(ToOutline(item.get_BoundingBox(null)));
						IList<Element> list4 = new FilteredElementCollector(doc).WhereElementIsNotElementType().WherePasses(val7).ToElements();
						IEnumerable<Element> enumerable = list3.Except(list4, new EqualUniqueId());
						IEnumerable<Element> enumerable2 = list4.Except(list3, new EqualUniqueId());
						foreach (Element item2 in enumerable)
						{
							Parameter val8 = item2.get_Parameter(parameterDefinition);
							if (val8 != null)
							{
								string text2 = val8.AsString() ?? string.Empty;
								string text3;
								if (text2.Length > text.Length)
								{
									int num = text2.IndexOf(text);
									text3 = ((num < 2 || text2[num - 2] != ',') ? text2.Remove(num, text.Length + 2) : text2.Remove(num - 2, text.Length + 2));
								}
								else
								{
									text3 = string.Empty;
								}
								val8.Set(text3);
							}
						}
						foreach (Element item3 in enumerable2)
						{
							Parameter val9 = item3.get_Parameter(parameterDefinition);
							string text4 = (((int)val9 != 0) ? val9.AsString() : null) ?? string.Empty;
							string text5 = (text4.Length <= 0) ? text : (text4 + ", " + text);
							Parameter val10 = item3.get_Parameter(parameterDefinition);
							if ((int)val10 != 0)
							{
								val10.Set(text5);
							}
						}
					}
				}
			}
		}

		public string GetAdditionalInformation()
		{
			return "Zone Modified Updater: updates zone parameter of elements when a zone is modified";
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
			return "ZoneModifiedUpdater";
		}

		private static Outline ToOutline(BoundingBoxXYZ bb)
		{
			return new Outline(bb.get_Min(), bb.get_Max());
		}
	}
}
