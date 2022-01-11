using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Active { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
