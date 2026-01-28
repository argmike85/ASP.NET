using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.DataAccess;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Контроллер для работы с предпочтениями.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController : ControllerBase
    {
        private readonly DataContext _context;

        public PreferencesController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить все доступные роли сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCustomerPreferencesAsync(Guid customerId)
        {
            try
            {
                var customerWithPreferences = await _context.Customers
                .Include(c => c.CustomerPreferences)
                    .ThenInclude(cp => cp.Preference)
                .FirstOrDefaultAsync(c => c.Id == customerId);

                if (customerWithPreferences == null)
                {
                    return NotFound();
                }

                var preferenceResponse = customerWithPreferences.GetPreferences()
                    .Select(preference => new PreferenceResponse
                    {
                        Id = preference.Id,
                        Name = preference.Name
                    })
                    .ToList();

                return Ok(preferenceResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}