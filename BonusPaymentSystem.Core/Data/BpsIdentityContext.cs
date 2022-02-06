using BonusPaymentSystem.Core.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Data
{
    public class BpsIdentityContext : IdentityDbContext<ApplicationUser>
    {
        public BpsIdentityContext(string connectionString) : base(GetOptions(connectionString))
        {
        }

        public BpsIdentityContext(DbContextOptions<BpsIdentityContext> options)
            : base(options)
        {
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }
    }
}
