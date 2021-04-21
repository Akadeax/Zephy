using System;
using System.Collections.Generic;
using System.Text;

namespace server.database
{
    public abstract class MongoSeeder
    {
        public const int MAX_SEED = 10000;

        public abstract void Seed(SeederEntriesAmount amount);
    }
}
