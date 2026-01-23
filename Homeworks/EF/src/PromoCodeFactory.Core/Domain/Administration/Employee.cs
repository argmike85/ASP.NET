using System;
using System.ComponentModel.DataAnnotations;

namespace PromoCodeFactory.Core.Domain.Administration
{
    public class Employee
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
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public int AppliedPromocodesCount { get; set; }
    }
}