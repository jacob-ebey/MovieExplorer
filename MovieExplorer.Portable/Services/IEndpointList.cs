using System;

namespace MovieExplorer.Services
{
    interface IEndpointList
    {
        Uri NowPlayingUri { get; }

        Uri TopRatedUri { get; }

        Uri PopularUri { get; }

        Uri GetSimilarUri(string movieId);

        Uri GetVideosUri(string movieId);
    }
}