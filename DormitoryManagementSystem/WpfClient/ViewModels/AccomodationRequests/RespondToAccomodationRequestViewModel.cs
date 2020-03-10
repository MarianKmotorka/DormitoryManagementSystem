using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models;
using Library.Models.Rooms;
using WpfClient.Events;

namespace WpfClient.ViewModels.AccomodationRequests
{
    public class RespondToAccomodationRequestViewModel : Screen, INotifyDataErrorInfo
    {
        private readonly IRoomsEndpoint _roomsEndpoint;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAccomodationRequestsEndpoint _accomodationRequestsEndpoint;
        private bool _loading;
        private string _searchText = "";
        private string _message;

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; NotifyOfPropertyChange(nameof(SearchText)); }
        }

        public string Message
        {
            get => _message;
            set { _message = value; NotifyOfPropertyChange(nameof(Message)); }
        }

        public ObservableCollection<RoomLookup> Rooms { get; set; } = new ObservableCollection<RoomLookup>();

        public object GoBackViewModel { get; set; }

        public RoomLookup SelectedRoom { get; set; }

        public int RequestId { get; set; }


        public RespondToAccomodationRequestViewModel(IRoomsEndpoint roomsEndpoint, IEventAggregator eventAggregator,
            IAccomodationRequestsEndpoint accomodationRequestsEndpoint)
        {
            _roomsEndpoint = roomsEndpoint;
            _eventAggregator = eventAggregator;
            _accomodationRequestsEndpoint = accomodationRequestsEndpoint;
        }

        public async Task LoadRooms()
        {
            if (SelectedRoom?.RoomNumber == SearchText)
                return;

            var pageRequestModel = new PagedRequestModel { PageSize = 7 };
            pageRequestModel.Sorts.Add("RoomNumber");
            pageRequestModel.Filters.Add("FreeBeds>0");
            pageRequestModel.Filters.Add($"RoomNumber@={SearchText}");

            Loading = true;
            var result = await _roomsEndpoint.GetAll(pageRequestModel);
            Loading = false;

            Rooms.Clear();
            foreach (var room in result.Data)
            {
                Rooms.Add(room);
            }
        }

        public void GoBack()
        {
            _ = GoBackViewModel ?? throw new ArgumentNullException(nameof(GoBackViewModel));

            _eventAggregator.PublishOnUIThread(new GoBackEvent(GoBackViewModel));
        }

        public async Task Approve()
        {
            SelectedRoomError = "";

            if (SelectedRoom == null)
            {
                SelectedRoomError = IoC.Get<ResourceDictionary>("language")["Required"].ToString();
                return;
            }

            Loading = true;

            var result = await _accomodationRequestsEndpoint.ApproveAccomodationRequest(RequestId, SelectedRoom.Id, Message);

            Loading = false;

            if (result.Fail)
            {
                var error = result.ErrorDetails.Select(x => x.Message).First();
                SelectedRoomError = IoC.Get<ResourceDictionary>("language")[error].ToString();
                return;
            }

            GoBack();
        }

        public async Task Reject()
        {
            SelectedRoomError = "";

            Loading = true;

            var result = await _accomodationRequestsEndpoint.RejectAccomodationRequest(RequestId, Message);

            Loading = false;

            if (result.Fail)
            {
                var error = result.ErrorDetails.Select(x => x.Message).First();
                SelectedRoomError = IoC.Get<ResourceDictionary>("language")[error].ToString();
                return;
            }

            GoBack();
        }

        protected async override void OnViewLoaded(object view)
        {
            await LoadRooms();
        }

        #region INotifyDataErrorInfo

        private string _selectedRoomError;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public string SelectedRoomError
        {
            get => _selectedRoomError;
            set { _selectedRoomError = value; ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Rooms))); }
        }

        public bool HasErrors => !string.IsNullOrEmpty(SelectedRoomError);

        public IEnumerable GetErrors(string propertyName) => propertyName == nameof(Rooms) ? new[] { SelectedRoomError } : null;

        #endregion
    }
}
