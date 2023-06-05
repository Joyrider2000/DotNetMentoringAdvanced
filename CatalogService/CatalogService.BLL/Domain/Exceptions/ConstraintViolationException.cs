namespace CatalogService.BLL.Domain.Exceptions
{
    public class ConstraintViolationException : Exception
    {
        public ConstraintViolationException()
            : base("Constraint violation.")
        {
        }

        public ConstraintViolationException(string message)
            : base(message)
        {
        }

        public ConstraintViolationException(string message, Exception inner)
            : base(message, inner)
        {
        }


    }
}
