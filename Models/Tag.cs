using System;
using System.Collections.Generic;

namespace ProDiaryApplication.Models
{
    public partial class Tag
    {
        public int TagId { get; set; }
        public string TagName { get; set; } = null!;
    }
}
