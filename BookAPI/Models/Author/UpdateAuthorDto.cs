using System.ComponentModel.DataAnnotations;

namespace BookAPI.Models.Author
{
    public class UpdateAuthorDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public required string LastName { get; set; }

        [StringLength(250)]
        public string? Bio { get; set; }

    }
}
