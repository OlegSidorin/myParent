﻿/*
 * Created by SharpDevelop.
 * User: Sidorin
 * Date: 07.05.2021
 * Time: 21:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

using System.Reflection;
using System.IO;
//using System.Windows.Media.Imaging;
using adWin = Autodesk.Windows;

using Ookii.Dialogs.Wpf;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace myParent
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("CAAF4A32-AE0E-47E3-94D8-A8A5D02923A3")]
	public partial class ThisApplication : IExternalApplication
	{
		public string dllName { get; set; } = "myParent";
		public string TabName { get; set; } = "JL Secrets";
		public string PanelName { get; set; } = "Revit API NuGet Example 2019";
		public string PanelTransferring { get; set; } = "Modeless Properties";
		public string Button_01 { get; set; } = "Properties Grid";
		public string Button_02 { get; set; } = "Uninstall";
		public string path { get; set; } = Assembly.GetExecutingAssembly().Location;

		RibbonPanel RibbonPanelCurrent { get; set; }
		RibbonPanel RibbonPanelWithSingleButtonForLater { get; set; }
		private void Module_Startup(object sender, EventArgs e)
		{

		}

		private void Module_Shutdown(object sender, EventArgs e)
		{

		}

        public Result OnShutdown(UIControlledApplication a)
        { return Result.Succeeded; }

        public Result OnStartup(UIControlledApplication a)
        {
            ParentSupportMethods myParentSupportMethods = new ParentSupportMethods();
            myParentSupportMethods.myTA = this;

            string stringCommand01Button = "Set Development Path Root";

            Properties.Settings.Default.AssemblyNeedLoading = true;
            Properties.Settings.Default.Save();

            String exeConfigPath = Path.GetDirectoryName(path) + "\\" + dllName + ".dll";
            a.CreateRibbonTab(TabName);
            RibbonPanelCurrent = a.CreateRibbonPanel(TabName, PanelName);

            PushButtonData myPushButtonData01 = new PushButtonData(stringCommand01Button, stringCommand01Button, exeConfigPath, dllName + ".InvokeSetDevelopmentPath");

            ComboBoxData cbData = new ComboBoxData("DeveloperSwitch") { ToolTip = "Select an Option", LongDescription = "Select a number or letter" };
            ComboBox ComboBox01 = RibbonPanelCurrent.AddStackedItems(cbData, myPushButtonData01)[0] as ComboBox;


            string stringProductVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Default Company Name\\Revit API NuGet Example 2019").GetValue("ProductVersion").ToString();
            //Bug fix here by Max Sun (01/05/19)

            ComboBox01.AddItem(new ComboBoxMemberData("Release", "Release: " + stringProductVersion));
            ComboBox01.AddItem(new ComboBoxMemberData("Development", "C# Developer Mode"));
            ComboBox01.CurrentChanged += new EventHandler<Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs>(SwitchBetweenDeveloperAndRelease);

            RibbonPanelCurrent.AddItem(myParentSupportMethods.myPushButton_01(Button_01, path));
            RibbonPanelCurrent.AddItem(myParentSupportMethods.myPushButton_02(Button_02, path));
            RibbonPanel PRLChecklistsPanel2 = a.CreateRibbonPanel(TabName, PanelTransferring);
            //PRLChecklistsPanel2.Visible = false;

            myParentSupportMethods.PlaceButtonOnModifyRibbon();
            return Result.Succeeded;
        }

        public void SwitchBetweenDeveloperAndRelease(object sender, Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs e)
        {
            try
            {
                ComboBox cBox = sender as ComboBox;

                //PushButton myPushButton00 = RibbonPanelCurrent.GetItems()[1] as PushButton;
                PushButton myPushButton02 = RibbonPanelCurrent.GetItems()[2] as PushButton;
                PushButton myPushButton03 = RibbonPanelCurrent.GetItems()[3] as PushButton;

                //if (cBox.Current.Name == "Development") myPushButton00.ClassName = dllName + ".Invoke00Development";
                if (cBox.Current.Name == "Development") myPushButton02.ClassName = dllName + ".Invoke01Development";
                if (cBox.Current.Name == "Development") myPushButton03.ClassName = dllName + ".Invoke02";

                if (cBox.Current.Name == "Release") myPushButton02.ClassName = dllName + ".Invoke01";
                if (cBox.Current.Name == "Release") myPushButton03.ClassName = dllName + ".Invoke02";
            }

            #region catch and finally
            catch (Exception ex)
            {
                TaskDialog.Show("Catch", "Failed due to: " + ex.Message);
            }
            finally
            {
            }
            #endregion
        }


        #region Revit Macros generated code
        private void InternalStartup()
		{
			this.Startup += new System.EventHandler(Module_Startup);
			this.Shutdown += new System.EventHandler(Module_Shutdown);
		}
		#endregion
	}
}