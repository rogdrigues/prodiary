using System;
using System.Collections.Generic;

namespace ProDiaryApplication.Models
{
    public partial class MemoAddition
    {
        public int MemoAdditionId { get; set; }
        public int? MemoId { get; set; }
        public byte[]? MemoAttachments { get; set; }
        public string? MemoContentAddition { get; set; }

        public virtual Memo? Memo { get; set; }
    }
}
