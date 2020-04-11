namespace AlgoApp.ViewModels
{
    public class CommonListViewViewModel<T> : BaseListViewModel<T>
    {
        private bool isBusy;

        public bool IsBusy
        {
            get => isBusy;
            set => SetValue(out isBusy, value);
        }
    }
}
