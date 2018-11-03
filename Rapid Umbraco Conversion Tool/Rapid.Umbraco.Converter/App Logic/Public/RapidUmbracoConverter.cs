using Codetreehouse.RapidUmbracoConverter.Tools.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Codetreehouse.RapidUmbracoConverter.Tools
{
    public class RapidUmbracoConverterTool
    {
        ServiceContext _serviceContext = null;

        IRapidUmbracoDocumentTypeHandler _documentTypeLogic;
        UmbracoTemplateLogic _templateLogic;

        public RapidUmbracoConverterTool(ServiceContext services)
        {
            _serviceContext = services ?? throw new RapidUmbracoConverterSetupException();

            _documentTypeLogic = new UmbracoDocumentTypeLogic(services);
            _templateLogic = new UmbracoTemplateLogic(services);
        }

        /// <summary>
        /// Deletes all of the document types within Umbraco 
        /// </summary>
        /// <param name="removeOnlyConverted"></param>
        public void DeleteAllDocumentTypes(bool removeOnlyConverted = true)
        {
            _documentTypeLogic.RemoveAllDocumentTypes(removeOnlyConverted);
        }

        /// <summary>
        /// Deletes all of the templates within Umbraco
        /// </summary>
        public void DeleteAllTemplates()
        {
            _templateLogic.Delete();
        }

        /// <summary>
        /// Takes the pair collection of RapidUmbracoConverionObject and the correpsonding Document Type to create and attatch the appropriate Template
        /// </summary>
        /// <param name="convertedMarkupAndDocumentTypes"></param>
        public void ConvertTemplates(IEnumerable<Tuple<RapidUmbracoConversionObject, IContentType>> convertedMarkupAndDocumentTypes, params FileCopyPair[] assetDirectories)
        {
            _templateLogic.Convert(convertedMarkupAndDocumentTypes, assetDirectories);
        }

        /// <summary>
        /// Converts markup files from a directory into Document Types using the Umbraco Services API
        /// </summary>
        /// <param name="templateDirectory">The location of the annotated files</param>
        /// <param name="allowedExtensions">The extensions that will be included in the file system search query</param>
        /// <returns></returns>
        public IEnumerable<Tuple<RapidUmbracoConversionObject, IContentType>> ConvertDocumentTypes(string templateDirectory, params string[] allowedExtensions)
        {
            return _documentTypeLogic.ConvertMarkupToDocumentTypes(templateDirectory, allowedExtensions);
        }


    }
}
