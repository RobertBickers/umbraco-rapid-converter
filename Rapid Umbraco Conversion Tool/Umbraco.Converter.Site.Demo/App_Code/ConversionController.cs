using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Editors;

namespace RapidUmbracoConverter.Controllers
{
    [Umbraco.Web.Mvc.PluginController("RapidUmbracoConverter")]
    public class ConversionController : UmbracoAuthorizedJsonController
    {
        // we will add a method here later
        public string BeginConvert()
        {
            return "Complete";

        }
    }
}

