
using Codetreehouse.RapidUmbracoConverter.Tools;
using Codetreehouse.RapidUmbracoConverter.Tools.Entities;
using Rapid.Umbraco.Converter.Entities.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Umbraco.Web.Editors;
using Umbraco.Web.WebApi;

namespace RapidUmbracoConverter.Controllers
{
    [Umbraco.Web.Mvc.PluginController("RapidUmbracoConverter")]
    public class ConverterController : UmbracoAuthorizedApiController
    {
        // we will add a method here later
        [HttpGet]
        public GenerationCompletionObject BeginConvert(string templateDirectory)
        {
            RapidUmbracoConverterTool rapidConverter = new RapidUmbracoConverterTool(Services);

            //Create the document types
            rapidConverter.DeleteAllDocumentTypes(removeOnlyConverted: false);
            var generatedPairDocumentTypes = rapidConverter.ConvertDocumentTypes(templateDirectory, ".html");

            //Create the templates
            rapidConverter.DeleteAllTemplates();

            List<FileCopyPair> copyPair = new List<FileCopyPair>()
            {
                //Images
                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/icons", "icons", "/Content/icons"),
                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/icons/brankic", "icons/brankic", "/Content/brankic"),
                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/icons/entypo", "icons/entypo", "/Content/entypo"),
                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/icons/icomoon", "icons/icomoon", "/Content/icomoon"),

                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/images", "images", "/Content/Images"),
                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/images/bgs", "images/bgs", "/Content/Images/bgs"),
                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/images/circle-icons", "images/circle-icons", "/Content/Images/circle-icons"),
                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/images/logos", "images/logos", "/Content/Images/logos"),
                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/images/slider", "images/slider", "/Content/Images/slider"),
                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/images/social", "images/social", "/Content/Images/social"),
                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/images/tabs", "images/tabs", "/Content/Images/tabs"),
                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/images/testimonials", "images/testimonials", "/Content/Images/testimonials"),


                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/css/compiled", "css/compiled", "/Content/css/compiled"),
                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/css/vendor", "css/vendor", "/Content/css/vendor"),

                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/js", "js/", "/Content/js/"),
                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/js/bootstrap", "js/boostrap", "/Content/js/bootstrap"),
                new FileCopyPair(HttpContext.Current.Server.MapPath("~/"), "/react-demo/js/vendor", "js/vendor", "/Content/js/vendor"),

            };

            rapidConverter.ConvertTemplates(generatedPairDocumentTypes, copyPair.ToArray());



            return new GenerationCompletionObject(true, "Action complete", generatedPairDocumentTypes.Select(x => x.Item2).ToList());




        }
    }
}
