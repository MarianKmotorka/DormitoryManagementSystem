using System.Linq;
using Library.Models;

namespace Library.Api.Utils
{
    public class UrlBuilder
    {
        public static string Build(string route, PagedRequestModel model)
        {
            var url = route + $"?PageSize={model.PageSize}&Page={model.PageNumber}";

            if (model.Filters.Any())
            {
                url += "&Filters=";

                foreach (var filter in model.Filters)
                {
                    url += $"{filter},";
                }

                url = url.Substring(0, url.Length - 1);
            }

            if (model.Sorts.Any())
            {
                url += "&Sorts=";

                foreach (var sort in model.Sorts)
                {
                    url += $"{sort},";
                }

                url = url.Substring(0, url.Length - 1);

            }

            return url;
        }
    }
}
