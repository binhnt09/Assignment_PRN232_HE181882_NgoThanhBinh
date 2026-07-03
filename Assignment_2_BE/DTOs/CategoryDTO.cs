namespace Assignment_2_BE.DTOs
{
    public class CategoryDTO
    {
        public short CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategoryDesciption { get; set; } = null!;
        public short? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; }
    }
}
