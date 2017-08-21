using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using InMemoryDB.Models;

namespace InMemoryDB.Data
{
    public class DbInitializer
    {
        public static void Initializer(UserManager<ApplicationUser> userManager)
        {
            var aUser = new ApplicationUser
            {

                UserName = "student@test.com",
                Email = "student@test.com",
            };

            var r = userManager.CreateAsync(aUser, "Pa$$w0rd");

        }
    }
}
