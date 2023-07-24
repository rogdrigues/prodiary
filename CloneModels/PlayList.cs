using System;
using System.Collections.Generic;

namespace ProDiaryApplication.CloneModels
{
    public partial class PlayList
    {
        public int Id { get; set; }
        public string? PlayListName { get; set; }
        public int? Owner { get; set; }
    }
}
