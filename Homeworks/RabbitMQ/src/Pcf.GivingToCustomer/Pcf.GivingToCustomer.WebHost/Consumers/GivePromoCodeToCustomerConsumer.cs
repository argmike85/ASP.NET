using MassTransit;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.WebHost.Mappers;
using Pcf.Shared.Events;
using System.Linq;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.WebHost.Consumers
{
    public class GivePromoCodeToCustomerConsumer : IConsumer<PromoCodeReceivedEvent>
    {
        private readonly IRepository<PromoCode> _promoCodesRepository;
        private readonly IRepository<Preference> _preferencesRepository;
        private readonly IRepository<Customer> _customersRepository;
        public GivePromoCodeToCustomerConsumer(IRepository<PromoCode> promoCodesRepository,
            IRepository<Preference> preferencesRepository, IRepository<Customer> customersRepository)
        {
            _promoCodesRepository = promoCodesRepository;
            _preferencesRepository = preferencesRepository;
            _customersRepository = customersRepository;
        }
        public async Task Consume(ConsumeContext<PromoCodeReceivedEvent> context)
        {
            var message = context.Message;
            var preference = await _preferencesRepository.GetByIdAsync(message.PreferenceId);

            if (preference == null)
                return;

            //  Получаем клиентов с этим предпочтением:
            var customers = await _customersRepository
                .GetWhere(d => d.Preferences.Any(x =>
                    x.Preference.Id == preference.Id));

            PromoCode promoCode = PromoCodeMapper.MapFromModel(new Models.GivePromoCodeRequest()
            {
                BeginDate = message.BeginDate.ToString(),
                EndDate = message.EndDate.ToString(),
                PartnerId = message.PartnerId,
                PreferenceId = preference.Id,
                PromoCode = message.Code,
                PromoCodeId = message.PromoCodeId,
                ServiceInfo = message.ServiceInfo
            },
            preference,
            customers);

            await _promoCodesRepository.AddAsync(promoCode);
        }
    }
}
