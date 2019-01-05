
using Codetreehouse.RapidUmbracoConverter.Tools;
using Codetreehouse.RapidUmbracoConverter.Tools.Entities;
using Rapid.Umbraco.Converter.Entities.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Umbraco.Web.Editors;
using Umbraco.Web.WebApi;

namespace RapidUmbracoConverter.Controllers
{
    public class ConversionPostObject
    {

        public ConversionPostObject(string templateDirectory, List<FileCopyPair> copyPairCollection)
        {
            TemplateDirectory = templateDirectory;
            CopyPairCollection = copyPairCollection;
        }

        public string TemplateDirectory { get; set; }

        public List<FileCopyPair> CopyPairCollection { get; set; }

    }


    public class TemplateDirectoryPostObject
    {
        public TemplateDirectoryPostObject()
        {

        }

        public TemplateDirectoryPostObject(string templateDirectory)
        {
            TemplateDirectory = templateDirectory;
        }

        public string TemplateDirectory { get; set; }

    }


    public class FileInformationObject
    {
        public FileInformationObject(string fileName, string fileExtension, bool hasMarkupReferences)
        {
            FileName = fileName;
            FileExtension = fileExtension;
            HasMarkupReferences = hasMarkupReferences;
        }

        public string FileName { get; set; }
        public string FileExtension { get; set; }

        public bool HasMarkupReferences { get; set; }


        public override string ToString()
        {
            return FileName + " Has Markup References: " + HasMarkupReferences;

        }


    }






    [Umbraco.Web.Mvc.PluginController("RapidUmbracoConverter")]
    public class ConverterController : UmbracoAuthorizedApiController
    {
        RapidUmbracoConverterTool rapidConverter = null;
        public ConverterController()
        {
            rapidConverter = new RapidUmbracoConverterTool(Services);
        }


        private static object fileReadLockObject = new object();

        [HttpPost]
        public List<FileInformationObject> GetConvertableFiles([FromBody]TemplateDirectoryPostObject directoryObject)
        {
            List<FileInformationObject> items = new List<FileInformationObject>();

            try
            {

                lock (fileReadLockObject)
                {
                    string mappedPath = HttpContext.Current.Server.MapPath(directoryObject.TemplateDirectory);

                    IEnumerable<RapidUmbracoConversionObject> conversionObjects = rapidConverter.GetConversionObjects(mappedPath, ".html");

                    items.AddRange(conversionObjects
                        .Select(
                        c =>
                        new FileInformationObject(c.FileName, Path.GetExtension(c.FileName), c.PropertyCollection.Count > 0)));

                    return items;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return items;


        }

        // we will add a method here later
        [HttpPost]
        public GenerationCompletionObject BeginConvert([FromBody]ConversionPostObject postObject)
        {

            string templateDirectory = postObject.TemplateDirectory;

            //Create the document types
            rapidConverter.DeleteAllDocumentTypes(removeOnlyConverted: false);
            IEnumerable<System.Tuple<RapidUmbracoConversionObject, Umbraco.Core.Models.IContentType>> generatedPairDocumentTypes = rapidConverter.ConvertDocumentTypes(templateDirectory, ".html");

            //Create the templates
            rapidConverter.DeleteAllTemplates();

            Debug.WriteLine(postObject.CopyPairCollection.Count);


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


            List<RapidUmbracoConversionObject> convertedItems = generatedPairDocumentTypes.Select(x => x.Item1).ToList();

            return new GenerationCompletionObject(true, "Action complete", convertedItems);
        }
    }
}
