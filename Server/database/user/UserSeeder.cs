﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Database.User
{
    public class UserSeeder : MongoSeeder
    {
        readonly UserCrud userCrud = new UserCrud();

        public override void Seed(SeederEntriesAmount amount)
        {
            if (amount.userSeederAmount < 1 || amount.userSeederAmount > MAX_SEED)
                throw new ArgumentException("out of bounds", "amount");

            if (userCrud.DocumentCount > 0) return;

            for (int i = 0; i < amount.userSeederAmount; i++)
            {
                string name = Faker.Name.FullName(Faker.NameFormats.Standard);
                userCrud.CreateOne(new User
                {
                    identifier = name.Replace(" ", "_").ToLower(),
                    fullName = name,
                    // sha512 hash of "test" with salt "zephy"
                    password = "$6$zephy$SBy0yf3Se8dxEXxzsBS1USPE3uWhTKzAZeRQo2xY87qSSha5X7oiJJEU64/wxsk9SJIwNKA/UDNy3zX.GJnjR1",
                    status = Faker.Company.CatchPhrase(),
                });
            }
        }
    }
}
