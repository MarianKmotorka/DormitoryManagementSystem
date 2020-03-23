using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.Guests;
using MaterialDesignThemes.Wpf;
using WpfClient.Events;
using WpfClient.Helpers;

namespace WpfClient.ViewModels.Guests
{
    public class GuestListViewModel : Screen
    {
        private bool _loading;
        private bool _ascending = true;
        private bool _firstNameSort = true;
        private bool _lastNameSort;
        private bool _roomNumberSort;
        private string _firstNameFilter;
        private string _lastNameFilter;
        private string _roomNumberFilter;
        private int _pages;
        private int _pageNumber = 1;
        private int _pageSize = 10;
        private GuestLookup _selectedGuest;
        private readonly IGuestsEndpoint _guestsEndpoint;
        private readonly IEventAggregator _eventAggregator;

        public IEnumerable<int> PageSizeOptions => new[] { 2, 5, 10, 20, 50 };

        #region FILTERS & SORTS

        public bool Ascending
        {
            get => _ascending;
            set { _ascending = value; NotifyOfPropertyChange(nameof(Ascending)); }
        }

        public bool FirstNameSort
        {
            get => _firstNameSort;
            set { _firstNameSort = value; NotifyOfPropertyChange(nameof(FirstNameSort)); }
        }

        public bool LastNameSort
        {
            get => _lastNameSort;
            set { _lastNameSort = value; NotifyOfPropertyChange(nameof(LastNameSort)); }
        }

        public bool RoomNumberSort
        {
            get => _roomNumberSort;
            set { _roomNumberSort = value; NotifyOfPropertyChange(nameof(RoomNumberSort)); }
        }

        public string FirstNameFilter
        {
            get => _firstNameFilter;
            set { _firstNameFilter = value; NotifyOfPropertyChange(nameof(FirstNameFilter)); }
        }

        public string LastNameFilter
        {
            get => _lastNameFilter;
            set { _lastNameFilter = value; NotifyOfPropertyChange(nameof(LastNameFilter)); }
        }

        public string RoomNumberFilter
        {
            get => _roomNumberFilter;
            set { _roomNumberFilter = value; NotifyOfPropertyChange(nameof(RoomNumberFilter)); }
        }

        public int PageSize
        {
            get => _pageSize;
            set { _pageSize = value; NotifyOfPropertyChange(nameof(PageSize)); }
        }

        public int Pages
        {
            get => _pages;
            set { _pages = value; NotifyOfPropertyChange(nameof(Pages)); }
        }

        public int PageNumber
        {
            get => _pageNumber;
            set { _pageNumber = value; NotifyOfPropertyChange(nameof(PageNumber)); }
        }

        #endregion

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public GuestLookup SelectedGuest
        {
            get => _selectedGuest;
            set { _selectedGuest = value; NotifyOfPropertyChange(nameof(SelectedGuest)); }
        }

        public ObservableCollection<GuestLookup> Guests { get; set; } = new ObservableCollection<GuestLookup>();

        public GuestListViewModel(IGuestsEndpoint guestsEndpoint, IEventAggregator eventAggregator)
        {
            _guestsEndpoint = guestsEndpoint;
            _eventAggregator = eventAggregator;
        }

        public async Task PreviousPage()
        {
            if (PageNumber <= 1)
                return;

            PageNumber -= 1;
            await Load();
        }

        public async Task NextPage()
        {
            if (PageNumber >= Pages)
                return;

            PageNumber += 1;
            await Load();
        }

        public async Task Load()
        {
            Loading = true;

            var result = await _guestsEndpoint.GetAll(Utils.GetPagedRequestModel(GetType(), this));

            Loading = false;

            PageSize = result.PageSize;
            PageNumber = result.PageNumber;
            Pages = result.Pages;

            Guests.Clear();

            foreach (var item in result.Data)
                Guests.Add(item);
        }

        public void OpenDetail()
        {
            _eventAggregator.PublishOnUIThread(new OpenGuestDetailEvent(this, SelectedGuest.Id));
        }

        public async Task OnDeleteDialogClosing(DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false)
                return;

            await _guestsEndpoint.Delete(SelectedGuest.Id);
            await Load();
        }

        protected async override void OnViewLoaded(object view)
        {
            await Load();
        }
    }
}
