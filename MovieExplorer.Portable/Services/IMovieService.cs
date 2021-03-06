// <copyright file="IMovieService.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

using MovieExplorer.Models;
using System.IO;
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

        /// <summary>
        /// Do a search.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A result indicating success and the data.</returns>
        Task<ServiceResult<ResultsModel<MovieListResult>>> SearchAsync(string query);

        /// <summary>
        /// Get's a stream for the movies poster.
        /// </summary>
        /// <param name="posterPath">The poster path.</param>
        /// <returns>A stream for the image.</returns>
        Task<Stream> GetMoviePosterAsync(string posterPath, string imageSize);
    }
}