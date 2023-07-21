using System;
using System.Collections.Generic;

namespace ProDiaryApplication.Models
{
    public partial class Memo
    {
        public Memo()
        {
            MemoAdditions = new HashSet<MemoAddition>();
        }

        public int MemoId { get; set; }
        public string MemoTitle { get; set; } = null!;
        public string MemoContent { get; set; } = null!;
        public byte[]? MemoAttachments { get; set; }
        public DateTime? MemoCreated { get; set; }
        public DateTime? MemoUpdated { get; set; }
        public string MemoAuthor { get; set; } = null!;

        public virtual ICollection<MemoAddition> MemoAdditions { get; set; }
    }
}
