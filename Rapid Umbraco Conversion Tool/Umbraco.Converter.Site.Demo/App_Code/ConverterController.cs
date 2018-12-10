
using Codetreehouse.RapidUmbracoConverter.Tools;
using Codetreehouse.RapidUmbracoConverter.Tools.Entities;
using System.Collections.Generic;
using System.Web.Http;
using Umbraco.Web.Editors;
using Umbraco.Web.WebApi;

namespace RapidUmbracoConverter.Controllers
{
    [Umbraco.Web.Mvc.PluginController("RapidUmbracoConverter")]
    public class ConverterController : UmbracoAuthorizedApiController
    {
        // we will add a method here later
        [HttpGet]
        public string BeginConvert(string templateDirectory)
        {

            RapidUmbracoConverterTool rapidConverter = new RapidUmbracoConverterTool(Services);

            //Create the document types
            rapidConverter.DeleteAllDocumentTypes(removeOnlyConverted: true);
            var generatedPairDocumentTypes = rapidConverter.ConvertDocumentTypes(templateDirectory, ".html");

            //Create the templates
            rapidConverter.DeleteAllTemplates();

            List<FileCopyPair> copyPair = new List<FileCopyPair>();

            copyPair.Add(new FileCopyPair(directoryRoot, source, markupReference, destination));

            rapidConverter.ConvertTemplates(generatedPairDocumentTypes);


            return "Complete";


        }
    }
}
