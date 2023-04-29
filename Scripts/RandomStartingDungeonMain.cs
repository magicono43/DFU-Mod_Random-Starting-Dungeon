// Project:         RandomStartingDungeon mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2023 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    8/12/2020, 5:05 PM
// Last Edit:		4/29/2023, 9:50 AM
// Version:			1.12
// Special Thanks:  Jehuty, TheLacus, Hazelnut
// Modifier:

using DaggerfallConnect;
using DaggerfallConnect.Arena2;
using DaggerfallConnect.Utility;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;
using DaggerfallWorkshop.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using Wenzil.Console;

namespace RandomStartingDungeon
{
    public class RandomStartingDungeonMain : MonoBehaviour
	{
        static RandomStartingDungeonMain Instance;

        static Mod mod;

        // Region Specific and Quest Dungeon Options
        public static bool QuestDungStartCheck { get; set; }
        public static bool IsolatedIslandStartCheck { get; set; }
        public static bool PopulatedIslandStartCheck { get; set; }

        // Dungeon Location Climate Options
        public static bool OceanStartCheck { get; set; }
        public static bool DesertStartCheck { get; set; }
        public static bool DesertHotStartCheck { get; set; }
        public static bool MountainStartCheck { get; set; }
        public static bool RainforestStartCheck { get; set; }
        public static bool SwampStartCheck { get; set; }
        public static bool SubtropicalStartCheck { get; set; }
        public static bool MountainWoodsStartCheck { get; set; }
        public static bool WoodlandsStartCheck { get; set; }
        public static bool HauntedWoodlandsStartCheck { get; set; }

        // Dungeon Type Options
        public static bool ScorpionNestStartCheck { get; set; }
        public static bool VolcanicCavesStartCheck { get; set; }
        public static bool BarbarianStrongholdStartCheck { get; set; }
        public static bool DragonsDenStartCheck { get; set; }
        public static bool GiantStrongholdStartCheck { get; set; }
        public static bool SpiderNestStartCheck { get; set; }
        public static bool RuinedCastleStartCheck { get; set; }
        public static bool HarpyNestStartCheck { get; set; }
        public static bool LaboratoryStartCheck { get; set; }
        public static bool VampireHauntStartCheck { get; set; }
        public static bool CovenStartCheck { get; set; }
        public static bool NaturalCaveStartCheck { get; set; }
        public static bool MineStartCheck { get; set; }
        public static bool DesecratedTempleStartCheck { get; set; }
        public static bool PrisonStartCheck { get; set; }
        public static bool HumanStrongholdStartCheck { get; set; }
        public static bool OrcStrongholdStartCheck { get; set; }
        public static bool CryptStartCheck { get; set; }
        public static bool CemeteryStartCheck { get; set; }

        // Start Date Options
        public static bool RandomStartDateCheck { get; set; }
        public static bool Winter { get; set; }
        public static bool Spring { get; set; }
        public static bool Summer { get; set; }
        public static bool Fall { get; set; }

        // Misc Options
        public static int SafeZoneSizeSetting { get; set; }

        // General "Global" Variables
        public static bool AlreadyRolled { get; set; }
        public static Dictionary<int, int[]> QuickRerollDictionary { get; set; }
        public static List<int> QuickRerollValidRegions { get; set; }
        const int editorFlatArchive = 199;
        const int spawnMarkerFlatIndex = 11;
        const int itemMarkerFlatIndex = 18;
        public static SpawnPoints SpawnPointGlobal { get; set; }
        public static DFLocation DungLocationGlobal { get; set; }

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            mod = initParams.Mod;
            var go = new GameObject(mod.Title);
            go.AddComponent<RandomStartingDungeonMain>(); // Add script to the scene.

            mod.LoadSettingsCallback = LoadSettings; // To enable use of the "live settings changes" feature in-game.

            mod.IsReady = true;
        }

        private void Start()
        {
            Debug.Log("Begin mod init: Random Starting Dungeon");

            Instance = this;

            mod.LoadSettings();

            StartGameBehaviour.OnStartGame += RandomizeSpawn_OnStartGame;

            RegisterRandomStartingDungeonCommands();

            Debug.Log("Finished mod init: Random Starting Dungeon");
        }

        private static void LoadSettings(ModSettings modSettings, ModSettingsChange change)
        {
            // Region Specific and Quest Dungeon Options
            QuestDungStartCheck = mod.GetSettings().GetValue<bool>("Quest&IslandOptions", "QuestDungeons");
            IsolatedIslandStartCheck = mod.GetSettings().GetValue<bool>("Quest&IslandOptions", "IsolatedIslandDungeons");
            PopulatedIslandStartCheck = mod.GetSettings().GetValue<bool>("Quest&IslandOptions", "PopulatedIslandDungeons");

            // Dungeon Location Climate Options
            OceanStartCheck = mod.GetSettings().GetValue<bool>("ClimateOptions", "Ocean");
            DesertStartCheck = mod.GetSettings().GetValue<bool>("ClimateOptions", "Desert");
            DesertHotStartCheck = mod.GetSettings().GetValue<bool>("ClimateOptions", "HotDesert");
            MountainStartCheck = mod.GetSettings().GetValue<bool>("ClimateOptions", "Mountain");
            RainforestStartCheck = mod.GetSettings().GetValue<bool>("ClimateOptions", "Rainforest");
            SwampStartCheck = mod.GetSettings().GetValue<bool>("ClimateOptions", "Swamp");
            SubtropicalStartCheck = mod.GetSettings().GetValue<bool>("ClimateOptions", "Subtropical");
            MountainWoodsStartCheck = mod.GetSettings().GetValue<bool>("ClimateOptions", "MountainWoods");
            WoodlandsStartCheck = mod.GetSettings().GetValue<bool>("ClimateOptions", "Woodlands");
            HauntedWoodlandsStartCheck = mod.GetSettings().GetValue<bool>("ClimateOptions", "HauntedWoodlands");

            // Dungeon Type Options
            ScorpionNestStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "ScorpionNest");
            VolcanicCavesStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "VolcanicCaves");
            BarbarianStrongholdStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "BarbarianStronghold");
            DragonsDenStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "DragonsDen");
            GiantStrongholdStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "GiantStronghold");
            SpiderNestStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "SpiderNest");
            RuinedCastleStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "RuinedCastle");
            HarpyNestStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "HarpyNest");
            LaboratoryStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "Laboratory");
            VampireHauntStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "VampireHaunt");
            CovenStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "Coven");
            NaturalCaveStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "NaturalCave");
            MineStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "Mine");
            DesecratedTempleStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "DesecratedTemple");
            PrisonStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "Prison");
            HumanStrongholdStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "HumanStronghold");
            OrcStrongholdStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "OrcStronghold");
            CryptStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "Crypt");
            CemeteryStartCheck = mod.GetSettings().GetValue<bool>("DungeonTypeOptions", "Cemetery");

            // Starting Date Options
            RandomStartDateCheck = mod.GetSettings().GetValue<bool>("StartDateOptions", "RandomStartingDate");
            Winter = mod.GetSettings().GetValue<bool>("StartDateOptions", "WinterMonths");
            Spring = mod.GetSettings().GetValue<bool>("StartDateOptions", "SpringMonths");
            Summer = mod.GetSettings().GetValue<bool>("StartDateOptions", "SummerMonths");
            Fall = mod.GetSettings().GetValue<bool>("StartDateOptions", "FallMonths");

            // Misc Options
            SafeZoneSizeSetting = mod.GetSettings().GetValue<int>("MiscOptions", "SafeZone");

            AlreadyRolled = false;
        }

        public static void RegisterRandomStartingDungeonCommands()
        {
            Debug.Log("[RandomStartingDungeon] Trying to register console commands.");
            try
            {
                ConsoleCommandsDatabase.RegisterCommand(ManualRandomTeleport.name, ManualRandomTeleport.description, ManualRandomTeleport.usage, ManualRandomTeleport.Execute);
                ConsoleCommandsDatabase.RegisterCommand(TransformDungPos.name, TransformDungPos.description, TransformDungPos.usage, TransformDungPos.Execute);
                ConsoleCommandsDatabase.RegisterCommand(CurrentBlockInfo.name, CurrentBlockInfo.description, CurrentBlockInfo.usage, CurrentBlockInfo.Execute);
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("Error Registering RandomStartingDungeon Console commands: {0}", e.Message));
            }
        }

        private static class ManualRandomTeleport
        {
            public static readonly string name = "manual_random_teleport";
            public static readonly string description = "Randomly Teleport To Dungeon Based On Current Sessions Options";
            public static readonly string usage = "Randomly Teleport To Dungeon";

            public static string Execute(params string[] args)
            {
                PickRandomDungeonTeleport();

                return "Teleporting To Random Dungeon Now...";
            }
        }

        private static class TransformDungPos
        {
            public static readonly string name = "transform_dung_pos";
            public static readonly string description = "Randomly Transform Position Of Player Inside Dungeon";
            public static readonly string usage = "Randomly Transform Player Position In Dungeon";

            public static string Execute(params string[] args)
            {
                bool successCheck = TransformPlayerPosition();

                if (successCheck)
                    return "Transforming Player Dungeon Position...";
                else
                    return "Transformation Failed, Could Not Find Valid Dungeon Position.";
            }
        }

        private static class CurrentBlockInfo
        {
            public static readonly string name = "current_block_info";
            public static readonly string description = "Display The Block Info Of Block Player Is Currently Standing In";
            public static readonly string usage = "Display The Current Block Info";

            public static string Execute(params string[] args)
            {
                FindCurrentBlockInfo();

                return "Current Block Info Displayed...";
            }
        }

        #region Methods and Functions

        public static void RandomizeSpawn_OnStartGame(object sender, EventArgs e)
        {
            TextFile.Token[] tokens = DaggerfallUnity.Instance.TextProvider.CreateTokens(
                        TextFile.Formatting.JustifyCenter,
                        "Do you want to be sent to a random dungeon?",
                        "",
                        "(If Yes, expect 1-2 minutes of unresponsiveness",
                        "while initial location filtering is processed.)");
            DaggerfallMessageBox randomStartConfirmBox = new DaggerfallMessageBox(DaggerfallUI.UIManager);

            randomStartConfirmBox.SetTextTokens(tokens);
            randomStartConfirmBox.AddButton(DaggerfallMessageBox.MessageBoxButtons.Yes);
            randomStartConfirmBox.AddButton(DaggerfallMessageBox.MessageBoxButtons.No);
            randomStartConfirmBox.OnButtonClick += ConfirmRandomStart_OnButtonClick;
            DaggerfallUI.UIManager.PushWindow(randomStartConfirmBox);

            if (RandomStartDateCheck) // If option is enabled at all.
            {
                int time = 0;
                if (!Winter && !Spring && !Summer && !Fall) {} // Do Nothing                                                                                                                                          0000
                else if (!Winter && !Spring && !Summer && Fall) { time = UnityEngine.Random.Range(237, 311); } // Roll between Fall months                                                                            0001
                else if (!Winter && !Spring && Summer && !Fall) { time = UnityEngine.Random.Range(147, 221); } // Roll between Summer months                                                                          0010
                else if (!Winter && !Spring && Summer && Fall) { time = UnityEngine.Random.Range(147, 321); } // Roll between Summer and Fall months                                                                  0011
                else if (!Winter && Spring && !Summer && !Fall) { time = UnityEngine.Random.Range(57, 131); } // Roll between Spring months                                                                           0100
                else if (!Winter && Spring && !Summer && Fall) { time = Dice100.SuccessRoll(50) ? UnityEngine.Random.Range(57, 131) : UnityEngine.Random.Range(237, 311); } // Roll between Spring or Fall months     0101
                else if (!Winter && Spring && Summer && !Fall) { time = UnityEngine.Random.Range(57, 231); } // Roll between Spring and Summer months                                                                 0110
                else if (!Winter && Spring && Summer && Fall) { time = UnityEngine.Random.Range(57, 321); } // Roll between Spring and Fall months                                                                    0111
                else if (Winter && !Spring && !Summer && !Fall) { time = UnityEngine.Random.Range(327, 401); } // Roll between Winter months                                                                          1000
                else if (Winter && !Spring && !Summer && Fall) { time = UnityEngine.Random.Range(237, 411); } // Roll between Winter and Fall months                                                                  1001
                else if (Winter && !Spring && Summer && !Fall) { time = Dice100.SuccessRoll(50) ? UnityEngine.Random.Range(327, 401) : UnityEngine.Random.Range(147, 221); } // Roll between Winter or Summer months  1010
                else if (Winter && !Spring && Summer && Fall) { time = UnityEngine.Random.Range(147, 411); } // Roll between Winter and Summer and Fall months                                                        1011
                else if (Winter && Spring && !Summer && !Fall) { time = UnityEngine.Random.Range(327, 501); } // Roll between Winter and Spring months                                                                1100
                else if (Winter && Spring && !Summer && Fall) { time = UnityEngine.Random.Range(237, 501); } // Roll between Winter and Spring and Fall months                                                        1101
                else if (Winter && Spring && Summer && !Fall) { time = UnityEngine.Random.Range(327, 591); } // Roll between Winter and Summer months                                                                 1110
                else { time = UnityEngine.Random.Range(1, 360); } // Roll between Any months                                                                                                                          1111

                Debug.LogFormat("Time Increase By {0} Days!", time);
                time = time * 86400; // time = number of days, 86400 = seconds in a day.
                DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.RaiseTime(time);
            }
        }

        public static void ConfirmRandomStart_OnButtonClick(DaggerfallMessageBox sender, DaggerfallMessageBox.MessageBoxButtons messageBoxButton)
        {
            sender.CloseWindow();
            if (messageBoxButton == DaggerfallMessageBox.MessageBoxButtons.Yes)
            {
                PickRandomDungeonTeleport();
            }
            else if (messageBoxButton == DaggerfallMessageBox.MessageBoxButtons.No)
            {
                return;
            }
        }

        public static void PickRandomDungeonTeleport()
        {
            DFRegion regionInfo = new DFRegion();
            int[] foundIndices = new int[0];
            List<int> validRegionIndexes = new List<int>();
            Dictionary<int, int[]> regionValidDungGrabBag = new Dictionary<int, int[]>();
			regionValidDungGrabBag.Clear(); // Attempts to clear dictionary to keep from compile errors about duplicate keys.

            if (!AlreadyRolled)
            {
                for (int n = 0; n < 62; n++)
                {
                    regionInfo = DaggerfallUnity.Instance.ContentReader.MapFileReader.GetRegion(n);
                    if (regionInfo.LocationCount <= 0) // Add the if-statements to keep "invalid" regions from being put into grab-bag, also use this for some settings.
                        continue;
                    if (n == 31) // Index for "High Rock sea coast" or the "region" that holds the location of the two player boats, as well as the Mantellan Crux story dungeon.
                        continue;
                    if (!IsolatedIslandStartCheck && n == 61) // Index for "Cybiades" the isolated region that has only one single location on the whole island, that being a dungeon.
                        continue;
                    // Get indices for all dungeons of this type
                    foundIndices = CollectDungeonIndicesOfType(regionInfo, n);
                    if (foundIndices.Length == 0)
                        continue;

                    regionValidDungGrabBag.Add(n, foundIndices);
                    validRegionIndexes.Add(n);
                    AlreadyRolled = true; // Too keep this code-block from reprocessing every-time this function is ran again in the same play session.
                    QuickRerollDictionary = regionValidDungGrabBag;
                    QuickRerollValidRegions = validRegionIndexes;
                }
            }

            if (!AlreadyRolled)
            {
                if (validRegionIndexes.Count > 0)
                {
                    int randomRegionIndex = RandomRegionRoller(validRegionIndexes.ToArray());
                    foundIndices = regionValidDungGrabBag[randomRegionIndex];

                    // Select a random dungeon location index from available list then get its location data
                    int RandDungIndex = UnityEngine.Random.Range(0, foundIndices.Length);
                    DFLocation dungLocation = DaggerfallUnity.Instance.ContentReader.MapFileReader.GetLocation(randomRegionIndex, foundIndices[RandDungIndex]);

                    // Spawn inside dungeon at this world position
                    DFPosition mapPixel = MapsFile.LongitudeLatitudeToMapPixel((int)dungLocation.MapTableData.Longitude, dungLocation.MapTableData.Latitude);
                    DFPosition worldPos = MapsFile.MapPixelToWorldCoord(mapPixel.X, mapPixel.Y);
                    GameManager.Instance.PlayerEnterExit.RespawnPlayer(
                        worldPos.X,
                        worldPos.Y,
                        true,
                        true);

                    regionInfo = DaggerfallUnity.Instance.ContentReader.MapFileReader.GetRegion(randomRegionIndex);
                    Debug.LogFormat("Random Region Index # {0} has {1} locations = {2} and {3} of those are valid dungeons", randomRegionIndex, regionInfo.LocationCount, regionInfo.Name, foundIndices.Length);
                    Debug.LogFormat("Random Dungeon Index # {0} in the Region {1} = {2}, Dungeon Type is a {3}", RandDungIndex, regionInfo.Name, dungLocation.Name, dungLocation.MapTableData.DungeonType.ToString());
                }
                else
                {
                    Debug.Log("No Valid Dungeon Locations To Teleport To, Try Making Your Settings Less Strict.");
                }
            }
            else
            {
                if (QuickRerollValidRegions.Count > 0)
                {
                    int randomRegionIndex = RandomRegionRoller(QuickRerollValidRegions.ToArray());
                    foundIndices = QuickRerollDictionary[randomRegionIndex];

                    // Select a random dungeon location index from available list then get its location data
                    int RandDungIndex = UnityEngine.Random.Range(0, foundIndices.Length);
                    DFLocation dungLocation = DaggerfallUnity.Instance.ContentReader.MapFileReader.GetLocation(randomRegionIndex, foundIndices[RandDungIndex]);
                    DungLocationGlobal = dungLocation;

                    SpawnPoints[] SpawnLocations = RandomBlockLocationPicker(dungLocation);
                    if (SpawnLocations != null)
                    {
                        int RandSpawnIndex = UnityEngine.Random.Range(0, SpawnLocations.Length);
                        SpawnPoints spawnPoint = new SpawnPoints(SpawnLocations[RandSpawnIndex].flatPosition, SpawnLocations[RandSpawnIndex].dungeonX, SpawnLocations[RandSpawnIndex].dungeonZ);
                        spawnPoint.flatPosition = SpawnLocations[RandSpawnIndex].flatPosition;
                        spawnPoint.dungeonX = SpawnLocations[RandSpawnIndex].dungeonX;
                        spawnPoint.dungeonZ = SpawnLocations[RandSpawnIndex].dungeonZ;
                        SpawnPointGlobal = spawnPoint;
                    }
                    else
                        Debug.Log("Transformation Failed, Could Not Find Valid Dungeon Position.");

                    // Spawn inside dungeon at this world position
                    DFPosition mapPixel = MapsFile.LongitudeLatitudeToMapPixel(dungLocation.MapTableData.Longitude, dungLocation.MapTableData.Latitude);
                    DFPosition worldPos = MapsFile.MapPixelToWorldCoord(mapPixel.X, mapPixel.Y);
                    GameManager.Instance.PlayerEnterExit.RespawnPlayer(
                        worldPos.X,
                        worldPos.Y,
                        true,
                        true);

                    regionInfo = DaggerfallUnity.Instance.ContentReader.MapFileReader.GetRegion(randomRegionIndex);
                    Debug.LogFormat("Random Region Index # {0} has {1} locations = {2} and {3} of those are valid dungeons", randomRegionIndex, regionInfo.LocationCount, regionInfo.Name, foundIndices.Length);
                    Debug.LogFormat("Random Dungeon Index # {0} in the Region {1} = {2}, Dungeon Type is a {3}", RandDungIndex, regionInfo.Name, dungLocation.Name, dungLocation.MapTableData.DungeonType.ToString());
                }
                else
                {
                    Debug.Log("No Valid Dungeon Locations To Teleport To, Try Making Your Settings Less Strict.");
                }
                PlayerEnterExit.OnRespawnerComplete += TeleToSpawnPoint_OnRespawnerComplete;
            }
			// Likely in a later version of this mod, make a menu system similar to the Skyrim Mod "Live Another Life" for the options and background settings possibly of a new character.
            // Also for that "Live Another Life" version, likely add towns/homes/cities, etc to the list of places that can be randomly teleported and brought to and such.
        }

        public static void TeleToSpawnPoint_OnRespawnerComplete()
        {
            if (GameManager.Instance.PlayerEnterExit.IsPlayerInsideDungeon && AlreadyRolled) // This will defintiely have to be changed, for logic with more discrimination on when it runs.
            {
                bool successCheck = TransformPlayerPosition();

                if (!successCheck)
                    DaggerfallUI.AddHUDText("Transformation Failed, Could Not Find Valid Dungeon Position.", 6.00f);


                GameObject player = GameManager.Instance.PlayerObject;
                PlayerEntity playerEntity = player.GetComponent<DaggerfallEntityBehaviour>().Entity as PlayerEntity;
                DaggerfallEntityBehaviour[] entityBehaviours = FindObjectsOfType<DaggerfallEntityBehaviour>();

                if (player != null)
                {
                    for (int i = 0; i < entityBehaviours.Length; i++)
                    {
                        DaggerfallEntityBehaviour entityBehaviour = entityBehaviours[i];
                        if (entityBehaviour.EntityType == EntityTypes.EnemyMonster || entityBehaviour.EntityType == EntityTypes.EnemyClass)
                        {
                            if (Vector3.Distance(entityBehaviour.transform.position, GameManager.Instance.PlayerController.transform.position) <= SafeZoneSizeSetting)
                            {
                                // Is it hostile or pacified?
                                EnemySenses enemySenses = entityBehaviour.GetComponent<EnemySenses>();
                                EnemyEntity enemyEntity = entityBehaviour.Entity as EnemyEntity;
                                if (!enemySenses.QuestBehaviour && enemyEntity.MobileEnemy.Team != MobileTeams.PlayerAlly)
                                {
                                    Destroy(entityBehaviour.gameObject);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static bool TransformPlayerPosition()
        {
            DFLocation dungLocation = DungLocationGlobal;
            SpawnPoints[] SpawnLocations = RandomBlockLocationPicker(dungLocation);
            if (SpawnLocations != null)
            {
                int RandSpawnIndex = UnityEngine.Random.Range(0, SpawnLocations.Length);
                SpawnPoints spawnPoint = new SpawnPoints(SpawnLocations[RandSpawnIndex].flatPosition, SpawnLocations[RandSpawnIndex].dungeonX, SpawnLocations[RandSpawnIndex].dungeonZ);
                spawnPoint.flatPosition = SpawnLocations[RandSpawnIndex].flatPosition;
                spawnPoint.dungeonX = SpawnLocations[RandSpawnIndex].dungeonX;
                spawnPoint.dungeonZ = SpawnLocations[RandSpawnIndex].dungeonZ;

                // Teleport PC to the randomly determined "spawn point" within the current dungeon.
                Vector3 dungeonBlockPosition = new Vector3(spawnPoint.dungeonX * RDBLayout.RDBSide, 0, spawnPoint.dungeonZ * RDBLayout.RDBSide);
                GameManager.Instance.PlayerObject.transform.localPosition = dungeonBlockPosition + spawnPoint.flatPosition;
                GameManager.Instance.PlayerMotor.FixStanding();
                return true;
            }
            else
                return false;
        }

        public static SpawnPoints[] RandomBlockLocationPicker(DFLocation location)
        {
            List<SpawnPoints> spawnPointsList = new List<SpawnPoints>();

            // Step through dungeon layout to find all blocks with markers
            foreach (var dungeonBlock in location.Dungeon.Blocks) // May put a "try-catch" block here to catch some compile errors due to no location being defined.
            {
                // Get block data
                DFBlock blockData = DaggerfallUnity.Instance.ContentReader.BlockFileReader.GetBlock(dungeonBlock.BlockName);

                // Skip misplaced overlapping N block at -1,-1 in Orsinium
                // This must be a B block to close out dungeon on that edge, not an N block which opens dungeon to void
                // DaggerfallDungeon skips this N block during layout, so prevent it being available to quest system
                if (location.MapTableData.MapId == 19021260 &&
                    dungeonBlock.X == -1 && dungeonBlock.Z == -1 && dungeonBlock.BlockName == "N0000065.RDB")
                {
                    continue;
                }

                switch(dungeonBlock.BlockName) 
                {
                    case "W0000002.RDB":
                    case "W0000004.RDB":
                    case "W0000005.RDB":
                    case "W0000009.RDB":
                    case "W0000013.RDB":
                    case "W0000017.RDB":
                    case "W0000018.RDB":
                    case "W0000024.RDB":
                    case "W0000029.RDB":
                        continue; // Filters out all "unfair" underwater blocks.
                    case "N0000004.RDB":
                    case "N0000005.RDB":
                    case "N0000006.RDB":
                    case "N0000023.RDB":
                    case "N0000030.RDB":
                    case "N0000033.RDB":
                    case "N0000034.RDB":
                    case "N0000036.RDB":
                    case "N0000037.RDB":
                    case "N0000038.RDB":
                    case "N0000046.RDB":
                    case "N0000054.RDB":
                    case "N0000061.RDB":
                        continue; // Filters out all "unfair" dry blocks.
                    default:
                        break;
                }

                // Iterate all groups
                foreach (DFBlock.RdbObjectRoot group in blockData.RdbBlock.ObjectRootList)
                {
                    // Skip empty object groups
                    if (null == group.RdbObjects)
                        continue;

                    // Look for flats in this group
                    foreach (DFBlock.RdbObject obj in group.RdbObjects)
                    {
                        // Look for editor flats
                        Vector3 position = new Vector3(obj.XPos, -obj.YPos, obj.ZPos) * MeshReader.GlobalScale;
                        if (obj.Type == DFBlock.RdbResourceTypes.Flat)
                        {
                            if (obj.Resources.FlatResource.TextureArchive == editorFlatArchive)
                            {
                                switch (obj.Resources.FlatResource.TextureRecord) // May consider eventually adding more valid spawn locations than just quest-markers.
                                {
                                    case spawnMarkerFlatIndex:
                                        spawnPointsList.Add(CreateSpawnPoint(position, dungeonBlock.X, dungeonBlock.Z));
                                        break;
                                    case itemMarkerFlatIndex:
                                        spawnPointsList.Add(CreateSpawnPoint(position, dungeonBlock.X, dungeonBlock.Z));
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            // Assign arrays if at least one quest marker found
            if (spawnPointsList.Count > 0)
                return spawnPointsList.ToArray();
            else
                return null;
        }

        public class SpawnPoints
        {
            public Vector3 flatPosition;                // Position of marker flat in block layout
            public int dungeonX;                        // Dungeon block X position in location
            public int dungeonZ;                        // Dungeon block Z position in location

            public SpawnPoints(Vector3 flatPosition, int dungeonX, int dungeonZ)
            {
                this.flatPosition = flatPosition;
                this.dungeonX = dungeonX;
                this.dungeonZ = dungeonZ;
            }
        }

        public static SpawnPoints CreateSpawnPoint(Vector3 flatPosition, int dungeonX = 0, int dungeonZ = 0)
        {
            SpawnPoints spawnPoints = new SpawnPoints(flatPosition, dungeonX, dungeonZ);
            spawnPoints.flatPosition = flatPosition;
            spawnPoints.dungeonX = dungeonX;
            spawnPoints.dungeonZ = dungeonZ;

            return spawnPoints;
        }

        public static int[] CollectDungeonIndicesOfType(DFRegion regionData, int regionIndex)
        {
            List<int> foundLocationIndices = new List<int>();

            // Collect all dungeon types
            for (int i = 0; i < regionData.LocationCount; i++)
            {
                // Discard all non-dungeon location types
                if (!IsLocationType(regionData.MapTable[i].LocationType) || IsDungeonType(regionData.MapTable[i].DungeonType))
                    continue;
                // Discard all dungeon locations that don't comply with climate filter settings
                DFLocation dungLocation = DaggerfallUnity.Instance.ContentReader.MapFileReader.GetLocation(regionIndex, i);

                switch (dungLocation.Climate.WorldClimate)
                {
                    case (int)MapsFile.Climates.Ocean:
                        if (!OceanStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.Desert:
                        if (!DesertStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.Desert2:
                        if (!DesertHotStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.Mountain:
                        if (!MountainStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.Rainforest:
                        if (!RainforestStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.Swamp:
                        if (!SwampStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.Subtropical:
                        if (!SubtropicalStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.MountainWoods:
                        if (!MountainWoodsStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.Woodlands:
                        if (!WoodlandsStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.HauntedWoodlands:
                        if (!HauntedWoodlandsStartCheck)
                            continue;
                        break;
                    default:
                        break;
                }

                // Discard Main-quest Dungeons if the setting has these disabled, they are disabled by default
                if (!QuestDungStartCheck && MainQuestDungeonChecker(regionIndex, dungLocation.Name))
                    continue;
                // Discard Isolated Island Dungeons With No Local Towns/Homes if the setting has these disabled, they are enabled by default
                if (!IsolatedIslandStartCheck && IsolatedIslandChecker(regionIndex, dungLocation.Name))
                    continue;
                // Discard Populated Island Dungeons With Local Towns/Homes if the setting has these disabled, they are enabled by default
                if (!PopulatedIslandStartCheck && PopulatedIslandChecker(regionIndex, dungLocation.Name))
                    continue;

                foundLocationIndices.Add(i);
            }

            return foundLocationIndices.ToArray();
        }

        public static bool IsLocationType(DFRegion.LocationTypes locationType)
        {
            // Consider 3 major dungeon types and 2 graveyard types as dungeons
            // Will exclude locations with dungeons, such as Daggerfall, Wayrest, Sentinel
            if (locationType == DFRegion.LocationTypes.DungeonKeep ||
                locationType == DFRegion.LocationTypes.DungeonLabyrinth ||
                locationType == DFRegion.LocationTypes.DungeonRuin ||
                locationType == DFRegion.LocationTypes.Graveyard)
            {
                return true;
            }

            return false;
        }

        public static bool IsDungeonType(DFRegion.DungeonTypes dungeonType)
        {
            // Will exclude Cemetery type dungeons by default, the ones that are revealed by default and very small interior size.
            switch(dungeonType)
            {
                case DFRegion.DungeonTypes.ScorpionNest:
                    if (!ScorpionNestStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.VolcanicCaves:
                    if (!VolcanicCavesStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.BarbarianStronghold:
                    if (!BarbarianStrongholdStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.DragonsDen:
                    if (!DragonsDenStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.GiantStronghold:
                    if (!GiantStrongholdStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.SpiderNest:
                    if (!SpiderNestStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.RuinedCastle:
                    if (!RuinedCastleStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.HarpyNest:
                    if (!HarpyNestStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.Laboratory:
                    if (!LaboratoryStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.VampireHaunt:
                    if (!VampireHauntStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.Coven:
                    if (!CovenStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.NaturalCave:
                    if (!NaturalCaveStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.Mine:
                    if (!MineStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.DesecratedTemple:
                    if (!DesecratedTempleStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.Prison:
                    if (!PrisonStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.HumanStronghold:
                    if (!HumanStrongholdStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.OrcStronghold:
                    if (!OrcStrongholdStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.Crypt:
                    if (!CryptStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.Cemetery:
                    if (!CemeteryStartCheck)
                        return true;
                    break;
                default:
                    break;
            }

            return false;
        }

        public static bool MainQuestDungeonChecker(int regionIndex, string dungName) // Will have to do some testing with this, not sure if this will work right.
        {
            switch (regionIndex)
            {
                case 31:
                    if (dungName.ToLower() == "mantellan_crux")
                        return true;
                    break;
                case 26:
                    if (dungName.ToLower() == "orsiniumcastle1")
                        return true;
                    break;
                case 20:
                    if (dungName.ToLower() == "sentinelcastle")
                        return true;
                    break;
                case 16:
                    if (dungName.ToLower() == "shedungent1")
                        return true;
                    break;
                case 23:
                    if (dungName.ToLower() == "wayrestcastle" || dungName.ToLower() == "woodbournehall4")
                        return true;
                    break;
                case 40:
                    if (dungName.ToLower() == "llugwychcastle")
                        return true;
                    break;
                case 33:
                    if (dungName.ToLower() == "lysandustomb1")
                        return true;
                    break;
                case 17:
                    if (dungName.ToLower() == "daggerfallcastle" || dungName.ToLower() == "pirateerhold1" || dungName.ToLower() == "castle_necromoghan")
                        return true;
                    break;
                case 9:
                    if (dungName.ToLower() == "dirennitower2")
                        return true;
                    break;
                case 1:
                    if (dungName.ToLower() == "scourgbarrow1")
                        return true;
                    break;
                default:
                    return false;
            }

            return false;
        }

        public static bool IsolatedIslandChecker(int regionIndex, string dungName) // Will have to do some testing with this, not sure if this will work right.
        {
            switch (regionIndex)
            {
                case 61:
                    if (dungName.ToLower() == "ruins of cosh hall")
                        return true;
                    break;
                case 58:
                    if (dungName.ToLower() == "ruins of yeomford hall" || dungName.ToLower() == "the citadel of hearthham" || dungName.ToLower() == "the elyzanna assembly")
                        return true;
                    break;
                default:
                    return false;
            }

            return false;
        }

        public static bool PopulatedIslandChecker(int regionIndex, string dungName) // Will have to do some testing with this, not sure if this will work right.
        {
            switch (regionIndex)
            {
                case 19:
                    return true;
                case 9:
                    return true;
                case 17:
                    if (dungName.ToLower() == "the cabal of morgyrrya")
                        return true;
                    break;
                case 21:
                    if (dungName.ToLower() == "the crypts of hawkhouse" || dungName.ToLower() == "the moorwing cemetery" || dungName.ToLower() == "the ashham tombs" || dungName.ToLower() == "the buckingwing graveyard" || dungName.ToLower() == "the tombs of greenford")
                        return true;
                    break;
                case 36:
                    if (dungName.ToLower() == "the house of lithivam" || dungName.ToLower() == "tower kinghart")
                        return true;
                    break;
                case 23:
                    if (dungName.ToLower() == "the yagrator aviary")
                        return true;
                    break;
                case 52:
                    if (dungName.ToLower() == "ruins of the klolpum farmstead" || dungName.ToLower() == "the prison of rhurpur")
                        return true;
                    break;
                case 50:
                    if (dungName.ToLower() == "the tower of ghorkke")
                        return true;
                    break;
                case 20:
                    if (dungName.ToLower() == "castle faallem")
                        return true;
                    break;
                default:
                    return false;
            }

            return false;
        }

        public static int RandomRegionRoller(int[] regionList)
        {
            int randIndex = UnityEngine.Random.Range(0, regionList.Length);
            return regionList[randIndex];
        }

        #endregion

        #region Console Command Specific Methods

        public static void FindCurrentBlockInfo()
        {
            PlayerGPS playerGPS = GameManager.Instance.PlayerGPS;
            if (playerGPS.HasCurrentLocation)
            {
                DFLocation location = playerGPS.CurrentLocation;
                GameObject gameObjectPlayerAdvanced = null; // used to hold reference to instance of GameObject "PlayerAdvanced"
                gameObjectPlayerAdvanced = GameObject.Find("PlayerAdvanced");
                float playerPosX = gameObjectPlayerAdvanced.transform.position.x / RDBLayout.RDBSide;
                float playerPosY = gameObjectPlayerAdvanced.transform.position.z / RDBLayout.RDBSide;
                Debug.LogFormat("X-pos = {0} || Y-pos = {1}", (int)(Mathf.Floor(playerPosX)), (int)(Mathf.Floor(playerPosY)));

                if (location.HasDungeon)
                {
                    foreach (var dungeonBlock in location.Dungeon.Blocks)
                    {
                        if (dungeonBlock.X == (int)Mathf.Floor(playerPosX) && dungeonBlock.Z == (int)Mathf.Floor(playerPosY))
                        {
                            DaggerfallUI.AddHUDText(String.Format("Current Dungeon Block Name = {0} || Block X-pos = {1} || Block Z-pos = {2}", dungeonBlock.BlockName, dungeonBlock.X, dungeonBlock.Z), 5.00f);
                        }
                    }
                }
            }
        }

        #endregion

    }
}