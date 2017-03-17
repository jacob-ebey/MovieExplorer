using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieExplorer.Models;
using MovieExplorer.Services;
using MovieExplorer.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MovieExplorer.Portable.Tests.ViewModels
{
    // You get the idea... A proper archetecture (even though I may have missued 
    // parts of MVVMCross as it's my first time with it) allows for simple yet 
    // effective tests by allowing you to mock out the service interfaces.
    [TestClass]
    public class MainViewModelTests
    {
        #region Mock Data

        static readonly ServiceResult<ResultsModel<MovieListResult>> TopRatedResult = new ServiceResult<ResultsModel<MovieListResult>>
        {
            Succeeded = true,
            Data = new ResultsModel<MovieListResult>
            {
                Results = new MovieListResult[]
                {
                    new MovieListResult()
                }
            }
        };

        static readonly ServiceResult<ResultsModel<MovieListResult>> PopularResult = new ServiceResult<ResultsModel<MovieListResult>>
        {
            Succeeded = true,
            Data = new ResultsModel<MovieListResult>
            {
                Results = new MovieListResult[]
                {
                    new MovieListResult()
                }
            }
        };

        static readonly ServiceResult<ResultsModel<MovieListResult>> NowPlayingResult = new ServiceResult<ResultsModel<MovieListResult>>
        {
            Succeeded = true,
            Data = new ResultsModel<MovieListResult>
            {
                Results = new MovieListResult[]
                {
                    new MovieListResult()
                }
            }
        };

        static readonly IEnumerable<MovieListResult> FavoriteItems = new MovieListResult[]
        {
            new MovieListResult()
        };

        #endregion
        
        [TestMethod]
        public async Task OnNavigatedToAsync_ShouldLoadCategories()
        {
            // Setup
            Mock<IMovieService> movieMock = new Mock<IMovieService>();
            movieMock
                .Setup(m => m.GetTopRatedAsync())
                .ReturnsAsync(() => TopRatedResult)
                .Verifiable("Did not try to retrieve the top rated movies.");
            movieMock
                .Setup(m => m.GetPopularAsync())
                .ReturnsAsync(() => PopularResult)
                .Verifiable("Did not try to retrieve the popular movies.");
            movieMock
                .Setup(m => m.GetNowPlayingAsync())
                .ReturnsAsync(() => NowPlayingResult)
                .Verifiable("Did not try to retrieve the now playing movies.");

            Mock<IFavoritesService> watchlistMock = new Mock<IFavoritesService>();
            ObservableCollection<MovieListResult> watchlist = new ObservableCollection<MovieListResult>(FavoriteItems);
            watchlistMock.SetupGet(m => m.Favorites).Returns(watchlist);

            //Act
            MainViewModel vm = new MainViewModel(movieMock.Object, watchlistMock.Object, null);
            await vm.OnNavigatedToAsync();
            
            // Assert
            Assert.IsTrue(TopRatedResult.Data.Results.SequenceEqual(vm.TopRated), "The top rated results were not what was expected.");
            Assert.IsTrue(PopularResult.Data.Results.SequenceEqual(vm.Popular), "The popular results were not what was expected.");
            Assert.IsTrue(NowPlayingResult.Data.Results.SequenceEqual(vm.NowPlaying), "The now playing results were not what was expected.");
            Assert.IsTrue(FavoriteItems.SequenceEqual(vm.Favorites), "The favorites results were not what was expected.");
        }
    }
}
