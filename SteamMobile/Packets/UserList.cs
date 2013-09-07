﻿using System;
using System.Collections.Generic;

namespace SteamMobile.Packets
{
    public class UserList : Packet
    {
        public class User
        {
            public readonly string UserType;    // RohBot/Steam
            public readonly string Rank;        // Owner/Officer/Moderator/Member/Guest
            public readonly string Name;
            public readonly string Avatar;      // Only valid for Steam users
            public readonly string Playing;        // Only valid for Steam users

            public User(string userType, string rank, string name, string avatar, string playing)
            {
                UserType = userType;
                Rank = rank;
                Name = name;
                Avatar = avatar;
                Playing = playing;
            }
        }

        public override string Type { get { return "userList"; } }
        public List<User> Users = new List<User>();

        public void AddUser(string type, string rank, string name, string avatar = "", string playing = "")
        {
            Users.Add(new User(type, rank, name, avatar, playing));
        }
    }
}
