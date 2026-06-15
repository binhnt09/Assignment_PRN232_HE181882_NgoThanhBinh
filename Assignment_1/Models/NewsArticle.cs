using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment_1.Models;

public partial class NewsArticle
{
    [Key]
    [Required(ErrorMessage = "Mã bài viết không được để trống")]
    [StringLength(20, ErrorMessage = "Mã bài viết không được vượt quá 20 ký tự")]
    public string? NewsArticleId { get; set; }

    [Required(ErrorMessage = "Tiêu đề không được để trống")]
    [StringLength(400, ErrorMessage = "Tiêu đề quá dài")]
    public string? NewsTitle { get; set; }

    [Required(ErrorMessage = "Tiêu đề phụ không được để trống")]
    public string? Headline { get; set; }

    [Required(ErrorMessage = "Ngày tạo không được để trống")]
    public DateTime? CreatedDate { get; set; }

    public string? NewsContent { get; set; }

    public string? NewsSource { get; set; }

    public short? CategoryId { get; set; }

    public bool? NewsStatus { get; set; }

    public short? CreatedById { get; set; }

    public short? UpdatedById { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Category? Category { get; set; }

    public virtual SystemAccount? CreatedBy { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
