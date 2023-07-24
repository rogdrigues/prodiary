using System;
using System.Collections.Generic;

namespace ProDiaryApplication.Models
{
    public partial class Task
    {
        public int TaskId { get; set; }
        public string TaskTitle { get; set; } = null!;
        public string TaskContent { get; set; } = null!;
        public string? TaskStatus { get; set; }
        public DateTime? TaskDate { get; set; }
        public DateTime? TaskBegin { get; set; }
        public DateTime? TaskEnd { get; set; }
        public int Id { get; set; }

        public virtual Account IdNavigation { get; set; } = null!;
    }
}
