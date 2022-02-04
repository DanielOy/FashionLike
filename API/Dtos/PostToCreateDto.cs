using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class PostToCreateDto
    {
        [Required]
        public string FileName { get; set; }
        [Required]
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public List<string> Tags { get; set; }
    }
}
