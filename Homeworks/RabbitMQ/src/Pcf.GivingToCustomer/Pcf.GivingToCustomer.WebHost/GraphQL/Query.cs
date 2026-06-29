using HotChocolate;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.WebHost.GraphQL
{
    public class Query
    {
        public async Task<Customer> GetCustomer(
            Guid id,
            [Service] IRepository<Customer> repository)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Customer>> GetCustomers(
            [Service] IRepository<Customer> repository)
        {
            return await repository.GetAllAsync();
        }
    }
}
