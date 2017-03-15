using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform.IoC;
using MovieExplorer.Services;
using MvvmCross.Platform;
using System.Collections.Generic;
using System.Reflection;
using MovieExplorer.Converters;
using MvvmCross.Platform.Converters;

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

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            registry.AddOrOverwrite("FormatConverter", new FormatConverter());
            registry.AddOrOverwrite("StringDateConverter", new StringDateConverter());
        }

        protected override IMvxApplication CreateApp()
        {
            return new MovieApp();
        }
    }
}