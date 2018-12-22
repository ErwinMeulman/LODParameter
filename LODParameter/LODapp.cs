using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LODParameter
{
	public class LODapp
	{
		public const string CURRENT_LOD_PARAM_NAME = "Current_LOD";

		public const string TARGET_LOD_PARAM_NAME = "Target_LOD";

		public const string MEA_PARAM_NAME = "MEA";

		public const string ZONE_PARAM_NAME = "Zone";

		public const string ZONE_FAMILY_NAME = "Project Zone";

		public const string HELP_URL = "http://www.ikerd.com/lodmanager/user-guide";

		public const int DEFAULT_CURRENT_LOD = 200;

		protected internal static BuiltInCategory[] lodCatArray;

		private IList<UpdaterId> registeredUpdaters;

		public Result OnStartup(UIControlledApplication app)
		{
			app.CreateRibbonTab("LOD Manager");
			RibbonPanel panel = app.CreateRibbonPanel("LOD Manager", "LOD Parameters");
			AddRibbonItems(panel);
			LODupdater lODupdater = new LODupdater(app.get_ActiveAddInId());
			UpdaterRegistry.RegisterUpdater(lODupdater, true);
			ElementFilter val = new ElementCategoryFilter(-2005000, true);
			UpdaterRegistry.AddTrigger(lODupdater.GetUpdaterId(), val, Element.GetChangeTypeElementAddition());
			app.get_ControlledApplication().add_DocumentOpened((EventHandler<DocumentOpenedEventArgs>)lodApp_DocumentOpened);
			app.get_ControlledApplication().add_DocumentClosing((EventHandler<DocumentClosingEventArgs>)lodApp_DocumentClosing);
			return 0;
		}

		public Result OnShutdown(UIControlledApplication app)
		{
			LODupdater lODupdater = new LODupdater(app.get_ActiveAddInId());
			UpdaterRegistry.UnregisterUpdater(lODupdater.GetUpdaterId());
			app.get_ControlledApplication().remove_DocumentOpened((EventHandler<DocumentOpenedEventArgs>)lodApp_DocumentOpened);
			app.get_ControlledApplication().remove_DocumentClosing((EventHandler<DocumentClosingEventArgs>)lodApp_DocumentClosing);
			return 0;
		}

		public void lodApp_DocumentOpened(object sender, DocumentOpenedEventArgs args)
		{
			Document val = args.get_Document();
			AddInId id = args.get_Document().get_Application().get_ActiveAddInId();
			ElementFilter val2 = new ElementCategoryFilter(-2005000, true);
			try
			{
				registeredUpdaters = new List<UpdaterId>(4);
				LODParameterUpdater lODParameterUpdater = new LODParameterUpdater(id);
				UpdaterRegistry.RegisterUpdater(lODParameterUpdater, val, true);
				Parameter lODparameter = GetLODparameter(args.get_Document(), "Current_LOD");
				ChangeType val3 = Element.GetChangeTypeParameter(lODparameter);
				UpdaterRegistry.AddTrigger(lODParameterUpdater.GetUpdaterId(), val2, val3);
				Parameter lODparameter2 = GetLODparameter(args.get_Document(), "Target_LOD");
				val3 = Element.GetChangeTypeParameter(lODparameter2);
				UpdaterRegistry.AddTrigger(lODParameterUpdater.GetUpdaterId(), val2, val3);
				registeredUpdaters.Add(lODParameterUpdater.GetUpdaterId());
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}

		public void lodApp_DocumentClosing(object sender, DocumentClosingEventArgs args)
		{
			AddInId val = args.get_Document().get_Application().get_ActiveAddInId();
			foreach (UpdaterId registeredUpdater in registeredUpdaters)
			{
				UpdaterRegistry.UnregisterUpdater(registeredUpdater);
			}
		}

		private void AddRibbonItems(RibbonPanel panel)
		{
			string location = Assembly.GetExecutingAssembly().Location;
			string str = location.Remove(location.Length - "Contents\\LODParameter.dll".Length);
			string str2 = str + "Resources\\Icons\\";
			PushButtonData val = new PushButtonData("CreateLODparameter", "Create\nParameters", location, "LODParameter.CreateLODParameter");
			val.set_AvailabilityClassName("LODParameter.CreateParametersButtonAvailability");
			val.SetContextualHelp(new ContextualHelp(2, "http://www.ikerd.com/lodmanager/user-guide"));
			PushButton val2 = panel.AddItem(val) as PushButton;
			val2.set_ToolTip("Create LOD parameters for this document");
			val2.set_LongDescription("Adds Current and Target LOD fields and MEA to each element in document.\nYou must run this command before using other LOD-related commands");
			val2.set_LargeImage((ImageSource)new BitmapImage(new Uri(str2 + "LODCREATEICON.png")));
			PushButtonData val3 = new PushButtonData("SetLOD", "Set\nLOD", location, "LODParameter.SetLODofSelection");
			val3.set_AvailabilityClassName("LODParameter.ParameterRequiredButtonAvailability");
			val3.SetContextualHelp(new ContextualHelp(2, "http://www.ikerd.com/lodmanager/user-guide"));
			PushButton val4 = panel.AddItem(val3) as PushButton;
			val4.set_ToolTip("Set LOD of Selection");
			val4.set_LongDescription("Sets the LOD of all selected items.\nSelect a detail view to set the LOD of all items in that view.");
			val4.set_LargeImage((ImageSource)new BitmapImage(new Uri(str2 + "SETLODICON.png")));
			PushButtonData val5 = new PushButtonData("LODfilters", "LOD\nFilters", location, "LODParameter.FilterByLOD");
			val5.set_AvailabilityClassName("LODParameter.ParameterRequiredButtonAvailability");
			val5.SetContextualHelp(new ContextualHelp(2, "http://www.ikerd.com/lodmanager/user-guide"));
			PushButton val6 = panel.AddItem(val5) as PushButton;
			val6.set_ToolTip("Apply graphical LOD filters");
			val6.set_LongDescription("Applys visibility graphics filters such as coloring,transparency, and visibility to items based on their LOD.");
			val6.set_LargeImage((ImageSource)new BitmapImage(new Uri(str2 + "FILTERLODICON.png")));
		}

		protected internal static IList<Element> GetElements(Document doc, ICollection<ElementId> ids)
		{
			IList<Element> list = new List<Element>(ids.Count);
			foreach (ElementId id in ids)
			{
				list.Add(doc.GetElement(id));
			}
			return list;
		}

		protected internal static IList<Element> GetElements(Document doc, ICollection<Reference> refs)
		{
			IList<Element> list = new List<Element>(refs.Count);
			foreach (Reference @ref in refs)
			{
				list.Add(doc.GetElement(@ref));
			}
			return list;
		}

		protected internal static IList<ElementId> GetElementIds(Document doc, ICollection<Reference> refs)
		{
			IList<ElementId> list = new List<ElementId>(refs.Count);
			foreach (Reference @ref in refs)
			{
				list.Add(doc.GetElement(@ref).get_Id());
			}
			return list;
		}

		protected internal static Definition GetParameterDefinition(Document doc, string name)
		{
			BindingMap val = doc.get_ParameterBindings();
			DefinitionBindingMapIterator val2 = val.ForwardIterator();
			val2.Reset();
			while (val2.MoveNext())
			{
				InternalDefinition val3 = val2.get_Key();
				if (val3.get_Name().Equals(name))
				{
					return val3;
				}
			}
			return null;
		}

		protected internal static Parameter GetLODparameter(Document doc, Definition def)
		{
			FilteredElementCollector val = new FilteredElementCollector(doc);
			ElementMulticategoryFilter val2 = new ElementMulticategoryFilter((ICollection<BuiltInCategory>)lodCatArray);
			val.WherePasses(val2);
			foreach (Element item in val)
			{
				if (item != null)
				{
					return item.get_Parameter(def);
				}
			}
			throw new NullReferenceException("Document must have an object with the LOD parameterin order to get the Parameter");
		}

		protected internal static Parameter GetLODparameter(Document doc, string name)
		{
			FilteredElementCollector val = new FilteredElementCollector(doc);
			ElementMulticategoryFilter val2 = new ElementMulticategoryFilter((ICollection<BuiltInCategory>)lodCatArray);
			val.WherePasses(val2);
			foreach (Element item in val)
			{
				if (item != null)
				{
					return item.LookupParameter(name);
				}
			}
			throw new NullReferenceException("Document must have an object with the LOD parameterin order to get the Parameter. Or perhaps you spelled the parameter name wrong.");
		}

		protected internal static void SetParameterOfElements(IEnumerable<Element> elems, Definition paramDef, int value)
		{
			foreach (Element elem in elems)
			{
				Parameter val = elem.get_Parameter(paramDef);
				if (val != null)
				{
					try
					{
						val.Set(value);
					}
					catch (Exception)
					{
					}
				}
			}
		}

		protected internal static void SetParameterOfElements(IEnumerable<Element> elems, Definition paramDef, string value)
		{
			foreach (Element elem in elems)
			{
				Parameter val = elem.get_Parameter(paramDef);
				if (val != null && !val.get_IsReadOnly())
				{
					val.Set(value);
				}
			}
		}

		protected internal static void SetParameterOfElements(ElementSet elems, Definition paramDef, int value)
		{
			foreach (Element elem in elems)
			{
				Element val = elem;
				Parameter val2 = val.get_Parameter(paramDef);
				if (val2 != null && !val2.get_IsReadOnly())
				{
					val2.Set(value);
				}
			}
		}

		protected internal static void SetParameterOfElementsIfNotSet(IEnumerable<Element> elems, Definition paramDef, int value)
		{
			foreach (Element elem in elems)
			{
				Parameter val = elem.get_Parameter(paramDef);
				if (val != null && !val.get_IsReadOnly() && !val.get_HasValue())
				{
					val.Set(value);
				}
			}
		}

		protected internal static void SetParameterOfElementsIfNotSet(ElementSet elems, Definition paramDef, int value)
		{
			foreach (Element elem in elems)
			{
				Element val = elem;
				Parameter val2 = val.get_Parameter(paramDef);
				if (val2 != null && !val2.get_IsReadOnly() && !val2.get_HasValue())
				{
					val2.Set(value);
				}
			}
		}

		static LODapp()
		{
			BuiltInCategory[] array = new BuiltInCategory[107];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			lodCatArray = (BuiltInCategory[])array;
		}
	}
}
