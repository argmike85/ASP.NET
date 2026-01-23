using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Customer
        : BaseEntity
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string FullName => $"{FirstName} {LastName}";
        [MaxLength(100)]
        public string Email { get; set; }
        public ICollection<CustomerPreference> CustomerPreferences { get; set; }
        public ICollection<PromoCode> PromoCodes { get; set; }

        //TODO: Списки Preferences и Promocodes 
    }
}