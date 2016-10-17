using Codetreehouse.RapidUmbracoConverter.Tools.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Codetreehouse.RapidUmbracoConverter.Tools
{
    public class RapidUmbracoConverter
    {
        ServiceContext _serviceContext = null;

        UmbracoEntityBuilder entityBuilder;
        UmbracoFileContentParser fileReader;

        public RapidUmbracoConverter(ServiceContext services)
        {
            _serviceContext = services;

            entityBuilder = new UmbracoEntityBuilder(services);
            fileReader = new UmbracoFileContentParser();
        }

        /// <summary>
        /// Deletes all of the document types within Umbraco 
        /// </summary>
        /// <param name="removeOnlyConverted"></param>
        public void DeleteAllDocumentTypes(bool removeOnlyConverted = true)
        {
            var contentTypes = _serviceContext.ContentTypeService.GetAllContentTypes();
            foreach (var contentType in contentTypes.Where(c => (!removeOnlyConverted) || (c.AdditionalData.ContainsKey("IsFromUmbracoTemplateConverter") && (((bool)c.AdditionalData["IsFromUmbracoTemplateConverter"]) == true))))
            {
                Debug.WriteLine("Deleting Document Types: " + contentType.Name);
                _serviceContext.ContentTypeService.Delete(contentType);
            }
        }

        /// <summary>
        /// Deletes all of the templates within Umbraco
        /// </summary>
        public void DeleteAllTemplates()
        {
            var templates = _serviceContext.FileService.GetTemplates();
            foreach (var item in templates)
            {
                _serviceContext.FileService.DeleteTemplate(item.Alias);
            }
        }


        public void CreateTemplatesFromConversion(IEnumerable<Tuple<RapidUmbracoConversionObject, IContentType>> convertedMarkupAndDocumentTypes)
        {
            List<ITemplate> templateList = new List<ITemplate>();

            foreach (var item in convertedMarkupAndDocumentTypes)
            {
                RapidUmbracoConversionObject conversionObject = item.Item1;
                IContentType contentType = item.Item2;

                Debug.WriteLine("Conversion Object:" + item.Item1.Name);

                var attempt = _serviceContext.FileService.CreateTemplateForContentType(contentType.Alias, contentType.Name);
                if (attempt.Success)
                {
                    ITemplate template = attempt.Result.Entity;

                    Debug.WriteLine("Template created for Document Type. Id: " + template.Id);

                    template.Content = template.Content;
                    Debug.WriteLine("Content has been set, attempting conversion of content");

                    templateList.Add(template);
                }
            }

            _serviceContext.FileService.SaveTemplate(templateList);
        }


        /// <summary>
        /// Converts markup files from a directory into Document Types using the Umbraco Services API
        /// </summary>
        /// <param name="templateDirectory">The location of the annotated files</param>
        /// <param name="allowedExtensions">The extensions that will be included in the file system search query</param>
        /// <returns></returns>
        public IEnumerable<Tuple<RapidUmbracoConversionObject, IContentType>> ConvertMarkupToDocumentTypes(string templateDirectory, params string[] allowedExtensions)
        {
            List<Tuple<RapidUmbracoConversionObject, IContentType>> pairedCollection = new List<Tuple<RapidUmbracoConversionObject, IContentType>>();

            List<IContentType> umbracoContentTypeList = new List<IContentType>();

            //Retrieve the conversion objects from the file system
            foreach (var conversionObject in fileReader.GetUmbracoConversionObjects(templateDirectory, allowedExtensions))
            {
                Debug.WriteLine("Building Document Type from file:" + conversionObject.Name);

                IContentType umbracoContentType = entityBuilder.BuildDocumentType(umbracoContentTypeList, conversionObject);

                //Add the content tpye to the list
                umbracoContentTypeList.Add(umbracoContentType);

                //Add the Conversion object, and the generated content type
                pairedCollection.Add(new Tuple<RapidUmbracoConversionObject, IContentType>(conversionObject, umbracoContentType));
            }

            _serviceContext.ContentTypeService.Save(umbracoContentTypeList);

            return pairedCollection;
        }


    }
}
