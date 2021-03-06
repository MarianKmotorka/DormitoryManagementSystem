﻿using System;
using System.Linq;
using Application.Common.Pagination;
using Sieve.Models;
using Sieve.Services;

namespace Infrastracture.Pagination
{
    public class PaginationService : SieveProcessor, IPaginationService
    {
        public PaginationService(ISieveCustomSortMethods sortMethods, ISieveCustomFilterMethods filterMethods)
            : base(Microsoft.Extensions.Options.Options.Create(new SieveOptions()),
                  sortMethods,
                  filterMethods)
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
                    .Where(t => typeof(IFilteringSortingProfile).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                    .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var mappingMethod = type.GetMethod(nameof(IFilteringSortingProfile.MapProperties));
                mappingMethod?.Invoke(instance, new[] { mapper });
            }
        }
    }
}
