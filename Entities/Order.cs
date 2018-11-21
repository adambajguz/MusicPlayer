using Eshop.Core.Entities;
using System;
using System.Collections.Generic;

namespace EShop.Core.Entities
{
    public class Order : BaseEntity<int>
    {
        public DateTime OrderDate { get; set; }
        public string Address { get; set; }
        public string ContractingAuthority { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int? DiscountCouponId { get; set; }
        public string Email { get; set; }
        public virtual DiscountCoupon DiscountCoupon { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual User User { get; set; }
        
    }
}