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
        private int _runningTaskCount = 0;

        public bool ShowLoader { get { return _runningTaskCount > 0; } }

        public bool InverseShowLoader { get { return !ShowLoader; } }

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
            RaisePropertyChanged(nameof(InverseShowLoader));
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
            finally
            {
                _runningTaskCount--;
                RaisePropertyChanged(nameof(ShowLoader));
                RaisePropertyChanged(nameof(InverseShowLoader));
            }
        }

        protected void LogException(Exception e)
        {
            // TODO: Log exceptions.
        }
    }
}
