using Customers.Grpc;
using Grpc.Core;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.WebHost.Grpc
{
    public class CustomerGrpcService : CustomerService.CustomerServiceBase
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerGrpcService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public override async Task<CustomerReply> GetCustomer(
            CustomerRequest request,
            ServerCallContext context)
        {
            var customer = await _customerRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (customer == null)
            {
                return new CustomerReply
                {
                    Found = false
                };
            }
            return new CustomerReply
            {
                Id = customer.Id.ToString(),
                Name = customer.FullName,
                Email = customer.Email,
                Found = true
            };
        }
    }
}