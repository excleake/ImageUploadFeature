using System.ComponentModel.DataAnnotations;

namespace ImageUploadFeature.API.Entities
{
    public abstract class Profile
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = default!;

        public List<Image> Images { get; set; } = [];
    }
}
