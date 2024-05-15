using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TaskMgmtApi.Context;

public partial class TaskMgmtContext : DbContext
{
    public TaskMgmtContext()
    {
    }

    public TaskMgmtContext(DbContextOptions<TaskMgmtContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<StaffInTask> StaffInTasks { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Staff>(entity =>
        {
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.NameNonUnicode).HasMaxLength(100);
            entity.Property(e => e.ShortName).HasMaxLength(100);
        });

        modelBuilder.Entity<StaffInTask>(entity =>
        {
            entity.ToTable("StaffInTask");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.ToTable("Task");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Label).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
