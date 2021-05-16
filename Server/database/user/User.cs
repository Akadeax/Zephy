using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Server.Database.User
{
    public enum OnlineStatus
    {
        Offline, Online
    }

    public class BaseUser
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string _id;
        public string fullName;
        public string status;
        public string identifier;

    }

    public class User : BaseUser
    {
        public string password;

        public BaseUser ToBaseUser()
        {
            return new BaseUser
            {
                _id = _id,
                fullName = fullName,
                status = status,
                identifier = identifier
            };
        }
    }

    public class ListedUser : BaseUser
    {
        public OnlineStatus onlineStatus;

        public ListedUser(BaseUser user, OnlineStatus onlineStatus)
        {
            _id = user._id;
            fullName = user.fullName;
            status = user.status;
            identifier = user.identifier;
            this.onlineStatus = onlineStatus;
        }
    }
}
