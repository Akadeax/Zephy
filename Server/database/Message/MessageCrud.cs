using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.Messages
{
    class MessageCrud : MongoCrud<Message>
    {
        public const string COLLECTION_NAME = "messages";

        public MessageCrud(string database) : base (database, COLLECTION_NAME)
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
