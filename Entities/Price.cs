using Eshop.Core.Entities;
using System;
using System.Collections.Generic;

namespace EShop.Core.Entities
{
    public class Price : BaseEntity<int>
    {
        public decimal Value { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}