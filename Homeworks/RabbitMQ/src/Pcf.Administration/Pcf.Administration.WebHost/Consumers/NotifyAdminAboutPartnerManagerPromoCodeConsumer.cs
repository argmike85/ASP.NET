using MassTransit;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Domain.Administration;
using Pcf.Shared.Events;
using System.Threading.Tasks;

namespace Pcf.Administration.WebHost.Consumers
{
    public class NotifyAdminAboutPartnerManagerPromoCodeConsumer : IConsumer<PromoCodeReceivedEvent>
    {
        private readonly IRepository<Employee> _employeeRepository;
        public NotifyAdminAboutPartnerManagerPromoCodeConsumer(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public async Task Consume(ConsumeContext<PromoCodeReceivedEvent> context)
        {
            var message = context.Message;
            if (message.PartnerManagerId.HasValue)
            {
                var employee = await _employeeRepository.GetByIdAsync(message.PartnerManagerId.Value);

                if (employee == null)
                    return;

                employee.AppliedPromocodesCount++;

                await _employeeRepository.UpdateAsync(employee);
            }
        }
    }
}
