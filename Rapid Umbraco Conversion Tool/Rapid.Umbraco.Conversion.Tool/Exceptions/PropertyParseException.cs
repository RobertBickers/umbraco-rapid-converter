using System;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Codetreehouse.RapidUmbracoConverter.Tools.Exceptions
{
    public class PropertyParseException : Exception
    {
        public PropertyParseException(string message) : base(message)
        {
        }

        public string Alias { get; internal set; }
        public string EditorAttempt { get; internal set; }
        public List<IDataTypeDefinition> RegisteredDataTypes { get; internal set; }
    }
}
