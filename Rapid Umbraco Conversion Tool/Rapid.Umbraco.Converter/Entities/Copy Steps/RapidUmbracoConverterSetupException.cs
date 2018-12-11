using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Codetreehouse.RapidUmbracoConverter.Tools.Entities
{
    public class RapidUmbracoConverterSetupException : Exception
    {
        public RapidUmbracoConverterSetupException()
        {
        }

        public RapidUmbracoConverterSetupException(string message) : base(message)
        {

        }

        public RapidUmbracoConverterSetupException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RapidUmbracoConverterSetupException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
