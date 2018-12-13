angular.module("umbraco").controller("RapidUmbracoConverterDashboardController", function ($scope, userService, $http, umbRequestHelper) {
    var vm = this;

    vm.TemplatePath = "~/react-demo";
    vm.UserName = "";
    vm.Response = "";
    vm.GeneratedDocumentTypes = [];
    vm.GeneratedDocumentTypeNumber = 0;

    vm.FileCopyPairCollection = [];

    vm.NewFileCopyPairSource = {
        VirtualPath: "",
        Source: "",
        MarkupReference: "",
        DestinationFolder: ""
    };

    vm.AddNewFileCopyPair = function () {
        vm.FileCopyPairCollection.push(JSON.parse(JSON.stringify(vm.NewFileCopyPairSource)));

        vm.NewFileCopyPairSource.VirtualPath = "";
        vm.NewFileCopyPairSource.Source = "";
        vm.NewFileCopyPairSource.MarkupReference = "";
        vm.NewFileCopyPairSource.DestinationFolder = "";

    };


    userService.getCurrentUser().then(function (user) {
        console.log(user);
        vm.UserName = user.name;
    });


    vm.BeginConvert = function () {

        if (vm.TemplatePath == '') {
            alert("Please enter a valid template directory");
            $("#templatePath").focus();
        }
        else {

            vm.Response = "Conversion Started";

            umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/RapidUmbracoConverter/Converter/BeginConvert?templateDirectory=" + vm.TemplatePath),
                "Failed to retrieve all Person data").then(function (data) {
                    vm.Response = data.Message;
                    vm.GeneratedDocumentTypes = data.ContentTypes;
                    vm.GeneratedDocumentTypeNumber = data.ContentTypes.length;


                });
        }
    }
});