using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Server.Database.User
{
    public class UserCrud : MongoCrud<User>
    {
        public const string COLLECTION_NAME = "users";

        public UserCrud() : base(COLLECTION_NAME) { }

        public BaseUser ReadOneBase(Expression<Func<User, bool>> filter = null)
        {
            return ReadOne(filter).ToBaseUser();
        }

        public List<BaseUser> ReadManyBase(Expression<Func<User, bool>> filter = null)
        {
            if (filter == null) filter = x => true;

            List <BaseUser> baseList = new List<BaseUser>();
            foreach(User u in ReadMany(filter))
            {
                baseList.Add(u.ToBaseUser());
            }
            return baseList;
        }

        public List<ListedUser> ReadManyListedPaginated(int page, int pageSize, Expression<Func<User, bool>> filter = null)
        {
            if (filter == null) filter = x => true;

            List<BaseUser> paginatedUsers = collection
                .Aggregate()
                .Match(filter)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .As<BaseUser>()
                .ToList();

            return ToListed(paginatedUsers);
        }

        public List<ListedUser> ToListed(List<BaseUser> users)
        {
            List<ListedUser> listed = new List<ListedUser>();
            foreach (BaseUser u in users)
            {
                OnlineStatus status = ActiveUsers.IsLoggedIn(u._id) ?
                    OnlineStatus.Online :
                    OnlineStatus.Offline;
                listed.Add(new ListedUser(u, status));
            }

            return listed;
        }
    }
}
