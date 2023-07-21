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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=localhost;database=DiaryNote;uid=sa;pwd=123;TrustServerCertificate=true");
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
                    .HasConstraintName("FK__MemoAddit__MemoI__5CD6CB2B");
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
                    .HasConstraintName("FK__MemoTag__MemoID__4E88ABD4");

                entity.HasOne(d => d.Tag)
                    .WithMany()
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK__MemoTag__TagID__4F7CD00D");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.Property(e => e.TagName).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
