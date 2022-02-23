using System;

namespace Core.Entities
{
    public  class Comment
    {
        public Guid Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
