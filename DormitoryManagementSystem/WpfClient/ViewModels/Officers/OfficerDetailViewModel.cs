using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using WpfClient.ModelWrappers;

namespace WpfClient.ViewModels.Officers
{
    public class OfficerDetailViewModel : Screen
    {
        private bool _isEditing = true;
        private bool _loading;
        private bool _editedSuccessfully;
        private readonly IOfficersEndpoint _officersEndpoint;

        public OfficerModelWrapper Model { get; set; } = new OfficerModelWrapper();

        public string OfficerId { get; set; } = null;

        public bool IsEditButtonVisible => !IsEditing && OfficerId != null;

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

        public OfficerDetailViewModel(IOfficersEndpoint officersEndpoint)
        {
            _officersEndpoint = officersEndpoint;
        }

        public void Edit()
        {
            IsEditing = true;
        }

        public async Task SubmitEdit()
        {
            EditedSuccessfully = false;

            if (!Model.ValidateModel())
                return;

            Loading = true;

            var result = await _officersEndpoint.Edit(OfficerId, Model.Model);

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

            Model.Model = await _officersEndpoint.GetDetail(OfficerId);
            NotifyOfPropertyChange(nameof(Model));

            Loading = false;
        }
    }
}
