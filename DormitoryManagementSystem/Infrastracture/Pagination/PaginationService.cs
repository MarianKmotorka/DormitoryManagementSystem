using Application.Common.Pagination;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;

namespace Infrastracture.Pagination
{
    public class PaginationService : SieveProcessor, IPaginationService
    {
        public PaginationService() : base(Microsoft.Extensions.Options.Options.Create(new SieveOptions()))
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            AddSievePropertyMapperProfilesFromAssembly(mapper);

            return mapper;
        }

        private void AddSievePropertyMapperProfilesFromAssembly(SievePropertyMapper mapper)
        {
            var types = typeof(IPaginationService).Assembly.GetExportedTypes()
                    .Where(t => typeof(IFilteringMapperProfile).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                    .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var mappingMethod = type.GetMethod(nameof(IFilteringMapperProfile.MapProperties));
                mappingMethod?.Invoke(instance, new[] { mapper });
            }
        }
    }
}
