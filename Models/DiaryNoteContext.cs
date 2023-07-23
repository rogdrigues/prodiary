using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

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
        public virtual DbSet<VerificationCode> VerificationCodes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                                          .SetBasePath(Directory.GetCurrentDirectory())
                                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyCnn"));
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

            modelBuilder.Entity<VerificationCode>(entity =>
            {
                entity.HasKey(e => e.VerifyId)
                    .HasName("PK__Verifica__0A2710A9BEADE697");

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
