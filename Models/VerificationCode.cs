using System;
using System.Collections.Generic;

namespace ProDiaryApplication.Models
{
    public partial class VerificationCode
    {
        public int VerifyId { get; set; }
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime? Created { get; set; }
    }
}
