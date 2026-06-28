using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pcf.GivingToCustomer.Core.Abstractions;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.WebHost.Models;

namespace Pcf.GivingToCustomer.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения клиентов
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController
        : ControllerBase
    {
        private readonly IRepository<Preference> _preferencesRepository;
        private readonly IPreferenceCache _preferenceCache;

        public PreferencesController(IRepository<Preference> preferencesRepository, IPreferenceCache preferenceCache)
        {
            _preferencesRepository = preferencesRepository;
            _preferenceCache = preferenceCache;
        }
        
        /// <summary>
        /// Получить список предпочтений
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PreferenceResponse>>> GetPreferencesAsync()
        {
            var response = new List<PreferenceResponse>();
            var cached = await _preferenceCache.GetAsync();
            if (cached != null)
            {
                response = cached.Select(x => new PreferenceResponse()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
                return Ok(response);
            }

            var preferences = await _preferencesRepository.GetAllAsync();

            await _preferenceCache.SetAsync(preferences.ToList());

            response = preferences.Select(x => new PreferenceResponse()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return Ok(response);
        }
    }
}