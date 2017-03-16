using System;

namespace MovieExplorer.Services
{
    public interface IEndpointList
    {
        Uri NowPlayingUri { get; }

        Uri TopRatedUri { get; }

        Uri PopularUri { get; }

        Uri GetSimilarUri(string movieId);

        Uri GetVideosUri(string movieId);

        Uri GetSearchUri(string query);
    }
}