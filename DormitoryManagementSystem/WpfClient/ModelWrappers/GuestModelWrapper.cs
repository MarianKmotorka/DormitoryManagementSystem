using Library.Models.Guests;
using WpfClient.Validation;

namespace WpfClient.ModelWrappers
{
    public class GuestModelWrapper : ValidationWrapper<GuestModel>
    {
        public GuestModelWrapper() : base(new GuestModel())
        {
        }

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
    }
}
