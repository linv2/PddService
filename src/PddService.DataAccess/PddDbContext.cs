using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using PddService.DataAccess.Entity;

namespace PddService.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class PddDbContext : DbContext
    {
        public PddDbContext(DbContextOptions<PddDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
        }

        public DbSet<PrintTemplate> PrintTemplate { get; set; }
        public DbSet<Express> Express { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Mall> Mall { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }

        public DbSet<ExpressFollow> ExpressFollow { get; set; }
        public DbSet<OrderRefund> OrderRefund { get; set; }

        public DbSet<AsyncTask> AsyncTask { get; set; }

    }
}