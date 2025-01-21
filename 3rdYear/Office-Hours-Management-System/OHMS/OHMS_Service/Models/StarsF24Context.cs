using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OHMS_Service.Models;

public partial class StarsF24Context : DbContext
{
    public StarsF24Context()
    {
    }

    public StarsF24Context(DbContextOptions<StarsF24Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Calendar> Calendars { get; set; }

    public virtual DbSet<OfficeHour> OfficeHours { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=MyConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Appointment>(entity =>
    {
        entity.HasKey(e => e.Id).HasName("PK__Appointm__3213E83FA2F396D1");

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()  // Auto-generated value on insert
            .HasColumnName("id");
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.EndDate).HasColumnType("datetime");
        entity.Property(e => e.StartDate).HasColumnType("datetime");
        entity.Property(e => e.Title).HasMaxLength(500);

        entity.HasOne(d => d.Calendar).WithMany(p => p.Appointments)
            .HasForeignKey(d => d.CalendarId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Appointments_CalendarId");

        entity.HasOne(d => d.Student).WithMany(p => p.Appointments)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Appointments_StudentId");
    });

    modelBuilder.Entity<Calendar>(entity =>
    {
        entity.HasKey(e => e.Id).HasName("PK__Calendar__3213E83FD79C68C0");

        entity.HasIndex(e => e.CalendarCode, "UQ__Calendar__8DDBB9FDD99AB099").IsUnique();

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()  // Auto-generated value on insert
            .HasColumnName("id");
        entity.Property(e => e.CalendarCode).HasMaxLength(6);
        entity.Property(e => e.Name).HasMaxLength(100);

        entity.HasOne(d => d.Teacher).WithMany(p => p.Calendars)
            .HasForeignKey(d => d.TeacherId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Calendars_TeacherId");
    });

    modelBuilder.Entity<OfficeHour>(entity =>
    {
        entity.HasKey(e => e.Id).HasName("PK__OfficeHo__3213E83F3E869EA1");

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()  // Auto-generated value on insert
            .HasColumnName("id");
        
        entity.Property(e => e.EndDate)
            .HasColumnType("time(1)")
            .IsRequired();

        entity.Property(e => e.StartDate)
            .HasColumnType("time(1)")
            .IsRequired();

        // Add DayOfWeek property
        entity.Property(e => e.DayOfWeek)
            .HasColumnType("nvarchar(50)")
            .IsRequired();

        entity.HasOne(d => d.Calendar).WithMany(p => p.OfficeHours)
            .HasForeignKey(d => d.CalendarId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_OfficeHours_CalendarId");
    });

    modelBuilder.Entity<Student>(entity =>
    {
        entity.HasKey(e => e.Id).HasName("PK__Students__3213E83F38A42CFC");

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()  // Auto-generated value on insert
            .HasColumnName("id");
        entity.Property(e => e.Email).HasMaxLength(100);
        entity.Property(e => e.Name).HasMaxLength(100);

        entity.HasMany(d => d.Calendars).WithMany(p => p.Students)
            .UsingEntity<Dictionary<string, object>>(
                "StudentCalendar",
                r => r.HasOne<Calendar>().WithMany()
                    .HasForeignKey("CalendarId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentCalendars_CalendarId"),
                l => l.HasOne<Student>().WithMany()
                    .HasForeignKey("StudentId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentCalendars_StudentId"),
                j =>
                {
                    j.HasKey("StudentId", "CalendarId").HasName("PK__StudentC__F7F9D7DDB5C91C1A");
                    j.ToTable("StudentCalendars");
                });
    });

    modelBuilder.Entity<Teacher>(entity =>
    {
        entity.HasKey(e => e.Id).HasName("PK__Teachers__3213E83FACB62D68");

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()  // Auto-generated value on insert
            .HasColumnName("id");
        entity.Property(e => e.Email).HasMaxLength(100);
        entity.Property(e => e.Name).HasMaxLength(100);
    });

    OnModelCreatingPartial(modelBuilder);
}

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
