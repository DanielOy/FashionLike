using System;

namespace API.Dtos
{
    public class ConsultCommentDto
    {
        public string Id { get; set; }
        public string User { get; set; }
        public bool IsOwn { get; set; }
        public DateTime CreationDate { get; set; }
        public string Text { get; set; }
    }
}
