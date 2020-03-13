using System;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.Rooms;
using WpfClient.Events;

namespace WpfClient.ViewModels.Rooms
{
    public class RoomDetailViewModel : Screen
    {
        private bool _loading;
        private readonly IEventAggregator _eventAggregator;
        private readonly IGuestsEndpoint _guestsEndpoint;
        private readonly IRoomsEndpoint _roomsEndpoint;

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public object GoBackViewModel { get; set; }

        public bool IsMyRoomPage { get; set; }

        public RoomModel Model { get; set; } = new RoomModel();

        public RoomDetailViewModel(IEventAggregator eventAggregator, IRoomsEndpoint roomsEndpoint, IGuestsEndpoint guestsEndpoint)
        {
            _eventAggregator = eventAggregator;
            _guestsEndpoint = guestsEndpoint;
            _roomsEndpoint = roomsEndpoint;
        }

        public void GoBack()
        {
            _ = GoBackViewModel ?? throw new ArgumentNullException(nameof(GoBackViewModel));

            _eventAggregator.PublishOnUIThread(new GoBackEvent(GoBackViewModel));
        }

        public void OpenGuestDetail(string guestId)
        {
            _eventAggregator.PublishOnUIThread(new OpenGuestDetailEvent(this, guestId));
        }

        protected async override void OnViewLoaded(object view)
        {
            Loading = true;

            Model = IsMyRoomPage
                ? await _guestsEndpoint.GetMyRoomDetail()
                : await _roomsEndpoint.GetDetail(Model.Id);

            Loading = false;

            NotifyOfPropertyChange(nameof(Model));
        }
    }
}
