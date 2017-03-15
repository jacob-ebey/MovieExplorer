namespace MovieExplorer.Services
{
    public enum ToastDuration { Short, Long }

    public interface IToastService
    {
        void ShowToast(string message, ToastDuration duration);
    }
}
