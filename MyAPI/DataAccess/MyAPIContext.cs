using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MyAPI.Models;

namespace MyAPI.DataAccess
{
    public class MyAPIContext : DbContext
    {
        public MyAPIContext(DbContextOptions<MyAPIContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.username);
            });
        }
    }
}
