using Core.Entities;
using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Active { get; set; }
        public List<string> Tags { get; set; }
    }
}
