// Project:         RandomStartingDungeon mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2020 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    8/12/2020, 5:05 PM
// Last Edit:		8/23/2020, 5:50 PM
// Version:			1.00
// Special Thanks:  Jehuty, TheLacus, Hazelnut
// Modifier:

using System;
using UnityEngine;
using Wenzil.Console;

namespace RandomStartingDungeon
{
    public static class RandomStartingDungeonConsoleCommands
    {
        const string noInstanceMessage = "Random Starting Dungeon instance not found.";

        public static void RegisterCommands()
        {
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
                var randomStartingDungeon = RandomStartingDungeon.Instance;
                if (randomStartingDungeon == null)
                    return noInstanceMessage;

                RandomStartingDungeon.PickRandomDungeonTeleport();

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
                var randomStartingDungeon = RandomStartingDungeon.Instance;
                if (randomStartingDungeon == null)
                    return noInstanceMessage;

                bool successCheck = RandomStartingDungeon.TransformPlayerPosition();

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
                var randomStartingDungeon = RandomStartingDungeon.Instance;
                if (randomStartingDungeon == null)
                    return noInstanceMessage;

                RandomStartingDungeon.FindCurrentBlockInfo();

                return "Transforming Player Dungeon Position...";
            }
        }
    }
}
