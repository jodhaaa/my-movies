namespace MyMovie.Application.Dto.Movie
{
    /// <summary>
    /// Merged movie details.
    /// </summary>
    public class MergedMovieDto : MinimalMovieDto
    {
        public IEnumerable<MergedPriceDto> Prices { get; set; }
    }

    /// <summary>
    /// Merged prices.
    /// </summary>
    public class MergedPriceDto
    {
        public decimal? Price { get; set; }
        public string Provider { get; set; }
    }
}