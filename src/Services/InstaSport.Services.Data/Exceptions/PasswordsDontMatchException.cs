namespace InstaSport.Services.Data.Exceptions
{
    public class PasswordsDontMatchException : InvalidPropertyException
    {
        public PasswordsDontMatchException(string property, string message, Exception innerException) : base(property, message, innerException)
        {
            Property = property;
        }
    }
}
