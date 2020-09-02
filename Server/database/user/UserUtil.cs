using MongoDB.Bson;
using Server.database.role;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.user
{
    class UserUtil
    {
        public static List<User> GetUsersWithPermission(UserCrud crud, List<ObjectId> roles)
        {
            List<User> withPermission = new List<User>();

            List<User> allUsers = crud.ReadMany();
            foreach(User user in allUsers)
            {
                foreach(ObjectId role in user.roles)
                {
                    if (roles.Contains(role)) withPermission.Add(user);
                }
            }

            return withPermission;
        }
    }
}
