using System;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Codetreehouse.RapidUmbracoConverter.Tools.Entities
{
    public class RapidUmbracoConverterResponse
    {
        public List<RapidUmbracoConversionObject> ConvertionObects { get; set; }
        public List<IContentType> DocumentTypes { get; set; }
        public IEnumerable<Tuple<RapidUmbracoConversionObject, IContentType>> GeneratedPairs { get; set; }
        public string Message { get; set; }
    }
}
