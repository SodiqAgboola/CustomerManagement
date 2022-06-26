using CustomerManagementSystem.HelperMethods;
using CustomerManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(
         DbContextOptions<ApplicationDBContext> options) 
            : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<OtpValidation> OtpValidations { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyUtcDateTimeConverter();

            #region Seed Data

            var locations = new List<Location>()
             {
                 new Location
                 {
                     Id = 1,
                     CountryName = "Nigeria",
                     CountryCode = "116",
                     StateName = "LAGOS",
                     StateCode = "36",
                     LgaName = "EPE",
                     LgaCode = "754"
                 },
                 new Location
                 {
                     Id = 2,
                     CountryName = "Nigeria",
                     CountryCode = "116",
                     StateName = "LAGOS",
                     StateCode = "36",
                     LgaName = "IBEJU LEKKI",
                     LgaCode = "753"
                 },
                 new Location
                 {
                     Id = 3,
                     CountryName = "Nigeria",
                     CountryCode = "116",
                     StateName = "LAGOS",
                     StateCode = "36",
                     LgaName = "LAGOS MAINLAND",
                     LgaCode = "748"
                 }

             };

            for (int i = 0; i < locations.Count(); i++)
            {
                var location = locations[i];
                builder.Entity<Location>().HasData(location);
            }
            #endregion

        }
    }
}
