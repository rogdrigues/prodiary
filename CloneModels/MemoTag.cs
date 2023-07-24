using System;
using System.Collections.Generic;

namespace ProDiaryApplication.CloneModels
{
    public partial class MemoTag
    {
        public int? MemoId { get; set; }
        public int? TagId { get; set; }

        public virtual Memo? Memo { get; set; }
        public virtual Tag? Tag { get; set; }
    }
}
