using System.Collections.Generic;

namespace API.Dtos
{
    public class PostToUpdateDto
    {
        public string FileName { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public List<string> Tags { get; set; }
    }
}
