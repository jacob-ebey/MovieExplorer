using MovieExplorer.Exceptions;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MovieExplorer.ViewModels
{
    public class BaseViewModel : MvxViewModel, INotifyPropertyChanged
    {
        private readonly Dictionary<string, List<Action<object>>> _observingCache = new Dictionary<string, List<Action<object>>>();
        private int _runningTaskCount = 0;

        public bool ShowLoader { get { return _runningTaskCount > 0; } }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        protected async Task ShowLoaderAsync(Func<Task> asyncAction)
        {
            _runningTaskCount++;
            RaisePropertyChanged(nameof(ShowLoader));
            try
            {
                await asyncAction();
            }
            catch (MessageException e)
            {
                ErrorMessage = e.UserMessage;
                LogException(e);
            }
            catch (Exception e)
            {
                LogException(e);
            }

        }

        protected void LogException(Exception e)
        {
            // TODO: Log exceptions.
        }
    }
}
