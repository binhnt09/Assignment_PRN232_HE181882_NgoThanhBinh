namespace Assignment_2_BE.DTOs
{
    public class SystemAccountResponseDTO
    {
        public short AccountId { get; set; }
        public string? AccountName { get; set; }
        public string? AccountEmail { get; set; }
        public int? AccountRole { get; set; }
        // We DO NOT include AccountPassword here for security reasons!
    }
}
