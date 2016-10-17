using System.Collections.Generic;

namespace Codetreehouse.RapidUmbracoConverter.Tools.Entities
{
    public class RapidUmbracoConversionObject
    {
        public RapidUmbracoConversionObject()
        {
            PropertyCollection = new List<UmbracoConversionProperty>();
        }

        public string FileContent { get; internal set; }
        public string FileName { get; internal set; }
        public object FilePath { get; internal set; }
        public string Name { get; internal set; }

        public List<UmbracoConversionProperty> PropertyCollection { get; private set; }
    }
}
