namespace CartingService.DAL.Exceptions
{
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException() 
            : base(string.Format("Requested property has not been found."))
        {
        }

        public PropertyNotFoundException(string message)
            : base(message)
        {
        }

        public PropertyNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }


    }
}
