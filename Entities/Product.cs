using Eshop.Core.Entities;
using System.Collections.Generic;

namespace EShop.Core.Entities
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public int Count { get; set; }

        public virtual Price CurrentPrice { get; set; }
        public int? CurrentPriceId { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string CategoryIdString { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}