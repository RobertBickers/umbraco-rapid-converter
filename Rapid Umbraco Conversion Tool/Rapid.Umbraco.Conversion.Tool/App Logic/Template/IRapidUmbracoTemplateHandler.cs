using System;
using System.Collections.Generic;
using Codetreehouse.RapidUmbracoConverter.Tools.Entities;
using Umbraco.Core.Models;

namespace Codetreehouse.RapidUmbracoConverter.Tools
{
    public interface IRapidUmbracoTemplateHandler
    {
        void Delete();
        void Convert(IEnumerable<Tuple<RapidUmbracoConversionObject, IContentType>> convertedMarkupAndDocumentTypes, FileCopyPair[] assetDirectories);
    }
}