namespace CartingService.BLL.Entities
{
    public class CartEntity
    {
        public string Id { get; set; }

        public IEnumerable<CartItemEntity>? Items { get; set; }
    }
}