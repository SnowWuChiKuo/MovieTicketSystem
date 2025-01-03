﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServerSide.Models.EFModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Price> Prices { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Screening> Screenings { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<SeatStatus> SeatStatuses { get; set; }

    public virtual DbSet<Theater> Theaters { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketSeat> TicketSeats { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasOne(d => d.Member).WithMany(p => p.Carts)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carts_Members");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TicketItems");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK_CartItems_Carts");

            entity.HasOne(d => d.Ticket).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Tickets");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DiscountType)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Users");

            entity.Property(e => e.Account)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ConfirmCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.Property(e => e.Cast)
                .IsRequired()
                .HasMaxLength(1000);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(2000);
            entity.Property(e => e.Director)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.PosterUrl)
                .HasMaxLength(70)
                .HasColumnName("PosterURL");
            entity.Property(e => e.ReleaseDate).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(70);
            entity.Property(e => e.TrailerUrl)
                .HasMaxLength(70)
                .HasColumnName("TrailerURL");
            entity.Property(e => e.Updated).HasColumnType("datetime");

            entity.HasOne(d => d.Genre).WithMany(p => p.Movies)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movies_Genres");

            entity.HasOne(d => d.Rating).WithMany(p => p.Movies)
                .HasForeignKey(d => d.RatingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movies_Ratings");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Coupon).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CouponId)
                .HasConstraintName("FK_Orders_Coupons");

            entity.HasOne(d => d.Member).WithMany(p => p.Orders)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Members");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.Property(e => e.TicketName)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Orders");

            entity.HasOne(d => d.Ticket).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Tickets");
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Price1).HasColumnName("Price");
            entity.Property(e => e.SalesType)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.TicketType)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Movie).WithMany(p => p.Prices)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prices_Movies");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.Property(e => e.Comment).HasMaxLength(2000);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Member).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reviews_Members");

            entity.HasOne(d => d.Movie).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reviews_Movies");
        });

        modelBuilder.Entity<Screening>(entity =>
        {
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Movie).WithMany(p => p.Screenings)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Screenings_Movies");

            entity.HasOne(d => d.Theater).WithMany(p => p.Screenings)
                .HasForeignKey(d => d.TheaterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Screenings_Theaters");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.Property(e => e.Row)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Theater).WithMany(p => p.Seats)
                .HasForeignKey(d => d.TheaterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seats_Theaters");
        });

        modelBuilder.Entity<SeatStatus>(entity =>
        {
            entity.ToTable("SeatStatus");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Screening).WithMany(p => p.SeatStatuses)
                .HasForeignKey(d => d.ScreeningId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeatStatus_Screenings");

            entity.HasOne(d => d.Seat).WithMany(p => p.SeatStatuses)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeatStatus_Seats");
        });

        modelBuilder.Entity<Theater>(entity =>
        {
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Screening).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ScreeningId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Screenings");
        });

        modelBuilder.Entity<TicketSeat>(entity =>
        {
            entity.HasOne(d => d.Seat).WithMany(p => p.TicketSeats)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketSeats_Seats");

            entity.HasOne(d => d.SeatNavigation).WithMany(p => p.TicketSeats)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketSeats_Tickets");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Users_1");

            entity.Property(e => e.Account)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(70)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}