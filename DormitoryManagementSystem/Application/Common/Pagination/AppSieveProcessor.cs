using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Reflection;

namespace Application.Common.Pagination
{
    internal class AppSieveProcessor : SieveProcessor
    {
        public AppSieveProcessor() : base(Options.Create(new SieveOptions()))
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            AddSievePropertyMapperProfilesFromAssembly(mapper);

            return mapper;
        }

        private void AddSievePropertyMapperProfilesFromAssembly(SievePropertyMapper mapper)
        {
            var types = Assembly.GetExecutingAssembly().GetExportedTypes()
                    .Where(t => typeof(ISievePropertyMapperProfile).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                    .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var mappingMethod = type.GetMethod(nameof(ISievePropertyMapperProfile.MapProperties));
                mappingMethod?.Invoke(instance, new[] { mapper });
            }
        }
    }
}
