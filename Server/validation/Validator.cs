using System;
using System.Collections.Generic;
using System.Text;

namespace Server.validation
{
    class Validator
    {
        public static bool Validate(string name)
        {
            UserCrud uc = new UserCrud("Zephy");
            User user = uc.ReadRecord(x => x.name == name);

            return (user != null);
        }

    }
}
