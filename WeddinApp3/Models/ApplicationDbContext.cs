using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeddinApp3.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //Setting up Db context to allow access to the database and store data
        //passing options to the base class to initialise a new instance
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //setting up databases in SQL Server 
        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Venue> Venues { get; set; }

    }
}
