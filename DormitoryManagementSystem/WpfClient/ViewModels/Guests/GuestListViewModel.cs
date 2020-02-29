using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models;
using Library.Models.Guests;
using WpfClient.Events;

namespace WpfClient.ViewModels.Guests
{
    public class GuestListViewModel : Screen
    {
        private bool _loading;
        private int _pages;
        private int _pageNumber = 1;
        private int _selectedPageSize = 10;
        private GuestLookup _selectedGuest;
        private readonly IGuestsEndpoint _guestsEndpoint;
        private readonly IEventAggregator _eventAggregator;

        public IEnumerable<int> PageSizeOptions => new[] { 2, 5, 10, 20, 50 };

        public int PageSize
        {
            get => _selectedPageSize;
            set { _selectedPageSize = value; NotifyOfPropertyChange(nameof(PageSize)); }
        }

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
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
            Guests.Clear();

            var pagedRequestModel = new PagedRequestModel
            {
                PageSize = PageSize,
                PageNumber = PageNumber
            };

            var result = await _guestsEndpoint.GetAll(pagedRequestModel);

            Loading = false;

            PageSize = result.PageSize;
            PageNumber = result.PageNumber;
            Pages = result.Pages;

            foreach (var item in result.Data)
            {
                Guests.Add(item);
            }
        }

        public void OpenDetail()
        {
            _eventAggregator.PublishOnUIThread(new OpenGuestDetailEvent(this, SelectedGuest.Id));
        }

        protected async override void OnViewLoaded(object view)
        {
            await Load();
        }
    }
}
