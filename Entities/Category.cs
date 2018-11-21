using Eshop.Core.Entities;
using System.Collections.Generic;

namespace EShop.Core.Entities
{
    public class Category : BaseEntity<int>
    {
        public int? ParentId { get; set; }
        public string CategoryName { get; set; }
        public virtual Category Parent { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}