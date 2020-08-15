// Project:         RandomStartingDungeon mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2020 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    8/12/2020, 5:05 PM
// Last Edit:		8/15/2020, 2:40 PM
// Version:			1.00
// Special Thanks:  Jehuty
// Modifier:

using DaggerfallConnect;
using DaggerfallConnect.Arena2;
using DaggerfallConnect.Utility;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.MagicAndEffects;
using DaggerfallWorkshop.Game.Utility;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RandomStartingDungeon
{
    public class RandomStartingDungeon : MonoBehaviour
	{
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

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            mod = initParams.Mod;
            var go = new GameObject("RandomStartingDungeon");
            go.AddComponent<RandomStartingDungeon>();
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

            StartGameBehaviour.OnStartGame += RandomizeSpawn_OnStartGame;

            EntityEffectBroker.OnNewMagicRound += TeleRandomDungeonTest_OnNewMagicRound; // Just for testing purposes, final will be only for starting new character, and a console command.

            Debug.Log("Finished mod init: RandomStartingDungeon");
		}

        #endregion

        #region Methods and Functions

        // Testing

        private static void TeleRandomDungeonTest_OnNewMagicRound()
        {
            DFRegion regionInfo = new DFRegion();
            int[] foundIndices = new int[0];
            List<int> validRegionIndexes = new List<int>();
            Dictionary<int, int[]> regionValidDungGrabBag = new Dictionary<int, int[]>();
			regionValidDungGrabBag.Clear(); // Attempts to clear dictionary to keep from compile errors about duplicate keys.

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
                validRegionIndexes.Add(n); // Obviously will need to do a lot of testing, but I think all of the main settings and options are set-up for now.
            }

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

            // Will likely attempt to figure out some way to teleport the player somewhere "random" within the dungeon, instead of always at the exit of it, make an option for this as well.

            // Possibly will also add a custom console command for this mod that will allow the player to teleport to a random dungeon on use of said command, check clock mod for examples.

            // So this seems to be working, at least gets player to the entrance inside the random dungeon. Will work on getting this properly migrated into parent mod and settings.

            Debug.LogFormat("Random Region Index # {0} has {1} locations = {2} and {3} of those are valid dungeons", randomRegionIndex, regionInfo.LocationCount, regionInfo.Name, foundIndices.Length);
            Debug.LogFormat("Random Dungeon Index # {0} in the Region {1} = {2}, Dungeon Type is a {3}", RandDungIndex, regionInfo.Name, dungLocation.Name, dungLocation.MapTableData.DungeonType.ToString());
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
                // Discard all dungeon locations that don't comply with climate filter settings           // regionData.MapTable[i].LocationId //
                DFLocation dungLocation = DaggerfallUnity.Instance.ContentReader.MapFileReader.GetLocation(regionIndex, i);
                if (!oceanStartCheck && dungLocation.Climate.WorldClimate == (int)MapsFile.Climates.Ocean)                       // Ocean (Vanilla does not have any random dungeons in this climate) 
                    continue;
                if (!desertStartCheck && dungLocation.Climate.WorldClimate == (int)MapsFile.Climates.Desert)                     // Desert 
                    continue;
                if (!desertHotStartCheck && dungLocation.Climate.WorldClimate == (int)MapsFile.Climates.Desert2)                 // Desert2 (Hottest desert, dak'fron has this) 
                    continue;
                if (!mountainStartCheck && dungLocation.Climate.WorldClimate == (int)MapsFile.Climates.Mountain)                 // Mountain 
                    continue;
                if (!rainforestStartCheck && dungLocation.Climate.WorldClimate == (int)MapsFile.Climates.Rainforest)             // Rainforest 
                    continue;
                if (!swampStartCheck && dungLocation.Climate.WorldClimate == (int)MapsFile.Climates.Swamp)                       // Swamp 
                    continue;
                if (!subtropicalStartCheck && dungLocation.Climate.WorldClimate == (int)MapsFile.Climates.Subtropical)           // Subtropical 
                    continue;
                if (!mountainWoodsStartCheck && dungLocation.Climate.WorldClimate == (int)MapsFile.Climates.MountainWoods)       // MountainWoods 
                    continue;
                if (!woodlandsStartCheck && dungLocation.Climate.WorldClimate == (int)MapsFile.Climates.Woodlands)               // Woodlands 
                    continue;
                if (!hauntedWoodlandsStartCheck && dungLocation.Climate.WorldClimate == (int)MapsFile.Climates.HauntedWoodlands) // HauntedWoodlands 
                    continue;
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
            if (!cemeteryStartCheck && dungeonType == DFRegion.DungeonTypes.Cemetery)
                return true;
            if (!scorpionNestStartCheck && dungeonType == DFRegion.DungeonTypes.ScorpionNest)
                return true;
            if (!volcanicCavesStartCheck && dungeonType == DFRegion.DungeonTypes.VolcanicCaves)
                return true;
            if (!barbarianStrongholdStartCheck && dungeonType == DFRegion.DungeonTypes.BarbarianStronghold)
                return true;
            if (!dragonsDenStartCheck && dungeonType == DFRegion.DungeonTypes.DragonsDen)
                return true;
            if (!giantStrongholdStartCheck && dungeonType == DFRegion.DungeonTypes.GiantStronghold)
                return true;
            if (!spiderNestStartCheck && dungeonType == DFRegion.DungeonTypes.SpiderNest)
                return true;
            if (!ruinedCastleStartCheck && dungeonType == DFRegion.DungeonTypes.RuinedCastle)
                return true;
            if (!harpyNestStartCheck && dungeonType == DFRegion.DungeonTypes.HarpyNest)
                return true;
            if (!laboratoryStartCheck && dungeonType == DFRegion.DungeonTypes.Laboratory)
                return true;
            if (!vampireHauntStartCheck && dungeonType == DFRegion.DungeonTypes.VampireHaunt)
                return true;
            if (!covenStartCheck && dungeonType == DFRegion.DungeonTypes.Coven)
                return true;
            if (!naturalCaveStartCheck && dungeonType == DFRegion.DungeonTypes.NaturalCave)
                return true;
            if (!mineStartCheck && dungeonType == DFRegion.DungeonTypes.Mine)
                return true;
            if (!desecratedTempleStartCheck && dungeonType == DFRegion.DungeonTypes.DesecratedTemple)
                return true;
            if (!prisonStartCheck && dungeonType == DFRegion.DungeonTypes.Prison)
                return true;
            if (!humanStrongholdStartCheck && dungeonType == DFRegion.DungeonTypes.HumanStronghold)
                return true;
            if (!orcStrongholdStartCheck && dungeonType == DFRegion.DungeonTypes.OrcStronghold)
                return true;
            if (!cryptStartCheck && dungeonType == DFRegion.DungeonTypes.Crypt)
                return true;

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

        // Testing

        private static void RandomizeSpawn_OnStartGame(object sender, EventArgs e)
        {

        }

        #endregion

    }
}