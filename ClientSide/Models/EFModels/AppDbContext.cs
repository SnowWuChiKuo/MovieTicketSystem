using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ClientSide.Models.EFModels
{
	public partial class AppDbContext : DbContext
	{
		public AppDbContext()
			: base("name=AppDbContext5")
		{
		}

		public virtual DbSet<CartItem> CartItems { get; set; }
		public virtual DbSet<Cart> Carts { get; set; }
		public virtual DbSet<Coupon> Coupons { get; set; }
		public virtual DbSet<Genre> Genres { get; set; }
		public virtual DbSet<Member> Members { get; set; }
		public virtual DbSet<Movy> Movies { get; set; }
		public virtual DbSet<OrderItem> OrderItems { get; set; }
		public virtual DbSet<Order> Orders { get; set; }
		public virtual DbSet<Price> Prices { get; set; }
		public virtual DbSet<Rating> Ratings { get; set; }
		public virtual DbSet<Review> Reviews { get; set; }
		public virtual DbSet<Screening> Screenings { get; set; }
		public virtual DbSet<Seat> Seats { get; set; }
		public virtual DbSet<SeatStatu> SeatStatus { get; set; }
		public virtual DbSet<Theater> Theaters { get; set; }
		public virtual DbSet<Ticket> Tickets { get; set; }
		public virtual DbSet<TicketSeat> TicketSeats { get; set; }
		public virtual DbSet<User> Users { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Genre>()
				.HasMany(e => e.Movies)
				.WithRequired(e => e.Genre)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Member>()
				.Property(e => e.Account)
				.IsUnicode(false);

			modelBuilder.Entity<Member>()
				.Property(e => e.Email)
				.IsUnicode(false);

			modelBuilder.Entity<Member>()
				.Property(e => e.PasswordHash)
				.IsUnicode(false);

			modelBuilder.Entity<Member>()
				.Property(e => e.ConfirmCode)
				.IsUnicode(false);

			modelBuilder.Entity<Member>()
				.HasMany(e => e.Carts)
				.WithRequired(e => e.Member)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Member>()
				.HasMany(e => e.Orders)
				.WithRequired(e => e.Member)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Member>()
				.HasMany(e => e.Reviews)
				.WithRequired(e => e.Member)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Movy>()
				.HasMany(e => e.Prices)
				.WithRequired(e => e.Movy)
				.HasForeignKey(e => e.MovieId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Movy>()
				.HasMany(e => e.Reviews)
				.WithRequired(e => e.Movy)
				.HasForeignKey(e => e.MovieId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Movy>()
				.HasMany(e => e.Screenings)
				.WithRequired(e => e.Movy)
				.HasForeignKey(e => e.MovieId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Order>()
				.HasMany(e => e.OrderItems)
				.WithRequired(e => e.Order)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Rating>()
				.HasMany(e => e.Movies)
				.WithRequired(e => e.Rating)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Screening>()
				.HasMany(e => e.SeatStatus)
				.WithRequired(e => e.Screening)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Screening>()
				.HasMany(e => e.Tickets)
				.WithRequired(e => e.Screening)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Seat>()
				.Property(e => e.Row)
				.IsFixedLength()
				.IsUnicode(false);

			modelBuilder.Entity<Seat>()
				.HasMany(e => e.SeatStatus)
				.WithRequired(e => e.Seat)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Seat>()
				.HasMany(e => e.TicketSeats)
				.WithRequired(e => e.Seat)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Theater>()
				.HasMany(e => e.Screenings)
				.WithRequired(e => e.Theater)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Theater>()
				.HasMany(e => e.Seats)
				.WithRequired(e => e.Theater)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Ticket>()
				.HasMany(e => e.CartItems)
				.WithRequired(e => e.Ticket)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Ticket>()
				.HasMany(e => e.OrderItems)
				.WithRequired(e => e.Ticket)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Ticket>()
				.HasMany(e => e.TicketSeats)
				.WithRequired(e => e.Ticket)
				.HasForeignKey(e => e.SeatId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<User>()
				.Property(e => e.Account)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.Email)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.PasswordHash)
				.IsUnicode(false);
		}
	}
}
