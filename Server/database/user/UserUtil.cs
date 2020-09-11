﻿using MongoDB.Bson;
using Server.database.channel;
using Server.database.role;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.user
{
    class UserUtil
    {
        readonly UserCrud userCrud;
        readonly ChannelCrud channelCrud;

        public UserUtil(UserCrud userCrud = null, ChannelCrud channelCrud = null)
        {
            this.userCrud = userCrud;
            this.channelCrud = channelCrud;
        }

        public List<User> GetUsersThatCanView(Channel channel)
        {
            List<User> canView = new List<User>();

            List<User> allUsers = userCrud.ReadMany();
            foreach(User currUser in allUsers)
            {
                if (UserCanViewChannel(currUser, channel)) canView.Add(currUser);
            }

            return canView;
        }

        public bool UserCanViewChannel(string userId, string channelId)
        {
            return UserCanViewChannel(userCrud.ReadOneById(userId), channelCrud.ReadOneById(channelId));
        }

        public bool UserCanViewChannel(User user, Channel channel)
        {
            foreach(string roleId in user.roles)
            {
                if (channel.roles.Contains(roleId)) return true;
            }

            return false;
        }
    }
}
