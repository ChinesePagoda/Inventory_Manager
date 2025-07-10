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

        double counter_Logo = 0;
        DateTime time1_DateTime = DateTime.Now;
        DateTime time2_DateTime = DateTime.Now;

        StringBuilder debug_StringBuilder;

        int counter_ShowItems_Int = 0, counter_ShowFacilities_Int = 1, counter_Panel_Int = 0,
            counter_Assembler_Int = 1, counter_Refinery_Int = 1, counter_CombinedRefining_Int = 1,
            counter_Connector_Int = 1, counter_CryoChamber_Int = 1, counter_Sorter_Int = 1,
            counter_HydrogenTank_Int = 1, counter_OxydrogenTank_Int = 1,
            counter_CargoContainer_Int = 1,
            maxNumber_AssemblerPanel_Int = 0, maxNumber_RefineryPanel_Int = 0,
            counter_AutoProduction_Int = 1,
            counter_CombiningLikeTerms_Int = 1, counter_CombiningLikeTerms_CargoContainer_Int = 1,
            counter_Sub_Function_Interval_Int = 1;

        const int itemAmountInEachScreen = 28,
            facilityAmountInEachScreen = 20,
            method_Total_Int = 10;
        const float itemBox_ColumnInterval_Float = 73,
            itemBox_RowInterval_Float = 125,
            amountBox_Height_Float = 24,
            facilityBox_RowInterval_Float = 25.5f;
        const string information_Section = "Information",
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


        Color card_Background_Color_Overall = new Color(10, 20, 40);
        Color ore_Background_Color = new Color(10, 5, 2);
        Color ingot_Background_Color = new Color(5, 2, 15);
        //Color border_Color = new Color(0, 130, 255);
        Color font_Color_Overall = new Color(230, 255, 255);
        Color progressbar_Color = new Color(30, 50, 90);
        Color oreCard_Background_Color = new Color(50, 40, 20);
        Color ingotCard_Background_Color = new Color(35, 25, 45);


        public Program()
        {

            BuildBlockList();

            SetDefultConfiguration();

            BuildTranslateDic();

            BuildProductionList();

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
            if (counter_Logo++ >= 360f) counter_Logo = 0f;

            //  512 X 320
            IMyTextSurface panel = Me.GetSurface(0);

            if (panel == null) return;
            panel.ContentType = ContentType.SCRIPT;

            MySpriteDrawFrame frame = panel.DrawFrame();

            float x = 512 / 2, y1 = 205 + Convert.ToSingle(counter_Logo) / 360f;
            DrawLogo(ref frame, x, y1, 200);
            PanelWriteText(ref frame, "Inventory_Management\nWith_Graphic_Interface_V1.3\nby Hi.James", x, y1 + 110 + Convert.ToSingle(counter_Logo / 360), 1f, TextAlignment.CENTER);

            frame.Dispose();

        }

        public void Assemblers_CooperativeMode(bool true_False)
        {
            foreach (var assembler in assemblers)
            {
                if (!assembler.CooperativeMode == true_False) assembler.CooperativeMode = true_False;
            }
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

                panel.ScriptBackgroundColor = font_Color_Overall;

                MySpriteDrawFrame frame = panel.DrawFrame();

                DrawContentBox(panel, ref frame);

                frame.Dispose();
            }
        }

        public void DrawContentBox(IMyTextPanel panel, ref MySpriteDrawFrame frame)
        {
            float x_Left = itemBox_ColumnInterval_Float / 2 + 1.5f, x_Right = itemBox_ColumnInterval_Float + 2 + (512 - itemBox_ColumnInterval_Float - 4) / 2, x_Title = 70, y_Title = itemBox_ColumnInterval_Float + 2 + Convert.ToSingle(panel.CustomData);
            float progressBar_YCorrect = 0f, progressBarWidth = 512 - itemBox_ColumnInterval_Float - 6, progressBarHeight = itemBox_ColumnInterval_Float - 3;
            float fontsize_ProgressBar_Float = 1.2f;

            //  Title
            DrawBox(ref frame, x_Left, x_Left + Convert.ToSingle(panel.CustomData), itemBox_ColumnInterval_Float, itemBox_ColumnInterval_Float, card_Background_Color_Overall);
            DrawBox(ref frame, 512 - x_Left, x_Left + Convert.ToSingle(panel.CustomData), itemBox_ColumnInterval_Float, itemBox_ColumnInterval_Float, card_Background_Color_Overall);
            DrawBox(ref frame, 512 / 2, x_Left + Convert.ToSingle(panel.CustomData), 512 - itemBox_ColumnInterval_Float * 2 - 4, itemBox_ColumnInterval_Float, card_Background_Color_Overall);
            PanelWriteText(ref frame, panels_Overall[0].GetOwnerFactionTag(), 512 / 2, 2 + Convert.ToSingle(panel.CustomData), 2.3f, TextAlignment.CENTER);

            DrawLogo(ref frame, x_Left, x_Left + Convert.ToSingle(panel.CustomData), itemBox_ColumnInterval_Float);
            DrawLogo(ref frame, 512 - x_Left, x_Left + Convert.ToSingle(panel.CustomData), itemBox_ColumnInterval_Float);

            for (int i = 1; i <= 5; i++)
            {
                float y = i * itemBox_ColumnInterval_Float + itemBox_ColumnInterval_Float / 2f + 1.5f + Convert.ToSingle(panel.CustomData);

                DrawBox(ref frame, x_Left, y, itemBox_ColumnInterval_Float, itemBox_ColumnInterval_Float, card_Background_Color_Overall);
                DrawBox(ref frame, x_Right, y, (512 - itemBox_ColumnInterval_Float - 4), itemBox_ColumnInterval_Float, card_Background_Color_Overall);
            }

            for (int i = 1; i <= 7; i++)
            {
                float y = 6f * itemBox_ColumnInterval_Float + itemBox_ColumnInterval_Float / 2f + 1.5f + Convert.ToSingle(panel.CustomData);
                float x = x_Left + itemBox_ColumnInterval_Float * (i - 1);
                DrawBox(ref frame, x, y, itemBox_ColumnInterval_Float, itemBox_ColumnInterval_Float, card_Background_Color_Overall);

            }

            //  All Cargo
            float y1 = itemBox_ColumnInterval_Float + itemBox_ColumnInterval_Float / 2 + 1.5f + Convert.ToSingle(panel.CustomData);
            MySprite sprite = new MySprite()
            {
                Data = "Textures\\FactionLogo\\Builders\\BuilderIcon_1.dds",
                Position = new Vector2(x_Left, y1),
                Size = new Vector2(itemBox_ColumnInterval_Float - 2, itemBox_ColumnInterval_Float - 2),
                Alignment = TextAlignment.CENTER,
                Color = font_Color_Overall,
            };
            frame.Add(sprite);
            string percentage_String, finalValue_String;
            CalculateAll(out percentage_String, out finalValue_String);
            ProgressBar(frame, x_Right, y1 + progressBar_YCorrect, progressBarWidth, progressBarHeight, percentage_String);
            PanelWriteText(ref frame, cargoContainers.Count.ToString(), x_Title, y_Title, 0.55f, TextAlignment.RIGHT);
            PanelWriteText(ref frame, percentage_String, x_Right, y_Title, fontsize_ProgressBar_Float, TextAlignment.CENTER);
            PanelWriteText(ref frame, finalValue_String, x_Right, y_Title + itemBox_ColumnInterval_Float / 2, fontsize_ProgressBar_Float, TextAlignment.CENTER);

            //  H2
            float y2 = y1 + itemBox_ColumnInterval_Float;
            sprite = new MySprite()
            {
                Data = "IconHydrogen",
                Position = new Vector2(x_Left, y2),
                Size = new Vector2(itemBox_ColumnInterval_Float - 2, itemBox_ColumnInterval_Float - 2),
                Alignment = TextAlignment.CENTER,
                Color = font_Color_Overall,
            };
            frame.Add(sprite);
            CalcualateGasTank(hydrogenTanks, out percentage_String, out finalValue_String);
            PanelWriteText(ref frame, hydrogenTanks.Count.ToString(), x_Title, y_Title + itemBox_ColumnInterval_Float, 0.55f, TextAlignment.RIGHT);
            ProgressBar(frame, x_Right, y2 + progressBar_YCorrect, progressBarWidth, progressBarHeight, percentage_String);
            PanelWriteText(ref frame, percentage_String, x_Right, y_Title + itemBox_ColumnInterval_Float, fontsize_ProgressBar_Float, TextAlignment.CENTER);
            PanelWriteText(ref frame, finalValue_String, x_Right, y_Title + itemBox_ColumnInterval_Float + itemBox_ColumnInterval_Float / 2, fontsize_ProgressBar_Float, TextAlignment.CENTER);

            //  O2
            float y3 = y2 + itemBox_ColumnInterval_Float;
            sprite = new MySprite()
            {
                Data = "IconOxygen",
                Position = new Vector2(x_Left, y3),
                Size = new Vector2(itemBox_ColumnInterval_Float - 2, itemBox_ColumnInterval_Float - 2),
                Alignment = TextAlignment.CENTER,
                Color = font_Color_Overall,
            };
            frame.Add(sprite);
            CalcualateGasTank(oxygenTanks, out percentage_String, out finalValue_String);
            PanelWriteText(ref frame, oxygenTanks.Count.ToString(), x_Title, y_Title + itemBox_ColumnInterval_Float * 2, 0.55f, TextAlignment.RIGHT);
            ProgressBar(frame, x_Right, y3 + progressBar_YCorrect, progressBarWidth, progressBarHeight, percentage_String);
            PanelWriteText(ref frame, percentage_String, x_Right, y_Title + itemBox_ColumnInterval_Float * 2, fontsize_ProgressBar_Float, TextAlignment.CENTER);
            PanelWriteText(ref frame, finalValue_String, x_Right, y_Title + itemBox_ColumnInterval_Float * 2 + itemBox_ColumnInterval_Float / 2, fontsize_ProgressBar_Float, TextAlignment.CENTER);

            //  Power
            float y4 = y3 + itemBox_ColumnInterval_Float;
            sprite = new MySprite()
            {
                Data = "IconEnergy",
                Position = new Vector2(x_Left, y4),
                Size = new Vector2(itemBox_ColumnInterval_Float - 2, itemBox_ColumnInterval_Float - 2),
                Alignment = TextAlignment.CENTER,
                Color = font_Color_Overall,
            };
            frame.Add(sprite);
            CalculatePowerProducer(out percentage_String, out finalValue_String);
            PanelWriteText(ref frame, powerProducers.Count.ToString(), x_Title, y_Title + itemBox_ColumnInterval_Float * 3, 0.55f, TextAlignment.RIGHT);
            ProgressBar(frame, x_Right, y4 + progressBar_YCorrect, progressBarWidth, progressBarHeight, percentage_String);
            PanelWriteText(ref frame, percentage_String, x_Right, y_Title + itemBox_ColumnInterval_Float * 3, fontsize_ProgressBar_Float, TextAlignment.CENTER);
            PanelWriteText(ref frame, finalValue_String, x_Right, y_Title + itemBox_ColumnInterval_Float * 3 + itemBox_ColumnInterval_Float / 2, fontsize_ProgressBar_Float, TextAlignment.CENTER);

            //  IGC
            float y_6thRow_Float = 5f * itemBox_ColumnInterval_Float + itemBox_ColumnInterval_Float / 2f + 1.5f + Convert.ToSingle(panel.CustomData);
            float x_IGCTag_Float = x_Left + itemBox_ColumnInterval_Float / 2f + 5f;
            float y_IGCTag_Float = y_6thRow_Float - itemBox_ColumnInterval_Float / 2f + 2f;
            float y_IGCTag2_Float = y_6thRow_Float + 2f;
            string igcTag_String = GetValue_from_CustomData(information_Section, "IGCTAG");

            IGCSignifier(ref frame, x_Left, y_6thRow_Float, itemBox_ColumnInterval_Float, font_Color_Overall);
            PanelWriteText(ref frame, igcTag_String, x_IGCTag_Float, y_IGCTag_Float, fontsize_ProgressBar_Float, TextAlignment.LEFT);
            PanelWriteText(ref frame, AntennaDistance(), x_IGCTag_Float, y_IGCTag2_Float, fontsize_ProgressBar_Float, TextAlignment.LEFT);

            //  Facility
            float y_7thRow_Float = 6f * itemBox_ColumnInterval_Float + itemBox_ColumnInterval_Float / 2f + 1.5f + Convert.ToSingle(panel.CustomData);

            FacilitySignifier(ref frame, x_Left, y_7thRow_Float, itemBox_ColumnInterval_Float, font_Color_Overall);

            //  Inventory
            float x_7thRow_2ndColumn_Float = x_Left + itemBox_ColumnInterval_Float * 1f;
            InventorySignifier(ref frame, x_7thRow_2ndColumn_Float, y_7thRow_Float, itemBox_ColumnInterval_Float, font_Color_Overall);

            //  Cargo Residues
            float x_Residues_Float = x_Left + itemBox_ColumnInterval_Float * 2f;
            float x_PercentageSign_Float = x_Residues_Float + itemBox_ColumnInterval_Float / 2f - 3f;
            float y_PercentageSign_Float = y_7thRow_Float - itemBox_ColumnInterval_Float / 2f - 1f;
            DrawIcon(ref frame, "Textures\\FactionLogo\\Builders\\BuilderIcon_1.dds", x_Residues_Float, y_7thRow_Float, itemBox_ColumnInterval_Float, itemBox_ColumnInterval_Float, font_Color_Overall);
            PanelWriteText(ref frame, "%", x_PercentageSign_Float, y_PercentageSign_Float, 0.95f, TextAlignment.RIGHT);

            //  Refresh Rate
            float x_RefreshRate_Float = x_Left + itemBox_ColumnInterval_Float * 3f;
            RefreshRateSignifier(ref frame, x_RefreshRate_Float, y_7thRow_Float, itemBox_ColumnInterval_Float, 3f, font_Color_Overall, card_Background_Color_Overall);

            //  Combined_Refining
            float x_Combined_Refining_Float = x_Left + itemBox_ColumnInterval_Float * 4f;
            float x_Combined_Refining_Mode_Float = x_PercentageSign_Float + itemBox_ColumnInterval_Float * 2f;
            string combined_Refining_Mode_String = GetValue_from_CustomData(ore_Section, combinedMode_Key);
            DrawIcon(ref frame, "MyObjectBuilder_Ore/Stone", x_Combined_Refining_Float, y_7thRow_Float, itemBox_ColumnInterval_Float, itemBox_ColumnInterval_Float, font_Color_Overall);
            PanelWriteText(ref frame, combined_Refining_Mode_String, x_Combined_Refining_Mode_Float, y_PercentageSign_Float, 0.95f, TextAlignment.RIGHT);

            //  AutoProdcution
            float x_AutoProduction_Float = x_Left + itemBox_ColumnInterval_Float * 5f;
            DrawIcon(ref frame, "MyObjectBuilder_PhysicalGunObject/WelderItem", x_AutoProduction_Float, y_7thRow_Float, itemBox_ColumnInterval_Float, itemBox_ColumnInterval_Float, font_Color_Overall);


            //  FunctionalSign
            float width_DangerSign_Float = itemBox_ColumnInterval_Float * 0.7f;
            if (function_ShowItems_Bool == false)
            {
                DrawIcon(ref frame, "Danger", x_Left, y1, width_DangerSign_Float, width_DangerSign_Float, font_Color_Overall);
            }
            if (function_ShowFacilities_Bool == false)
            {
                DrawIcon(ref frame, "Danger", x_Left, y_7thRow_Float, width_DangerSign_Float, width_DangerSign_Float, font_Color_Overall);
            }
            if (function_InventoryManagement_Bool == false)
            {
                DrawIcon(ref frame, "Danger", x_7thRow_2ndColumn_Float, y_7thRow_Float, width_DangerSign_Float, width_DangerSign_Float, font_Color_Overall);
            }
            if (function_BroadCastConnectorGPS_Bool == false)
            {
                DrawIcon(ref frame, "Danger", x_Left, y_6thRow_Float, width_DangerSign_Float, width_DangerSign_Float, font_Color_Overall);
            }
            if (function_ShowCargoContainerRatio_Bool == false)
            {
                DrawIcon(ref frame, "Danger", x_Residues_Float, y_7thRow_Float, width_DangerSign_Float, width_DangerSign_Float, font_Color_Overall);
            }
            if(function_AutoProduction_Bool == false)
            {
                DrawIcon(ref frame, "Danger", x_AutoProduction_Float, y_7thRow_Float, width_DangerSign_Float, width_DangerSign_Float, font_Color_Overall);
            }
        }

        public void ProgressBar(MySpriteDrawFrame frame, float x, float y, float width, float height, string ratio)
        {
            string[] ratiogroup = ratio.Split('%');
            float ratio_Float = Convert.ToSingle(ratiogroup[0]);
            float currentWidth = width * ratio_Float / 100;
            float currentX = x - width / 2 + currentWidth / 2;

            if (ratio_Float == 0) return;

            DrawBox(ref frame, currentX, y, currentWidth, height, progressbar_Color);
        }

        public void DrawLogo(ref MySpriteDrawFrame frame, float x, float y, float width)
        {
            MySprite sprite = new MySprite()
            {
                Type = SpriteType.TEXTURE,
                Data = "Screen_LoadingBar",
                Position = new Vector2(x, y),
                Size = new Vector2(width - 6, width - 6),
                RotationOrScale = Convert.ToSingle(counter_Logo / 360 * 2 * Math.PI),
                Alignment = TextAlignment.CENTER,
                Color = font_Color_Overall,
            };
            frame.Add(sprite);

            sprite = new MySprite()
            {
                Type = SpriteType.TEXTURE,
                Data = "Screen_LoadingBar",
                Position = new Vector2(x, y),
                Size = new Vector2(width / 2, width / 2),
                RotationOrScale = Convert.ToSingle(2 * Math.PI - counter_Logo / 360 * 2 * Math.PI),
                Alignment = TextAlignment.CENTER,
                Color = font_Color_Overall,

            };
            frame.Add(sprite);

            sprite = new MySprite()
            {
                Type = SpriteType.TEXTURE,
                Data = "Screen_LoadingBar",
                Position = new Vector2(x, y),
                Size = new Vector2(width / 4, width / 4),
                RotationOrScale = Convert.ToSingle(Math.PI + counter_Logo / 360 * 2 * Math.PI),
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
            double currentOutput = 0, totalOutput = 0;
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

        public void DrawBox(ref MySpriteDrawFrame frame, float x, float y, float width, float height, Color border_Color, Color background_Color)
        {
            //Echo($"width={width} | height={height}");


            MySprite sprite;

            sprite = MySprite.CreateSprite("SquareSimple", new Vector2(x, y), new Vector2(width - 1, height - 1));
            sprite.Color = border_Color;
            frame.Add(sprite);

            sprite = MySprite.CreateSprite("SquareSimple", new Vector2(x, y), new Vector2(width - 3, height - 3));
            sprite.Color = background_Color;
            frame.Add(sprite);
        }

        public void DrawBox(ref MySpriteDrawFrame frame, float x, float y, float width, float height, Color background_Color)
        {
            MySprite sprite;
            sprite = MySprite.CreateSprite("SquareSimple", new Vector2(x, y), new Vector2(width - 2, height - 2));
            sprite.Color = background_Color;
            frame.Add(sprite);
        }

        public void PanelWriteText(ref MySpriteDrawFrame frame, string text, float x, float y, float fontSize, TextAlignment alignment)
        {
            MySprite sprite = new MySprite()
            {
                Type = SpriteType.TEXT,
                Data = text,
                Position = new Vector2(x, y),
                RotationOrScale = fontSize,
                Color = font_Color_Overall,
                Alignment = alignment,
                FontId = "LoadingScreen"
                //FontId = "Monospace"
            };
            frame.Add(sprite);
        }

        public void PanelWriteText(ref MySpriteDrawFrame frame, string text, float x, float y, float width_Float, float height_Float, float fontSize, TextAlignment alignment)
        {

            float x_Clip_Float = x,
                y_Clip_Float = y;

            switch (alignment)
            {
                case TextAlignment.LEFT:
                    x_Clip_Float = x;
                    break;
                case TextAlignment.CENTER:
                    x_Clip_Float = x - width_Float / 2f;
                    break;
                case TextAlignment.RIGHT:
                    x_Clip_Float = x - width_Float;
                    break;
            }


            using (frame.Clip((int)x_Clip_Float, (int)y_Clip_Float, (int)width_Float, (int)height_Float))
            {
                MySprite sprite = new MySprite()
                {
                    Type = SpriteType.TEXT,
                    Data = text,
                    Position = new Vector2(x, y),
                    RotationOrScale = fontSize,
                    Color = font_Color_Overall,
                    Alignment = alignment,
                    FontId = "LoadingScreen"
                    //FontId = "Monospace"
                };

                frame.Add(sprite);
            }
        }

        public void PanelWriteText(ref MySpriteDrawFrame frame, string text, float x, float y, float fontSize, TextAlignment alignment, Color co)
        {
            MySprite sprite = new MySprite()
            {
                Type = SpriteType.TEXT,
                Data = text,
                Position = new Vector2(x, y),
                RotationOrScale = fontSize,
                Color = co,
                Alignment = alignment,
                FontId = "LoadingScreen"
            };
            frame.Add(sprite);
        }

        public void IGCSignifier(ref MySpriteDrawFrame frame, float x, float y, float width, Color co)
        {

            float x1_Float = x - width / 4f;
            float y_Triangle_Float = y - width * 0.2f;
            float width_Triangle_Float = width * 0.35f;
            float border_Float = 7f;
            float interval_Float = 14f, heightInterval_Float = 16f;
            float height1_Float = width * 0.7f, height2_Float = height1_Float - heightInterval_Float, height3_Float = height2_Float - heightInterval_Float;
            float y_Bar1_Float = y + heightInterval_Float / 2f, y_Bar2_Float = y + heightInterval_Float;

            DrawIcon(ref frame, "Triangle", x1_Float, y_Triangle_Float, width_Triangle_Float, width_Triangle_Float, co, 180f);
            DrawIcon(ref frame, "SquareSimple", x1_Float, y, border_Float, height1_Float, co);
            DrawIcon(ref frame, "SquareSimple", x1_Float + interval_Float * 1f, y_Bar2_Float, border_Float, height3_Float, co);
            DrawIcon(ref frame, "SquareSimple", x1_Float + interval_Float * 2f, y_Bar1_Float, border_Float, height2_Float, co);
            DrawIcon(ref frame, "SquareSimple", x1_Float + interval_Float * 3f, y, border_Float, height1_Float, co);


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

        public void DrawIcon(ref MySpriteDrawFrame frame, string icon, float x, float y, float width, float height, Color picture_Color)
        {
            var sprite = new MySprite
            {
                Type = SpriteType.TEXTURE,
                Data = icon,
                Position = new Vector2(x, y),
                RotationOrScale = 0,
                Size = new Vector2(width, height),
                Color = picture_Color,
                Alignment = TextAlignment.CENTER
            };

            frame.Add(sprite);
        }

        public void DrawIcon(ref MySpriteDrawFrame frame, string icon, float x, float y, float width, float height, Color picture_Color, float rotation)
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

        public void FacilitySignifier(ref MySpriteDrawFrame frame, float x, float y, float width, Color co)
        {
            float width_LargeBox_Float = width * 0.8f;
            float interval_Float = width * 0.145f;
            float y1_Interval_Float = y - width * 0.17f * 0.5f - interval_Float * 0.5f;
            float y2_Interval_Float = y + width * 0.17f * 0.5f + interval_Float * 0.5f;
            float x_Interval_Float = x - width * 0.17f * 0.5f - interval_Float * 0.5f;


            DrawBox(ref frame, x, y, width_LargeBox_Float, width_LargeBox_Float, co);
            DrawBox(ref frame, x, y1_Interval_Float, width_LargeBox_Float, interval_Float, card_Background_Color_Overall);
            DrawBox(ref frame, x, y2_Interval_Float, width_LargeBox_Float, interval_Float, card_Background_Color_Overall);
            DrawBox(ref frame, x_Interval_Float, y, interval_Float, width_LargeBox_Float, card_Background_Color_Overall);

        }

        public void InventorySignifier(ref MySpriteDrawFrame frame, float x, float y, float width, Color co)
        {
            float width_LargeBox_Float = width * 0.8f;
            float height_LargeBox_Float = width_LargeBox_Float * 0.5f;
            float width_SmallBox_Float = width_LargeBox_Float - 16f;
            float height_SmallBox_Float = height_LargeBox_Float - 8f;
            float width_Arrow_Float = height_SmallBox_Float * 1.5f;
            float width_Square_Float = width_Arrow_Float * 0.5f;
            float y_LargeBox_Float = y + height_LargeBox_Float * 0.5f;
            float y_SmallBox_Float = y + height_SmallBox_Float * 0.5f;
            float y_Arraw_Float = y;
            float y_Square_Float = y_Arraw_Float - width_Arrow_Float * 0.5f - width_Square_Float * 0.5f;

            DrawBox(ref frame, x, y_LargeBox_Float, width_LargeBox_Float, height_LargeBox_Float, co);
            DrawBox(ref frame, x, y_SmallBox_Float, width_SmallBox_Float, height_SmallBox_Float, card_Background_Color_Overall);
            DrawIcon(ref frame, "Triangle", x, y_Arraw_Float, width_Arrow_Float, width_Arrow_Float, co, 180f);
            DrawIcon(ref frame, "SquareSimple", x, y_Square_Float, width_Square_Float, width_Square_Float, co);
        }

        public void RefreshRateSignifier(ref MySpriteDrawFrame frame, float x, float y, float width, float border_Float, Color border_Color, Color background_Color)
        {
            x++;
            float width_SingleTriangle_Float = width / 3f;
            float height_SingleTriangle_Float = width * 0.65f;
            float width_InnerSingleTriangle_Float = width_SingleTriangle_Float - border_Float * 2f;
            float height_InnerSingleTriangle_Float = height_SingleTriangle_Float - border_Float * 2f;
            float x_LeftTriangle_Float = x - width_SingleTriangle_Float + 5f;
            float x_RightTriangle_Float = x + width_SingleTriangle_Float - 5f;
            float x_InnerMiddleTriangle_Float = x - 1f;
            float x_InnerRightTriangle_Float = x_RightTriangle_Float - 1f;


            DrawIcon(ref frame, "Triangle", x_LeftTriangle_Float, y, height_SingleTriangle_Float, width_SingleTriangle_Float, border_Color, 90f);
            DrawIcon(ref frame, "Triangle", x, y, height_SingleTriangle_Float, width_SingleTriangle_Float, border_Color, 90f);
            DrawIcon(ref frame, "Triangle", x_RightTriangle_Float, y, height_SingleTriangle_Float, width_SingleTriangle_Float, border_Color, 90f);

            string refreshRate_String = GetValue_from_CustomData(information_Section, refreshRate_Key);

            switch (refreshRate_String)
            {
                case "F":
                    DrawIcon(ref frame, "Triangle", x_InnerMiddleTriangle_Float, y, height_InnerSingleTriangle_Float, width_InnerSingleTriangle_Float, background_Color, 90f);
                    DrawIcon(ref frame, "Triangle", x_InnerRightTriangle_Float, y, height_InnerSingleTriangle_Float, width_InnerSingleTriangle_Float, background_Color, 90f);
                    break;
                case "FF":
                    DrawIcon(ref frame, "Triangle", x_InnerRightTriangle_Float, y, height_InnerSingleTriangle_Float, width_InnerSingleTriangle_Float, background_Color, 90f);
                    break;
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

        public void BuildTranslateDic()
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

            if (itemList.Length > FindMax(index_Array) * itemAmountInEachScreen)
            {
                foreach (var panel in panels_Items)
                {
                    string[] arry = panel.CustomName.Split(':');
                    if (Convert.ToInt16(arry[1]) < FindMax(index_Array))
                    {
                        WriteSinglePanelCustomData(panel, arry[1], true, itemList);
                    }
                    else
                    {
                        WriteSinglePanelCustomData(panel, arry[1], false, itemList);
                    }
                }
            }
            else
            {
                foreach (var panel in panels_Items)
                {
                    string[] arry = panel.CustomName.Split(':');
                    WriteSinglePanelCustomData(panel, arry[1], true, itemList);
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

        public void WriteSinglePanelCustomData(IMyTextPanel panel, string groupNumber, bool isEnoughScreen, ItemList[] itemList)
        {
            panel.WriteText("", false);

            for (int i = 0; i < itemAmountInEachScreen; i++)
            {
                int k = (Convert.ToInt16(groupNumber) - 1) * itemAmountInEachScreen + i;
                int x = (i + 1) % 7;
                if (x == 0) x = 7;
                int y = Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(i + 1) / 7)));

                if (k > itemList.Length - 1)
                {
                    WriteValue_to_CustomData(panel, panelInformation_Section, Amount_Key, i.ToString());
                    return;
                }
                else
                {
                    if (x == 7 && y == 4)
                    {
                        if (isEnoughScreen)
                        {
                            WriteSingleItemCustomData(panel, i + 1, itemList[k]);
                        }
                        else
                        {
                            double residus = itemList.Length - itemAmountInEachScreen * Convert.ToInt16(groupNumber) + 1;
                            WriteTheLastItemCustomData(panel, i + 1, residus);
                        }
                    }
                    else
                    {
                        WriteSingleItemCustomData(panel, i + 1, itemList[k]);
                    }

                    WriteValue_to_CustomData(panel, panelInformation_Section, Amount_Key, (i + 1).ToString());
                    panel.WriteText(itemList[k].Name, true);
                    panel.WriteText("\n", true);

                }
            }
        }

        public void WriteSingleItemCustomData(IMyTextPanel panel, int index_Int, ItemList item_IL)
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
                if(Int64.MaxValue > amount2_Double / efficiency_Double)
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

            WriteValue_to_CustomData(panel, index_Int.ToString(), itemType_Key, itemType_String);
            WriteValue_to_CustomData(panel, index_Int.ToString(), itemAmount1_Key, amount1_Double.ToString());
            WriteValue_to_CustomData(panel, index_Int.ToString(), itemAmount2_Key, amount2_Double.ToString());
            WriteValue_to_CustomData(panel, index_Int.ToString(), time_Key, time_String);
            WriteValue_to_CustomData(panel, index_Int.ToString(), time1_Key, time1_DT.ToString());
            WriteValue_to_CustomData(panel, index_Int.ToString(), time2_Key, time2_DT.ToString());
            WriteValue_to_CustomData(panel, index_Int.ToString(), productionAmount_Key, productionAmount_Double.ToString());
        }

        public void WriteTheLastItemCustomData(IMyTextPanel panel, int index_Int, double residue_Double)
        {
            WriteValue_to_CustomData(panel, index_Int.ToString(), itemType_Key, "AH_BoreSight");
            WriteValue_to_CustomData(panel, index_Int.ToString(), itemAmount2_Key, residue_Double.ToString());
            WriteValue_to_CustomData(panel, index_Int.ToString(), time_Key, "");
            WriteValue_to_CustomData(panel, index_Int.ToString(), productionAmount_Key, "0");
        }

        public void DrawItemPanels()
        {
            foreach (var panel in panels_Items_All) DrawSinglePanel(panel, progressbar_Color, card_Background_Color_Overall);
            foreach (var panel in panels_Items_Ore) DrawSinglePanel(panel, oreCard_Background_Color, ore_Background_Color);
            foreach (var panel in panels_Items_Ingot) DrawSinglePanel(panel, ingotCard_Background_Color, ingot_Background_Color);
            foreach (var panel in panels_Items_Component) DrawSinglePanel(panel, progressbar_Color, card_Background_Color_Overall);
            foreach (var panel in panels_Items_AmmoMagazine) DrawSinglePanel(panel, progressbar_Color, card_Background_Color_Overall);
        }

        public void DrawSinglePanel(IMyTextPanel panel, Color cardColor, Color backgroundColor)
        {
            panel.ContentType = ContentType.SCRIPT;
            panel.ScriptBackgroundColor = card_Background_Color_Overall;
            MySpriteDrawFrame frame = panel.DrawFrame();

            string refreshCounter_String = GetValue_from_CustomData(panel, panelInformation_Section, counter_Key);
            int indexMax_Int = Convert.ToInt16(GetValue_from_CustomData(panel, panelInformation_Section, Amount_Key));

            if (refreshCounter_String == null || refreshCounter_String != "0")
            {
                WriteValue_to_CustomData(panel, panelInformation_Section, counter_Key, "0");
                refreshCounter_String = "0";
            }
            else WriteValue_to_CustomData(panel, panelInformation_Section, counter_Key, "1");

            float refreshCounter_Float = Convert.ToInt16(refreshCounter_String);

            DrawBox(ref frame, 512 / 2, 512 / 2 + refreshCounter_Float, 520, 520, backgroundColor);

            for (int i = 0; i < itemAmountInEachScreen; i++)
            {
                int x = (i + 1) % 7;
                if (x == 0) x = 7;
                int y = Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(Convert.ToDouble(i + 1) / 7)));

                if (i > indexMax_Int - 1) break;
                else DrawSingleItemUnit(panel, ref frame, i + 1, x, y, cardColor);
            }

            frame.Dispose();
        }

        public void DrawSingleItemUnit(IMyTextPanel panel, ref MySpriteDrawFrame frame, int index_Int, float x, float y, Color cardColor)
        {
            string itemName_String = GetValue_from_CustomData(panel, index_Int.ToString(), itemType_Key);
            double amount_Double = Convert.ToDouble(GetValue_from_CustomData(panel, index_Int.ToString(), itemAmount2_Key));
            string time_String = GetValue_from_CustomData(panel, index_Int.ToString(), time_Key);
            double amount_Production_Double = Convert.ToDouble(GetValue_from_CustomData(panel, index_Int.ToString(), productionAmount_Key));


            //  Main box
            float refreshCounter_Float = Convert.ToSingle(GetValue_from_CustomData(panel, panelInformation_Section, counter_Key));
            float x1 = Convert.ToSingle((x - 1) * itemBox_ColumnInterval_Float + (itemBox_ColumnInterval_Float - 1) / 2 + 1.25f);
            float y1 = Convert.ToSingle((y - 1) * (itemBox_RowInterval_Float + 1) + (itemBox_RowInterval_Float) / 2 + 2f) + refreshCounter_Float;
            DrawBox(ref frame, x1, y1, itemBox_ColumnInterval_Float, itemBox_RowInterval_Float, cardColor);

            //  Name text
            float x_Name = x1 - (itemBox_ColumnInterval_Float - 3) / 2 + 1;
            float y_Name = y1 - (itemBox_RowInterval_Float - 3) / 2 + 1;
            PanelWriteText(ref frame, TranslateName(itemName_String), x_Name, y_Name, itemBox_ColumnInterval_Float - 3f, 15f, 0.55f, TextAlignment.LEFT);

            //  Picture box
            float y_Picture_Float = y_Name + 12 + itemBox_ColumnInterval_Float / 2f;
            MySprite sprite = MySprite.CreateSprite(itemName_String, new Vector2(x1, y_Picture_Float), new Vector2(itemBox_ColumnInterval_Float - 2, itemBox_ColumnInterval_Float - 2));
            frame.Add(sprite);

            //  Amount text
            float x_Text_Amount = x1 + (itemBox_ColumnInterval_Float - 3) / 2 - 1;
            float y_Text_Amount = y_Picture_Float + itemBox_ColumnInterval_Float / 2f - 6f;
            PanelWriteText(ref frame, AmountUnitConversion(amount_Double / 1000000, false), x_Text_Amount, y_Text_Amount, 0.8f, TextAlignment.RIGHT);

            //  AutoProductionAmount text
            float y_Text_Production_Amount = y_Text_Amount - 12f;
            if (amount_Production_Double != 0)
            {
                PanelWriteText(ref frame, AmountUnitConversion(amount_Production_Double / 1000000, false), x_Text_Amount, y_Text_Production_Amount, 0.5f, TextAlignment.RIGHT);
            }

            //  Time remaining
            float y_TimeRemaining_Float = y_Text_Amount + 25f;
            PanelWriteText(ref frame, time_String, x_Text_Amount, y_TimeRemaining_Float, itemBox_ColumnInterval_Float - 4f, 15f, 0.57f, TextAlignment.RIGHT);
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
                if (refineries.Count > 0) FacilitiesDivideIntoGroup(refineryList, panels_Refineries, maxNumber_RefineryPanel_Int, ore_Background_Color, oreCard_Background_Color);
            }
            else
            {
                Echo("Ass");
                counter_Panel_Int = counter_ShowFacilities_Int - panels_Refineries.Count - 3;
                if (assemblers.Count > 0) FacilitiesDivideIntoGroup(assemblerList, panels_Assemblers, maxNumber_AssemblerPanel_Int, ingot_Background_Color, ingotCard_Background_Color);
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

        public void FacilitiesDivideIntoGroup(Facility_Struct[] facilityList, List<IMyTextPanel> facilityPanels, int maxNumber_Panel_Int, Color borderColor, Color backgroundColor)
        {
            if (facilityList.Length == 0 || facilityPanels.Count == 0) return;

            Echo($"{counter_Panel_Int + 1}/{facilityPanels.Count}");

            if (facilityList.Length > maxNumber_Panel_Int * facilityAmountInEachScreen)
            {
                //  Not enough panel
                var panel = facilityPanels[counter_Panel_Int];

                if (panel.CustomData != "0") panel.CustomData = "0";
                else panel.CustomData = "1";

                if (panel.ContentType != ContentType.SCRIPT) panel.ContentType = ContentType.SCRIPT;
                MySpriteDrawFrame frame = panel.DrawFrame();
                string[] arry = panel.CustomName.Split(':');
                if (Convert.ToInt16(arry[1]) < maxNumber_Panel_Int)
                {
                    DrawFullFacilityScreen(panel, ref frame, arry[1], true, facilityList, borderColor, backgroundColor);
                }
                else
                {
                    DrawFullFacilityScreen(panel, ref frame, arry[1], false, facilityList, borderColor, backgroundColor);
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

                if (panel.ContentType != ContentType.SCRIPT) panel.ContentType = ContentType.SCRIPT;
                MySpriteDrawFrame frame = panel.DrawFrame();
                string[] arry = panel.CustomName.Split(':');
                DrawFullFacilityScreen(panel, ref frame, arry[1], true, facilityList, borderColor, backgroundColor);
                frame.Dispose();

                Echo(panel.CustomName);
            }


        }

        public void DrawFullFacilityScreen(IMyTextPanel panel, ref MySpriteDrawFrame frame, string groupNumber, bool isEnoughScreen, Facility_Struct[] facilityList, Color borderColor, Color backgroundColor)
        {
            panel.WriteText("", false);

            DrawFacilityScreenFrame(panel, ref frame, borderColor, backgroundColor);

            for (int i = 0; i < facilityAmountInEachScreen; i++)
            {
                int k = (Convert.ToInt16(groupNumber) - 1) * facilityAmountInEachScreen + i;

                if (k > facilityList.Length - 1) return;//Last facility is finished.

                if (i == facilityAmountInEachScreen - 1)
                {
                    if (isEnoughScreen)
                    {
                        DrawSingleFacilityUnit(panel, ref frame, (k + 1).ToString() + ". " + facilityList[k].Name + " ×" + facilityList[k].Productivity + "%", facilityList[k].IsProducing_Bool, AmountUnitConversion(facilityList[k].ItemAmount, false), facilityList[k].Picture, facilityList[k].IsRepeatMode_Bool, facilityList[k].IsCooperativeMode_Bool, facilityList[k].IsEnabled_Bool, i);
                    }
                    else
                    {
                        double residus = facilityList.Length - facilityAmountInEachScreen * Convert.ToInt16(groupNumber) + 1;
                        DrawSingleFacilityUnit(panel, ref frame, "+ " + residus.ToString() + " Facilities", false, "0", "Empty", false, false, false, i);
                    }
                }
                else
                {
                    DrawSingleFacilityUnit(panel, ref frame, (k + 1).ToString() + ". " + facilityList[k].Name + " ×" + facilityList[k].Productivity + "%", facilityList[k].IsProducing_Bool, AmountUnitConversion(facilityList[k].ItemAmount, false), facilityList[k].Picture, facilityList[k].IsRepeatMode_Bool, facilityList[k].IsCooperativeMode_Bool, facilityList[k].IsEnabled_Bool, i);
                }

                panel.WriteText($"{(k + 1).ToString() + ".\n"}{facilityList[k].Name}", true);
                panel.WriteText($"\n{facilityList[k].Picture}", true);
                panel.WriteText("\n\n", true);
            }
        }

        public void DrawFacilityScreenFrame(IMyTextPanel panel, ref MySpriteDrawFrame frame, Color borderColor, Color backgroundColor)
        {
            float lineWith_Float = 1f, screenCenter_Float = 512 / 2;

            //DrawBox(frame, 512 / 2, 512 / 2 + Convert.ToSingle(panel.CustomData), 514, 514, Color.Black);
            DrawBox(ref frame, screenCenter_Float, screenCenter_Float + Convert.ToSingle(panel.CustomData), 514, 514, backgroundColor);

            for (int i = 0; i <= 20; i++)
            {
                DrawBox(ref frame, screenCenter_Float, 1 + facilityBox_RowInterval_Float * i, 512, lineWith_Float, borderColor);
            }

            float x1 = 1, x2 = x1 + 92, x3 = x2 + facilityBox_RowInterval_Float, x4 = x3 + facilityBox_RowInterval_Float, x5 = 512, x31 = (x3 + x4) / 2;
            float[] linePosition_X_Float = { x1, x2, x3, x4, x5 };
            foreach (var x in linePosition_X_Float)
            {
                DrawBox(ref frame, x, screenCenter_Float, lineWith_Float, 512, borderColor);
            }
        }

        public void DrawSingleFacilityUnit(IMyTextPanel panel, ref MySpriteDrawFrame frame, string Name, bool isProducing, string itemAmount, string picture, bool isRepeating, bool isCooperative, bool isEnabled, int index)
        {
            //  ItemAmount box
            float h = facilityBox_RowInterval_Float;
            float width = 92f;
            float x1 = Convert.ToSingle(1 + width / 2);
            float y1 = Convert.ToSingle(1 + h / 2 + h * index) + Convert.ToSingle(panel.CustomData);
            float textY = y1 - h / 2 + 2F, textSize = 0.75f;
            //DrawBox(frame, x1, y1, width, h, background_Color);
            if (isRepeating) PanelWriteText(ref frame, "RE", x1 - width / 2 + 2f, textY, textSize, TextAlignment.LEFT);
            PanelWriteText(ref frame, itemAmount, x1 + width / 2 - 2f, textY, textSize, TextAlignment.RIGHT);

            //  picture box
            float x2 = x1 + width / 2 + h / 2 + 0.5f;
            float boxWH_Float = h - 1;
            //DrawBox(frame, x2, y1, h, h, background_Color);
            MySprite sprite;
            if (picture != "Empty")
            {
                sprite = MySprite.CreateSprite(TranslateSpriteName(picture), new Vector2(x2, y1), new Vector2(boxWH_Float, boxWH_Float));
                frame.Add(sprite);
            }

            //  isproduction box
            float x3 = x2 + h - 0.5f;
            if (isEnabled)
            {
                if (isProducing) DrawBox(ref frame, x3, y1, boxWH_Float, boxWH_Float, new Color(0, 140, 0));
                else DrawBox(ref frame, x3, y1, boxWH_Float, boxWH_Float, new Color(130, 100, 0));
            }
            else
            {
                DrawBox(ref frame, x3, y1, boxWH_Float, boxWH_Float, new Color(178, 9, 9));
            }
            if (isCooperative) DrawImage(frame, "Circle", x3, y1, h - 7, new Color(0, 0, 255));


            //  name box
            float nameBoxWidth = Convert.ToSingle((512 - x3 - h / 2) - 2);
            float x4 = x3 + h / 2 + nameBoxWidth / 2 + 0.5f;
            //DrawBox(frame, x4, y1, nameBoxWidth, h, background_Color);
            PanelWriteText(ref frame, Name, x4 - nameBoxWidth / 2 + 1f, textY, textSize, TextAlignment.LEFT);
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

        public void DrawImage(MySpriteDrawFrame frame, string name, float x, float y, float width, Color co)
        {
            MySprite sprite = new MySprite()
            {
                Type = SpriteType.TEXTURE,
                Data = name,
                Position = new Vector2(x, y),
                Size = new Vector2(width - 6, width - 6),
                Color = co,
                Alignment = TextAlignment.CENTER,
            };
            frame.Add(sprite);
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

            for (int i = 0; i <= 9; i++)
            {
                var cargoContainer = cargoContainers[counter_CargoContainer_Int - 1];
                string newName_String = GetValue_from_CustomData(cargoContainer, information_Section, customName_Key);
                if (newName_String == "")
                {
                    newName_String = cargoContainer.CustomName;
                    WriteValue_to_CustomData(cargoContainer, information_Section, customName_Key, newName_String);
                }

                double ratio_Double = (double)cargoContainer.GetInventory().CurrentVolume / (double)cargoContainer.GetInventory().MaxVolume;
                ratio_Double = Math.Round(ratio_Double * 100, 1);

                cargoContainer.CustomName = newName_String + "__" + ratio_Double.ToString() + "%";

                if (counter_CargoContainer_Int >= cargoContainers.Count)
                {
                    counter_CargoContainer_Int = 1;
                    stage_Key = nextStage;
                    return;
                }
                counter_CargoContainer_Int++;

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

        public void BuildProductionList()
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
            int i = 1;
            foreach (var block in blocks)
            {
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
