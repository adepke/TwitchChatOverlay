/*
	Copyright (c) 2018 Andrew Depke
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TwitchChatOverlay
{
    class Utils
    {
        public static Color ColorFromString(string Input)
        {
            Color Result = new Color();

            var Values = Input.Split(':');

            Result.R = Byte.Parse(Values[0]);
            Result.G = Byte.Parse(Values[1]);
            Result.B = Byte.Parse(Values[2]);
            Result.A = Byte.Parse(Values[3]);

            return Result;
        }

        public static string ColorToString(Color Input)
        {
            return Input.R + ":" + Input.G + ":" + Input.B + ":" + Input.A;
        }
    }
}
