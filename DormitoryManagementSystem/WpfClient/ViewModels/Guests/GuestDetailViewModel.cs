using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using WpfClient.Models;

namespace WpfClient.ViewModels.Guests
{
    public class GuestDetailViewModel : Screen
    {
        private bool _isEditing;
        private bool _loading;
        private bool _editedSuccessfully;
        private readonly IGuestsEndpoint _guestsEndpoint;

        public GuestModelWrapper Model { get; set; } = new GuestModelWrapper();

        public string GuestId { get; set; } = null;

        public bool IsEditButtonVisible => !IsEditing && GuestId != null;

        public bool IsEditing
        {
            get => _isEditing;
            set { _isEditing = value; NotifyOfPropertyChange(nameof(IsEditing)); }
        }

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public bool EditedSuccessfully
        {
            get => _editedSuccessfully;
            set { _editedSuccessfully = value; NotifyOfPropertyChange(nameof(EditedSuccessfully)); }
        }

        public GuestDetailViewModel(IGuestsEndpoint guestsEndpoint)
        {
            _guestsEndpoint = guestsEndpoint;
        }

        public void Edit()
        {
            IsEditing = true;
        }

        public async Task SubmitEdit()
        {
            EditedSuccessfully = false;

            if (!Model.ValidateModel(nameof(Model.Password)))
                return;

            Loading = true;

            var result = await _guestsEndpoint.EditGuest(GuestId, Model.Model);

            Loading = false;

            if (result.Fail)
            {
                foreach (var propErrors in result.Errors)
                    foreach (var error in propErrors.Value)
                        Model.AddError(propErrors.Key, error);

                return;
            }

            EditedSuccessfully = true;
        }

        protected async override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            Loading = true;

            Model.Model = await _guestsEndpoint.GetGuestDetail(GuestId);
            NotifyOfPropertyChange(nameof(Model));

            Loading = false;
        }
    }
}
