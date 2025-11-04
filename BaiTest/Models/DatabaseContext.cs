using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BaiTest.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ExamAssignment> ExamAssignments { get; set; }

    public virtual DbSet<ExamRoom> ExamRooms { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-K2L2LVGO;Initial Catalog=Test;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExamAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExamAssi__3214EC07E8F3B693");

            entity.Property(e => e.AssignedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Room).WithMany(p => p.ExamAssignments)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_Assignment_Room");

            entity.HasOne(d => d.Student).WithMany(p => p.ExamAssignments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Assignment_Student");
        });

        modelBuilder.Entity<ExamRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExamRoom__3214EC07A27D4B5D");

            entity.Property(e => e.RoomCode).HasMaxLength(20);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC075E8CCDF1");

            entity.Property(e => e.Class).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.StudentCode).HasMaxLength(20);
            entity.Property(e => e.Subject).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
