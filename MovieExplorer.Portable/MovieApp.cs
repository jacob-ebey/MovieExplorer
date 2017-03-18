// <copyright file="MovieApp.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

using MovieExplorer.ViewModels;
using MvvmCross.Core.ViewModels;

namespace MovieExplorer
{
    public class MovieApp : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();
            
            RegisterAppStart<MainViewModel>();
        }
    }
}
