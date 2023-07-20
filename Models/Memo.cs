using System;
using System.Collections.Generic;

namespace ProDiaryApplication.Models
{
    public partial class Memo
    {
        public int MemoId { get; set; }
        public string MemoTitle { get; set; } = null!;
        public string MemoContent { get; set; } = null!;
        public byte[]? MemoAttachments { get; set; }
        public DateTime? MemoCreated { get; set; }
        public DateTime? MemoUpdated { get; set; }
        public string MemoAuthor { get; set; } = null!;
    }
}
