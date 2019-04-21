namespace Suppliers.Domain.Exceptions
{
    using System;

    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}