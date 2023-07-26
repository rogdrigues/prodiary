using System;
using System.Collections.Generic;

namespace ProDiaryApplication.CloneModels
{
    public partial class Song
    {
        public Song()
        {
            PlayListSongs = new HashSet<PlayListSong>();
        }

        public int Id { get; set; }
        public string? LinkToFile { get; set; }
        public string? LinkToWeb { get; set; }
        public string? Status { get; set; }
        public int? Owner { get; set; }
        public string? Title { get; set; }
        public int? Category { get; set; }
        public int? Author { get; set; }
        public string? ImageUrl { get; set; }
        public string? Time { get; set; }

        public virtual Singer? AuthorNavigation { get; set; }
        public virtual Account? OwnerNavigation { get; set; }
        public virtual ICollection<PlayListSong> PlayListSongs { get; set; }
    }
}
