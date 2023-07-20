using System;
using System.Collections.Generic;

namespace ProDiaryApplication.Models
{
    public partial class Account
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public byte[]? Avatar { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
