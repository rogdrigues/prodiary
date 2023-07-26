using System;
using System.Collections.Generic;

namespace ProDiaryApplication.CloneModels
{
    public partial class PlayListSong
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public int PlayListId { get; set; }
        public int? Status { get; set; }

        public virtual PlayList PlayList { get; set; } = null!;
        public virtual Song Song { get; set; } = null!;
    }
}
