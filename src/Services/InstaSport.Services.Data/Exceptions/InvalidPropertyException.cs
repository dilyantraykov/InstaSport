using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaSport.Services.Data.Exceptions
{
    public class InvalidPropertyException : Exception
    {
        public string Property { get; set; }

        public InvalidPropertyException(string property)
        {
            Property = property;
        }

        public InvalidPropertyException(string property, string message) : base(message)
        {
            Property = property;
        }

        public InvalidPropertyException(string property, string message, Exception innerException) : base(message, innerException)
        {
            Property = property;
        }
    }
}
