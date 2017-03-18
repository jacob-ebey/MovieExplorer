// <copyright file="Setup.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

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
using MovieExplorer.Droid.Services;
using System.Net.Http;
using ModernHttpClient;

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

            // PCL Services
            Mvx.RegisterSingleton(new HttpClient(new NativeMessageHandler()));
            Mvx.RegisterSingleton<IEndpointList>(new EndpointList());
            Mvx.LazyConstructAndRegisterSingleton<IMovieService>(() => new MovieService(Mvx.Resolve<HttpClient>(), Mvx.Resolve<IEndpointList>(), Mvx.Resolve<ILogger>()));
            Mvx.LazyConstructAndRegisterSingleton<IFavoritesService>(() => new FavoritesService(Mvx.Resolve<IFileService>(), Mvx.Resolve<ILogger>()));

            // Native Services
            Mvx.RegisterSingleton<IToastService>(new ToastService());
            Mvx.RegisterSingleton<IUriService>(new UriService());
            Mvx.RegisterSingleton<IFileService>(new FileService());
            Mvx.RegisterSingleton<ILogger>(new Logger());
        }

        public override void Initialize()
        {
            base.Initialize();

            Mvx.Resolve<ILogger>().Init();
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            registry.AddOrOverwrite(nameof(FormatConverter), new FormatConverter());
            registry.AddOrOverwrite(nameof(StringDateConverter), new StringDateConverter());
        }

        protected override IMvxApplication CreateApp()
        {
            return new MovieApp();
        }
    }
}