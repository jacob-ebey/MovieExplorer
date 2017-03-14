using System;

using Android.App;
using Android.Runtime;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform;
using MovieExplorer.Services;

namespace MovieExplorer.Droid
{
    class App : Application
    {
        protected App(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            MvxSimpleIoCContainer.Initialize();

            Mvx.RegisterType<IEndpointList, EndpointList>();
            Mvx.RegisterType<IMovieService, MovieService>();

            base.OnCreate();
        }
    }
}