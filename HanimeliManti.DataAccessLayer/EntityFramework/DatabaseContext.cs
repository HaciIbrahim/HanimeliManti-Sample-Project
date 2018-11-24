using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HanimeliManti.Entities;

namespace HanimeliManti.DataAccessLayer.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public DbSet<HanimeliUser> HanimeliUsers { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Liked> Likes { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }

    }
}
