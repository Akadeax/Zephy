using System;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Server.database.Roles
{
    class RoleCache
    {
        private Dictionary<ObjectId, Role> cachedRoles;
        private readonly RoleCrud roleCrud;

        public RoleCache(string database)
        {
            roleCrud = new RoleCrud(database);
            ReloadCache();
        }

        public bool Empty
        {
            get
            {
                return cachedRoles.Count == 0;
            }
        }

        public void ReloadCache()
        {
            cachedRoles = new Dictionary<ObjectId, Role>();

            List<Role> roles = roleCrud.LoadRoles();
            foreach (Role role in roles)
            {
                cachedRoles.Add(role._id, role);
            }
        }

        public Role GetRole(ObjectId id)
        {
            return cachedRoles.GetValueOrDefault(id, null);
        }

        public List<Role> GetRoles()
        {
            return new List<Role>(cachedRoles.Values);
        }
    }
}
