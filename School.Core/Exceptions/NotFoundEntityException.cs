namespace School.Core.Exceptions
{
    public class NotFoundEntityException : Exception
    {
        public NotFoundEntityException(string type, string id) : base($"{type} with id: {id} not found") { }

        public NotFoundEntityException(string type, int id) : base($"{type} with id: {id} not found") { }
    }
}
