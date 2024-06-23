
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Contexts
{
    public class MvcProjectDbContext : IdentityDbContext<IdentityUser>
    {
        public MvcProjectDbContext(DbContextOptions<MvcProjectDbContext> options): base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-many relationship User Make Orders
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            // Configure the one-to-many relationship Instructor Add Courses
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(u => u.Courses)
                .HasForeignKey(c => c.InstructorId);

            //Configure the one to many relationship between seller and products 
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(U => U.Products)
                .HasForeignKey(p => p.SellerID);
            /////////////////////////////////////////////////////////////
            modelBuilder.Entity<SessionBid>()
  .HasOne(s => s.Photographer)
  .WithMany(u => u.PhotographerSessions)
  .HasForeignKey(s => s.PhotographerId)
  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SessionBid>()
                .HasOne(s => s.Client)
                .WithMany(u => u.ClientSessions)
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SessionBid>().ToTable("SessionBids");

            modelBuilder.Entity<Comment>()
           .HasOne(c => c.User)
           .WithMany(u => u.Comments)
           .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.SessionBid)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.SessionBidId);
            /////////////////////////////////////////////////////////////////////////////////////////

         modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.PhotographerImages)
            .WithOne(p => p.Photographer)
            .HasForeignKey(p => p.PhotographerId);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        
        public DbSet<Course> Courses { get; set; }
        
        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }

        public DbSet<PhotographerImages> PhotographerImages { get; set; }

        public DbSet<SessionBid> SessionBids { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Payment> Payments { get; set; }

    }

}

