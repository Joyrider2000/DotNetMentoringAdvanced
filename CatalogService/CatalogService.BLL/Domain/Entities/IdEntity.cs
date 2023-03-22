namespace CatalogService.BLL.Domain.Entities
{
    public abstract class IdEntity : ValidatableEntity
    {
        public int Id { get; set; }
    }
}
