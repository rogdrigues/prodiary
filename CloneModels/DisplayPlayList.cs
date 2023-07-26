using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDiaryApplication.CloneModels
{
    internal class DisplayPlayList
    {
        public int Id { get; set; }
        public string? PlayListName { get; set; }
        public int? Owner { get; set; }
        public bool? IsClicked { get; set; }

    }
}
