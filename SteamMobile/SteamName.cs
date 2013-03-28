﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using SteamKit2;

namespace SteamMobile
{
    public static class SteamName
    {
        public static string Get(SteamID steamId)
        {
            return Steam.Friends.GetFriendPersonaName(steamId);
        }
    }
}
