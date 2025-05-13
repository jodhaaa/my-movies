import { useState } from "react";
import { useGetMoviesQuery } from "../../data/slices/api/endpoints/moviesEndpoint";
import MovieTile from "./MovieTile";
import { TailSpin } from "react-loader-spinner";

const MovieList = () => {
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 4;

  // Fetch movies with pagination
  // The offset is calculated based on the current page and page size
  const { data, isLoading } = useGetMoviesQuery(
    {
      offset: (currentPage - 1) * pageSize,
      pageSize,
      sortBy: "title",
    },
    {
      refetchOnFocus: true,
      refetchOnReconnect: true,
      refetchOnMountOrArgChange: true,
    }
  );

  const totalPages = data?.totalCount ? Math.ceil(data.totalCount / pageSize) : 1;

  const handleNextPage = () => {
    if (currentPage < totalPages) {
      setCurrentPage(currentPage + 1);
    }
  };

  const handlePreviousPage = () => {
    if (currentPage > 1) {
      setCurrentPage(currentPage - 1);
    }
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <TailSpin height="80" width="80" color="#4fa94d" ariaLabel="loading" />
      </div>
    );
  } else {
    return (
      <div className="flex flex-col items-center">
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          {data?.items && data.items.map((movie) => (
            <MovieTile key={movie.id} movie={movie} />
          ))}
        </div>

        <div className="flex justify-center items-center gap-4 mt-6">
          <button
            onClick={handlePreviousPage}
            disabled={currentPage === 1}
            className={`px-4 py-2 rounded ${currentPage === 1
                ? "bg-gray-300 text-gray-500 cursor-not-allowed"
                : "bg-gray-300 text-gray-700 hover:bg-gray-400"
              }`}
          >
            Previous
          </button>
          <span className="text-sm text-gray-700">
            Page {currentPage} of {totalPages}
          </span>
          <button
            onClick={handleNextPage}
            disabled={currentPage === totalPages}
            className={`px-4 py-2 rounded ${currentPage === totalPages
                ? "bg-gray-300 text-gray-500 cursor-not-allowed"
                : "bg-gray-300 text-gray-700 hover:bg-gray-400"
              }`}
          >
            Next
          </button>
        </div>
      </div>
    );
  }
};

export default MovieList;