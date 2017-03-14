using MovieExplorer.Models;
using System.Threading.Tasks;

namespace MovieExplorer.Services
{
    /// <summary>
    /// The movie service abstraction.
    /// </summary>
    public interface IMovieService
    {
        /// <summary>
        /// Get's the latest movies that are now in theaters.
        /// </summary>
        /// <returns>A result indicating success and the data.</returns>
        Task<ServiceResult<ResultsModel<MovieListResult>>> GetNowPlayingAsync();

        /// <summary>
        /// Get's the top rated movies that are now in theaters.
        /// </summary>
        /// <returns>A result indicating success and the data.</returns>
        Task<ServiceResult<ResultsModel<MovieListResult>>> GetTopRatedAsync();

        /// <summary>
        /// Get's the popular movies that are now in theaters.
        /// </summary>
        /// <returns>A result indicating success and the data.</returns>
        Task<ServiceResult<ResultsModel<MovieListResult>>> GetPopularAsync();

        /// <summary>
        /// Get's similar movies to the given one.
        /// </summary>
        /// <returns>A result indicating success and the data.</returns>
        Task<ServiceResult<ResultsModel<MovieListResult>>> GetSimilarAsync(string movieId);

        /// <summary>
        /// Get's the videos for a movie.
        /// </summary>
        /// <returns>A result indicating success and the data.</returns>
        Task<ServiceResult<VideoResults>> GetVideosAsync(string movieId);
    }
}