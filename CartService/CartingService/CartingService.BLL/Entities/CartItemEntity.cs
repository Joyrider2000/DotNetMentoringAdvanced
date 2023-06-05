namespace CartingService.BLL.Entities
{
    public class CartItemEntity
    {
        public int Id { get; set; }
        
        public string? Name { get; set; }

        public ImageEntity? Image { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

    }
}
