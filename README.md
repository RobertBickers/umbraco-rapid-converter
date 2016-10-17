# Rapid Umbraco Conversion Tool
.NET Library that facilitates a notation based conversion into Umbraco Document Types and Templates

Use the following notation to define Document Types within Markup: 

```[[{  }]]``` - Wrap the property that will be added to the Document Type

Use property notation within your markup to define

* alias
* tab
* editor

```[[{alias="header" tab="features" editorAlias="Umbraco.Textbox" }]]```

Example:

```html
<div id="features">
    <div class="container">
        <div class="row header">
            <div class="col-md-12">
                <h2>
                    [[{alias="header" tab="features" editorAlias="Umbraco.Textbox" }]]
                </h2>
                <p>
                    [[{alias="strapline" tab="features" editorAlias="Umbraco.NoEdit" }]]
                </p>
            </div>
        </div>
     </div>
</div>
```

To consume the method within your application, simply instantiate the ```RapidUmbracoConverter``` class passing in the Services available within an ```UmbracoSurfaceController``` and call: 

```RapidUmbracoConverter.GetUmbracoConversionObjects(string templateDirectory, string[] allowedExtensions)```


The template directory is the location of the files that contain the property notation. The method will itterate through all of the files with the defined extension, extracting the markup as it goes. It will then add then add the Document Types to your Umbraco site








