using Sieve.Services;

namespace Application.Common.Pagination
{
    public interface IFilteringMapperProfile
    {
        void MapProperties(SievePropertyMapper mapper);
    }
}
