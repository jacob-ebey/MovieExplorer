using MovieExplorer.Services;
using MovieExplorer.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

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
