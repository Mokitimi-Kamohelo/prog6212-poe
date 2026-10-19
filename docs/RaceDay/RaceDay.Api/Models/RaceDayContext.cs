using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RaceDay.Api.Models;

public partial class RaceDayContext : DbContext
{
    public RaceDayContext(DbContextOptions<RaceDayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Enrolment> Enrolments { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventType> EventTypes { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.CategoryType).HasMaxLength(20);
            entity.Property(e => e.CategoryValue).HasMaxLength(50);

            entity.HasOne(d => d.Event).WithMany(p => p.Categories)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Categories_Events");
        });

        modelBuilder.Entity<Enrolment>(entity =>
        {
            entity.HasIndex(e => new { e.ParticipantId, e.EventId }, "UQ_Enrolments_Participant_Event").IsUnique();

            entity.Property(e => e.EnrolmentDate).HasDefaultValueSql("(sysdatetime())", "DF_Enrolments_EnrolmentDate");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Registered", "DF_Enrolments_Status");

            entity.HasOne(d => d.Category).WithMany(p => p.Enrolments)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrolments_Category");

            entity.HasOne(d => d.Event).WithMany(p => p.Enrolments)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrolments_Event");

            entity.HasOne(d => d.Participant).WithMany(p => p.Enrolments)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrolments_Participant");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())", "DF_Events_CreatedAt");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Distance).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.EventName).HasMaxLength(150);
            entity.Property(e => e.Location).HasMaxLength(200);

            entity.HasOne(d => d.EventType).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Events_EventType");

            entity.HasOne(d => d.Organiser).WithMany(p => p.Events)
                .HasForeignKey(d => d.OrganiserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Events_Organiser");
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.HasIndex(e => e.TypeName, "UQ_EventTypes_TypeName").IsUnique();

            entity.Property(e => e.TypeName).HasMaxLength(20);
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasIndex(e => e.EnrolmentId, "UQ_Results_Enrolment").IsUnique();

            entity.HasOne(d => d.Enrolment).WithOne(p => p.Result)
                .HasForeignKey<Result>(d => d.EnrolmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Results_Enrolments");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.RoleName, "UQ_Roles_RoleName").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(20);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email, "UQ_Users_Email").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())", "DF_Users_CreatedAt");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
