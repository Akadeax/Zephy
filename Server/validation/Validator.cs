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
            User user = uc.ReadOne(name);

            return (user != null);
        }

    }
}
