namespace CartingService.DAL.Exceptions
{
    public class ItemExistsException : Exception
    {
        public ItemExistsException()
            : base(string.Format("Item alrealy exists."))
        {
        }

        public ItemExistsException(string message)
            : base(message)
        {
        }

        public ItemExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }


    }
}
