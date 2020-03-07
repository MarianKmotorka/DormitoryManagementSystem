using Library.Models.Repairers;
using WpfClient.Validation;

namespace WpfClient.ModelWrappers
{
    public class RepairerModelWrapper : ValidationWrapper<RepairerModel>
    {
        public RepairerModelWrapper() : base(new RepairerModel())
        {
        }

        public string Email
        {
            get => GetValue<string>();
            set => SetValue(value);
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
    }
}
