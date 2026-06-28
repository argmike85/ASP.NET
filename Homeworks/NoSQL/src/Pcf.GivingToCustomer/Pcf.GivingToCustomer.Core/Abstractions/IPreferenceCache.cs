using Pcf.GivingToCustomer.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.Abstractions
{
    public interface IPreferenceCache
    {
        Task<List<Preference>> GetAsync();
        Task SetAsync(List<Preference> preferences);
    }
}