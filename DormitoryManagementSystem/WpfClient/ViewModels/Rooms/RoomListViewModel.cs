using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.Rooms;
using WpfClient.Events;
using WpfClient.Helpers;

namespace WpfClient.ViewModels.Rooms
{
    public class RoomListViewModel : Screen
    {
        private bool _loading;
        private int _pages;
        private int _pageNumber;
        private readonly IRoomsEndpoint _roomsEndpoint;
        private readonly IEventAggregator _eventAggregator;

        #region FILTERS & SORTS

        public IEnumerable<int> PageSizeOptions => new[] { 2, 5, 10, 20, 50 };

        public IEnumerable<char> FilterOperators => new[] { '=', '>', '<' };

        public bool Ascending { get; set; } = true;

        public bool RoomNumberSort { get; set; } = true;

        public bool FreeBedsSort { get; set; }

        public string RoomNumberFilter { get; set; }

        public int? FreeBedsFilter { get; set; }

        public char FreeBedsFilterOperator { get; set; } = '=';

        public int PageSize { get; set; } = 10;

        public int Pages { get => _pages; set { _pages = value; NotifyOfPropertyChange(nameof(Pages)); } }

        public int PageNumber { get => _pageNumber; set { _pageNumber = value; NotifyOfPropertyChange(nameof(PageNumber)); } }

        #endregion

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public RoomLookup SelectedRoom { get; set; }

        public ObservableCollection<RoomLookup> Rooms { get; set; } = new ObservableCollection<RoomLookup>();

        public RoomListViewModel(IRoomsEndpoint roomsEndpoint, IEventAggregator eventAggregator)
        {
            _roomsEndpoint = roomsEndpoint;
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

            var result = await _roomsEndpoint.GetAll(Utils.GetPagedRequestModel(GetType(), this));

            Loading = false;

            PageSize = result.PageSize;
            PageNumber = result.PageNumber;
            Pages = result.Pages;

            Rooms.Clear();

            foreach (var item in result.Data)
                Rooms.Add(item);
        }

        public void OpenDetail()
        {
            _eventAggregator.PublishOnUIThread(new OpenRoomDetailEvent(this, SelectedRoom.Id));
        }

        protected async override void OnViewLoaded(object view)
        {
            await Load();
        }
    }
}
