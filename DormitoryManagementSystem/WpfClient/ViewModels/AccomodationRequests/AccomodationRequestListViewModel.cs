using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library.Api.Interfaces;
using Library.Models.AccomodationRequests;
using WpfClient.Events;
using WpfClient.Helpers;

namespace WpfClient.ViewModels.AccomodationRequests
{
    public class AccomodationRequestListViewModel : Screen
    {
        private bool _loading;
        private int _pages;
        private int _pageNumber = 1;
        private int _pageSize = 10;
        private bool _ascending = true;
        private bool _accomodationStartDateUtcSort;
        private bool _accomodationEndDateUtcSort;
        private bool _requesterDistanceFromHomeSort = true;

        private int? _requesterDistanceFromHomeFilter;
        private DateTime? _accomodationEndDateUtcFilter;
        private DateTime? _accomodationStartDateUtcFilter;
        private string _requestStateFilter;

        private char _requesterDistanceFromHomeFilterOperator = '>';
        private char _accomodationEndDateUtcFilterOperator = '<';
        private char _accomodationStartDateUtcFilterOperator = '<';

        private readonly IEventAggregator _eventAggregator;
        private readonly IAccomodationRequestsEndpoint _accomodationRequestsEndpoint;
        private readonly IGuestsEndpoint _guestsEndpoint;

        public bool IsMyAccomodationRequests { get; set; }

        public ObservableCollection<AccomodationRequestLookup> AccomodationRequests { get; set; }
            = new ObservableCollection<AccomodationRequestLookup>();

        public AccomodationRequestLookup SelectedAccomodationRequest { get; set; }

        public bool Loading
        {
            get => _loading;
            set { _loading = value; NotifyOfPropertyChange(nameof(Loading)); }
        }

        public IEnumerable<int> PageSizeOptions => new[] { 2, 5, 10, 20, 50 };

        #region FILTERS & SORTS

        public IEnumerable<char> FilterOperators => new[] { '<', '>' };

        public IEnumerable<string> RequestStateFilters
           => new[]
           {
               "",
               AccomodationRequestState.Active.ToString(),
               AccomodationRequestState.Approved.ToString(),
               AccomodationRequestState.Refused.ToString()
           };

        public int? RequesterDistanceFromHomeFilter
        {
            get => _requesterDistanceFromHomeFilter;
            set { _requesterDistanceFromHomeFilter = value; NotifyOfPropertyChange(nameof(RequesterDistanceFromHomeFilter)); }
        }

        public char RequesterDistanceFromHomeFilterOperator
        {
            get => _requesterDistanceFromHomeFilterOperator;
            set { _requesterDistanceFromHomeFilterOperator = value; NotifyOfPropertyChange(nameof(RequesterDistanceFromHomeFilterOperator)); }
        }

        public DateTime? AccomodationStartDateUtcFilter
        {
            get => _accomodationStartDateUtcFilter;
            set { _accomodationStartDateUtcFilter = value; NotifyOfPropertyChange(nameof(AccomodationStartDateUtcFilter)); }
        }

        public char AccomodationStartDateUtcFilterOperator
        {
            get => _accomodationStartDateUtcFilterOperator;
            set { _accomodationStartDateUtcFilterOperator = value; NotifyOfPropertyChange(nameof(AccomodationStartDateUtcFilterOperator)); }
        }

        public DateTime? AccomodationEndDateUtcFilter
        {
            get => _accomodationEndDateUtcFilter;
            set { _accomodationEndDateUtcFilter = value; NotifyOfPropertyChange(nameof(AccomodationEndDateUtcFilter)); }
        }

        public char AccomodationEndDateUtcFilterOperator
        {
            get => _accomodationEndDateUtcFilterOperator;
            set { _accomodationEndDateUtcFilterOperator = value; NotifyOfPropertyChange(nameof(AccomodationEndDateUtcFilterOperator)); }
        }

        public string RequestStateFilter
        {
            get => _requestStateFilter;
            set { _requestStateFilter = value; NotifyOfPropertyChange(nameof(RequestStateFilter)); }
        }

        public bool Ascending
        {
            get => _ascending;
            set { _ascending = value; NotifyOfPropertyChange(nameof(Ascending)); }
        }

        public bool AccomodationStartDateUtcSort
        {
            get => _accomodationStartDateUtcSort;
            set { _accomodationStartDateUtcSort = value; NotifyOfPropertyChange(nameof(AccomodationStartDateUtcSort)); }
        }

        public bool AccomodationEndDateUtcSort
        {
            get => _accomodationEndDateUtcSort;
            set { _accomodationEndDateUtcSort = value; NotifyOfPropertyChange(nameof(AccomodationEndDateUtcSort)); }
        }

        public bool RequesterDistanceFromHomeSort
        {
            get => _requesterDistanceFromHomeSort;
            set { _requesterDistanceFromHomeSort = value; NotifyOfPropertyChange(nameof(RequesterDistanceFromHomeSort)); }
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

        public AccomodationRequestListViewModel(IEventAggregator eventAggregator, IAccomodationRequestsEndpoint accomodationRequestsEndpoint,
            IGuestsEndpoint guestsEndpoint)
        {
            _eventAggregator = eventAggregator;
            _accomodationRequestsEndpoint = accomodationRequestsEndpoint;
            _guestsEndpoint = guestsEndpoint;
        }

        public void OpenDetail()
        {
            _eventAggregator.PublishOnUIThread(new OpenAccomodationRequestDetailEvent(this, SelectedAccomodationRequest.Id));
        }

        public void NewAccomodationRequest()
        {
            _eventAggregator.PublishOnUIThread(new OpenNewAccomodationRequestViewEvent(this));
        }

        public async Task Load()
        {
            var pagedRequestModel = Utils.GetPagedRequestModel(GetType(), this, omitProperties: nameof(AccomodationRequestLookup.RequestState));

            if (!string.IsNullOrWhiteSpace(RequestStateFilter))
            {
                var requestStateInt = Enum.TryParse<AccomodationRequestState>(RequestStateFilter, out var parsed)
                    ? (int)parsed
                    : throw new Exception("Invalid request state value");

                pagedRequestModel.Filters.Add($"{nameof(AccomodationRequestLookup.RequestState)}=={requestStateInt}");
            }

            Loading = true;

            var result = IsMyAccomodationRequests
                ? await _guestsEndpoint.GetMyAccomodationRequests(pagedRequestModel)
                : await _accomodationRequestsEndpoint.GetAll(pagedRequestModel);

            Loading = false;

            PageSize = result.PageSize;
            PageNumber = result.PageNumber;
            Pages = result.Pages;

            AccomodationRequests.Clear();
            foreach (var item in result.Data)
            {
                AccomodationRequests.Add(item);
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
