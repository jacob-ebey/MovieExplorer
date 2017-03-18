// <copyright file="BaseViewModel.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

using MovieExplorer.Models;
using MovieExplorer.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieExplorer.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel, INotifyPropertyChanged
    {
        private int _runningTaskCount = 0;

        protected BaseViewModel()
        {
            MovieSelectedCommand = new MvxCommand<MovieListResult>(r =>
            {
                if (r != null)
                {
                    ShowViewModel<DetailViewModel>(r);
                }
            });
        }

        /// <summary>
        /// A command that expects a <see cref="MovieListResult"/> passed as the parameter.
        /// </summary>
        public ICommand MovieSelectedCommand { get; }

        public bool ShowLoader { get { return _runningTaskCount > 0; } }

        public bool InverseShowLoader { get { return !ShowLoader; } }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        public virtual void OnResume() { }

        protected async Task ShowLoaderAsync(Func<Task> asyncAction, [CallerMemberName] string callerMemberName = null)
        {
            _runningTaskCount++;
            RaisePropertyChanged(nameof(ShowLoader));
            RaisePropertyChanged(nameof(InverseShowLoader));
            try
            {
                await asyncAction();
            }
            catch (Exception e)
            {
                LogException(e, callerMemberName);
            }
            finally
            {
                _runningTaskCount--;
                RaisePropertyChanged(nameof(ShowLoader));
                RaisePropertyChanged(nameof(InverseShowLoader));
            }
        }

        protected void LogException(Exception e, [CallerMemberName] string callerMemberName = null)
        {
            Mvx.Resolve<ILogger>().LogException(e, callerMemberName);
        }
    }
}
