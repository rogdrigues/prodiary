using System;
using System.Collections.Generic;

namespace ProDiaryApplication.CloneModels
{
    public partial class PlayList
    {
        public PlayList()
        {
            PlayListSongs = new HashSet<PlayListSong>();
        }

        public int Id { get; set; }
        public string? PlayListName { get; set; }
        public int? Owner { get; set; }

        public virtual ICollection<PlayListSong> PlayListSongs { get; set; }
    }
}
