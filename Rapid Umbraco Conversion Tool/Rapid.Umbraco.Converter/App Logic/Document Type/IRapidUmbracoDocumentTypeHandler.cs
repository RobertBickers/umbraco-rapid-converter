using Codetreehouse.RapidUmbracoConverter.Tools.Entities;
using System;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Codetreehouse.RapidUmbracoConverter.Tools
{
    public interface IRapidUmbracoDocumentTypeHandler
    {
        void RemoveAllDocumentTypes(bool removeOnlyConverted);

        IEnumerable<RapidUmbracoConversionObject> GetConversionObjects(string templateDirectory, params string[] allowedExtensions);

        IEnumerable<Tuple<RapidUmbracoConversionObject, IContentType>> ConvertMarkupToDocumentTypes(string templateDirectory, string[] allowedExtensions);
    }
}
