// Project:         RandomStartingDungeon mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2020 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    8/12/2020, 5:05 PM
// Last Edit:		8/15/2020, 2:40 PM
// Version:			1.00
// Special Thanks:  Jehuty
// Modifier:

using System;
using System.Globalization;
using UnityEngine;
using Wenzil.Console;

namespace RandomStartingDungeon
{
    public static class RandomStartingDungeonConsoleCommands
    {
        const string noInstanceMessage = "Convenient Clock instance not found.";

        public static void RegisterCommands()
        {
            try
            {
                ConsoleCommandsDatabase.RegisterCommand(SetConvenientClockMode.name, SetConvenientClockMode.description, SetConvenientClockMode.usage, SetConvenientClockMode.Execute);
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("Error Registering RealGrass Console commands: {0}", e.Message));
            }
        }

        private static class SetConvenientClockMode
        {
            public static readonly string name = "convenient_clock_mode";
            public static readonly string description = "Set mode: 0 - 6";
            public static readonly string usage = "Set Convenient Clock Mode";

            public static string Execute(params string[] args)
            {
                var convenientClock = ConvenientClock.Instance;
                if (convenientClock == null)
                    return noInstanceMessage;

                int value;
                if (args.Length < 1 || !int.TryParse(args[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                    return usage;

                if ((value < 0 || value > 6) && (args.Length < 2 || args[1] != "-force"))
                    return "Only value 0 - 6 is valid";

                convenientClock.DisplayMode = value;

                if (value == 0)
                    return "0: Convenient clock mode is disabled";

                else if (value == 1)
                    return "1: Convenient clock mode is set to Circle w. Clock";

                else if (value == 2)
                    return "2: Convenient clock mode is set to Circle";

                else if (value == 3)
                    return "3: Convenient clock mode is set to Time Bar";

                else if (value == 4)
                    return "4: Convenient clock mode is set to Time text";

                else if (value == 5)
                    return "5: Convenient clock mode is set to Weekday-Time Text";

                else if (value == 6)
                    return "6: Convenient clock mode is set to Time-Weekday-Day Text";
                else
                    return "?: Not a valid value.";
            }
        }
    }
}
