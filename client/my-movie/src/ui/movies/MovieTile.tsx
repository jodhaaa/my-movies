import type { MergedMovieDto } from "../../data/models/moviesModels";

const MovieTile = ({ movie }: { movie: MergedMovieDto }) => {
    const defaultPoster = import.meta.env.VITE_DEFAULT_POSTER_URL || '';

    return (
        <div className="relative rounded-lg overflow-hidden shadow-lg transition-all duration-300 transform hover:scale-105 flex flex-col h-full">
            {/* Image Section */}
            <div className="w-full h-62"> {/* Fixed height for the image */}
                <img
                    src={movie.poster || defaultPoster}
                    alt={`${movie.title} poster`}
                    className="w-full h-full object-cover"
                    onError={(e) => {
                        (e.target as HTMLImageElement).src = defaultPoster;
                    }}
                />
            </div>

            {/* Text Section */}
            <div className="bg-black p-4 text-white flex flex-col justify-end flex-grow">
                <div className="mt-auto">
                    <h3 className="text-lg font-bold mb-2 line-clamp-2 h-12 overflow-hidden">
                        {movie.title}
                    </h3>
                    <div className="flex items-center mb-2">
                        <span className="ml-1 text-sm">{movie.rating}</span>
                        <span className="mx-2 text-sm text-gray-400">|</span>
                        <span className="text-sm">{movie.year}</span>
                    </div>
                    <div className="flex flex-wrap gap-1 mb-4">
                        {movie.genre && movie.genre.split(',').map((genre, index) => (
                            <span key={index} className="text-xs bg-gray-700 px-2 py-1 rounded">
                                {genre}
                            </span>
                        ))}
                    </div>
                </div>

                {/* Prices Section */}
                <div className="flex flex-col gap-1">
                    {movie.prices && movie.prices.map((p, index) => (
                        <div key={index} className="text-sm">
                            <span className="font-semibold">{p.provider}:</span> {p.price ? `$${p.price?.toFixed(2)}` : 'N/A'}
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default MovieTile;