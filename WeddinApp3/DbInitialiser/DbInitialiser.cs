using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeddinApp3.Models;
using WeddinApp3.Utility;

namespace WeddinApp3.DbInitialiser
{
    public class DbInitialiser : IDbInitialiser
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
 

        public DbInitialiser(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)

        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        
        // method to check migrations are updated and id admin role has been created 
        public void Initialise()
        {
            try
            { //If condition to check all migrations have been pushed to the database
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }catch (Exception) {

            }
            // If admin role is included return back, if not, create all roles 
            if (_db.Roles.Any(x=>x.Name == Utility.Helper.Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Supplier)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Couple)).GetAwaiter().GetResult();

            //create new admin user 
            _userManager.CreateAsync(new ApplicationUser
            {

                UserName = "admin@gmail2.com",
                Email = "admin@gmail2.com",
                EmailConfirmed = true,
                Name = "AdminUser2"

            }, "Admin123&456").GetAwaiter().GetResult();

            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Email == "admin@gmail2.com");
            _userManager.AddToRoleAsync(user, Helper.Admin).GetAwaiter().GetResult(); 
  
        }
    }
}
