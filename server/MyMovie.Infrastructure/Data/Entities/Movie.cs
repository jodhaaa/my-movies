using MyMovie.Application.Const;
using MyMovie.Application.Dto;
using MyMovie.Application.Dto.Movie;

namespace MyMovie.Infrastructure.Data.Entities
{
    /// <summary>
    /// Movie Entity
    /// </summary>
    public class Movie : BaseEntity
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Poster { get; set; }
        public string Type { get; set; }
        public string Rated { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public string Metascore { get; set; }
        public string Rating { get; set; }
        public string Votes { get; set; }
        public decimal? CinemaWorldPrice { get; set; }
        public decimal? FilmWorldPrice { get; set; }

        /// <summary>
        /// Convert to merged movie DTO.
        /// </summary>
        /// <returns></returns>
        public MergedMovieDto ToDto()
        {
            return new MergedMovieDto()
            {
                Rated = Rated,
                Released = Released,
                Runtime = Runtime,
                Genre = Genre,
                Director = Director,
                Writer = Writer,
                Actors = Actors,
                Plot = Plot,
                Language = Language,
                Country = Country,
                Awards = Awards,
                Metascore = Metascore,
                Rating = Rating,
                Votes = Votes,
                Prices = new List<MergedPriceDto>()
                {
                    new MergedPriceDto() { Price = CinemaWorldPrice, Provider = Providers.CinemaWorld},
                    new MergedPriceDto() { Price = FilmWorldPrice, Provider = Providers.FilmWorld}
                },
                Id = Id,
                Title = Title,
                Year = Year,
                Poster = Poster,
                Type = Type
            };
        }
    }
}
