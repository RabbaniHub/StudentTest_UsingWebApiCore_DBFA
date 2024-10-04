using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StudentTestUsingWebApiCoreDBFA.Models;

public partial class StudentDbContext : DbContext
{
    public StudentDbContext()
    {
    }

    public StudentDbContext(DbContextOptions<StudentDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<TestSubmission> TestSubmissions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=AHMAD\\SQLEXPRESS;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasIndex(e => e.QuestionId, "IX_Answers_QuestionId");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers).HasForeignKey(d => d.QuestionId);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("students");
        });

        modelBuilder.Entity<TestSubmission>(entity =>
        {
            entity.HasKey(e => e.SelectedAnswerId);

            entity.HasIndex(e => e.QuestionId, "IX_TestSubmissions_QuestionId");

            entity.HasOne(d => d.Question).WithMany(p => p.TestSubmissions).HasForeignKey(d => d.QuestionId);
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_UserLogins_UserId").IsUnique();

            entity.HasOne(d => d.User).WithOne(p => p.UserLogin).HasForeignKey<UserLogin>(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
