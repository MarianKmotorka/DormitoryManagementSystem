using Sieve.Services;

namespace Application.Common.Pagination
{
    public interface IFilteringSortingProfile
    {
        void MapProperties(SievePropertyMapper mapper);
    }
}
