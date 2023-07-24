using System;
using System.Collections.Generic;

namespace ProDiaryApplication.Models
{
    public partial class Account
    {
        public Account()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public byte[]? Avatar { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
