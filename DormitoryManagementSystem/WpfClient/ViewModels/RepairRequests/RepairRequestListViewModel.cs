using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.RepairRequests;
using WpfClient.Events;
using WpfClient.Helpers;

namespace WpfClient.ViewModels.RepairRequests
{
    public class RepairRequestListViewModel : Screen
    {
        private bool _loading;
        private int _pages;
        private int _pageNumber = 1;
        private int _pageSize = 10;
        private bool _ascending = true;
        private bool _createdOnSort = true;
        private bool _willBeFixedOnSort;
        private bool _fixedOnSort;

        private DateTime? _createdOnFilter;
        private DateTime? _willBeFixedOnFilter;
        private DateTime? _fixedOnFilter;
        private string _stateFilter;

        private char _createdOnFilterOperator = '=';
        private char _willBeFixedOnFilterOperator = '=';
        private char _fixedOnFilterOperator = '=';

        private readonly IEventAggregator _eventAggregator;
        private readonly IRepairRequestsEndpoint _repairRequestsEndpoint;
        private readonly IGuestsEndpoint _guestsEndpoint;

        public bool IsMyRepairRequests { get; set; }

        public ObservableCollection<RepairRequestLookup> RepairRequests { get; set; }
            = new ObservableCollection<RepairRequestLookup>();

        public RepairRequestLookup SelectedRepairRequest { get; set; }

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public IEnumerable<int> PageSizeOptions => new[] { 2, 5, 10, 20, 50 };

        #region FILTERS & SORTS

        public IEnumerable<char> FilterOperators => new[] { '=', '<', '>' };

        public IEnumerable<string> RequestStateFilters
           => new[]
           {
               "",
               RepairRequestState.Accepted.ToString(),
               RepairRequestState.Fixed.ToString(),
               RepairRequestState.Pending.ToString(),
               RepairRequestState.Refused.ToString()
           };

        public DateTime? CreatedOnFilter
        {
            get => _createdOnFilter;
            set { _createdOnFilter = value; NotifyOfPropertyChange(nameof(CreatedOnFilter)); }
        }

        public char CreatedOnFilterOperator
        {
            get => _createdOnFilterOperator;
            set { _createdOnFilterOperator = value; NotifyOfPropertyChange(nameof(CreatedOnFilterOperator)); }
        }

        public DateTime? WillBeFixedOnFilter
        {
            get => _fixedOnFilter;
            set { _fixedOnFilter = value; NotifyOfPropertyChange(nameof(WillBeFixedOnFilter)); }
        }

        public char WillBeFixedOnFilterOperator
        {
            get => _fixedOnFilterOperator;
            set { _fixedOnFilterOperator = value; NotifyOfPropertyChange(nameof(WillBeFixedOnFilterOperator)); }
        }

        public DateTime? FixedOnFilter
        {
            get => _willBeFixedOnFilter;
            set { _willBeFixedOnFilter = value; NotifyOfPropertyChange(nameof(FixedOnFilter)); }
        }

        public char FixedOnFilterOperator
        {
            get => _willBeFixedOnFilterOperator;
            set { _willBeFixedOnFilterOperator = value; NotifyOfPropertyChange(nameof(FixedOnFilterOperator)); }
        }

        public string StateFilter
        {
            get => _stateFilter;
            set { _stateFilter = value; NotifyOfPropertyChange(nameof(StateFilter)); }
        }

        public bool Ascending
        {
            get => _ascending;
            set { _ascending = value; NotifyOfPropertyChange(nameof(Ascending)); }
        }

        public bool CreatedOnSort
        {
            get => _createdOnSort;
            set { _createdOnSort = value; NotifyOfPropertyChange(nameof(CreatedOnSort)); }
        }

        public bool WillBeFixedOnSort
        {
            get => _willBeFixedOnSort;
            set { _willBeFixedOnSort = value; NotifyOfPropertyChange(nameof(WillBeFixedOnSort)); }
        }

        public bool FixedOnSort
        {
            get => _fixedOnSort;
            set { _fixedOnSort = value; NotifyOfPropertyChange(nameof(FixedOnSort)); }
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

        public RepairRequestListViewModel(IEventAggregator eventAggregator, IRepairRequestsEndpoint repairRequestsEndpoint,
            IGuestsEndpoint guestsEndpoint)
        {
            _eventAggregator = eventAggregator;
            _repairRequestsEndpoint = repairRequestsEndpoint;
            _guestsEndpoint = guestsEndpoint;
        }

        public void OpenDetail()
        {
            _eventAggregator.PublishOnUIThread(new OpenRepairRequestDetailEvent(this, SelectedRepairRequest.Id, IsMyRepairRequests));
        }

        public void NewRepairRequest()
        {
            _eventAggregator.PublishOnUIThread(new OpenNewRepairRequestViewEvent(this));
        }

        public async Task Load()
        {
            var pagedRequestModel = Utils.GetPagedRequestModel(GetType(), this, omitProperties: nameof(RepairRequestLookup.State));

            if (!string.IsNullOrWhiteSpace(StateFilter))
            {
                var requestStateInt = Enum.TryParse<RepairRequestState>(StateFilter, out var parsed)
                    ? (int)parsed
                    : throw new Exception("Invalid request state value");

                pagedRequestModel.Filters.Add($"{nameof(RepairRequestLookup.State)}=={requestStateInt}");
            }

            Loading = true;

            var result = IsMyRepairRequests
                ? await _guestsEndpoint.GetMyRepairRequests(pagedRequestModel)
                : await _repairRequestsEndpoint.GetAll(pagedRequestModel);

            Loading = false;

            PageSize = result.PageSize;
            PageNumber = result.PageNumber;
            Pages = result.Pages;

            RepairRequests.Clear();
            foreach (var item in result.Data)
            {
                RepairRequests.Add(item);
            }
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

        protected async override void OnViewLoaded(object view)
        {
            await Load();
        }
    }
}
