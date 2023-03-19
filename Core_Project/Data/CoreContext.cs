using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Project.Data
{
    public class CoreContext : DbContext
    {
        public CoreContext(DbContextOptions<CoreContext> options)
        : base(options)
        {
        }
        public DbSet<Core_Project.DataBase.CF> CF { get; set; }
        public DbSet<Core_Project.DataBase.IDB_COURS> IDB_COURS { get; set; }
        public DbSet<Core_Project.DataBase.CORE_STUDENTS> CORE_STUDENTS { get; set; }

    }
}
