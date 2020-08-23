// Project:         RandomStartingDungeon mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2020 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    8/12/2020, 5:05 PM
// Last Edit:		8/23/2020, 5:50 PM
// Version:			1.00
// Special Thanks:  Jehuty, TheLacus, Hazelnut
// Modifier:

using DaggerfallConnect;
using DaggerfallConnect.Arena2;
using DaggerfallConnect.Utility;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;
using DaggerfallWorkshop.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RandomStartingDungeon
{
    public class RandomStartingDungeon : MonoBehaviour
	{
        static RandomStartingDungeon instance;

        public static RandomStartingDungeon Instance
        {
            get { return instance ?? (instance = FindObjectOfType<RandomStartingDungeon>()); }
        }

        static Mod mod;

        // Region Specific and Quest Dungeon Options
        public static bool questDungStartCheck { get; set; }
        public static bool isolatedIslandStartCheck { get; set; }
        public static bool populatedIslandStartCheck { get; set; }

        // Dungeon Location Climate Options
        public static bool oceanStartCheck { get; set; }
        public static bool desertStartCheck { get; set; }
        public static bool desertHotStartCheck { get; set; }
        public static bool mountainStartCheck { get; set; }
        public static bool rainforestStartCheck { get; set; }
        public static bool swampStartCheck { get; set; }
        public static bool subtropicalStartCheck { get; set; }
        public static bool mountainWoodsStartCheck { get; set; }
        public static bool woodlandsStartCheck { get; set; }
        public static bool hauntedWoodlandsStartCheck { get; set; }

        // Dungeon Type Options
        public static bool cemeteryStartCheck { get; set; }
        public static bool scorpionNestStartCheck { get; set; }
        public static bool volcanicCavesStartCheck { get; set; }
        public static bool barbarianStrongholdStartCheck { get; set; }
        public static bool dragonsDenStartCheck { get; set; }
        public static bool giantStrongholdStartCheck { get; set; }
        public static bool spiderNestStartCheck { get; set; }
        public static bool ruinedCastleStartCheck { get; set; }
        public static bool harpyNestStartCheck { get; set; }
        public static bool laboratoryStartCheck { get; set; }
        public static bool vampireHauntStartCheck { get; set; }
        public static bool covenStartCheck { get; set; }
        public static bool naturalCaveStartCheck { get; set; }
        public static bool mineStartCheck { get; set; }
        public static bool desecratedTempleStartCheck { get; set; }
        public static bool prisonStartCheck { get; set; }
        public static bool humanStrongholdStartCheck { get; set; }
        public static bool orcStrongholdStartCheck { get; set; }
        public static bool cryptStartCheck { get; set; }

        // General "Global" Variables
        public static bool alreadyRolled { get; set; }
        public static Dictionary<int, int[]> quickRerollDictionary { get; set; }
        public static List<int> quickRerollValidRegions { get; set; }
        const int editorFlatArchive = 199;
        const int spawnMarkerFlatIndex = 11;
        const int itemMarkerFlatIndex = 18;
        public static SpawnPoints spawnPointGlobal { get; set; }
        public static DFLocation dungLocationGlobal { get; set; }

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            mod = initParams.Mod;
            instance = new GameObject("RandomStartingDungeon").AddComponent<RandomStartingDungeon>(); // Add script to the scene.
        }
		
		void Awake()
        {
            ModSettings settings = mod.GetSettings();
            // Region Specific and Quest Dungeon Options
            bool questDungeons = settings.GetBool("Quest&IslandOptions", "questDungeons");
            bool isolatedIslandDungeons = settings.GetBool("Quest&IslandOptions", "isolatedIslandDungeons");
            bool populatedIslandDungeons = settings.GetBool("Quest&IslandOptions", "populatedIslandDungeons");

            // Dungeon Location Climate Options
            bool oceanDungs = settings.GetBool("ClimateOptions", "ocean");
            bool desertDungs = settings.GetBool("ClimateOptions", "desert");
            bool hotDesertDungs = settings.GetBool("ClimateOptions", "hotDesert");
            bool mountainDungs = settings.GetBool("ClimateOptions", "mountain");
            bool rainforestDungs = settings.GetBool("ClimateOptions", "rainforest");
            bool swampDungs = settings.GetBool("ClimateOptions", "swamp");
            bool mountainWoodsDungs = settings.GetBool("ClimateOptions", "mountainWoods");
            bool woodlandsDungs = settings.GetBool("ClimateOptions", "woodlands");
            bool hauntedWoodlandsDungs = settings.GetBool("ClimateOptions", "hauntedWoodlands");

            // Dungeon Type Options
            bool cemeteryDungs = settings.GetBool("DungeonTypeOptions", "cemetery");
            bool scorpionNestDungs = settings.GetBool("DungeonTypeOptions", "scorpionNest");
            bool volcanicCavesDungs = settings.GetBool("DungeonTypeOptions", "volcanicCaves");
            bool barbarianStrongholdDungs = settings.GetBool("DungeonTypeOptions", "barbarianStronghold");
            bool dragonsDenDungs = settings.GetBool("DungeonTypeOptions", "dragonsDen");
            bool giantStrongholdDungs = settings.GetBool("DungeonTypeOptions", "giantStronghold");
            bool spiderNestDungs = settings.GetBool("DungeonTypeOptions", "spiderNest");
            bool ruinedCastleDungs = settings.GetBool("DungeonTypeOptions", "ruinedCastle");
            bool harpyNestDungs = settings.GetBool("DungeonTypeOptions", "harpyNest");
            bool laboratoryDungs = settings.GetBool("DungeonTypeOptions", "laboratory");
            bool vampireHauntDungs = settings.GetBool("DungeonTypeOptions", "vampireHaunt");
            bool covenDungs = settings.GetBool("DungeonTypeOptions", "coven");
            bool naturalCaveDungs = settings.GetBool("DungeonTypeOptions", "naturalCave");
            bool mineDungs = settings.GetBool("DungeonTypeOptions", "mine");
            bool desecratedTempleDungs = settings.GetBool("DungeonTypeOptions", "desecratedTemple");
            bool prisonDungs = settings.GetBool("DungeonTypeOptions", "prison");
            bool humanStrongholdDungs = settings.GetBool("DungeonTypeOptions", "humanStronghold");
            bool orcStrongholdDungs = settings.GetBool("DungeonTypeOptions", "orcStronghold");
            bool cryptDungs = settings.GetBool("DungeonTypeOptions", "crypt");

            InitMod(questDungeons, isolatedIslandDungeons, populatedIslandDungeons,
                oceanDungs, desertDungs, hotDesertDungs, mountainDungs, rainforestDungs, swampDungs, mountainWoodsDungs, woodlandsDungs, hauntedWoodlandsDungs,
                cemeteryDungs, scorpionNestDungs, volcanicCavesDungs, barbarianStrongholdDungs, dragonsDenDungs, giantStrongholdDungs, spiderNestDungs,
                ruinedCastleDungs, harpyNestDungs, laboratoryDungs, vampireHauntDungs, covenDungs, naturalCaveDungs, mineDungs, desecratedTempleDungs,
                prisonDungs, humanStrongholdDungs, orcStrongholdDungs, cryptDungs);

            mod.IsReady = true;
        }
		
		private void Start()
        {
            RandomStartingDungeonConsoleCommands.RegisterCommands();
        }
		
		#region InitMod and Settings
		
		private static void InitMod(bool questDungeons, bool isolatedIslandDungeons, bool populatedIslandDungeons,
            bool oceanDungs, bool desertDungs, bool hotDesertDungs, bool mountainDungs, bool rainforestDungs, bool swampDungs, bool mountainWoodsDungs,
            bool woodlandsDungs, bool hauntedWoodlandsDungs, bool cemeteryDungs, bool scorpionNestDungs, bool volcanicCavesDungs, bool barbarianStrongholdDungs,
            bool dragonsDenDungs, bool giantStrongholdDungs, bool spiderNestDungs, bool ruinedCastleDungs, bool harpyNestDungs, bool laboratoryDungs,
            bool vampireHauntDungs, bool covenDungs, bool naturalCaveDungs, bool mineDungs, bool desecratedTempleDungs, bool prisonDungs,
            bool humanStrongholdDungs, bool orcStrongholdDungs, bool cryptDungs)
        {
            Debug.Log("Begin mod init: RandomStartingDungeon");

            // Region Specific and Quest Dungeon Options
            if (questDungeons)
            {
                Debug.Log("RandomStartingDungeon: Quest Dungeons Are Allowed To Be Spawned In");
                questDungStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Quest Dungeons Are Not Allowed To Be Spawned In");
                questDungStartCheck = false;
            }

            if (isolatedIslandDungeons)
            {
                Debug.Log("RandomStartingDungeon: Isolated Island Dungeons Are Allowed To Be Spawned In");
                isolatedIslandStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Isolated Island Dungeons Are Not Allowed To Be Spawned In");
                isolatedIslandStartCheck = false;
            }

            if (populatedIslandDungeons)
            {
                Debug.Log("RandomStartingDungeon: Populated Island Dungeons Are Allowed To Be Spawned In");
                populatedIslandStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Populated Island Dungeons Are Not Allowed To Be Spawned In");
                populatedIslandStartCheck = false;
            }


            // Dungeon Location Climate Options
            if (oceanDungs)
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Ocean Climates Can Be Spawned In");
                oceanStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Ocean Climates Can Not Be Spawned In");
                oceanStartCheck = false;
            }

            if (desertDungs)
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Desert Climates Can Be Spawned In");
                desertStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Desert Climates Can Not Be Spawned In");
                desertStartCheck = false;
            }

            if (hotDesertDungs)
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Hot Desert Climates Can Be Spawned In");
                desertHotStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Hot Desert Climates Can Not Be Spawned In");
                desertHotStartCheck = false;
            }

            if (mountainDungs)
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Mountain Climates Can Be Spawned In");
                mountainStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Mountain Climates Can Not Be Spawned In");
                mountainStartCheck = false;
            }

            if (rainforestDungs)
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Rainforest Climates Can Be Spawned In");
                rainforestStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Rainforest Climates Can Not Be Spawned In");
                rainforestStartCheck = false;
            }

            if (swampDungs)
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Swamp Climates Can Be Spawned In");
                swampStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Swamp Climates Can Not Be Spawned In");
                swampStartCheck = false;
            }

            if (mountainWoodsDungs)
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Mountain Woods Climates Can Be Spawned In");
                mountainWoodsStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Mountain Woods Climates Can Not Be Spawned In");
                mountainWoodsStartCheck = false;
            }

            if (woodlandsDungs)
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Woodlands Climates Can Be Spawned In");
                woodlandsStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Woodlands Climates Can Not Be Spawned In");
                woodlandsStartCheck = false;
            }

            if (hauntedWoodlandsDungs)
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Haunted Woodlands Climates Can Be Spawned In");
                hauntedWoodlandsStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Dungeons Located Inside Haunted Woodlands Climates Can Not Be Spawned In");
                hauntedWoodlandsStartCheck = false;
            }


            // Dungeon Type Options
            if (cemeteryDungs)
            {
                Debug.Log("RandomStartingDungeon: Cemetery Dungeon Types Can Be Spawned In");
                cemeteryStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Cemetery Dungeon Types Can Not Be Spawned In");
                cemeteryStartCheck = false;
            }

            if (scorpionNestDungs)
            {
                Debug.Log("RandomStartingDungeon: Scorpion Nest Dungeon Types Can Be Spawned In");
                scorpionNestStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Scorpion Nest Dungeon Types Can Not Be Spawned In");
                scorpionNestStartCheck = false;
            }

            if (volcanicCavesDungs)
            {
                Debug.Log("RandomStartingDungeon: Volcanic Caves Dungeon Types Can Be Spawned In");
                volcanicCavesStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Volcanic Caves Dungeon Types Can Not Be Spawned In");
                volcanicCavesStartCheck = false;
            }

            if (barbarianStrongholdDungs)
            {
                Debug.Log("RandomStartingDungeon: Barbarian Stronghold Dungeon Types Can Be Spawned In");
                barbarianStrongholdStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Barbarian Stronghold Dungeon Types Can Not Be Spawned In");
                barbarianStrongholdStartCheck = false;
            }

            if (dragonsDenDungs)
            {
                Debug.Log("RandomStartingDungeon: Dragons Den Dungeon Types Can Be Spawned In");
                dragonsDenStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Dragons Den Dungeon Types Can Not Be Spawned In");
                dragonsDenStartCheck = false;
            }

            if (giantStrongholdDungs)
            {
                Debug.Log("RandomStartingDungeon: Giant Stronghold Dungeon Types Can Be Spawned In");
                giantStrongholdStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Giant Stronghold Dungeon Types Can Not Be Spawned In");
                giantStrongholdStartCheck = false;
            }

            if (spiderNestDungs)
            {
                Debug.Log("RandomStartingDungeon: Spider Nest Dungeon Types Can Be Spawned In");
                spiderNestStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Spider Nest Dungeon Types Can Not Be Spawned In");
                spiderNestStartCheck = false;
            }

            if (ruinedCastleDungs)
            {
                Debug.Log("RandomStartingDungeon: Ruined Castle Dungeon Types Can Be Spawned In");
                ruinedCastleStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Ruined Castle Dungeon Types Can Not Be Spawned In");
                ruinedCastleStartCheck = false;
            }

            if (harpyNestDungs)
            {
                Debug.Log("RandomStartingDungeon: Harpy Nest Dungeon Types Can Be Spawned In");
                harpyNestStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Harpy Nest Dungeon Types Can Not Be Spawned In");
                harpyNestStartCheck = false;
            }

            if (laboratoryDungs)
            {
                Debug.Log("RandomStartingDungeon: Laboratory Dungeon Types Can Be Spawned In");
                laboratoryStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Laboratory Dungeon Types Can Not Be Spawned In");
                laboratoryStartCheck = false;
            }

            if (vampireHauntDungs)
            {
                Debug.Log("RandomStartingDungeon: Vampire Haunt Dungeon Types Can Be Spawned In");
                vampireHauntStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Vampire Haunt Dungeon Types Can Not Be Spawned In");
                vampireHauntStartCheck = false;
            }

            if (covenDungs)
            {
                Debug.Log("RandomStartingDungeon: Coven Dungeon Types Can Be Spawned In");
                covenStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Coven Dungeon Types Can Not Be Spawned In");
                covenStartCheck = false;
            }

            if (naturalCaveDungs)
            {
                Debug.Log("RandomStartingDungeon: Natural Cave Dungeon Types Can Be Spawned In");
                naturalCaveStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Natural Cave Dungeon Types Can Not Be Spawned In");
                naturalCaveStartCheck = false;
            }

            if (mineDungs)
            {
                Debug.Log("RandomStartingDungeon: Mine Dungeon Types Can Be Spawned In");
                mineStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Mine Dungeon Types Can Not Be Spawned In");
                mineStartCheck = false;
            }

            if (desecratedTempleDungs)
            {
                Debug.Log("RandomStartingDungeon: Desecrated Temple Dungeon Types Can Be Spawned In");
                desecratedTempleStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Desecrated Temple Dungeon Types Can Not Be Spawned In");
                desecratedTempleStartCheck = false;
            }

            if (prisonDungs)
            {
                Debug.Log("RandomStartingDungeon: Prison Dungeon Types Can Be Spawned In");
                prisonStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Prison Dungeon Types Can Not Be Spawned In");
                prisonStartCheck = false;
            }

            if (humanStrongholdDungs)
            {
                Debug.Log("RandomStartingDungeon: Human Stronghold Dungeon Types Can Be Spawned In");
                humanStrongholdStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Human Stronghold Dungeon Types Can Not Be Spawned In");
                humanStrongholdStartCheck = false;
            }

            if (orcStrongholdDungs)
            {
                Debug.Log("RandomStartingDungeon: Orc Stronghold Dungeon Types Can Be Spawned In");
                orcStrongholdStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Orc Stronghold Dungeon Types Can Not Be Spawned In");
                orcStrongholdStartCheck = false;
            }

            if (cryptDungs)
            {
                Debug.Log("RandomStartingDungeon: Crypt Dungeon Types Can Be Spawned In");
                cryptStartCheck = true;
            }
            else
            {
                Debug.Log("RandomStartingDungeon: Crypt Dungeon Types Can Not Be Spawned In");
                cryptStartCheck = false;
            }

            alreadyRolled = false;

            StartGameBehaviour.OnStartGame += RandomizeSpawn_OnStartGame;

            Debug.Log("Finished mod init: RandomStartingDungeon");
		}

        #endregion

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

            if (!alreadyRolled)
            {
                for (int n = 0; n < 62; n++)
                {
                    regionInfo = DaggerfallUnity.Instance.ContentReader.MapFileReader.GetRegion(n);
                    if (regionInfo.LocationCount <= 0) // Add the if-statements to keep "invalid" regions from being put into grab-bag, also use this for some settings.
                        continue;
                    if (n == 31) // Index for "High Rock sea coast" or the "region" that holds the location of the two player boats, as well as the Mantellan Crux story dungeon.
                        continue;
                    if (!isolatedIslandStartCheck && n == 61) // Index for "Cybiades" the isolated region that has only one single location on the whole island, that being a dungeon.
                        continue;
                    // Get indices for all dungeons of this type
                    foundIndices = CollectDungeonIndicesOfType(regionInfo, n);
                    if (foundIndices.Length == 0)
                        continue;

                    regionValidDungGrabBag.Add(n, foundIndices);
                    validRegionIndexes.Add(n);
                    alreadyRolled = true; // Too keep this code-block from reprocessing every-time this function is ran again in the same play session.
                    quickRerollDictionary = regionValidDungGrabBag;
                    quickRerollValidRegions = validRegionIndexes;
                }
            }

            if (!alreadyRolled)
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
                if (quickRerollValidRegions.Count > 0)
                {
                    int randomRegionIndex = RandomRegionRoller(quickRerollValidRegions.ToArray());
                    foundIndices = quickRerollDictionary[randomRegionIndex];

                    // Select a random dungeon location index from available list then get its location data
                    int RandDungIndex = UnityEngine.Random.Range(0, foundIndices.Length);
                    DFLocation dungLocation = DaggerfallUnity.Instance.ContentReader.MapFileReader.GetLocation(randomRegionIndex, foundIndices[RandDungIndex]);
                    dungLocationGlobal = dungLocation;

                    SpawnPoints[] SpawnLocations = RandomBlockLocationPicker(dungLocation);
                    if (SpawnLocations != null)
                    {
                        int RandSpawnIndex = UnityEngine.Random.Range(0, SpawnLocations.Length);
                        SpawnPoints spawnPoint = new SpawnPoints(SpawnLocations[RandSpawnIndex].flatPosition, SpawnLocations[RandSpawnIndex].dungeonX, SpawnLocations[RandSpawnIndex].dungeonZ);
                        spawnPoint.flatPosition = SpawnLocations[RandSpawnIndex].flatPosition;
                        spawnPoint.dungeonX = SpawnLocations[RandSpawnIndex].dungeonX;
                        spawnPoint.dungeonZ = SpawnLocations[RandSpawnIndex].dungeonZ;
                        spawnPointGlobal = spawnPoint;
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
            if (GameManager.Instance.PlayerEnterExit.IsPlayerInsideDungeon && alreadyRolled) // This will defintiely have to be changed, for logic with more discrimination on when it runs.
            {
                bool successCheck = TransformPlayerPosition();

                if (!successCheck)
                    DaggerfallUI.AddHUDText("Transformation Failed, Could Not Find Valid Dungeon Position.", 6.00f);
            }
        }

        public static bool TransformPlayerPosition()
        {
            DFLocation dungLocation = dungLocationGlobal;
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
                        if (!oceanStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.Desert:
                        if (!desertStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.Desert2:
                        if (!desertHotStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.Mountain:
                        if (!mountainStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.Rainforest:
                        if (!rainforestStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.Swamp:
                        if (!swampStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.Subtropical:
                        if (!subtropicalStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.MountainWoods:
                        if (!mountainWoodsStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.Woodlands:
                        if (!woodlandsStartCheck)
                            continue;
                        break;
                    case (int)MapsFile.Climates.HauntedWoodlands:
                        if (!hauntedWoodlandsStartCheck)
                            continue;
                        break;
                    default:
                        break;
                }

                // Discard Main-quest Dungeons if the setting has these disabled, they are disabled by default
                if (!questDungStartCheck && MainQuestDungeonChecker(regionIndex, dungLocation.Name))
                    continue;
                // Discard Isolated Island Dungeons With No Local Towns/Homes if the setting has these disabled, they are enabled by default
                if (!isolatedIslandStartCheck && IsolatedIslandChecker(regionIndex, dungLocation.Name))
                    continue;
                // Discard Populated Island Dungeons With Local Towns/Homes if the setting has these disabled, they are enabled by default
                if (!populatedIslandStartCheck && PopulatedIslandChecker(regionIndex, dungLocation.Name))
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
                case DFRegion.DungeonTypes.Cemetery:
                    if (!cemeteryStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.ScorpionNest:
                    if (!scorpionNestStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.VolcanicCaves:
                    if (!volcanicCavesStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.BarbarianStronghold:
                    if (!barbarianStrongholdStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.DragonsDen:
                    if (!dragonsDenStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.GiantStronghold:
                    if (!giantStrongholdStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.SpiderNest:
                    if (!spiderNestStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.RuinedCastle:
                    if (!ruinedCastleStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.HarpyNest:
                    if (!harpyNestStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.Laboratory:
                    if (!laboratoryStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.VampireHaunt:
                    if (!vampireHauntStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.Coven:
                    if (!covenStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.NaturalCave:
                    if (!naturalCaveStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.Mine:
                    if (!mineStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.DesecratedTemple:
                    if (!desecratedTempleStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.Prison:
                    if (!prisonStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.HumanStronghold:
                    if (!humanStrongholdStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.OrcStronghold:
                    if (!orcStrongholdStartCheck)
                        return true;
                    break;
                case DFRegion.DungeonTypes.Crypt:
                    if (!cryptStartCheck)
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