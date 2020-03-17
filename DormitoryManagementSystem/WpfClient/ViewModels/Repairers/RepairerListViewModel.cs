using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.Repairers;
using MaterialDesignThemes.Wpf;
using WpfClient.Events;
using WpfClient.Helpers;

namespace WpfClient.ViewModels.Repairers
{
    public class RepairerListViewModel : Screen
    {
        private bool _loading;
        private bool _ascending = true;
        private bool _firstNameSort = true;
        private bool _lastNameSort;
        private string _firstNameFilter;
        private string _lastNameFilter;
        private int _pages;
        private int _pageNumber = 1;
        private int _pageSize = 10;
        private RepairerLookup _selectedRepairer;
        private readonly IRepairersEndpoint _repairersEndpoint;
        private readonly IEventAggregator _eventAggregator;

        public IEnumerable<int> PageSizeOptions => new[] { 2, 5, 10, 20, 50 };

        #region FILTERS & SORTS

        public bool Ascending
        {
            get => _ascending;
            set { _ascending = value; NotifyOfPropertyChange(nameof(Ascending)); }
        }

        public bool FirstNameSort
        {
            get => _firstNameSort;
            set { _firstNameSort = value; NotifyOfPropertyChange(nameof(FirstNameSort)); }
        }

        public bool LastNameSort
        {
            get => _lastNameSort;
            set { _lastNameSort = value; NotifyOfPropertyChange(nameof(LastNameSort)); }
        }

        public string FirstNameFilter
        {
            get => _firstNameFilter;
            set { _firstNameFilter = value; NotifyOfPropertyChange(nameof(FirstNameFilter)); }
        }

        public string LastNameFilter
        {
            get => _lastNameFilter;
            set { _lastNameFilter = value; NotifyOfPropertyChange(nameof(LastNameFilter)); }
        }

        public int PageSize
        {
            get => _pageSize;
            set { _pageSize = value; NotifyOfPropertyChange(nameof(PageSize)); }
        }

        public int Pages
        {
            get => _pages;
            set { _pages = value; NotifyOfPropertyChange(nameof(Pages)); }
        }

        public int PageNumber
        {
            get => _pageNumber;
            set { _pageNumber = value; NotifyOfPropertyChange(nameof(PageNumber)); }
        }

        #endregion

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public RepairerLookup SelectedRepairer
        {
            get => _selectedRepairer;
            set { _selectedRepairer = value; NotifyOfPropertyChange(nameof(SelectedRepairer)); }
        }

        public ObservableCollection<RepairerLookup> Repairers { get; set; } = new ObservableCollection<RepairerLookup>();

        public RepairerListViewModel(IRepairersEndpoint repairersEndpoint, IEventAggregator eventAggregator)
        {
            _repairersEndpoint = repairersEndpoint;
            _eventAggregator = eventAggregator;
        }

        public async Task PreviousPage()
        {
            if (PageNumber <= 1)
                return;

            PageNumber -= 1;
            await Load();
        }

        public async Task NextPage()
        {
            if (PageNumber >= Pages)
                return;

            PageNumber += 1;
            await Load();
        }

        public async Task Load()
        {
            Loading = true;

            var result = await _repairersEndpoint.GetAll(Utils.GetPagedRequestModel(GetType(), this));

            Loading = false;

            PageSize = result.PageSize;
            PageNumber = result.PageNumber;
            Pages = result.Pages;

            Repairers.Clear();
            foreach (var item in result.Data)
            {
                Repairers.Add(item);
            }
        }

        public void OpenDetail()
        {
            _eventAggregator.PublishOnUIThread(new OpenRepairerDetailEvent(this, SelectedRepairer.Id));
        }

        public async Task OnDeleteDialogClosing(DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false)
                return;

            await _repairersEndpoint.Delete(SelectedRepairer.Id);
            await Load();
        }

        protected async override void OnViewLoaded(object view)
        {
            await Load();
        }
    }
}
