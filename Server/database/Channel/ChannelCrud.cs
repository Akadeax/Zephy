using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.Channels
{
    class ChannelCrud : MongoCrud<Channel>
    {
        public const string COLLECTION_NAME = "channels";

        public ChannelCrud(string database) : base(database, COLLECTION_NAME)
        {

        }

        public void Create()
        {

        }

        public void Update()
        {

        }

        public void Delete()
        {

        }

        public void ReadOne()
        {

        }

        public void ReadMany()
        {

        }
    }
}
