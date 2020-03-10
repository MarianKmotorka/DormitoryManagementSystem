using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.Guests;
using WpfClient.Events;
using WpfClient.Validation;

namespace WpfClient.ViewModels.Guests
{
    public class GuestRegistrationViewModel : ValidationWrapper<GuestModel>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IGuestsEndpoint _guestsEndpoint;
        private bool _loading;

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        #region Model properties

        public string FirstName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }


        public string LastName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Email
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Password
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string PhoneNumber
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Country
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string City
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Street
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string HouseNumber
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string PostCode
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string IdCardNumber
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int DistanceFromHome
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public string DormitoryCardNumber
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string RoomNumber
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        #endregion

        public GuestRegistrationViewModel(IEventAggregator eventAggregator, IGuestsEndpoint guestsEndpoint)
            : base(new GuestModel())
        {
            _eventAggregator = eventAggregator;
            _guestsEndpoint = guestsEndpoint;
        }

        public async Task Register()
        {
            if (!ValidateModel())
                return;

            Loading = true;

            var result = await _guestsEndpoint.Register(Model);

            Loading = false;

            if (result.Fail)
            {
                AddErrors(result.ErrorDetails);
                return;
            }

            _eventAggregator.PublishOnUIThread(new GuestRegisteredEvent());
        }
    }
}
