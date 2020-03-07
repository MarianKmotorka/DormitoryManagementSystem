using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using WpfClient.Events;
using WpfClient.ModelWrappers;

namespace WpfClient.ViewModels.Repairers
{
    public class RepairerDetailViewModel : Screen
    {
        private bool _isEditing;
        private bool _loading;
        private bool _editedSuccessfully;
        private readonly IRepairersEndpoint _repairersEndpoint;
        private readonly IEventAggregator _eventAggregator;

        public RepairerModelWrapper Model { get; set; } = new RepairerModelWrapper();

        public object GoBackViewModel { get; set; }

        public string RepairerId { get; set; } = null;

        public bool IsMyInfoPage => RepairerId == null;

        public bool IsEditButtonVisible => !IsEditing && RepairerId != null;

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

        public RepairerDetailViewModel(IRepairersEndpoint repairersEndpoint, IEventAggregator eventAggregator)
        {
            _repairersEndpoint = repairersEndpoint;
            _eventAggregator = eventAggregator;
        }

        public void Edit()
        {
            IsEditing = true;
            EditedSuccessfully = false;
        }

        public void GoBack()
        {
            _ = GoBackViewModel ?? throw new ArgumentNullException(nameof(GoBackViewModel));

            _eventAggregator.PublishOnUIThread(new GoBackEvent(GoBackViewModel));
        }

        public async Task SubmitEdit()
        {
            EditedSuccessfully = false;

            if (!Model.ValidateModel())
                return;

            Loading = true;

            var result = await _repairersEndpoint.Edit(RepairerId, Model.Model);

            Loading = false;

            if (result.Fail)
            {
                foreach (var propErrors in result.Errors)
                    foreach (var error in propErrors.Value)
                        Model.AddError(propErrors.Key, error);

                return;
            }

            EditedSuccessfully = true;
            IsEditing = false;
        }

        protected async override void OnViewLoaded(object view)
        {
            Loading = true;

            Model.Model = await _repairersEndpoint.GetDetail(RepairerId);
            NotifyOfPropertyChange(nameof(Model));

            Loading = false;
        }
    }
}
