using System;
using System.Collections.Generic;
using Codetreehouse.RapidUmbracoConverter.Tools.Entities;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using System.Diagnostics;
using System.IO;

namespace Codetreehouse.RapidUmbracoConverter.Tools
{
    internal class UmbracoTemplateLogic
    {
        private ServiceContext _serviceContext;

        internal UmbracoTemplateLogic(ServiceContext services)
        {
            this._serviceContext = services;
        }

        internal void Delete()
        {
            var templates = _serviceContext.FileService.GetTemplates();
            foreach (var item in templates)
            {
                _serviceContext.FileService.DeleteTemplate(item.Alias);
            }
        }


        internal void Convert(IEnumerable<Tuple<RapidUmbracoConversionObject, IContentType>> convertedMarkupAndDocumentTypes, FileCopyPair[] assetDirectories)
        {
            List<ITemplate> templateList = new List<ITemplate>();

            //Copy all of the files from the defined asset directories
            foreach (FileCopyPair directory in assetDirectories)
            {
                DirectoryCopier.Copy(directory.CombinedSource, directory.CombinedDestination);
            }


            foreach (Tuple<RapidUmbracoConversionObject, IContentType> item in convertedMarkupAndDocumentTypes)
            {
                RapidUmbracoConversionObject conversionObject = item.Item1;
                IContentType documentType = item.Item2;

                Debug.WriteLine("Conversion Object:" + item.Item1.Name);

                //Create the strongly type template
                var attempt = _serviceContext.FileService.CreateTemplateForContentType(documentType.Alias, documentType.Name);

                if (attempt.Success)
                {
                    ITemplate template = attempt.Result.Entity;

                    Debug.WriteLine("Template created for Document Type. Template Id: " + template.Id);

                    //Replace the markup
                    string fileContents = this.ReplaceTagsWithUmbracoHelpers(conversionObject);

                    //Append file's content to the .cshtml template
                    template.Content += Environment.NewLine;
                    template.Content += Environment.NewLine;
                    template.Content += fileContents;


                    //Replace any of the copied asset directories
                    foreach (FileCopyPair copyPair in assetDirectories)
                    {
                        string replaceString = $"href=\"{copyPair.MarkupReference}";
                        template.Content = template.Content.Replace(replaceString, $"href=\"{copyPair.Destination}");

                        replaceString = $"src=\"{copyPair.MarkupReference}";
                        template.Content = template.Content.Replace(replaceString, $"src=\"{copyPair.Destination}");
                    }
                    

                    //Save the template to initialise the ID
                    Debug.WriteLine($"Saving template: {template.Name}");
                    _serviceContext.FileService.SaveTemplate(template);

                    //Set the default template on the paired content type
                    Debug.WriteLine($"Setting DocumentType {documentType.Name}'s DefaultTemplate");
                    documentType.SetDefaultTemplate(template);
                    _serviceContext.ContentTypeService.Save(documentType);

                }
            }
        }

        private string ReplaceTagsWithUmbracoHelpers(RapidUmbracoConversionObject conversionObject)
        {
            string fileContents = conversionObject.FileContent;
            foreach (var convertedProperty in conversionObject.PropertyCollection)
            {
                if (!String.IsNullOrWhiteSpace(convertedProperty.OriginalTag))
                {
                    //TODO: Switch on the property type
                    fileContents = fileContents.Replace(convertedProperty.OriginalTag, $"@Umbraco.Field(\"{convertedProperty.Alias}\")");
                }
            }

            return fileContents;
        }

    }
}