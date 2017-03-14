using MovieExplorer.ViewModels;
using MvvmCross.Core.ViewModels;

namespace MovieExplorer
{
    class MovieApp : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            RegisterAppStart<MainViewModel>();
        }
    }
}
