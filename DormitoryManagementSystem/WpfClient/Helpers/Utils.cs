using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Library.Models;

namespace WpfClient.Helpers
{
    public static class Utils
    {
        public static PagedRequestModel GetPagedRequestModel(Type type, object viewModelInstance, params string[] omitProperties)
        {
            var properties = type.GetProperties();

            var pageRequestModel = new PagedRequestModel
            {
                PageSize = (int)(properties.SingleOrDefault(x => x.Name == nameof(PagedRequestModel.PageSize))?.GetValue(viewModelInstance) ?? 10),
                PageNumber = (int)(properties.SingleOrDefault(x => x.Name == nameof(PagedRequestModel.PageNumber))?.GetValue(viewModelInstance) ?? 1),
            };

            pageRequestModel.Sorts.AddRange(GetSorts(properties, viewModelInstance));
            pageRequestModel.Filters.AddRange(GetStringFilters(properties, viewModelInstance));
            pageRequestModel.Filters.AddRange(GetIntFilters(properties, viewModelInstance));
            pageRequestModel.Filters.AddRange(GetDateTimeFilters(properties, viewModelInstance));

            pageRequestModel.Filters.RemoveAll(x => omitProperties.Any(omitProp => x.StartsWith(omitProp)));

            return pageRequestModel;
        }

        private static IEnumerable<string> GetSorts(IEnumerable<PropertyInfo> properties, object viewModelInstance)
        {
            var sortPropertyName = properties
                .Where(x => x.Name.EndsWith("Sort"))
                .Where(x => x.PropertyType == typeof(bool))
                .Single(x => (bool)x.GetValue(viewModelInstance))
                .Name;

            var sortPropertyNameWithoutSortSuffix =
                sortPropertyName.Substring(0, sortPropertyName.LastIndexOf('S'));

            var ascending = (bool)(properties.SingleOrDefault(x => x.Name == "Ascending")?.GetValue(viewModelInstance) ?? true);
            return new string[] { $"{(ascending ? "" : "-")}{sortPropertyNameWithoutSortSuffix}" };
        }

        private static IEnumerable<string> GetStringFilters(IEnumerable<PropertyInfo> properties, object viewModelInstance)
        {
            var filterProperties = properties
                .Where(x => x.Name.EndsWith("Filter"))
                .Where(x => x.PropertyType == typeof(string))
                .Where(x => !string.IsNullOrWhiteSpace((string)x.GetValue(viewModelInstance)))
                .Select(x => new { Name = x.Name.Substring(0, x.Name.LastIndexOf('F')), Value = (string)x.GetValue(viewModelInstance) });

            var filterExpressions = filterProperties.Select(x => $"{x.Name}@={x.Value}");

            return filterExpressions;
        }

        private static IEnumerable<string> GetIntFilters(IEnumerable<PropertyInfo> properties, object viewModelInstance)
        {
            var filterProperties = properties
                .Where(x => x.Name.EndsWith("Filter"))
                .Where(x => x.PropertyType == typeof(int?))
                .Select(x => new { Name = x.Name.Substring(0, x.Name.LastIndexOf('F')), Value = (int?)x.GetValue(viewModelInstance) })
                .Where(x => x.Value.HasValue);

            var filterOperatorProperties = properties
                .Where(x => x.Name.EndsWith("FilterOperator"))
                .Where(x => x.PropertyType == typeof(char))
                .Select(x => new { Name = x.Name.Substring(0, x.Name.LastIndexOf('F')), Operator = (char)x.GetValue(viewModelInstance) });

            foreach (var filter in filterProperties)
            {
                var matchingFilterOperator = filterOperatorProperties.Single(x => x.Name == filter.Name);

                yield return $"{filter.Name}{matchingFilterOperator.Operator}{filter.Value}";
            }
        }

        private static IEnumerable<string> GetDateTimeFilters(IEnumerable<PropertyInfo> properties, object viewModelInstance)
        {
            var filterProperties = properties
                .Where(x => x.Name.EndsWith("Filter"))
                .Where(x => x.PropertyType == typeof(DateTime?))
                .Select(x => new { Name = x.Name.Substring(0, x.Name.LastIndexOf('F')), Value = (DateTime?)x.GetValue(viewModelInstance) })
                .Where(x => x.Value.HasValue);

            var filterOperatorProperties = properties
                .Where(x => x.Name.EndsWith("FilterOperator"))
                .Where(x => x.PropertyType == typeof(char))
                .Select(x => new { Name = x.Name.Substring(0, x.Name.LastIndexOf('F')), Operator = (char)x.GetValue(viewModelInstance) });

            foreach (var filter in filterProperties)
            {
                var matchingFilterOperator = filterOperatorProperties.Single(x => x.Name == filter.Name);

                yield return $"{filter.Name}{matchingFilterOperator.Operator}{filter.Value}";
            }
        }
    }
}
