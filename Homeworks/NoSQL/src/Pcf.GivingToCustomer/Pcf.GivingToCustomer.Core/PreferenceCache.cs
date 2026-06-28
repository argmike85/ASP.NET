using Microsoft.Extensions.Caching.Distributed;
using Pcf.GivingToCustomer.Core.Abstractions;
using Pcf.GivingToCustomer.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core
{
    public class PreferenceCache : IPreferenceCache
    {
        private const string Key = "preferences";
        private readonly IDistributedCache _cache;

        public PreferenceCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<List<Preference>> GetAsync()
        {
            var json = await _cache.GetStringAsync(Key);

            if (string.IsNullOrEmpty(json))
                return null;

            return JsonSerializer.Deserialize<List<Preference>>(json);
        }

        public async Task SetAsync(List<Preference> preferences)
        {
            var json = JsonSerializer.Serialize(preferences);

            await _cache.SetStringAsync(Key, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
        }
    }
}
