using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform.IoC;
using MovieExplorer.Services;
using MvvmCross.Platform;

namespace MovieExplorer.Droid
{
    class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override void InitializeIoC()
        {
            base.InitializeIoC();

            MvxSimpleIoCContainer.Initialize();

            Mvx.RegisterType<IEndpointList, EndpointList>();
            Mvx.RegisterType<IMovieService, MovieService>();
        }

        protected override IMvxApplication CreateApp()
        {
            return new MovieApp();
        }
    }
}