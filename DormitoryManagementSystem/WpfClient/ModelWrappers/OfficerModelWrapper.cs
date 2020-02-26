using Library.Models.Officers;
using WpfClient.Validation;

namespace WpfClient.ModelWrappers
{
    public class OfficerModelWrapper : ValidationWrapper<OfficerModel>
    {
        public OfficerModelWrapper() : base(new OfficerModel())
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

        public string OfficeNumber
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
    }
}
