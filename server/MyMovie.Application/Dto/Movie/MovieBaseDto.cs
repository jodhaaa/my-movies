namespace MyMovie.Application.Dto.Movie
{
    /// <summary>
    /// Base DTO for movie
    /// </summary>
    public class MovieBaseDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Poster { get; set; }
        public string Type { get; set; }
    }
}
