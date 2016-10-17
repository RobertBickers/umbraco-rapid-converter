# Rapid Umbraco Conversion Tool
.NET Library that facilitates a notation based conversion from simple markup into Umbraco DocumentTypes and Templates

---
#### What is it for?

The Rapid Umbraco Conversion tool lets you quickly import designer.html files into Umbraco by using some simple notation within the html.

It is ideal for converting templates such as those purchased at [Wrapbootstrap.com](https://wrapbootstrap.com/ "Wrapbootstrap") into Umbraco Websites.

Simply replace the text in these templates with a suitable tag, and run the Converter against your modified files to see them get created in no time.  



---


Use the following notation to define Document Types within Markup: 

```[[{  }]]``` - Wrap the property that will be added to the Document Type

Use property notation within your markup to define

* label (name)
* alias
* tab
* editor

```[[{alias="header" tab="features" editorAlias="Umbraco.Textbox" label="header" }]]```

***

### Defining properties in your markup

Example:

```.../Markup/home.html```

```html
<div id="features">
    <div class="container">
        <div class="row header">
            <div class="col-md-12">
                <h2>
                    [[{alias="header" tab="features" editorAlias="Umbraco.Textbox" label="Header" }]]
                </h2>
                <p>
                    [[{alias="strapline" tab="features" editorAlias="Umbraco.NoEdit" label="Strap line" }]]
                </p>
            </div>
        </div>
     </div>
</div>
```

***

### Generating the Umbraco DocumentTypes
To consume the method within your application, simply instantiate the ```RapidUmbracoConverter``` class passing in the Services available within an ```UmbracoSurfaceController``` and call: 

```RapidUmbracoConverter.GetUmbracoConversionObjects(string templateDirectory, string[] allowedExtensions)```


The template directory is the location of the files that contain the property notation. The method will itterate through all of the files with the defined extension, extracting the markup as it goes. It will then add then add the Document Types to your Umbraco site

Given the location of the .html file above and the property notation that has been added

```new RapidUmbracoConverter(Services).GetUmbracoConversionObjects(".../Markup/home.html", ".html")```

Would create a _DocumentType_ named '**_Home_**', with alias '**_homeDocumentType_**'. It will have one _tab_ labelled '**_features_**'. In that _tab_ would be two properties, header (textbox) and strapline (label) respectively. 

______

### Generating the Umbraco Templates along side the DocumentTypes

To create the Umbraco Templates, call the ```CreateTemplatesFromConversion(generatedPairDocumentTypes)``` method. This takes the  outputed from the ```GetUmbracoConversionObjects``` method.

This will copy the content from the .html file, replacing the [[{}]] notation with the appropriate Umbraco helper method before creating the ```.cshtml``` and linking it to the document type

Example:

```C#
RapidUmbracoConverter rapidConverter = new RapidUmbracoConverter(Services);

 //Create the document types
rapidConverter.DeleteAllDocumentTypes(removeOnlyConverted: true);
var generatedPairDocumentTypes = rapidConverter.ConvertMarkupToDocumentTypes(templateDirectory, ".html");

//Create the templates
rapidConverter.DeleteAllTemplates();
rapidConverter.CreateTemplatesFromConversion(generatedPairDocumentTypes);
```
***
Note: the ```DeleteAllDocumentTypes``` and ```DeleteAllTemplates``` methods can be useful when getting everything set up, the ```DeleteAllDocumentTypes``` method has an boolean parameter that lets your define whether you wish to delete only the DocumentTypes that have been created during the conversion proccess as DocumentTypes with a corresponding flag in .AdditionalData, or all of the DocumentTypes within the Umbraco build. **Don't** use this method without first ensuring that it is valid to delete the Document Types, as this cannot be undone. 



