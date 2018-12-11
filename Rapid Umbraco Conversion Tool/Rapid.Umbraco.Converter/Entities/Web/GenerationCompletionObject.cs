using Codetreehouse.RapidUmbracoConverter.Tools.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Rapid.Umbraco.Converter.Entities.Web
{
    public class GenerationCompletionObject
    {
        public GenerationCompletionObject(bool isComplete, string message, List<RapidUmbracoConversionObject> contentTypes)
        {
            IsComplete = isComplete;
            Message = message;
            ContentTypes = contentTypes;
        }

        public bool IsComplete { get; set; }

        public string Message { get; set; }

        public List<RapidUmbracoConversionObject> ContentTypes { get; set; }


    }
}
