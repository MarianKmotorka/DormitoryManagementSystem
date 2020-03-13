using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.Offices;
using WpfClient.Events;
using WpfClient.Helpers;

namespace WpfClient.ViewModels.Offices
{
    public class OfficeListViewModel : Screen
    {
        private bool _loading;
        private int _pages;
        private readonly IOfficesEndpoint _officesEndpoint;
        private readonly IEventAggregator _eventAggregator;

        #region FILTERS & SORTS

        public IEnumerable<int> PageSizeOptions => new[] { 2, 5, 10, 20, 50 };

        public IEnumerable<char> FilterOperators => new[] { '=', '>', '<' };

        public bool Ascending { get; set; } = true;

        public bool OfficeNumberSort { get; set; } = true;

        public bool FreeTablesSort { get; set; }

        public string OfficeNumberFilter { get; set; }

        public int? FreeTablesFilter { get; set; }

        public char FreeTablesFilterOperator { get; set; } = '=';

        public int PageSize { get; set; } = 10;

        public int Pages { get => _pages; set { _pages = value; NotifyOfPropertyChange(nameof(Pages)); } }

        public int PageNumber { get; set; } = 1;

        #endregion

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public OfficeLookup SelectedOffice { get; set; }

        public ObservableCollection<OfficeLookup> Offices { get; set; } = new ObservableCollection<OfficeLookup>();

        public OfficeListViewModel(IOfficesEndpoint officesEndpoint, IEventAggregator eventAggregator)
        {
            _officesEndpoint = officesEndpoint;
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

            var result = await _officesEndpoint.GetAll(Utils.GetPagedRequestModel(GetType(), this));

            Loading = false;

            PageSize = result.PageSize;
            PageNumber = result.PageNumber;
            Pages = result.Pages;

            Offices.Clear();
            foreach (var item in result.Data)
            {
                Offices.Add(item);
            }
        }

        public void OpenDetail()
        {
            _eventAggregator.PublishOnUIThread(new OpenOfficeDetailEvent(this, SelectedOffice.Id));
        }

        protected async override void OnViewLoaded(object view)
        {
            await Load();
        }
    }
}
