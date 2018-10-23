
namespace Codetreehouse.RapidUmbracoConverter.Tools.Entities
{
    public class UmbracoConversionProperty
    {
        public UmbracoConversionProperty(string originalTag)
        {
            OriginalTag = originalTag;
        }
        public string OriginalTag { get; internal set; }

        public string Alias { get; internal set; }
        public string Description { get; internal set; }
        public string Editor { get; internal set; }
        public string Label { get; internal set; }
        public string Tab { get; internal set; }
    }
}