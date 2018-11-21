using Eshop.Core.Entities;

namespace EShop.Core.Entities
{
    public class OrderProduct : BaseEntity<int>
    {
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}