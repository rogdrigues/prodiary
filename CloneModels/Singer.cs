using System;
using System.Collections.Generic;

namespace ProDiaryApplication.CloneModels
{
    public partial class Singer
    {
        public Singer()
        {
            Songs = new HashSet<Song>();
        }

        public int Id { get; set; }
        public string? SingerName { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
    }
}
