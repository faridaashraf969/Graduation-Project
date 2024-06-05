
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
            ///////
            modelBuilder.Entity<SessionRequest>()
             .HasOne(sr => sr.Photographer)
             .WithMany(p => p.CreatedSessionRequests)
             .HasForeignKey(sr => sr.PhotographerId)
             .OnDelete(DeleteBehavior.Restrict);
            //////
            modelBuilder.Entity<SessionRequest>()
               .HasOne(sr => sr.Client)
               .WithMany(c => c.CreatedSessionRequests)
               .HasForeignKey(sr => sr.ClientId)
               .OnDelete(DeleteBehavior.Restrict);
            /////////////
            modelBuilder.Entity<Proposal>()
              .HasOne(p => p.Photographer)
              .WithMany(p => p.Proposals)
              .HasForeignKey(p => p.PhotographerId)
              .OnDelete(DeleteBehavior.Restrict);
            /////////////////
            modelBuilder.Entity<Proposal>()
                 .HasOne(p => p.Client)
                 .WithMany(c => c.Proposals)
                 .HasForeignKey(p => p.ClientId)
                 .OnDelete(DeleteBehavior.Restrict);
            //////////////
            modelBuilder.Entity<Proposal>()
                .HasOne(p => p.SessionRequest)
                .WithMany(sr => sr.Proposals)
                .HasForeignKey(p => p.SessionRequestId)
                .OnDelete(DeleteBehavior.Cascade);
            //////////////
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
            //////////////
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        
        public DbSet<Course> Courses { get; set; }
        //public DbSet<OrderDetail> OrderDetail { get; set; }
        //public DbSet<OrderHeader> OrderHeader { get; set; }
        //public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
      }

    }

