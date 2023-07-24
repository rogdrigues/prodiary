using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProDiaryApplication.Models
{
    public partial class DiaryNoteContext : DbContext
    {
        public DiaryNoteContext()
        {
        }

        public DiaryNoteContext(DbContextOptions<DiaryNoteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Memo> Memos { get; set; } = null!;
        public virtual DbSet<MemoAddition> MemoAdditions { get; set; } = null!;
        public virtual DbSet<MemoTag> MemoTags { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<VerificationCode> VerificationCodes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=localhost;database=DiaryNote;uid=sa;pwd=sa;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Username).HasMaxLength(100);
            });

            modelBuilder.Entity<Memo>(entity =>
            {
                entity.ToTable("Memo");

                entity.Property(e => e.MemoId).HasColumnName("MemoID");

                entity.Property(e => e.MemoAuthor).HasMaxLength(255);

                entity.Property(e => e.MemoCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MemoTitle).HasMaxLength(255);

                entity.Property(e => e.MemoUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MemoAddition>(entity =>
            {
                entity.ToTable("MemoAddition");

                entity.Property(e => e.MemoAdditionId).HasColumnName("MemoAdditionID");

                entity.Property(e => e.MemoId).HasColumnName("MemoID");

                entity.HasOne(d => d.Memo)
                    .WithMany(p => p.MemoAdditions)
                    .HasForeignKey(d => d.MemoId)
                    .HasConstraintName("FK__MemoAddit__MemoI__2B3F6F97");
            });

            modelBuilder.Entity<MemoTag>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MemoTag");

                entity.Property(e => e.MemoId).HasColumnName("MemoID");

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.HasOne(d => d.Memo)
                    .WithMany()
                    .HasForeignKey(d => d.MemoId)
                    .HasConstraintName("FK__MemoTag__MemoID__34C8D9D1");

                entity.HasOne(d => d.Tag)
                    .WithMany()
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK__MemoTag__TagID__35BCFE0A");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.Property(e => e.TagName).HasMaxLength(255);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasKey(e => e.TaskId)
                    .HasName("PK__Task__7C6949D1AF37F02F");

                entity.ToTable("Task");

                entity.Property(e => e.TaskId).HasColumnName("TaskID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TaskBegin).HasColumnType("datetime");

                entity.Property(e => e.TaskDate).HasColumnType("datetime");

                entity.Property(e => e.TaskEnd).HasColumnType("datetime");

                entity.Property(e => e.TaskStatus).HasMaxLength(255);

                entity.Property(e => e.TaskTitle).HasMaxLength(255);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Task__ID__2E1BDC42");
            });

            modelBuilder.Entity<VerificationCode>(entity =>
            {
                entity.HasKey(e => e.VerifyId)
                    .HasName("PK__Verifica__0A2710A9CD37FB23");

                entity.ToTable("VerificationCode");

                entity.Property(e => e.VerifyId).HasColumnName("VerifyID");

                entity.Property(e => e.Code)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
