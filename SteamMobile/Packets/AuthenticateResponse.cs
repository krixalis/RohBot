﻿using System;

namespace SteamMobile.Packets
{
    public class AuthenticateResponse : Packet
    {
        public override string Type
        {
            get { return "authResponse"; }
        }

        public string Name;
        public string Tokens;
        public bool Success;

        public override void Handle(Connection connection)
        {
            throw new NotSupportedException();
        }
    }
}
