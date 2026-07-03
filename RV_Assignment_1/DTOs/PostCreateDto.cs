using System.ComponentModel.DataAnnotations;

namespace RV_Assignment_1.DTOs
{
    public class PostCreateDto
    {
        [Required(ErrorMessage = "Title không được để trống!")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "Title phải từ 5 đến 150 ký tự!")]
        public string Title { get; set; } = null!;

        public string? Summary { get; set; }

        [Required(ErrorMessage = "Content không được để trống!")]
        public string Content { get; set; } = null!;

        public string? Source { get; set; }

        [Range(0, 1, ErrorMessage = "Status chỉ nhận giá trị 0 hoặc 1!")]
        public int Status { get; set; }

        public int CategoryId { get; set; }
        public int CreatedById { get; set; }
    }

}
