using System;
using System.ComponentModel.DataAnnotations;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class PromoCode
        : BaseEntity
    {
        [MaxLength(20)]
        public string Code { get; set; }
        [MaxLength(100)]
        public string ServiceInfo { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }
        [MaxLength(100)]
        public string PartnerName { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid PreferenceId { get; set; }

        public Preference Preference { get; set; }
    }
}