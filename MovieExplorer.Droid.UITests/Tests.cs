using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using Xamarin.UITest.Android;
using Xamarin.UITest.Configuration;
using MovieExplorer.ViewModels;

namespace MovieExplorer.Droid.UITests
{
    [TestFixture]
    public class Tests
    {
        AndroidApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            app = ConfigureApp
                .Android
                .ApkFile ("../../../MovieExplorer.Droid/bin/Release/com.ebey.movieexplorer.apk")
                .StartApp(AppDataMode.Clear);
        }

        [Test]
        public void AppLaunchesAndLoadsItems()
        {
            app.Screenshot("First screen.");

            app.WaitForElement("poster_image", "No items were loaded.");
        }

        [Test]
        public void CanShowDetail()
        {
            app.Screenshot("First screen.");

            var posterImage = app
                .WaitForElement("poster_image", "No items were loaded.")
                .First();

            app.Tap("poster_image");

            var button = app
                .WaitForElement("favorite_button", "Couldn't find the favorites button.")
                .First();

            Assert.AreEqual(DetailViewModel.AddToFavoritesLabel, button.Text, "The favorite button label was no the expected value.");
        }

        [Test]
        public void CanAddToWatchlist()
        {
            CanShowDetail();

            app.Tap("favorite_button");

            var button = app
                .WaitForElement("favorite_button", "Couldn't find the favorites button.")
                .First();

            Assert.AreEqual(DetailViewModel.RemoveFromFavoritesLabel, button.Text, "The favorite button label was no the expected value.");

            app.Back();

            app.WaitForElement("favorites_list", "Couldn't find the favorites list.");
        }

        [Test]
        public void CanSearch()
        {
            app.WaitForElement("search_image", "Couldn't find the search image.");
            app.Tap("search_image");
            app.WaitForElement("search_entry", "Couldn't verify the search page opened.");
            app.EnterText("search_entry", "Fight Club");
            app.WaitForElement("poster_image", "Coulnd't find any results.");
            app.Tap("poster_image");
            app.WaitForElement("favorite_button", "Couldn't verify a detail page opened.");
        }
    }
}

