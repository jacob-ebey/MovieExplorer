// <copyright file="IToastService.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

namespace MovieExplorer.Services
{
    public enum ToastDuration { Short, Long }

    public interface IToastService
    {
        void ShowToast(string message, ToastDuration duration);
    }
}
