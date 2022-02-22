using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Description { get; set; }
        public string PictureName { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Active { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Reaction> Reactions { get; set; }
    }
}
