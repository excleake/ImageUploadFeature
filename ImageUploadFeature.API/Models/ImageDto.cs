namespace ImageUploadFeature.API.Models
{
    public class ImageDto
    {
        public Guid Id { get; set; }
        public string Base64 { get; set; } = default!;
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
}
