using System;
using System.Linq;
using Library.Models;

namespace WpfClient.Helpers
{
    public static class Utils
    {
        public static PagedRequestModel GetPageRequestModel(Type type, object viewModelInstance)
        {
            var properties = type.GetProperties();

            var pageRequestModel = new PagedRequestModel
            {
                PageSize = (int)properties.Single(x => x.Name == nameof(PagedRequestModel.PageSize)).GetValue(viewModelInstance),
                PageNumber = (int)properties.Single(x => x.Name == nameof(PagedRequestModel.PageNumber)).GetValue(viewModelInstance),
            };

            var sortPropertyName = properties
                .Where(x => x.Name.EndsWith("Sort"))
                .Single(x => (bool)x.GetValue(viewModelInstance))
                .Name;

            var sortPropertyNameWithoutSortSuffix =
                sortPropertyName.Substring(0, sortPropertyName.LastIndexOf('S'));

            var ascending = (bool)properties.Single(x => x.Name == "Ascending").GetValue(viewModelInstance);
            pageRequestModel.Sorts = new[] { $"{(ascending ? "" : "-")}{sortPropertyNameWithoutSortSuffix}" };

            var filterProperties = properties
                .Where(x => x.Name.EndsWith("Filter"))
                .Where(x => !string.IsNullOrWhiteSpace((string)x.GetValue(viewModelInstance)))
                .Select(x => new { x.Name, Value = (string)x.GetValue(viewModelInstance) });

            var filterPropertiesWithoutFilterSuffix = filterProperties
                .Select(x => new { Name = x.Name.Substring(0, x.Name.LastIndexOf('F')), x.Value })
                .Select(x => $"{x.Name}@={x.Value}");

            pageRequestModel.Filters = filterPropertiesWithoutFilterSuffix;

            return pageRequestModel;
        }
    }
}
