using System.ComponentModel.DataAnnotations;

namespace ImageUploadFeature.API.Entities
{
    public class Image
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; } = default!;

        [Required]
        public string Base64 { get; set; } = default!;

        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
}
