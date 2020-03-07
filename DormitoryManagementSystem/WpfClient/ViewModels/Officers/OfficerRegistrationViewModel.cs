using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using WpfClient.ModelWrappers;

namespace WpfClient.ViewModels.Officers
{
    public class OfficerRegistrationViewModel : Screen
    {
        private readonly IOfficersEndpoint _officersEndpoint;
        private bool _loading;
        private bool _success;

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public bool Success
        {
            get => _success;
            set { _success = value; NotifyOfPropertyChange(nameof(Success)); }
        }

        public OfficerModelWrapper Model { get; set; } = new OfficerModelWrapper();

        public OfficerRegistrationViewModel(IOfficersEndpoint officersEndpoint)
        {
            _officersEndpoint = officersEndpoint;
        }

        public async Task Register()
        {
            Success = false;

            if (!Model.ValidateModel())
                return;

            Loading = true;

            var result = await _officersEndpoint.Register(Model.Model);

            Loading = false;

            if (result.Fail)
            {
                foreach (var propError in result.Errors)
                    foreach (var error in propError.Value)
                        Model.AddError(propError.Key, error);

                return;
            }

            Success = true;
        }
    }
}
