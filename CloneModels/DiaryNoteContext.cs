using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProDiaryApplication.CloneModels
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
        public virtual DbSet<MemoTag> MemoTags { get; set; } = null!;
        public virtual DbSet<PlayList> PlayLists { get; set; } = null!;
        public virtual DbSet<PlayListSong> PlayListSongs { get; set; } = null!;
        public virtual DbSet<Singer> Singers { get; set; } = null!;
        public virtual DbSet<Song> Songs { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;

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

            modelBuilder.Entity<MemoTag>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MemoTag");

                entity.Property(e => e.MemoId).HasColumnName("MemoID");

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.HasOne(d => d.Memo)
                    .WithMany()
                    .HasForeignKey(d => d.MemoId)
                    .HasConstraintName("FK__MemoTag__MemoID__3F466844");

                entity.HasOne(d => d.Tag)
                    .WithMany()
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK__MemoTag__TagID__403A8C7D");
            });

            modelBuilder.Entity<PlayList>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PlayList");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PlayListName).HasMaxLength(150);
            });

            modelBuilder.Entity<PlayListSong>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PlayListSong");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PlayListId).HasColumnName("PlayListID");

                entity.Property(e => e.SongId).HasColumnName("SongID");
            });

            modelBuilder.Entity<Singer>(entity =>
            {
                entity.ToTable("Singer");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SingerName).HasMaxLength(150);
            });

            modelBuilder.Entity<Song>(entity =>
            {
                entity.ToTable("Song");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(500)
                    .HasColumnName("ImageURL");

                entity.Property(e => e.Time).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.HasOne(d => d.AuthorNavigation)
                    .WithMany(p => p.Songs)
                    .HasForeignKey(d => d.Author)
                    .HasConstraintName("FK_Song_Singer");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Songs)
                    .HasForeignKey(d => d.Owner)
                    .HasConstraintName("FK_Song_Account");
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
