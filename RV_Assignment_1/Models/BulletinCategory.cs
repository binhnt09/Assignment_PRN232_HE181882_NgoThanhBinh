using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RV_Assignment_1.Models;

public partial class BulletinCategory
{
    [Key]
    public int CategoryId { get; set; }
    [Required]
    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<BulletinPost> BulletinPosts { get; set; } = new List<BulletinPost>();
}
