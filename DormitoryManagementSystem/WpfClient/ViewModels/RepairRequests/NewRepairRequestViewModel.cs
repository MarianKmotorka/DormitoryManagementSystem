using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.Rooms;
using WpfClient.Events;
using WpfClient.ModelWrappers;

namespace WpfClient.ViewModels.RepairRequests
{
    public class NewRepairRequestViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IGuestsEndpoint _guestsEndpoint;
        private readonly IRepairRequestsEndpoint _repairRequestsEndpoint;
        private bool _loading;
        private bool _hasRoom;

        public bool HasRoom
        {
            get => _hasRoom;
            set { _hasRoom = value; NotifyOfPropertyChange(nameof(HasRoom)); }
        }


        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public object GoBackViewModel { get; set; }

        public NewRepairRequestModelWrapper Model { get; set; } = new NewRepairRequestModelWrapper();

        public ObservableCollection<RoomItemTypeLookup> Items { get; set; } = new ObservableCollection<RoomItemTypeLookup>();

        public NewRepairRequestViewModel(IEventAggregator eventAggregator, IGuestsEndpoint guestsEndpoint,
            IRepairRequestsEndpoint repairRequestsEndpoint)
        {
            _eventAggregator = eventAggregator;
            _guestsEndpoint = guestsEndpoint;
            _repairRequestsEndpoint = repairRequestsEndpoint;
        }

        public async Task Submit()
        {
            if (!Model.ValidateModel())
                return;

            Loading = true;

            var result = await _repairRequestsEndpoint.Create(Model.Model);

            Loading = false;

            if (result.Fail)
            {
                foreach (var error in result.ErrorDetails)
                {
                    var propName = error.PropertyName;

                    if (propName == "RoomItemTypeId")
                        propName = nameof(Model.RoomItemType);

                    Model.AddError(propName, error.Message, error.CustomState);
                }

                return;
            }

            GoBack();
        }

        public void GoBack()
        {
            _ = GoBackViewModel ?? throw new ArgumentNullException(nameof(GoBackViewModel));

            _eventAggregator.PublishOnUIThread(new GoBackEvent(GoBackViewModel));
        }

        protected async override void OnViewLoaded(object view)
        {
            Loading = true;

            var room = await _guestsEndpoint.GetMyRoomDetail();

            Loading = false;

            if (room == null)
                return;

            HasRoom = true;

            foreach (var item in room.Items)
                Items.Add(item);
        }
    }
}
