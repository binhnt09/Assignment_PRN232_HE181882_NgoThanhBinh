using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RV_Assignment_1.Models;

public partial class BulletinPost
{
    [Key]
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public string? Summary { get; set; }
    [Required]
    public string Content { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? Source { get; set; }

    public int Status { get; set; }

    public int CategoryId { get; set; }

    public int CreatedById { get; set; }

    public virtual BulletinCategory Category { get; set; } = null!;
}
