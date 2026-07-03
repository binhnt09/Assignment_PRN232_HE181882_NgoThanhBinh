using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RV_Assignment_1.Models;

public partial class MemberAccount
{
    [Key]
    public int AccountId { get; set; }
    [Required]
    public string FullName { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    public int Role { get; set; }
    [Required]
    public string Password { get; set; } = null!;
}
