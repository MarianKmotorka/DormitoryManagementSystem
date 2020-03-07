using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.Officers;
using WpfClient.Events;
using WpfClient.Helpers;

namespace WpfClient.ViewModels.Officers
{
    public class OfficerListViewModel : Screen
    {
        private bool _loading;
        private bool _ascending = true;
        private bool _firstNameSort = true;
        private bool _lastNameSort;
        private bool _officeNumberSort;
        private string _firstNameFilter;
        private string _lastNameFilter;
        private string _officeNumberFilter;
        private int _pages;
        private int _pageNumber = 1;
        private int _pageSize = 10;
        private OfficerLookup _selectedOfficer;
        private readonly IOfficersEndpoint _officersEndpoint;
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

        public bool OfficeNumberSort
        {
            get => _officeNumberSort;
            set { _officeNumberSort = value; NotifyOfPropertyChange(nameof(OfficeNumberSort)); }
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

        public string OfficeNumberFilter
        {
            get => _officeNumberFilter;
            set { _officeNumberFilter = value; NotifyOfPropertyChange(nameof(OfficeNumberFilter)); }
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

        public OfficerLookup SelectedOfficer
        {
            get => _selectedOfficer;
            set { _selectedOfficer = value; NotifyOfPropertyChange(nameof(SelectedOfficer)); }
        }

        public ObservableCollection<OfficerLookup> Officers { get; set; } = new ObservableCollection<OfficerLookup>();

        public OfficerListViewModel(IOfficersEndpoint officersEndpoint, IEventAggregator eventAggregator)
        {
            _officersEndpoint = officersEndpoint;
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
            Officers.Clear();

            var result = await _officersEndpoint.GetAll(Utils.GetPagedRequestModel(GetType(), this));

            Loading = false;

            PageSize = result.PageSize;
            PageNumber = result.PageNumber;
            Pages = result.Pages;

            foreach (var item in result.Data)
            {
                Officers.Add(item);
            }
        }

        public void OpenDetail()
        {
            _eventAggregator.PublishOnUIThread(new OpenOfficerDetailEvent(this, SelectedOfficer.Id));
        }

        protected async override void OnViewLoaded(object view)
        {
            await Load();
        }
    }
}
