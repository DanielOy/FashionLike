namespace API.Dtos
{
    public class ReactionPostDto
    {
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public int ReactionCount { get; set; }
        public int PostId { get; set; }
        public string PostDescription { get; set; }
    }
}
