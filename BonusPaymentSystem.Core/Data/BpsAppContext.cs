﻿using BonusPaymentSystem.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Data
{
    public class BpsAppContext : DbContext
    {

        public BpsAppContext(string connectionString) : base(GetOptions(connectionString))
        {
        }

        public BpsAppContext(DbContextOptions<BpsAppContext> options)
            : base(options)
        {
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCampaing>()
                .HasKey(nameof(UserCampaing.CampaingId), nameof(UserCampaing.SallerId));
        }

        public DbSet<ApplicationUser> AppUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCampaing>  UserCampaings { get; set; }
        public DbSet<Campaing> Campaings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Parameter> Parmeters { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Rol> Rols { get; set; }
        //public DbSet<RoleUser> RoleUsers { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<Login> Logins { get; set; }


    }
}
