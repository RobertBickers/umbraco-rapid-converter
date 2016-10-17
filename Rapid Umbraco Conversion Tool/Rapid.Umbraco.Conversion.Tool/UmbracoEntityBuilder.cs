using Codetreehouse.RapidUmbracoConverter.Tools.Entities;
using Codetreehouse.RapidUmbracoConverter.Tools.Exceptions;
using Codetreehouse.RapidUmbracoConverter.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Codetreehouse.RapidUmbracoConverter.Tools
{
    public class UmbracoEntityBuilder
    {
        ServiceContext _serviceContext;

        public UmbracoEntityBuilder(ServiceContext serviceContext)
        {
            _serviceContext = serviceContext;
        }


        /// <summary>
        /// Builds the Umbraco Document Type from the RapidUmbracoConversionObject
        /// </summary>
        /// <param name="umbracoDocumentTypeList"></param>
        /// <param name="conversionObject"></param>
        /// <returns></returns>
        public virtual IContentType BuildDocumentType(List<IContentType> umbracoDocumentTypeList, RapidUmbracoConversionObject conversionObject)
        {
            IContentType documentType = new ContentType(-1);
            documentType.Name = conversionObject.Name;
            documentType.Alias = conversionObject.Name.FirstCharacterToLower() + "DocumentType";

            documentType.AdditionalData.Add("IsFromUmbracoTemplateConverter", true);
            documentType.AdditionalData.Add("UmbracoTemplateConverterDate", DateTime.Now);
            documentType.AdditionalData.Add("UmbracoTemplateConverterOriginalFilepath", conversionObject.FilePath);

            this.ValidateAlias(umbracoDocumentTypeList, documentType);

            //Add the properties to the content type
            IEnumerable<IDataTypeDefinition> dataTypeDefinitionCollection = _serviceContext.DataTypeService.GetAllDataTypeDefinitions();

            Debug.Indent();
            foreach (var property in conversionObject.PropertyCollection)
            {
                string tabName = property.Tab.Trim();

                AddTabName(documentType, tabName);
                AddPropertyToContentType(documentType, property, tabName, dataTypeDefinitionCollection);
            }
            Debug.Unindent();

            return documentType;
        }


        /// <summary>
        /// Adds the property to the Document Type
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="property"></param>
        /// <param name="tabName"></param>
        /// <param name="registeredDataTypes"></param>
        /// <returns></returns>
        public virtual bool AddPropertyToContentType(IContentType documentType, UmbracoConversionProperty property, string tabName, IEnumerable<IDataTypeDefinition> registeredDataTypes)
        {
            if (property.Editor == "Umbraco.RichEdit")
                property.Editor = "Umbraco.TinyMCEv3";

            if (registeredDataTypes.Any(d => d.PropertyEditorAlias == property.Editor))
            {
                DataTypeDefinition dataTypeDefintion = new DataTypeDefinition(property.Editor);
                PropertyType propertyType = new PropertyType(dataTypeDefintion, property.Alias.FirstCharacterToLower());

                Debug.WriteLine($"Added property: {property.Alias} ({property.Tab}) - {documentType.Name}");
                documentType.AddPropertyType(propertyType, tabName);

                return true;
            }
            else
            {
                throw new PropertyParseException($"There was a problem parsing the property for {property.Alias}. Ensure this property editor exists and is registered: { property.Editor}")
                {
                    Alias = property.Alias,
                    EditorAttempt = property.Editor,
                    RegisteredDataTypes = registeredDataTypes.ToList()
                };
            }
        }


        /// <summary>
        /// Checks the validity of an alias, if it is not unique, a unique identifer will be appended
        /// </summary>
        /// <param name="documentTypeList"></param>
        /// <param name="documentType"></param>
        private void ValidateAlias(List<IContentType> documentTypeList, IContentType documentType)
        {
            bool isDuplicate = false;

            //Add an underscrore to the alias if it does not have a valid start character
            if (!Char.IsLetter(documentType.Alias.FirstOrDefault()))
                documentType.Alias.Insert(0, "_");

            foreach (var currentContentType in documentTypeList)
            {
                if (currentContentType.Alias.Contains(documentType.Alias.FirstCharacterToLower()))
                {
                    isDuplicate = true;
                    break;
                }
            }

            if (isDuplicate)
                documentType.Alias += "_" + Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Adds a new tab to the Document Type
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="tabName"></param>
        private void AddTabName(IContentType documentType, string tabName)
        {
            //Add the property group as it doesn't already exist
            if (documentType.PropertyGroups.SingleOrDefault(x => x.Name == tabName) == null)
            {
                Debug.WriteLine("Adding tab: " + tabName);
                documentType.AddPropertyGroup(tabName);
            }
        }





    }
}
