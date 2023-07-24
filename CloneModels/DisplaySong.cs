using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDiaryApplication.CloneModels
{
    class DisplaySong
    {
        public string Title { get; set; }
        public string ID { get; set; }
        public int? Owner { get; set; }
        public int? Category { get; set; }
        public string Status { get; set; }


        public string Author { get; set; }
        public int AuthorID { get; set; }

        public string Time { get; set; }
        public string? ImageURL { get; set; }

        public string? LinkToFile { get; set; }
        public bool? IsClicked { get; set; }


    }
}
