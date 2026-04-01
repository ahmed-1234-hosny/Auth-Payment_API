using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Data
{
	public class AppDBContext : DbContext
	{
		public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
		{
		}
		public DbSet<Payment> Payment { get; set; }
		public DbSet<PaymentItem> Items { get; set; }	
		public DbSet<User> Users { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }
		
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<User>()
		  .HasIndex(u => u.Email)
		  .IsUnique();

			builder.Entity<Payment>()
				.Property(p => p.TotalAmount)
				.HasColumnType("decimal(18,2)");

			builder.Entity<PaymentItem>()
				.Property(p => p.Price)
				.HasColumnType("decimal(18,2)");
		}
	}
}
