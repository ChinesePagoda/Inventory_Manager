using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;
using System.Collections.Immutable;
using VRageRender;
using Sandbox.Game.Entities;
using VRage.Game.VisualScripting.Utils;
using VRage.Library.Compiler;
using System.Diagnostics;

namespace IngameScript
{
    partial class Program: MyGridProgram
    {
        MyIni _ini;
                

        List<string> spritesList = new List<string>();
        List<IMyTextPanel> panels = new List<IMyTextPanel>();
        List<IMyTextPanel> panels_Items_All = new List<IMyTextPanel>();
        List<IMyTextPanel> panels_Items_Ore = new List<IMyTextPanel>();
        List<IMyTextPanel> panels_Items_Ingot = new List<IMyTextPanel>();
        List<IMyTextPanel> panels_Items_Component = new List<IMyTextPanel>();
        List<IMyTextPanel> panels_Items_AmmoMagazine = new List<IMyTextPanel>();
        List<IMyTextPanel> panels_Refineries = new List<IMyTextPanel>();
        List<IMyTextPanel> panels_Assemblers = new List<IMyTextPanel>();
        List<IMyTextPanel> panels_Overall = new List<IMyTextPanel>();
        List<IMyReactor> reactors = new List<IMyReactor>();
        List<IMyGasGenerator> gasGenerators = new List<IMyGasGenerator>();

        Dictionary<string, string> translator = new Dictionary<string, string>();

        List<IMyPowerProducer> powerProducers = new List<IMyPowerProducer>();
        List<IMyAssembler> assemblers = new List<IMyAssembler>();
        List<IMyRefinery> refineries = new List<IMyRefinery>();
        List<IMyShipConnector> connectors = new List<IMyShipConnector>();
        List<IMyCockpit> cockpits = new List<IMyCockpit>();
        List<IMyCryoChamber> cryoChambers = new List<IMyCryoChamber>();
        List<IMyConveyorSorter> sorters = new List<IMyConveyorSorter>();

        List<IMyCargoContainer> cargoContainers = new List<IMyCargoContainer>();

        List<IMyGasTank> hydrogenTanks = new List<IMyGasTank>();
        List<IMyGasTank> oxygenTanks = new List<IMyGasTank>();

        List<IMyRadioAntenna> radioAntennas = new List<IMyRadioAntenna>();

        float counter_Logo_Float = 0;
        DateTime time1_DateTime = DateTime.Now;
        DateTime time2_DateTime = DateTime.Now;

        StringBuilder debug_StringBuilder;

        int counter_ShowItems_Int = 0, counter_ShowFacilities_Int = 1, counter_Panel_Int = 0,
            counter_Assembler_Int = 1, counter_Refinery_Int = 1, counter_CombinedRefining_Int = 1,
            counter_Connector_Int = 1, counter_CryoChamber_Int = 1, counter_Sorter_Int = 1,
            counter_HydrogenTank_Int = 1, counter_OxydrogenTank_Int = 1,
            counter_CargoContainer_Int = 0,
            maxNumber_AssemblerPanel_Int = 0, maxNumber_RefineryPanel_Int = 0,
            counter_AutoProduction_Int = 1,
            counter_CombiningLikeTerms_Int = 1, counter_CombiningLikeTerms_CargoContainer_Int = 1,
            counter_Sub_Function_Interval_Int = 1;

        const int 
            itemBox_RowNumbers_Int = 4,
            itemBox_ColumnNumbers_Int = 7,
            itemAmountInEachScreen_Int = itemBox_RowNumbers_Int * itemBox_ColumnNumbers_Int,
            facilityAmountInEachScreen_Int = 20,
            method_Total_Int = 10;
        const string 
            information_Section = "Information",
            function_On_Off_Section = "Function_On_Off(Y/N)",
            translateList_Section = "Translate_List",
            autoProductionList_Section = "AutoProduction_List",
            length_Key = "Length";
        const string stage_ShowItems = "Stage_ShowItems",
            stage_ShowFacilities = "Stage_ShowFacilities",
            stage_Assembler_Clear = "Stage_Assembler_Clear",
            stage_Refinery_Clear = "Stage_Refinery_Clear",
            stage_Connector_Clear = "Stage_Connector_Clear",
            stage_CryoChamber_Clear = "Stage_CryoChamber_Clear",
            stage_Sorter_Clear = "Stage_Sorter_Clear",
            stage_HydrogenTank = "Stage_HydrogenTank",
            stage_OxygenTank = "Stage_OxygenTank",
            stage_ShowCargoContainerResidues = "Stage_ShowCargoContainerRatio",
            stage_Combined_Refining = "Stage_Combined_Refining",
            stage_AutoProduction = "Stage_AutoProduction",
            stage_CombiningLikeTerms = "Stage_CombiningLikeTerms";
        const string function_ShowOverall = "ShowOverall",
            function_ShowItems = "ShowItems",
            function_ShowFacilities = "ShowFacilities",
            function_InventoryManagement = "InventoryManagement",
            function_BroadCastConnectorGPS = "BroadCastConnectorGPS",
            function_ShowCargoContainerRatio = "ShowCargoContainerRatio",
            function_AutoProduction = "AutoProduction";
        bool function_ShowOverall_Bool = true,
            function_ShowItems_Bool = true,
            function_ShowFacilities_Bool = true,
            function_InventoryManagement_Bool = true,
            function_BroadCastConnectorGPS_Bool = true,
            function_ShowCargoContainerRatio_Bool = true,
            function_AutoProduction_Bool = true;

        const string customName_Key = "CustomName";
        const string volumeThreshold_Key = "VolumeThreshold";
        const string refreshRate_Key = "Refresh_Rate(F_FF_FFF)";

        const string ore_Section = "Ore",
            combinedMode_Key = "Combined_Mode";

        const string panelInformation_Section = "Panel_Information",
            counter_Key = "Counter",
            Amount_Key = "Amount";

        const string itemType_Key = "Item_Type",
            itemAmount1_Key = "Item_Amount_1",
            itemAmount2_Key = "Item_Amount_2",
            time_Key = "Time",
            time1_Key = "Time1",
            time2_Key = "Time2",
            productionAmount_Key = "ProductionAmount";

        string stage_Key = "";

        string[] FunctionName_Array = 
        { 
            function_ShowOverall, 
            function_ShowItems, 
            function_ShowFacilities, 
            function_InventoryManagement, 
            function_BroadCastConnectorGPS, 
            function_ShowCargoContainerRatio,
            function_AutoProduction
        };

        public struct ItemList
        {
            public string Name;
            public double ProductionAmount;
            public double Amount1;
            public double Amount2;
            public DateTime Time1;
            public DateTime Time2;
        }
        ItemList[] itemList_All;
        ItemList[] itemList_Ore;
        ItemList[] itemList_Ingot;
        ItemList[] itemList_Component;
        ItemList[] itemList_AmmoMagazine;

        public struct ProdcutionProperty
        {
            public string ComponentName;
            public string ProductionName;
            public double ProductionAmount;
        }
        ProdcutionProperty[] productionList;

        public struct Facility_Struct
        {
            public bool IsEnabled_Bool;
            public string Name;
            public bool IsProducing_Bool;
            public bool IsCooperativeMode_Bool;
            public bool IsRepeatMode_Bool;
            public string Picture;
            public double ItemAmount;
            public string Productivity;
        }
        Facility_Struct[] refineryList;
        Facility_Struct[] assemblerList;

        Dictionary<string, double> method_Unified_Dic = new Dictionary<string, double>();
        Dictionary<string, double> method_Refinery_Dic;
        Dictionary<string, double> allitems_InAssemblers_Dic;
        Dictionary<string, double> allItems_InRefineries_Dic;
        Dictionary<string, double> allItems_Old_Dic = new Dictionary<string, double>();
        Dictionary<string, double> allItems_Old_Assemblers_Dic;
        Dictionary<string, MyIni> panel_UI_Info_Dic;

        Color card_Background_Color_Overall = new Color(10, 20, 40);
        Color ore_Background_Color = new Color(10, 5, 2);
        Color ingot_Background_Color = new Color(5, 2, 15);
        //Color border_Color = new Color(0, 130, 255);
        Color font_Color_Overall = new Color(230, 255, 255);
        Color progressbar_Color = new Color(30, 50, 90);
        Color oreCard_Background_Color = new Color(50, 40, 20);
        Color ingotCard_Background_Color = new Color(35, 25, 45);
        Color ammo_Background_Color = new Color(11, 25, 25);
        Color ammoCard_Background_Color = new Color(0, 60, 60);



        public Program()
        {

            BuildBlockList();

            SetDefultConfiguration();

            Build_TranslateDic();

            Build_ProductionList();

            Build_SpriteList();

            GetFunctionOrder();

            RefreshRate();

            Build_MethodDic(method_Unified_Dic);

            SetRefineriesDefalutCustomData();

        }

        public void Save()
        {

        }

        public void BuildBlockList()
        {
            GridTerminalSystem.GetBlocksOfType(panels, b => b.IsSameConstructAs(Me));
            GridTerminalSystem.GetBlocksOfType(panels_Overall, b => b.IsSameConstructAs(Me) && b.CustomName.Contains("LCD_Overall_Display"));
            GridTerminalSystem.GetBlocksOfType(panels_Items_All, b => b.IsSameConstructAs(Me) && b.CustomName.Contains("LCD_Inventory_Display:"));
            GridTerminalSystem.GetBlocksOfType(panels_Items_Ore, b => b.IsSameConstructAs(Me) && b.CustomName.Contains("LCD_Ore_Inventory_Display:"));
            GridTerminalSystem.GetBlocksOfType(panels_Items_Ingot, b => b.IsSameConstructAs(Me) && b.CustomName.Contains("LCD_Ingot_Inventory_Display:"));
            GridTerminalSystem.GetBlocksOfType(panels_Items_Component, b => b.IsSameConstructAs(Me) && b.CustomName.Contains("LCD_Component_Inventory_Display:"));
            GridTerminalSystem.GetBlocksOfType(panels_Items_AmmoMagazine, b => b.IsSameConstructAs(Me) && b.CustomName.Contains("LCD_AmmoMagazine_Inventory_Display:"));
            GridTerminalSystem.GetBlocksOfType(panels_Refineries, b => b.IsSameConstructAs(Me) && b.CustomName.Contains("LCD_Refinery_Inventory_Display:"));
            GridTerminalSystem.GetBlocksOfType(panels_Assemblers, b => b.IsSameConstructAs(Me) && b.CustomName.Contains("LCD_Assembler_Inventory_Display:"));
            GridTerminalSystem.GetBlocksOfType(powerProducers, b => b.IsSameConstructAs(Me));
            GridTerminalSystem.GetBlocksOfType(reactors, b => b.IsSameConstructAs(Me));
            GridTerminalSystem.GetBlocksOfType(gasGenerators, b => b.IsSameConstructAs(Me));


            GridTerminalSystem.GetBlocksOfType(assemblers, b => b.IsSameConstructAs(Me));
            GridTerminalSystem.GetBlocksOfType(refineries, b => b.IsSameConstructAs(Me) && !b.BlockDefinition.ToString().Contains("Shield"));
            GridTerminalSystem.GetBlocksOfType(connectors, b => b.IsSameConstructAs(Me));
            GridTerminalSystem.GetBlocksOfType(cockpits, b => b.IsSameConstructAs(Me));
            GridTerminalSystem.GetBlocksOfType(cryoChambers, b => b.IsSameConstructAs(Me));
            GridTerminalSystem.GetBlocksOfType(sorters, b => b.IsSameConstructAs(Me));

            GridTerminalSystem.GetBlocksOfType(cargoContainers, b => b.IsSameConstructAs(Me));

            GridTerminalSystem.GetBlocksOfType(hydrogenTanks, b => b.IsSameConstructAs(Me) && !b.DefinitionDisplayNameText.ToString().Contains("Oxygen") && !b.DefinitionDisplayNameText.ToString().Contains("氧气"));
            GridTerminalSystem.GetBlocksOfType(oxygenTanks, b => b.IsSameConstructAs(Me) && !b.DefinitionDisplayNameText.ToString().Contains("Hydrogen") && !b.DefinitionDisplayNameText.ToString().Contains("氢气"));

            GridTerminalSystem.GetBlocksOfType(radioAntennas, b => b.IsSameConstructAs(Me));
        }

        public void SetDefultConfiguration()
        {
            WriteDefaultItem(information_Section, "LCD_Overall_Display", "LCD_Overall_Display | Fill In CustomName of Panel");
            WriteDefaultItem(information_Section, "LCD_Inventory_Display", "LCD_Inventory_Display:X | X=1,2,3... | Fill In CustomName of Panel");
            WriteDefaultItem(information_Section, "LCD_Ore_Inventory_Display", "LCD_Ore_Inventory_Display:X | X=1,2,3... | Fill In CustomName of Panel");
            WriteDefaultItem(information_Section, "LCD_Ingot_Inventory_Display", "LCD_Ingot_Inventory_Display:X | X=1,2,3... | Fill In CustomName of Panel");
            WriteDefaultItem(information_Section, "LCD_Component_Inventory_Display", "LCD_Component_Inventory_Display:X | X=1,2,3... | Fill In CustomName of Panel");
            WriteDefaultItem(information_Section, "LCD_AmmoMagazine_Inventory_Display", "LCD_AmmoMagazine_Inventory_Display:X | X=1,2,3... | Fill In CustomName of Panel");
            WriteDefaultItem(information_Section, "LCD_Refinery_Inventory_Display", "LCD_Refinery_Inventory_Display:X | X=1,2,3... | Fill In CustomName of Panel");
            WriteDefaultItem(information_Section, "LCD_Assembler_Inventory_Display", "LCD_Assembler_Inventory_Display:X | X=1,2,3... | Fill In CustomName of Panel");
            WriteDefaultItem(information_Section, "Assemblers_CooperativeMode", "CO_ON or CO_OFF | Fill In Argument of Programmable Block And Press Run");
            WriteDefaultItem(information_Section, "Clear_Assembler_Queue", "CLS | Fill In Argument of Programmable Block And Press Run");
            WriteDefaultItem(information_Section, "IGCTAG", "CHANNEL1");
            WriteDefaultItem(information_Section, volumeThreshold_Key, "6000");
            WriteDefaultItem(information_Section, refreshRate_Key, "FF");

            FunctionOnOff(true);

            WriteDefaultItem(ore_Section, combinedMode_Key, "1");
            for (int index_Int = 1; index_Int <= method_Total_Int; index_Int++) WriteDefaultItem(ore_Section, index_Int.ToString(), "");


            WriteDefaultItem(translateList_Section, length_Key, "1");
            WriteDefaultItem(translateList_Section, "1", "AH_BoreSight:More");


            WriteDefaultItem(autoProductionList_Section, length_Key, "3");
            WriteDefaultItem(autoProductionList_Section, "1", "MyObjectBuilder_Component/SteelPlate:MyObjectBuilder_BlueprintDefinition/SteelPlate:5000");
            WriteDefaultItem(autoProductionList_Section, "2", "MyObjectBuilder_Component/Construction:MyObjectBuilder_BlueprintDefinition/ConstructionComponent:5000");
            WriteDefaultItem(autoProductionList_Section, "3", "MyObjectBuilder_Component/InteriorPlate:MyObjectBuilder_BlueprintDefinition/InteriorPlate:5000");


            foreach (var panel in panels_Items_All) panel.CustomData = "";
            foreach (var panel in panels_Items_Ore) panel.CustomData = "";
            foreach (var panel in panels_Items_Ingot) panel.CustomData = "";
            foreach (var panel in panels_Items_Component) panel.CustomData = "";
            foreach (var panel in panels_Items_AmmoMagazine) panel.CustomData = "";

        }

        public void Build_MethodDic(Dictionary<string, double> method_Dic)
        {

            for (int index_Int = 1; index_Int <= method_Total_Int; index_Int++)
            {
                string value_String;
                value_String = GetValue_from_CustomData(ore_Section, index_Int.ToString());
                ParseMethodValue(value_String, method_Dic);
            }
            foreach (var key in method_Dic.Keys) Echo($"{key}={method_Dic[key] / 1000000}");
        }

        public void Build_MethodDic(Dictionary<string, double> method_Dic, IMyRefinery refinery_Block)
        {

            for (int index_Int = 1; index_Int <= method_Total_Int; index_Int++)
            {
                string value_String;
                value_String = GetValue_from_CustomData(refinery_Block, ore_Section, index_Int.ToString());
                ParseMethodValue(value_String, method_Dic);
            }
            foreach (var key in method_Dic.Keys) Echo($"{key}={method_Dic[key] / 1000000}");
        }

        public void ParseMethodValue(string value_String, Dictionary<string, double> method_Dic)
        {
            string[] value_Array = value_String.Split(':');

            if (value_Array.Length == 2 && !method_Dic.ContainsKey(value_Array[0]))
            {
                method_Dic.Add(value_Array[0], Convert.ToDouble(value_Array[1]) * 1000000);
            }
            else if (value_Array.Length == 2 && method_Dic.ContainsKey(value_Array[0]))
            {
                method_Dic[value_Array[0]] += Convert.ToDouble(value_Array[1]) * 1000000;
            }
        }

        public void SetRefineriesDefalutCustomData()
        {
            foreach (var refinery in refineries)
            {
                WriteDefaultItem(refinery, ore_Section, combinedMode_Key, "1");
                for (int index_Int = 1; index_Int <= method_Total_Int; index_Int++) WriteDefaultItem(refinery, ore_Section, index_Int.ToString(), "");
            }
        }

        public void WriteDefaultItem(string section, string key, string value)
        {
            string valueTemp_String = GetValue_from_CustomData(section, key);

            if (valueTemp_String == "")
            {
                WriteValue_to_CustomData(section, key, value);
            }
        }

        public void WriteDefaultItem(IMyRefinery refinery_Block, string section, string key, string value)
        {
            string valueTemp_String = GetValue_from_CustomData(refinery_Block, section, key);

            if (valueTemp_String == "") WriteValue_to_CustomData(refinery_Block, section, key, value);
        }

        public void FunctionOnOff(bool true_False)
        {
            if (true_False)
            {
                foreach (var functionName in FunctionName_Array)
                {
                    WriteDefaultItem(function_On_Off_Section, functionName, "Y");
                }
            }
            else
            {
                foreach (var functionName in FunctionName_Array)
                {
                    WriteDefaultItem(function_On_Off_Section, functionName, "N");
                }
            }
        }

        public void Build_SpriteList()
        {
            if (panels.Count < 1)
            {
                if (Me.SurfaceCount > 0)
                {
                    Me.GetSurface(0).GetSprites(spritesList);
                }
            }
            else
            {
                panels[0].GetSprites(spritesList);
            }
        }

        public void GetFunctionOrder()
        {
            string value_String;
            value_String = GetValue_from_CustomData(function_On_Off_Section, function_ShowOverall);
            if (value_String != "Y") function_ShowOverall_Bool = false;
            value_String = GetValue_from_CustomData(function_On_Off_Section, function_ShowItems);
            if (value_String != "Y") function_ShowItems_Bool = false;
            value_String = GetValue_from_CustomData(function_On_Off_Section, function_ShowFacilities);
            if (value_String != "Y") function_ShowFacilities_Bool = false;
            value_String = GetValue_from_CustomData(function_On_Off_Section, function_InventoryManagement);
            if (value_String != "Y") function_InventoryManagement_Bool = false;
            value_String = GetValue_from_CustomData(function_On_Off_Section, function_BroadCastConnectorGPS);
            if (value_String != "Y") function_BroadCastConnectorGPS_Bool = false;
            value_String = GetValue_from_CustomData(function_On_Off_Section, function_ShowCargoContainerRatio);
            if (value_String != "Y") function_ShowCargoContainerRatio_Bool = false;
            value_String = GetValue_from_CustomData(function_On_Off_Section, function_AutoProduction);
            if (value_String != "Y") function_AutoProduction_Bool = false;
        }

        public void RefreshRate()
        {
            string refreshRate_String = GetValue_from_CustomData(information_Section, refreshRate_Key);

            switch (refreshRate_String)
            {
                case "F":
                    Runtime.UpdateFrequency = UpdateFrequency.Once | UpdateFrequency.Update100;
                    break;
                case "FF":
                    Runtime.UpdateFrequency = UpdateFrequency.Once | UpdateFrequency.Update10;
                    break;
                case "FFF":
                    Runtime.UpdateFrequency = UpdateFrequency.Once | UpdateFrequency.Update1;
                    break;
            }

        }

        public void WriteValue_to_CustomData(string section, string key, string value)
        {
            _ini = new MyIni();
            // This time we _must_ check for failure since the user may have written invalid ini.
            MyIniParseResult result;
            if (!_ini.TryParse(Me.CustomData, out result))
                throw new Exception(result.ToString());

            _ini.Set(section, key, value);
            Me.CustomData = _ini.ToString();
        }

        public void WriteValue_to_CustomData(IMyCargoContainer block, string section, string key, string value)
        {
            _ini = new MyIni();
            // This time we _must_ check for failure since the user may have written invalid ini.
            MyIniParseResult result;
            if (!_ini.TryParse(block.CustomData, out result))
                throw new Exception(result.ToString());

            _ini.Set(section, key, value);
            block.CustomData = _ini.ToString();
        }

        public void WriteValue_to_CustomData(IMyTextPanel block, string section, string key, string value)
        {
            _ini = new MyIni();
            // This time we _must_ check for failure since the user may have written invalid ini.
            MyIniParseResult result;
            if (!_ini.TryParse(block.CustomData, out result))
                throw new Exception(result.ToString());

            _ini.Set(section, key, value);
            block.CustomData = _ini.ToString();
        }

        public void WriteValue_to_CustomData(IMyRefinery block, string section, string key, string value)
        {
            _ini = new MyIni();
            // This time we _must_ check for failure since the user may have written invalid ini.
            MyIniParseResult result;
            if (!_ini.TryParse(block.CustomData, out result))
                throw new Exception(result.ToString());

            _ini.Set(section, key, value);
            block.CustomData = _ini.ToString();
        }

        public string GetValue_from_CustomData(string section, string key)
        {
            _ini = new MyIni();
            // This time we _must_ check for failure since the user may have written invalid ini.
            MyIniParseResult result;
            if (!_ini.TryParse(Me.CustomData, out result))
                throw new Exception(result.ToString());

            string DefaultValue = "";

            // Read the integer value. If it does not exist, return the default for this value.
            return _ini.Get(section, key).ToString(DefaultValue);
        }

        public string GetValue_from_CustomData(IMyShipConnector block, string section, string key)
        {
            _ini = new MyIni();
            // This time we _must_ check for failure since the user may have written invalid ini.
            MyIniParseResult result;
            if (!_ini.TryParse(block.CustomData, out result))
                throw new Exception(result.ToString());

            string DefaultValue = "";

            // Read the integer value. If it does not exist, return the default for this value.
            return _ini.Get(section, key).ToString(DefaultValue);
        }

        public string GetValue_from_CustomData(IMyCargoContainer block, string section, string key)
        {
            _ini = new MyIni();
            // This time we _must_ check for failure since the user may have written invalid ini.
            MyIniParseResult result;
            if (!_ini.TryParse(block.CustomData, out result))
                throw new Exception(result.ToString());

            string DefaultValue = "";

            // Read the integer value. If it does not exist, return the default for this value.
            return _ini.Get(section, key).ToString(DefaultValue);
        }

        public string GetValue_from_CustomData(IMyTextPanel block, string section, string key)
        {
            _ini = new MyIni();
            // This time we _must_ check for failure since the user may have written invalid ini.
            MyIniParseResult result;
            if (!_ini.TryParse(block.CustomData, out result))
                throw new Exception(result.ToString());

            string DefaultValue = "";

            // Read the integer value. If it does not exist, return the default for this value.
            return _ini.Get(section, key).ToString(DefaultValue);
        }

        public string GetValue_from_CustomData(IMyRefinery block, string section, string key)
        {
            _ini = new MyIni();
            // This time we _must_ check for failure since the user may have written invalid ini.
            MyIniParseResult result;
            if (!_ini.TryParse(block.CustomData, out result))
                throw new Exception(result.ToString());

            string DefaultValue = "";

            // Read the integer value. If it does not exist, return the default for this value.
            return _ini.Get(section, key).ToString(DefaultValue);
        }

        public void ProgrammableBlockScreen()
        {
            counter_Logo_Float++;
            if (counter_Logo_Float >= 360f) counter_Logo_Float = 0f;

            //  512 X 320
            IMyTextSurface panel = Me.GetSurface(0);

            if (panel == null) return;
            panel.ContentType = ContentType.SCRIPT;

            RectangleF
                visibleArea_RectangleF = new RectangleF
                (
                    (panel.TextureSize - panel.SurfaceSize) / 2f + new Vector2(0, counter_Logo_Float / 360f),
                    panel.SurfaceSize
                ),
                logo_RectangleF = new RectangleF
                (
                    visibleArea_RectangleF.Position,
                    new Vector2(visibleArea_RectangleF.Width, visibleArea_RectangleF.Height * 0.6f)
                ),
                text_RectangleF = new RectangleF
                (
                    visibleArea_RectangleF.Position + new Vector2(0, logo_RectangleF.Height),
                    new Vector2(visibleArea_RectangleF.Width, visibleArea_RectangleF.Height * 0.4f)
                );

            logo_RectangleF = ScalingViewport(logo_RectangleF, 0.8f, 2);

            MySpriteDrawFrame frame = panel.DrawFrame();
            DrawLogo(ref frame, logo_RectangleF);
            PanelWriteText
                (
                    ref frame,
                    "Inventory_Management\nWith_Graphic_Interface_V1.3\nby Hi.James",
                    text_RectangleF,
                    1f,
                    font_Color_Overall,
                    TextAlignment.CENTER
                );
            frame.Dispose();

        }

        /*###############################################*/
        /*###############     Overall     ###############*/
        public void OverallDisplay()
        {
            if (function_ShowOverall_Bool != true) return;

            foreach (var panel in panels_Overall)
            {
                if (panel.CustomData != "0") panel.CustomData = "0";
                else panel.CustomData = "0.001";

                if (panel.ContentType != ContentType.SCRIPT) panel.ContentType = ContentType.SCRIPT;

                panel.ScriptBackgroundColor = card_Background_Color_Overall;

                MySpriteDrawFrame frame = panel.DrawFrame();

                DrawContentBox(panel, ref frame);

                frame.Dispose();
            }
        }

        public void DrawContentBox(IMyTextPanel panel, ref MySpriteDrawFrame frame)
        {            
            RectangleF visibleArea_RectangleF = new RectangleF
                (
                    (panel.TextureSize - panel.SurfaceSize) / 2f + new Vector2(0, Convert.ToSingle(panel.CustomData)),
                    panel.SurfaceSize
                );

            float sideLength_Float;

            if (visibleArea_RectangleF.Width <= visibleArea_RectangleF.Height) sideLength_Float = visibleArea_RectangleF.Width;
            else sideLength_Float = visibleArea_RectangleF.Height;

            RectangleF viewport_RectangleF = new RectangleF
                (
                    new Vector2
                    (
                        visibleArea_RectangleF.Center.X - sideLength_Float / 2,
                        visibleArea_RectangleF.Center.Y - sideLength_Float / 2
                    ),
                    new Vector2(sideLength_Float, sideLength_Float)
                );
            DrawBox(ref frame, viewport_RectangleF, font_Color_Overall);
            
            float
                fontsize_ScalingFactor_Float = sideLength_Float / 512f,
                fontsize_Tag_Float = 0.6f,
                fontsize_ProgressBar_Float = 1.2f,
                height_Row_Float = viewport_RectangleF.Height / 7f,
                border_Float = height_Row_Float * 0.02f,
                contentArea_ScalingFactor_Float = 0.85f,
                warningSign_ScalingFactor_Float = 0.7f;

            Vector2
                iconBox_BackGround_Size_Vector2 = new Vector2
                (
                    height_Row_Float - 2f * border_Float,
                    height_Row_Float - 2f * border_Float
                ),
                title_BackGround_Size_Vector2 = new Vector2
                (
                    viewport_RectangleF.Width - 2f * height_Row_Float - 2f * border_Float,
                    height_Row_Float - 2f * border_Float
                ),
                progressBar_BackGround_Size_Vector2 = new Vector2
                (
                    viewport_RectangleF.Width - height_Row_Float - 2f * border_Float,
                    height_Row_Float - 2f * border_Float
                ),
                progressBar_Text_Size_Vector2 = new Vector2
                (
                    viewport_RectangleF.Width - height_Row_Float - 2f * border_Float,
                    (height_Row_Float - 2f * border_Float) / 2f
                );




            //  Title
            RectangleF
                logo_Left_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(viewport_RectangleF.X + border_Float, viewport_RectangleF.Y + border_Float),
                    iconBox_BackGround_Size_Vector2
                ),
                logo_Left_Content_RectangleF = ScalingViewport
                (
                    logo_Left_BackGround_RectangleF,
                    contentArea_ScalingFactor_Float
                ),
                logo_Right_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(viewport_RectangleF.Right - logo_Left_BackGround_RectangleF.Width - border_Float, logo_Left_BackGround_RectangleF.Y),
                    iconBox_BackGround_Size_Vector2
                ),
                logo_Right_Content_RectangleF = ScalingViewport
                (
                    logo_Right_BackGround_RectangleF,
                    contentArea_ScalingFactor_Float
                ),
                title_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(logo_Left_BackGround_RectangleF.X + height_Row_Float, viewport_RectangleF.Y + border_Float),
                    title_BackGround_Size_Vector2
                );
            DrawBox(ref frame, logo_Left_BackGround_RectangleF, card_Background_Color_Overall);
            DrawBox(ref frame, title_BackGround_RectangleF, card_Background_Color_Overall);
            DrawBox(ref frame, logo_Right_BackGround_RectangleF, card_Background_Color_Overall);
            DrawLogo(ref frame, logo_Left_Content_RectangleF);
            DrawLogo(ref frame, logo_Right_Content_RectangleF);
            PanelWriteText
            (
                ref frame,
                panel.GetOwnerFactionTag(),
                title_BackGround_RectangleF,
                2.3f * fontsize_ScalingFactor_Float, 
                font_Color_Overall,
                TextAlignment.CENTER
            );

            //  Cargo
            RectangleF
                cargo_Icon_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(logo_Left_BackGround_RectangleF.X, logo_Left_BackGround_RectangleF.Y + height_Row_Float),
                    iconBox_BackGround_Size_Vector2
                ),
                cargo_Icon_Content_RectangleF = ScalingViewport
                (
                    cargo_Icon_BackGround_RectangleF,
                    contentArea_ScalingFactor_Float
                ),
                cargo_ProgressBar_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(cargo_Icon_BackGround_RectangleF.X + height_Row_Float, cargo_Icon_BackGround_RectangleF.Y),
                    progressBar_BackGround_Size_Vector2
                ),
                cargo_ProgressBar_Text_Ratio_RectangleF = new RectangleF
                (
                    new Vector2(cargo_ProgressBar_BackGround_RectangleF.X, cargo_ProgressBar_BackGround_RectangleF.Y),
                    progressBar_Text_Size_Vector2
                ),
                cargo_ProgressBar_Text_Actual_RectangleF = new RectangleF
                (
                    new Vector2(cargo_ProgressBar_BackGround_RectangleF.X, cargo_ProgressBar_BackGround_RectangleF.Center.Y),
                    progressBar_Text_Size_Vector2
                );
            DrawBox(ref frame, cargo_Icon_BackGround_RectangleF, card_Background_Color_Overall);
            DrawIcon(ref frame, "Textures\\FactionLogo\\Builders\\BuilderIcon_1.dds", cargo_Icon_Content_RectangleF, font_Color_Overall);
            PanelWriteText
            (
                ref frame,
                cargoContainers.Count.ToString(),
                cargo_Icon_Content_RectangleF, 
                fontsize_Tag_Float * fontsize_ScalingFactor_Float, 
                font_Color_Overall, 
                TextAlignment.RIGHT
            );
            if (function_ShowItems_Bool == false)
            {
                DrawIcon(ref frame, "Danger", cargo_Icon_Content_RectangleF, font_Color_Overall);
            }

            DrawBox(ref frame, cargo_ProgressBar_BackGround_RectangleF, card_Background_Color_Overall);
            string percentage_String, finalValue_String;
            CalculateAll(out percentage_String, out finalValue_String);
            ProgressBar(ref frame, cargo_ProgressBar_BackGround_RectangleF, percentage_String);
            PanelWriteText
            (
                ref frame,
                percentage_String,
                cargo_ProgressBar_Text_Ratio_RectangleF,
                fontsize_ProgressBar_Float * fontsize_ScalingFactor_Float,
                font_Color_Overall,
                TextAlignment.CENTER
            );
            PanelWriteText
            (
                ref frame,
                finalValue_String,
                cargo_ProgressBar_Text_Actual_RectangleF,
                fontsize_ProgressBar_Float * fontsize_ScalingFactor_Float,
                font_Color_Overall,
                TextAlignment.CENTER
            );

            //  H2
            RectangleF
                hydrogen_Icon_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(cargo_Icon_BackGround_RectangleF.X, cargo_Icon_BackGround_RectangleF.Y + height_Row_Float),
                    iconBox_BackGround_Size_Vector2
                ),
                hydrogen_Icon_Content_RectangleF = ScalingViewport
                (
                    hydrogen_Icon_BackGround_RectangleF,
                    contentArea_ScalingFactor_Float
                ),
                hydrogen_ProgressBar_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(hydrogen_Icon_BackGround_RectangleF.X + height_Row_Float, hydrogen_Icon_BackGround_RectangleF.Y),
                    progressBar_BackGround_Size_Vector2
                ),
                hydrogen_ProgressBar_Text_Ratio_RectangleF = new RectangleF
                (
                    new Vector2(hydrogen_ProgressBar_BackGround_RectangleF.X, hydrogen_ProgressBar_BackGround_RectangleF.Y),
                    progressBar_Text_Size_Vector2
                ),
                hydrogen_ProgressBar_Text_Actual_RectangleF = new RectangleF
                (
                    new Vector2(hydrogen_ProgressBar_BackGround_RectangleF.X, hydrogen_ProgressBar_BackGround_RectangleF.Center.Y),
                    progressBar_Text_Size_Vector2
                );
            DrawBox(ref frame, hydrogen_Icon_BackGround_RectangleF, card_Background_Color_Overall);
            DrawBox(ref frame, hydrogen_ProgressBar_BackGround_RectangleF, card_Background_Color_Overall);
            DrawIcon(ref frame, "IconHydrogen", hydrogen_Icon_Content_RectangleF, font_Color_Overall);
            PanelWriteText
            (
                ref frame, 
                hydrogenTanks.Count.ToString(), 
                hydrogen_Icon_Content_RectangleF, 
                fontsize_Tag_Float * fontsize_ScalingFactor_Float, 
                font_Color_Overall, 
                TextAlignment.RIGHT
            );

            CalcualateGasTank(hydrogenTanks, out percentage_String, out finalValue_String);
            ProgressBar(ref frame, hydrogen_ProgressBar_BackGround_RectangleF, percentage_String);
            PanelWriteText
            (
                ref frame,
                percentage_String,
                hydrogen_ProgressBar_Text_Ratio_RectangleF,
                fontsize_ProgressBar_Float * fontsize_ScalingFactor_Float,
                font_Color_Overall,
                TextAlignment.CENTER
            );
            PanelWriteText
            (
                ref frame,
                finalValue_String,
                hydrogen_ProgressBar_Text_Actual_RectangleF,
                fontsize_ProgressBar_Float * fontsize_ScalingFactor_Float,
                font_Color_Overall,
                TextAlignment.CENTER
            );

            //  O2
            RectangleF
                oxydrogen_Icon_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(hydrogen_Icon_BackGround_RectangleF.X, hydrogen_Icon_BackGround_RectangleF.Y + height_Row_Float),
                    iconBox_BackGround_Size_Vector2
                ),
                oxydrogen_Icon_Content_RectangleF = ScalingViewport
                (
                    oxydrogen_Icon_BackGround_RectangleF,
                    contentArea_ScalingFactor_Float
                ),
                oxydrogen_ProgressBar_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(oxydrogen_Icon_BackGround_RectangleF.X + height_Row_Float, oxydrogen_Icon_BackGround_RectangleF.Y),
                    progressBar_BackGround_Size_Vector2
                ),
                oxydrogen_ProgressBar_Text_Ratio_RectangleF = new RectangleF
                (
                    new Vector2(oxydrogen_ProgressBar_BackGround_RectangleF.X, oxydrogen_Icon_BackGround_RectangleF.Y),
                    progressBar_Text_Size_Vector2
                ),
                oxydrogen_ProgressBar_Text_Actual_RectangleF = new RectangleF
                (
                    new Vector2(oxydrogen_ProgressBar_BackGround_RectangleF.X, oxydrogen_Icon_BackGround_RectangleF.Center.Y),
                    progressBar_Text_Size_Vector2
                );
            DrawBox(ref frame, oxydrogen_Icon_BackGround_RectangleF, card_Background_Color_Overall);
            DrawBox(ref frame, oxydrogen_ProgressBar_BackGround_RectangleF, card_Background_Color_Overall);
            DrawIcon(ref frame, "IconOxygen", oxydrogen_Icon_Content_RectangleF, font_Color_Overall);
            PanelWriteText
            (
                ref frame, 
                oxygenTanks.Count.ToString(),
                oxydrogen_Icon_Content_RectangleF, 
                fontsize_Tag_Float * fontsize_ScalingFactor_Float, 
                font_Color_Overall, 
                TextAlignment.RIGHT
            );

            CalcualateGasTank(oxygenTanks, out percentage_String, out finalValue_String);
            ProgressBar(ref frame, oxydrogen_ProgressBar_BackGround_RectangleF, percentage_String);
            PanelWriteText
            (
                ref frame,
                percentage_String,
                oxydrogen_ProgressBar_Text_Ratio_RectangleF,
                fontsize_ProgressBar_Float * fontsize_ScalingFactor_Float,
                font_Color_Overall,
                TextAlignment.CENTER
            );
            PanelWriteText
            (
                ref frame,
                finalValue_String,
                oxydrogen_ProgressBar_Text_Actual_RectangleF,
                fontsize_ProgressBar_Float * fontsize_ScalingFactor_Float,
                font_Color_Overall,
                TextAlignment.CENTER
            );

            //  Power
            RectangleF
                power_Icon_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(oxydrogen_Icon_BackGround_RectangleF.X, oxydrogen_Icon_BackGround_RectangleF.Y + height_Row_Float),
                    iconBox_BackGround_Size_Vector2
                ),
                power_Icon_Content_RectangleF = ScalingViewport
                (
                    power_Icon_BackGround_RectangleF,
                    contentArea_ScalingFactor_Float
                ),
                power_ProgressBar_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(power_Icon_BackGround_RectangleF.X + height_Row_Float, power_Icon_BackGround_RectangleF.Y),
                    progressBar_BackGround_Size_Vector2
                ),
                power_ProgressBar_Text_Ratio_RectangleF = new RectangleF
                (
                    new Vector2(power_ProgressBar_BackGround_RectangleF.X, power_ProgressBar_BackGround_RectangleF.Y),
                    progressBar_Text_Size_Vector2
                ),
                power_ProgressBar_Text_Actual_RectangleF = new RectangleF
                (
                    new Vector2(power_ProgressBar_BackGround_RectangleF.X, power_ProgressBar_BackGround_RectangleF.Center.Y),
                    progressBar_Text_Size_Vector2
                );
            DrawBox(ref frame, power_Icon_BackGround_RectangleF, card_Background_Color_Overall);
            DrawBox(ref frame, power_ProgressBar_BackGround_RectangleF, card_Background_Color_Overall);
            DrawIcon(ref frame, "IconEnergy", power_Icon_Content_RectangleF, font_Color_Overall);
            PanelWriteText
            (
                ref frame, 
                powerProducers.Count.ToString(), 
                power_Icon_Content_RectangleF, 
                fontsize_Tag_Float * fontsize_ScalingFactor_Float, 
                font_Color_Overall, 
                TextAlignment.RIGHT
            );

            CalculatePowerProducer(out percentage_String, out finalValue_String);
            ProgressBar(ref frame, power_ProgressBar_BackGround_RectangleF, percentage_String);
            PanelWriteText
            (
                ref frame,
                percentage_String,
                power_ProgressBar_Text_Ratio_RectangleF,
                fontsize_ProgressBar_Float * fontsize_ScalingFactor_Float,
                font_Color_Overall,
                TextAlignment.CENTER
            );
            PanelWriteText
            (
                ref frame,
                finalValue_String,
                power_ProgressBar_Text_Actual_RectangleF,
                fontsize_ProgressBar_Float * fontsize_ScalingFactor_Float,
                font_Color_Overall,
                TextAlignment.CENTER
            );

            //  Antenna
            RectangleF
                antenna_Icon_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(power_Icon_BackGround_RectangleF.X, power_Icon_BackGround_RectangleF.Y + height_Row_Float),
                    iconBox_BackGround_Size_Vector2
                ),
                antenna_Icon_Content_RectangleF = ScalingViewport
                (
                    antenna_Icon_BackGround_RectangleF,
                    contentArea_ScalingFactor_Float
                ),
                antenna_ProgressBar_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(antenna_Icon_BackGround_RectangleF.X + height_Row_Float, antenna_Icon_BackGround_RectangleF.Y),
                    progressBar_BackGround_Size_Vector2
                ),
                antenna_ProgressBar_Text_Ratio_RectangleF = new RectangleF
                (
                    new Vector2(antenna_ProgressBar_BackGround_RectangleF.X, antenna_ProgressBar_BackGround_RectangleF.Y),
                    progressBar_Text_Size_Vector2
                ),
                antenna_ProgressBar_Text_Actual_RectangleF = new RectangleF
                (
                    new Vector2(antenna_ProgressBar_BackGround_RectangleF.X, antenna_ProgressBar_BackGround_RectangleF.Center.Y),
                    progressBar_Text_Size_Vector2
                );
            DrawBox(ref frame, antenna_Icon_BackGround_RectangleF, card_Background_Color_Overall);
            DrawBox(ref frame, antenna_ProgressBar_BackGround_RectangleF, card_Background_Color_Overall);
            IGCSignifier(ref frame, antenna_Icon_Content_RectangleF, font_Color_Overall);
            if (function_BroadCastConnectorGPS_Bool == false)
            {
                DrawIcon(ref frame, "Danger", antenna_Icon_Content_RectangleF, font_Color_Overall);
            }

            string igcTag_String = GetValue_from_CustomData(information_Section, "IGCTAG");
            PanelWriteText
            (
                ref frame,
                " " + igcTag_String,
                antenna_ProgressBar_Text_Ratio_RectangleF,
                fontsize_ProgressBar_Float * fontsize_ScalingFactor_Float,
                font_Color_Overall,
                TextAlignment.LEFT
            );
            PanelWriteText
            (
                ref frame,
                " " + AntennaDistance(),
                antenna_ProgressBar_Text_Actual_RectangleF,
                fontsize_ProgressBar_Float * fontsize_ScalingFactor_Float,
                font_Color_Overall,
                TextAlignment.LEFT
            );

            //  Facility
            RectangleF
                showFacilities_Icon_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(antenna_Icon_BackGround_RectangleF.X, antenna_Icon_BackGround_RectangleF.Y + height_Row_Float),
                    iconBox_BackGround_Size_Vector2
                ),
                showFacilities_Icon_Content_RectangleF = ScalingViewport
                (
                    showFacilities_Icon_BackGround_RectangleF,
                    contentArea_ScalingFactor_Float
                );
            DrawBox(ref frame, showFacilities_Icon_BackGround_RectangleF, card_Background_Color_Overall);
            FacilitySignifier(ref frame, showFacilities_Icon_Content_RectangleF, font_Color_Overall);
            if (function_ShowFacilities_Bool == false)
            {
                DrawIcon(ref frame, "Danger", showFacilities_Icon_Content_RectangleF, font_Color_Overall);
            }

            //  Inventory
            RectangleF
                inventory_Icon_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(showFacilities_Icon_BackGround_RectangleF.X + height_Row_Float, showFacilities_Icon_BackGround_RectangleF.Y),
                    iconBox_BackGround_Size_Vector2
                ),
                inventory_Icon_Content_RectangleF = ScalingViewport
                (
                    inventory_Icon_BackGround_RectangleF,
                    contentArea_ScalingFactor_Float
                ),
                inventory_WarningSign_RectangleF = ScalingViewport
                (
                    inventory_Icon_BackGround_RectangleF,
                    warningSign_ScalingFactor_Float
                );

            DrawBox(ref frame, inventory_Icon_BackGround_RectangleF, card_Background_Color_Overall);
            InventorySignifier(ref frame, inventory_Icon_Content_RectangleF, font_Color_Overall, card_Background_Color_Overall);
            if (function_InventoryManagement_Bool == false)
            {
                DrawIcon(ref frame, "Danger", inventory_WarningSign_RectangleF, font_Color_Overall);
            }

            //  Cargo Residues
            RectangleF
                cargoResidues_Icon_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(inventory_Icon_BackGround_RectangleF.X + height_Row_Float, inventory_Icon_BackGround_RectangleF.Y),
                    iconBox_BackGround_Size_Vector2
                ),
                cargoResidues_Icon_Content_RectangleF = ScalingViewport
                (
                    cargoResidues_Icon_BackGround_RectangleF,
                    contentArea_ScalingFactor_Float
                ),
                cargoResidues_WarningSign_RectangleF = ScalingViewport
                (
                    cargoResidues_Icon_BackGround_RectangleF,
                    warningSign_ScalingFactor_Float
                );
            DrawBox(ref frame, cargoResidues_Icon_BackGround_RectangleF, card_Background_Color_Overall);
            DrawIcon(ref frame, "Textures\\FactionLogo\\Builders\\BuilderIcon_1.dds", cargoResidues_Icon_Content_RectangleF, font_Color_Overall);
            if (function_ShowCargoContainerRatio_Bool == false)
            {
                DrawIcon(ref frame, "Danger", cargoResidues_WarningSign_RectangleF, font_Color_Overall);
            }
            PanelWriteText
            (
                ref frame,
                "%",
                cargoResidues_Icon_Content_RectangleF,
                fontsize_Tag_Float * fontsize_ScalingFactor_Float,
                font_Color_Overall,
                TextAlignment.RIGHT
            );

            //  Refresh Rate
            RectangleF
                refreshRate_Icon_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(cargoResidues_Icon_BackGround_RectangleF.X + height_Row_Float, cargoResidues_Icon_BackGround_RectangleF.Y),
                    iconBox_BackGround_Size_Vector2
                ),
                refreshRate_Icon_Content_RectangleF = ScalingViewport
                (
                    refreshRate_Icon_BackGround_RectangleF,
                    contentArea_ScalingFactor_Float
                );
            DrawBox(ref frame, refreshRate_Icon_BackGround_RectangleF, card_Background_Color_Overall);
            RefreshRateSignifier(ref frame, refreshRate_Icon_Content_RectangleF, font_Color_Overall, card_Background_Color_Overall);

            //  Combined_Refining
            RectangleF
                combinedRefining_Icon_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(refreshRate_Icon_BackGround_RectangleF.X + height_Row_Float, refreshRate_Icon_BackGround_RectangleF.Y),
                    iconBox_BackGround_Size_Vector2
                ),
                combinedRefining_Icon_Content_RectangleF = ScalingViewport
                (
                    combinedRefining_Icon_BackGround_RectangleF,
                    contentArea_ScalingFactor_Float
                );
            DrawBox(ref frame, combinedRefining_Icon_BackGround_RectangleF, card_Background_Color_Overall);
            DrawIcon(ref frame, "MyObjectBuilder_Ore/Stone", combinedRefining_Icon_Content_RectangleF, font_Color_Overall);
            string combined_Refining_Mode_String = GetValue_from_CustomData(ore_Section, combinedMode_Key);
            PanelWriteText
            (
                ref frame,
                combined_Refining_Mode_String,
                combinedRefining_Icon_Content_RectangleF,
                fontsize_Tag_Float * fontsize_ScalingFactor_Float,
                font_Color_Overall,
                TextAlignment.RIGHT
            );

            //  AutoProduction
            RectangleF
                autoProdcution_Icon_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(combinedRefining_Icon_BackGround_RectangleF.X + height_Row_Float, combinedRefining_Icon_BackGround_RectangleF.Y),
                    iconBox_BackGround_Size_Vector2
                ),
                autoProdcution_Icon_Content_RectangleF = ScalingViewport
                (
                    autoProdcution_Icon_BackGround_RectangleF,
                    contentArea_ScalingFactor_Float
                ),
                autoProdcution_WarningSign_RectangleF = ScalingViewport
                (
                    autoProdcution_Icon_BackGround_RectangleF,
                    warningSign_ScalingFactor_Float
                );
            DrawBox(ref frame, autoProdcution_Icon_BackGround_RectangleF, card_Background_Color_Overall);
            DrawIcon(ref frame, "MyObjectBuilder_PhysicalGunObject/WelderItem", autoProdcution_Icon_Content_RectangleF, font_Color_Overall);
            if (function_AutoProduction_Bool == false)
            {
                DrawIcon(ref frame, "Danger", autoProdcution_WarningSign_RectangleF, font_Color_Overall);
            }

            //  Empty_Slot
            RectangleF
                emptySlot_Icon_BackGround_RectangleF = new RectangleF
                (
                    new Vector2(autoProdcution_Icon_BackGround_RectangleF.X + height_Row_Float, autoProdcution_Icon_BackGround_RectangleF.Y),
                    iconBox_BackGround_Size_Vector2
                ),
                emptySlot_Icon_Content_RectangleF = ScalingViewport
                (
                    emptySlot_Icon_BackGround_RectangleF,
                    contentArea_ScalingFactor_Float
                );
            DrawBox(ref frame, emptySlot_Icon_BackGround_RectangleF, card_Background_Color_Overall);

        }

        public RectangleF ScalingViewport(RectangleF datumPoint_RectangleF, float content_ScalingFactor_Float, int ratio_Pixel_1_2_Int = 1)
        {
            float 
                width_Float = datumPoint_RectangleF.Width,
                height_Float = datumPoint_RectangleF.Height;

            if(ratio_Pixel_1_2_Int == 1)
            {
                width_Float = width_Float * content_ScalingFactor_Float;
                height_Float = height_Float * content_ScalingFactor_Float;
            }
            else
            {
                if(width_Float < height_Float)
                {
                    width_Float = width_Float * content_ScalingFactor_Float;
                    height_Float -= 2f * width_Float * (1f - content_ScalingFactor_Float);
                }
                else
                {
                    height_Float = height_Float * content_ScalingFactor_Float;
                    width_Float -= 2f * height_Float * (1f - content_ScalingFactor_Float);
                }
            }

            float
                x_Float = datumPoint_RectangleF.Center.X - width_Float / 2f,
                y_Float = datumPoint_RectangleF.Center.Y - height_Float / 2f;

            return new RectangleF(new Vector2(x_Float, y_Float), new Vector2(width_Float, height_Float));
        }

        public void ProgressBar(ref MySpriteDrawFrame frame, RectangleF viewport_RectangleF, string ratio)
        {
            string[] ratiogroup = ratio.Split('%');
            float ratio_Float = Convert.ToSingle(ratiogroup[0]);
            float currentWidth = viewport_RectangleF.Width * ratio_Float / 100;
            float currentX = viewport_RectangleF.Center.X - viewport_RectangleF.Width / 2 + currentWidth / 2;

            if (ratio_Float == 0) return;

            DrawIcon(ref frame, "SquareSimple", currentX, viewport_RectangleF.Center.Y, currentWidth, viewport_RectangleF.Height, progressbar_Color);
        }

        public void DrawLogo(ref MySpriteDrawFrame frame, RectangleF viewport_RectangleF)
        {
            float
                x_Float = viewport_RectangleF.Center.X,
                y_Float = viewport_RectangleF.Center.Y,
                width_Float = viewport_RectangleF.Width,
                height_Float = viewport_RectangleF.Height;

            if (width_Float > height_Float) width_Float = height_Float;

            MySprite sprite = new MySprite()
            {
                Type = SpriteType.TEXTURE,
                Data = "Screen_LoadingBar",
                Position = new Vector2(x_Float, y_Float),
                Size = new Vector2(width_Float, width_Float),
                RotationOrScale = Convert.ToSingle(counter_Logo_Float / 360 * 2 * Math.PI),
                Alignment = TextAlignment.CENTER,
                Color = font_Color_Overall,
            };
            frame.Add(sprite);

            sprite = new MySprite()
            {
                Type = SpriteType.TEXTURE,
                Data = "Screen_LoadingBar",
                Position = new Vector2(x_Float, y_Float),
                Size = new Vector2(width_Float * 0.6f, width_Float * 0.6f),
                RotationOrScale = Convert.ToSingle(2 * Math.PI - counter_Logo_Float / 360 * 2 * Math.PI),
                Alignment = TextAlignment.CENTER,
                Color = font_Color_Overall,

            };
            frame.Add(sprite);

            sprite = new MySprite()
            {
                Type = SpriteType.TEXTURE,
                Data = "Screen_LoadingBar",
                Position = new Vector2(x_Float, y_Float),
                Size = new Vector2(width_Float * 0.33f, width_Float * 0.33f),
                RotationOrScale = Convert.ToSingle(Math.PI + counter_Logo_Float / 360 * 2 * Math.PI),
                Alignment = TextAlignment.CENTER,
                Color = font_Color_Overall,
            };
            frame.Add(sprite);

        }

        public void CalculateAll(out string percentage_String, out string finalValue_String)
        {
            double currentVolume_Double = 0, totalVolume_Double = 0;

            foreach (var cargoContainer in cargoContainers)
            {
                currentVolume_Double += ((double)cargoContainer.GetInventory().CurrentVolume);
                totalVolume_Double += ((double)cargoContainer.GetInventory().MaxVolume);
            }

            percentage_String = Math.Round(currentVolume_Double / totalVolume_Double * 100, 1).ToString() + "%";
            finalValue_String = AmountUnitConversion(currentVolume_Double * 1000, false) + " L / " + AmountUnitConversion(totalVolume_Double * 1000, false) + " L";
        }

        public void CalcualateGasTank(List<IMyGasTank> tanks, out string percentage_String, out string finalValue_String)
        {
            double currentVolume_Double = 0, totalVolume_Double = 0;

            foreach (var tank in tanks)
            {
                currentVolume_Double += tank.Capacity * tank.FilledRatio;
                totalVolume_Double += tank.Capacity;
            }

            percentage_String = Math.Round(currentVolume_Double / totalVolume_Double * 100, 1).ToString() + "%";
            finalValue_String = AmountUnitConversion(currentVolume_Double, false) + " L / " + AmountUnitConversion(totalVolume_Double, false) + " L";
        }

        public void CalculatePowerProducer(out string percentage_String, out string finalValue_String)
        {
            float currentOutput = 0, totalOutput = 0;
            foreach (var powerProducer in powerProducers)
            {
                currentOutput += powerProducer.CurrentOutput;
                totalOutput += powerProducer.MaxOutput;
            }

            percentage_String = Math.Round(currentOutput / totalOutput * 100, 1).ToString() + "%";
            finalValue_String = AmountUnitConversion(currentOutput * 1000000, true) + " W / " + AmountUnitConversion(totalOutput * 1000000, true) + " W";
        }

        public string AmountUnitConversion(double amount, bool isPowerPorducer)
        {
            double temp = 0;
            string result = "";

            if (amount >= 1000000000000000)
            {
                temp = Math.Round(amount / 1000000000000000, 1);
                result = temp.ToString() + "KT";
            }
            else if (amount >= 1000000000000)
            {
                temp = Math.Round(amount / 1000000000000, 1);
                result = temp.ToString() + "T";
            }
            else if (amount >= 1000000000)
            {
                temp = Math.Round(amount / 1000000000, 1);
                if (isPowerPorducer) result = temp.ToString() + "G";
                else result = temp.ToString() + "B";
            }
            else if (amount >= 1000000)
            {
                temp = Math.Round(amount / 1000000, 1);
                result = temp.ToString() + "M";
            }
            else if (amount >= 1000)
            {
                temp = Math.Round(amount / 1000, 1);
                result = temp.ToString() + "K";
            }
            else
            {
                temp = Math.Round(amount, 1);
                result = temp.ToString();
            }

            return result;
        }

        public void DrawBox(ref MySpriteDrawFrame frame, RectangleF viewPort_RectangleF, Color background_Color)
        {
            DrawIcon(ref frame, "SquareSimple", viewPort_RectangleF, background_Color);
        }

        public void PanelWriteText(ref MySpriteDrawFrame frame, string text, RectangleF viewport_REctangleF, float fontSize, Color font_Color, TextAlignment alignment = TextAlignment.LEFT)
        {

            float 
                x_Float, 
                y_Float = viewport_REctangleF.Y;

            if(alignment == TextAlignment.LEFT) x_Float = viewport_REctangleF.X;
            else if(alignment == TextAlignment.CENTER) x_Float = viewport_REctangleF.Center.X;
            else x_Float = viewport_REctangleF.Right;

            using 
            (
                frame.Clip
                    (
                        (int)(viewport_REctangleF.X - viewport_REctangleF.Width * 0.05f), 
                        (int)viewport_REctangleF.Y, 
                        (int)(viewport_REctangleF.Width * 1.1f),
                        (int)viewport_REctangleF.Height
                    )
            )
            {
                MySprite sprite = new MySprite()
                {
                    Type = SpriteType.TEXT,
                    Data = text,
                    Position = new Vector2(x_Float, y_Float),
                    RotationOrScale = fontSize,
                    Color = font_Color,
                    Alignment = alignment,
                    FontId = "LoadingScreen"
                    //FontId = "Monospace"
                };

                frame.Add(sprite);
            }
        }

        public void IGCSignifier(ref MySpriteDrawFrame frame, RectangleF viewport_RectangleF, Color icon_Color)
        {
            float
                width_Float = viewport_RectangleF.Width / 8f,
                height_Float = viewport_RectangleF.Height / 3f;

            RectangleF
                bar1_RectangleF = new RectangleF
                (
                    new Vector2(viewport_RectangleF.Right - width_Float, viewport_RectangleF.Y),
                    new Vector2(width_Float, height_Float * 3f)
                ),
                bar2_RectangleF = new RectangleF
                (
                    new Vector2(viewport_RectangleF.Right - width_Float * 3f, viewport_RectangleF.Y + height_Float),
                    new Vector2(width_Float, height_Float * 2f)
                ),
                bar3_RectangleF = new RectangleF
                (
                    new Vector2(viewport_RectangleF.Right - width_Float * 5f, viewport_RectangleF.Y + height_Float * 2f),
                    new Vector2(width_Float, height_Float)
                ),
                bar4_RectangleF = new RectangleF
                (
                    new Vector2(viewport_RectangleF.Right - width_Float * 7f, viewport_RectangleF.Y + height_Float * 0.5f),
                    new Vector2(width_Float, height_Float * 3f - height_Float * 0.5f)
                ),
                triangle_RectangleF = new RectangleF
                (
                    new Vector2(viewport_RectangleF.X, viewport_RectangleF.Y),
                    new Vector2(width_Float * 3f, height_Float * 1.5f)
                );

            DrawBox(ref frame, bar1_RectangleF, icon_Color);
            DrawBox(ref frame, bar2_RectangleF, icon_Color);
            DrawBox(ref frame, bar3_RectangleF, icon_Color);
            DrawBox(ref frame, bar4_RectangleF, icon_Color);
            DrawIcon(ref frame, "Triangle", triangle_RectangleF, icon_Color, 180f);
        }

        public string AntennaDistance()
        {
            float distance_Float = 0;
            int k = 1;
            foreach(var radioAntenna in radioAntennas)
            {
                if(radioAntenna.Enabled && radioAntenna.EnableBroadcasting)
                {
                    if (k == 1)
                    {
                        distance_Float = radioAntenna.Radius;
                        k++;
                    }
                    else if (distance_Float < radioAntenna.Radius) distance_Float = radioAntenna.Radius;
                }
            }

            return AmountUnitConversion(distance_Float, false) + "m";
        }

        public void DrawIcon(ref MySpriteDrawFrame frame, string icon, float x, float y, float width, float height, Color picture_Color, float rotation = 0)
        {
            var sprite = new MySprite
            {
                Type = SpriteType.TEXTURE,
                Data = icon,
                Position = new Vector2(x, y),
                RotationOrScale = Convert.ToSingle(rotation / 360f * 2f * Math.PI),
                Size = new Vector2(width, height),
                Color = picture_Color,
                Alignment = TextAlignment.CENTER
            };

            frame.Add(sprite);
        }

        public void DrawIcon(ref MySpriteDrawFrame frame, string icon, RectangleF viewport_RectagnleF, Color picture_Color, float rotation = 0)
        {
            var sprite = new MySprite
            {
                Type = SpriteType.TEXTURE,
                Data = icon,
                Position = new Vector2(viewport_RectagnleF.Center.X, viewport_RectagnleF.Center.Y),
                RotationOrScale = Convert.ToSingle(rotation / 360f * 2f * Math.PI),
                Size = new Vector2(viewport_RectagnleF.Width, viewport_RectagnleF.Height),
                Color = picture_Color,
                Alignment = TextAlignment.CENTER
            };

            frame.Add(sprite);
        }

        public void FacilitySignifier(ref MySpriteDrawFrame frame, RectangleF viewport_RectangleF, Color icon_Color)
        {
            float
                width_SmallBox_Float = viewport_RectangleF.Height / 3f,
                width_LargeBox_Float = viewport_RectangleF.Width - width_SmallBox_Float,
                scalingFactor_Float = 0.7f;

            RectangleF
                box_Small_Up_RectangleF = new RectangleF
                (
                    new Vector2(viewport_RectangleF.X, viewport_RectangleF.Y),
                    new Vector2(width_SmallBox_Float, width_SmallBox_Float)
                ),
                box_Small_Middle_RectangleF = new RectangleF
                (
                    new Vector2(box_Small_Up_RectangleF.X, box_Small_Up_RectangleF.Y + width_SmallBox_Float),
                    new Vector2(width_SmallBox_Float, width_SmallBox_Float)
                ),
                box_Small_Down_RectangleF = new RectangleF
                (
                    new Vector2(box_Small_Middle_RectangleF.X, box_Small_Middle_RectangleF.Y + width_SmallBox_Float),
                    new Vector2(width_SmallBox_Float, width_SmallBox_Float)
                ),

                box_Large_Up_RectangleF = new RectangleF
                (
                    new Vector2(viewport_RectangleF.X + width_SmallBox_Float, viewport_RectangleF.Y),
                    new Vector2(width_LargeBox_Float, width_SmallBox_Float)
                ),
                box_Large_Middle_RectangleF = new RectangleF
                (
                    new Vector2(box_Large_Up_RectangleF.X, box_Large_Up_RectangleF.Y + width_SmallBox_Float),
                    new Vector2(width_LargeBox_Float, width_SmallBox_Float)
                ),
                box_Large_Down_RectangleF = new RectangleF
                (
                    new Vector2(box_Large_Middle_RectangleF.X, box_Large_Middle_RectangleF.Y + width_SmallBox_Float),
                    new Vector2(width_LargeBox_Float, width_SmallBox_Float)
                );

            List<RectangleF> viewport_List = new List<RectangleF>()
            {
                box_Small_Up_RectangleF,
                box_Small_Middle_RectangleF,
                box_Small_Down_RectangleF,
                box_Large_Up_RectangleF,
                box_Large_Middle_RectangleF,
                box_Large_Down_RectangleF
            };

            for(int i = 0; i <= 2; i++)
            {
                RectangleF viewport_Temp = viewport_List[i];
                viewport_Temp = ScalingViewport(viewport_Temp, scalingFactor_Float);
                DrawBox(ref frame, viewport_Temp, icon_Color);
            }

            for(int i = 3; i <= 5; i++)
            {
                RectangleF viewport_Temp = viewport_List[i];
                viewport_Temp = ScalingViewport(viewport_Temp, scalingFactor_Float, 2);
                DrawBox(ref frame, viewport_Temp, icon_Color);
            }


        }

        public void InventorySignifier(ref MySpriteDrawFrame frame, RectangleF viewport_RectangleF, Color front_Color, Color backGround_Color)
        {
            RectangleF
                box_Exterior_RectangleF = new RectangleF
                (
                    new Vector2(viewport_RectangleF.X, viewport_RectangleF.Center.Y),
                    new Vector2(viewport_RectangleF.Width, viewport_RectangleF.Height / 2f)
                ),
                box_Inner_RectangleF = ScalingViewport
                (
                    viewport_RectangleF,
                    0.7f
                ),
                arrow_Box_RectangleF = new RectangleF
                (
                    new Vector2
                    (
                        viewport_RectangleF.Center.X - viewport_RectangleF.Width * 0.125f, 
                        viewport_RectangleF.Y
                    ),
                    new Vector2(viewport_RectangleF.Width * 0.25f, viewport_RectangleF.Height * 0.25f)
                ),
                arrow_Triangle_RectangleF = ScalingViewport
                (
                    viewport_RectangleF,
                    0.6f
                );

            DrawBox(ref frame, box_Exterior_RectangleF, front_Color);
            DrawBox(ref frame, box_Inner_RectangleF, backGround_Color);
            DrawBox(ref frame, arrow_Box_RectangleF, front_Color);
            DrawIcon(ref frame, "Triangle", arrow_Triangle_RectangleF, front_Color, 180f);
        }

        public void RefreshRateSignifier(ref MySpriteDrawFrame frame, RectangleF viewport_RectangleF, Color border_Color, Color background_Color)
        {
            float
                scalingFactor_Float = 0.85f,
                width_Triangle_Exterior_Float = viewport_RectangleF.Width / 3f,
                width_Triagnle_Inner_Float = width_Triangle_Exterior_Float * scalingFactor_Float,
                height_Triangle_Exterior_Float = viewport_RectangleF.Height,
                height_Triangle_Inner_Float = viewport_RectangleF.Height * scalingFactor_Float;


            Vector2
                size_Triangle_Exterior_Float = new Vector2(width_Triangle_Exterior_Float, height_Triangle_Exterior_Float),
                size_Triangle_Inner_Float = new Vector2(width_Triagnle_Inner_Float, height_Triangle_Inner_Float);


            RectangleF
                triangle_Exterior_Left_RectangleF = new RectangleF
                (
                    new Vector2
                    (
                        viewport_RectangleF.X + width_Triangle_Exterior_Float * (1 - scalingFactor_Float) * 2f, 
                        viewport_RectangleF.Y
                    ),
                    size_Triangle_Exterior_Float
                ),
                triangle_Exterior_Middle_RectangleF = new RectangleF
                (
                    new Vector2
                    (
                        triangle_Exterior_Left_RectangleF.X + width_Triangle_Exterior_Float - width_Triangle_Exterior_Float * (1 - scalingFactor_Float), 
                        viewport_RectangleF.Y
                        ),
                    size_Triangle_Exterior_Float
                ),
                triangle_Exterior_Right_RectangleF = new RectangleF
                (
                    new Vector2
                    (
                        triangle_Exterior_Middle_RectangleF.X + width_Triangle_Exterior_Float - width_Triangle_Exterior_Float * (1 - scalingFactor_Float), 
                        viewport_RectangleF.Y
                    ),
                    size_Triangle_Exterior_Float
                ),


                triangle_Inner_Left_RectangleF = ScalingViewport
                (
                    triangle_Exterior_Left_RectangleF,
                    scalingFactor_Float
                ),
                triangle_Inner_Middle_RectangleF = ScalingViewport
                (
                    triangle_Exterior_Middle_RectangleF,
                    scalingFactor_Float
                ),
                triangle_Inner_Right_RectangleF = ScalingViewport
                (
                    triangle_Exterior_Right_RectangleF,
                    scalingFactor_Float
                );



            DrawIcon
            (
                ref frame,
                "Triangle",
                triangle_Exterior_Left_RectangleF.Center.X, 
                triangle_Exterior_Left_RectangleF.Center.Y, 
                triangle_Exterior_Left_RectangleF.Height * scalingFactor_Float, 
                triangle_Exterior_Left_RectangleF.Width, 
                border_Color, 
                90f
            );
            DrawIcon
            (
                ref frame,
                "Triangle",
                triangle_Exterior_Middle_RectangleF.Center.X,
                triangle_Exterior_Middle_RectangleF.Center.Y,
                triangle_Exterior_Middle_RectangleF.Height * scalingFactor_Float,
                triangle_Exterior_Middle_RectangleF.Width, 
                border_Color, 
                90f
            );
            DrawIcon
            (
                ref frame,
                "Triangle",
                triangle_Exterior_Right_RectangleF.Center.X,
                triangle_Exterior_Right_RectangleF.Center.Y,
                triangle_Exterior_Right_RectangleF.Height * scalingFactor_Float,
                triangle_Exterior_Right_RectangleF.Width, 
                border_Color, 
                90f
            );



            string refreshRate_String = GetValue_from_CustomData(information_Section, refreshRate_Key);

            if (refreshRate_String == "FFF")
            {
                return;
            }
            else if (refreshRate_String == "FF")
            {
                DrawIcon
                (
                    ref frame,
                    "Triangle",
                    triangle_Inner_Right_RectangleF.Center.X,
                    triangle_Inner_Right_RectangleF.Center.Y,
                    triangle_Inner_Right_RectangleF.Height * scalingFactor_Float,
                    triangle_Inner_Right_RectangleF.Width,
                    background_Color,
                    90f
                );
            }
            else if (refreshRate_String == "F")
            {
                DrawIcon
                (
                    ref frame,
                    "Triangle",
                    triangle_Inner_Right_RectangleF.Center.X,
                    triangle_Inner_Right_RectangleF.Center.Y,
                    triangle_Inner_Right_RectangleF.Height * scalingFactor_Float,
                    triangle_Inner_Right_RectangleF.Width,
                    background_Color,
                    90f
                );
                DrawIcon
                (
                    ref frame,
                    "Triangle",
                    triangle_Inner_Middle_RectangleF.Center.X,
                    triangle_Inner_Middle_RectangleF.Center.Y,
                    triangle_Inner_Middle_RectangleF.Height * scalingFactor_Float,
                    triangle_Inner_Middle_RectangleF.Width,
                    background_Color,
                    90f
                );
            }
            else
            {
                DrawIcon
                (
                    ref frame,
                    "Triangle",
                    triangle_Inner_Right_RectangleF.Center.X,
                    triangle_Inner_Right_RectangleF.Center.Y,
                    triangle_Inner_Right_RectangleF.Height * scalingFactor_Float,
                    triangle_Inner_Right_RectangleF.Width,
                    background_Color,
                    90f
                );
                DrawIcon
                (
                    ref frame,
                    "Triangle",
                    triangle_Inner_Middle_RectangleF.Center.X,
                    triangle_Inner_Middle_RectangleF.Center.Y,
                    triangle_Inner_Middle_RectangleF.Height * scalingFactor_Float,
                    triangle_Inner_Middle_RectangleF.Width,
                    background_Color,
                    90f
                );
                DrawIcon
                (
                    ref frame,
                    "Triangle",
                    triangle_Inner_Right_RectangleF.Center.X,
                    triangle_Inner_Right_RectangleF.Center.Y,
                    triangle_Inner_Right_RectangleF.Height * scalingFactor_Float,
                    triangle_Inner_Right_RectangleF.Width,
                    background_Color,
                    90f
                );
            }

        }
        /*###############     Overall     ###############*/
        /*###############################################*/


        /*#################################################*/
        /*###############     ShowItems     ###############*/

        public void ShowItems(string nextStage)
        {
            const int counter_TotalCycle_Int = 7;

            if (!function_ShowItems_Bool)
            {
                stage_Key = nextStage;
                counter_ShowItems_Int = 1;
                return;
            }

            Echo($"{counter_ShowItems_Int}/{counter_TotalCycle_Int}");
            switch (counter_ShowItems_Int)
            {
                case 1:
                    Echo("GetItems");
                    GetAllItems();
                    break;
                case 2:
                    Echo("AllItems");
                    ItemDivideInGroups(itemList_All, panels_Items_All);
                    break;
                case 3:
                    Echo("Ore");
                    ItemDivideInGroups(itemList_Ore, panels_Items_Ore);
                    break;
                case 4:
                    Echo("Ingot");
                    ItemDivideInGroups(itemList_Ingot, panels_Items_Ingot);
                    break;
                case 5:
                    Echo("Component");
                    ItemDivideInGroups(itemList_Component, panels_Items_Component);
                    break;
                case 6:
                    Echo("AmmoMagazine");
                    ItemDivideInGroups(itemList_AmmoMagazine, panels_Items_AmmoMagazine);
                    break;
                case 7:
                    Echo("DrawItemPanels");
                    DrawItemPanels();
                    break;
            }

            if (counter_ShowItems_Int >= counter_TotalCycle_Int)
            {
                stage_Key = nextStage;
                counter_ShowItems_Int = 1;
                return;
            }
            counter_ShowItems_Int++;
        }

        public void Build_TranslateDic()
        {
            string value = GetValue_from_CustomData(translateList_Section, length_Key);
            int length = Convert.ToInt16(value);

            for (int i = 1; i <= length; i++)
            {
                value = GetValue_from_CustomData(translateList_Section, i.ToString());
                string[] result = value.Split(':');

                translator.Add(result[0], result[1]);
            }
        }

        public void GetAllItems()
        {
            time1_DateTime = time2_DateTime;
            if (time2_DateTime == null) time1_DateTime = DateTime.Now;
            time2_DateTime = DateTime.Now;

            Dictionary<string, double> allItems_Dic = new Dictionary<string, double>();
            panel_UI_Info_Dic = new Dictionary<string, MyIni>();

            foreach (var cargoContainer in cargoContainers)
            {
                var items = new List<MyInventoryItem>();
                cargoContainer.GetInventory().GetItems(items);

                foreach (var item in items)
                {
                    if (allItems_Dic.ContainsKey(item.Type.ToString())) allItems_Dic[item.Type.ToString()] += (double)item.Amount.RawValue;
                    else allItems_Dic.Add(item.Type.ToString(), (double)item.Amount.RawValue);
                }
            }

            foreach (var cargoContainer in oxygenTanks)
            {
                var items = new List<MyInventoryItem>();
                cargoContainer.GetInventory().GetItems(items);

                foreach (var item in items)
                {
                    if (allItems_Dic.ContainsKey(item.Type.ToString())) allItems_Dic[item.Type.ToString()] += (double)item.Amount.RawValue;
                    else allItems_Dic.Add(item.Type.ToString(), (double)item.Amount.RawValue);
                }
            }

            foreach (var cargoContainer in hydrogenTanks)
            {
                var items = new List<MyInventoryItem>();
                cargoContainer.GetInventory().GetItems(items);

                foreach (var item in items)
                {
                    if (allItems_Dic.ContainsKey(item.Type.ToString())) allItems_Dic[item.Type.ToString()] += (double)item.Amount.RawValue;
                    else allItems_Dic.Add(item.Type.ToString(), (double)item.Amount.RawValue);
                }
            }

            foreach (var reactor in reactors)
            {
                var items = new List<MyInventoryItem>();
                reactor.GetInventory().GetItems(items);

                foreach (var item in items)
                {
                    if (allItems_Dic.ContainsKey(item.Type.ToString())) allItems_Dic[item.Type.ToString()] += (double)item.Amount.RawValue;
                    else allItems_Dic.Add(item.Type.ToString(), (double)item.Amount.RawValue);
                }
            }

            foreach (var cargoContainer in assemblers)
            {
                var items = new List<MyInventoryItem>();
                cargoContainer.OutputInventory.GetItems(items);

                foreach (var item in items)
                {
                    if (allItems_Dic.ContainsKey(item.Type.ToString())) allItems_Dic[item.Type.ToString()] += (double)item.Amount.RawValue;
                    else allItems_Dic.Add(item.Type.ToString(), (double)item.Amount.RawValue);
                }
            }

            foreach (var cargoContainer in refineries)
            {
                var items = new List<MyInventoryItem>();
                cargoContainer.InputInventory.GetItems(items);

                foreach (var item in items)
                {
                    if (allItems_Dic.ContainsKey(item.Type.ToString())) allItems_Dic[item.Type.ToString()] += (double)item.Amount.RawValue;
                    else allItems_Dic.Add(item.Type.ToString(), (double)item.Amount.RawValue);
                }
            }

            foreach (var gasGenerator in gasGenerators)
            {
                var items = new List<MyInventoryItem>();
                gasGenerator.GetInventory().GetItems(items);

                foreach (var item in items)
                {
                    if (allItems_Dic.ContainsKey(item.Type.ToString())) allItems_Dic[item.Type.ToString()] += (double)item.Amount.RawValue;
                    else allItems_Dic.Add(item.Type.ToString(), (double)item.Amount.RawValue);
                }

            }


            itemList_All = new ItemList[allItems_Dic.Count];

            int k = 0;
            foreach (var key in allItems_Dic.Keys)
            {
                itemList_All[k].Name = key;
                itemList_All[k].Amount2 = allItems_Dic[key];
                itemList_All[k].Time1 = time1_DateTime;
                itemList_All[k].Time2 = time2_DateTime;
                if (allItems_Old_Dic.ContainsKey(key)) itemList_All[k].Amount1 = allItems_Old_Dic[key];
                else itemList_All[k].Amount1 = 0;
                foreach(var item in productionList)
                {
                    if (item.ComponentName == key) itemList_All[k].ProductionAmount = item.ProductionAmount * 1000000;
                }
                k++;
            }
            allItems_Old_Dic.Clear();
            allItems_Old_Dic = allItems_Dic;


            itemList_Ore = new ItemList[LengthOfEachCategory("MyObjectBuilder_Ore")];
            itemList_Ingot = new ItemList[LengthOfEachCategory("MyObjectBuilder_Ingot")];
            itemList_AmmoMagazine = new ItemList[LengthOfEachCategory("MyObjectBuilder_AmmoMagazine")];

            TransferItemsList(itemList_Ore, "MyObjectBuilder_Ore");
            TransferItemsList(itemList_Ingot, "MyObjectBuilder_Ingot");
            TransferItemsList(itemList_AmmoMagazine, "MyObjectBuilder_AmmoMagazine");

            itemList_Component = new ItemList[itemList_All.Length - itemList_Ore.Length - itemList_Ingot.Length - itemList_AmmoMagazine.Length];

            k = 0;
            foreach (var item in itemList_All)
            {
                if (item.Name.IndexOf("MyObjectBuilder_Ore") == -1 && item.Name.IndexOf("MyObjectBuilder_Ingot") == -1 && item.Name.IndexOf("MyObjectBuilder_AmmoMagazine") == -1)
                {
                    itemList_Component[k] = item;
                    k++;
                }
            }
        }

        public int LengthOfEachCategory(string tag)
        {
            Dictionary<string, double> keyValuePairs = new Dictionary<string, double>();

            foreach (var item in itemList_All)
            {
                if (item.Name.IndexOf(tag) != -1)
                {
                    keyValuePairs.Add(item.Name, item.Amount1);
                }
            }

            return keyValuePairs.Count;
        }

        public void TransferItemsList(ItemList[] itemList, string tag)
        {
            int k = 0;
            foreach (var item in itemList_All)
            {
                if (item.Name.IndexOf(tag) != -1)
                {
                    itemList[k] = item;
                    k++;
                }
            }
        }

        public void ItemDivideInGroups(ItemList[] itemList, List<IMyTextPanel> panels_Items)
        {
            if (panels_Items.Count == 0) return;

            //  get all panel numbers
            int[] index_Array = new int[panels_Items.Count];
            int k = 0;
            foreach (var panel in panels_Items)
            {
                //  get current panel number
                string[] arry = panel.CustomName.Split(':');
                index_Array[k] = Convert.ToInt16(arry[1]);
                k++;
            }

            if (itemList.Length > FindMax(index_Array) * itemAmountInEachScreen_Int)
            {
                foreach (var panel in panels_Items)
                {
                    if (panel_UI_Info_Dic.ContainsKey(panel.CustomName)) continue;
                    string[] arry = panel.CustomName.Split(':');
                    if (Convert.ToInt16(arry[1]) < FindMax(index_Array))
                    {
                        WriteSinglePanelInfo(panel, arry[1], true, itemList);
                    }
                    else
                    {
                        WriteSinglePanelInfo(panel, arry[1], false, itemList);
                    }
                }
            }
            else
            {
                foreach (var panel in panels_Items)
                {
                    if (panel_UI_Info_Dic.ContainsKey(panel.CustomName)) continue;
                    string[] arry = panel.CustomName.Split(':');
                    WriteSinglePanelInfo(panel, arry[1], true, itemList);
                }
            }
        }

        public int FindMax(int[] arry)
        {
            int p = 0;
            for (int i = 0; i < arry.Length; i++)
            {
                if (i == 0) p = arry[i];
                else if (arry[i] > p) p = arry[i];
            }

            return p;
        }

        public void WriteSinglePanelInfo(IMyTextPanel panel, string groupNumber, bool isEnoughScreen, ItemList[] itemList)
        {
            panel.WriteText("", false);

            MyIni ini_Temp = new MyIni();

            for (int i = 0; i < itemAmountInEachScreen_Int; i++)
            {
                int itemIndex_Int = (Convert.ToInt16(groupNumber) - 1) * itemAmountInEachScreen_Int + i;
                int x = (i + 1) % 7;
                if (x == 0) x = 7;
                int y = Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(i + 1) / 7)));

                if (itemIndex_Int > itemList.Length - 1)
                {
                    ini_Temp.Set(panelInformation_Section, Amount_Key, i.ToString());
                    break;
                }
                else
                {
                    if (x == 7 && y == 4)
                    {
                        if (isEnoughScreen)
                        {
                            WriteSingleItemInfo(ref ini_Temp, i + 1, itemList[itemIndex_Int]);
                        }
                        else
                        {
                            double residus = itemList.Length - itemAmountInEachScreen_Int * Convert.ToInt16(groupNumber) + 1;
                            WriteTheLastItemInfo(ref ini_Temp, i + 1, residus);
                        }
                    }
                    else
                    {
                        WriteSingleItemInfo(ref ini_Temp, i + 1, itemList[itemIndex_Int]);
                    }

                    ini_Temp.Set(panelInformation_Section, Amount_Key, (i + 1).ToString());

                    panel.WriteText(itemList[itemIndex_Int].Name, true);
                    panel.WriteText("\n", true);

                }
            }

            panel_UI_Info_Dic.Add(panel.CustomName, ini_Temp);


        }
        
        public void WriteSingleItemInfo(ref MyIni panelUI_Info_Ini, int index_Int, ItemList item_IL)
        {
            string itemType_String = item_IL.Name;
            double amount1_Double = item_IL.Amount1;
            double amount2_Double = item_IL.Amount2;
            DateTime time1_DT = item_IL.Time1;
            DateTime time2_DT = item_IL.Time2;
            double productionAmount_Double = item_IL.ProductionAmount;

            double amountDifference_Double = amount1_Double - amount2_Double;
            TimeSpan timeDifference_TimeSpan = time2_DT - time1_DT;

            long timeRemaining_Long = 0;
            if (timeDifference_TimeSpan.Ticks > 0 && amountDifference_Double > 0)
            {
                double efficiency_Double = amountDifference_Double / timeDifference_TimeSpan.Ticks;
                if (Int64.MaxValue > amount2_Double / efficiency_Double)
                {
                    timeRemaining_Long = Convert.ToInt64(amount2_Double / efficiency_Double);
                }
            }
            TimeSpan timeRemaining_TimeSpan = new TimeSpan(timeRemaining_Long);

            string time_String = "";
            if (timeRemaining_TimeSpan.Days == 0 && timeRemaining_Long != 0)
            {
                time_String = timeRemaining_TimeSpan.Hours.ToString() + ":" + timeRemaining_TimeSpan.Minutes.ToString() + ":" + timeRemaining_TimeSpan.Seconds.ToString();
            }
            else if (timeRemaining_TimeSpan.Days > 0 && timeRemaining_Long != 0)
            {
                time_String = timeRemaining_TimeSpan.Days.ToString() + "d" + timeRemaining_TimeSpan.Hours.ToString() + ":" + timeRemaining_TimeSpan.Minutes.ToString();
            }

            panelUI_Info_Ini.Set(index_Int.ToString(), itemType_Key, itemType_String);
            panelUI_Info_Ini.Set(index_Int.ToString(), itemAmount1_Key, amount1_Double.ToString());
            panelUI_Info_Ini.Set(index_Int.ToString(), itemAmount2_Key, amount2_Double.ToString());
            panelUI_Info_Ini.Set(index_Int.ToString(), time_Key, time_String);
            panelUI_Info_Ini.Set(index_Int.ToString(), time1_Key, time1_DT.ToString());
            panelUI_Info_Ini.Set(index_Int.ToString(), time2_Key, time2_DT.ToString());
            panelUI_Info_Ini.Set(index_Int.ToString(), productionAmount_Key, productionAmount_Double.ToString());

        }

        public void WriteTheLastItemInfo(ref MyIni panelUI_Info_Ini, int index_Int, double residue_Double)
        {
            panelUI_Info_Ini.Set(index_Int.ToString(), itemType_Key, "AH_BoreSight");
            panelUI_Info_Ini.Set(index_Int.ToString(), itemAmount2_Key, residue_Double.ToString());
            panelUI_Info_Ini.Set(index_Int.ToString(), time_Key, "");
            panelUI_Info_Ini.Set(index_Int.ToString(), productionAmount_Key, "0");
        }

        public void DrawItemPanels()
        {
            foreach (var panel in panels_Items_All) DrawSinglePanel(panel, progressbar_Color, card_Background_Color_Overall);
            foreach (var panel in panels_Items_Ore) DrawSinglePanel(panel, oreCard_Background_Color, ore_Background_Color);
            foreach (var panel in panels_Items_Ingot) DrawSinglePanel(panel, ingotCard_Background_Color, ingot_Background_Color);
            foreach (var panel in panels_Items_Component) DrawSinglePanel(panel, progressbar_Color, card_Background_Color_Overall);
            foreach (var panel in panels_Items_AmmoMagazine) DrawSinglePanel(panel, ammoCard_Background_Color, ammo_Background_Color);
        }

        public void DrawSinglePanel(IMyTextPanel panel, Color cardColor, Color backgroundColor)
        {
            panel.ContentType = ContentType.SCRIPT;
            panel.ScriptBackgroundColor = card_Background_Color_Overall;

            MySpriteDrawFrame frame = panel.DrawFrame();

            string refreshCounter_String = GetValue_from_CustomData(panel, panelInformation_Section, counter_Key);
            int indexMax_Int = Convert.ToInt16(panel_UI_Info_Dic[panel.CustomName].Get(panelInformation_Section, Amount_Key).ToString());

            if (refreshCounter_String == null || refreshCounter_String != "0")
            {
                WriteValue_to_CustomData(panel, panelInformation_Section, counter_Key, "0");
                refreshCounter_String = "0";
            }
            else WriteValue_to_CustomData(panel, panelInformation_Section, counter_Key, "1");

            float refreshCounter_Float = Convert.ToInt16(refreshCounter_String);

            RectangleF visibleArea_RectangleF = new RectangleF
                (
                    (panel.TextureSize - panel.SurfaceSize) / 2f + new Vector2(0, refreshCounter_Float),
                    panel.SurfaceSize
                );

            float sideLength_Float;

            if (visibleArea_RectangleF.Width <= visibleArea_RectangleF.Height) sideLength_Float = visibleArea_RectangleF.Width;
            else sideLength_Float = visibleArea_RectangleF.Height;

            float scalingFactor_Float = sideLength_Float / 512f;

            RectangleF viewport_RectangleF = new RectangleF
                (
                    new Vector2
                    (
                        visibleArea_RectangleF.Center.X - sideLength_Float / 2,
                        visibleArea_RectangleF.Center.Y - sideLength_Float / 2
                    ),
                    new Vector2(sideLength_Float, sideLength_Float)
                );
            DrawBox(ref frame, viewport_RectangleF, backgroundColor);


            for (int itemIndex_Int = 0; itemIndex_Int < itemAmountInEachScreen_Int; itemIndex_Int++)
            {
                int x = (itemIndex_Int + 1) % itemBox_ColumnNumbers_Int;
                if (x == 0) x = itemBox_ColumnNumbers_Int;
                int y = Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(itemIndex_Int + 1) / itemBox_ColumnNumbers_Int)));

                RectangleF card_Range_RectangleF = new RectangleF
                    (
                        viewport_RectangleF.Position +
                        new Vector2
                            (
                                viewport_RectangleF.Width / itemBox_ColumnNumbers_Int * Convert.ToSingle(x - 1),
                                viewport_RectangleF.Height / itemBox_RowNumbers_Int * Convert.ToSingle(y - 1)
                            ),
                        new Vector2
                            (
                                viewport_RectangleF.Width / itemBox_ColumnNumbers_Int, 
                                viewport_RectangleF.Height / itemBox_RowNumbers_Int
                            )
                    );

                if (itemIndex_Int > indexMax_Int - 1) break;
                else DrawSingleItemUnit(panel, ref frame, itemIndex_Int + 1, card_Range_RectangleF, scalingFactor_Float, cardColor);
            }

            frame.Dispose();
        }

        public void DrawSingleItemUnit(IMyTextPanel panel, ref MySpriteDrawFrame frame, int index_Int, RectangleF viewport_RectangleF, float scalingFactor_Float, Color cardColor)
        {
            MyIni panelUI_Ini = panel_UI_Info_Dic[panel.CustomName];

            string itemName_String = panelUI_Ini.Get(index_Int.ToString(), itemType_Key).ToString();
            double amount_Double = Convert.ToDouble(panelUI_Ini.Get(index_Int.ToString(), itemAmount2_Key).ToString());
            string time_String = panelUI_Ini.Get(index_Int.ToString(), time_Key).ToString();
            double amount_Production_Double = Convert.ToDouble(panelUI_Ini.Get(index_Int.ToString(), productionAmount_Key).ToString());

            //  Main box
            RectangleF card_BackGround_RectangleF = ScalingViewport(viewport_RectangleF, 0.96f, 2);
            DrawBox(ref frame, card_BackGround_RectangleF, cardColor);

            //  Name text
            RectangleF text_Name_RectangleF = ScalingViewport(card_BackGround_RectangleF, 0.95f, 2);
            PanelWriteText(ref frame, TranslateName(itemName_String), text_Name_RectangleF, 0.55f * scalingFactor_Float, font_Color_Overall);

            //  Picture box
            RectangleF
                picture_RectangleF = new RectangleF
                (
                    card_BackGround_RectangleF.Position + new Vector2(0, card_BackGround_RectangleF.Height * 0.12f),
                    new Vector2(card_BackGround_RectangleF.Width, card_BackGround_RectangleF.Width)
                );
            DrawIcon(ref frame, itemName_String, picture_RectangleF, font_Color_Overall);

            //  Amount text
            RectangleF
                text_Amount_RectangleF = new RectangleF
                (
                    picture_RectangleF.Position + new Vector2(0, picture_RectangleF.Height * 0.96f),
                    new Vector2(card_BackGround_RectangleF.Width, card_BackGround_RectangleF.Height * 0.2f)
                ),
                text_Amount_Content_RectangleF = ScalingViewport(text_Amount_RectangleF, 0.93f, 2);
            PanelWriteText
                (
                    ref frame,
                    AmountUnitConversion(amount_Double / 1000000, false),
                    text_Amount_Content_RectangleF,
                    0.8f * scalingFactor_Float,
                    font_Color_Overall,
                    TextAlignment.RIGHT
                );

            //  AutoProductionAmount text
            RectangleF
                text_AutoProduction_RectangleF = new RectangleF
                (
                    new Vector2(picture_RectangleF.X, picture_RectangleF.Bottom - card_BackGround_RectangleF.Height * 0.1f),
                    new Vector2(card_BackGround_RectangleF.Width, card_BackGround_RectangleF.Height * 0.11f)
                ),
                text_AutoProduction_Content_RectangleF = ScalingViewport(text_AutoProduction_RectangleF, 0.96f, 2);
            if (amount_Production_Double != 0)
            {
                PanelWriteText
                (
                    ref frame,
                    AmountUnitConversion(amount_Production_Double / 1000000, false),
                    text_AutoProduction_Content_RectangleF,
                    0.5f * scalingFactor_Float,
                    font_Color_Overall,
                    TextAlignment.RIGHT
                );
            }

            //  Time remaining
            RectangleF
                text_Time_RectangleF = new RectangleF
                (
                    new Vector2(card_BackGround_RectangleF.X, card_BackGround_RectangleF.Bottom - card_BackGround_RectangleF.Height * 0.13f),
                    new Vector2(card_BackGround_RectangleF.Width, card_BackGround_RectangleF.Height * 0.14f)
                );
            PanelWriteText
            (
                ref frame,
                time_String,
                text_Time_RectangleF,
                0.57f * scalingFactor_Float,
                font_Color_Overall,
                TextAlignment.RIGHT
            );


        }

        public string ShortName(string name)
        {
            string[] temp = name.Split('/');

            if (temp.Length == 2)
            {
                return temp[1];
            }
            else
            {
                return name;
            }
        }

        public string TranslateName(string name)
        {
            if (translator.ContainsKey(name))
            {
                return translator[name];
            }
            else
            {
                return ShortName(name);
            }
        }

        /*###############     ShowItems     ###############*/
        /*#################################################*/

        /*###########################################################*/
        /*###############   Refinery_And_Assembler    ###############*/

        public void ShowFacilities(string nextStage)
        {
            if (!function_ShowFacilities_Bool)
            {
                stage_Key = nextStage;
                return;
            }

            Echo($"{counter_ShowFacilities_Int}/{panels_Refineries.Count + panels_Assemblers.Count + 2}");


            if (counter_ShowFacilities_Int == 1)
            {
                Echo("GetFacilities");
                GetFacilities();
                maxNumber_RefineryPanel_Int = GetMaxNumber(refineryList, panels_Refineries);
                maxNumber_AssemblerPanel_Int = GetMaxNumber(assemblerList, panels_Assemblers);
                counter_ShowFacilities_Int++;
                return;
            }
            else if (counter_ShowFacilities_Int == 2)
            {
                Echo("MaxPanelNumber");
                maxNumber_RefineryPanel_Int = GetMaxNumber(refineryList, panels_Refineries);
                maxNumber_AssemblerPanel_Int = GetMaxNumber(assemblerList, panels_Assemblers);
                counter_ShowFacilities_Int++;
                return;
            }


            if (counter_ShowFacilities_Int <= panels_Refineries.Count + 2)
            {
                Echo("Ref");
                counter_Panel_Int = counter_ShowFacilities_Int - 3;
                if (refineries.Count > 0) FacilitiesDivideIntoGroup(refineryList, panels_Refineries, maxNumber_RefineryPanel_Int, oreCard_Background_Color, ore_Background_Color);
            }
            else
            {
                Echo("Ass");
                counter_Panel_Int = counter_ShowFacilities_Int - panels_Refineries.Count - 3;
                if (assemblers.Count > 0) FacilitiesDivideIntoGroup(assemblerList, panels_Assemblers, maxNumber_AssemblerPanel_Int, ingotCard_Background_Color, ingot_Background_Color);
            }


            if (counter_ShowFacilities_Int >= panels_Refineries.Count + panels_Assemblers.Count + 2)
            {
                counter_ShowFacilities_Int = 1;
                stage_Key = nextStage;
                return;
            }
            counter_ShowFacilities_Int++;

        }

        public void GetFacilities()
        {

            refineryList = new Facility_Struct[refineries.Count];

            int k = 0;
            foreach (var refinery in refineries)
            {
                refineryList[k].Name = refinery.CustomName;
                refineryList[k].IsEnabled_Bool = refinery.Enabled;
                refineryList[k].IsProducing_Bool = refinery.IsProducing;
                if (GetValue_from_CustomData(refinery, ore_Section, combinedMode_Key) == "1") refineryList[k].IsCooperativeMode_Bool = true;
                else refineryList[k].IsCooperativeMode_Bool = false;
                refineryList[k].IsRepeatMode_Bool = false;


                List<MyInventoryItem> items = new List<MyInventoryItem>();
                refinery.InputInventory.GetItems(items);
                if (items.Count == 0)
                {
                    refineryList[k].Picture = "Empty";
                    refineryList[k].ItemAmount = 0;
                }
                else
                {
                    refineryList[k].Picture = items[0].Type.ToString();
                    refineryList[k].ItemAmount = (double)items[0].Amount;
                }

                char[] delimiterChars = { ':', '：' };
                string[] str1 = refinery.DetailedInfo.Split('%');
                string[] str2 = str1[0].Split(delimiterChars);
                refineryList[k].Productivity = str2[str2.Length - 1];

                k++;
            }

            assemblerList = new Facility_Struct[assemblers.Count];

            k = 0;
            foreach (var assembler in assemblers)
            {
                assemblerList[k].Name = assembler.CustomName;
                assemblerList[k].IsEnabled_Bool = assembler.Enabled;
                assemblerList[k].IsProducing_Bool = assembler.IsProducing;
                assemblerList[k].IsCooperativeMode_Bool = assembler.CooperativeMode;
                assemblerList[k].IsRepeatMode_Bool = assembler.Repeating;


                List<MyProductionItem> items = new List<MyProductionItem>();
                assembler.GetQueue(items);
                if (items.Count == 0)
                {
                    assemblerList[k].Picture = "Empty";
                    assemblerList[k].ItemAmount = 0;
                }
                else
                {
                    assemblerList[k].Picture = items[0].BlueprintId.ToString();
                    assemblerList[k].ItemAmount = (double)items[0].Amount;
                }


                char[] delimiterChars = { ':', '：' };
                string[] str1 = assembler.DetailedInfo.Split('%');
                string[] str2 = str1[0].Split(delimiterChars);
                assemblerList[k].Productivity = str2[str2.Length - 1];

                k++;
            }
        }

        public int GetMaxNumber(Facility_Struct[] facilityList, List<IMyTextPanel> facilityPanels)
        {
            if (facilityList.Length == 0 || facilityPanels.Count == 0) return 0;

            //  get all panel numbers
            int[] findMax = new int[facilityPanels.Count];
            int k = 0;
            foreach (var panel in facilityPanels)
            {
                //  get current panel number
                string[] arry = panel.CustomName.Split(':');
                findMax[k] = Convert.ToInt16(arry[1]);
                k++;
            }

            return FindMax(findMax);
        }

        public void FacilitiesDivideIntoGroup(Facility_Struct[] facilityList, List<IMyTextPanel> facilityPanels, int maxNumber_Panel_Int, Color card_Color, Color background_Color)
        {
            if (facilityList.Length == 0 || facilityPanels.Count == 0) return;

            Echo($"{counter_Panel_Int + 1}/{facilityPanels.Count}");

            if (facilityList.Length > maxNumber_Panel_Int * facilityAmountInEachScreen_Int)
            {
                //  Not enough panel
                var panel = facilityPanels[counter_Panel_Int];

                if (panel.CustomData != "0") panel.CustomData = "0";
                else panel.CustomData = "1";
                float refreshCounter_Float = Convert.ToSingle(panel.CustomData);

                if (panel.ContentType != ContentType.SCRIPT) panel.ContentType = ContentType.SCRIPT;
                panel.ScriptBackgroundColor = card_Background_Color_Overall;

                RectangleF visibleArea_RectangleF = new RectangleF
                (
                    (panel.TextureSize - panel.SurfaceSize) / 2f + new Vector2(0, refreshCounter_Float),
                    panel.SurfaceSize
                );

                float sideLength_Float;

                if (visibleArea_RectangleF.Width <= visibleArea_RectangleF.Height) sideLength_Float = visibleArea_RectangleF.Width;
                else sideLength_Float = visibleArea_RectangleF.Height;

                float scalingFactor_Float = sideLength_Float / 512f;

                RectangleF viewport_RectangleF = new RectangleF
                    (
                        new Vector2
                        (
                            visibleArea_RectangleF.Center.X - sideLength_Float / 2,
                            visibleArea_RectangleF.Center.Y - sideLength_Float / 2
                        ),
                        new Vector2(sideLength_Float, sideLength_Float)
                    );

                MySpriteDrawFrame frame = panel.DrawFrame();

                DrawBox(ref frame, viewport_RectangleF, background_Color);

                string[] arry = panel.CustomName.Split(':');
                if (Convert.ToInt16(arry[1]) < maxNumber_Panel_Int)
                {
                    DrawFullFacilityScreen(panel, ref frame, viewport_RectangleF, arry[1], true, facilityList, background_Color, card_Color);
                }
                else
                {
                    DrawFullFacilityScreen(panel, ref frame, viewport_RectangleF, arry[1], false, facilityList, background_Color, card_Color);
                }
                frame.Dispose();

                Echo(panel.CustomName);
            }
            else
            {
                //  Enough panel
                var panel = facilityPanels[counter_Panel_Int];

                if (panel.CustomData != "0") panel.CustomData = "0";
                else panel.CustomData = "1";
                float refreshCounter_Float = Convert.ToSingle(panel.CustomData);

                if (panel.ContentType != ContentType.SCRIPT) panel.ContentType = ContentType.SCRIPT;
                panel.ScriptBackgroundColor = card_Background_Color_Overall;

                RectangleF visibleArea_RectangleF = new RectangleF
                (
                    (panel.TextureSize - panel.SurfaceSize) / 2f + new Vector2(0, refreshCounter_Float),
                    panel.SurfaceSize
                );

                float sideLength_Float;

                if (visibleArea_RectangleF.Width <= visibleArea_RectangleF.Height) sideLength_Float = visibleArea_RectangleF.Width;
                else sideLength_Float = visibleArea_RectangleF.Height;

                float scalingFactor_Float = sideLength_Float / 512f;

                RectangleF viewport_RectangleF = new RectangleF
                    (
                        new Vector2
                        (
                            visibleArea_RectangleF.Center.X - sideLength_Float / 2,
                            visibleArea_RectangleF.Center.Y - sideLength_Float / 2
                        ),
                        new Vector2(sideLength_Float, sideLength_Float)
                    );

                MySpriteDrawFrame frame = panel.DrawFrame();

                DrawBox(ref frame, viewport_RectangleF, background_Color);

                string[] arry = panel.CustomName.Split(':');
                DrawFullFacilityScreen(panel, ref frame, viewport_RectangleF, arry[1], true, facilityList, background_Color, card_Color);
                frame.Dispose();

                Echo(panel.CustomName);
            }
        }

        public void DrawFullFacilityScreen(IMyTextPanel panel, ref MySpriteDrawFrame frame, RectangleF viewport_RectangleF, string groupNumber, bool isEnoughScreen, Facility_Struct[] facilityList, Color background_Color, Color card_Color)
        {
            panel.WriteText("", false);

            float height_SingleUnit_Float = viewport_RectangleF.Height / facilityAmountInEachScreen_Int;

            for (int facility_Index_Local_Int = 0; facility_Index_Local_Int < facilityAmountInEachScreen_Int; facility_Index_Local_Int++)
            {
                int facility_Index_WholeList_Int = (Convert.ToInt16(groupNumber) - 1) * facilityAmountInEachScreen_Int + facility_Index_Local_Int;

                if (facility_Index_WholeList_Int > facilityList.Length - 1) return;//Last facility is finished.

                RectangleF singleUnit_RectangleF = new RectangleF
                (
                    viewport_RectangleF.Position + new Vector2(0, height_SingleUnit_Float * facility_Index_Local_Int),
                    new Vector2(viewport_RectangleF.Width, height_SingleUnit_Float)
                );

                if (facility_Index_Local_Int == facilityAmountInEachScreen_Int - 1)
                {
                    if (isEnoughScreen)
                    {
                        DrawSingleFacilityUnit
                        (
                            panel,
                            ref frame, 
                            singleUnit_RectangleF, 
                            (facility_Index_WholeList_Int + 1).ToString() + ". " + 
                            facilityList[facility_Index_WholeList_Int].Name + " ×" + 
                            facilityList[facility_Index_WholeList_Int].Productivity + "%", 
                            facilityList[facility_Index_WholeList_Int].IsProducing_Bool, 
                            AmountUnitConversion(facilityList[facility_Index_WholeList_Int].ItemAmount, false), 
                            facilityList[facility_Index_WholeList_Int].Picture, 
                            facilityList[facility_Index_WholeList_Int].IsRepeatMode_Bool, 
                            facilityList[facility_Index_WholeList_Int].IsCooperativeMode_Bool, 
                            facilityList[facility_Index_WholeList_Int].IsEnabled_Bool, 
                            facility_Index_Local_Int,
                            card_Color
                        );
                    }
                    else
                    {
                        double residus = facilityList.Length - facilityAmountInEachScreen_Int * Convert.ToInt16(groupNumber) + 1;
                        DrawSingleFacilityUnit
                        (
                            panel, 
                            ref frame, 
                            singleUnit_RectangleF, 
                            "+ " + residus.ToString() + " Facilities",
                            false, 
                            "0",
                            "Empty", 
                            false,
                            false,
                            false,
                            facility_Index_Local_Int,
                            card_Color
                            
                        );
                    }
                }
                else
                {
                    DrawSingleFacilityUnit
                    (
                        panel, 
                        ref frame,
                        singleUnit_RectangleF, 
                        (facility_Index_WholeList_Int + 1).ToString() + ". " + 
                        facilityList[facility_Index_WholeList_Int].Name + " ×" + 
                        facilityList[facility_Index_WholeList_Int].Productivity + "%", 
                        facilityList[facility_Index_WholeList_Int].IsProducing_Bool, 
                        AmountUnitConversion(facilityList[facility_Index_WholeList_Int].ItemAmount, false), 
                        facilityList[facility_Index_WholeList_Int].Picture,
                        facilityList[facility_Index_WholeList_Int].IsRepeatMode_Bool, 
                        facilityList[facility_Index_WholeList_Int].IsCooperativeMode_Bool, 
                        facilityList[facility_Index_WholeList_Int].IsEnabled_Bool, 
                        facility_Index_Local_Int,
                        card_Color
                    );
                }

                panel.WriteText($"{(facility_Index_WholeList_Int + 1).ToString() + ".\n"}{facilityList[facility_Index_WholeList_Int].Name}", true);
                panel.WriteText($"\n{facilityList[facility_Index_WholeList_Int].Picture}", true);
                panel.WriteText("\n\n", true);
            }
        }

        public void DrawSingleFacilityUnit(IMyTextPanel panel, ref MySpriteDrawFrame frame, RectangleF viewport_RectangleF, string Name, bool isProducing, string itemAmount, string picture, bool isRepeating, bool isCooperative, bool isEnabled, int index, Color card_Color)
        {
            float
                fontsize_ScalingFactor_Float = viewport_RectangleF.Width / 512f,
                background_ScalingFactor_Float = 0.92f;
            float row_Interval = viewport_RectangleF.Height;
            float fontsize_Float = 0.75f;

            //  ItemAmount box
            RectangleF 
                itemAmount_Box_RectangleF = new RectangleF
                (
                    viewport_RectangleF.Position,
                    new Vector2(row_Interval * 4f, row_Interval)
                ),
                itemAmount_Box_Background_RectangleF = ScalingViewport(itemAmount_Box_RectangleF, background_ScalingFactor_Float, 2),
                itemAmount_Box_Content_RectangleF = ScalingViewport(itemAmount_Box_Background_RectangleF, 0.93f, 2);
            DrawBox(ref frame, itemAmount_Box_Background_RectangleF, card_Color);
            if (isRepeating) PanelWriteText(ref frame, "RE", itemAmount_Box_Content_RectangleF, fontsize_Float * fontsize_ScalingFactor_Float, font_Color_Overall, TextAlignment.LEFT);
            PanelWriteText(ref frame, itemAmount, itemAmount_Box_Content_RectangleF, fontsize_Float * fontsize_ScalingFactor_Float, font_Color_Overall,  TextAlignment.RIGHT);

            //  picture box
            RectangleF 
                picture_Box_RectangleF = new RectangleF
                (
                    itemAmount_Box_RectangleF.Position + new Vector2(itemAmount_Box_RectangleF.Width, 0),
                    new Vector2(row_Interval, row_Interval)
                ),
                picture_Box_Background_RectangleF = ScalingViewport(picture_Box_RectangleF, background_ScalingFactor_Float);
            DrawBox(ref frame, picture_Box_Background_RectangleF, card_Color);
            if(picture != "Empty") DrawIcon(ref frame, TranslateSpriteName(picture), picture_Box_Background_RectangleF, font_Color_Overall);

            //  production box
            RectangleF
                production_Box_RectangleF = new RectangleF
                (
                    picture_Box_RectangleF.Position + new Vector2(picture_Box_RectangleF.Width, 0),
                    new Vector2(row_Interval, row_Interval)
                ),
                production_Box_Background_RectangleF = ScalingViewport(production_Box_RectangleF, background_ScalingFactor_Float),
                production_Box_Content_RectangleF = ScalingViewport(production_Box_Background_RectangleF, 0.9f);
            if (isEnabled)
            {
                if (isProducing) DrawBox(ref frame, production_Box_Background_RectangleF, new Color(0, 140, 0));
                else DrawBox(ref frame, production_Box_Background_RectangleF, new Color(130, 100, 0));
            }
            else
            {
                DrawBox(ref frame, production_Box_Background_RectangleF, new Color(178, 9, 9));
            }
            if (isCooperative) DrawIcon(ref frame, "Circle", production_Box_Content_RectangleF, new Color(0, 0, 255));

            //  name box
            RectangleF
                name_Box_RectangleF = new RectangleF
                (
                    production_Box_RectangleF.Position + new Vector2(production_Box_RectangleF.Width, 0),
                    new Vector2(viewport_RectangleF.Width - production_Box_RectangleF.Width - picture_Box_RectangleF.Width - itemAmount_Box_RectangleF.Width, row_Interval)
                ),
                name_Box_Background_RectangleF = ScalingViewport(name_Box_RectangleF, background_ScalingFactor_Float, 2),
                name_Box_Content_RectangleF = ScalingViewport(name_Box_Background_RectangleF, 0.93f, 2);
            DrawBox(ref frame, name_Box_Background_RectangleF, card_Color);
            PanelWriteText(ref frame, Name, name_Box_Content_RectangleF, fontsize_Float * fontsize_ScalingFactor_Float, font_Color_Overall);
        }

        public string TranslateSpriteName(string name)
        {
            string[] blueprintIds = name.Split('/');
            string blueprintId_String = blueprintIds[blueprintIds.Length - 1];

            string temp = "Textures\\FactionLogo\\Empty.dds";
            foreach (var sprite in spritesList)
            {

                foreach(var productionitem in productionList)
                {
                    if(productionitem.ProductionName.IndexOf(blueprintId_String) != -1)
                    {
                        blueprintId_String = productionitem.ComponentName;
                    }
                }

                if (sprite.IndexOf(blueprintId_String) != -1)
                {
                    temp = sprite;
                    break;
                }
            }
            return temp;
        }

        /*###############   Refinery_And_Assembler    ###############*/
        /*###########################################################*/


        /*#######################################################*/
        /*###############     Assembler_Clear     ###############*/
        public void Assembler_Clear(string nextStage)
        {
            if (assemblers.Count < 1 || cargoContainers.Count < 1 || !function_InventoryManagement_Bool)
            {
                stage_Key = nextStage;
                return;
            }


            Echo($"{counter_Assembler_Int}/{assemblers.Count}");

            Assembler_Check(assemblers[counter_Assembler_Int - 1]);


            if (counter_Assembler_Int >= assemblers.Count)
            {
                counter_Assembler_Int = 1;
                stage_Key = nextStage;
                return;
            }
            counter_Assembler_Int++;
        }

        public void Assembler_Check(IMyAssembler assembler)
        {
            if (assembler.Mode == MyAssemblerMode.Assembly)
            {
                Input_And_Output_Inventory(assembler);
            }
            else
            {
                ClearInventory(assembler.InputInventory);
            }
        }

        public void Input_And_Output_Inventory(IMyProductionBlock productionBlock)
        {
            ClearInventory(productionBlock.OutputInventory);
            if (productionBlock.IsQueueEmpty || !productionBlock.IsQueueEmpty & !productionBlock.IsWorking)
            {
                ClearInventory(productionBlock.InputInventory);
            }
        }

        public void ClearInventory(IMyInventory inventory_Block)
        {
            foreach (var cargoContainer in cargoContainers)
            {
                List<MyInventoryItem> items = new List<MyInventoryItem>();
                inventory_Block.GetItems(items);

                if (items.Count < 1) return;

                foreach (var item in items)
                {
                    bool tf = inventory_Block.TransferItemTo(cargoContainer.GetInventory(), item);
                }
            }
        }
        /*###############     Assembler_Clear     ###############*/
        /*#######################################################*/


        /*######################################################*/
        /*###############     Refinery_Clear     ###############*/
        public void Refinery_Clear(string nextStage)
        {
            if (refineries.Count < 1 || cargoContainers.Count < 1 || !function_InventoryManagement_Bool)
            {
                stage_Key = nextStage;
                return;
            }


            Echo($"{counter_Refinery_Int}/{refineries.Count}");

            Input_And_Output_Inventory(refineries[counter_Refinery_Int - 1]);


            if (counter_Refinery_Int >= refineries.Count)
            {
                counter_Refinery_Int = 1;
                stage_Key = nextStage;
                return;
            }
            counter_Refinery_Int++;
        }
        /*###############     Refinery_Clear     ###############*/
        /*######################################################*/


        /*#######################################################*/
        /*###############     Connector_Clear     ###############*/
        public void Connector_Clear(string nextStage)
        {
            if (connectors.Count < 1 || cargoContainers.Count < 1 || !function_InventoryManagement_Bool)
            {
                stage_Key = nextStage;
                return;
            }


            Echo($"{counter_Connector_Int}/{connectors.Count}");

            ClearInventory(connectors[counter_Connector_Int - 1].GetInventory());


            if (counter_Connector_Int >= connectors.Count)
            {
                counter_Connector_Int = 1;
                stage_Key = nextStage;
                return;
            }
            counter_Connector_Int++;
        }
        /*###############     Connector_Clear     ###############*/
        /*#######################################################*/


        /*#########################################################*/
        /*###############     CryoChamber_Clear     ###############*/
        public void CryoChamber_Clear(string nextStage)
        {
            if (cryoChambers.Count < 1 || cargoContainers.Count < 1 || !function_InventoryManagement_Bool)
            {
                stage_Key = nextStage;
                return;
            }


            Echo($"{counter_CryoChamber_Int}/{cryoChambers.Count}");

            ClearInventory(cryoChambers[counter_CryoChamber_Int - 1].GetInventory());


            if (counter_CryoChamber_Int >= cryoChambers.Count)
            {
                counter_CryoChamber_Int = 1;
                stage_Key = nextStage;
                return;
            }
            counter_CryoChamber_Int++;
        }
        /*###############     CryoChamber_Clear     ###############*/
        /*#########################################################*/


        /*####################################################*/
        /*###############     Sorter_Clear     ###############*/
        public void Sorter_Clear(string nextStage)
        {
            if (sorters.Count < 1 || cargoContainers.Count < 1 || !function_InventoryManagement_Bool)
            {
                stage_Key = nextStage;
                return;
            }

            Echo($"{counter_Sorter_Int}/{sorters.Count}");

            ClearInventory(sorters[counter_Sorter_Int - 1].GetInventory());


            if (counter_Sorter_Int >= sorters.Count)
            {
                counter_Sorter_Int = 1;
                stage_Key = nextStage;
                return;
            }
            counter_Sorter_Int++;
        }
        /*###############     Sorter_Clear     ###############*/
        /*####################################################*/


        /*###############################################*/
        /*###############     GasTank     ###############*/
        public void GasTank(List<IMyGasTank> gasTanks, ref int counter_Int, string bottleName_String, string nextStage)
        {
            if (gasTanks.Count < 1 || cargoContainers.Count < 1 || !function_InventoryManagement_Bool)
            {
                counter_Int = 1;
                stage_Key = nextStage;
                return;
            }


            Echo($"{counter_Int}/{gasTanks.Count}");

            Bottles_to_Tanks(bottleName_String, gasTanks[counter_Int - 1]);


            if (counter_Int >= gasTanks.Count)
            {
                counter_Int = 1;
                stage_Key = nextStage;
                return;
            }
            counter_Int++;
        }

        public void Bottles_to_Tanks(string itemType, IMyGasTank Gastank)
        {

            if (!Gastank.AutoRefillBottles) Gastank.AutoRefillBottles = true;

            foreach (var cargoContainer in cargoContainers)
            {
                List<MyInventoryItem> items = new List<MyInventoryItem>();
                cargoContainer.GetInventory().GetItems(items);
                foreach (var item in items)
                {
                    string str = item.Type.ToString();
                    if (str.IndexOf(itemType) != -1)
                    {
                        bool tf = cargoContainer.GetInventory().TransferItemTo(Gastank.GetInventory(), item);
                    }
                }
            }

        }
        /*###############     GasTank     ###############*/
        /*###############################################*/


        /*  Broadcast Connectors GPS    */
        public void Broadcast_Connectors_GPS()
        {

            if (!function_BroadCastConnectorGPS_Bool) return;

            List<IMyShipConnector> connectors_BroadCast = new List<IMyShipConnector>();
            GridTerminalSystem.GetBlocksOfType(connectors_BroadCast, block => block.IsConnected == false && GetValue_from_CustomData(block, "Connector_Tag", "ForAutoParking") != "No" && block.IsSameConstructAs(Me));

            if (connectors_BroadCast.Count == 0) return;
            
            StringBuilder sb = new StringBuilder();

            sb.Clear();
            sb.Append(connectors_BroadCast.Count.ToString());
            sb.Append("=");

            foreach (var connector in connectors_BroadCast)
            {
                sb.Append("【");
                sb.Append(connector.CustomName.ToString());
                sb.Append("：");
                sb.Append(connector.GetPosition().X.ToString());
                sb.Append("：");
                sb.Append(connector.GetPosition().Y.ToString());
                sb.Append("：");
                sb.Append(connector.GetPosition().Z.ToString());
                sb.Append("：");
                sb.Append(connector.WorldMatrix.Forward.X.ToString());
                sb.Append("：");
                sb.Append(connector.WorldMatrix.Forward.Y.ToString());
                sb.Append("：");
                sb.Append(connector.WorldMatrix.Forward.Z.ToString());
            }

            string value_String = GetValue_from_CustomData(information_Section, "IGCTAG");
            if (value_String == null || value_String == "") WriteValue_to_CustomData(information_Section, "IGCTAG", "CHANNEL1");
            IGC.SendBroadcastMessage(value_String, sb.ToString());

            WriteValue_to_CustomData("Connectors_Information", "Value1", sb.ToString());



            sb.Clear();
            sb.Append(connectors_BroadCast.Count.ToString());
            sb.Append("=");

            foreach (var connector in connectors_BroadCast)
            {
                sb.Append("[");
                sb.Append(connector.CustomName.ToString());
                sb.Append(":");
                sb.Append(connector.GetPosition().X.ToString());
                sb.Append(":");
                sb.Append(connector.GetPosition().Y.ToString());
                sb.Append(":");
                sb.Append(connector.GetPosition().Z.ToString());
                sb.Append(":");
                sb.Append(connector.WorldMatrix.Forward.X.ToString());
                sb.Append(":");
                sb.Append(connector.WorldMatrix.Forward.Y.ToString());
                sb.Append(":");
                sb.Append(connector.WorldMatrix.Forward.Z.ToString());
            }

            value_String = GetValue_from_CustomData(information_Section, "IGCTAG");
            IGC.SendBroadcastMessage(value_String, sb.ToString());

            WriteValue_to_CustomData("Connectors_Information", "Value2", sb.ToString());

        }
        /*  Broadcast Connectors GPS    */


        /*#############################################################*/
        /*###############   ShowCargoContainerResidues  ###############*/
        public void ShowCargoContainerResidues(string nextStage)
        {
            if (cargoContainers.Count < 1 || !function_ShowCargoContainerRatio_Bool)
            {
                stage_Key = nextStage;
                return;
            }

            Echo($"{counter_CargoContainer_Int}/{cargoContainers.Count}");

            string volumeThreshold_String = GetValue_from_CustomData(information_Section, volumeThreshold_Key);
            double volumeThreshold_Double = Convert.ToDouble(volumeThreshold_String) / 1000;

            for (int i = 0; i <= 9; i++)
            {
                counter_CargoContainer_Int++;
                if (counter_CargoContainer_Int > cargoContainers.Count)
                {
                    counter_CargoContainer_Int = 0;
                    stage_Key = nextStage;
                    return;
                }


                var cargoContainer = cargoContainers[counter_CargoContainer_Int - 1];

                string newName_String;
                double ratio_Double;

                if ((double)cargoContainer.GetInventory().MaxVolume < volumeThreshold_Double)
                {
                    continue;
                }
                else
                {
                    newName_String = GetValue_from_CustomData(cargoContainer, information_Section, customName_Key);
                    if (newName_String == "")
                    {
                        newName_String = cargoContainer.CustomName;
                        WriteValue_to_CustomData(cargoContainer, information_Section, customName_Key, newName_String);
                    }
                    ratio_Double = (double)cargoContainer.GetInventory().CurrentVolume / (double)cargoContainer.GetInventory().MaxVolume;
                }

                ratio_Double = Math.Round(ratio_Double * 100, 1);

                cargoContainer.CustomName = newName_String + "__" + ratio_Double.ToString() + "%";


            }



        }
        /*###############   ShowCargoContainerResidues  ###############*/
        /*#############################################################*/


        /*#####################################################*/
        /*###############   Combined_Refining   ###############*/
        public void CheckEachRefinery(string nextStage)
        {
            stage_Key = nextStage;

            if (refineries.Count < 1) return;

            for (int index_Int = 1; index_Int <= 2; index_Int++)
            {

                IMyRefinery refinery_Block = refineries[counter_CombinedRefining_Int - 1];

                Echo($"{counter_CombinedRefining_Int}/{refineries.Count}");
                Echo(refinery_Block.CustomName);

                if (counter_CombinedRefining_Int++ >= refineries.Count) counter_CombinedRefining_Int = 1;

                if (!refinery_Block.Enabled) continue;

                switch (GetValue_from_CustomData(ore_Section, combinedMode_Key))
                {
                    case "0":
                        IndependentMode(refinery_Block);
                        break;
                    case "1":
                        UnifiedMode(refinery_Block);
                        break;
                    case "2":
                        MixedMode(refinery_Block);
                        break;
                }
            }
        }

        public void IndependentMode(IMyRefinery refinery_Block)
        {
            Echo("IndependentMode");

            WriteValue_to_CustomData(refinery_Block, ore_Section, combinedMode_Key, "0");

            method_Refinery_Dic = new Dictionary<string, double>();

            Build_MethodDic(method_Refinery_Dic, refinery_Block);

            if (method_Refinery_Dic.Count < 1) return;

            GetItemsInRefinery(refinery_Block);

            CompareOre(refinery_Block, method_Refinery_Dic);

        }

        public void UnifiedMode(IMyRefinery refinery_Block)
        {
            Echo("UnifiedMode");

            WriteValue_to_CustomData(refinery_Block, ore_Section, combinedMode_Key, "1");

            if (method_Unified_Dic.Count < 1) return;

            GetItemsInRefinery(refinery_Block);

            CompareOre(refinery_Block, method_Unified_Dic);

        }

        public void MixedMode(IMyRefinery refinery_Block)
        {
            Echo("MixedMode");

            method_Refinery_Dic = new Dictionary<string, double>();

            Build_MethodDic(method_Refinery_Dic, refinery_Block);

            if (method_Refinery_Dic.Count > 0)
            {
                WriteValue_to_CustomData(refinery_Block, ore_Section, combinedMode_Key, "0");

                GetItemsInRefinery(refinery_Block);

                CompareOre(refinery_Block, method_Refinery_Dic);
            }
            else
            {
                WriteValue_to_CustomData(refinery_Block, ore_Section, combinedMode_Key, "1");

                if (method_Unified_Dic.Count < 1) return;

                GetItemsInRefinery(refinery_Block);

                CompareOre(refinery_Block, method_Unified_Dic);

            }


        }

        public void GetItemsInRefinery(IMyRefinery refinery_Block)
        {
            var items = new List<MyInventoryItem>();
            allItems_InRefineries_Dic = new Dictionary<string, double>();

            refinery_Block.InputInventory.GetItems(items);

            foreach (var item in items)
            {
                if (allItems_InRefineries_Dic.ContainsKey(item.Type.ToString())) allItems_InRefineries_Dic[item.Type.ToString()] += (double)item.Amount.RawValue;
                else allItems_InRefineries_Dic.Add(item.Type.ToString(), (double)item.Amount.RawValue);
            }

        }

        public void CompareOre(IMyRefinery refinery_Block, Dictionary<string, double> method_Dic)
        {
            foreach (var key in allItems_InRefineries_Dic.Keys) if (!method_Dic.ContainsKey(key)) MoveAwayItem(refinery_Block, key);

            foreach (var key in method_Dic.Keys) RegulateItem(refinery_Block, key, method_Dic);
        }

        public void RegulateItem(IMyRefinery refinery_Block, string itemName_String, Dictionary<string, double> method_Dic)
        {
            foreach (var cargoContainer in cargoContainers)
            {
                MyFixedPoint amount_FP;
                var items = new List<MyInventoryItem>();
                double itemAmount_Difference_Double;

                GetItemsInRefinery(refinery_Block);

                if (allItems_InRefineries_Dic.ContainsKey(itemName_String)) itemAmount_Difference_Double = allItems_InRefineries_Dic[itemName_String] - method_Dic[itemName_String];
                else itemAmount_Difference_Double = -method_Dic[itemName_String];

                amount_FP.RawValue = Convert.ToInt64(Math.Abs(itemAmount_Difference_Double));

                if (itemAmount_Difference_Double > 0)
                {
                    refinery_Block.InputInventory.GetItems(items);

                    foreach (var item in items)
                    {
                        if (item.Type.ToString() == itemName_String)
                        {
                            refinery_Block.InputInventory.TransferItemTo(cargoContainer.GetInventory(), item, amount_FP);
                        }
                    }
                }
                else if (itemAmount_Difference_Double < 0)
                {
                    cargoContainer.GetInventory().GetItems(items);

                    foreach (var item in items)
                    {
                        if (item.Type.ToString() == itemName_String)
                        {
                            cargoContainer.GetInventory().TransferItemTo(refinery_Block.InputInventory, item, amount_FP);
                        }
                    }
                }
                else return;
            }
        }

        public void MoveAwayItem(IMyRefinery refinery_Block, string itemName_String)
        {

            foreach (var cargoContainer in cargoContainers)
            {

                if (!allItems_InRefineries_Dic.ContainsKey(itemName_String)) return;

                var items = new List<MyInventoryItem>();
                refinery_Block.InputInventory.GetItems(items);

                foreach (var item in items)
                {
                    if (item.Type.ToString() == itemName_String)
                    {
                        refinery_Block.InputInventory.TransferItemTo(cargoContainer.GetInventory(), item);
                    }
                }
            }
        }


        /*###############   Combined_Refining   ###############*/
        /*#####################################################*/


        /*##################################################*/
        /*###############   AutoProduction   ###############*/

        public void Build_ProductionList()
        {
            int length_Int = Convert.ToInt16(GetValue_from_CustomData(autoProductionList_Section, length_Key));
            productionList = new ProdcutionProperty[length_Int];

            for(int i = 1; i <= length_Int; i++)
            {
                string value_String = GetValue_from_CustomData(autoProductionList_Section, i.ToString());

                string[] value_Array = value_String.Split(':');

                if(value_Array.Length == 3)
                {
                    productionList[i - 1].ComponentName = value_Array[0];
                    productionList[i - 1].ProductionName = value_Array[1];
                    productionList[i - 1].ProductionAmount = Convert.ToDouble(value_Array[2]);
                }
            }
        }

        public void AutoProduction(string nextStage)
        {
            const int counter_TotalCycle_Int = 2;

            if (!function_AutoProduction_Bool)
            {
                stage_Key = nextStage;
                counter_AutoProduction_Int = 1;
                return;
            }

            Echo($"{counter_AutoProduction_Int}/{counter_TotalCycle_Int}");
            switch (counter_AutoProduction_Int)
            {
                case 1:
                    Echo("PrepareData");
                    GetItemsFromAssemblers();
                    SumItems_Old_Assemblers();
                    break;
                case 2:
                    Echo("SendProductionOrder");
                    SendProductionOrder();
                    break;
            }

            if (counter_AutoProduction_Int >= counter_TotalCycle_Int)
            {
                stage_Key = nextStage;
                counter_AutoProduction_Int = 1;
                return;
            }
            counter_AutoProduction_Int++;


        }

        public void GetItemsFromAssemblers()
        {
            allitems_InAssemblers_Dic = new Dictionary<string, double>();

            foreach(var assembler in assemblers)
            {
                List <MyProductionItem> items = new List<MyProductionItem>();
                assembler.GetQueue(items);
                foreach (var item in items)
                {
                    if (allitems_InAssemblers_Dic.ContainsKey(item.BlueprintId.ToString())) allitems_InAssemblers_Dic[item.BlueprintId.ToString()] += (double)item.Amount.RawValue;
                    else allitems_InAssemblers_Dic.Add(item.BlueprintId.ToString(), (double)item.Amount.RawValue);
                }
            }

            foreach(var item in allitems_InAssemblers_Dic.Keys)
            {
                debug_StringBuilder.Append($"Item={item}");
                debug_StringBuilder.Append("\n");
                debug_StringBuilder.Append($"Amount={allitems_InAssemblers_Dic[item]}");
                debug_StringBuilder.Append("\n");

            }
        }

        public void SumItems_Old_Assemblers()
        {
            allItems_Old_Assemblers_Dic = new Dictionary<string, double>();
            allItems_Old_Assemblers_Dic = allItems_Old_Dic;
            foreach (var item in productionList)
            {
                if (allitems_InAssemblers_Dic.ContainsKey(item.ProductionName))
                {
                    if (allItems_Old_Dic.ContainsKey(item.ComponentName))
                    {
                        allItems_Old_Assemblers_Dic[item.ComponentName] += 
                            allitems_InAssemblers_Dic[item.ProductionName];
                    }
                    else
                    {
                        allItems_Old_Assemblers_Dic.Add
                            (
                            item.ComponentName,
                            allitems_InAssemblers_Dic[item.ProductionName]
                            );
                    }

                }
            }
        }

        public void SendProductionOrder()
        {
            foreach(var item in productionList)
            {
                if (allItems_Old_Assemblers_Dic.ContainsKey(item.ComponentName))
                {
                    double residus_Double = item.ProductionAmount * 1000000 - allItems_Old_Assemblers_Dic[item.ComponentName];
                    if (residus_Double > 0) AddItemToAssemblerQueue(item.ProductionName, residus_Double / 1000000);
                }
                else
                {
                    AddItemToAssemblerQueue(item.ProductionName, item.ProductionAmount);
                }
            }
        }

        public void AddItemToAssemblerQueue(string itemName_String, double itemAmount_Double)
        {
            foreach(var assembler in assemblers)
            {
                if (!assembler.CooperativeMode)
                {
                    MyDefinitionId item_MyDefinitionId = MyDefinitionId.Parse(itemName_String);
                    assembler.AddQueueItem(item_MyDefinitionId, itemAmount_Double);
                    return;
                }
            }
        }

        /*###############   AutoProduction   ###############*/
        /*##################################################*/



        /*################################################################*/
        /*####################   CombiningLikeTerms   ####################*/

        public void CombiningLikeTerms(string nextStage)
        {
            if (cargoContainers.Count < 2 || !function_InventoryManagement_Bool)
            {
                stage_Key = nextStage;
                return;
            }

            Echo($"{counter_CombiningLikeTerms_Int}/2");

            switch (counter_CombiningLikeTerms_Int)
            {
                case 1:
                    Echo("TransferItemsToFrontBoxes");
                    TransferItemsToFrontBoxes();
                    break;
                case 2:
                    Echo("CargoContainerCycle");
                    CargoContainerCycle(nextStage);
                    break;
            }
        }

        public void TransferItemsToFrontBoxes()
        {
            counter_CombiningLikeTerms_Int = 2;

            for (int index_CountDown_Int = cargoContainers.Count; index_CountDown_Int >= 2; index_CountDown_Int--)
            {
                IMyCargoContainer currentCargoContainer = cargoContainers[index_CountDown_Int - 1];
                if (currentCargoContainer.GetInventory().CurrentVolume == 0) continue;


                for (int index_CountForward_Int = 1; index_CountForward_Int < index_CountDown_Int; index_CountForward_Int++)
                {
                    if (currentCargoContainer.GetInventory().CurrentVolume == 0) break;

                    IMyCargoContainer targetCargoContainer = cargoContainers[index_CountForward_Int - 1];
                    if (targetCargoContainer.GetInventory().CurrentVolume == targetCargoContainer.GetInventory().MaxVolume) continue;

                    List<MyInventoryItem> items = new List<MyInventoryItem>();
                    currentCargoContainer.GetInventory().GetItems(items);

                    foreach (var item in items)
                    {
                        currentCargoContainer.GetInventory().TransferItemTo
                            (
                            targetCargoContainer.GetInventory(), 
                            item
                            );
                    }
                }
            }
        }

        public void CargoContainerCycle(string nextStage)
        {
            for(int i = 1; i <= 2; i++)
            {
                int cargoContainer_Index_Int = counter_CombiningLikeTerms_CargoContainer_Int + i - 1;
                if(cargoContainer_Index_Int > cargoContainers.Count)
                {
                    counter_CombiningLikeTerms_CargoContainer_Int = 1;
                    cargoContainer_Index_Int = counter_CombiningLikeTerms_CargoContainer_Int + i - 1;
                }

                IMyCargoContainer currentCargoContainer = cargoContainers[cargoContainer_Index_Int - 1];

                List<MyInventoryItem> items = new List<MyInventoryItem>();
                currentCargoContainer.GetInventory().GetItems(items);

                foreach (var item in items)
                {
                    currentCargoContainer.GetInventory().TransferItemTo
                        (
                        currentCargoContainer.GetInventory(),
                        item
                        );
                }
            }

            stage_Key = nextStage;
            counter_CombiningLikeTerms_Int = 1;

        }


        /*####################   CombiningLikeTerms   ####################*/
        /*################################################################*/



        /*######################################################*/
        /*####################   Argument   ####################*/

        public void Argument_Handler(string argument)
        {
            if(argument == "CLS")
            {
                AssemblerCLS();
            }
            else if (argument == "CO_ON")
            {
                AssemblerCO_ON();
            }
            else if (argument == "CO_OFF")
            {
                AssemblerCO_OFF();
            }
            else if(argument == "LCD_REF")
            {
                RefreshLCD();
            }
            else
            {
                string[] argument_Array = argument.Split('=');
                if (argument_Array.Length == 2)
                {
                    switch (argument_Array[0])
                    {
                        case "RE_REF":
                            ReNameBlocks(refineries, argument_Array[1]);
                            break;
                        case "RE_ASS":
                            ReNameBlocks(assemblers, argument_Array[1]);
                            break;
                        case "RE_BOX":
                            ReNameBlocks(cargoContainers, argument_Array[1]);
                            break;
                    }
                }
            }
        }

        public void AssemblerCLS()
        {
            foreach (var assembler in assemblers) assembler.ClearQueue();
        }

        public void AssemblerCO_ON()
        {
            foreach (var assembler in assemblers) if (assembler.CooperativeMode != true) assembler.CooperativeMode = true;
        }

        public void AssemblerCO_OFF()
        {
            foreach (var assembler in assemblers) if (assembler.CooperativeMode != false) assembler.CooperativeMode = false;
        }

        public void ReNameBlocks(List<IMyRefinery> blocks, string newName_String)
        {
            int i = 1;
            foreach (var block in blocks)
            {
                block.CustomName = BuildIndexString(blocks.Count, newName_String, i);
                i++;
            }
        }

        public void ReNameBlocks(List<IMyAssembler> blocks, string newName_String)
        {
            int i = 1;
            foreach (var block in blocks)
            {
                block.CustomName = BuildIndexString(blocks.Count, newName_String, i);
                i++;
            }
        }

        public void ReNameBlocks(List<IMyCargoContainer> blocks, string newName_String)
        {
            string volumeThreshold_String = GetValue_from_CustomData(information_Section, volumeThreshold_Key);
            double volumeThreshold_Double = Convert.ToDouble(volumeThreshold_String) / 1000;

            int i = 1;
            foreach (var block in blocks)
            {
                if ((double)block.GetInventory().MaxVolume < volumeThreshold_Double) continue;

                block.CustomName = BuildIndexString(blocks.Count, newName_String, i);
                WriteValue_to_CustomData(block, information_Section, customName_Key, BuildIndexString(blocks.Count, newName_String, i));
                i++;
            }
        }

        public string BuildIndexString(int totalLength_Int, string newName_String, int index_Original_Int)
        {
            string index_String = index_Original_Int.ToString();

            totalLength_Int = Convert.ToString(totalLength_Int).Length;

            int difference_Int = totalLength_Int - index_String.Length;

            for (int prefix_Int = 1; prefix_Int <= difference_Int; prefix_Int++)
            {
                index_String = "0" + index_String;
            }

            newName_String = newName_String + "_" + index_String;

            return newName_String;
        }

        public void RefreshLCD()
        {
            List<IMyTextPanel> all_LCDs = new List<IMyTextPanel>();

            all_LCDs.AddRange(panels_Items_All);
            all_LCDs.AddRange(panels_Items_Ore);
            all_LCDs.AddRange(panels_Items_Ingot);
            all_LCDs.AddRange(panels_Items_Component);
            all_LCDs.AddRange(panels_Items_AmmoMagazine);
            all_LCDs.AddRange(panels_Refineries);
            all_LCDs.AddRange(panels_Assemblers);
            all_LCDs.AddRange(panels_Overall);

            foreach(var lcd in all_LCDs)
            {
                float heightOffset_Float = Convert.ToSingle(counter_Logo_Float);

                if (lcd.ContentType != ContentType.SCRIPT) lcd.ContentType = ContentType.SCRIPT;
                MySpriteDrawFrame frame = lcd.DrawFrame();

                RectangleF viewport_RectangleF = new RectangleF(
                    (lcd.TextureSize - lcd.SurfaceSize) / 2f,
                    lcd.SurfaceSize + new Vector2(0, heightOffset_Float)
                );

                DrawIcon(ref frame, "UVChecker",
                    viewport_RectangleF.Center.X, viewport_RectangleF.Center.Y,
                    viewport_RectangleF.Width, viewport_RectangleF.Height,
                    Color.White);

                frame.Dispose();


            }

        }

        /*####################   Argument   ####################*/
        /*######################################################*/





        public void MainLogic()
        {
            if (stage_Key == "") stage_Key = stage_ShowItems;

            if (counter_Sub_Function_Interval_Int < 10 && stage_Key == stage_Assembler_Clear) stage_Key = stage_ShowItems;

            Echo($"{stage_Key}");



            switch (stage_Key)
            {
                case stage_ShowItems:
                    if (counter_Sub_Function_Interval_Int >= 10) counter_Sub_Function_Interval_Int = 1;
                    ShowItems(stage_ShowFacilities);
                    break;
                case stage_ShowFacilities:
                    ShowFacilities(stage_Combined_Refining);
                    break;
                case stage_Combined_Refining:
                    counter_Sub_Function_Interval_Int++;
                    CheckEachRefinery(stage_AutoProduction);
                    break;

                case stage_AutoProduction:
                    AutoProduction(stage_Assembler_Clear);
                    break;
                case stage_Assembler_Clear:
                    Assembler_Clear(stage_Refinery_Clear);
                    break;
                case stage_Refinery_Clear:
                    Refinery_Clear(stage_Connector_Clear);
                    break;
                case stage_Connector_Clear:
                    Connector_Clear(stage_CryoChamber_Clear);
                    break;
                case stage_CryoChamber_Clear:
                    CryoChamber_Clear(stage_Sorter_Clear);
                    break;
                case stage_Sorter_Clear:
                    Sorter_Clear(stage_HydrogenTank);
                    break;
                case stage_HydrogenTank:
                    GasTank(hydrogenTanks, ref counter_HydrogenTank_Int, "HydrogenBottle", stage_OxygenTank);
                    break;
                case stage_OxygenTank:
                    GasTank(oxygenTanks, ref counter_OxydrogenTank_Int, "OxygenBottle", stage_CombiningLikeTerms);
                    break;
                case stage_CombiningLikeTerms:
                    CombiningLikeTerms(stage_ShowCargoContainerResidues);
                    break;
                case stage_ShowCargoContainerResidues:
                    ShowCargoContainerResidues(stage_ShowItems);
                    break;
            }

            Broadcast_Connectors_GPS();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            Echo("Main");

            debug_StringBuilder = new StringBuilder();

            ProgrammableBlockScreen();

            OverallDisplay();

            MainLogic();

            Argument_Handler(argument);

        }
    }
}
