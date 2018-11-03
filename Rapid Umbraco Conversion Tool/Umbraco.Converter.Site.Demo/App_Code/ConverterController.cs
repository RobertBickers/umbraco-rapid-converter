
using Codetreehouse.RapidUmbracoConverter.Tools;
using System.Web.Http;
using Umbraco.Web.Editors;

namespace RapidUmbracoConverter.Controllers
{
    [Umbraco.Web.Mvc.PluginController("RapidUmbracoConverter")]
    public class ConverterApiController : UmbracoAuthorizedJsonController
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
            rapidConverter.ConvertTemplates(generatedPairDocumentTypes);


            return "Complete";


        }
    }
}
