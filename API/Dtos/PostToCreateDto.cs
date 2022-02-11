using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class PostToCreateDto
    {
        [Required]
        [RegularExpression(@".{1,}\.[a-zA-Z]{2,5}",
            ErrorMessage = "The filename need to have the name and extension of the file")]
        public string FileName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageBase64 { get; set; }

        public List<string> Tags { get; set; }
    }
}
