using Autodesk.Revit.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LODParameter
{
	public class ZoneData
	{
		protected internal enum Directions
		{
			North,
			South,
			East,
			West
		}

		private Dimension[] m_AssociatedDimensions = (Dimension[])new Dimension[4];

		public string Name
		{
			get;
			set;
		}

		public string UniqueId
		{
			get;
			private set;
		}

		public Level TopLevel
		{
			get;
			set;
		}

		public Level BaseLevel
		{
			get;
			set;
		}

		public Grid NorthGrid
		{
			get;
			set;
		}

		public Grid SouthGrid
		{
			get;
			set;
		}

		public Grid EastGrid
		{
			get;
			set;
		}

		public Grid WestGrid
		{
			get;
			set;
		}

		public double TopOffset
		{
			get;
			set;
		}

		public double BaseOffset
		{
			get;
			set;
		}

		public double NorthOffset
		{
			get;
			set;
		}

		public double SouthOffset
		{
			get;
			set;
		}

		public double EastOffset
		{
			get;
			set;
		}

		public double WestOffset
		{
			get;
			set;
		}

		public ZoneData()
		{
			Name = null;
			UniqueId = null;
			TopLevel = null;
			BaseLevel = null;
		}

		public ZoneData(Element zone)
		{
			Document val = zone.get_Document();
			Name = zone.LookupParameter("Name").AsString();
			UniqueId = zone.get_UniqueId();
			TopLevel = (val.GetElement(zone.LookupParameter("Top Level").AsElementId()) as Level);
			BaseLevel = (val.GetElement(zone.LookupParameter("Base Level").AsElementId()) as Level);
			TopOffset = zone.LookupParameter("Top Offset").AsDouble();
			BaseOffset = zone.LookupParameter("Base Offset").AsDouble();
			BoundingBoxXYZ val2 = zone.get_BoundingBox(null);
			double y = val2.get_Max().get_Y();
			double y2 = val2.get_Min().get_Y();
			double x = val2.get_Max().get_X();
			double x2 = val2.get_Min().get_X();
			IList<Dimension> list = LookupAssociatedDimensions(zone);
			foreach (Dimension item in list)
			{
				Grid val3 = null;
				foreach (Reference reference in item.get_References())
				{
					Reference val4 = reference;
					Element val5 = val.GetElement(val4);
					if (val5 is Grid)
					{
						val3 = (val5 as Grid);
					}
				}
				double num = item.get_Value() ?? 0.0;
				if (num == 0.0)
				{
					if (NearlyIntersects(item.get_Origin().get_Y(), y))
					{
						NorthGrid = val3;
						NorthOffset = 0.0;
						m_AssociatedDimensions[0] = item;
					}
					else if (NearlyIntersects(item.get_Origin().get_Y(), y2))
					{
						SouthGrid = val3;
						SouthOffset = 0.0;
						m_AssociatedDimensions[1] = item;
					}
					else if (NearlyIntersects(item.get_Origin().get_X(), x))
					{
						EastGrid = val3;
						EastOffset = 0.0;
						m_AssociatedDimensions[2] = item;
					}
					else if (NearlyIntersects(item.get_Origin().get_X(), x2))
					{
						WestGrid = val3;
						WestOffset = 0.0;
						m_AssociatedDimensions[3] = item;
					}
				}
				else
				{
					Line val6 = item.get_Curve() as Line;
					bool flag = val6.get_Direction().get_Y() >= 0.99 || val6.get_Direction().get_Y() <= -0.99;
					bool flag2 = val6.get_Direction().get_X() >= 0.99 || val6.get_Direction().get_X() <= -0.99;
					if (flag)
					{
						double[] array = new double[2]
						{
							item.get_Origin().get_Y() - num / 2.0,
							item.get_Origin().get_Y() + num / 2.0
						};
						for (int i = 0; i <= 1; i++)
						{
							if (NearlyIntersects(array[i], y))
							{
								NorthGrid = val3;
								NorthOffset = ((i == 1) ? num : (0.0 - num));
								m_AssociatedDimensions[0] = item;
								break;
							}
							if (NearlyIntersects(array[i], y2))
							{
								SouthGrid = val3;
								SouthOffset = ((i == 1) ? num : (0.0 - num));
								m_AssociatedDimensions[1] = item;
								break;
							}
						}
					}
					else if (flag2)
					{
						double[] array2 = new double[2]
						{
							item.get_Origin().get_X() - num / 2.0,
							item.get_Origin().get_X() + num / 2.0
						};
						for (int j = 0; j <= 1; j++)
						{
							if (NearlyIntersects(array2[j], x))
							{
								EastGrid = val3;
								EastOffset = ((j == 1) ? num : (0.0 - num));
								m_AssociatedDimensions[2] = item;
								break;
							}
							if (NearlyIntersects(array2[j], x2))
							{
								WestGrid = val3;
								WestOffset = ((j == 1) ? num : (0.0 - num));
								m_AssociatedDimensions[3] = item;
								break;
							}
						}
					}
				}
			}
			if ((object)NorthGrid == null)
			{
				NorthOffset = y;
			}
			if ((object)SouthGrid == null)
			{
				SouthOffset = y2;
			}
			if ((object)EastGrid == null)
			{
				EastOffset = x;
			}
			if ((object)WestGrid == null)
			{
				WestOffset = x2;
			}
		}

		public Element CreateRevitProjectZone(Document doc)
		{
			double westOffset = WestOffset;
			object westGrid = (object)WestGrid;
			double num = westOffset + ((westGrid != null) ? westGrid.get_Curve().GetEndPoint(0).get_X() : 0.0);
			double southOffset = SouthOffset;
			object southGrid = (object)SouthGrid;
			double num2 = southOffset + ((southGrid != null) ? southGrid.get_Curve().GetEndPoint(0).get_Y() : 0.0);
			XYZ val = new XYZ(num, num2, 0.0);
			double eastOffset = EastOffset;
			object eastGrid = (object)EastGrid;
			double num3 = eastOffset + ((eastGrid != null) ? eastGrid.get_Curve().GetEndPoint(0).get_X() : 0.0);
			double northOffset = NorthOffset;
			object northGrid = (object)NorthGrid;
			double num4 = northOffset + ((northGrid != null) ? northGrid.get_Curve().GetEndPoint(0).get_Y() : 0.0);
			double num5 = num3 - num;
			double num6 = num4 - num2;
			FamilySymbol revitZoneSymbol = GetRevitZoneSymbol(doc);
			FamilyInstance val2 = doc.get_Create().NewFamilyInstance(val, revitZoneSymbol, BaseLevel, 0);
			val2.LookupParameter("Name").Set(Name);
			val2.LookupParameter("Top Level").Set(TopLevel.get_Id());
			val2.LookupParameter("Top Offset").Set(TopOffset);
			val2.LookupParameter("Base Offset").Set(BaseOffset);
			val2.LookupParameter("Length").Set(num5);
			val2.LookupParameter("Width").Set(num6);
			doc.Regenerate();
			UniqueId = val2.get_UniqueId();
			ViewPlan viewForLevel = GetViewForLevel(BaseLevel);
			if (viewForLevel == null)
			{
				throw new Exception("No plan view exists for the base level of this project zone. A view must exist for dimensions to be created.");
			}
			Grid[] array = (Grid[])new Grid[4]
			{
				NorthGrid,
				SouthGrid,
				EastGrid,
				WestGrid
			};
			double[] array2 = new double[4]
			{
				NorthOffset,
				SouthOffset,
				EastOffset,
				WestOffset
			};
			Reference[] sideReferences = GetSideReferences(val2, viewForLevel);
			Reference[] gridReferences = GetGridReferences(array, viewForLevel);
			for (int i = 0; i < 4; i++)
			{
				if (array[i] != null)
				{
					if (array2[i] >= 1E-09 || array2[i] <= -1E-09)
					{
						ReferenceArray val3 = new ReferenceArray();
						val3.Append(sideReferences[i]);
						val3.Append(gridReferences[i]);
						Line val4 = CreateDimensionLine(val2, (Directions)i, array2[i]);
						Dimension val5 = doc.get_Create().NewDimension(viewForLevel, val4, val3);
						val5.set_IsLocked(true);
					}
					else
					{
						doc.get_Create().NewAlignment(viewForLevel, sideReferences[i], gridReferences[i]);
					}
				}
			}
			return val2;
		}

		public void UpdateRevitProjectZone(FamilyInstance projectZone)
		{
			Document val = projectZone.get_Document();
			Dimension[] associatedDimensions = m_AssociatedDimensions;
			foreach (Dimension val2 in associatedDimensions)
			{
				if (val2 != null)
				{
					val2.set_IsLocked(false);
				}
			}
			double westOffset = WestOffset;
			object westGrid = (object)WestGrid;
			double num = westOffset + ((westGrid != null) ? westGrid.get_Curve().GetEndPoint(0).get_X() : 0.0);
			double southOffset = SouthOffset;
			object southGrid = (object)SouthGrid;
			double num2 = southOffset + ((southGrid != null) ? southGrid.get_Curve().GetEndPoint(0).get_Y() : 0.0);
			XYZ val3 = new XYZ(num, num2, 0.0);
			double eastOffset = EastOffset;
			object eastGrid = (object)EastGrid;
			double num3 = eastOffset + ((eastGrid != null) ? eastGrid.get_Curve().GetEndPoint(0).get_X() : 0.0);
			double northOffset = NorthOffset;
			object northGrid = (object)NorthGrid;
			double num4 = northOffset + ((northGrid != null) ? northGrid.get_Curve().GetEndPoint(0).get_Y() : 0.0);
			double num5 = num3 - num;
			double num6 = num4 - num2;
			XYZ val4 = projectZone.get_Location().get_Point();
			XYZ val5 = val3 - val4;
			ElementTransformUtils.MoveElement(val, projectZone.get_Id(), val5);
			projectZone.LookupParameter("Name").Set(Name);
			projectZone.LookupParameter("Top Level").Set(TopLevel.get_Id());
			projectZone.LookupParameter("Base Level").Set(BaseLevel.get_Id());
			projectZone.LookupParameter("Top Offset").Set(TopOffset);
			projectZone.LookupParameter("Base Offset").Set(BaseOffset);
			projectZone.LookupParameter("Length").Set(num5);
			projectZone.LookupParameter("Width").Set(num6);
			val.Regenerate();
			ViewPlan viewForLevel = GetViewForLevel(BaseLevel);
			if (viewForLevel == null)
			{
				throw new Exception("No plan view exists for the base level of this project zone. A view must exist for dimensions to be created.");
			}
			Grid[] array = (Grid[])new Grid[4]
			{
				NorthGrid,
				SouthGrid,
				EastGrid,
				WestGrid
			};
			double[] array2 = new double[4]
			{
				NorthOffset,
				SouthOffset,
				EastOffset,
				WestOffset
			};
			Reference[] sideReferences = GetSideReferences(projectZone, viewForLevel);
			Reference[] gridReferences = GetGridReferences(array, viewForLevel);
			for (int j = 0; j < 4; j++)
			{
				if (m_AssociatedDimensions[j] != null)
				{
					bool flag = false;
					if (array[j] != null)
					{
						foreach (Reference reference in m_AssociatedDimensions[j].get_References())
						{
							Reference val6 = reference;
							ElementId val7 = val6.get_ElementId();
							if (val7 == array[j].get_Id())
							{
								flag = true;
							}
						}
					}
					if (!flag)
					{
						val.Delete(m_AssociatedDimensions[j].get_Id());
						m_AssociatedDimensions[j] = null;
					}
					if (m_AssociatedDimensions[j] != null && !m_AssociatedDimensions[j].get_Value().HasValue && (array2[j] >= 1E-09 || array2[j] <= 1E-09))
					{
						val.Delete(m_AssociatedDimensions[j].get_Id());
						m_AssociatedDimensions[j] = null;
					}
				}
				if (array[j] != null && m_AssociatedDimensions[j] == null)
				{
					if (array2[j] >= 1E-09 || array2[j] <= -1E-09)
					{
						ReferenceArray val8 = new ReferenceArray();
						val8.Append(sideReferences[j]);
						val8.Append(gridReferences[j]);
						Line val9 = CreateDimensionLine(projectZone, (Directions)j, array2[j]);
						m_AssociatedDimensions[j] = val.get_Create().NewDimension(viewForLevel, val9, val8);
					}
					else
					{
						m_AssociatedDimensions[j] = val.get_Create().NewAlignment(viewForLevel, sideReferences[j], gridReferences[j]);
					}
				}
			}
			Dimension[] associatedDimensions2 = m_AssociatedDimensions;
			foreach (Dimension val10 in associatedDimensions2)
			{
				if (val10 != null)
				{
					val10.set_IsLocked(true);
				}
			}
		}

		public static void UpdateRevitProjectZones(Document doc, ICollection<ZoneData> zoneDatas)
		{
			IList<FamilyInstance> projectZones = GetProjectZones(doc);
			bool[] array = new bool[projectZones.Count];
			foreach (ZoneData zoneData in zoneDatas)
			{
				if (zoneData.UniqueId != null)
				{
					for (int i = 0; i < projectZones.Count; i++)
					{
						if (projectZones[i].get_UniqueId() == zoneData.UniqueId)
						{
							array[i] = true;
							zoneData.UpdateRevitProjectZone(projectZones[i]);
							break;
						}
					}
				}
				else
				{
					zoneData.CreateRevitProjectZone(doc);
				}
			}
			for (int j = 0; j < projectZones.Count; j++)
			{
				if (!array[j])
				{
					doc.Delete(projectZones[j].get_Id());
				}
			}
		}

		protected internal static IList<FamilyInstance> GetProjectZones(Document doc)
		{
			FamilySymbol revitZoneSymbol = GetRevitZoneSymbol(doc);
			FamilyInstanceFilter val = new FamilyInstanceFilter(doc, revitZoneSymbol.get_Id());
			FilteredElementCollector val2 = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfCategory(-2000151).WherePasses(val);
			return val2.ToElements().Cast<FamilyInstance>().ToList();
		}

		protected internal static IList<ZoneData> GetProjectZonesAsZoneData(Document doc)
		{
			IList<FamilyInstance> projectZones = GetProjectZones(doc);
			IList<ZoneData> list = new List<ZoneData>(projectZones.Count);
			foreach (FamilyInstance item in projectZones)
			{
				list.Add(new ZoneData(item));
			}
			return list;
		}

		protected internal static FamilySymbol GetRevitZoneSymbol(Document doc)
		{
			IList<Element> list = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).ToElements();
			foreach (FamilySymbol item in list)
			{
				FamilySymbol val = item;
				if (val.get_FamilyName() == "Project Zone")
				{
					return val;
				}
			}
			string location = Assembly.GetExecutingAssembly().Location;
			string str = location.Remove(location.Length - "LODParameter.dll".Length);
			string text = str + "Project Zone.rfa";
			Transaction val2 = new Transaction(doc, "Load Project Zone Family");
			try
			{
				val2.Start();
				FamilySymbol val3 = default(FamilySymbol);
				doc.LoadFamilySymbol(text, "Project Zone", ref val3);
				val3.Activate();
				val2.Commit();
				return val3;
			}
			catch (Exception innerException)
			{
				throw new Exception("Failed to load Project Zone Family", innerException);
			}
		}

		protected static IList<Dimension> LookupAssociatedDimensions(Element e)
		{
			Document val = e.get_Document();
			IEnumerable<Dimension> source = new FilteredElementCollector(val).WhereElementIsNotElementType().OfClass(typeof(Dimension)).ToElements()
				.Cast<Dimension>();
			return (from dim in source
			where RefersToElement(dim, e)
			select dim).ToList();
		}

		protected static bool RefersToElement(Dimension dim, Element elem)
		{
			Document val = elem.get_Document();
			foreach (Reference reference in dim.get_References())
			{
				Reference val2 = reference;
				if (val2.get_ElementId() == elem.get_Id())
				{
					return true;
				}
			}
			return false;
		}

		protected static bool NearlyIntersects(double c1, double c2)
		{
			return Math.Abs(c1 - c2) <= 0.001;
		}

		protected static Line CreateDimensionLine(FamilyInstance projectZone, Directions side, double dimOffset)
		{
			BoundingBoxXYZ val = projectZone.get_BoundingBox(null);
			double num;
			double num2;
			switch (side)
			{
			case Directions.North:
				num = val.get_Max().get_Y();
				num2 = 0.5 * (val.get_Min().get_X() + val.get_Max().get_X());
				break;
			case Directions.South:
				num = val.get_Min().get_Y();
				num2 = 0.5 * (val.get_Min().get_X() + val.get_Max().get_X());
				break;
			case Directions.East:
				num = val.get_Max().get_X();
				num2 = 0.5 * (val.get_Min().get_Y() + val.get_Max().get_Y());
				break;
			case Directions.West:
				num = val.get_Min().get_X();
				num2 = 0.5 * (val.get_Min().get_Y() + val.get_Max().get_Y());
				break;
			default:
				throw new ArgumentException("Side must be north, south, east, or west");
			}
			XYZ val2;
			XYZ val3;
			if ((uint)side > 1u)
			{
				if ((uint)(side - 2) > 1u)
				{
					throw new ArgumentException("Side must be north, south, east, or west");
				}
				val2 = new XYZ(num, num2, 0.0);
				val3 = new XYZ(num - dimOffset, num2, 0.0);
			}
			else
			{
				val2 = new XYZ(num2, num, 0.0);
				val3 = new XYZ(num2, num - dimOffset, 0.0);
			}
			return Line.CreateBound(val2, val3);
		}

		protected static Reference[] GetSideReferences(FamilyInstance projectZone, View view)
		{
			Reference[] array = (Reference[])new Reference[4];
			BoundingBoxXYZ val = projectZone.get_BoundingBox(null);
			double c = val.get_Max().get_X() - val.get_Min().get_X();
			double c2 = val.get_Max().get_Y() - val.get_Min().get_Y();
			double z = val.get_Min().get_Z();
			Options val2 = new Options();
			val2.set_ComputeReferences(true);
			val2.set_View(view);
			GeometryElement source = projectZone.get_Geometry(val2);
			GeometryInstance val3 = ((IEnumerable<GeometryObject>)source).First((GeometryObject g) => g is GeometryInstance) as GeometryInstance;
			GeometryElement val4 = val3.get_SymbolGeometry();
			int num = 0;
			foreach (GeometryObject item in val4)
			{
				if (item is Line)
				{
					Line val5 = item as Line;
					if (!(val5.GetEndPoint(0).get_Z() >= z + 0.01) && !(val5.GetEndPoint(1).get_Z() >= z + 0.01))
					{
						Directions? nullable = null;
						if (val5.get_Direction().get_X() >= 0.99 || val5.get_Direction().get_X() <= -0.99)
						{
							double y = val5.GetEndPoint(0).get_Y();
							if (NearlyIntersects(y, c2))
							{
								nullable = Directions.North;
							}
							else if (NearlyIntersects(y, 0.0))
							{
								nullable = Directions.South;
							}
						}
						else if (val5.get_Direction().get_Y() >= 0.99 || val5.get_Direction().get_Y() <= -0.99)
						{
							double x = val5.GetEndPoint(0).get_X();
							if (NearlyIntersects(x, c))
							{
								nullable = Directions.East;
							}
							else if (NearlyIntersects(x, 0.0))
							{
								nullable = Directions.West;
							}
						}
						if (nullable.HasValue && array[(int)nullable.Value] == null)
						{
							array[(int)nullable.Value] = val5.get_Reference();
							num++;
							if (num == 4)
							{
								return array;
							}
						}
					}
				}
			}
			return null;
		}

		protected static Reference[] GetGridReferences(Grid[] grids, View view)
		{
			Reference[] array = (Reference[])new Reference[grids.Length];
			Document val = null;
			foreach (Grid val2 in grids)
			{
				if (val2 != null)
				{
					val = val2.get_Document();
					break;
				}
			}
			if (val != null)
			{
				Options val3 = new Options();
				val3.set_ComputeReferences(true);
				val3.set_View(view);
				for (int j = 0; j < grids.Length; j++)
				{
					if (grids[j] != null)
					{
						GeometryElement source = grids[j].get_Geometry(val3);
						Line val4 = (from GeometryObject g in (IEnumerable)source
						where g is Line
						select g as Line).First();
						array[j] = val4.get_Reference();
					}
				}
				return array;
			}
			return array;
		}

		protected static ViewPlan GetViewForLevel(Level level)
		{
			Document val = level.get_Document();
			IList<Element> source = new FilteredElementCollector(val).WhereElementIsNotElementType().OfClass(typeof(ViewPlan)).ToElements();
			string levelId = level.get_UniqueId();
			IEnumerable<ViewPlan> enumerable = from _003C_003Eh__TransparentIdentifier0 in (from Element view in source
			select new
			{
				view = view,
				viewPlan = (view as ViewPlan)
			}).Where(_003C_003Eh__TransparentIdentifier0 =>
			{
				Level genLevel = _003C_003Eh__TransparentIdentifier0.viewPlan.get_GenLevel();
				return (int)genLevel != 0 && genLevel.get_UniqueId().Equals(levelId);
			})
			select _003C_003Eh__TransparentIdentifier0.viewPlan;
			ViewDiscipline discipline;
			foreach (ViewPlan item in enumerable)
			{
				discipline = item.get_Discipline();
				if (((object)discipline).Equals((object)"Coordination"))
				{
					return item;
				}
			}
			foreach (ViewPlan item2 in enumerable)
			{
				discipline = item2.get_Discipline();
				if (((object)discipline).Equals((object)"Structural"))
				{
					return item2;
				}
			}
			return enumerable.FirstOrDefault();
		}
	}
}
