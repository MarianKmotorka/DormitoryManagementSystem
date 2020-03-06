using System;
using Library.Models.AccomodationRequests;
using WpfClient.Validation;

namespace WpfClient.ModelWrappers
{
    public class NewAccomodationRequestModelWrapper : ValidationWrapper<NewAccomodationRequestModel>
    {
        public NewAccomodationRequestModelWrapper() : base(new NewAccomodationRequestModel())
        {
        }

        public DateTime AccomodationStartDateUtc
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime AccomodationEndDateUtc
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public string RequesterMessage
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}
