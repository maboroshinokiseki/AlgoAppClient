using System.Collections.ObjectModel;

namespace AlgoApp.ViewModels
{
    public class BaseListViewModel<T> : BaseViewModel
    {
        private ObservableCollection<T> items;
        public ObservableCollection<T> Items
        {
            get => items;
            set => SetValue(out items, value);
        }
    }
}
