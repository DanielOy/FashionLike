using System;

namespace API.Dtos
{
    public class ConsultCommentDto
    {
        public string User { get; set; }
        public DateTime CreationDate { get; set; }
        public string Text { get; set; }
    }
}
